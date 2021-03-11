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
        private readonly BlingClient blingClient;
        private readonly Config config;

        public BlingPedidoService(BlingClient blingClient, Config config)
        {
            this.blingClient = blingClient;
            this.config = config;
        }

        public List<PedidoItem> GetPedidos(Dictionary<string, string> situacoes, DateTime initialDate)
        {
            situacoes.TryGetValue("ABERTO", out string situacaoEmAberto);
            situacoes.TryGetValue("ANDAMENTO", out string situacaoEmAndamento);

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
                return new List<PedidoItem>();
            }

            if (string.IsNullOrWhiteSpace(config.BlingOrderNum))
            {
                // Contorno para pedidos do Íntegra
                List<PedidoItem> pedidosIntegra = null;
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
                    pedidosIntegra = new List<PedidoItem>();
                }

                // Junta as listas de pedidos
                pedidos.AddRange(pedidosIntegra);
            }

            return pedidos;
        }
    }
}
