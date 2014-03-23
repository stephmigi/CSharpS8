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
            bool anExceptionHasBeenCaught = false;
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
            // code can pass through here because an exception of
            // another type could have been caught
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Code should never pass through here.");
            }
            // call this code wether an exception has been 
            // caught or not
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
        /// Measures the time taken to perform a parse test 
        /// on a string with a given parsing method
        /// The test is executed NB_LOOPS times
        /// </summary>
        /// <param name="stringToParse">the string to parse</param>
        /// <param name="parseMethod">the method to use</param>
        /// <returns>The test's execution time in ticks</returns>
        private long PerformParseTest(string stringToParse, Action<string> parseMethod)
        {
            return TestHelpers.TimeFunctionExecution(NB_LOOPS, () => parseMethod(stringToParse) );
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

        #endregion

        #region String builder complexity & performance test

        // Compare a "naive" string build (with concatenation) with a 
        // "better" String build (with a string builder)
        [Test]
        public void BuildStringPerformanceTest()
        {
            //number of times pattern is appended
            int count = 1000;
            string pattern = "abcd";

            // number of times test is run
            int testRepetition = 1000;

            long naiveStringBuildTime = PerformStringBuild(BuildNaiveString, testRepetition, count, pattern);
            long betterStringBuildTime = PerformStringBuild(BuildBetterString, testRepetition, count, pattern);

            Console.WriteLine("Naive build string time : " + naiveStringBuildTime);
            Console.WriteLine("Better build string time : " + betterStringBuildTime);
            Console.WriteLine("Ratio : " + naiveStringBuildTime / betterStringBuildTime);
        }


        [Test]
        public void BuildStringComplexityTest()
        {
            int count = 1000;
            string pattern = "abcd";

            // run test multiple times 500 times, then 1000, then 2000... to see 
            // the data evolution
            int testRepetition = 0;
            while (testRepetition < 3000)
            {
                testRepetition += 500;

                long naiveStringBuildTime = PerformStringBuild(BuildNaiveString, testRepetition, count, pattern);
                long betterStringBuildTime = PerformStringBuild(BuildBetterString, testRepetition, count, pattern);

                Console.WriteLine("n = {0}, Naive : {1}, Better : {2}", 
                    testRepetition, 
                    naiveStringBuildTime, 
                    betterStringBuildTime);
            } 
        }

        /// <summary>
        /// Measure the time taken to build a string 
        /// </summary>
        /// <param name="buildingMethod">A building function to use</param>
        /// <param name="testCount">Number of times the test has to be executed</param>
        /// <param name="count">Number of times the pattern has to be appended</param>
        /// <param name="pattern">The pattern used</param>
        /// <returns>The elapsed time in ticks</returns>
        private long PerformStringBuild(Func<string, int, string> buildingMethod, int testCount, int count, string pattern)
        {
            return TestHelpers.TimeFunctionExecution(testCount, () => buildingMethod(pattern, count));
        }

        /// <summary>
        /// Build a string with simple concatenation
        /// </summary>
        /// <param name="pattern">The pattern to append</param>
        /// <param name="count">The number of times to append the pattern</param>
        /// <returns>The newly built string</returns>
        private string BuildNaiveString(string pattern, int count)
        {
            string s = string.Empty;
            while (--count > 0)
            {
                s += pattern;
            }
            return s;
        }

        /// <summary>
        /// Build a new string with a StringBuilder
        /// </summary>
        /// <param name="pattern">The pattern to append</param>
        /// <param name="count">The number of times to append the pattern</param>
        /// <returns>The newly built string</returns>
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
