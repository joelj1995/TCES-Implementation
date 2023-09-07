using Assembler.Logic;
using Assembler.Logic.Hack;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Test.Hack
{
    public class TestHackParser
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestSpecForCCommandComponentDomains()
        {
            Assert.That(HackParser.Dests.Length, Is.EqualTo(8));
            Assert.That(HackParser.Comps.Length, Is.EqualTo(28));
            Assert.That(HackParser.Jumps.Length, Is.EqualTo(8));
        }

        [Test]
        public void TestHasMoreCommands()
        {
            var inputStream = Helpers.GetStreamReaderForString("1\n2\n3\n");
            var parser = new HackParser(inputStream);
            Assert.That(parser.hasMoreCommands());
            inputStream.ReadToEnd();
            Assert.That(!parser.hasMoreCommands());
        }

        [Test]
        public void TestAdvance()
        {
            var inputStream = Helpers.GetStreamReaderForString("1\n2\n3\n");
            var parser = new HackParser(inputStream);
            Assert.That(parser.hasMoreCommands());
            parser.advance();
            Assert.That(parser.hasMoreCommands());
            parser.advance();
            Assert.That(parser.hasMoreCommands());
            parser.advance();
            Assert.That(!parser.hasMoreCommands());
        }

        [Test]
        public void TestCommandType()
        {
            var inputStream = Helpers.GetStreamReaderForString("(OUTPUT_FIRST)\n\tD=M\n\t@0");
            var parser = new HackParser(inputStream);
            parser.advance();
            Assert.That(parser.commandType(), Is.EqualTo(CommandType.L_COMMAND));
            parser.advance();
            Assert.That(parser.commandType(), Is.EqualTo(CommandType.C_COMMAND));
            parser.advance();
            Assert.That(parser.commandType(), Is.EqualTo(CommandType.A_COMMAND));
        }

        [Test]
        public void TestSymbolForL()
        {
            var inputStream = Helpers.GetStreamReaderForString("\t\t(OUTPUT_FIRST)\t\n");
            var parser = new HackParser(inputStream);
            parser.advance();
            Assert.That(parser.symbol(), Is.EqualTo("OUTPUT_FIRST"));
        }

        [Test]
        public void TestSymbolForAConstant()
        {
            var inputStream = Helpers.GetStreamReaderForString("\t\t@7\t\n");
            var parser = new HackParser(inputStream);
            parser.advance();
            Assert.That(parser.symbol(), Is.EqualTo("7"));
        }

        [Test]
        public void TestSymbolForANamed()
        {
            var inputStream = Helpers.GetStreamReaderForString("\t\t@SOME_NAME\t\n");
            var parser = new HackParser(inputStream);
            parser.advance();
            Assert.That(parser.symbol(), Is.EqualTo("SOME_NAME"));
        }

        [Test]
        public void TestDest()
        {
            var destinations = new string[] 
            {
                "0;JGT",
                "M=D-1;JGT",
                "D=M",
                "MD=0;JGT",
                "A=0;JGT",
                "AM=0",
                "AD=0;JGT",
                "AMD=0;JGT"
            };
            var inputStream = Helpers.GetStreamReaderForString(String.Join("\n", destinations));
            var parser = new HackParser(inputStream);
            parser.advance();
            Assert.That(parser.dest(), Is.EqualTo(null));
            Assert.That(parser.userSpace(), Is.EqualTo(true));
            parser.advance();
            Assert.That(parser.dest(), Is.EqualTo("M"));
            Assert.That(parser.userSpace(), Is.EqualTo(true));
            parser.advance();
            Assert.That(parser.dest(), Is.EqualTo("D"));
            Assert.That(parser.userSpace(), Is.EqualTo(true));
            parser.advance();
            Assert.That(parser.dest(), Is.EqualTo("MD"));
            Assert.That(parser.userSpace(), Is.EqualTo(true));
            parser.advance();
            Assert.That(parser.dest(), Is.EqualTo("A"));
            Assert.That(parser.userSpace(), Is.EqualTo(true));
            parser.advance();
            Assert.That(parser.dest(), Is.EqualTo("AM"));
            Assert.That(parser.userSpace(), Is.EqualTo(true));
            parser.advance();
            Assert.That(parser.dest(), Is.EqualTo("AD"));
            Assert.That(parser.userSpace(), Is.EqualTo(true));
            parser.advance();
            Assert.That(parser.dest(), Is.EqualTo("AMD"));
            Assert.That(parser.userSpace(), Is.EqualTo(true));
        }

        public void TestDestKernelSpace()
        {
            var destinations = new string[]
            {
                "0;JGT!",
                "M=D-1;JGT!",
                "D=M!",
                "MD=0;JGT!",
                "A=0;JGT!",
                "AM=0!",
                "AD=0;JGT!",
                "AMD=0;JGT!"
            };
            var inputStream = Helpers.GetStreamReaderForString(String.Join("\n", destinations));
            var parser = new HackParser(inputStream);
            parser.advance();
            Assert.That(parser.dest(), Is.EqualTo(null));
            Assert.That(parser.userSpace(), Is.EqualTo(false));
            parser.advance();
            Assert.That(parser.dest(), Is.EqualTo("M"));
            Assert.That(parser.userSpace(), Is.EqualTo(false));
            parser.advance();
            Assert.That(parser.dest(), Is.EqualTo("D"));
            Assert.That(parser.userSpace(), Is.EqualTo(false));
            parser.advance();
            Assert.That(parser.dest(), Is.EqualTo("MD"));
            Assert.That(parser.userSpace(), Is.EqualTo(false));
            parser.advance();
            Assert.That(parser.dest(), Is.EqualTo("A"));
            Assert.That(parser.userSpace(), Is.EqualTo(false));
            parser.advance();
            Assert.That(parser.dest(), Is.EqualTo("AM"));
            Assert.That(parser.userSpace(), Is.EqualTo(false));
            parser.advance();
            Assert.That(parser.dest(), Is.EqualTo("AD"));
            Assert.That(parser.userSpace(), Is.EqualTo(true));
            parser.advance();
            Assert.That(parser.dest(), Is.EqualTo("AMD"));
            Assert.That(parser.userSpace(), Is.EqualTo(true));
        }

        [Test]
        public void TestInvalidDest()
        {
            Assert.Throws<ParserException>(() =>
            {
                var inputStream = Helpers.GetStreamReaderForString("X=M");
                var parser = new HackParser(inputStream);
                parser.advance();
                var dest = parser.dest();
            });
        }

        [Test]
        public void TestComp()
        {
            var random = new Random(4);
            var comps = HackParser.Comps
                .Select(comp => new KeyValuePair<string, string>(comp, $"{RandDest(random)}{comp}{RandJump(random)}"))
                .ToList();
            var inputStream = Helpers.GetStreamReaderForString(String.Join("\n", comps.Select(c => c.Value)));
            var parser = new HackParser(inputStream);
            foreach (var command in comps)
            {
                parser.advance();
                Assert.That(parser.comp(), Is.EqualTo(command.Key), $"Command: {command.Value}");
            }
        }

        [Test]
        public void TestInvalidComp()
        {
            Assert.Throws<ParserException>(() =>
            {
                var inputStream = Helpers.GetStreamReaderForString("A=X+1");
                var parser = new HackParser(inputStream);
                parser.advance();
                var dest = parser.comp();
            });
        }

        [Test]
        public void TestJump()
        {
            var random = new Random(4);
            Func<string?, string> formatJump = (jump) => jump == null ? "" : ";" + jump;
            var jumps = HackParser.Jumps
                .Select(jump => new KeyValuePair<string?, string>(jump, $"{RandDest(random)}{RandComp(random)}{formatJump(jump)}"))
                .ToList();
            var inputStream = Helpers.GetStreamReaderForString(String.Join("\n", jumps.Select(c => c.Value)));
            var parser = new HackParser(inputStream);
            foreach (var jump in jumps)
            {
                parser.advance();
                Assert.That(parser.jump(), Is.EqualTo(jump.Key), $"Command: {jump.Value}");
            }
        }

        [Test]
        public void TestInvalidJump()
        {
            Assert.Throws<ParserException>(() =>
            {
                var inputStream = Helpers.GetStreamReaderForString("A=X+1;JGQ");
                var parser = new HackParser(inputStream);
                parser.advance();
                var dest = parser.jump();
            });
        }

        [Test]
        public void TestCommandTypeForCommentsAndWhitespace()
        {
            var inputStream = Helpers.GetStreamReaderForString("// some notes\n\t\t     \n\t@0");
            var parser = new HackParser(inputStream);
            parser.advance();
            Assert.That(parser.commandType(), Is.EqualTo(CommandType.EMPTY));
            parser.advance();
            Assert.That(parser.commandType(), Is.EqualTo(CommandType.EMPTY));
            parser.advance();
            Assert.That(parser.commandType(), Is.EqualTo(CommandType.A_COMMAND));
        }

        [Test]
        public void TestLogicalPosition()
        {
            var inputStream = Helpers.GetStreamReaderForString("//empty\n(empty)\nM=A");
            var parser = new HackParser(inputStream);
            parser.advance();
            Assert.That(parser.LogicalPosition, Is.EqualTo(0));
            parser.advance();
            Assert.That(parser.LogicalPosition, Is.EqualTo(0));
            parser.advance();
            Assert.That(parser.LogicalPosition, Is.EqualTo(1));
        }

        [Test]
        public void TestInlineCommentsIgnored()
        {
            var inputStream = Helpers.GetStreamReaderForString("D=M;JGT              // D=first number");
            var parser = new HackParser(inputStream);
            parser.advance();
            Assert.That(parser.commandType(), Is.EqualTo(CommandType.C_COMMAND));
            Assert.That(parser.dest(), Is.EqualTo("D"));
            Assert.That(parser.comp(), Is.EqualTo("M"));
            Assert.That(parser.jump(), Is.EqualTo("JGT"));
        }

        public static string RandDest(Random random)
        {
            int idx = random.Next(0, HackParser.Dests.Length - 1);
            var dest = HackParser.Dests[idx];
            if (dest == null) return "";
            return dest + "=";
        }

        public static string RandComp(Random random)
        {
            int idx = random.Next(0, HackParser.Jumps.Length - 1);
            return HackParser.Comps[idx];
        }

        public static string RandJump(Random random)
        {
            int idx = random.Next(0, HackParser.Jumps.Length - 1);
            var dest = HackParser.Jumps[idx];
            if (dest == null) return "";
            return ";" + dest;
        }
    }
}
