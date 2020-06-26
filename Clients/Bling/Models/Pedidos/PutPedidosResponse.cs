using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlingIntegrationTagplus.Clients.Bling.Models.Pedidos
{
    public class PutPedidoItem
    {

        [JsonProperty("numero")]
        public string Numero { get; set; }

        [JsonProperty("mensagem")]
        public string Mensagem { get; set; }
    }

    public class PutPedido
    {

        [JsonProperty("pedido")]
        public PutPedidoItem Pedido { get; set; }
    }

    public class PutRetorno
    {

        [JsonProperty("pedidos")]
        public IList<PutPedido> Pedidos { get; set; }
    }

    public class PutPedidosResponse
    {

        [JsonProperty("retorno")]
        public PutRetorno Retorno { get; set; }
    }
}
