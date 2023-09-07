using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Hack;

namespace VMtranslator.Test.Hack
{
    public class TestHackCommandL
    {
        [Test]
        public void TestEquals()
        {
            var aCommand = HackCommand.L("LOCATION");
            var theSameCommand = HackCommand.L("LOCATION");
            var aDifferentCommand = HackCommand.L("1");
            Assert.That(aCommand, Is.EqualTo(theSameCommand));
            Assert.That(aCommand, Is.Not.EqualTo(aDifferentCommand));
        }

        [Test]
        public void TestClone()
        {
            var aCommand = HackCommand.L("LOCATION");
            var aCloneOfCommand = aCommand.clone();
            Assert.That(aCommand, Is.EqualTo(aCloneOfCommand));
        }

        [Test]
        public void TestEncode()
        {
            var addressCommandLOCATION = HackCommand.L("LOCATION");
            Assert.That(addressCommandLOCATION.encode(), Is.EqualTo("(LOCATION)"));
        }
    }
}
