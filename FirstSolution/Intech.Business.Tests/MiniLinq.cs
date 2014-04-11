using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    public static class MiniLinqExtension
    {
        public static IEnumerable<TResult> Select<T, TResult>(this IEnumerable<T> e, Func<T, TResult> map)
        {
            foreach (var x in e)
                yield return map(x);
        }

        public static IEnumerable<T> Where<T>(this IEnumerable<T> e, Func<T, bool> filter)
        {
            foreach (var x in e)
                if (filter(x)) yield return x;
        }

        public static IEnumerable<T> Limit<T>(this IEnumerable<T> e, int count)
        {
            foreach (var x in e)
                if (--count >= 0) yield return x;
                else break;
        }

        public static IEnumerable<T> Skip<T>(this IEnumerable<T> e, int count)
        {
            foreach (var x in e)
                if (--count < 0) yield return x;
            // Same as:
            //using( var enumerator = e.GetEnumerator() )
            //{
            //    while( enumerator.MoveNext() )
            //    {
            //        if( --count < 0 ) yield return enumerator.Current;
            //    }
            //}
        }

        public static int Count(this IEnumerable e)
        {
            var enumerator = e.GetEnumerator();
            int count = 0;
            while (enumerator.MoveNext()) count++;
            return count;
        }
    }
}