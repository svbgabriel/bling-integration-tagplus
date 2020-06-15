using BlingIntegrationTagplus.Models.TagPlus;
using RestSharp;
using System.Net;

namespace BlingIntegrationTagplus.Clients
{
    class TagPlusClient
    {
        private string AccessToken { get; set; }

        public TagPlusClient(string accessToken)
        {
            this.AccessToken = accessToken;
        }

        public void ExecuteCreateOrder(CreateOrderBody body)
        {
            var client = new RestClient("https://api.tagplus.com.br");
            var request = new RestRequest("pedidos", DataFormat.Json);
            request.AddHeader("X-Api-Version", "2.0");
            request.AddHeader("Authorization", $"Bearer {AccessToken}");
            request.AddJsonBody(body);
            var response = client.Post(request);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                // TODO
            }
            else
            {
                // TODO
            }
        }
    }
}
