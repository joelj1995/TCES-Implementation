using JackCompiler.Lib.Jack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Test.Jack
{
    internal class TestJackVMWriter
    {
        [SetUp] 
        public void SetUp() 
        {
            memoryStreamForWriter = new MemoryStream();
            resultReader = new StreamReader(memoryStreamForWriter);
            writer = new StreamWriter(memoryStreamForWriter);
            vmWriter = new JackVMWriter(writer);
            toDispose = new CompositeDisposable(new IDisposable[] { memoryStreamForWriter, resultReader, writer });
        }

        [TearDown]
        public void TearDown()
        {
            toDispose.Dispose();
        }

        [Test]
        public void TestWriteFunctionNoArgs()
        {
            var expected = "function Main.main 0";
            vmWriter.WriteFunction("Main.main", 0);
            AssertOutputMatches(expected);
        }

        [Test]
        public void TestWriteCall()
        {
            var expected = "call Math.multiply 2";
            vmWriter.WriteCall("Math.multiply", 2);
            AssertOutputMatches(expected);
        }

        [Test]
        public void TestWritePush()
        {
            var expected = "push constant 1";
            vmWriter.WritePush(Interface.Segment.CONST, 1);
            AssertOutputMatches(expected);
        }

        [Test]
        public void TestWriteReturn()
        {
            var expected = "return";
            vmWriter.WriteReturn();
            AssertOutputMatches(expected);
        }

        [Test]
        public void TestWriteArithmetic()
        {
            var expected = "add";
            vmWriter.WriteArithmetic(Interface.Command.ADD);
            AssertOutputMatches(expected);
        }

        [Test]
        public void TestWritePop()
        {
            var expected = "pop temp 0";
            vmWriter.WritePop(Interface.Segment.TEMP, 0);
            AssertOutputMatches(expected);
        }

        [Test]
        public void TestWriteLabel()
        {
            var expected = "label WHILE_EXP0";
            vmWriter.WriteLabel("WHILE_EXP0");
            AssertOutputMatches(expected);
        }

        [Test]
        public void TestWriteGoto()
        {
            var expected = "goto WHILE_EXP0";
            vmWriter.WriteGoto("WHILE_EXP0");
            AssertOutputMatches(expected);
        }

        [Test]
        public void TestWriteIf()
        {
            var expected = "if-goto IF_TRUE0";
            vmWriter.WriteIf("IF_TRUE0");
            AssertOutputMatches(expected);
        }

        private void AssertOutputMatches(string expected)
        {
            writer.Flush();
            memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
            Assert.That(expected, Is.EqualTo(resultReader.ReadToEnd().Trim()));
        }

        private MemoryStream memoryStreamForWriter;
        private StreamReader resultReader;
        private StreamWriter writer;
        private JackVMWriter vmWriter;
        private IDisposable toDispose;
    }
}
