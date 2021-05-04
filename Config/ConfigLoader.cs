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
            // Carrega o arquivo de configuração
            try
            {
                DotEnv.Config(filePath: "config.txt");
                var envReader = new EnvReader();
                // Carrega a API KEY do Bling
                string blingApiKey = envReader.GetStringValue("BLING_API_KEY");
                // Carrega a data inicial do Bling
                string blingInitialDate = envReader.GetStringValue("BLING_INITIAL_DATE");
                // Carrega o Token do Tagplus
                string tagplusToken = envReader.GetStringValue("TAGPLUS_TOKEN");
                // Carrega o número de pedido especifico
                envReader.TryGetStringValue("BLING_ORDER_NUM", out string blingOrderNum);
                // Carrega a URL da API do Bling
                string blingApiUrl = envReader.GetStringValue("BLING_API_URL");
                // Carrega a URL da API do Tagplus
                string tagplusApiUrl = envReader.GetStringValue("TAGPLUS_API_URL");
                return new Config(blingApiKey, blingInitialDate, tagplusToken, blingApiUrl, tagplusApiUrl, blingOrderNum);
            }
            catch (FileNotFoundException)
            {
                throw new ConfigException("O arquivo config.txt não foi encontrado");
            }
            catch (Exception e)
            {
                throw new ConfigException(e.Message);
            }
        }
    }
}
