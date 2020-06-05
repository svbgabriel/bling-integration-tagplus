using System;
using System.IO;
using System.Net;

namespace bling_integration_tagplus
{
    class BlingClient
    {
        private string apiKey { get; set; }

        public BlingClient(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public void ExecuteGetOrder()
        {
            var request = HttpWebRequest.Create($"https://bling.com.br/Api/v2/pedidos/json&apikey={apiKey}");
            request.ContentType = "application/json";
            request.Method = "GET";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    Console.Out.WriteLine("Error. Server returned status code: {0}", response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                        Console.Out.WriteLine("Empty Response");
                    else
                        Console.Out.WriteLine("Response Body: \r\n {0}", content);
                }
            }
        }
    }
}
