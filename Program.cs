using BlingIntegrationTagplus.Clients.Bling;
using BlingIntegrationTagplus.Clients.TagPlus;
using BlingIntegrationTagplus.Clients.TagPlus.Models.FormasPagamento;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Fornecedores;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Pedidos;
using BlingIntegrationTagplus.Exceptions;
using BlingIntegrationTagplus.Models;
using BlingIntegrationTagplus.Services;
using BlingIntegrationTagplus.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

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
                .WriteTo.File($"logs{Path.AltDirectorySeparatorChar}integration-{DateTime.Now:yyyyMMdd}.log")
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("############################################");
            Log.Information("## Bem vindo a integração Bling - Tagplus ##");
            Log.Information("############################################");

            var versao = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            Log.Information($"Versão: {versao}");

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
            var situacaoService = new SituacaoService(blingClient);
            var blingPedidoService = new BlingPedidoService(blingClient, config);
            var produtoService = new ProdutoService(tagPlusClient);
            var faturaService = new FaturaService();

            // Encontra as situações
            Dictionary<string, string> situacoes = null;
            try
            {
                situacoes = situacaoService.GetSituacoes();
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
            situacoes.TryGetValue("IMPORTADO", out var situacaoImportado);

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

            // Encontra os fornecedores
            IList<GetFornecedoresResponse> fornecedores = null;
            try
            {
                fornecedores = tagPlusClient.GetFornecedores();
            }
            catch (TagPlusException e)
            {
                Log.Error($"Não foi possível recuperar os fornecedores: {e.Message}");
                Log.Information("Aperte Enter para fechar");
                Console.ReadLine();
                Log.Information("Encerrando");
                Log.CloseAndFlush();
                Environment.Exit(-1);
            }

            // Recupera o dicionário de fornecedores
            Dictionary<string, string> dicFornecedores = null;
            try
            {
                dicFornecedores = FileUtils.ReadFornecedoresFile();
            }
            catch (UtilsException e)
            {
                Log.Error($"Não foi possível recuperar os fornecedores do arquivo: {e.Message}");
                Log.Information("Aperte Enter para fechar");
                Console.ReadLine();
                Log.Information("Encerrando");
                Log.CloseAndFlush();
                Environment.Exit(-1);
            }

            Log.Information("Configurações carregadas");

            // Recupera os pedidos do Bling
            Log.Information("Procurando pedidos no Bling...");

            var pedidos = blingPedidoService.GetPedidos(situacoes, initialDate);

            // Verifica se existem pedidos
            if (pedidos.Count == 0)
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
            foreach (var pedido in pedidos)
            {
                Log.Information("--------------------------------------------");
                Log.Information($"Tratando o Pedido {pedido.Pedido.Numero}");

                // Tenta recupera o Cliente de várias maneiras
                var clienteId = clieteService.GetCliente(pedido);

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
                        continue;
                    }
                }

                // Recupera os itens e coloca na lista intermediária
                List<Produto> produtos;
                try
                {
                    produtos = produtoService.GetListaProdutos(pedido);
                }
                catch (ProdutoException e)
                {
                    Log.Error(e.Message);
                    Log.Information("Aperte Enter para continuar");
                    Console.ReadLine();
                    continue;
                }

                // Monta a lista de pedidos
                var listaPedidos = produtoService.GetListaPedidos(produtos);

                // Recupera as faturas
                var faturas = faturaService.ConstructFatura(pedido, formasPagamento);

                if (faturas.Count == 0)
                {
                    Log.Information("Não foram encontradas faturas no pedido");
                    continue;
                }

                // Cria o corpo do pedido
                var body = new PedidoBody
                {
                    CodigoExterno = pedido.Pedido.Numero,
                    Cliente = clienteId,
                    Itens = listaPedidos,
                    Faturas = faturas,
                    ValorFrete = float.Parse(pedido.Pedido.Valorfrete, CultureInfo.InvariantCulture.NumberFormat),
                    Observacoes = $"Pedido: {pedido.Pedido.Numero}\n" +
                    $"Observações: {pedido.Pedido.Observacoes.Trim()}\nObservações internas: {pedido.Pedido.Observacaointerna}\n" +
                    $"Número Pedido Loja: {pedido.Pedido.NumeroPedidoLoja}\nTipo da Integração: {pedido.Pedido.TipoIntegracao}"
                };

                // Envia o novo pedido
                GetPedidosResponse response;
                try
                {
                    response = tagPlusClient.PostPedidos(body);
                    Log.Information($"Pedido cadastrado no TagPlus com o ID: {response.Id}");
                }
                catch (TagPlusException e)
                {
                    Log.Error($"Não foi possível cadastrar o pedido: {e.Message}");
                    continue;
                }

                // Monta a lista de corpo do pedido de compra
                var itensCompra = produtoService.GetListaPedidosCompra(produtos, response.Numero, fornecedores, dicFornecedores);

                // Envia os novos pedidos de compra
                var error = false;
                foreach (var pedidoCompraBody in itensCompra)
                {
                    try
                    {
                        var responsePedidoCompra = tagPlusClient.PostPedidoCompra(pedidoCompraBody);
                        Log.Information($"Pedido de compra cadastrado no TagPlus com o Número: {responsePedidoCompra.Numero}");
                    }
                    catch (ProdutoException e)
                    {
                        Log.Error($"Não foi possível cadastrar o pedido de compra: {e.Message}");
                        error = true;
                        break;
                    }
                }

                if (error)
                {
                    Log.Error("O pedido terá que ser cadastrado manualmente");
                    continue;
                }

                // Atualiza a situação no Bling
                Log.Information("Atualizando a situação no Bling");
                try
                {
                    blingClient.ExecuteUpdateOrder(pedido.Pedido.Numero, situacaoImportado);
                    Log.Information($"O pedido {pedido.Pedido.Numero} foi atualizado");
                }
                catch (BlingException e)
                {
                    Log.Error($"Não foi possível atualizar o pedido {pedido.Pedido.Numero} no Bling: {e.Message}");
                    Log.Error($"O pedido {pedido.Pedido.Numero} deve ser atualizado manualmente no Bling");
                    Log.Information("Aperte Enter para continuar");
                    Console.ReadLine();
                }
            }

            Log.Information("--------------------------------------------");
            Log.Information("Processo finalizado");
            Log.Information("Aperte Enter para fechar");
            Console.ReadLine();
            Log.CloseAndFlush();
        }
    }
}
