using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Test
{
    internal class TestProgramFlow
    {
        [Test]
        public void TestBasicLoop()
        {
            string result = Helpers.TryTranslate("BasicLoop");
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void TestFibonacciSeries()
        {
            string result = Helpers.TryTranslate("FibonacciSeries");
            Assert.That(result, Is.Not.Empty);
        }
    }
}
