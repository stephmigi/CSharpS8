using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    class TestHelpers
    {
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
    }
}
