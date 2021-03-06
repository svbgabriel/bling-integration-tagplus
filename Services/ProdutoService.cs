using BlingIntegrationTagplus.Clients.Bling.Models.Pedidos;
using BlingIntegrationTagplus.Clients.TagPlus;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Fornecedores;
using BlingIntegrationTagplus.Clients.TagPlus.Models.PedidosCompra;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Produtos;
using BlingIntegrationTagplus.Exceptions;
using BlingIntegrationTagplus.Models;
using BlingIntegrationTagplus.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BlingIntegrationTagplus.Services
{
    class ProdutoService
    {
        private readonly TagPlusClient _tagPlusClient;

        public ProdutoService(TagPlusClient tagPlusClient)
        {
            _tagPlusClient = tagPlusClient;
        }

        public List<Produto> GetListaProdutos(PedidoItem pedido)
        {
            var itens = new List<Produto>();
            for (var i = 0; i < pedido.Pedido.Itens.Count; i++)
            {
                var blingItem = pedido.Pedido.Itens[i].Item;
                GetProdutosResponse produtoServico;
                try
                {
                    produtoServico = _tagPlusClient.GetProduto(blingItem.Codigo);
                }
                catch (TagPlusException e)
                {
                    throw new ProdutoException($"Erro durante a busca pelo produto com o código: {blingItem.Codigo}. Erro: {e.Message}");
                }
                if (produtoServico == null)
                {
                    throw new ProdutoException($"O produto de código: {blingItem.Codigo} não foi encontrado no TagPlus");
                }

                // Tenta extrair o distribuidor
                string fornecedor;
                try
                {
                    fornecedor = StringUtils.ExtractBetweenParentheses(produtoServico.Descricao);
                }
                catch (Exception e)
                {
                    throw new ProdutoException($"Não foi possível extrair o distribuidor de: {produtoServico.Descricao}. Erro: {e.Message}");
                }

                var produto = new Produto
                {
                    NumItem = i,
                    Id = produtoServico.Id,
                    Nome = produtoServico.Descricao,
                    Fornecedor = fornecedor,
                    Qtd = Convert.ToInt32(float.Parse(blingItem.Quantidade, CultureInfo.InvariantCulture.NumberFormat)),
                    ValorUnitario = float.Parse(blingItem.Valorunidade, CultureInfo.InvariantCulture.NumberFormat),
                    ValorDesconto = float.Parse(blingItem.DescontoItem, CultureInfo.InvariantCulture.NumberFormat)
                };
                itens.Add(produto);
            }

            return itens;
        }

        public List<Clients.TagPlus.Models.Pedidos.Item> GetListaPedidos(IEnumerable<Produto> produtos)
        {
            var itens = new List<Clients.TagPlus.Models.Pedidos.Item>();
            foreach (var produto in produtos)
            {
                var tagPlusItem = new Clients.TagPlus.Models.Pedidos.Item
                {
                    NumItem = produto.NumItem,
                    ProdutoServico = produto.Id,
                    Qtd = produto.Qtd,
                    ValorUnitario = produto.ValorUnitario,
                    ValorDesconto = produto.ValorDesconto
                };
                itens.Add(tagPlusItem);
            }

            return itens;
        }

        public IEnumerable<PedidoCompraBody> GetListaPedidosCompra(List<Produto> produtos, int numero, IList<GetFornecedoresResponse> fornecedores, Dictionary<string, string> dicFornecedores)
        {
            IList<PedidoCompraBody> itensCompra = new List<PedidoCompraBody>();
            // Separa por fornecedor
            var produtosPorFornecedor = produtos.GroupBy(p => p.Fornecedor);
            foreach (var produtosLista in produtosPorFornecedor)
            {

                // Busca o fornecedor
                var key = produtosLista.Key;
                dicFornecedores.TryGetValue(key, out var cnpjFornecedor);
                var idFornecedor = 0;
                foreach (var fornecedor in fornecedores)
                {
                    if (fornecedor.CNPJ != null && fornecedor.CNPJ.Equals(cnpjFornecedor))
                    {
                        idFornecedor = fornecedor.Id;
                        break;
                    }
                }

                // Prepara a lista
                var itens = new List<Clients.TagPlus.Models.PedidosCompra.Item>();
                foreach (var produto in produtos)
                {
                    // Pedido de Compra
                    var tagPlusItemCompra = new Clients.TagPlus.Models.PedidosCompra.Item
                    {
                        NumItem = produto.NumItem,
                        ProdutoServico = produto.Id,
                        Qtd = produto.Qtd,
                        ValorUnitario = produto.ValorUnitario,
                        ValorDesconto = produto.ValorDesconto
                    };
                    itens.Add(tagPlusItemCompra);
                }

                var pedidoCompraBody = new PedidoCompraBody
                {
                    Itens = itens,
                    Fornecedor = idFornecedor,
                    Observacoes = $"Número do Pedido Tagplus: {numero}\nEntrega até: {DateTime.Now.AddDays(7)}"
                };

                itensCompra.Add(pedidoCompraBody);

            }

            return itensCompra;
        }
    }
}
