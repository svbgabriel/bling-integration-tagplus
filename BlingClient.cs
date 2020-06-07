using BlingIntegrationTagplus.Models;
using RestSharp;
using System;
using System.Net;

namespace BlingIntegrationTagplus
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
            var response = client.Get<PedidosResponse>(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            else
            {
                return response.Data;
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
            var response = client.Get<PedidosResponse>(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            else
            {
                return response.Data;
            }
        }
    }
}
