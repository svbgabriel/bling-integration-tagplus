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
using System.Linq;

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

            // Inicializa o cliente do Bling
            var blingClient = new BlingClient(blingApiKey);

            // Encontra as situações
            var situacoes = blingClient.ExecuteGetSituacao();
            var situacaoImportado = situacoes.Retorno.Situacoes.First(situacao => situacao.Situacao.Nome.Equals("Importado no TagPlus"));
            var situacaoEmAberto = situacoes.Retorno.Situacoes.First(situacao => situacao.Situacao.Nome.Equals("Em aberto"));

            // Recupera os pedidos do Bling
            Console.WriteLine();
            Console.WriteLine("Procurando pedidos no Bling...");

            Clients.Bling.Models.Pedidos.GetPedidosResponse pedidos = null;
            try
            {
                pedidos = blingClient.ExecuteGetOrder(situacaoEmAberto.Situacao.Id);
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

            TagPlusClient tagPlusClient = new TagPlusClient(code);

            // Encontra os tipos de contato
            var tiposContato = tagPlusClient.GetTiposContatos();
            var emailContato = tiposContato.First(contato => contato.Descricao.Equals("Email"));
            var celularContato = tiposContato.First(contato => contato.Descricao.Equals("Celular"));
            var telefoneContato = tiposContato.First(contato => contato.Descricao.Equals("Telefone"));

            // Encontra as formas de pagamento
            var formasPagamento = tagPlusClient.GetFormasPagamentos();

            // Envia os pedidos para o TagPlus
            Console.WriteLine($"Foram encontrados {pedidos.Retorno.Pedidos.Count} pedido(s)");
            foreach (PedidoItem pedido in pedidos.Retorno.Pedidos)
            {
                Console.WriteLine();
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine($"Tratando o Pedido {pedido.Pedido.Numero}");
                // Tenta recupera o Cliente de várias maneiras
                int clienteId = 0;
                if (pedido.Pedido.Cliente.Cnpj != null)
                {
                    if (ValidateUtils.IsCpf(pedido.Pedido.Cliente.Cnpj))
                    {
                        clienteId = tagPlusClient.GetClienteByCpf(pedido.Pedido.Cliente.Cnpj);
                    }
                    else if (ValidateUtils.IsCnpj(pedido.Pedido.Cliente.Cnpj))
                    {
                        clienteId = tagPlusClient.GetClienteByCnpj(pedido.Pedido.Cliente.Cnpj);
                    }

                }
                else
                {
                    clienteId = tagPlusClient.GetClienteByRazaoSocial(pedido.Pedido.Cliente.Nome);
                }
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

                    // Preenche as informações de contato
                    cliente.Contatos = new List<Contato>();
                    // Verifica se existe o e-mail
                    if (pedido.Pedido.Cliente.Email != null)
                    {
                        Contato email = new Contato
                        {
                            TipoContato = emailContato.Id,
                            Descricao = pedido.Pedido.Cliente.Email,
                            Principal = true
                        };
                        cliente.Contatos.Add(email);
                    }
                    // Verifica se existe o celular
                    if (pedido.Pedido.Cliente.Celular != null)
                    {
                        Contato celular = new Contato
                        {
                            TipoContato = celularContato.Id,
                            Descricao = pedido.Pedido.Cliente.Celular,
                            Principal = true
                        };
                        cliente.Contatos.Add(celular);
                    }
                    // Verifica se existe o telefone
                    if (pedido.Pedido.Cliente.Fone != null)
                    {
                        Contato fone = new Contato
                        {
                            Id = telefoneContato.Id,
                            Descricao = pedido.Pedido.Cliente.Fone,
                            Principal = true
                        };
                        cliente.Contatos.Add(fone);
                    }

                    // Caso não seja encontrado nenhum contrato, remove do cadastro
                    if (cliente.Contatos.Count == 0)
                    {
                        cliente.Contatos = null;
                    }

                    // Preenche o endereço, se estiver disponível
                    if (pedido.Pedido.Transporte != null)
                    {
                        cliente.Enderecos = new List<Endereco>();
                        Endereco endereco = new Endereco
                        {
                            Logradouro = pedido.Pedido.Transporte.EnderecoEntrega.Endereco,
                            Numero = pedido.Pedido.Transporte.EnderecoEntrega.Numero,
                            Bairro = pedido.Pedido.Transporte.EnderecoEntrega.Bairro,
                            Complemento = pedido.Pedido.Transporte.EnderecoEntrega.Complemento,
                            Cep = pedido.Pedido.Transporte.EnderecoEntrega.Cep.Replace(".", ""),
                            Principal = true
                        };
                        cliente.Enderecos.Add(endereco);
                    }
                    else if (pedido.Pedido.Cliente.Endereco != null && pedido.Pedido.Cliente.Numero != null
                        && pedido.Pedido.Cliente.Cep != null && pedido.Pedido.Cliente.Bairro != null)
                    {
                        cliente.Enderecos = new List<Endereco>();
                        Endereco endereco = new Endereco
                        {
                            Logradouro = pedido.Pedido.Cliente.Endereco,
                            Numero = pedido.Pedido.Cliente.Numero,
                            Bairro = pedido.Pedido.Cliente.Bairro,
                            Complemento = pedido.Pedido.Cliente.Complemento,
                            Cep = pedido.Pedido.Cliente.Cep.Replace(".", ""),
                            Principal = true
                        };
                        cliente.Enderecos.Add(endereco);
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
                    Clients.TagPlus.Models.Pedidos.Item tagPlusItem = new Clients.TagPlus.Models.Pedidos.Item
                    {
                        NumItem = i,
                        ProdutoServico = produtoServico,
                        Qtd = Convert.ToInt32(float.Parse(blingItem.Quantidade, CultureInfo.InvariantCulture.NumberFormat)),
                        ValorUnitario = float.Parse(blingItem.Valorunidade, CultureInfo.InvariantCulture.NumberFormat),
                        ValorDesconto = float.Parse(blingItem.DescontoItem, CultureInfo.InvariantCulture.NumberFormat)
                    };
                    itens.Add(tagPlusItem);
                }

                // Recupera as faturas
                IList<Fatura> faturas = new List<Fatura>();
                Fatura fatura = new Fatura
                {
                    Parcelas = new List<Clients.TagPlus.Models.Pedidos.Parcela>()
                };
                foreach (ParcelaItem parcelaWrapper in pedido.Pedido.Parcelas)
                {
                    Clients.Bling.Models.Pedidos.Parcela parcela = parcelaWrapper.Parcela;
                    int formaPagamento = formasPagamento.First(forma => forma.Descricao.Equals(parcela.FormaPagamento.Descricao)).Id;
                    // Converte a data de vencimento
                    string date = DateTime.Parse(parcela.DataVencimento).ToString("yyyy-MM-dd");
                    Clients.TagPlus.Models.Pedidos.Parcela parcelaTagPlus = new Clients.TagPlus.Models.Pedidos.Parcela
                    {
                        ValorParcela = float.Parse(parcela.Valor, CultureInfo.InvariantCulture.NumberFormat),
                        DataVencimento = date
                    };
                    fatura.Parcelas.Add(parcelaTagPlus);
                    fatura.FormaPagamento = formaPagamento;
                }
                faturas.Add(fatura);

                // Cria o corpo do pedido
                PedidoBody body = new PedidoBody
                {
                    CodigoExterno = pedido.Pedido.Numero,
                    Cliente = clienteId,
                    Itens = itens,
                    Faturas = faturas,
                    ValorFrete = float.Parse(pedido.Pedido.Valorfrete, CultureInfo.InvariantCulture.NumberFormat),
                    Observacoes = $"Pedido: {pedido.Pedido.Numero}\n" +
                    $"Observações: {pedido.Pedido.Observacoes.Trim()}\nObservações internas: {pedido.Pedido.Observacaointerna}\n" +
                    $"Número Pedido Loja: {pedido.Pedido.NumeroPedidoLoja}\nTipo da Integração: {pedido.Pedido.TipoIntegracao}"
                };

                // Envia o novo pedido
                try
                {
                    Clients.TagPlus.Models.Pedidos.GetPedidosResponse response = tagPlusClient.PostPedidos(body);
                    Console.WriteLine($"Pedido cadastrado no TagPlus com o ID: {response.Id}");
                }
                catch (TagPlusException e)
                {
                    Console.WriteLine($"Não foi possível cadastrar o pedido: {e.Message}");
                }

                // Atualiza a situação no Bling
                Console.WriteLine("Atualizando a situação no Bling");
                try
                {
                    var pedidoUpdated = blingClient.ExecuteUpdateOrder(pedido.Pedido.Numero, situacaoImportado.Situacao.Id);
                    Console.WriteLine($"O pedido {pedido.Pedido.Numero} foi atualizado");
                }
                catch (BlingException e)
                {
                    Console.WriteLine($"Não foi possível atualizar o pedido {pedido.Pedido.Numero} no Bling: {e.Message}");
                }

                Console.WriteLine("--------------------------------------------");
                Console.WriteLine();
            }

            Console.WriteLine("Processo finalizado");
        }
    }
}
