using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class ExceptionsTests
    {
        #region Exception basics test

        [Test]
        public void SimpleScenario()
        {
            bool anExceptionHasBeenCaught = false; ;
            _secondMethodHasBeenCalled = false;
            try
            {
                ContainerMethod();
            }

            // prevents compilation because next catches are more specific
            //catch (Exception ex)
            //{
            //    Assert.Fail(" Code should never pass through here. ");
            //}
            catch (InvalidOperationException ex)
            {
                Assert.That(ex, Is.Not.Null);
                anExceptionHasBeenCaught = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Code should never pass through here.");
            }
            finally
            {
                Assert.That(_secondMethodHasBeenCalled, Is.False);
                Assert.That(anExceptionHasBeenCaught, Is.True);
            }
        }

        bool _secondMethodHasBeenCalled;

        private void ContainerMethod()
        {
            try
            {
                FirstMethod();
                SecondMethod();
            }
            catch (Exception ex)
            {
                // log exception
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private void FirstMethod()
        {
            throw new InvalidOperationException();
        }

        private void SecondMethod()
        {
            _secondMethodHasBeenCalled = true;
        }

        #endregion

        #region Exception Performance test

        [Test]
        public void ParseIntPerformanceTest()
        {
            string stringToParse = "kzejgopzejgpozegpoze";

            long normalWithExceptions = PerformParseTest("100", ParseIntWithExceptions);
            long normalWithOutExceptions = PerformParseTest("100", ParseIntWithoutExceptions);

            long exceptionWithExceptions = PerformParseTest(stringToParse, ParseIntWithExceptions);
            long exceptionWithOutExceptions = PerformParseTest(stringToParse, ParseIntWithoutExceptions);

            Console.WriteLine("Normal case : ");
            Console.WriteLine("Parse with exceptions case : " + normalWithExceptions);
            Console.WriteLine("Parse without exceptions case : " + normalWithOutExceptions);
            Console.WriteLine("Ratio : " + normalWithExceptions / normalWithOutExceptions);

            Console.WriteLine("Exception case : ");
            Console.WriteLine("Parse with exceptions case : " + exceptionWithExceptions);
            Console.WriteLine("Parse without exceptions case : " + exceptionWithOutExceptions);
            Console.WriteLine("Ratio : " + exceptionWithExceptions / exceptionWithOutExceptions);
        }

        private const int NB_LOOPS = 100;

        private long PerformParseTest(string stringToParse, Func<Stopwatch, int, string, long> parseMethod)
        {
            return parseMethod(new Stopwatch(), NB_LOOPS, stringToParse);
        }

        private long ParseIntWithExceptions(Stopwatch w, int loops, string stringToParse)
        {
            w.Start();
            for (int i = 0; i < loops; ++i)
            {
                try
                {
                    int result = Int32.Parse(stringToParse);
                }
                catch { }
            }
            w.Stop();
            return w.ElapsedTicks;
        }

        private long ParseIntWithoutExceptions(Stopwatch w, int loops, string stringToParse)
        {
            w.Start();
            for (int i = 0; i < loops; ++i)
            {
                int result;
                Int32.TryParse(stringToParse, out result);
            }
            w.Stop();
            return w.ElapsedTicks;
        }
        #endregion

        #region String builder complexity & performance test

        [Test]
        public void BuildStringPerformanceTest()
        {
            int testRepetition = 1000;
            int count = 1000;
            string pattern = "abcd";
            var sw = new Stopwatch();

            long naiveStringBuildTime = PerformNaiveStringBuild(testRepetition, count, pattern, sw);
            long betterStringBuildTime = PerformBetterStringBuild(testRepetition, count, pattern, sw);

            Console.WriteLine("Naive build string time : " + naiveStringBuildTime);
            Console.WriteLine("Better build string time : " + betterStringBuildTime);
            Console.WriteLine("Ratio : " + naiveStringBuildTime / betterStringBuildTime);
        }

        [Test]
        public void BuildStringComplexityTest()
        {
            int testRepetition = 0;
            int count = 100;
            string pattern = "abcd";
            var sw = new Stopwatch();

            while (testRepetition < 10000)
            {
                testRepetition += 100;
                long naiveStringBuildTime = PerformNaiveStringBuild(testRepetition, count, pattern, sw);
                long betterStringBuildTime = PerformBetterStringBuild(testRepetition, count, pattern, sw);
                Console.WriteLine("n = {0}, Naive : {1}, Better : {2}", 
                    testRepetition, 
                    naiveStringBuildTime, 
                    betterStringBuildTime);
            } 
        }

        private long PerformBetterStringBuild(int testCount, int count, string pattern, Stopwatch sw)
        {
            sw.Restart();
            sw.Start();

            for (int i = 0; i < testCount; i++)
            {
                BuildBetterString(pattern, count);
            }
            sw.Stop();

            return sw.ElapsedTicks;
        }

        private long PerformNaiveStringBuild(int testCount, int count, string pattern, Stopwatch sw)
        {
            sw.Restart();
            sw.Start();
            for (int i = 0; i < testCount; i++)
            {
                BuildNaiveString(pattern, count);
            }
            sw.Stop();

            return sw.ElapsedTicks;
        }

        private string BuildNaiveString(string pattern, int count)
        {
            string s = string.Empty;
            while (--count > 0)
            {
                s += pattern;
            }
            return s;
        }

        private string BuildBetterString(string pattern, int count)
        {
            StringBuilder sb = new StringBuilder();
            while (--count > 0)
            {
                sb.Append(pattern);
            }
            return sb.ToString();
        }
    }

    #endregion
}
