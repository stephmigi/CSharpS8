using Intech.Business;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class FirstUnitTest
    {
        [Test]
        public void FirstTest()
        {
            // arrange
            int a = 1;
            int b = 3;
            int sum;

            // act
            sum = a + b;

            // assert 
            Assert.That(sum > a && sum > b);

            WhereAmI();
        }

        public void WhereAmI()
        {
            DirectoryInfo d = TestHelpers.SolutionFolder;
        }
    }
}
