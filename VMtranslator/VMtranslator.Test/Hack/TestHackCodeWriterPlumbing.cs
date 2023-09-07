using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Hack;

namespace VMtranslator.Test.Hack
{
    internal class TestHackCodeWriterPlumbing : TestHackCodeWriter
    {
        [Test]
        public void TestSetFileName()
        {
            // I'm not sure why this is needed yet but maybe
            // a purpose will become clear later
            codeWriter.setFileName("Test");
        }

        [Test]
        public void TestClose()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                codeWriter.close();
            });
        }

        [Test]
        public void TestWriteInit()
        {
            var expected = cleanString(@"
                // HackCodeGeneratorForHeader
                @256
                D=A
                @SP
                M=D
                // HackCodeGeneratorForFunctionCall
                @RET_1
                D=A
                @SP
                A=M
                M=D
                D=A+1
                @SP
                M=D
                @LCL
                A=M
                D=A
                @SP
                A=M
                M=D
                D=A+1
                @SP
                M=D
                @ARG
                A=M
                D=A
                @SP
                A=M
                M=D
                D=A+1
                @SP
                M=D
                @THIS
                A=M
                D=A
                @SP
                A=M
                M=D
                D=A+1
                @SP
                M=D
                @THAT
                A=M
                D=A
                @SP
                A=M
                M=D
                D=A+1
                @SP
                M=D
                @5
                D=A
                @SP
                D=M-D
                @ARG
                M=D
                @SP
                D=M
                @LCL
                M=D
                @Sys.init
                0;JMP
                (RET_1)
            ");
            codeWriter.writeInit();
            Assert.That(GetWriterText(), Does.EndWith(expected));
        }
    }
}
