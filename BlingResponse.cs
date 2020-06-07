namespace BlingIntegrationTagplus.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class PedidosResponse
    {
        [JsonProperty("retorno")]
        public Retorno Retorno { get; set; }
    }

    public partial class Retorno
    {
        [JsonProperty("pedidos")]
        public PedidoElement[] Pedidos { get; set; }
    }

    public partial class PedidoElement
    {
        [JsonProperty("pedido")]
        public PedidoPedido Pedido { get; set; }
    }

    public partial class PedidoPedido
    {
        [JsonProperty("desconto")]
        public string Desconto { get; set; }

        [JsonProperty("observacoes")]
        public string Observacoes { get; set; }

        [JsonProperty("observacaointerna")]
        public string Observacaointerna { get; set; }

        [JsonProperty("data")]
        public DateTimeOffset Data { get; set; }

        [JsonProperty("numero")]
        public long Numero { get; set; }

        [JsonProperty("numeroPedidoLoja")]
        public long NumeroPedidoLoja { get; set; }

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

        [JsonProperty("loja")]
        public long Loja { get; set; }

        [JsonProperty("dataPrevista")]
        public DateTimeOffset DataPrevista { get; set; }

        [JsonProperty("tipoIntegracao")]
        public string TipoIntegracao { get; set; }

        [JsonProperty("cliente")]
        public Cliente Cliente { get; set; }

        [JsonProperty("itens")]
        public Iten[] Itens { get; set; }

        [JsonProperty("parcelas")]
        public ParcelaElement[] Parcelas { get; set; }

        [JsonProperty("nota")]
        public Nota Nota { get; set; }

        [JsonProperty("transporte")]
        public Transporte Transporte { get; set; }
    }

    public partial class Cliente
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("cnpj")]
        public string Cnpj { get; set; }

        [JsonProperty("ie")]
        public string Ie { get; set; }

        [JsonProperty("rg")]
        public long Rg { get; set; }

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

    public partial class Iten
    {
        [JsonProperty("item")]
        public Dictionary<string, string> Item { get; set; }
    }

    public partial class Nota
    {
        [JsonProperty("serie")]
        public long Serie { get; set; }

        [JsonProperty("numero")]
        public string Numero { get; set; }

        [JsonProperty("dataEmissao")]
        public DateTimeOffset DataEmissao { get; set; }

        [JsonProperty("situacao")]
        public long Situacao { get; set; }

        [JsonProperty("chaveAcesso")]
        public string ChaveAcesso { get; set; }

        [JsonProperty("valorNota")]
        public string ValorNota { get; set; }
    }

    public partial class ParcelaElement
    {
        [JsonProperty("parcela")]
        public ParcelaParcela Parcela { get; set; }
    }

    public partial class ParcelaParcela
    {
        [JsonProperty("idLancamento")]
        public long IdLancamento { get; set; }

        [JsonProperty("valor")]
        public string Valor { get; set; }

        [JsonProperty("dataVencimento")]
        public DateTimeOffset DataVencimento { get; set; }

        [JsonProperty("obs")]
        public string Obs { get; set; }

        [JsonProperty("destino")]
        public long Destino { get; set; }

        [JsonProperty("forma_pagamento")]
        public FormaPagamento FormaPagamento { get; set; }
    }

    public partial class FormaPagamento
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("codigoFiscal")]
        public long CodigoFiscal { get; set; }
    }

    public partial class Transporte
    {
        [JsonProperty("transportadora")]
        public string Transportadora { get; set; }

        [JsonProperty("cnpj")]
        public string Cnpj { get; set; }

        [JsonProperty("tipo_frete")]
        public string TipoFrete { get; set; }

        [JsonProperty("volumes")]
        public VolumeElement[] Volumes { get; set; }

        [JsonProperty("enderecoEntrega")]
        public EnderecoEntrega EnderecoEntrega { get; set; }
    }

    public partial class EnderecoEntrega
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("endereco")]
        public string Endereco { get; set; }

        [JsonProperty("numero")]
        public long Numero { get; set; }

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

    public partial class VolumeElement
    {
        [JsonProperty("volume")]
        public VolumeVolume Volume { get; set; }
    }

    public partial class VolumeVolume
    {
        [JsonProperty("idServico")]
        public long IdServico { get; set; }

        [JsonProperty("servico")]
        public string Servico { get; set; }

        [JsonProperty("codigoRastreamento")]
        public string CodigoRastreamento { get; set; }

        [JsonProperty("dataSaida")]
        public DateTimeOffset DataSaida { get; set; }

        [JsonProperty("prazoEntregaPrevisto")]
        public long PrazoEntregaPrevisto { get; set; }

        [JsonProperty("valorFretePrevisto")]
        public string ValorFretePrevisto { get; set; }

        [JsonProperty("valorDeclarado")]
        public string ValorDeclarado { get; set; }

        [JsonProperty("remessa")]
        public Remessa Remessa { get; set; }

        [JsonProperty("dimensoes")]
        public Dimensoes Dimensoes { get; set; }
    }

    public partial class Dimensoes
    {
        [JsonProperty("peso")]
        public string Peso { get; set; }

        [JsonProperty("altura")]
        public long Altura { get; set; }

        [JsonProperty("largura")]
        public long Largura { get; set; }

        [JsonProperty("comprimento")]
        public long Comprimento { get; set; }

        [JsonProperty("diametro")]
        public long Diametro { get; set; }
    }

    public partial class Remessa
    {
        [JsonProperty("numero")]
        public string Numero { get; set; }

        [JsonProperty("dataCriacao")]
        public DateTimeOffset DataCriacao { get; set; }
    }
}
