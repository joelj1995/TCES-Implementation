using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Interface;

namespace VMtranslator.Test.Hack
{
    internal class TestHackCodeWriterForStackOperations : TestHackCodeWriter
    {
        #region Argument
        [Test]
        public void TestWritePushArgument()
        {
            var lines = new string[]
            {
                "@4",
                "D=A",
                "@ARG",
                "A=M",
                "A=A+D",
                "D=M",
                "@SP",
                "A=M",
                "M=D",
                "D=A+1",
                "@SP",
                "M=D",
            };
            codeWriter.writePushPop(Core.Interface.CommandType.C_PUSH, "argument", 4);
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }

        [Test]
        public void TestWritePushPopForPopArgument()
        {
            var lines = new string[]
            {
                "@4",
                "D=A",
                "@ARG",
                "D=M+D",
                "@R13",
                "M=D",
                "@SP",
                "D=M-1",
                "M=D",
                "A=D",
                "D=M",
                "@R13",
                "A=M",
                "M=D"
            };
            codeWriter.writePushPop(Core.Interface.CommandType.C_POP, "argument", 4);
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }
        #endregion

        #region Constant
        [Test]
        public void TestWritePushConstant()
        {
            var lines = new string[]
            {
                "@8",    // A = 8
                "D=A",   // D = A = 8
                "@SP",   // A = 0
                "A=M",   // A = *A = 256
                "M=D",   // *A = D = 8
                "D=A+1", // D = A + 1 = 256 + 1 = 257
                "@SP",   // A = 0
                "M=D",   // *A = D = 257
            };
            codeWriter.writePushPop(Core.Interface.CommandType.C_PUSH, "constant", 8);
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }
        #endregion

        #region Pointer
        [Test]
        public void TestWritePushPopForPushPointer()
        {
            var lines = new string[]
            {
                "@THIS",
                "D=M",
                "@SP",
                "A=M",
                "M=D",
                "D=A+1",
                "@SP",
                "M=D",
            };
            codeWriter.writePushPop(Core.Interface.CommandType.C_PUSH, "pointer", 0);
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }

        [Test]
        public void TestWritePushPopForPopPointer()
        {
            var lines = new string[]
            {
                "@SP",
                "D=M-1",
                "M=D",
                "A=D",
                "D=M",
                "@THAT",
                "M=D"
            };
            codeWriter.writePushPop(Core.Interface.CommandType.C_POP, "pointer", 1);
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }
        #endregion

        #region Static
        [Test]
        public void TestWritePushPopForPushStatic()
        {
            var expected = cleanString(@"
                @Xxx.3
                D=M
                @SP
                A=M
                M=D
                D=A+1
                @SP
                M=D
            ");
            codeWriter.setFileName("Xxx");
            codeWriter.writePushPop(Core.Interface.CommandType.C_PUSH, "static", 3);
            Assert.That(GetWriterText(), Does.EndWith(expected));
        }

        [Test]
        public void TestWritePushPopForPopStatic()
        {
            var expected = cleanString(@"
                @SP
                D=M-1
                M=D
                A=D
                D=M
                @Xxx.3
                M=D
            ");
            codeWriter.setFileName("Xxx");
            codeWriter.writePushPop(Core.Interface.CommandType.C_POP, "static", 3);
            Assert.That(GetWriterText(), Does.EndWith(expected));
        }
        #endregion

        #region Temp
        [Test]
        public void TestWritePushPopForPushTemp()
        {
            var lines = new string[]
            {
                "@R5",
                "A=M",
                "D=A",
                "@SP",
                "A=M",
                "M=D",
                "D=A+1",
                "@SP",
                "M=D"
            };
            codeWriter.writePushPop(Core.Interface.CommandType.C_PUSH, "temp", 0);
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }

        [Test]
        public void TestWritePushPopForPopTemp()
        {
            var lines = new string[]
            {
                "@SP",
                "D=M-1",
                "M=D",
                "A=D",
                "D=M",
                "@R5",
                "M=D"
            };
            codeWriter.writePushPop(Core.Interface.CommandType.C_POP, "temp", 0);
            Assert.That(GetWriterText(), Does.EndWith(buildString(lines)));
        }
        #endregion
    }
}
