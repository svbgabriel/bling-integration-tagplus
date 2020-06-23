using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlingIntegrationTagplus.Clients.TagPlus.Models
{
    public class Error
    {

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("dataPath")]
        public string DataPath { get; set; }

        [JsonProperty("schemaKey")]
        public string SchemaKey { get; set; }

        [JsonProperty("subResults")]
        public object SubResults { get; set; }
    }


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

        [JsonProperty("errors")]
        public IList<Error> Errors { get; set; }
    }
}
