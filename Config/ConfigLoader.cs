using BlingIntegrationTagplus.Exceptions;
using BlingIntegrationTagplus.Models;
using dotenv.net;
using dotenv.net.Utilities;
using System;
using System.IO;

namespace BlingIntegrationTagplus
{
    class ConfigLoader
    {

        protected ConfigLoader() { }

        public static Config LoadConfig()
        {
            // Verifica se o arquivo existe
            if (!File.Exists("config.txt"))
            {
                throw new ConfigException("O arquivo config.txt não foi encontrado");
            }

            // Carrega o arquivo de configuração
            try
            {
                DotEnv.Load(new DotEnvOptions(envFilePaths: new []{ "config.txt" }));
                // Carrega a API KEY do Bling
                var blingApiKey = EnvReader.GetStringValue("BLING_API_KEY");
                // Carrega a data inicial do Bling
                var blingInitialDate = EnvReader.GetStringValue("BLING_INITIAL_DATE");
                // Carrega o Token do Tagplus
                var tagplusToken = EnvReader.GetStringValue("TAGPLUS_TOKEN");
                // Carrega o número de pedido especifico
                EnvReader.TryGetStringValue("BLING_ORDER_NUM", out var blingOrderNum);
                // Carrega a URL da API do Bling
                var blingApiUrl = EnvReader.GetStringValue("BLING_API_URL");
                // Carrega a URL da API do Tagplus
                var tagplusApiUrl = EnvReader.GetStringValue("TAGPLUS_API_URL");
                return new Config(blingApiKey, blingInitialDate, tagplusToken, blingApiUrl, tagplusApiUrl, blingOrderNum);
            }
            catch (Exception e)
            {
                throw new ConfigException(e.Message);
            }
        }
    }
}
