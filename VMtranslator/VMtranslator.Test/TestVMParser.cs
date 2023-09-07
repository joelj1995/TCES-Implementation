using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core;
using VMtranslator.Core.Hack;
using VMtranslator.Core.Interface;

namespace VMtranslator.Test
{
    public class TestVMParser
    {
        [Test]
        public void TestAdvanceAndHasMore()
        {
            var input = "\n\n\n";
            var inputStream = Helpers.GetStreamReaderForString(input);
            var parser = new VMParser(inputStream);
            Assert.IsTrue(parser.hasMoreCommands());
            parser.advance();
            Assert.IsTrue(parser.hasMoreCommands());
            parser.advance();
            Assert.IsTrue(parser.hasMoreCommands());
            parser.advance();
            Assert.IsFalse(parser.hasMoreCommands());
            Assert.Throws<InvalidOperationException>(() => parser.advance());
        }

        [Test]
        public void TestCommandType()
        {
            var sb = new StringBuilder();
            foreach (var command in VMConstants.arithmeticAndLogicCommands)
                sb.AppendLine(command);
            sb.AppendLine("push");
            sb.AppendLine("pop");
            sb.AppendLine("label LOOP_START");
            sb.AppendLine("goto END_PROGRAM");
            sb.AppendLine("if-goto LOOP_START");
            sb.AppendLine("function Main.fibonacci 0");
            sb.AppendLine("call Main.fibonacci 1");
            sb.AppendLine("return");
            var inputStream = Helpers.GetStreamReaderForString(sb.ToString());
            var parser = new VMParser(inputStream);
            foreach (var _ in VMConstants.arithmeticAndLogicCommands)
            {
                parser.advance(); Assert.That(parser.commandType(), Is.EqualTo(CommandType.C_ARITHMETIC));
            }
            parser.advance(); Assert.That(parser.commandType(), Is.EqualTo(CommandType.C_PUSH));
            parser.advance(); Assert.That(parser.commandType(), Is.EqualTo(CommandType.C_POP));
            parser.advance(); Assert.That(parser.commandType(), Is.EqualTo(CommandType.C_LABEL));
            parser.advance(); Assert.That(parser.commandType(), Is.EqualTo(CommandType.C_GOTO));
            parser.advance(); Assert.That(parser.commandType(), Is.EqualTo(CommandType.C_IF));
            parser.advance(); Assert.That(parser.commandType(), Is.EqualTo(CommandType.C_FUNCTION));
            parser.advance(); Assert.That(parser.commandType(), Is.EqualTo(CommandType.C_CALL));
            parser.advance(); Assert.That(parser.commandType(), Is.EqualTo(CommandType.C_RETURN));
        }

        [Test]
        public void TestCommandTypeForInvalidCommand()
        {
            Assert.Throws<SyntaxException>(() =>
            {
                var input = "foo 1";
                var inputStream = Helpers.GetStreamReaderForString(input);
                var parser = new VMParser(inputStream);
                parser.advance();
                parser.commandType();
            });
        }

        [Test]
        public void TestArg1()
        {
            var sb = new StringBuilder();
            sb.AppendLine("gt");
            sb.AppendLine("push constant 32767");
            var inputStream = Helpers.GetStreamReaderForString(sb.ToString());
            var parser = new VMParser(inputStream);
            parser.advance();
            Assert.That(parser.arg1(), Is.EqualTo("gt"));
            parser.advance();
            Assert.That(parser.arg1(), Is.EqualTo("constant"));
        }

        [Test]
        public void TestArg2()
        {
            var inputStream = Helpers.GetStreamReaderForString("push constant 32767");
            var parser = new VMParser(inputStream);
            parser.advance();
            Assert.That(parser.arg2(), Is.EqualTo(32767));
        }
    }
}
