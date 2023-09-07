using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Test
{
    public class TestStackArithmetic
    {
        [Test]
        public void TestSimpleAdd()
        {
            string result = Helpers.TryTranslate("SimpleAdd");
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void TestStackTest()
        {
            string result = Helpers.TryTranslate("StackTest");
            Assert.That(result, Is.Not.Empty);
        }
    }
}
