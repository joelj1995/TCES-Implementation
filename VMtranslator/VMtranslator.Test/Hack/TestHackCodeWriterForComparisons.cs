using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Interface;

namespace VMtranslator.Test.Hack
{
    internal class TestHackCodeWriterForComparisons : TestHackCodeWriter
    {
        [Test]
        public void TestWriteArithmeticForEq()
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
                "@YES_1",
                "D;JEQ",
                "@0",
                "D=A",
                "@EXIT_2",
                "0;JMP",
                "(YES_1)",
                "@1",
                "A=-A",
                "D=A",
                "(EXIT_2)",
                "@SP",
                "A=M-1",
                "M=D"
            };
            codeWriter.writeArithmetic("eq");
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }

        [Test]
        public void TestWriteArithmeticForGt()
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
                "@YES_1",
                "D;JGT",
                "@0",
                "D=A",
                "@EXIT_2",
                "0;JMP",
                "(YES_1)",
                "@1",
                "A=-A",
                "D=A",
                "(EXIT_2)",
                "@SP",
                "A=M-1",
                "M=D"
            };
            codeWriter.writeArithmetic("gt");
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }

        [Test]
        public void TestWriteArithmeticForLt()
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
                "@YES_1",
                "D;JLT",
                "@0",
                "D=A",
                "@EXIT_2",
                "0;JMP",
                "(YES_1)",
                "@1",
                "A=-A",
                "D=A",
                "(EXIT_2)",
                "@SP",
                "A=M-1",
                "M=D"
            };
            codeWriter.writeArithmetic("lt");
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }
    }
}
