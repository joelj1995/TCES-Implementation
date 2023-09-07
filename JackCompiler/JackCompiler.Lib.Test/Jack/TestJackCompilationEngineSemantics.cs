using JackCompiler.Lib.Jack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Test.Jack
{
    internal class TestJackCompilationEngineSemantics
    {
        [SetUp]
        public void Setup()
        {
            config = new JackConfiguration()
            {
                OutputXml = true,
                SkipSymbols = true,
            };
        }

        [Test]
        public void TestCompileClass()
        {
            var input = "class Main { }";
            var expected = @"
                <class>
                  <keyword> class </keyword>
                  <identifier> Main </identifier>
                    <symbol> { </symbol>
                    <symbol> } </symbol>
                </class>
            ";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            using (var expectedStream = Helpers.GetStreamReaderForString(expected))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var engine = new JackCompilationEngine(config, inputStream, writer);
                engine.CompileClass();
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                Helpers.AssertXmlEqual(expectedStream, resultReader);
            }
        }

        [Test]
        public void TestCompileSubroutine()
        {
            var input = "function void main() { }";
            var expected = @"
                <subroutineDec>
                  <keyword> function </keyword>
                  <keyword> void </keyword>
                  <identifier> main </identifier>
                  <symbol> ( </symbol>
                  <parameterList>
                  </parameterList>
                  <symbol> ) </symbol>
                  <subroutineBody>
                    <symbol> { </symbol>
                    <statements></statements>
                    <symbol> } </symbol>
                  </subroutineBody>
                </subroutineDec>
            ";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            using (var expectedStream = Helpers.GetStreamReaderForString(expected))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var engine = new JackCompilationEngine(config, inputStream, writer);
                engine.CompileSubroutine();
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                Helpers.AssertXmlEqual(expectedStream, resultReader);
            }
        }

        [Test]
        public void TestCompileSubroutineStatementBlockTransition()
        {
            var input = @"
                function void main() {
                    var SquareGame game;
                    return;
                }";
            var expected = @"
                <subroutineDec>
                <keyword> function </keyword>
                <keyword> void </keyword>
                <identifier> main </identifier>
                <symbol> ( </symbol>
                <parameterList>
                </parameterList>
                <symbol> ) </symbol>
                <subroutineBody>
                    <symbol> { </symbol>
                    <varDec>
                    <keyword> var </keyword>
                    <identifier> SquareGame </identifier>
                    <identifier> game </identifier>
                    <symbol> ; </symbol>
                    </varDec>
                    <statements>
                    <returnStatement>
                        <keyword> return </keyword>
                        <symbol> ; </symbol>
                    </returnStatement>
                    </statements>
                    <symbol> } </symbol>
                </subroutineBody>
                </subroutineDec>
            ";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            using (var expectedStream = Helpers.GetStreamReaderForString(expected))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var engine = new JackCompilationEngine(config, inputStream, writer);
                engine.CompileSubroutine();
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                Helpers.AssertXmlEqual(expectedStream, resultReader);
            }
        }

        [Test]
        public void TestCompileVarDec()
        {
            var input = "var SquareGame game;";
            var expected = @"
                <varDec>
                    <keyword> var </keyword>
                    <identifier> SquareGame </identifier>
                    <identifier> game </identifier>
                    <symbol> ; </symbol>
                </varDec>
            ";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            using (var expectedStream = Helpers.GetStreamReaderForString(expected))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var engine = new JackCompilationEngine(config, inputStream, writer);
                engine.CompileVarDec();
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                Helpers.AssertXmlEqual(expectedStream, resultReader);
            }
        }

        [Test]
        public void TestCompileVarDecWithComma()
        {
            var input = "function void main() { var int i, sum; }";
            var expected = @"
                <subroutineDec>
                    <keyword> function </keyword>
                    <keyword> void </keyword>
                    <identifier> main </identifier>
                    <symbol> ( </symbol>
                    <parameterList>
                    </parameterList>
                    <symbol> ) </symbol>
                    <subroutineBody>
                        <symbol> { </symbol>
                        <varDec>
                            <keyword> var </keyword>
                            <keyword> int </keyword>
                            <identifier> i </identifier>
                            <symbol> , </symbol>
                            <identifier> sum </identifier>
                            <symbol> ; </symbol>
                        </varDec>
                        <statements></statements>
                        <symbol> } </symbol>
                    </subroutineBody>
                </subroutineDec>
            ";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            using (var expectedStream = Helpers.GetStreamReaderForString(expected))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var engine = new JackCompilationEngine(config, inputStream, writer);
                engine.CompileSubroutine();
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                Helpers.AssertXmlEqual(expectedStream, resultReader);
            }
        }

        [Test]
        public void TestCompileLet()
        {
            var input = "let game = game;";
            var expected = @"
                <letStatement>
                  <keyword> let </keyword>
                  <identifier> game </identifier>
                  <symbol> = </symbol>
                  <expression>
                    <term>
                      <identifier> game </identifier>
                    </term>
                  </expression>
                  <symbol> ; </symbol>
                </letStatement>
            ";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            using (var expectedStream = Helpers.GetStreamReaderForString(expected))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var engine = new JackCompilationEngine(config, inputStream, writer);
                engine.CompileLet();
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                Helpers.AssertXmlEqual(expectedStream, resultReader);
            }
        }

        [Test]
        public void TestCompileDo()
        {
            var input = "do game.run();";
            var expected = @"
                <doStatement>
                    <keyword> do </keyword>
                    <identifier> game </identifier>
                    <symbol> . </symbol>
                    <identifier> run </identifier>
                    <symbol> ( </symbol>
                    <expressionList>
                    </expressionList>
                    <symbol> ) </symbol>
                    <symbol> ; </symbol>
                </doStatement>
            ";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            using (var expectedStream = Helpers.GetStreamReaderForString(expected))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var engine = new JackCompilationEngine(config, inputStream, writer);
                engine.CompileDo();
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                Helpers.AssertXmlEqual(expectedStream, resultReader);
            }
        }

        [Test]
        public void TestCompileReturn()
        {
            var input = "return this;";
            var expected = @"
                <returnStatement>
                    <keyword> return </keyword>
                        <expression>
                            <term>
                                <keyword> this </keyword>
                            </term>
                        </expression>
                    <symbol> ; </symbol>
                </returnStatement>
            ";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            using (var expectedStream = Helpers.GetStreamReaderForString(expected))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var engine = new JackCompilationEngine(config, inputStream, writer);
                engine.CompileReturn();
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                Helpers.AssertXmlEqual(expectedStream, resultReader);
            }
        }

        [Test]
        public void TestCompileClassVarDec()
        {
            var input = "class Main { field int x, y; }";
            var expected = @"
                <class>
                  <keyword> class </keyword>
                  <identifier> Main </identifier>
                  <symbol> { </symbol>
                  <classVarDec>
                    <keyword> field </keyword>
                    <keyword> int </keyword>
                    <identifier> x </identifier>
                    <symbol> , </symbol>
                    <identifier> y </identifier>
                    <symbol> ; </symbol>
                  </classVarDec>
                  <symbol> } </symbol>
                </class>
            ";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            using (var expectedStream = Helpers.GetStreamReaderForString(expected))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var engine = new JackCompilationEngine(config, inputStream, writer);
                engine.CompileClass();
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                Helpers.AssertXmlEqual(expectedStream, resultReader);
            }
        }

        [Test]
        public void TestCompileExpression()
        {
            var input = "x,";
            var expected = @"
                <expression>
                    <term>
                    <identifier> x </identifier>
                    </term>
                </expression>
            ";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            using (var expectedStream = Helpers.GetStreamReaderForString(expected))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var engine = new JackCompilationEngine(config, inputStream, writer);
                engine.CompileExpression();
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                Helpers.AssertXmlEqual(expectedStream, resultReader);
            }
        }

        [Test]
        public void TestCompileExpressionList()
        {
            var input = "x, y, x, y";
            var expected = @"
                <expressionList>
                <expression>
                    <term>
                    <identifier> x </identifier>
                    </term>
                </expression>
                <symbol> , </symbol>
                <expression>
                    <term>
                    <identifier> y </identifier>
                    </term>
                </expression>
                <symbol> , </symbol>
                <expression>
                    <term>
                    <identifier> x </identifier>
                    </term>
                </expression>
                <symbol> , </symbol>
                <expression>
                    <term>
                    <identifier> y </identifier>
                    </term>
                </expression>
                </expressionList>
            ";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            using (var expectedStream = Helpers.GetStreamReaderForString(expected))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var engine = new JackCompilationEngine(config, inputStream, writer);
                engine.CompileExpressionList();
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                Helpers.AssertXmlEqual(expectedStream, resultReader);
            }
        }

        [Test]
        public void TestCompileIf()
        {
            var input = "if (x) {  } else { }";
            var expected = @"
                <ifStatement>
                    <keyword> if </keyword>
                    <symbol> ( </symbol>
                    <expression>
                    <term>
                        <identifier> x </identifier>
                    </term>
                    </expression>
                    <symbol> ) </symbol>
                    <symbol> { </symbol>
                    <statements>
                    </statements>
                    <symbol> } </symbol>
                    <keyword> else </keyword>
                    <symbol> { </symbol>
                    <statements>
                    </statements>
                    <symbol> } </symbol>
                </ifStatement>
            ";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            using (var expectedStream = Helpers.GetStreamReaderForString(expected))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var engine = new JackCompilationEngine(config, inputStream, writer);
                engine.CompileIf();
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                Helpers.AssertXmlEqual(expectedStream, resultReader);
            }
        }

        [Test]
        public void TestCompileWhile()
        {
            var input = "while (x) { do Screen.setColor(x); }";
            var expected = @"
                <whileStatement>
                    <keyword> while </keyword>
                    <symbol> ( </symbol>
                    <expression>
                    <term>
                        <identifier> x </identifier>
                    </term>
                    </expression>
                    <symbol> ) </symbol>
                    <symbol> { </symbol>
                    <statements>
                    <doStatement>
                        <keyword> do </keyword>
                        <identifier> Screen </identifier>
                        <symbol> . </symbol>
                        <identifier> setColor </identifier>
                        <symbol> ( </symbol>
                        <expressionList>
                        <expression>
                            <term>
                            <identifier> x </identifier>
                            </term>
                        </expression>
                        </expressionList>
                        <symbol> ) </symbol>
                        <symbol> ; </symbol>
                    </doStatement>
                    </statements>
                    <symbol> } </symbol>
                </whileStatement>
            ";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            using (var expectedStream = Helpers.GetStreamReaderForString(expected))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var engine = new JackCompilationEngine(config, inputStream, writer);
                engine.CompileWhile();
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                Helpers.AssertXmlEqual(expectedStream, resultReader);
            }
        }

        [Test]
        public void TestCompileTerm()
        {
            var input = "SquareGame.new();";
            var expected = @"
                <term>
                    <identifier> SquareGame </identifier>
                    <symbol> . </symbol>
                    <identifier> new </identifier>
                    <symbol> ( </symbol>
                    <expressionList>
                    </expressionList>
                    <symbol> ) </symbol>
                </term>
            ";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            using (var expectedStream = Helpers.GetStreamReaderForString(expected))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var engine = new JackCompilationEngine(config, inputStream, writer);
                engine.CompileTerm();
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                Helpers.AssertXmlEqual(expectedStream, resultReader);
            }
        }

        [Test]
        public void TestCompileParameterList()
        {
            var input = "(int Ax, int Ay, int Asize)";
            var expected = @"<root>
                <symbol> ( </symbol>
                <parameterList>
                    <keyword> int </keyword>
                    <identifier> Ax </identifier>
                    <symbol> , </symbol>
                    <keyword> int </keyword>
                    <identifier> Ay </identifier>
                    <symbol> , </symbol>
                    <keyword> int </keyword>
                    <identifier> Asize </identifier>
                </parameterList>
                <symbol> ) </symbol></root>
            ";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            using (var expectedStream = Helpers.GetStreamReaderForString(expected))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var engine = new JackCompilationEngine(config, inputStream, writer);
                writer.WriteLine("<root>");
                engine.CompileParameterList();
                writer.WriteLine("</root>");
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                Helpers.AssertXmlEqual(expectedStream, resultReader);
            }
        }

        private JackConfiguration config;
    }
}
