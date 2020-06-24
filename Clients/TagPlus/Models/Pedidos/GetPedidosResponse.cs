using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlingIntegrationTagplus.Clients.TagPlus.Models.Pedidos
{
    public class Departamento
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }
    }

    public class Cliente
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("razao_social")]
        public string RazaoSocial { get; set; }

        [JsonProperty("cpf")]
        public object Cpf { get; set; }

        [JsonProperty("cnpj")]
        public object Cnpj { get; set; }
    }

    public class Funcionario
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }
    }

    public class Vendedor
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }
    }

    public class ProdutoServico
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

    public class Iten
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("item")]
        public int Item { get; set; }

        [JsonProperty("produto_servico")]
        public ProdutoServico ProdutoServico { get; set; }

        [JsonProperty("qtd")]
        public int Qtd { get; set; }

        [JsonProperty("valor_unitario")]
        public double ValorUnitario { get; set; }

        [JsonProperty("valor_desconto")]
        public int ValorDesconto { get; set; }

        [JsonProperty("detalhes")]
        public string Detalhes { get; set; }

        [JsonProperty("valor_subtotal")]
        public double ValorSubtotal { get; set; }
    }

    public class FormaPagamento
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
        public int NumParcela { get; set; }

        [JsonProperty("documento")]
        public string Documento { get; set; }

        [JsonProperty("valor_parcela")]
        public int ValorParcela { get; set; }

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
        public FormaPagamento FormaPagamento { get; set; }

        [JsonProperty("qtd_parcelas")]
        public int QtdParcelas { get; set; }

        [JsonProperty("valor_total_parcelas")]
        public int ValorTotalParcelas { get; set; }

        [JsonProperty("parcelas")]
        public IList<ParcelaResponse> Parcelas { get; set; }
    }

    public class GetPedidosResponse
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("codigo_externo")]
        public string CodigoExterno { get; set; }

        [JsonProperty("numero")]
        public int Numero { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data_criacao")]
        public string DataCriacao { get; set; }

        [JsonProperty("hora_criacao")]
        public object HoraCriacao { get; set; }

        [JsonProperty("data_alteracao")]
        public string DataAlteracao { get; set; }

        [JsonProperty("data_abertura")]
        public string DataAbertura { get; set; }

        [JsonProperty("data_entrega")]
        public string DataEntrega { get; set; }

        [JsonProperty("hora_entrega")]
        public string HoraEntrega { get; set; }

        [JsonProperty("data_confirmacao")]
        public object DataConfirmacao { get; set; }

        [JsonProperty("valor_frete")]
        public float ValorFrete { get; set; }

        [JsonProperty("valor_desconto")]
        public int ValorDesconto { get; set; }

        [JsonProperty("valor_acrescimo")]
        public int ValorAcrescimo { get; set; }

        [JsonProperty("valor_total")]
        public double ValorTotal { get; set; }

        [JsonProperty("observacoes")]
        public string Observacoes { get; set; }

        [JsonProperty("departamento")]
        public Departamento Departamento { get; set; }

        [JsonProperty("cliente")]
        public Cliente Cliente { get; set; }

        [JsonProperty("funcionario")]
        public Funcionario Funcionario { get; set; }

        [JsonProperty("vendedor")]
        public Vendedor Vendedor { get; set; }

        [JsonProperty("itens")]
        public IList<Iten> Itens { get; set; }

        [JsonProperty("faturas")]
        public IList<FaturaResponse> Faturas { get; set; }
    }
}
