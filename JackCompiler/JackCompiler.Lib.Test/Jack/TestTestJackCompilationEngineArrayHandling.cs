using JackCompiler.Lib.Jack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Test.Jack
{
    internal class TestTestJackCompilationEngineArrayHandling
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
        public void TestArrayAssignment()
        {
            var input = "function void main() { var Array a; var int i; let a[i] = 1;  }";
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
                            <identifier> Array </identifier>
                            <identifier> a </identifier>
                            <symbol> ; </symbol>
                        </varDec>
                        <varDec>
                            <keyword> var </keyword>
                            <keyword> int </keyword>
                            <identifier> i </identifier>
                            <symbol> ; </symbol>
                        </varDec>
                        <statements>
                            <letStatement>
                            <keyword> let </keyword>
                            <identifier> a </identifier>
                            <symbol> [ </symbol>
                            <expression>
                                <term>
                                    <identifier> i </identifier>
                                </term>
                            </expression>
                            <symbol> ] </symbol>
                            <symbol> = </symbol>
                            <expression>
                            <term>
                                <integerConstant> 1 </integerConstant>
                            </term>
                            </expression>
                            <symbol> ; </symbol>
                            </letStatement>
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
        public void TestArrayAccess()
        {
            var input = "function void main() { var Array a; var int i; let i = a[1];  }";
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
                            <identifier> Array </identifier>
                            <identifier> a </identifier>
                            <symbol> ; </symbol>
                        </varDec>
                        <varDec>
                            <keyword> var </keyword>
                            <keyword> int </keyword>
                            <identifier> i </identifier>
                            <symbol> ; </symbol>
                        </varDec>
                        <statements>
                            <letStatement>
                            <keyword> let </keyword>
                            <identifier> i </identifier>
                            <symbol> = </symbol>
                            <expression>
                            <term>
                                <identifier> a </identifier>
                                <symbol> [ </symbol>
                                <expression>
                                    <term>
                                        <integerConstant> 1 </integerConstant>
                                    </term>
                                </expression>
                                <symbol> ] </symbol>
                            </term>
                            </expression>
                            <symbol> ; </symbol>
                            </letStatement>
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

        private JackConfiguration config;
    }
}
