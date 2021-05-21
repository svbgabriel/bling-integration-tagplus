using BlingIntegrationTagplus.Clients.Bling;
using BlingIntegrationTagplus.Clients.Bling.Models.Situacao;
using BlingIntegrationTagplus.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace BlingIntegrationTagplus.Services
{
    class SituacaoService
    {

        private readonly BlingClient _blingClient;

        public SituacaoService(BlingClient blingClient)
        {
            _blingClient = blingClient;
        }

        public Dictionary<string, string> GetSituacoes()
        {
            GetSituacaoResponse situacoes;
            try
            {
                situacoes = _blingClient.ExecuteGetSituacao();
            }
            catch (BlingException e)
            {
                throw new SituacaoException($"Não foi possível recuperar as situações: {e.Message}");
            }

            var situacaoImportado = situacoes.Retorno.Situacoes.FirstOrDefault(situacao => situacao.Situacao.Nome.Equals("Importado no TagPlus"));
            var situacaoEmAberto = situacoes.Retorno.Situacoes.FirstOrDefault(situacao => situacao.Situacao.Nome.Equals("Em aberto"));
            var situacaoEmAndamento = situacoes.Retorno.Situacoes.FirstOrDefault(situacao => situacao.Situacao.Nome.Equals("Em andamento"));

            if (situacaoImportado == null)
            {
                throw new SituacaoException($"Não foi possível recuperar a situação: Importado no TagPlus");
            }

            if (situacaoEmAberto == null)
            {
                throw new SituacaoException($"Não foi possível recuperar a situação: Em aberto");
            }

            if (situacaoEmAndamento == null)
            {
                throw new SituacaoException($"Não foi possível recuperar a situação: Em andamento");
            }

            var situacaoImportadoId = situacaoImportado.Situacao.Id;
            var situacaoEmAbertoId = situacaoEmAberto.Situacao.Id;
            var situacaoEmAndamentoId = situacaoEmAndamento.Situacao.Id;

            return new Dictionary<string, string>
            {
                { "IMPORTADO", situacaoImportadoId },
                { "ABERTO", situacaoEmAbertoId },
                { "ANDAMENTO", situacaoEmAndamentoId }
            };
        }
    }
}
