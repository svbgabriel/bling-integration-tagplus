using Newtonsoft.Json;

namespace BlingIntegrationTagplus.Models.Bling
{
    public partial class Erro
    {

        [JsonProperty("cod")]
        public int Cod { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }
    }

    public partial class Erros
    {

        [JsonProperty("erro")]
        public Erro Erro { get; set; }
    }

    public partial class RetornoError
    {

        [JsonProperty("erros")]
        public Erros Erros { get; set; }
    }

    public partial class PedidosResponseError
    {

        [JsonProperty("retorno")]
        public RetornoError Retorno { get; set; }
    }
}
