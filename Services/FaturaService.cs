using BlingIntegrationTagplus.Clients.Bling.Models.Pedidos;
using BlingIntegrationTagplus.Clients.TagPlus.Models.FormasPagamento;
using BlingIntegrationTagplus.Clients.TagPlus.Models.Pedidos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Serilog;

namespace BlingIntegrationTagplus.Services
{
    class FaturaService
    {
        public List<Fatura> ConstructFatura(PedidoItem pedido, IList<GetFormasPagamentoResponse> formasPagamento)
        {
            var faturas = new List<Fatura>();
            var fatura = new Fatura
            {
                Parcelas = new List<Clients.TagPlus.Models.Pedidos.Parcela>()
            };
            foreach (var parcelaWrapper in pedido.Pedido.Parcelas)
            {
                var parcela = parcelaWrapper.Parcela;

                // Contorno para o Boleto
                var formaPagamentoQuery = parcela.FormaPagamento.Descricao.Equals("Boleto (Conta a receber/pagar)",
                    StringComparison.OrdinalIgnoreCase)
                    ? "Boleto"
                    : parcela.FormaPagamento.Descricao;

                var formaPagamento =
                    formasPagamento.FirstOrDefault(forma =>
                        forma.Descricao.Equals(formaPagamentoQuery, StringComparison.OrdinalIgnoreCase));

                if (formaPagamento == null)
                {
                    Log.Error($"Forma de pagamento: {formaPagamentoQuery} não encontrada");
                    return new List<Fatura>();
                }

                var formaPagamentoId = formaPagamento.Id;
                // Converte a data de vencimento
                var date = DateTime.Parse(parcela.DataVencimento).ToString("yyyy-MM-dd");
                var parcelaTagPlus = new Clients.TagPlus.Models.Pedidos.Parcela
                {
                    ValorParcela = float.Parse(parcela.Valor, CultureInfo.InvariantCulture.NumberFormat),
                    DataVencimento = date
                };
                fatura.Parcelas.Add(parcelaTagPlus);
                fatura.FormaPagamento = formaPagamentoId;
            }

            faturas.Add(fatura);

            return faturas;
        }
    }
}
