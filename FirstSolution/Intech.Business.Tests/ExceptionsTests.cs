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
            string incorrect = "kzejgopzejgpozegpoze";
            string correct = "100";

            long normalWithExceptions = PerformParseTest(correct, ParseIntWithExceptions);
            long normalWithOutExceptions = PerformParseTest(correct, ParseIntWithoutExceptions);

            long exceptionWithExceptions = PerformParseTest(incorrect, ParseIntWithExceptions);
            long exceptionWithOutExceptions = PerformParseTest(incorrect, ParseIntWithoutExceptions);

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

        /// <summary>
        /// Performs a parse test on a string with a given parsing method
        /// The test is executed NB_LOOPS times
        /// </summary>
        /// <param name="stringToParse">the string to parse</param>
        /// <param name="parseMethod">the method to use</param>
        /// <returns>The test's execution time in ticks</returns>
        private long PerformParseTest(string stringToParse, Action<string> parseMethod)
        {
            return TimeFunctionExecution(NB_LOOPS, () => parseMethod(stringToParse) );
        }

        /// <summary>
        /// Parses a string with Int32.Parse
        /// </summary>
        /// <param name="toParse">String to parse</param>
        private void ParseIntWithExceptions(string toParse)
        {
            try
            {
                int result = Int32.Parse(toParse);
            }
            catch { }
        }

        /// <summary>
        /// Parse a string with Int32.TryParse
        /// </summary>
        /// <param name="toParse">String to parse</param>
        private void ParseIntWithoutExceptions(string toParse)
        {
            int result;
            Int32.TryParse(toParse, out result);
        }

        /// <summary>
        /// Counts the time in ticks of a function's execution
        /// executed n times
        /// </summary>
        /// <param name="loops">Number of times the method is executed</param>
        /// <param name="method">Method to be executed</param>
        /// <returns>Total executing time in ticks</returns>
        private long TimeFunctionExecution(int loops, Action method)
        {
            Stopwatch w = new Stopwatch();
            w.Start();

            for (int i = 0; i < loops; ++i)
            {
                method();
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
