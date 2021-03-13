using System.Text.RegularExpressions;

namespace BlingIntegrationTagplus.Utils
{
    class StringUtils
    {
        protected StringUtils() { }

        public static string ExtractBetweenParentheses(string value)
        {
            return Regex.Match(value, @"\(([^)]*)\)").Groups[1].Value;
        }
    }
}
