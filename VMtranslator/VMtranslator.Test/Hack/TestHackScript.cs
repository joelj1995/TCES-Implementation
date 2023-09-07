using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Hack;

namespace VMtranslator.Test.Hack
{
    public class TestHackScript
    {
        [Test]
        public void TestToString()
        {
            var commands = new IHackCommand[]
            {
                HackCommand.CommentF("test"),
                HackCommand.A("SP"),
                HackCommand.C("A", "!M", "JGE")
            };
            var script = HackScript.create(commands);
            var sb = new StringBuilder();
            sb.AppendLine("// test");
            sb.AppendLine("@SP");
            sb.AppendLine("A=!M;JGE");
            Assert.That(script.ToString, Is.EqualTo(sb.ToString()));
        }

        public void TestAdditionOperator()
        {
            var header = HackScript.create(HackCommand.CommentF("test"));
            var commands = new IHackCommand[]
            {
                HackCommand.A("SP"),
                HackCommand.C("A", "!M", "JGE")
            };
            var mainCode = HackScript.create(commands);
            var script = header + mainCode;
            var sb = new StringBuilder();
            sb.AppendLine("// test");
            sb.AppendLine("@SP");
            sb.AppendLine("A=!M;JGE");
            Assert.That(script.ToString, Is.EqualTo(sb.ToString()));
        }
    }
}
