using BlingIntegrationTagplus.Clients.TagPlus.Models;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Clientes;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Departamentos;
using BlingIntegrationTagplus.Clients.TagPlus.Models.FormasPagamento;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Pedidos;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Produtos;
using BlingIntegrationTagplus.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace BlingIntegrationTagplus.Clients.TagPlus
{
    class TagPlusClient
    {
        private string AccessToken { get; set; }

        public TagPlusClient(string accessToken)
        {
            AccessToken = accessToken;
        }

        public GetPedidosResponse PostPedidos(PedidoBody body)
        {
            var client = new RestClient("https://api.tagplus.com.br");
            var request = new RestRequest("pedidos", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("Authorization", $"Bearer {AccessToken}");
            request.AddJsonBody(body);
            var response = client.Post(request);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                var error = JsonConvert.DeserializeObject<TagPlusResponseError>(response.Content);
                throw new TagPlusException($"Código {error.ErrorCode} : {error.Message}");
            }
            else
            {
                var pedido = JsonConvert.DeserializeObject<GetPedidosResponse>(response.Content);
                return pedido;
            }
        }

        public IList<GetDepartamentosResponse> GetDepartamentos()
        {
            var client = new RestClient("https://api.tagplus.com.br");
            var request = new RestRequest("departamentos", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("Authorization", $"Bearer {AccessToken}");
            request.AddHeader("Accept", "application/json");
            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<TagPlusResponseError>(response.Content);
                throw new TagPlusException($"Código {error.ErrorCode} : {error.Message}");
            }
            else
            {
                var departamentos = JsonConvert.DeserializeObject<IList<GetDepartamentosResponse>>(response.Content);
                return departamentos;
            }
        }

        public int GetDepartamento(string nomeDepartamento)
        {
            var client = new RestClient("https://api.tagplus.com.br");
            var request = new RestRequest("departamentos", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("Authorization", $"Bearer {AccessToken}");
            request.AddQueryParameter("descricao", nomeDepartamento);
            request.AddHeader("Accept", "application/json");
            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<TagPlusResponseError>(response.Content);
                throw new TagPlusException($"Código {error.ErrorCode} : {error.Message}");
            }
            else
            {
                var departamentos = JsonConvert.DeserializeObject<IList<GetDepartamentosResponse>>(response.Content);
                // Caso não seja encontrado retorna 0
                if (departamentos.Count == 0)
                {
                    return 0;
                }
                int id = departamentos[0].Id;
                return id;
            }
        }

        public int GetCliente(string nome)
        {
            var client = new RestClient("https://api.tagplus.com.br");
            var request = new RestRequest("clientes", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("Authorization", $"Bearer {AccessToken}");
            request.AddQueryParameter("razao_social", nome);
            request.AddHeader("Accept", "application/json");
            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<TagPlusResponseError>(response.Content);
                throw new TagPlusException($"Código {error.ErrorCode} : {error.Message}");
            }
            else
            {
                var clientes = JsonConvert.DeserializeObject<IList<GetClientesResponse>>(response.Content);
                // Caso não seja encontrado retorna 0
                if (clientes.Count == 0)
                {
                    return 0;
                }
                int id = clientes[0].Id;
                return id;
            }
        }

        public int PostCliente(ClienteBody cliente)
        {
            var client = new RestClient("https://api.tagplus.com.br");
            var request = new RestRequest("clientes", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("Authorization", $"Bearer {AccessToken}");
            request.AddJsonBody(cliente);
            var response = client.Post(request);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                var error = JsonConvert.DeserializeObject<TagPlusResponseError>(response.Content);
                throw new TagPlusException($"Código {error.ErrorCode} : {error.Message}");
            }
            else
            {
                var clientes = JsonConvert.DeserializeObject<IList<GetClientesResponse>>(response.Content);
                // Caso não seja encontrado retorna 0
                if (clientes.Count == 0)
                {
                    return 0;
                }
                int id = clientes[0].Id;
                return id;
            }
        }

        public int GetProduto(string codigo)
        {
            var client = new RestClient("https://api.tagplus.com.br");
            var request = new RestRequest("produtos", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("Authorization", $"Bearer {AccessToken}");
            request.AddQueryParameter("codigo", codigo);
            request.AddHeader("Accept", "application/json");
            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<TagPlusResponseError>(response.Content);
                throw new TagPlusException($"Código {error.ErrorCode} : {error.Message}");
            }
            else
            {
                var produtos = JsonConvert.DeserializeObject<IList<GetProdutosResponse>>(response.Content);
                // Caso não seja encontrado retorna 0
                if (produtos.Count == 0)
                {
                    return 0;
                }
                int id = produtos[0].Id;
                return id;
            }
        }

        public int GetFormasPagamento(string descricao)
        {
            var client = new RestClient("https://api.tagplus.com.br");
            var request = new RestRequest("formas_pagamento", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("Authorization", $"Bearer {AccessToken}");
            request.AddQueryParameter("descricao", descricao);
            request.AddHeader("Accept", "application/json");
            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<TagPlusResponseError>(response.Content);
                throw new TagPlusException($"Código {error.ErrorCode} : {error.Message}");
            }
            else
            {
                var formas = JsonConvert.DeserializeObject<IList<GetFormasPagamentoResponse>>(response.Content);
                // Caso não seja encontrado retorna 0
                if (formas.Count == 0)
                {
                    return 0;
                }
                int id = formas[0].Id;
                return id;
            }
        }
    }
}
