using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkStats
{
    public class Hist<T>
        where T: IComparable<T>
    {
        private readonly Dictionary<T, int> _data;

        public Hist(IEnumerable<T> values, IEqualityComparer<T> comparer = null)
        {
            _data = new Dictionary<T, int>(comparer ?? EqualityComparer<T>.Default);

            foreach (T val in values)
            {
                Count(val);
            }
        }

        public IReadOnlyDictionary<T, int> Data
        {
            get { return _data; }
        }

        public void Count(T val)
        {
            SetCountOf(val, GetCountOf(val)+1);
        }

        public int Freq(T val)
        {
            return GetCountOf(val);
        }

        public IEnumerable<KeyValuePair<T, int>> Items
        {
            get { return _data; }
        }

        public IEnumerable<T> Values
        {
            get { return _data.Keys; }
        }

        public IEnumerable<int> Counts
        {
            get { return _data.Values; }
        }

        private void SetCountOf(T val, int count)
        {
            _data[val] = count;
        }

        private int GetCountOf(T val)
        {
            int count;
            if (!_data.TryGetValue(val, out count))
            {
                count = 0;
            }

            return count;
        }

        public IEqualityComparer<T> Comparer
        {
            get { return _data.Comparer; }
        }
    }
}
