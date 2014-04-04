using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    [TestFixture]
    class ITIDictionnaryTests
    {
        class StringInsensitive : IDicStrat<string>
        {
            public bool IsItEqual (string key1, string key2)
            {
                return StringComparer.OrdinalIgnoreCase.Equals(key1, key2);
            }    

            public int ComputeHashCode (string key)
            {
                return StringComparer.OrdinalIgnoreCase.GetHashCode(key);
            }
        }

        [Test]
        public void ourDictionaryTestIsNotPerfect()
        {
            IDicStrat<string> caseInsensitiveStrat = new StringInsensitive();
            ITIDictionary<string, int> dic = new ITIDictionary<string, int>(caseInsensitiveStrat);

            dic.Add("Paul", 435);
            Assert.That(dic["Paul"] == 435);

            Assert.That(dic["paul"] == 435);
        }
    }
}
