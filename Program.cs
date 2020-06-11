using dotenv.net;
using dotenv.net.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace BlingIntegrationTagplus
{
    class Program
    {
        static string CLIENT_ID = "r7Qww3H1q0aZxpHqQFKAwa47gYdWxHjW";

        static void Main(string[] args)
        {
            // Carrega o arquivo de configuração
            DotEnv.Config();
            var envReader = new EnvReader();

            using (var db = new IntegrationContext())
            {
                db.Database.Migrate();
                var token = db.SettingStrings.SingleOrDefault(setting => setting.name.Equals("TagplusToken"));
                if (token == null)
                {
                    Console.WriteLine("O Token do Tagplus não foi encontrado no banco de dados");
                }
            }

            Console.WriteLine("Bem vindo a integração Bling - Tagplus");
            Console.WriteLine();
            Console.WriteLine("Será necessário autorizar a integração no Tagplus");
            Console.WriteLine("O navegador será aberto para isso");
            Console.WriteLine("Por gentiliza, siga as instruções e insira o código gerado");
            OpenBrowser($"https://developers.tagplus.com.br/authorize?response_type=token&client_id={CLIENT_ID}&scope=write:produtos+read:pedidos");
            string code = Console.ReadLine();
        }

        static void OpenBrowser(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
        }
    }
}
