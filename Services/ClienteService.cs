using BlingIntegrationTagplus.Clients.Bling.Models.Pedidos;
using BlingIntegrationTagplus.Clients.TagPlus;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Clientes;
using BlingIntegrationTagplus.Exceptions;
using BlingIntegrationTagplus.Utils;
using System.Collections.Generic;

namespace BlingIntegrationTagplus.Services
{
    class ClienteService
    {
        private readonly TagPlusClient tagPlusClient;

        public ClienteService(TagPlusClient tagPlusClient)
        {
            this.tagPlusClient = tagPlusClient;
        }

        public int GetCliente(PedidoItem pedido)
        {
            int clienteId = 0;

            if (!string.IsNullOrWhiteSpace(pedido.Pedido.Cliente.Cnpj))
            {
                if (ValidateUtils.IsCpf(pedido.Pedido.Cliente.Cnpj))
                {
                    clienteId = tagPlusClient.GetClienteByCpf(pedido.Pedido.Cliente.Cnpj);
                }
                else if (ValidateUtils.IsCnpj(pedido.Pedido.Cliente.Cnpj))
                {
                    clienteId = tagPlusClient.GetClienteByCnpj(pedido.Pedido.Cliente.Cnpj);
                }

            }
            else
            {
                clienteId = tagPlusClient.GetClienteByRazaoSocial(pedido.Pedido.Cliente.Nome);
            }

            return clienteId;
        }

        public int CreateCliente(PedidoItem pedido, Dictionary<string, int> tiposContato)
        {
            tiposContato.TryGetValue("EMAIL", out int emailContato);
            tiposContato.TryGetValue("CELULAR", out int celularContato);
            tiposContato.TryGetValue("TELEFONE", out int telefoneContato);

            ClienteBody cliente = new ClienteBody();
            cliente.RazaoSocial = pedido.Pedido.Cliente.Nome;
            cliente.Ativo = true;
            if (!string.IsNullOrWhiteSpace(pedido.Pedido.Cliente.Cnpj))
            {
                if (ValidateUtils.IsCpf(pedido.Pedido.Cliente.Cnpj))
                {
                    cliente.Cpf = pedido.Pedido.Cliente.Cnpj;
                    cliente.Tipo = "F";
                }
                else if (ValidateUtils.IsCnpj(pedido.Pedido.Cliente.Cnpj))
                {
                    cliente.Cnpj = pedido.Pedido.Cliente.Cnpj;
                    cliente.Tipo = "J";
                }
            }

            // Preenche as informações de contato
            List<Contato> contatos = new List<Contato>();
            // Verifica se existe o telefone
            if (!string.IsNullOrWhiteSpace(pedido.Pedido.Cliente.Fone))
            {
                Contato fone = new Contato
                {
                    TipoContato = telefoneContato,
                    Descricao = pedido.Pedido.Cliente.Fone,
                    Principal = true
                };
                contatos.Add(fone);
            }
            // Verifica se existe o e-mail
            if (!string.IsNullOrWhiteSpace(pedido.Pedido.Cliente.Email))
            {
                Contato email = new Contato
                {
                    TipoContato = emailContato,
                    Descricao = pedido.Pedido.Cliente.Email,
                    Principal = true
                };
                contatos.Add(email);
            }
            // Verifica se existe o celular
            if (!string.IsNullOrWhiteSpace(pedido.Pedido.Cliente.Celular))
            {
                Contato celular = new Contato
                {
                    TipoContato = celularContato,
                    Descricao = pedido.Pedido.Cliente.Celular,
                    Principal = true
                };
                contatos.Add(celular);
            }

            // Caso existam contatos, adiciona ao contato
            if (contatos.Count > 0)
            {
                cliente.Contatos = contatos;
            }

            // Preenche o endereço, se estiver disponível
            if (pedido.Pedido.Transporte != null)
            {
                cliente.Enderecos = new List<Endereco>();
                Endereco endereco = new Endereco
                {
                    Logradouro = pedido.Pedido.Transporte.EnderecoEntrega.Endereco,
                    Numero = pedido.Pedido.Transporte.EnderecoEntrega.Numero,
                    Bairro = pedido.Pedido.Transporte.EnderecoEntrega.Bairro,
                    Complemento = pedido.Pedido.Transporte.EnderecoEntrega.Complemento,
                    Cep = pedido.Pedido.Transporte.EnderecoEntrega.Cep.Replace(".", ""),
                    Principal = true
                };
                cliente.Enderecos.Add(endereco);
            }
            else if (pedido.Pedido.Cliente.Endereco != null && pedido.Pedido.Cliente.Numero != null
                && pedido.Pedido.Cliente.Cep != null && pedido.Pedido.Cliente.Bairro != null)
            {
                cliente.Enderecos = new List<Endereco>();
                Endereco endereco = new Endereco
                {
                    Logradouro = pedido.Pedido.Cliente.Endereco,
                    Numero = pedido.Pedido.Cliente.Numero,
                    Bairro = pedido.Pedido.Cliente.Bairro,
                    Complemento = pedido.Pedido.Cliente.Complemento,
                    Cep = pedido.Pedido.Cliente.Cep.Replace(".", ""),
                    Principal = true
                };
                cliente.Enderecos.Add(endereco);
            }

            int clienteId;
            try
            {
                clienteId = tagPlusClient.PostCliente(cliente);
            }
            catch (TagPlusException e)
            {
                throw new ClienteException($"Não foi possível cadastrar o cliente: {e.Message}");
            }

            return clienteId;
        }
    }
}
