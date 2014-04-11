using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class StartingWithLinq
    {
        static IEnumerable<int> Numbers (int start, int count)
        {
            for (int i = start; i < count + start; i++)
            {
                yield return i;
            }
        }

        static IEnumerable<int> SawTeeth (int width)
        {
            while (true)
            {
                for (int i = 0; i < width; i++ )
                    yield return i;
            }
        }

        static IEnumerable<int> RandomNumbers(int seed)
        {
            var r = new Random(seed);
            for (;;) yield return r.Next();
        }

        static IEnumerable<T> Limit<T> (IEnumerable<T> enumerable, int limit)
        {
            foreach(var element in enumerable)
            {
                if (--limit >= 0) yield return element;
                else break;
            }
        }

        static IEnumerable<T> Skip<T>(IEnumerable<T> enumerable, int count)
        {
            foreach (var element in enumerable)
            {
                if (--count < 0) yield return element;
            }
        }

        static int Count<T>(IEnumerable<T> enumerable)
        {
            using (var enumerator = enumerable.GetEnumerator())
            {
                int count = 0;
                while (enumerator.MoveNext())
                    count++;
                return count;
            }
        }

        [Test]
        public void SimpleRanges()
        {
            CollectionAssert.AreEqual(Numbers(0, 5), new[] { 0, 1, 2, 3, 4 });
            int notTooMuch = 200;

            foreach (var i in SawTeeth(12))
            {
                Console.WriteLine(i);
                if (--notTooMuch == 0) break;
            }

            foreach (var i in Limit(SawTeeth(12), 25))
            {
                Console.WriteLine(i);
            }
        }

        [Test]
        public void SimpleRanges2()
        {
            CollectionAssert.AreEqual( Numbers( 0, 5 ), new[] { 0, 1, 2, 3, 4 } );

            int notTooMuch = 10;
            foreach( var i in SawTeeth( 12 ) )
            {
                Console.WriteLine( i );
                if( --notTooMuch == 0 ) break;
            }

            var noLimit = SawTeeth( 3 );
            var limited = MiniLinqExtension.Limit( noLimit, 9 );
            CollectionAssert.AreEqual( limited, new[] { 0, 1, 2, 0, 1, 2, 0, 1, 2 } );

            var onlyThe5Last = MiniLinqExtension.Skip( limited, 4 );
            CollectionAssert.AreEqual( onlyThe5Last, new[] { 1, 2, 0, 1, 2 } );

            var source = RandomNumbers( 154 );
            var the13ThatInterestMe = MiniLinqExtension.Limit( MiniLinqExtension.Skip( source, 20 ), 13 );

            var simpler = source.Skip( 20 ).Limit( 13 );
        }

        [Test]
        public void MiniLinqInAction()
        {
            var evenRandomNumbers = RandomNumbers( 125 ).Where( x => x % 2 == 0 );
            var simpler = RandomNumbers( 125 ).Select( x => x * 2.0 );
            var toString = RandomNumbers( 125 ).Select( x => x.ToString() );

            var complexSerie = RandomNumbers(12).Select( x => new { R = x, I = x * 2 } );

            var basedOnComplex = complexSerie
                                    .Where( c => c.I > 0 )
                                    .Limit( 50 );

            var linq = from x in RandomNumbers( 125 )
                       where x > 7
                       select x;

        }

    }
}
