using System;

namespace BlingIntegrationTagplus.Clients.Bling.Filters
{
    public class BuildOrdersFilter
    {
        private string filters;

        public BuildOrdersFilter()
        {
            filters = "";
        }

        public BuildOrdersFilter AddDateFilter(DateTime dateStart, DateTime dateEnd)
        {
            string dateStartString = dateStart.ToString("dd/MM/yyyy");
            string dateEndString = dateEnd.ToString("dd/MM/yyyy");
            string filter = $"dataEmissao[{dateStartString} TO {dateEndString}]";
            if (string.IsNullOrEmpty(filters))
            {
                filters = filter;
            }
            else
            {
                filters = $"{filters}; {filter}";
            }
            return this;
        }

        public BuildOrdersFilter AddSituation(string situacaoId)
        {
            string filter = $"idSituacao[{situacaoId}]";
            if (string.IsNullOrEmpty(filters))
            {
                filters = filter;
            }
            else
            {
                filters = $"{filters}; {filter}";
            }
            return this;
        }

        public string Build()
        {
            return filters;
        }
    }
}
