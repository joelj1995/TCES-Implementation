using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Hack;

namespace VMtranslator.Test.Hack
{
    public class TestHackCommandC
    {
        [Test]
        public void TestEquals()
        {
            var aCommand = HackCommand.C("A", "!M", "JMP");
            var theSameCommand = HackCommand.C("A", "!M", "JMP");
            var aDifferentCommand = HackCommand.C("A", "!M", "JGE");
            Assert.That(aCommand, Is.EqualTo(theSameCommand));
            Assert.That(aCommand, Is.Not.EqualTo(aDifferentCommand));
        }

        [Test]
        public void TestClone()
        {
            var aCommand = HackCommand.C("A", "!M", "JMP");
            var aCloneOfCommand = aCommand.clone();
            Assert.That(aCommand, Is.EqualTo(aCloneOfCommand));
        }

        [Test]
        public void TestEncode()
        {
            var computeCommandAllComponents = HackCommand.C("M", "D+A", "JNE");
            Assert.That(computeCommandAllComponents.encode(), Is.EqualTo("M=D+A;JNE"));
            var computeCommandNoJump = HackCommand.C("M", "D+A", null);
            Assert.That(computeCommandNoJump.encode(), Is.EqualTo("M=D+A"));
            var computeCommandNoDest = HackCommand.C(null, "D+A", "JNE");
            Assert.That(computeCommandNoDest.encode(), Is.EqualTo("D+A;JNE"));
            var computeCommandJustComp = HackCommand.C(null, "D+A", null);
            Assert.That(computeCommandJustComp.encode(), Is.EqualTo("D+A"));
        }
    }
}
