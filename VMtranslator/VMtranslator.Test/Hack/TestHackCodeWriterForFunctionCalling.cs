using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Test.Hack
{
    internal class TestHackCodeWriterForFunctionCalling : TestHackCodeWriter
    {
        [Test]
        public void TestWriteCall()
        {
            var expected = @"
                @_RET_1
                D=A
                @SP
                A=M
                M=D
                D=A+1
                @SP
                M=D
                // PUSH LCL
                @LCL
                A=M
                D=A
                @SP
                A=M
                M=D
                D=A+1
                @SP
                M=D
                // PUSH ARG
                @ARG
                A=M
                D=A
                @SP
                A=M
                M=D
                D=A+1
                @SP
                M=D
                // PUSH THIS
                @THIS
                A=M
                D=A
                @SP
                A=M
                M=D
                D=A+1
                @SP
                M=D
                // PUSH THAT
                @THAT
                A=M
                D=A
                @SP
                A=M
                M=D
                D=A+1
                @SP
                M=D
                // REPOSITION ARG
                // ARG = SP-n-5 = SP - 2 - 5
                @7
                D=A
                @SP
                D=M-D
                @ARG
                M=D
                // REPOSITION LCL
                // LCL = SP
                @SP
                D=M
                @LCL
                M=D
                // GOTO F
                @FLOO
                0;JMP
                (_RET_1)
            ";
            codeWriter.writeCall("FLOO", 2);
            Assert.That(GetWriterText(), Does.EndWith(cleanString(expected)));
        }

        [Test]
        public void TestWriteFunction()
        {
            var expected = @"
                (FLOO)
                // init local 0
                @0
                D=A
                @SP
                A=M
                M=D
                D=A+1
                @SP
                M=D
                // init local 1
                @0
                D=A
                @SP
                A=M
                M=D
                D=A+1
                @SP
                M=D
            ";
            codeWriter.writeFunction("FLOO", 2);
            Assert.That(GetWriterText(), Does.EndWith(cleanString(expected)));
        }

        [Test]
        public void TestWriteReturn()
        {
            var expected = @"
                // FRAME = LCL
                @LCL
                D=M
                @R13
                M=D
                // RET = *(FRAME-5)
                @5
                D=A
                @R13
                A=M-D
                D=M
                @R14
                M=D
                // *ARG = pop()
                // The idea is to have the return value (42) at the top of the stack after returning
                @SP
                D=M-1
                M=D
                A=D
                D=M
                @ARG
                A=M
                M=D
                // SP=ARG+1
                // restore stack pointer
                @ARG
                D=M
                @SP
                M=D+1
                // THAT= *(FRAME-1)
                @R13
                M=M-1
                A=M
                D=M
                @THAT
                M=D
                // THIS= *(FRAME-2)
                @R13
                M=M-1
                A=M
                D=M
                @THIS
                M=D
                // ARG= *(FRAME-3)
                @R13
                M=M-1
                A=M
                D=M
                @ARG
                M=D
                // LCL= *(FRAME-4)
                @R13
                M=M-1
                A=M
                D=M
                @LCL
                M=D
                // goto RET
                @R14
                A=M
                0;JMP
            ";
            codeWriter.writeReturn();
            var text = GetWriterText();
            var exepectedText = cleanString(expected);
            Assert.That(text, Does.EndWith(exepectedText));
        }
    }
}
