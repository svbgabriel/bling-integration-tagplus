namespace BlingIntegrationTagplus.Clients.Bling.Models.Pedidos
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public partial class Cliente
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("cnpj")]
        public string Cnpj { get; set; }

        [JsonProperty("ie")]
        public string Ie { get; set; }

        [JsonProperty("rg")]
        public string Rg { get; set; }

        [JsonProperty("endereco")]
        public string Endereco { get; set; }

        [JsonProperty("numero")]
        public string Numero { get; set; }

        [JsonProperty("complemento")]
        public string Complemento { get; set; }

        [JsonProperty("cidade")]
        public string Cidade { get; set; }

        [JsonProperty("bairro")]
        public string Bairro { get; set; }

        [JsonProperty("cep")]
        public string Cep { get; set; }

        [JsonProperty("uf")]
        public string Uf { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("celular")]
        public string Celular { get; set; }

        [JsonProperty("fone")]
        public string Fone { get; set; }
    }

    public partial class Item
    {

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("quantidade")]
        public string Quantidade { get; set; }

        [JsonProperty("valorunidade")]
        public string Valorunidade { get; set; }

        [JsonProperty("precocusto")]
        public string Precocusto { get; set; }

        [JsonProperty("descontoItem")]
        public string DescontoItem { get; set; }

        [JsonProperty("un")]
        public string Un { get; set; }

        [JsonProperty("pesoBruto")]
        public string PesoBruto { get; set; }

        [JsonProperty("largura")]
        public string Largura { get; set; }

        [JsonProperty("altura")]
        public string Altura { get; set; }

        [JsonProperty("profundidade")]
        public string Profundidade { get; set; }

        [JsonProperty("descricaoDetalhada")]
        public string DescricaoDetalhada { get; set; }

        [JsonProperty("unidadeMedida")]
        public string UnidadeMedida { get; set; }

        [JsonProperty("gtin")]
        public string Gtin { get; set; }
    }

    public partial class Iten
    {

        [JsonProperty("item")]
        public Item Item { get; set; }
    }

    public partial class Pagamento
    {

        [JsonProperty("categoria")]
        public string Categoria { get; set; }
    }

    public partial class FormaPagamento
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("codigoFiscal")]
        public int CodigoFiscal { get; set; }
    }

    public partial class Parcela
    {

        [JsonProperty("idLancamento")]
        public int IdLancamento { get; set; }

        [JsonProperty("valor")]
        public string Valor { get; set; }

        [JsonProperty("dataVencimento")]
        public string DataVencimento { get; set; }

        [JsonProperty("obs")]
        public string Obs { get; set; }

        [JsonProperty("destino")]
        public int Destino { get; set; }

        [JsonProperty("forma_pagamento")]
        public FormaPagamento FormaPagamento { get; set; }
    }

    public partial class ParcelaItem
    {

        [JsonProperty("parcela")]
        public Parcela Parcela { get; set; }
    }

    public partial class EnderecoEntrega
    {

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("endereco")]
        public string Endereco { get; set; }

        [JsonProperty("numero")]
        public string Numero { get; set; }

        [JsonProperty("complemento")]
        public string Complemento { get; set; }

        [JsonProperty("cidade")]
        public string Cidade { get; set; }

        [JsonProperty("bairro")]
        public string Bairro { get; set; }

        [JsonProperty("cep")]
        public string Cep { get; set; }

        [JsonProperty("uf")]
        public string Uf { get; set; }
    }

    public partial class Transporte
    {

        [JsonProperty("transportadora")]
        public string Transportadora { get; set; }

        [JsonProperty("cnpj")]
        public string Cnpj { get; set; }

        [JsonProperty("tipo_frete")]
        public string TipoFrete { get; set; }

        [JsonProperty("qtde_volumes")]
        public string QtdeVolumes { get; set; }

        [JsonProperty("enderecoEntrega")]
        public EnderecoEntrega EnderecoEntrega { get; set; }
    }

    public partial class Pedido
    {

        [JsonProperty("desconto")]
        public string Desconto { get; set; }

        [JsonProperty("observacoes")]
        public string Observacoes { get; set; }

        [JsonProperty("observacaointerna")]
        public string Observacaointerna { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("numero")]
        public string Numero { get; set; }

        [JsonProperty("numeroOrdemCompra")]
        public string NumeroOrdemCompra { get; set; }

        [JsonProperty("vendedor")]
        public string Vendedor { get; set; }

        [JsonProperty("valorfrete")]
        public string Valorfrete { get; set; }

        [JsonProperty("totalprodutos")]
        public string Totalprodutos { get; set; }

        [JsonProperty("totalvenda")]
        public string Totalvenda { get; set; }

        [JsonProperty("situacao")]
        public string Situacao { get; set; }

        [JsonProperty("dataSaida")]
        public string DataSaida { get; set; }

        [JsonProperty("loja")]
        public string Loja { get; set; }

        [JsonProperty("numeroPedidoLoja")]
        public string NumeroPedidoLoja { get; set; }

        [JsonProperty("tipoIntegracao")]
        public string TipoIntegracao { get; set; }

        [JsonProperty("dataPrevista")]
        public string DataPrevista { get; set; }

        [JsonProperty("cliente")]
        public Cliente Cliente { get; set; }

        [JsonProperty("itens")]
        public IList<Iten> Itens { get; set; }

        [JsonProperty("pagamento")]
        public Pagamento Pagamento { get; set; }

        [JsonProperty("parcelas")]
        public IList<ParcelaItem> Parcelas { get; set; }

        [JsonProperty("transporte")]
        public Transporte Transporte { get; set; }
    }

    public partial class PedidoItem
    {

        [JsonProperty("pedido")]
        public Pedido Pedido { get; set; }
    }

    public partial class Retorno
    {

        [JsonProperty("pedidos")]
        public IList<PedidoItem> Pedidos { get; set; }
    }

    public partial class GetPedidosResponse
    {

        [JsonProperty("retorno")]
        public Retorno Retorno { get; set; }
    }


}
