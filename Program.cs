using BlingIntegrationTagplus.Clients;
using BlingIntegrationTagplus.Databases;
using BlingIntegrationTagplus.Exceptions;
using BlingIntegrationTagplus.Models;
using BlingIntegrationTagplus.Models.Bling;
using BlingIntegrationTagplus.Utils;
using dotenv.net;
using dotenv.net.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BlingIntegrationTagplus
{
    class Program
    {
        static readonly string CLIENT_ID = "r7Qww3H1q0aZxpHqQFKAwa47gYdWxHjW";

        static void Main(string[] args)
        {
            Console.WriteLine("############################################");
            Console.WriteLine("## Bem vindo a integração Bling - Tagplus ##");
            Console.WriteLine("############################################");

            // Carrega o arquivo de configuração
            DotEnv.Config();
            var envReader = new EnvReader();
            // Carrega a API KEY do Bling
            var blingApiKey = envReader.GetStringValue("BLING_API_KEY");

            // Inicializa o banco de dados local
            using var db = new IntegrationContext();
            // Realiza as migrações
            db.Database.Migrate();
            // Verifica se o Token do Tagplus já está no banco de dados
            var token = db.SettingStrings.SingleOrDefault(setting => setting.Name.Equals("TagplusToken"));
            if (token == null)
            {
                Console.WriteLine();
                Console.WriteLine("O Token do Tagplus não foi encontrado no banco de dados");
                Console.WriteLine("Será necessário autorizar a integração no Tagplus");
                Console.WriteLine("O navegador será aberto para isso");
                Console.WriteLine("Por gentiliza, siga as instruções e insira o código gerado");
                OSUtils.OpenBrowser($"https://developers.tagplus.com.br/authorize?response_type=token&client_id={CLIENT_ID}&scope=write:produtos+read:pedidos");
                string code = Console.ReadLine();
                while (code.Length == 0)
                {
                    Console.WriteLine("Não foi informado o código. Por gentiliza, informe o código:");
                    code = Console.ReadLine();
                }
                db.Add(new SettingString("TagplusToken", code));
                db.SaveChanges();
            }
            Console.WriteLine();
            // Recupera os pedidos do Bling
            Console.WriteLine("Procurando pedidos no Bling...");
            var blingClient = new BlingClient(blingApiKey);
            PedidosResponse pedidos = null;
            try
            {
                pedidos = blingClient.ExecuteGetOrder();
            }
            catch (BlingException e)
            {
                Console.WriteLine($"Não foi possível recuperar os pedidos do Bling - {e.Message}");
                Environment.Exit(-1);
            }
        }
    }
}
