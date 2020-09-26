using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlingIntegrationTagplus.Clients.TagPlus.Models.PedidosCompra
{
    public class FornecedorResponse
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("razao_social")]
        public string RazaoSocial { get; set; }

        [JsonProperty("cpf")]
        public string Cpf { get; set; }

        [JsonProperty("cnpj")]
        public string Cnpj { get; set; }
    }

    public class ProdutoServicoResponse
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("tipo")]
        public string Tipo { get; set; }
    }

    public class ItemResponse
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("item")]
        public int Item { get; set; }

        [JsonProperty("produto_servico")]
        public ProdutoServicoResponse ProdutoServico { get; set; }

        [JsonProperty("qtd")]
        public int Qtd { get; set; }

        [JsonProperty("valor_unitario")]
        public double ValorUnitario { get; set; }

        [JsonProperty("valor_desconto")]
        public double ValorDesconto { get; set; }

        [JsonProperty("detalhes")]
        public string Detalhes { get; set; }

        [JsonProperty("valor_subtotal")]
        public double ValorSubtotal { get; set; }
    }

    public class FormaPagamentoResponse
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }
    }

    public class ParcelaResponse
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("parcela")]
        public int Parcela { get; set; }

        [JsonProperty("documento")]
        public string Documento { get; set; }

        [JsonProperty("valor_parcela")]
        public double ValorParcela { get; set; }

        [JsonProperty("data_vencimento")]
        public string DataVencimento { get; set; }

        [JsonProperty("informacao_1")]
        public object Informacao1 { get; set; }

        [JsonProperty("informacao_2")]
        public object Informacao2 { get; set; }

        [JsonProperty("informacao_3")]
        public object Informacao3 { get; set; }

        [JsonProperty("lancamento_financeiro_vinculado")]
        public object LancamentoFinanceiroVinculado { get; set; }
    }

    public class FaturaResponse
    {

        [JsonProperty("item")]
        public int Item { get; set; }

        [JsonProperty("forma_pagamento")]
        public FormaPagamentoResponse FormaPagamento { get; set; }

        [JsonProperty("qtd_parcelas")]
        public int QtdParcelas { get; set; }

        [JsonProperty("valor_total_parcelas")]
        public double ValorTotalParcelas { get; set; }

        [JsonProperty("parcelas")]
        public IList<ParcelaResponse> Parcelas { get; set; }
    }

    public class GetPedidoCompraResponse
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("numero")]
        public int Numero { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data_criacao")]
        public string DataCriacao { get; set; }

        [JsonProperty("data_entrega")]
        public string DataEntrega { get; set; }

        [JsonProperty("hora_entrega")]
        public string HoraEntrega { get; set; }

        [JsonProperty("data_confirmacao")]
        public string DataConfirmacao { get; set; }

        [JsonProperty("valor_frete")]
        public double ValorFrete { get; set; }

        [JsonProperty("valor_desconto")]
        public double ValorDesconto { get; set; }

        [JsonProperty("valor_total")]
        public double ValorTotal { get; set; }

        [JsonProperty("observacoes")]
        public string Observacoes { get; set; }

        [JsonProperty("fornecedor")]
        public FornecedorResponse Fornecedor { get; set; }

        [JsonProperty("funcionario")]
        public string Funcionario { get; set; }

        [JsonProperty("itens")]
        public IList<ItemResponse> Itens { get; set; }

        [JsonProperty("faturas")]
        public IList<FaturaResponse> Faturas { get; set; }

        [JsonProperty("anexos")]
        public object Anexos { get; set; }
    }

}
