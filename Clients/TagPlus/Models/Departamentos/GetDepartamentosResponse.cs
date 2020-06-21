using Newtonsoft.Json;

namespace BlingIntegrationTagplus.Clients.TagPlus.Models.Departamentos
{
    public class GetDepartamentosResponse
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }
    }
}
