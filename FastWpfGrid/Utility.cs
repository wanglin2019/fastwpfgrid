using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastWpfGrid
{
    public class Utility
    {
        public static int Count(IEnumerable source)
        {
            if (source == null)
                return 0;
        
            ICollection collection = source as ICollection;
            if (collection != null)
                return collection.Count;

            int num = 0;
            var enumerator = source.GetEnumerator();

            while (enumerator.MoveNext())
                checked { ++num; }
            return num;
        }

        public static object ElementAt(IEnumerable source, int index)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            var sourceList = source as IList;
            if (sourceList != null)
                return sourceList[index];

            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            var enumerator = source.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (index == 0)
                    return enumerator.Current;
                --index;
            }
            throw new ArgumentOutOfRangeException(nameof(index));
        }
    }
}
