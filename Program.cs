using BlingIntegrationTagplus.Clients.Bling;
using BlingIntegrationTagplus.Clients.Bling.Filters;
using BlingIntegrationTagplus.Clients.Bling.Models.Pedidos;
using BlingIntegrationTagplus.Clients.TagPlus;
using BlingIntegrationTagplus.Clients.TagPlus.Models.FormasPagamento;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Pedidos;
using BlingIntegrationTagplus.Exceptions;
using BlingIntegrationTagplus.Models;
using BlingIntegrationTagplus.Services;
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

        protected Program() { }

        static void Main()
        {
            // Inicializa o logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File($"logs{Path.AltDirectorySeparatorChar}integration-{DateTime.Now:yyyyMMddHHmmss}.log")
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("############################################");
            Log.Information("## Bem vindo a integração Bling - Tagplus ##");
            Log.Information("############################################");

            Log.Information("Carregando as configurações...");

            // Carrega as configurações
            Config config = null;
            try
            {
                config = ConfigLoader.LoadConfig();
            }
            catch (ConfigException e)
            {
                Log.Error(e.Message);
                Log.Information("Aperte Enter para fechar");
                Console.ReadLine();
                Log.CloseAndFlush();
                Environment.Exit(-1);
            }

            Log.Information("Iniciando o processo");

            // Verifica a data inicial
            var initialDate = DateTime.Parse(config.BlingInitialDate);
            if (initialDate.CompareTo(DateTime.Now) > 0)
            {
                Log.Information("A data inicial informada é maior que a data atual. O processo não será executado");
                Log.Information("Aperte Enter para fechar");
                Console.ReadLine();
                Log.Information("Processo finalizado");
                Log.CloseAndFlush();
                Environment.Exit(0);
            }

            // Inicializa os clientes
            var blingClient = new BlingClient(config.BlingApiKey, config.BlingApiUrl);
            var tagPlusClient = new TagPlusClient(config.TagplusToken, config.TagplusApiUrl);

            // Inicializa os Services
            var clieteService = new ClienteService(tagPlusClient);
            var tipoContatoService = new TipoContatoService(tagPlusClient);
            var SituacaoService = new SituacaoService(blingClient);

            // Encontra as situações
            Dictionary<string, string> situacoes = null;
            try
            {
                situacoes = SituacaoService.GetSituacoes();
            }
            catch (SituacaoException e)
            {
                Log.Error(e.Message);
                Log.Information("Aperte Enter para fechar");
                Console.ReadLine();
                Log.Information("Encerrando");
                Log.CloseAndFlush();
                Environment.Exit(-1);
            }
            situacoes.TryGetValue("IMPORTADO", out string situacaoImportado);
            situacoes.TryGetValue("ABERTO", out string situacaoEmAberto);
            situacoes.TryGetValue("ANDAMENTO", out string situacaoEmAndamento);

            // Encontra os tipos de contato
            Dictionary<string, int> tiposContato = null;
            try
            {
                tiposContato = tipoContatoService.GetListaContatos();
            }
            catch (TipoContatoException e)
            {
                Log.Error(e.Message);
                Log.Information("Aperte Enter para fechar");
                Console.ReadLine();
                Log.Information("Encerrando");
                Log.CloseAndFlush();
                Environment.Exit(-1);
            }

            // Encontra as formas de pagamento
            IList<GetFormasPagamentoResponse> formasPagamento = null;
            try
            {
                formasPagamento = tagPlusClient.GetFormasPagamentos();
            }
            catch (TagPlusException e)
            {
                Log.Error($"Não foi possível recuperar as formas de pagamento: {e.Message}");
                Log.Information("Aperte Enter para fechar");
                Console.ReadLine();
                Log.Information("Encerrando");
                Log.CloseAndFlush();
                Environment.Exit(-1);
            }

            // Recupera os pedidos do Bling
            Log.Information("Procurando pedidos no Bling...");

            List<PedidoItem> pedidos = null;
            try
            {
                BuildOrdersFilter filters = new BuildOrdersFilter();
                string filter = filters.AddDateFilter(initialDate, DateTime.Now)
                    .AddSituation(situacaoEmAberto)
                    .Build();

                // Verifica se somente um pedido será importado
                if (string.IsNullOrWhiteSpace(config.BlingOrderNum))
                {
                    pedidos = blingClient.ExecuteGetOrder(filter);
                }
                else
                {
                    Log.Information($"Procurando somente pelo pedido {config.BlingOrderNum} no Bling");
                    int orderNum = int.Parse(config.BlingOrderNum);
                    pedidos = blingClient.ExecuteGetOrder(orderNum);
                }
            }
            catch (BlingException e)
            {
                Log.Error($"Não foi possível recuperar os pedidos do Bling - {e.Message}");
                Log.Information("Aperte Enter para fechar");
                Console.ReadLine();
                Log.Information("Encerrando");
                Log.CloseAndFlush();
                Environment.Exit(-1);
            }

            if (string.IsNullOrWhiteSpace(config.BlingOrderNum))
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
                    Log.Error($"Não foi possível recuperar os pedidos do Íntegra no Bling - {e.Message}");
                    Log.Information("Aperte Enter para continuar");
                    Console.ReadLine();
                }

                // Junta as listas de pedidos
                pedidos.AddRange(pedidosIntegra);
            }

            // Verifica se existem pedidos
            if (pedidos == null || pedidos.Count == 0)
            {
                Log.Information("Não foram encontrados pedidos no Bling");
                Log.Information("Processo finalizado");
                Log.Information("Aperte Enter para fechar");
                Console.ReadLine();
                Log.CloseAndFlush();
                Environment.Exit(0);
            }

            // Envia os pedidos para o TagPlus
            Log.Information($"Foram encontrados {pedidos.Count} pedido(s)");
            foreach (PedidoItem pedido in pedidos)
            {
                Log.Information("--------------------------------------------");
                Log.Information($"Tratando o Pedido {pedido.Pedido.Numero}");

                // Tenta recupera o Cliente de várias maneiras
                int clienteId = clieteService.GetCliente(pedido);

                // Cria se não existir
                if (clienteId == 0)
                {
                    Log.Information($"Cliente {pedido.Pedido.Cliente.Nome} não foi encontrado, cadastrando...");

                    // Envia o novo cliente
                    try
                    {
                        clienteId = clieteService.CreateCliente(pedido, tiposContato);
                    }
                    catch (ClienteException e)
                    {
                        Log.Error(e.Message);
                        Log.Information("--------------------------------------------");
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
                    Log.Information($"Pedido cadastrado no TagPlus com o ID: {response.Id}");
                }
                catch (TagPlusException e)
                {
                    Log.Error($"Não foi possível cadastrar o pedido: {e.Message}");
                    Log.Information("--------------------------------------------");
                    continue;
                }

                // Atualiza a situação no Bling
                Log.Information("Atualizando a situação no Bling");
                try
                {
                    var pedidoUpdated = blingClient.ExecuteUpdateOrder(pedido.Pedido.Numero, situacaoImportado);
                    Log.Information($"O pedido {pedido.Pedido.Numero} foi atualizado");
                }
                catch (BlingException e)
                {
                    Log.Error($"Não foi possível atualizar o pedido {pedido.Pedido.Numero} no Bling: {e.Message}");
                    Log.Error($"O pedido {pedido.Pedido.Numero} deve ser atualizado manualmente no Bling");
                    Log.Information("Aperte Enter para continuar");
                    Console.ReadLine();
                }

                Log.Information("--------------------------------------------");
            }

            Log.Information("Processo finalizado");
            Log.Information("Aperte Enter para fechar");
            Console.ReadLine();
            Log.CloseAndFlush();
        }
    }
}
