using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Test.Hack
{
    internal class TestHackCodeWriterForUnaryOperations : TestHackCodeWriter
    {
        [Test]
        public void TestWriteArithmeticForNeg()
        {
            var lines = new string[]
            {
                "@SP",
                "A=M-1",
                "D=-M",
                "M=D"
            };
            codeWriter.writeArithmetic("neg");
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }

        [Test]
        public void TestWriteArithmeticForNot()
        {
            var lines = new string[]
            {
                "@SP",
                "A=M-1",
                "D=!M",
                "M=D"
            };
            codeWriter.writeArithmetic("not");
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }
    }
}
