using BlingIntegrationTagplus.Clients.Bling;
using BlingIntegrationTagplus.Clients.Bling.Filters;
using BlingIntegrationTagplus.Clients.Bling.Models.Pedidos;
using BlingIntegrationTagplus.Exceptions;
using BlingIntegrationTagplus.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlingIntegrationTagplus.Services
{
    class BlingPedidoService
    {
        private readonly BlingClient _blingClient;
        private readonly Config _config;

        public BlingPedidoService(BlingClient blingClient, Config config)
        {
            _blingClient = blingClient;
            _config = config;
        }

        public List<PedidoItem> GetPedidos(Dictionary<string, string> situacoes, DateTime initialDate)
        {
            situacoes.TryGetValue("ABERTO", out var situacaoEmAberto);
            situacoes.TryGetValue("ANDAMENTO", out var situacaoEmAndamento);

            List<PedidoItem> pedidos;
            try
            {
                var filters = new BuildOrdersFilter();
                var filter = filters.AddDateFilter(initialDate, DateTime.Now)
                    .AddSituation(situacaoEmAberto)
                    .Build();

                // Verifica se somente um pedido será importado
                if (string.IsNullOrWhiteSpace(_config.BlingOrderNum))
                {
                    pedidos = _blingClient.ExecuteGetOrder(filter);
                }
                else
                {
                    Log.Information($"Procurando somente pelo pedido {_config.BlingOrderNum} no Bling");
                    var orderNum = int.Parse(_config.BlingOrderNum);
                    pedidos = _blingClient.ExecuteGetOrder(orderNum);
                }
            }
            catch (BlingException e)
            {
                Log.Error($"Não foi possível recuperar os pedidos do Bling - {e.Message}");
                return new List<PedidoItem>();
            }

            if (string.IsNullOrWhiteSpace(_config.BlingOrderNum))
            {
                // Contorno para pedidos do Íntegra
                List<PedidoItem> pedidosIntegra;
                try
                {
                    var filters = new BuildOrdersFilter();
                    var filter = filters.AddDateFilter(initialDate, DateTime.Now)
                        .AddSituation(situacaoEmAndamento)
                        .Build();
                    var pedidosIntegraTotal = _blingClient.ExecuteGetOrder(filter);
                    // Filtra os pedidos para somente os do canal Íntegra
                    pedidosIntegra = pedidosIntegraTotal.Where(pedido => pedido.Pedido.TipoIntegracao.Equals("IntegraCommerce")).ToList();
                }
                catch (BlingException e)
                {
                    Log.Error($"Não foi possível recuperar os pedidos do Íntegra no Bling - {e.Message}");
                    pedidosIntegra = new List<PedidoItem>();
                }

                // Junta as listas de pedidos
                pedidos.AddRange(pedidosIntegra);
            }

            return pedidos;
        }
    }
}
