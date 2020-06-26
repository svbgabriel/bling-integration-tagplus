using Newtonsoft.Json;

namespace BlingIntegrationTagplus.Clients.TagPlus.Models.TiposContatos
{
    public class GetTiposContatosResponse
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }
    }
}
