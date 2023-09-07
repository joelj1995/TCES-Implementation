using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Interface;

namespace VMtranslator.Test.Hack
{
    internal class TestHackCodeWriterForProgramFlow : TestHackCodeWriter
    {
        

        [Test]
        public void TestWriteGoto()
        {
            var lines = new string[]
            {
                "@UP",
                "0;JMP"
            };
            codeWriter.writeGoto("UP");
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }

        [Test]
        public void TestWriteIf()
        {
            var lines = new string[]
            {
                // POP STACK
                "@SP",
                "D=M-1",
                "M=D",
                "A=D",
                "D=M",
                // JUMP TO LABEL IF NON-ZERO
                "@UP",
                "D;JNE"
            };
            codeWriter.writeIf("UP");
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }

        [Test]
        public void WriteLabel()
        {
            var label = "UP";
            codeWriter.writeLabel(label);
            Assert.That(GetWriterText(), Does.EndWith("(_UP_1)"));
        }
    }
}
