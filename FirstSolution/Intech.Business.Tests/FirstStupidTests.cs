using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class FirstStupidTests
    {
        [Test]
        public void StupidTestIsAlwaysOK()
        {
            // Arrange
            int x1 = 1;
            int x2 = 3;
            int sum;
            // Act
            sum = x1 + x2;
            // Assert
            Assert.That( sum > x1 && sum > x2 );
        }

        [Test]
        public void WhereAmI()
        {
            DirectoryInfo s = TestHelpers.SolutionFolder;
            Assert.That( s.Exists );
        }

    }
}
