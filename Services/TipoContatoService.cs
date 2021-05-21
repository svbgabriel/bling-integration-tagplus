using BlingIntegrationTagplus.Clients.TagPlus;
using BlingIntegrationTagplus.Clients.TagPlus.Models.TiposContatos;
using BlingIntegrationTagplus.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace BlingIntegrationTagplus.Services
{
    class TipoContatoService
    {

        private readonly TagPlusClient _tagPlusClient;

        public TipoContatoService(TagPlusClient tagPlusClient)
        {
            _tagPlusClient = tagPlusClient;
        }

        public Dictionary<string, int> GetListaContatos()
        {
            IList<GetTiposContatosResponse> tiposContato;
            try
            {
                tiposContato = _tagPlusClient.GetTiposContatos();
            }
            catch (TagPlusException e)
            {
                throw new TipoContatoException($"Não foi possível recuperar os tipos de contato: {e.Message}");
            }

            var emailContato = tiposContato.FirstOrDefault(contato => contato.Descricao.Equals("Email"));
            var celularContato = tiposContato.FirstOrDefault(contato => contato.Descricao.Equals("Celular"));
            var telefoneContato = tiposContato.FirstOrDefault(contato => contato.Descricao.Equals("Telefone"));

            if (emailContato == null)
            {
                throw new TipoContatoException($"Não foi possível recuperar o tipo de contato: Email");
            }

            if (celularContato == null)
            {
                throw new TipoContatoException($"Não foi possível recuperar o tipo de contato: Celular");
            }

            if (telefoneContato == null)
            {
                throw new TipoContatoException($"Não foi possível recuperar o tipo de contato: Telefone");
            }

            var emailContatoId = emailContato.Id;
            var celularContatoId = celularContato.Id;
            var telefoneContatoId = telefoneContato.Id;

            var contatos = new Dictionary<string, int>
            {
                { "EMAIL", emailContatoId },
                { "CELULAR", celularContatoId },
                { "TELEFONE", telefoneContatoId }
            };

            return contatos;
        }
    }
}
