using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Xafology.StringEvaluators
{
    public delegate object ParserDelegate(FunctionParseDelegateArgs e);

    // Dictionary: Key is function name
    public class FunctionParser : Dictionary<string, ParserDelegate>
    {
        // returns string with '?' instead of the function value
        public ParsedCriteria ParseToCriteria(string inputText)
        {
            var funcValues = new List<object>();
            var distinctFuncStringDic = new Dictionary<string, object>();

            foreach (string funcName in this.Keys)
            {
                // get full matches of function strings, e.g. {MyFunc('arg1')}
                string[] funcStrings = GetFunctionStrings(inputText, funcName);
                
                foreach (string funcString in funcStrings)
                {
                    if (string.IsNullOrEmpty(funcString)) continue;
                    
                    var delArgs = new FunctionParseDelegateArgs();
                    delArgs.FunctionName = funcName;
                    delArgs.FunctionArgs = GetFunctionArgs(funcString);

                    // get function value either from delegate or from cached dictionary
                    object funcValue = null;
                    if (!distinctFuncStringDic.TryGetValue(funcString, out funcValue))
                        distinctFuncStringDic.Add(funcString, this[funcName](delArgs));
                    funcValue = distinctFuncStringDic[funcString];

                    inputText = inputText.Replace(funcString, "?");
                    funcValues.Add(funcValue);
                }
            }
            var result = new ParsedCriteria();
            result.Criteria = inputText;
            result.Parameters = funcValues.ToArray();
            return result;
        }

        public string Parse(string inputText)
        {
            // replacement dictionary -->
            // key: substring to replace
            // value: new string
            var stringReplDic = new Dictionary<string, string>();

            foreach (string funcName in this.Keys)
            {
                // get full matches, e.g. {MyFunc('arg1')}
                string[] funcStrings = GetFunctionStrings(inputText, funcName);
                foreach (string funcString in funcStrings)
                {
                    if (string.IsNullOrEmpty(funcString)) continue;

                    // warning: the delegate function is called once per unique function string 
                    if (stringReplDic.ContainsKey(funcString)) continue;

                    var delArgs = new FunctionParseDelegateArgs();
                    delArgs.FunctionName = funcName;
                    delArgs.FunctionArgs = GetFunctionArgs(funcString);

                    string funcValue = Convert.ToString(this[funcName](delArgs));
                    stringReplDic.Add(funcString, funcValue);
                }
            }
            return StringHelpers.ReplaceText(inputText, stringReplDic);
        }

        public static string[] GetFunctionStrings(string inputText, string funcName)
        {
            //regex pattern: {***(***)}
            string pattern = "\\{" + funcName + "\\([^\\}]*\\)\\}";

            MatchCollection matchedItems = Regex.Matches(inputText, pattern);
            string[] result = new string[matchedItems.Count];
            for (int i = 0; i <= matchedItems.Count - 1; i++)
            {
                result[i] = matchedItems[i].Value;
            }
            return result;
        }

        // funcText looks like {MyFunc('value')}
        public static string[] GetFunctionArgs(string funcString)
        {
            //remove braces
            funcString = funcString.Substring(1, funcString.Length - 2);
            return StringHelpers.GetFuncArgs(funcString);
        }

        #region Helpers

        private class StringHelpers
        {
            public static string ReplaceText(string InpText, Dictionary<string, string> repl)
            {
                return repl.Aggregate(InpText, (s, r) => s.Replace(r.Key, r.Value));
            }

            public static string[] GetFuncArgs(string funcText)
            {
                string s = RemoveFuncName(funcText);
                string[] args = CSVLineToArray(s);
                return args;
            }

            public static string RemoveFuncName(string s)
            {
                int Index = s.IndexOf("(");
                if (Index != -1 & Right(s, 1) == ")")
                {
                    return s.Substring(Index + 1, s.Length - Index - 2);
                }
                else
                {
                    return s;
                }
            }

            public static string Right(string value, int length)
            {
                return value.Substring(value.Length - length);
            }

            public static string[] CSVLineToArray(string text)
            {
                string pattern = ",(?=(?:[^\\\"]*\\\"[^\\\"]*\\\")*(?![^\\\"]*\\\"))";
                System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(pattern);
                string[] result = r.Split(text);
                result = TrimQuotes(result);
                result = TrimSpace(result);
                return result;
            }

            public static string[] TrimQuotes(string[] sArr)
            {
                for (int i = 0; i < sArr.Length; i++)
                {
                    string s = sArr[i];
                    if (s.Substring(0, 1) == "\"")
                    {
                        s = s.Substring(2, s.Length);
                    }
                    if (Right(s, 1) == "\"")
                    {
                        s = s.Substring(1, s.Length - 1);
                    }
                    sArr[i] = s;
                }

                return sArr;
            }

            public static string[] TrimSpace(string[] sArr)
            {
                int i = 0;
                foreach (string s in sArr)
                {
                    sArr[i] = s.Trim();
                    i += 1;
                }
                return sArr;
            }
        }
        #endregion
    }
}
