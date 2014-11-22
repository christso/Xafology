using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.Utils.Data
{ 
    [Serializable]
    public sealed class ConvertException : Exception
    {
        // Methods
        public ConvertException(string origValue, Type destType)
            : this(origValue, destType, string.Empty)
        {
        }

        public ConvertException(string origValue, Type destType, string extraInfo)
            : this(origValue, destType, string.Empty, -1, -1, extraInfo, null)
        {
        }

        public ConvertException(string origValue, Type destType, string fieldName, int lineNumber, int columnNumber, string extraInfo, Exception innerEx)
            : base(MessageBuilder(origValue, destType, fieldName, lineNumber, columnNumber, extraInfo), innerEx)
        {
            this.MessageOriginal = string.Empty;
            this.FieldStringValue = origValue;
            this.FieldType = destType;
            this.LineNumber = lineNumber;
            this.ColumnNumber = columnNumber;
            this.FieldName = fieldName;
            this.MessageExtra = extraInfo;
            if ((origValue != null) && (destType != null))
            {
                this.MessageOriginal = "Error Converting '" + origValue + "' to type: '" + destType.Name + "'. ";
            }
        }

        private static string MessageBuilder(string origValue, Type destType, string fieldName, int lineNumber, int columnNumber, string extraInfo)
        {
            string str = string.Empty;
            if (lineNumber >= 0)
            {
                str = str + "Line: " + lineNumber.ToString() + ". ";
            }
            if (columnNumber >= 0)
            {
                str = str + "Column: " + columnNumber.ToString() + ". ";
            }
            if (!string.IsNullOrEmpty(fieldName))
            {
                str = str + "Field: " + fieldName + ". ";
            }
            if ((origValue != null) && (destType != null))
            {
                string str2 = str;
                str = str2 + "Error Converting '" + origValue + "' to type: '" + destType.Name + "'. ";
            }
            return (str + extraInfo);
        }

        // Properties
        public int ColumnNumber { get; internal set; }

        public string FieldName { get; internal set; }

        public string FieldStringValue { get; private set; }

        public Type FieldType { get; private set; }

        public int LineNumber { get; internal set; }

        public string MessageExtra { get; private set; }

        public string MessageOriginal { get; private set; }
    }

}
