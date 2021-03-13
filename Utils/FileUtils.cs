using BlingIntegrationTagplus.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;

namespace BlingIntegrationTagplus.Utils
{
    class FileUtils
    {
        private static readonly string FORNECEDORESFILE = "fornecedores.txt";

        protected FileUtils() { }

        public static Dictionary<string, string> ReadFornecedoresFile()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            try
            {
                // Abre o leitor
                using var sr = new StreamReader(FORNECEDORESFILE);
                // Lê linha a linha e divide pelo igual "="
                string line = sr.ReadLine();
                while (line != null)
                {
                    var split = line.Split("=");
                    dictionary.Add(split[0], split[1]);
                    line = sr.ReadLine();
                }
            }
            catch (IOException e)
            {
                throw new UtilsException($"Erro durante a leitura do arquivo: {e.Message}");
            }
            catch (Exception e)
            {
                throw new UtilsException($"Erro desconhecido durante a leitura do arquivo: {e.Message}");
            }

            return dictionary;
        }
    }
}
