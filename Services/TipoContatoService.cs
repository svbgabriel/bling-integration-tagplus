using BlingIntegrationTagplus.Clients.TagPlus;
using BlingIntegrationTagplus.Clients.TagPlus.Models.TiposContatos;
using BlingIntegrationTagplus.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace BlingIntegrationTagplus.Services
{
    class TipoContatoService
    {

        private readonly TagPlusClient tagPlusClient;

        public TipoContatoService(TagPlusClient tagPlusClient)
        {
            this.tagPlusClient = tagPlusClient;
        }

        public Dictionary<string, int> GetListaContatos()
        {
            IList<GetTiposContatosResponse> tiposContato = null;
            try
            {
                tiposContato = tagPlusClient.GetTiposContatos();
            }
            catch (TagPlusException e)
            {
                throw new TipoContatoException($"Não foi possível recuperar os tipos de contato: {e.Message}");
            }

            var emailContato = tiposContato.First(contato => contato.Descricao.Equals("Email")).Id;
            var celularContato = tiposContato.First(contato => contato.Descricao.Equals("Celular")).Id;
            var telefoneContato = tiposContato.First(contato => contato.Descricao.Equals("Telefone")).Id;

            var contatos = new Dictionary<string, int>()
            {
                { "EMAIL", emailContato },
                { "CELULAR", celularContato },
                { "TELEFONE", telefoneContato }
            };

            return contatos;
        }
    }
}
