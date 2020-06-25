using BlingIntegrationTagplus.Clients.Bling;
using BlingIntegrationTagplus.Clients.Bling.Models.Pedidos;
using BlingIntegrationTagplus.Clients.TagPlus;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Clientes;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Pedidos;
using BlingIntegrationTagplus.Databases;
using BlingIntegrationTagplus.Exceptions;
using BlingIntegrationTagplus.Utils;
using dotenv.net;
using dotenv.net.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace BlingIntegrationTagplus
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("############################################");
            Console.WriteLine("## Bem vindo a integração Bling - Tagplus ##");
            Console.WriteLine("############################################");

            // Carrega o arquivo de configuração
            DotEnv.Config();
            var envReader = new EnvReader();
            // Carrega a API KEY do Bling
            var blingApiKey = envReader.GetStringValue("BLING_API_KEY");

            // Inicializa o banco de dados local
            using var db = new IntegrationContext();
            // Realiza as migrações
            db.Database.Migrate();
            // Verifica se o Token do Tagplus já está no banco de dados e válido
            string code = DBUtils.RetriveTagPlusToken(db);
            // Recupera os pedidos do Bling
            Console.WriteLine();
            Console.WriteLine("Procurando pedidos no Bling...");
            var blingClient = new BlingClient(blingApiKey);
            PedidosResponse pedidos = null;
            try
            {
                pedidos = blingClient.ExecuteGetOrder();
            }
            catch (BlingException e)
            {
                Console.WriteLine($"Não foi possível recuperar os pedidos do Bling - {e.Message}");
                Environment.Exit(-1);
            }

            // Verifica se existem pedidos
            if (pedidos.Retorno.Pedidos == null || pedidos.Retorno.Pedidos.Count == 0)
            {
                Console.WriteLine("Não foram encontrados pedidos no Bling");
                Environment.Exit(0);
            }

            // Envia os pedidos para o TagPlus
            TagPlusClient tagPlusClient = new TagPlusClient(code);
            Console.WriteLine($"Foram encontrados {pedidos.Retorno.Pedidos.Count} pedido(s)");
            foreach (PedidoItem pedido in pedidos.Retorno.Pedidos)
            {
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine($"Tratando o Pedido {pedido.Pedido.Numero}");
                // Recupera o Cliente
                int clienteId = tagPlusClient.GetCliente(pedido.Pedido.Cliente.Nome);
                // Cria se não existir
                if (clienteId == 0)
                {
                    Console.WriteLine($"Cliente {pedido.Pedido.Cliente.Nome} não foi encontrado, cadastrando...");
                    ClienteBody cliente = new ClienteBody();
                    cliente.RazaoSocial = pedido.Pedido.Cliente.Nome;
                    cliente.Ativo = true;
                    if (!string.IsNullOrWhiteSpace(pedido.Pedido.Cliente.Cnpj))
                    {
                        if (ValidateUtils.IsCpf(pedido.Pedido.Cliente.Cnpj))
                        {
                            cliente.Cpf = pedido.Pedido.Cliente.Cnpj;
                            cliente.Tipo = "F";
                        }
                        else if (ValidateUtils.IsCnpj(pedido.Pedido.Cliente.Cnpj))
                        {
                            cliente.Cnpj = pedido.Pedido.Cliente.Cnpj;
                            cliente.Tipo = "J";
                        }
                    }

                    // Envia o novo cliente
                    try
                    {
                        clienteId = tagPlusClient.PostCliente(cliente);
                    }
                    catch (TagPlusException e)
                    {
                        Console.WriteLine($"Não foi possível cadastrar o cliente: {e.Message}");
                    }
                }

                // Recupera os Itens
                IList<Clients.TagPlus.Models.Pedidos.Item> itens = new List<Clients.TagPlus.Models.Pedidos.Item>();
                for (int i = 0; i < pedido.Pedido.Itens.Count; i++)
                {
                    Clients.Bling.Models.Pedidos.Item blingItem = pedido.Pedido.Itens[i].Item;
                    int produtoServico = tagPlusClient.GetProduto(blingItem.Codigo);
                    Clients.TagPlus.Models.Pedidos.Item tagPlusItem = new Clients.TagPlus.Models.Pedidos.Item();
                    tagPlusItem.NumItem = i;
                    tagPlusItem.ProdutoServico = produtoServico;
                    tagPlusItem.Qtd = Convert.ToInt32(float.Parse(blingItem.Quantidade, CultureInfo.InvariantCulture.NumberFormat));
                    tagPlusItem.ValorUnitario = float.Parse(blingItem.Valorunidade, CultureInfo.InvariantCulture.NumberFormat);
                    tagPlusItem.ValorDesconto = float.Parse(blingItem.DescontoItem, CultureInfo.InvariantCulture.NumberFormat);
                    itens.Add(tagPlusItem);
                }

                // Recupera as faturas
                IList<Fatura> faturas = new List<Fatura>();
                Fatura fatura = new Fatura();
                fatura.Parcelas = new List<Clients.TagPlus.Models.Pedidos.Parcela>();
                foreach (ParcelaItem parcelaWrapper in pedido.Pedido.Parcelas)
                {
                    Clients.Bling.Models.Pedidos.Parcela parcela = parcelaWrapper.Parcela;
                    int formaPagamento = tagPlusClient.GetFormasPagamento(parcela.FormaPagamento.Descricao);
                    // Converte a data de vencimento
                    string date = DateTime.Parse(parcela.DataVencimento).ToString("yyyy-MM-dd");
                    Clients.TagPlus.Models.Pedidos.Parcela parcelaTagPlus = new Clients.TagPlus.Models.Pedidos.Parcela();
                    parcelaTagPlus.ValorParcela = float.Parse(parcela.Valor, CultureInfo.InvariantCulture.NumberFormat);
                    parcelaTagPlus.DataVencimento = date;
                    fatura.Parcelas.Add(parcelaTagPlus);
                    fatura.FormaPagamento = formaPagamento;
                }
                faturas.Add(fatura);

                // Cria o corpo do pedido
                PedidoBody body = new PedidoBody();
                body.CodigoExterno = pedido.Pedido.Numero;
                body.Cliente = clienteId;
                body.Itens = itens;
                body.Faturas = faturas;
                body.ValorFrete = float.Parse(pedido.Pedido.Valorfrete, CultureInfo.InvariantCulture.NumberFormat);
                body.Observacoes = $"Pedido: {pedido.Pedido.Numero}\n" +
                    $"Observações: {pedido.Pedido.Observacoes.Trim()}\nObservações internas: {pedido.Pedido.Observacaointerna}\n" +
                    $"Número Pedido Loja: {pedido.Pedido.NumeroPedidoLoja}\nTipo da Integração: {pedido.Pedido.TipoIntegracao}";

                // Envia o novo pedido
                try
                {
                    GetPedidosResponse response = tagPlusClient.PostPedidos(body);
                    Console.WriteLine($"Pedido cadastrado no TagPlus com o ID: {response.Id}");
                }
                catch (TagPlusException e)
                {
                    Console.WriteLine($"Não foi possível cadastrar o pedido: {e.Message}");
                }

                Console.WriteLine("--------------------------------------------");
            }

            Console.WriteLine("Processo finalizado");
        }
    }
}
