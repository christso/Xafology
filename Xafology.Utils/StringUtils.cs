using System.Collections.Generic;
using System.IO;
using System.Text;

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

        public static Stream ConvertToCsvStream(string csvText)
        {
            byte[] csvBytes = Encoding.UTF8.GetBytes(csvText);
            return new MemoryStream(csvBytes);
        }
    }
}
