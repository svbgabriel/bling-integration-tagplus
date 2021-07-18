using BlingIntegrationTagplus.Clients.TagPlus.Models;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Clientes;
using BlingIntegrationTagplus.Clients.TagPlus.Models.FormasPagamento;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Fornecedores;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Pedidos;
using BlingIntegrationTagplus.Clients.TagPlus.Models.PedidosCompra;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Produtos;
using BlingIntegrationTagplus.Clients.TagPlus.Models.TiposContatos;
using BlingIntegrationTagplus.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Serilog;
using System.Collections.Generic;
using System.Net;

namespace BlingIntegrationTagplus.Clients.TagPlus
{
    class TagPlusClient
    {
        private readonly string AccessToken;
        private readonly string ApiUrl;

        public TagPlusClient(string accessToken, string apiUrl)
        {
            AccessToken = accessToken;
            ApiUrl = apiUrl;
        }

        public GetPedidosResponse PostPedidos(PedidoBody body)
        {
            var client = new RestClient(ApiUrl);
            client.UseNewtonsoftJson();
            var request = new RestRequest("pedidos", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("apikey", AccessToken);
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(body);
            var response = client.Post(request);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                var error = JsonConvert.DeserializeObject<TagPlusResponseError>(response.Content);
                Log.Error("TagPlus - PostPedidos(PedidoBody body) - Erro durante a criação do pedido");
                Log.Error(JsonConvert.SerializeObject(body));
                Log.Error($"Código {error.ErrorCode} : {error.Message}");
                throw new TagPlusException($"Código {error.ErrorCode} : {error.Message}");
            }
            else
            {
                var pedido = JsonConvert.DeserializeObject<GetPedidosResponse>(response.Content);
                return pedido;
            }
        }

        public int GetClienteByRazaoSocial(string nome)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("clientes", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("apikey", AccessToken);
            request.AddQueryParameter("razao_social", nome);
            request.AddHeader("Accept", "application/json");
            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<TagPlusResponseError>(response.Content);
                Log.Error("TagPlus - GetClienteByRazaoSocial(string nome) - Erro durante a recuperação do cliente");
                Log.Error($"nome: {nome}");
                Log.Error($"Código {error.ErrorCode} : {error.Message}");
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

        public int GetClienteByCpf(string cpf)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("clientes", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("apikey", AccessToken);
            request.AddQueryParameter("cpf", cpf);
            request.AddHeader("Accept", "application/json");
            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<TagPlusResponseError>(response.Content);
                Log.Error("TagPlus - GetClienteByRazaoSocial(string cpf) - Erro durante a recuperação do cliente");
                Log.Error($"cpf: {cpf}");
                Log.Error($"Código {error.ErrorCode} : {error.Message}");
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

        public int GetClienteByCnpj(string cnpj)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("clientes", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("apikey", AccessToken);
            request.AddQueryParameter("cnpj", cnpj);
            request.AddHeader("Accept", "application/json");
            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<TagPlusResponseError>(response.Content);
                Log.Error("TagPlus - GetClienteByRazaoSocial(string cnpj) - Erro durante a recuperação do cliente");
                Log.Error($"cnpj: {cnpj}");
                Log.Error($"Código {error.ErrorCode} : {error.Message}");
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

        public int PostCliente(ClienteBody clienteBody)
        {
            var client = new RestClient(ApiUrl);
            client.UseNewtonsoftJson();
            var request = new RestRequest("clientes", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("apikey", AccessToken);
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(clienteBody);
            var response = client.Post(request);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                var error = JsonConvert.DeserializeObject<TagPlusResponseError>(response.Content);
                Log.Error("TagPlus - PostCliente(ClienteBody clienteBody) - Erro durante a criação do cliente");
                Log.Error(JsonConvert.SerializeObject(clienteBody));
                Log.Error($"Código {error.ErrorCode} : {error.Message}");
                throw new TagPlusException($"Código {error.ErrorCode} : {error.Message}");
            }
            else
            {
                var cliente = JsonConvert.DeserializeObject<GetClientesResponse>(response.Content);
                return cliente.Id;
            }
        }

        public GetProdutosResponse GetProduto(string codigo)
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("produtos", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("apikey", AccessToken);
            request.AddQueryParameter("codigo", codigo);
            request.AddHeader("Accept", "application/json");
            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<TagPlusResponseError>(response.Content);
                Log.Error("TagPlus - GetProduto(string codigo) - Erro durante a recuperação do produto");
                Log.Error($"codigo: {codigo}");
                Log.Error($"Código {error.ErrorCode} : {error.Message}");
                throw new TagPlusException($"Código {error.ErrorCode} : {error.Message}");
            }
            else
            {
                var produtos = JsonConvert.DeserializeObject<IList<GetProdutosResponse>>(response.Content);
                // Caso não seja encontrado retorna null
                if (produtos.Count == 0)
                {
                    return null;
                }

                return produtos[0];
            }
        }

        public IList<GetFormasPagamentoResponse> GetFormasPagamentos()
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("formas_pagamento", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("apikey", AccessToken);
            request.AddHeader("Accept", "application/json");
            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<TagPlusResponseError>(response.Content);
                Log.Error("TagPlus - GetFormasPagamentos() - Erro durante a recuperação das formas de pagamento");
                Log.Error($"Código {error.ErrorCode} : {error.Message}");
                throw new TagPlusException($"Código {error.ErrorCode} : {error.Message}");
            }
            else
            {
                var formas = JsonConvert.DeserializeObject<IList<GetFormasPagamentoResponse>>(response.Content);
                return formas;
            }
        }

        public IList<GetTiposContatosResponse> GetTiposContatos()
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("tipos_contatos", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("apikey", AccessToken);
            request.AddHeader("Accept", "application/json");
            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<TagPlusResponseError>(response.Content);
                Log.Error("TagPlus - GetTiposContatos() - Erro durante a recuperação das tipos de contato");
                Log.Error($"Código {error.ErrorCode} : {error.Message}");
                throw new TagPlusException($"Código {error.ErrorCode} : {error.Message}");
            }
            else
            {
                var tiposContatos = JsonConvert.DeserializeObject<IList<GetTiposContatosResponse>>(response.Content);
                return tiposContatos;
            }
        }

        public GetPedidoCompraResponse PostPedidoCompra(PedidoCompraBody body)
        {
            var client = new RestClient(ApiUrl);
            client.UseNewtonsoftJson();
            var request = new RestRequest("pedidos_compra", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("apikey", AccessToken);
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(body);
            var response = client.Post(request);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                var error = JsonConvert.DeserializeObject<TagPlusResponseError>(response.Content);
                Log.Error("TagPlus - PostPedidos(PedidoCompraBody body) - Erro durante a criação do pedido de compra");
                Log.Error(JsonConvert.SerializeObject(body));
                Log.Error($"Código {error.ErrorCode} : {error.Message}");
                throw new TagPlusException($"Código {error.ErrorCode} : {error.Message}");
            }
            else
            {
                var pedido = JsonConvert.DeserializeObject<GetPedidoCompraResponse>(response.Content);
                return pedido;
            }
        }

        public IList<GetFornecedoresResponse> GetFornecedores()
        {
            var client = new RestClient(ApiUrl);
            var request = new RestRequest("fornecedores", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("apikey", AccessToken);
            request.AddHeader("Accept", "application/json");
            var response = client.Get(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<TagPlusResponseError>(response.Content);
                Log.Error("TagPlus - GetFornecedores() - Erro durante a recuperação dos fornecedores");
                Log.Error($"Código {error.ErrorCode} : {error.Message}");
                throw new TagPlusException($"Código {error.ErrorCode} : {error.Message}");
            }
            else
            {
                var fornecedores = JsonConvert.DeserializeObject<IList<GetFornecedoresResponse>>(response.Content);
                return fornecedores;
            }
        }
    }
}
