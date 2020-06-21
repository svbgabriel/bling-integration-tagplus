using Newtonsoft.Json;

namespace BlingIntegrationTagplus.Clients.TagPlus.Models
{
    public class TagPlusResponseError
    {
        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("dev_message")]
        public string DevMessage { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }
}
