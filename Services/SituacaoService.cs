using BlingIntegrationTagplus.Clients.Bling;
using BlingIntegrationTagplus.Clients.Bling.Models.Situacao;
using BlingIntegrationTagplus.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace BlingIntegrationTagplus.Services
{
    class SituacaoService
    {

        private readonly BlingClient blingClient;

        public SituacaoService(BlingClient blingClient)
        {
            this.blingClient = blingClient;
        }

        public Dictionary<string, string> GetSituacoes()
        {
            GetSituacaoResponse situacoes = null;
            try
            {
                situacoes = blingClient.ExecuteGetSituacao();
            }
            catch (BlingException e)
            {
                throw new SituacaoException($"Não foi possível recuperar as situações: {e.Message}");                
            }

            string situacaoImportado = situacoes.Retorno.Situacoes.First(situacao => situacao.Situacao.Nome.Equals("Importado no TagPlus")).Situacao.Id;
            string situacaoEmAberto = situacoes.Retorno.Situacoes.First(situacao => situacao.Situacao.Nome.Equals("Em aberto")).Situacao.Id;
            string situacaoEmAndamento = situacoes.Retorno.Situacoes.First(situacao => situacao.Situacao.Nome.Equals("Em andamento")).Situacao.Id;

            return new Dictionary<string, string>()
            {
                { "IMPORTADO", situacaoImportado },
                { "ABERTO", situacaoEmAberto },
                { "ANDAMENTO", situacaoEmAndamento }
            };
        }
    }
}
