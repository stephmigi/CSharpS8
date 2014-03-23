using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    public class TestHelpers
    {

        #region properties

        private static DirectoryInfo _solutionFolder;
        public static DirectoryInfo SolutionFolder
        {
            get
            {
                if (_solutionFolder == null)
                {
                    _solutionFolder = new DirectoryInfo(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(
                                Path.GetDirectoryName(
                                    Path.GetDirectoryName(
                                        new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath)))));
                }
                return _solutionFolder;

            }
        }

        private static DirectoryInfo _testSupportFolder;
        public static DirectoryInfo TestSupportFolder
        {
            get
            {
                if (_testSupportFolder == null)
                {
                    _testSupportFolder = new DirectoryInfo(Path.Combine(SolutionFolder.FullName, "Intech.BusinessTests", "TestSupport"));
                }
                return _testSupportFolder;
            }
        }

        #endregion

        #region methods 

        /// <summary>
        /// Counts the time in ticks of a function's execution
        /// executed n times.
        /// </summary>
        /// <param name="loops">Number of times the method is executed</param>
        /// <param name="method">Method to be executed</param>
        /// <returns>Total executing time in ticks</returns>
        public static long TimeFunctionExecution(int loops, Action method)
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
    }
}
