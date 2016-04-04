using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim
{
    /// <summary>
    /// This MultiMap is used to group Runs by specific feature parameters.
    /// These parameters of interest compose the Tuple, which serves as the key.
    /// Each Run that creates that Tuple as its key is grouped into the List<Run>
    /// </summary>
    public class MultiMap
    {
        Dictionary<Tuple<Strategy, int>, List<Run>> _dictionary = new Dictionary<Tuple<Strategy, int>, List<Run>>();

        public void Add(Tuple<Strategy, int> key, Run value)
        {
            List<Run> list;
            if (this._dictionary.TryGetValue(key, out list))
            {
                list.Add(value);
            }
            else
            {
                list = new List<Run>();
                list.Add(value);
                this._dictionary[key] = list;
            }
        }

        public IEnumerable<Tuple<Strategy, int>> Keys
        {
            get
            {
                return this._dictionary.Keys;
            }
        }

        public List<Run> this[Tuple<Strategy, int> key]
        {
            get
            {
                List<Run> list;
                if (!this._dictionary.TryGetValue(key, out list))
                {
                    list = new List<Run>();
                    this._dictionary[key] = list;
                }
                return list;
            }
        }
    }
}
