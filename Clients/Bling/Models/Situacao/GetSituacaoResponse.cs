using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlingIntegrationTagplus.Clients.Bling.Models.Situacao
{
    public class Situacao
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("idHerdado")]
        public string IdHerdado { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("cor")]
        public string Cor { get; set; }
    }

    public class Situaco
    {

        [JsonProperty("situacao")]
        public Situacao Situacao { get; set; }
    }

    public class Retorno
    {

        [JsonProperty("situacoes")]
        public IList<Situaco> Situacoes { get; set; }
    }

    public class GetSituacaoResponse
    {

        [JsonProperty("retorno")]
        public Retorno Retorno { get; set; }
    }

}
