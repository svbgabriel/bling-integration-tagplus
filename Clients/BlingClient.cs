using BlingIntegrationTagplus.Exceptions;
using BlingIntegrationTagplus.Models.Bling;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;

namespace BlingIntegrationTagplus.Clients
{
    class BlingClient
    {
        private string ApiKey { get; set; }

        public BlingClient(string apiKey)
        {
            this.ApiKey = apiKey;
        }

        public PedidosResponse ExecuteGetOrder()
        {
            var client = new RestClient("https://bling.com.br");
            var request = new RestRequest("Api/v2/pedidos/json", DataFormat.Json);
            request.AddQueryParameter("apikey", ApiKey);
            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<PedidosResponseError>(response.Content);
                throw new BlingException($"Código {error.Retorno.Erros.Erro.Cod} : {error.Retorno.Erros.Erro.Msg}");
            }
            else
            {
                var pedidos = JsonConvert.DeserializeObject<PedidosResponse>(response.Content);
                return pedidos;
            }
        }

        public PedidosResponse ExecuteGetOrder(DateTime dateStart, DateTime dateEnd)
        {
            // Formata a data
            string dateStartString = $"{dateStart.Day}/{dateStart.Month}/{dateStart.Year}";
            string dateEndString = $"{dateEnd.Day}/{dateEnd.Month}/{dateEnd.Year}";
            var client = new RestClient("https://bling.com.br");
            var request = new RestRequest("Api/v2/pedidos/json", DataFormat.Json);
            request.AddQueryParameter("apikey", ApiKey);
            request.AddQueryParameter("filters", $"dataEmissao[{dateStartString} TO {dateEndString}];");
            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<PedidosResponseError>(response.Content);
                throw new BlingException($"Código {error.Retorno.Erros.Erro.Cod} : {error.Retorno.Erros.Erro.Msg}");
            }
            else
            {
                var pedidos = JsonConvert.DeserializeObject<PedidosResponse>(response.Content);
                return pedidos;
            }
        }
    }
}
