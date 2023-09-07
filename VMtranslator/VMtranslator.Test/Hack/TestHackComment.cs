using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Hack;

namespace VMtranslator.Test.Hack
{
    public class TestHackComment
    {
        [Test]
        public void TestEquals()
        {
            var aCommand = HackCommand.Comment("This is good");
            var theSameCommand = HackCommand.Comment("This is good");
            var aDifferentCommand = HackCommand.Comment("This is bad");
            Assert.That(aCommand, Is.EqualTo(theSameCommand));
            Assert.That(aCommand, Is.Not.EqualTo(aDifferentCommand));
        }

        [Test]
        public void TestClone()
        {
            var aCommand = HackCommand.Comment("This is good");
            var aCloneOfCommand = aCommand.clone();
            Assert.That(aCommand, Is.EqualTo(aCloneOfCommand));
        }

        [Test]
        public void TestEncode()
        {
            var addressCommandSP = HackCommand.Comment("This is good");
            Assert.That(addressCommandSP.encode(), Is.EqualTo("//This is good"));
        }
    }
}
