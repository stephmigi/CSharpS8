using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    public class FromDelegateToLambda
    {
        [Test]
        public void DelegateUsageTest()
        {
            // Step 0
            long executionTimeInTicks = SimplePerf(1000, new TestPerformanceFunctionWithValue(DoSomething));

            // Step 1
            long executionTimeInTicks1 = SimplePerf(1000, DoSomething);

            //Step 2 : anonymous function
            long executionTimeInTicks2 = SimplePerf(1000, delegate( int counter)
            {
                return TimeSpan.Zero;
            });

            //Step 3 : lambda function
            long executionTimeInTicks3 = SimplePerf(1000, counter =>
            {
                return TimeSpan.Zero;
            });

            // action with no parameter, returns void
            TestPerformanceFunction noParameter = () => { };

            // one parameter : no () necessary
            TestPerformanceFunctionWithValue oneParameter = x => TimeSpan.Zero;

            // multiple parameters : () necessary
            TestPerformanceFunctionMultipleParams moreThanOneParameter = (x, s) => TimeSpan.Zero;
        }

        #region .NET 1.0
        [Test]
        public void WhatIsADelegate()
        {
            // Create an instance
            var testPerformanceDelegate = new TestPerformanceFunctionWithValue(DoSomething);

            // Call the instance 
            testPerformanceDelegate(3);

            var testPerformanceWithValueDelegate = 
                new TestPerformanceFunctionWithValue(DoSomethingElse);
            var span = testPerformanceWithValueDelegate(1);
        }
        #endregion

        #region next step
        [Test]
        public void WhatIsADelegate2()
        {
            TestPerformanceFunctionWithValue testPerformanceDelegate = DoSomething;
            TestPerformanceFunctionWithValue testPerformanceWithValueDelegate = DoSomethingElse;
            // Call the instance => same as 1.0
        }
        #endregion

        // This is a type definition
        private delegate void TestPerformanceFunction();
        private delegate TimeSpan TestPerformanceFunctionWithValue(int counter);
        private delegate TimeSpan TestPerformanceFunctionMultipleParams(int counter, int otherCounter);

        private long SimplePerf(int counter, TestPerformanceFunctionWithValue methodToExecute)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            methodToExecute(counter);
            
            sw.Stop();
            return sw.ElapsedTicks;
        }

        private TimeSpan DoSomething(int counter)
        {
            return TimeSpan.Zero;
        }

        private TimeSpan DoSomethingElse(int counter)
        {
            //code
            return TimeSpan.Zero;
        }
    }
}
