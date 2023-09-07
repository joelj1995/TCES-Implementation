using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Test
{
    internal class TestHelpers
    {
        [Test]
        public void SanityTestXmlComparisonGood()
        {
            var xml1 = @"
                <tokens>
                  <keyword> if </keyword>
                  <symbol> ( </symbol>
                </tokens>
            ";
            var xml2 = @"
                <tokens><keyword> if </keyword><symbol> ( </symbol></tokens>
            ";
            Helpers.AssertXmlEqual(xml1, xml2);
        }

        [Test]
        public void SanityTestXmlComparisonBad()
        {
            var xml1 = @"
                <tokens>
                  <symbol> ( </symbol>
                  <keyword> if </keyword>
                </tokens>
            ";
            var xml2 = @"
                <tokens><keyword> if </keyword><symbol> ( </symbol></tokens>
            ";
            Assert.Throws<AssertionException>(() =>
            {
                Helpers.AssertXmlEqual(xml1, xml2);
            });
        }
    }
}
