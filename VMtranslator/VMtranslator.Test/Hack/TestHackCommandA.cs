using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Hack;

namespace VMtranslator.Test.Hack
{
    public class TestHackCommandA
    {
        [Test]
        public void TestEquals()
        {
            var aCommand = HackCommand.A("SP");
            var theSameCommand = HackCommand.A("SP");
            var aDifferentCommand = HackCommand.A("1");
            Assert.That(aCommand, Is.EqualTo(theSameCommand));
            Assert.That(aCommand, Is.Not.EqualTo(aDifferentCommand));
        }

        [Test]
        public void TestClone()
        {
            var aCommand = HackCommand.A("SP");
            var aCloneOfCommand = aCommand.clone();
            Assert.That(aCommand, Is.EqualTo(aCloneOfCommand));
        }

        [Test]
        public void TestEncode()
        {
            var addressCommandSP = HackCommand.A("SP");
            Assert.That(addressCommandSP.encode(), Is.EqualTo("@SP"));
        }
    }
}
