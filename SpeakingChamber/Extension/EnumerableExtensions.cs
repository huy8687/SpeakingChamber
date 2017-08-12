using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakingChamber.Extension
{
    public static class EnumerableExtensions
    {
        public static int Count(this IEnumerable source)
        {
            var col = source as ICollection;
            if (col != null)
                return col.Count;

            int c = 0;
            var e = source.GetEnumerator();
            while (e.MoveNext())
                c++;
            return c;
        }

        public static int Pop(this IEnumerable source)
        {
            var col = source as ICollection;
            if (col != null)
                return col.Count;
            int c = 0;
            var e = source.GetEnumerator();
            while (e.MoveNext())
                c++;
            return c;
        }
    }
}
