using System.Collections.Generic;

namespace Xafology.Utils
{
    public class StringUtils
    {
        public static string QuoteJoinString(string separator, string quote, IEnumerable<string> values)
        {
            string result = "";
            foreach (string value in values)
            {
                if (result != "")
                    result += separator;
                result += " " + quote + value + quote;
            }
            return result;
        }
    }
}
