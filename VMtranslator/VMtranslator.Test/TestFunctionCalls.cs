using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Test
{
    internal class TestFunctionCalls
    {
        [Test]
        public void TestSimpleFunction()
        {
            string result = Helpers.TryTranslate("SimpleFunction");
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void TestFibonacciElement()
        {
            string result = Helpers.TryTranslateMany("FibonacciElement");
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void TestStaticsTest()
        {
            string result = Helpers.TryTranslateMany("StaticsTest");
            Assert.That(result, Is.Not.Empty);
        }
    }
}
