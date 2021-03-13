using Newtonsoft.Json;

namespace BlingIntegrationTagplus.Clients.TagPlus.Models.Fornecedores
{
    public class GetFornecedoresResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("razao_social")]
        public string RazaoSocial { get; set; }
        [JsonProperty("nome_fantasia")]
        public string NomeFantasia { get; set; }
        [JsonProperty("cpf")]
        public string CPF { get; set; }
        [JsonProperty("cnpj")]
        public string CNPJ { get; set; }
    }
}
