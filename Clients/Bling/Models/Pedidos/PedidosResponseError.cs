using Newtonsoft.Json;

namespace BlingIntegrationTagplus.Clients.Bling.Models.Pedidos
{
    public class Erro
    {

        [JsonProperty("cod")]
        public int Cod { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }
    }

    public class Erros
    {

        [JsonProperty("erro")]
        public Erro Erro { get; set; }
    }

    public class RetornoError
    {

        [JsonProperty("erros")]
        public Erros Erros { get; set; }
    }

    public class PedidosResponseError
    {

        [JsonProperty("retorno")]
        public RetornoError Retorno { get; set; }
    }
}
