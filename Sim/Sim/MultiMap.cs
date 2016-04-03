using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim
{
    public class MultiMap<V>
    {
        // 1
        Dictionary<Tuple<Strategy, int, List<int>>, List<V>> _dictionary = new Dictionary<Tuple<Strategy, int, List<int>>, List<V>>();

        // 2
        public void Add(Tuple<Strategy, int, List<int>> key, V value)
        {
            List<V> list;
            if (this._dictionary.TryGetValue(key, out list))
            {
                // 2A.
                list.Add(value);
            }
            else
            {
                // 2B.
                list = new List<V>();
                list.Add(value);
                this._dictionary[key] = list;
            }
        }

        // 3
        public IEnumerable<Tuple<Strategy, int, List<int>>> Keys
        {
            get
            {
                return this._dictionary.Keys;
            }
        }

        // 4
        public List<V> this[Tuple<Strategy, int, List<int>> key]
        {
            get
            {
                List<V> list;
                if (!this._dictionary.TryGetValue(key, out list))
                {
                    list = new List<V>();
                    this._dictionary[key] = list;
                }
                return list;
            }
        }
    }
}
