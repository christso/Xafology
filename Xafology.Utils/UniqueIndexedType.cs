using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.Utils
{
    public abstract class UniqueIndexedType<TKey, TValue>
    {
        private Dictionary<TKey, TValue> dict;

        public UniqueIndexedType()
        {
            dict = new Dictionary<TKey, TValue>();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            foreach (var pair in dict)
                yield return pair.Value;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return dict.TryGetValue(key, out value);
        }

        public void Add(TKey key, TValue value)
        {
            this.Add(key, value);
        }

        public void Add(TValue value)
        {
            this.Add(GetKey, value);
        }

        public TValue this[TKey key]
        {
            get
            {
                return dict[key];
            }
        }

        public void Add(Func<TValue, TKey> stringIndexDelegate, TValue value)
        {
            dict.Add(stringIndexDelegate(value), value);
        }

        public int Count
        {
            get
            {
                return dict.Count;
            }
        }



        protected abstract TKey GetKey(TValue value);
    }
}
