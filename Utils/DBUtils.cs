using BlingIntegrationTagplus.Databases;
using BlingIntegrationTagplus.Models;
using System;
using System.Linq;

namespace BlingIntegrationTagplus.Utils
{
    public class DBUtils
    {
        static readonly string CLIENT_ID = "r7Qww3H1q0aZxpHqQFKAwa47gYdWxHjW";
        static readonly string SCOPE = "read:produtos+read:formas_pagamento+read:departamentos+read:usuarios+read:tipos_contatos+write:pedidos+write:clientes";

        public static string RetriveTagPlusToken(IntegrationContext db)
        {
            string code;
            var token = db.TagPlusTokens.SingleOrDefault(setting => setting.Name.Equals("TagplusToken"));
            if (token == null || DateTime.Now.CompareTo(DateTime.Parse(token.ExpiresIn)) >= 0)
            {
                Console.WriteLine();
                Console.WriteLine("O Token do Tagplus não foi encontrado no banco de dados ou está expirado");
                Console.WriteLine("Será necessário autorizar a integração no Tagplus");
                Console.WriteLine("O navegador será aberto para isso");
                Console.WriteLine("Por gentiliza, siga as instruções e insira o código gerado");
                OSUtils.OpenBrowser($"https://developers.tagplus.com.br/authorize?response_type=token&client_id={CLIENT_ID}&scope={SCOPE}");
                code = Console.ReadLine();
                while (string.IsNullOrWhiteSpace(code))
                {
                    Console.WriteLine("Não foi informado o código. Por gentiliza, informe o código:");
                    code = Console.ReadLine();
                }
                // Gera a data de expiração
                // O Token expira em 24 horas, mas como "folga" ele será renovado em 20 horas
                DateTime expirationDate = DateTime.Now.AddHours(20);
                // Remove o registro antigo
                if (token == null)
                {
                    db.Add(new TagPlusToken("TagplusToken", code, expirationDate.ToString("dd/MM/yyyy HH:mm:ss")));
                }
                else
                {
                    token.Value = code;
                    token.ExpiresIn = expirationDate.ToString("dd/MM/yyyy HH:mm:ss");
                    db.Update(token);
                }
                db.SaveChanges();
            }
            else
            {
                code = token.Value;
            }
            return code;
        }
    }
}
