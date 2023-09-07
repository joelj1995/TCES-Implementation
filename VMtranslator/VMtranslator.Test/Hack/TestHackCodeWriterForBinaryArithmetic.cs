using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Interface;

namespace VMtranslator.Test.Hack
{
    internal class TestHackCodeWriterForBinaryArithmetic : TestHackCodeWriter
    {
        [Test]
        public void TestWriteArithmeticForAdd()
        {
            var lines = new string[]
            {
                "@SP",
                "D=M-1",
                "M=D",
                "A=M",
                "D=M",
                "A=A-1",
                "A=M",
                "D=A+D",
                "@SP",
                "A=M-1",
                "M=D"
            };
            codeWriter.writeArithmetic("add");
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }

        [Test]
        public void TestWriteArithmeticForAnd()
        {
            var lines = new string[]
            {
                "@SP",
                "D=M-1",
                "M=D",
                "A=M",
                "D=M",
                "A=A-1",
                "A=M",
                "D=A&D",
                "@SP",
                "A=M-1",
                "M=D"
            };
            codeWriter.writeArithmetic("and");
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }

        [Test]
        public void TestWriteArithmeticForOr()
        {
            var lines = new string[]
            {
                "@SP",
                "D=M-1",
                "M=D",
                "A=M",
                "D=M",
                "A=A-1",
                "A=M",
                "D=A|D",
                "@SP",
                "A=M-1",
                "M=D"
            };
            codeWriter.writeArithmetic("or");
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }

        [Test]
        public void TestWriteArithmeticForSub()
        {
            var lines = new string[]
            {
                "@SP",
                "D=M-1",
                "M=D",
                "A=M",
                "D=M",
                "A=A-1",
                "A=M",
                "D=A-D",
                "@SP",
                "A=M-1",
                "M=D"
            };
            codeWriter.writeArithmetic("sub");
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }
    }
}
