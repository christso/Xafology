using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.Utils
{
    public class DataStructUtils
    {
        public static T[] RandomizeArray<T>(T[] arg)
        {
            var random = new Random();
            var list = new List<KeyValuePair<int, T>>();
            for (int i = 0; i < arg.Length; i++)
            {
                list.Add(new KeyValuePair<int, T>(random.Next(), arg[i]));
            }
            IEnumerable<KeyValuePair<int, T>> sorted = list.OrderBy(x => x.Key).Select(x => x);

            T[] result = new T[arg.Length];
            int index = 0;
            foreach (var pair in sorted)
            {
                result[index] = pair.Value;
                index++;
            }
            return result;
        }
    }
}
