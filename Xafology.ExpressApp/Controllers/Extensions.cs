using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.Utilities
{
    public static class Extensions
    {
        public static string ToTraceString<T>(this IQueryable<T> t)
        {
            string sql = "";
            ObjectQuery<T> oqt = t as ObjectQuery<T>;
            if (oqt != null)
                sql = oqt.ToTraceString();
            return sql;
        }

        #region String and Byte Converter
        //http://stackoverflow.com/questions/472906/net-string-to-byte-array-c-sharp

        public static byte[] ToBytes(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        //static string ToString(this byte[] bytes)
        //{
        //    char[] chars = new char[bytes.Length / sizeof(char)];
        //    System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        //    return new string(chars);
        //}

        #endregion

        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length);
        }

        public static bool IsNumericType(this Type t)
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsNumericType(this object o)
        {
            return o.GetType().IsNumericType();
        }

        public static bool IsNumeric(this object o)
        {
            double result;
            return o != null && Double.TryParse(o.ToString(), out result);
        }

        public static Object GetPropValue(this Object obj, String name)
        {
            foreach (String part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        public static T GetPropValue<T>(this Object obj, String name)
        {
            Object retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }
    }
}
