using JackCompiler.Lib.Jack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Test.Jack
{
    internal class TestJackCompilationEngineSymbolLookup
    {
        [SetUp]
        public void Setup()
        {
            config = new JackConfiguration()
            {
                OutputXml = true,
                ExtendedIdentifierXml = true,
            };
        }

        [Test]
        public void TestDefineForClass()
        {
            var input = @"
                class Square {
                    field int x;
                }
            ";
            var expected = @"
                <class>
                  <keyword> class </keyword>
                  <identifierDefinition>
	                <name> Square </name>
	                <category> class </category>
	                <kind> NONE </kind>
                  </identifierDefinition>
                  <symbol> { </symbol>
                  <classVarDec>
                    <keyword> field </keyword>
                    <keyword> int </keyword>
                    <identifierDefinition>
	                  <name> x </name>
	                  <category> field </category>
	                  <kind> FIELD </kind>
                    </identifierDefinition>
                    <symbol> ; </symbol>
                  </classVarDec>
                  <symbol> } </symbol>
                </class>
            ";
            AssertCompilesTo(input, expected, "class");
        }

        [Test]
        public void TestDefineForSubroutine()
        {
            var input = @"
                method void dispose() {
                    var int x;
                }
            ";
            var expected = @"
                <subroutineDec>
                  <keyword> method </keyword>
                  <keyword> void </keyword>
                  <identifierDefinition>
	                <name> dispose </name>
	                <category> subroutine </category>
	                <kind> NONE </kind>
                  </identifierDefinition>
                  <symbol> ( </symbol>
                  <parameterList>
                  </parameterList>
                  <symbol> ) </symbol>
                  <subroutineBody>
                    <symbol> { </symbol>
                    <varDec>
                      <keyword> var </keyword>
                      <keyword> int </keyword>  
                      <identifierDefinition>
	                    <name> x </name>
	                    <category> var </category>
	                    <kind> VAR </kind>
                      </identifierDefinition>
                      <symbol> ; </symbol>
                    </varDec>
                    <statements>
                    </statements>
                    <symbol> } </symbol>
                </subroutineBody>
                </subroutineDec>
            ";
            AssertCompilesTo(input, expected, "subroutine");
        }

        [Test]
        public void TestDefineForParams()
        {
            var input = @"
                method void dispose(int x) {
                }
            ";
            var expected = @"
                <subroutineDec>
                  <keyword> method </keyword>
                  <keyword> void </keyword>
                  <identifierDefinition>
	                <name> dispose </name>
	                <category> subroutine </category>
	                <kind> NONE </kind>
                  </identifierDefinition>
                  <symbol> ( </symbol>
                  <parameterList>
                  <keyword> int </keyword>
                  <identifierDefinition>
                    <name> x </name>
                    <category> arg </category>
                    <kind> ARG </kind>
                  </identifierDefinition>
                  </parameterList>
                  <symbol> ) </symbol>
                  <subroutineBody>
                    <symbol> { </symbol>
                    <statements>
                    </statements>
                    <symbol> } </symbol>
                </subroutineBody>
                </subroutineDec>
            ";
            AssertCompilesTo(input, expected, "subroutine");
        }

        [Test]
        public void TestConsumeForLet()
        {
            var input = @"
                method void dispose() {
                    var int y;
                    let y = 0;
                }
            ";
            var expected = @"
                <subroutineDec>
                  <keyword> method </keyword>
                  <keyword> void </keyword>
                  <identifierDefinition>
	                <name> dispose </name>
	                <category> subroutine </category>
	                <kind> NONE </kind>
                  </identifierDefinition>
                  <symbol> ( </symbol>
                  <parameterList>
                  </parameterList>
                  <symbol> ) </symbol>
                  <subroutineBody>
                    <symbol> { </symbol>
                    <varDec>
                      <keyword> var </keyword>
                      <keyword> int </keyword>  
                      <identifierDefinition>
	                    <name> y </name>
	                    <category> var </category>
	                    <kind> VAR </kind>
                      </identifierDefinition>
                      <symbol> ; </symbol>
                    </varDec>
                    <statements>
                    <letStatement>
                      <keyword> let </keyword>
                      <identifierOperation>
                        <name> y </name>
                        <category> var </category>
                        <kind> VAR </kind>
                      </identifierOperation>
                      <symbol> = </symbol>
                      <expression>
                      <term>
                        <integerConstant> 0 </integerConstant>
                      </term>
                      </expression>
                      <symbol> ; </symbol>
                    </letStatement>
                    </statements>
                    <symbol> } </symbol>
                </subroutineBody>
                </subroutineDec>
            ";
            AssertCompilesTo(input, expected, "subroutine");
        }

        [Test]
        public void TestConsumeForExpression()
        {
            var input = @"
                method void dispose() {
                    var int x;
                    var int y;
                    let y = x;
                }
            ";
            var expected = @"
                <subroutineDec>
                  <keyword> method </keyword>
                  <keyword> void </keyword>
                  <identifierDefinition>
	                <name> dispose </name>
	                <category> subroutine </category>
	                <kind> NONE </kind>
                  </identifierDefinition>
                  <symbol> ( </symbol>
                  <parameterList>
                  </parameterList>
                  <symbol> ) </symbol>
                  <subroutineBody>
                    <symbol> { </symbol>
                    <varDec>
                      <keyword> var </keyword>
                      <keyword> int </keyword>  
                      <identifierDefinition>
	                    <name> x </name>
	                    <category> var </category>
	                    <kind> VAR </kind>
                      </identifierDefinition>
                      <symbol> ; </symbol>
                    </varDec>
                    <varDec>
                      <keyword> var </keyword>
                      <keyword> int </keyword>  
                      <identifierDefinition>
	                    <name> y </name>
	                    <category> var </category>
	                    <kind> VAR </kind>
                      </identifierDefinition>
                      <symbol> ; </symbol>
                    </varDec>
                    <statements>
                    <letStatement>
                      <keyword> let </keyword>
                      <identifierOperation>
                        <name> y </name>
                        <category> var </category>
                        <kind> VAR </kind>
                      </identifierOperation>
                      <symbol> = </symbol>
                      <expression>
                      <term>
                        <identifierOperation>
	                      <name> x </name>
	                      <category> var </category>
	                      <kind> VAR </kind>
                        </identifierOperation>
                      </term>
                      </expression>
                      <symbol> ; </symbol>
                    </letStatement>
                    </statements>
                    <symbol> } </symbol>
                </subroutineBody>
                </subroutineDec>
            ";
            AssertCompilesTo(input, expected, "subroutine");
        }

        private void AssertCompilesTo(string jackCode, string expectedXML, string scope)
        {
            using (var inputStream = Helpers.GetStreamReaderForString(jackCode))
            using (var expectedStream = Helpers.GetStreamReaderForString(expectedXML))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var engine = new JackCompilationEngine(config, inputStream, writer);
                switch (scope)
                {
                    case "class": engine.CompileClass(); break;
                    case "subroutine": engine.CompileSubroutine(); break;
                    default: throw new ArgumentException(scope);
                }
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                Helpers.AssertXmlEqual(expectedStream, resultReader);
            }
        }

        private JackConfiguration config;
    }
}
