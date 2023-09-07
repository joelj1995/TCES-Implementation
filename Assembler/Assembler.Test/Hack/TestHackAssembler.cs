using Assembler.Logic;
using Assembler.Logic.Hack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Test.Hack
{
    public class TestHackAssembler
    {
        HackAssembler assembler;

        [SetUp]
        public void Setup()
        {
            assembler = new HackAssembler();
        }

        [Test]
        public void TestBinaryForParserPositionC()
        {
            var inputStream = Helpers.GetStreamReaderForString("MD=M+1");
            var parser = new HackParser(inputStream);
            parser.advance();
            var binary = assembler.BinaryForParserPosition(parser);
            Assert.That(binary, Is.EqualTo("1111110111011000"));
        }

        [Test]
        public void TestBinaryForParserPositionCKernelSpace()
        {
            var inputStream = Helpers.GetStreamReaderForString("MD=M+1!");
            var parser = new HackParser(inputStream);
            parser.advance();
            var binary = assembler.BinaryForParserPosition(parser);
            Assert.That(binary, Is.EqualTo("1101110111011000"));
        }

        [Test]
        public void TestBinaryForParserPositionA()
        {
            var inputStream = Helpers.GetStreamReaderForString("@7");
            var parser = new HackParser(inputStream);
            parser.advance();
            var binary = assembler.BinaryForParserPosition(parser);
            Assert.That(binary, Is.EqualTo("0000000000000111"));
        }

        [Test]
        public void TestBuildSymbolTable()
        {
            var sb = new StringBuilder();
            sb.AppendLine("(ZERO)");
            sb.AppendLine("M=A+1");
            sb.AppendLine("(ONE)");
            sb.AppendLine("M=A+1");
            sb.AppendLine("M=A+1");
            sb.AppendLine("(THREE)");
            sb.AppendLine("@AZERO");
            sb.AppendLine("(FOUR)");
            sb.AppendLine("@AONE");
            var inputStream = Helpers.GetStreamReaderForString(sb.ToString());
            var parser = new HackParser(inputStream);
            assembler.BuildSymbolTable(inputStream);
            Assert.That(assembler.Symbols.getAddress("ZERO"), Is.EqualTo(0));
            Assert.That(assembler.Symbols.getAddress("ONE"), Is.EqualTo(1));
            Assert.That(assembler.Symbols.getAddress("THREE"), Is.EqualTo(3));
            Assert.That(assembler.Symbols.getAddress("FOUR"), Is.EqualTo(4));
            inputStream.BaseStream.Seek(0, SeekOrigin.Begin);
            assembler.GenerateBinaryCode(inputStream);
            Assert.That(assembler.Symbols.getAddress("AZERO"), Is.EqualTo(16 + 0));
            Assert.That(assembler.Symbols.getAddress("AONE"), Is.EqualTo(16 + 1));
        }
    }
}
