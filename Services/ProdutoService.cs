using BlingIntegrationTagplus.Clients.Bling.Models.Pedidos;
using BlingIntegrationTagplus.Clients.TagPlus;
using BlingIntegrationTagplus.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace BlingIntegrationTagplus.Services
{
    class ProdutoService
    {
        private readonly TagPlusClient tagPlusClient;

        public ProdutoService(TagPlusClient tagPlusClient)
        {
            this.tagPlusClient = tagPlusClient;
        }

        public List<Clients.TagPlus.Models.Pedidos.Item> GetListaProdutos(PedidoItem pedido)
        {
            List<Clients.TagPlus.Models.Pedidos.Item> itens = new List<Clients.TagPlus.Models.Pedidos.Item>();
            for (int i = 0; i < pedido.Pedido.Itens.Count; i++)
            {
                Clients.Bling.Models.Pedidos.Item blingItem = pedido.Pedido.Itens[i].Item;
                int produtoServico;
                try
                {
                    produtoServico = tagPlusClient.GetProduto(blingItem.Codigo);
                }
                catch (TagPlusException e)
                {
                    throw new ProdutoException($"Erro durante a busca pelo produto com o código: {blingItem.Codigo}. Erro: {e.Message}");
                }
                if (produtoServico == 0)
                {
                    throw new ProdutoException($"O produto de código: {blingItem.Codigo} não foi encontrado no TagPlus");
                }

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

            return itens;
        }
    }
}
