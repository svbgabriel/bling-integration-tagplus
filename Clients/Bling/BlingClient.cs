using BlingIntegrationTagplus.Clients.Bling.Models.Pedidos;
using BlingIntegrationTagplus.Clients.Bling.Models.Situacao;
using BlingIntegrationTagplus.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace BlingIntegrationTagplus.Clients.Bling
{
    class BlingClient
    {
        private string ApiKey { get; set; }
        private readonly int API_LIMIT = 100;

        public BlingClient(string apiKey)
        {
            ApiKey = apiKey;
        }

        public List<PedidoItem> ExecuteGetOrder()
        {
            int page = 1;
            bool hasNext = true;
            List<PedidoItem> pedidosResult = new List<PedidoItem>();
            var client = new RestClient("https://bling.com.br");
            while (hasNext)
            {
                var request = new RestRequest($"Api/v2/pedidos/page={page}/json", DataFormat.Json);
                request.AddQueryParameter("apikey", ApiKey);
                var response = client.Get(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var error = JsonConvert.DeserializeObject<PedidosResponseError>(response.Content);
                    throw new BlingException($"Código {error.Retorno.Erros.Erro.Cod} : {error.Retorno.Erros.Erro.Msg}");
                }
                else
                {
                    if (!response.Content.Contains("A informacao desejada nao foi encontrada"))
                    {
                        var pedidos = JsonConvert.DeserializeObject<GetPedidosResponse>(response.Content);
                        pedidosResult.AddRange(pedidos.Retorno.Pedidos);
                        page++;
                        if (pedidos.Retorno.Pedidos.Count < API_LIMIT)
                        {
                            hasNext = false;
                        }
                    }
                    else
                    {
                        hasNext = false;
                    }
                }
            }

            return pedidosResult;
        }

        public List<PedidoItem> ExecuteGetOrder(string filters)
        {
            int page = 1;
            bool hasNext = true;
            List<PedidoItem> pedidosResult = new List<PedidoItem>();
            var client = new RestClient("https://bling.com.br");
            while (hasNext)
            {
                var request = new RestRequest($"Api/v2/pedidos/page={page}/json", DataFormat.Json);
                request.AddQueryParameter("apikey", ApiKey);
                request.AddQueryParameter("filters", filters);
                var response = client.Get(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var error = JsonConvert.DeserializeObject<PedidosResponseError>(response.Content);
                    throw new BlingException($"Código {error.Retorno.Erros.Erro.Cod} : {error.Retorno.Erros.Erro.Msg}");
                }
                else
                {
                    if (!response.Content.Contains("A informacao desejada nao foi encontrada"))
                    {
                        var pedidos = JsonConvert.DeserializeObject<GetPedidosResponse>(response.Content);
                        pedidosResult.AddRange(pedidos.Retorno.Pedidos);
                        page++;
                        if (pedidos.Retorno.Pedidos.Count < API_LIMIT)
                        {
                            hasNext = false;
                        }
                    }
                    else
                    {
                        hasNext = false;
                    }
                }
            }

            return pedidosResult;
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
            string xmlTemplate = $"<?xml version=\"1.0\" encoding=\"UTF-8\"?><pedido><idSituacao>{situacao}</idSituacao></pedido>";
            var client = new RestClient("https://bling.com.br");
            var request = new RestRequest($"Api/v2/pedido/{numero}/json");
            request.AddHeader("Content-Type", "x-www-form-urlencoded");
            request.AddParameter("apikey", ApiKey);
            request.AddParameter("xml", xmlTemplate);
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
