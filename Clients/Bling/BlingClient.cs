using BlingIntegrationTagplus.Clients.Bling.Models.Pedidos;
using BlingIntegrationTagplus.Clients.Bling.Models.Situacao;
using BlingIntegrationTagplus.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;

namespace BlingIntegrationTagplus.Clients.Bling
{
    class BlingClient
    {
        private string ApiKey { get; set; }

        public BlingClient(string apiKey)
        {
            ApiKey = apiKey;
        }

        public GetPedidosResponse ExecuteGetOrder()
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
                var pedidos = JsonConvert.DeserializeObject<GetPedidosResponse>(response.Content);
                return pedidos;
            }
        }

        public GetPedidosResponse ExecuteGetOrder(DateTime dateStart, DateTime dateEnd)
        {
            // Formata a data
            string dateStartString = dateStart.ToString("dd/MM/yyyy");
            string dateEndString = dateEnd.ToString("dd/MM/yyyy");
            var client = new RestClient("https://bling.com.br");
            var request = new RestRequest("Api/v2/pedidos/json", DataFormat.Json);
            request.AddQueryParameter("apikey", ApiKey);
            request.AddQueryParameter("filters", $"dataEmissao[{dateStartString} TO {dateEndString}]");
            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<PedidosResponseError>(response.Content);
                throw new BlingException($"Código {error.Retorno.Erros.Erro.Cod} : {error.Retorno.Erros.Erro.Msg}");
            }
            else
            {
                var pedidos = JsonConvert.DeserializeObject<GetPedidosResponse>(response.Content);
                return pedidos;
            }
        }

        public GetSituacaoResponse ExecuteGetSituacao()
        {
            var client = new RestClient("https://bling.com.br");
            var request = new RestRequest("Api/v2/situacao/Vendas/json", DataFormat.Json);
            request.AddQueryParameter("apikey", ApiKey);
            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<PedidosResponseError>(response.Content);
                throw new BlingException($"Código {error.Retorno.Erros.Erro.Cod} : {error.Retorno.Erros.Erro.Msg}");
            }
            else
            {
                var pedido = JsonConvert.DeserializeObject<GetSituacaoResponse>(response.Content);
                return pedido;
            }
        }

        public PutPedidosResponse ExecuteUpdateOrder(string numero, string situacao)
        {
            string xml = $"<?xml version=\"1.0\" encoding=\"UTF - 8\"?><pedido><idSituacao>{situacao}</idSituacao></pedido>";
            var client = new RestClient("https://bling.com.br");
            var request = new RestRequest($"Api/v2/pedidos/{numero}/json");
            request.AddQueryParameter("apikey", ApiKey);
            request.AddQueryParameter("xml", xml);
            var response = client.Put(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<PedidosResponseError>(response.Content);
                throw new BlingException($"Código {error.Retorno.Erros.Erro.Cod} : {error.Retorno.Erros.Erro.Msg}");
            }
            else
            {
                var pedido = JsonConvert.DeserializeObject<PutPedidosResponse>(response.Content);
                return pedido;
            }
        }
    }
}
