using Newtonsoft.Json;

namespace BlingIntegrationTagplus.Clients.TagPlus.Models.FormasPagamento
{
    public class GetFormasPagamentoResponse
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("ativo")]
        public int Ativo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("picpay_token")]
        public object PicpayToken { get; set; }
    }
}
