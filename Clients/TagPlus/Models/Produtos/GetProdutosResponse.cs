using Newtonsoft.Json;

namespace BlingIntegrationTagplus.Clients.TagPlus.Models.Produtos
{
    public class GetProdutosResponse
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("valor_venda_varejo")]
        public int ValorVendaVarejo { get; set; }
    }

}
