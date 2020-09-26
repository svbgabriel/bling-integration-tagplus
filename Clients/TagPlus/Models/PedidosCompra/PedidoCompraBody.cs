using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlingIntegrationTagplus.Clients.TagPlus.Models.PedidosCompra
{

    public class Item
    {

        [JsonProperty("item")]
        public int NumItem { get; set; }

        [JsonProperty("produto_servico")]
        public int ProdutoServico { get; set; }

        [JsonProperty("qtd")]
        public int Qtd { get; set; }

        [JsonProperty("valor_unitario")]
        public float? ValorUnitario { get; set; }

        [JsonProperty("valor_desconto")]
        public float? ValorDesconto { get; set; }

        [JsonProperty("detalhes")]
        public string Detalhes { get; set; }

        [JsonProperty("valor_venda")]
        public float? ValorVenda { get; set; }
    }

    public class Parcela
    {

        [JsonProperty("documento")]
        public string Documento { get; set; }

        [JsonProperty("valor_parcela")]
        public float? ValorParcela { get; set; }

        [JsonProperty("data_vencimento")]
        public string DataVencimento { get; set; }
    }

    public class FaturaCompra
    {

        [JsonProperty("forma_pagamento")]
        public int FormaPagamento { get; set; }

        [JsonProperty("parcelas")]
        public IList<Parcela> Parcelas { get; set; }
    }

    public class PedidoCompraBody
    {

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data_entrega")]
        public string DataEntrega { get; set; }

        [JsonProperty("hora_entrega")]
        public string HoraEntrega { get; set; }

        [JsonProperty("fornecedor")]
        public int? Fornecedor { get; set; }

        [JsonProperty("itens")]
        public IList<Item> Itens { get; set; }

        [JsonProperty("faturas")]
        public IList<FaturaCompra> Faturas { get; set; }

        [JsonProperty("valor_frete")]
        public float? ValorFrete { get; set; }

        [JsonProperty("valor_desconto")]
        public float? ValorDesconto { get; set; }

        [JsonProperty("observacoes")]
        public string Observacoes { get; set; }
    }

}
