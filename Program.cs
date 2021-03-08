using BlingIntegrationTagplus.Clients.Bling;
using BlingIntegrationTagplus.Clients.Bling.Filters;
using BlingIntegrationTagplus.Clients.Bling.Models.Pedidos;
using BlingIntegrationTagplus.Clients.Bling.Models.Situacao;
using BlingIntegrationTagplus.Clients.TagPlus;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Clientes;
using BlingIntegrationTagplus.Clients.TagPlus.Models.FormasPagamento;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Pedidos;
using BlingIntegrationTagplus.Clients.TagPlus.Models.TiposContatos;
using BlingIntegrationTagplus.Exceptions;
using BlingIntegrationTagplus.Utils;
using dotenv.net;
using dotenv.net.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BlingIntegrationTagplus
{
    class Program
    {

        protected Program() {}

        static void Main()
        {
            Console.WriteLine("############################################");
            Console.WriteLine("## Bem vindo a integração Bling - Tagplus ##");
            Console.WriteLine("############################################");

            Console.WriteLine();
            Console.WriteLine("Preparando...");

            // Define a cor de erro
            ConsoleColor errorColor = ConsoleColor.Red;

            // Carrega o arquivo de configuração
            string blingApiKey = "";
            string blingInitialDate = "";
            string tagplusToken = "";
            string blingOrderNum = "";
            try
            {
                DotEnv.Config();
                var envReader = new EnvReader();
                // Carrega a API KEY do Bling
                blingApiKey = envReader.GetStringValue("BLING_API_KEY");
                // Carrega a data inicial do Bling
                blingInitialDate = envReader.GetStringValue("BLING_INITIAL_DATE");
                // Carrega o Token do Tagplus
                tagplusToken = envReader.GetStringValue("TAGPLUS_TOKEN");
                // Carrega o número de pedido especifico
                envReader.TryGetStringValue("BLING_ORDER_NUM", out blingOrderNum);
            }
            catch (FileNotFoundException)
            {
                Console.ForegroundColor = errorColor;
                Console.WriteLine($"O arquivo .env não foi encontrado");
                Console.WriteLine("Aperte Enter para fechar");
                Console.ReadLine();
                Console.ResetColor();
                Environment.Exit(-1);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = errorColor;
                Console.WriteLine($"Erro: {e.Message}");
                Console.WriteLine("Aperte Enter para fechar");
                Console.ReadLine();
                Console.ResetColor();
                Environment.Exit(-1);
            }

            // Inicializa o logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File($"logs{Path.AltDirectorySeparatorChar}integration-{DateTime.Now:yyyyMMddHHmmss}.log")
                .CreateLogger();

            Log.Information("Iniciando o processo");

            // Verifica a data inicial
            var initialDate = DateTime.Parse(blingInitialDate);
            if (initialDate.CompareTo(DateTime.Now) > 0)
            {
                Console.WriteLine("A data inicial informada é maior que a data atual. O processo não será executado");
                Console.WriteLine("Aperte Enter para fechar");
                Console.ReadLine();
                Log.Information("Processo finalizado");
                Log.CloseAndFlush();
                Environment.Exit(0);
            }

            // Inicializa o cliente do Bling
            var blingClient = new BlingClient(blingApiKey);

            // Encontra as situações
            GetSituacaoResponse situacoes = null;
            try
            {
                situacoes = blingClient.ExecuteGetSituacao();
            }
            catch (BlingException e)
            {
                Console.ForegroundColor = errorColor;
                Log.Error($"Não foi possível recuperar as situações: {e.Message}");
                Console.WriteLine($"Não foi possível recuperar as situações: {e.Message}");
                Console.WriteLine("Aperte Enter para fechar");
                Console.ReadLine();
                Console.ResetColor();
                Environment.Exit(-1);
            }
            var situacaoImportado = situacoes.Retorno.Situacoes.First(situacao => situacao.Situacao.Nome.Equals("Importado no TagPlus")).Situacao.Id;
            var situacaoEmAberto = situacoes.Retorno.Situacoes.First(situacao => situacao.Situacao.Nome.Equals("Em aberto")).Situacao.Id;
            var situacaoEmAndamento = situacoes.Retorno.Situacoes.First(situacao => situacao.Situacao.Nome.Equals("Em andamento")).Situacao.Id;

            TagPlusClient tagPlusClient = new TagPlusClient(tagplusToken);

            // Encontra os tipos de contato
            IList<GetTiposContatosResponse> tiposContato = null;
            try
            {
                tiposContato = tagPlusClient.GetTiposContatos();
            }
            catch (TagPlusException e)
            {
                Console.ForegroundColor = errorColor;
                Log.Error($"Não foi possível recuperar os tipos de contato: {e.Message}");
                Console.WriteLine($"Não foi possível recuperar os tipos de contato: {e.Message}");
                Console.WriteLine("Aperte Enter para fechar");
                Console.ReadLine();
                Console.ResetColor();
                Environment.Exit(-1);
            }
            var emailContato = tiposContato.First(contato => contato.Descricao.Equals("Email")).Id;
            var celularContato = tiposContato.First(contato => contato.Descricao.Equals("Celular")).Id;
            var telefoneContato = tiposContato.First(contato => contato.Descricao.Equals("Telefone")).Id;

            // Encontra as formas de pagamento
            IList<GetFormasPagamentoResponse> formasPagamento = null;
            try
            {
                formasPagamento = tagPlusClient.GetFormasPagamentos();
            }
            catch (TagPlusException e)
            {
                Console.ForegroundColor = errorColor;
                Log.Error($"Não foi possível recuperar as formas de pagamento: {e.Message}");
                Console.WriteLine($"Não foi possível recuperar as formas de pagamento: {e.Message}");
                Console.WriteLine("Aperte Enter para fechar");
                Console.ReadLine();
                Console.ResetColor();
                Environment.Exit(-1);
            }

            // Recupera os pedidos do Bling
            Console.WriteLine();
            Console.WriteLine("Procurando pedidos no Bling...");

            List<PedidoItem> pedidos = null;
            try
            {
                BuildOrdersFilter filters = new BuildOrdersFilter();
                string filter = filters.AddDateFilter(initialDate, DateTime.Now)
                    .AddSituation(situacaoEmAberto)
                    .Build();

                // Verifica se somente um pedido será importado
                if (string.IsNullOrWhiteSpace(blingOrderNum))
                {
                    pedidos = blingClient.ExecuteGetOrder(filter);
                }
                else
                {
                    Console.WriteLine($"Procurando somente pelo pedido {blingOrderNum} no Bling");
                    int orderNum = int.Parse(blingOrderNum);
                    pedidos = blingClient.ExecuteGetOrder(orderNum);                    
                }
            }
            catch (BlingException e)
            {
                Console.ForegroundColor = errorColor;
                Log.Error($"Não foi possível recuperar os pedidos do Bling - {e.Message}");
                Console.WriteLine($"Não foi possível recuperar os pedidos do Bling - {e.Message}");
                Console.WriteLine("Aperte Enter para fechar");
                Console.ReadLine();
                Console.ResetColor();
                Environment.Exit(-1);
            }

            if (string.IsNullOrWhiteSpace(blingOrderNum))
            {
                // Contorno para pedidos do Íntegra
                List<PedidoItem> pedidosIntegra = new List<PedidoItem>();
                try
                {
                    BuildOrdersFilter filters = new BuildOrdersFilter();
                    string filter = filters.AddDateFilter(initialDate, DateTime.Now)
                        .AddSituation(situacaoEmAndamento)
                        .Build();
                    List<PedidoItem> pedidosIntegraTotal = blingClient.ExecuteGetOrder(filter);
                    // Filtra os pedidos para somente os do canal Íntegra
                    pedidosIntegra = pedidosIntegraTotal.Where(pedido => pedido.Pedido.TipoIntegracao.Equals("IntegraCommerce")).ToList();
                }
                catch (BlingException e)
                {
                    Console.ForegroundColor = errorColor;
                    Log.Error($"Não foi possível recuperar os pedidos do Íntegra no Bling - {e.Message}");
                    Console.WriteLine($"Não foi possível recuperar os pedidos do Íntegra no Bling - {e.Message}");
                    Console.WriteLine("Aperte Enter para continuar");
                    Console.ResetColor();
                    Console.ReadLine();
                }

                // Junta as listas de pedidos
                pedidos.AddRange(pedidosIntegra);
            }

            // Verifica se existem pedidos
            if (pedidos == null || pedidos.Count == 0)
            {
                Console.WriteLine("Não foram encontrados pedidos no Bling");
                Log.Information("Processo finalizado");
                Console.WriteLine("Aperte Enter para fechar");
                Console.ReadLine();
                Log.CloseAndFlush();
                Environment.Exit(0);
            }

            // Envia os pedidos para o TagPlus
            Console.WriteLine($"Foram encontrados {pedidos.Count} pedido(s)");
            foreach (PedidoItem pedido in pedidos)
            {
                Console.WriteLine();
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine($"Tratando o Pedido {pedido.Pedido.Numero}");
                // Tenta recupera o Cliente de várias maneiras
                int clienteId = 0;
                if (!string.IsNullOrWhiteSpace(pedido.Pedido.Cliente.Cnpj))
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
                    List<Contato> contatos = new List<Contato>();
                    // Verifica se existe o telefone
                    if (!string.IsNullOrWhiteSpace(pedido.Pedido.Cliente.Fone))
                    {
                        Contato fone = new Contato
                        {
                            TipoContato = telefoneContato,
                            Descricao = pedido.Pedido.Cliente.Fone,
                            Principal = true
                        };
                        contatos.Add(fone);
                    }
                    // Verifica se existe o e-mail
                    if (!string.IsNullOrWhiteSpace(pedido.Pedido.Cliente.Email))
                    {
                        Contato email = new Contato
                        {
                            TipoContato = emailContato,
                            Descricao = pedido.Pedido.Cliente.Email,
                            Principal = true
                        };
                        contatos.Add(email);
                    }
                    // Verifica se existe o celular
                    if (!string.IsNullOrWhiteSpace(pedido.Pedido.Cliente.Celular))
                    {
                        Contato celular = new Contato
                        {
                            TipoContato = celularContato,
                            Descricao = pedido.Pedido.Cliente.Celular,
                            Principal = true
                        };
                        contatos.Add(celular);
                    }

                    // Caso existam contatos, adiciona ao contato
                    if (contatos.Count > 0)
                    {
                        cliente.Contatos = contatos;
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
                        Console.ForegroundColor = errorColor;
                        Log.Error($"Não foi possível cadastrar o cliente: {e.Message}");
                        Console.WriteLine($"Não foi possível cadastrar o cliente: {e.Message}");
                        Console.WriteLine("--------------------------------------------");
                        Console.WriteLine();
                        Console.ResetColor();
                        continue;
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
                    Console.ForegroundColor = errorColor;
                    Log.Error($"Não foi possível cadastrar o pedido: {e.Message}");
                    Console.WriteLine($"Não foi possível cadastrar o pedido: {e.Message}");
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine();
                    Console.ResetColor();
                    continue;
                }

                // Atualiza a situação no Bling
                Console.WriteLine("Atualizando a situação no Bling");
                try
                {
                    var pedidoUpdated = blingClient.ExecuteUpdateOrder(pedido.Pedido.Numero, situacaoImportado);
                    Console.WriteLine($"O pedido {pedido.Pedido.Numero} foi atualizado");
                }
                catch (BlingException e)
                {
                    Console.ForegroundColor = errorColor;
                    Log.Error($"Não foi possível atualizar o pedido {pedido.Pedido.Numero} no Bling: {e.Message}");
                    Console.WriteLine($"Não foi possível atualizar o pedido {pedido.Pedido.Numero} no Bling: {e.Message}");
                    Console.WriteLine("Aperte Enter para fechar");
                    Console.ResetColor();
                    Console.ReadLine();
                }

                Console.WriteLine("--------------------------------------------");
                Console.WriteLine();
            }

            Log.Information("Processo finalizado");
            Log.CloseAndFlush();
            Console.WriteLine("Processo finalizado");
            Console.WriteLine("Aperte Enter para fechar");
            Console.ReadLine();
        }
    }
}
