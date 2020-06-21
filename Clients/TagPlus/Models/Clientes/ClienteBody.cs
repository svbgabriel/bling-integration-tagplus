using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlingIntegrationTagplus.Clients.TagPlus.Models.Clientes
{
    public class Contato
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("tipo_contato")]
        public int TipoContato { get; set; }

        [JsonProperty("tipo_cadastro")]
        public int TipoCadastro { get; set; }

        [JsonProperty("detalhes")]
        public string Detalhes { get; set; }

        [JsonProperty("principal")]
        public bool Principal { get; set; }
    }

    public class Endereco
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("principal")]
        public bool Principal { get; set; }

        [JsonProperty("cep")]
        public string Cep { get; set; }

        [JsonProperty("logradouro")]
        public string Logradouro { get; set; }

        [JsonProperty("numero")]
        public string Numero { get; set; }

        [JsonProperty("complemento")]
        public string Complemento { get; set; }

        [JsonProperty("bairro")]
        public string Bairro { get; set; }

        [JsonProperty("pais")]
        public int Pais { get; set; }

        [JsonProperty("informacoes_adicionais")]
        public string InformacoesAdicionais { get; set; }

        [JsonProperty("tipo_cadastro")]
        public int TipoCadastro { get; set; }
    }

    public class Lancamento
    {

        [JsonProperty("id_financeiro")]
        public int IdFinanceiro { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("vencimento")]
        public string Vencimento { get; set; }

        [JsonProperty("valor")]
        public int Valor { get; set; }

        [JsonProperty("valor_com_juros")]
        public int ValorComJuros { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("telefone")]
        public string Telefone { get; set; }
    }

    public class NaoConfirmadoVencido
    {

        [JsonProperty("lancamentos")]
        public IList<Lancamento> Lancamentos { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("total_com_juros")]
        public int TotalComJuros { get; set; }
    }

    public class NaoConfirmadoNaoVencido
    {

        [JsonProperty("lancamentos")]
        public IList<Lancamento> Lancamentos { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("total_com_juros")]
        public int TotalComJuros { get; set; }
    }

    public class Confirmado
    {

        [JsonProperty("lancamentos")]
        public IList<Lancamento> Lancamentos { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("total_com_juros")]
        public int TotalComJuros { get; set; }
    }

    public class SaldoDevedor
    {

        [JsonProperty("nao_confirmado_vencido")]
        public NaoConfirmadoVencido NaoConfirmadoVencido { get; set; }

        [JsonProperty("nao_confirmado_nao_vencido")]
        public NaoConfirmadoNaoVencido NaoConfirmadoNaoVencido { get; set; }

        [JsonProperty("confirmado")]
        public Confirmado Confirmado { get; set; }
    }

    public class Extras
    {
    }

    public class ClienteBody
    {

        [JsonProperty("codigo_externo")]
        public string CodigoExterno { get; set; }

        [JsonProperty("id_entidade")]
        public int IdEntidade { get; set; }

        [JsonProperty("ativo")]
        public bool Ativo { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("tipo")]
        public string Tipo { get; set; }

        [JsonProperty("exterior")]
        public bool Exterior { get; set; }

        [JsonProperty("cpf")]
        public string Cpf { get; set; }

        [JsonProperty("cnpj")]
        public string Cnpj { get; set; }

        [JsonProperty("razao_social")]
        public string RazaoSocial { get; set; }

        [JsonProperty("nome_fantasia")]
        public string NomeFantasia { get; set; }

        [JsonProperty("ie")]
        public string Ie { get; set; }

        [JsonProperty("im")]
        public string Im { get; set; }

        [JsonProperty("responsavel")]
        public string Responsavel { get; set; }

        [JsonProperty("rg")]
        public string Rg { get; set; }

        [JsonProperty("data_nascimento")]
        public string DataNascimento { get; set; }

        [JsonProperty("sexo")]
        public string Sexo { get; set; }

        [JsonProperty("profissao")]
        public string Profissao { get; set; }

        [JsonProperty("filiacao_mae")]
        public string FiliacaoMae { get; set; }

        [JsonProperty("filiacao_pai")]
        public string FiliacaoPai { get; set; }

        [JsonProperty("estado_civil")]
        public string EstadoCivil { get; set; }

        [JsonProperty("renda_mensal")]
        public int RendaMensal { get; set; }

        [JsonProperty("recebe_email")]
        public bool RecebeEmail { get; set; }

        [JsonProperty("limite_credito")]
        public int LimiteCredito { get; set; }

        [JsonProperty("informacao_adicional")]
        public string InformacaoAdicional { get; set; }

        [JsonProperty("contatos")]
        public IList<Contato> Contatos { get; set; }

        [JsonProperty("enderecos")]
        public IList<Endereco> Enderecos { get; set; }

        [JsonProperty("vendedores")]
        public IList<int> Vendedores { get; set; }

        [JsonProperty("saldo_devedor")]
        public SaldoDevedor SaldoDevedor { get; set; }

        [JsonProperty("extras")]
        public Extras Extras { get; set; }
    }
}
