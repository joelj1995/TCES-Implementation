using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Test
{
    public class TestMemoryAccess
    {
        [Test]
        public void TestBasicTest()
        {
            string result = Helpers.TryTranslate("BasicTest");
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void TestPointerTest()
        {
            string result = Helpers.TryTranslate("PointerTest");
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void TestStaticTest()
        {
            string result = Helpers.TryTranslate("StaticTest");
            Assert.That(result, Is.Not.Empty);
        }
    }
}
