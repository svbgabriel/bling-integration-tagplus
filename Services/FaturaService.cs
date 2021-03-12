using BlingIntegrationTagplus.Clients.Bling.Models.Pedidos;
using BlingIntegrationTagplus.Clients.TagPlus.Models.FormasPagamento;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Pedidos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BlingIntegrationTagplus.Services
{
    class FaturaService
    {
        public List<Fatura> ConstructFatura(PedidoItem pedido, IList<GetFormasPagamentoResponse> formasPagamento)
        {
            List<Fatura> faturas = new List<Fatura>();
            Fatura fatura = new Fatura
            {
                Parcelas = new List<Clients.TagPlus.Models.Pedidos.Parcela>()
            };
            foreach (ParcelaItem parcelaWrapper in pedido.Pedido.Parcelas)
            {
                Clients.Bling.Models.Pedidos.Parcela parcela = parcelaWrapper.Parcela;
                int formaPagamento = formasPagamento.First(forma => forma.Descricao.Equals(parcela.FormaPagamento.Descricao)).Id;
                // Converte a data de vencimento
                string date = DateTime.Parse(parcela.DataVencimento).ToString("yyyy-MM-dd");
                Clients.TagPlus.Models.Pedidos.Parcela parcelaTagPlus = new Clients.TagPlus.Models.Pedidos.Parcela
                {
                    ValorParcela = float.Parse(parcela.Valor, CultureInfo.InvariantCulture.NumberFormat),
                    DataVencimento = date
                };
                fatura.Parcelas.Add(parcelaTagPlus);
                fatura.FormaPagamento = formaPagamento;
            }
            faturas.Add(fatura);

            return faturas;
        }
    }
}
