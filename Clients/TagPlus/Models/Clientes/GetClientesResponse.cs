using Newtonsoft.Json;

namespace BlingIntegrationTagplus.Clients.TagPlus.Models.Clientes
{
    public class GetClientesResponse
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("razao_social")]
        public string RazaoSocial { get; set; }

        [JsonProperty("nome_fantasia")]
        public object NomeFantasia { get; set; }

        [JsonProperty("cpf")]
        public object Cpf { get; set; }

        [JsonProperty("cnpj")]
        public object Cnpj { get; set; }
    }
}
