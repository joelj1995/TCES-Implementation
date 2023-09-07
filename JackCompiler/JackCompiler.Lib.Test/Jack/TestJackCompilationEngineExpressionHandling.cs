using JackCompiler.Lib.Jack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Test.Jack
{
    internal class TestJackCompilationEngineExpressionHandling
    {
        [SetUp]
        public void Setup()
        {
            config = new JackConfiguration()
            {
                OutputXml = true,
                SkipSymbols = true
            };
        }

        [Test]
        public void TestArithmetic()
        {
            var input = "x + size";
            var expected = @"
                <expression>
                  <term>
                    <identifier> x </identifier>
                  </term>
                  <symbol> + </symbol>
                  <term>
                    <identifier> size </identifier>
                  </term>
                </expression>
            ";
            AssertExpressionCompilesTo(input, expected);
        }

        [Test]
        public void TestInnerBrackets()
        {
            var input = "(y + size) < 254";
            var expected = @"
                <expression>
                    <term>
                        <symbol> ( </symbol>
                        <expression>
                            <term>
                                <identifier> y </identifier>
                            </term>
                            <symbol> + </symbol>
                            <term>
                                <identifier> size </identifier>
                            </term>
                        </expression>
                        <symbol> ) </symbol>
                    </term>
                    <symbol> &lt; </symbol>
                    <term>
                        <integerConstant> 254 </integerConstant>
                    </term>
                </expression>
            ";
            AssertExpressionCompilesTo(input, expected);
        }

        [Test]
        public void TestInnerBitwise()
        {
            var input = "((y + size) < 254) & ((x + size) < 510))";
            var expected = @"
              <expression>
                <term>
                  <symbol> ( </symbol>
                  <expression>
                    <term>
                      <symbol> ( </symbol>
                      <expression>
                        <term>
                          <identifier> y </identifier>
                        </term>
                        <symbol> + </symbol>
                        <term>
                          <identifier> size </identifier>
                        </term>
                      </expression>
                      <symbol> ) </symbol>
                    </term>
                    <symbol> &lt; </symbol>
                    <term>
                      <integerConstant> 254 </integerConstant>
                    </term>
                  </expression>
                  <symbol> ) </symbol>
                </term>
                <symbol> &amp; </symbol>
                <term>
                  <symbol> ( </symbol>
                  <expression>
                    <term>
                      <symbol> ( </symbol>
                      <expression>
                        <term>
                          <identifier> x </identifier>
                        </term>
                        <symbol> + </symbol>
                        <term>
                          <identifier> size </identifier>
                        </term>
                      </expression>
                      <symbol> ) </symbol>
                    </term>
                    <symbol> &lt; </symbol>
                    <term>
                      <integerConstant> 510 </integerConstant>
                    </term>
                  </expression>
                  <symbol> ) </symbol>
                </term>
              </expression>
            ";
            AssertExpressionCompilesTo(input, expected);
        }

        [Test]
        public void TestNot()
        {
            var input = "~exit";
            var expected = @"
                <expression>
                    <term>
                        <symbol> ~ </symbol>
                        <term>
                            <identifier> exit </identifier>
                        </term>
                    </term>
                </expression>
            ";
            AssertExpressionCompilesTo(input, expected);
        }

        private void AssertExpressionCompilesTo(string jackExpression, string expectedXML)
        {
            using (var inputStream = Helpers.GetStreamReaderForString(jackExpression))
            using (var expectedStream = Helpers.GetStreamReaderForString(expectedXML))
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

        private JackConfiguration config;
    }
}
