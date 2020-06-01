using System;

namespace bling_integration_tagplus
{
    class Program
    {
        static string CLIENT_ID = "r7Qww3H1q0aZxpHqQFKAwa47gYdWxHjW";

        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando a integração...");
            Console.WriteLine("Será necessário fazer o login no Tagplus");
            Console.WriteLine("Por gentiliza, copie e cole o abaixo endereço no navegador");
            Console.WriteLine($"https://developers.tagplus.com.br/authorize?response_type=token&client_id={CLIENT_ID}&scope=write:produtos+read:pedidos");
        }
    }
}
