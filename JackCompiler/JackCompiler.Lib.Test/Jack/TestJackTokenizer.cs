using JackCompiler.Lib.Interface;
using JackCompiler.Lib.Jack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Test.Jack
{
    internal class TestJackTokenizer
    {
        [Test]
        public void TestSampleFromChapter()
        {
            var input = "if (x < 153) {let city = \"Paris\";}";
            var expected = @"
                <tokens>
                  <keyword> if </keyword>
                  <symbol> ( </symbol>
                  <identifier> x </identifier>
                  <symbol> &lt; </symbol>
                  <integerConstant> 153 </integerConstant>
                  <symbol> ) </symbol>
                  <symbol> { </symbol>
                  <keyword> let </keyword>
                  <identifier> city </identifier>
                  <symbol> = </symbol>
                  <stringConstant> Paris </stringConstant>
                  <symbol> ; </symbol>
                  <symbol> } </symbol>
                </tokens>
            ";
            Helpers.CompareTokens(input, expected);
        }

        [Test]
        public void BasicIfTest()
        {
            var input = "if";
            var expected = @"
                    <tokens>
                      <keyword> if </keyword>
                    </tokens>
            ";
            Helpers.CompareTokens(input, expected);
        }

        [Test]
        public void BasicSymbolTest()
        {
            var input = "(";
            var expected = @"
                <tokens>
                    <symbol> ( </symbol>
                </tokens>
            ";
            Helpers.CompareTokens(input, expected);
        }

        [Test]
        public void BasicIdentifierTest()
        {
            var input = "x";
            var expected = @"
                <tokens>
                    <identifier> x </identifier>
                </tokens>
            ";
            Helpers.CompareTokens(input, expected);
        }

        [Test]
        public void BasicIntegerConstantTest()
        {
            var input = "153";
            var expected = @"
                <tokens>
                    <integerConstant> 153 </integerConstant>
                </tokens>
            ";
            Helpers.CompareTokens(input, expected);
        }

        [Test]
        public void BasicStringConstantTest()
        {
            var input = "\"Paris\"";
            var expected = @"
                <tokens>
                    <stringConstant> Paris </stringConstant>
                </tokens>
            ";
            Helpers.CompareTokens(input, expected);
        }

        [Test]
        public void TestSymbolAfterInt()
        {
            var input = "153)  {";
            var expected = @"
                <tokens>
                    <integerConstant> 153 </integerConstant>
                    <symbol> ) </symbol>
                    <symbol> { </symbol>
                </tokens>
            ";
            Helpers.CompareTokens(input, expected);
        }

        [Test]
        public void TestTokenType()
        {
            var input = "if";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            {
                var tokenizer = new JackTokenizer(inputStream);
                tokenizer.Advance();
                Assert.That(tokenizer.TokenType(), Is.EqualTo(TokenType.KEYWORD));
            }
        }

        [Test]
        public void TestKeyWord()
        {
            var input = "if";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            {
                var tokenizer = new JackTokenizer(inputStream);
                tokenizer.Advance();
                Assert.That(tokenizer.KeyWord(), Is.EqualTo(KeyWord.IF));
            }
        }

        [Test]
        public void TestSkipMultLineComments()
        {
            var input = "/** test */\nif";
            var expected = @"
                    <tokens>
                      <keyword> if </keyword>
                    </tokens>
            ";
            Helpers.CompareTokens(input, expected);
        }

        [Test]
        public void TestSymbol()
        {
            var input = "(";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            {
                var tokenizer = new JackTokenizer(inputStream);
                tokenizer.Advance();
                Assert.That(tokenizer.Symbol(), Is.EqualTo('('));
            }
        }

        [Test]
        public void TestIntVal()
        {
            var input = "13";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            {
                var tokenizer = new JackTokenizer(inputStream);
                tokenizer.Advance();
                Assert.That(tokenizer.IntVal, Is.EqualTo(13));
            }
        }

        [Test]
        public void TestStringVal()
        {
            var input = "\"floo\"";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            {
                var tokenizer = new JackTokenizer(inputStream);
                tokenizer.Advance();
                Assert.That(tokenizer.StringVal, Is.EqualTo("floo"));
            }
        }

        [Test]
        public void TestLineNumber()
        {
            var input = "13 \n 13 \n /* \n test \n */ \n //test2 \n 13";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            {
                var tokenizer = new JackTokenizer(inputStream);
                tokenizer.Advance();
                tokenizer.Advance();
                tokenizer.Advance();
                Assert.That(tokenizer.LineNumber, Is.EqualTo(7));
            }
        }

        [Test]
        public void TestLookAhead()
        {
            var input = "0 \n 1 \n 2 \n 3";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            {
                var tokenizer = new JackTokenizer(inputStream);
                tokenizer.Advance();
                Assert.That(tokenizer.IntVal, Is.EqualTo(0));
                tokenizer.Stash();
                tokenizer.StashPop();
                Assert.That(tokenizer.IntVal, Is.EqualTo(0));
                tokenizer.Stash();
                tokenizer.Advance();
                tokenizer.Advance();
                tokenizer.Advance();
                Assert.That(tokenizer.IntVal, Is.EqualTo(3));
                tokenizer.StashPop();
                Assert.That(tokenizer.IntVal, Is.EqualTo(0));
                tokenizer.Advance();
                Assert.That(tokenizer.IntVal, Is.EqualTo(1));
                tokenizer.Advance();
                Assert.That(tokenizer.IntVal, Is.EqualTo(2));
            }
        }

        [Test]
        public void TestDisposableLookAhead()
        {
            var input = "0 \n 1 \n 2 \n 3";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            {
                var tokenizer = new JackTokenizer(inputStream);
                tokenizer.Advance();
                Assert.That(tokenizer.IntVal, Is.EqualTo(0));
                using (var _ = tokenizer.LookAhead())
                {
                    tokenizer.Advance();
                    tokenizer.Advance();
                    tokenizer.Advance();
                    Assert.That(tokenizer.IntVal, Is.EqualTo(3));
                }
                Assert.That(tokenizer.IntVal, Is.EqualTo(0));
            }
        }

        [Test]
        public void TestDisposableLookAheadForExpression()
        {
            var input = "(y + size) < 254";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            {
                var tokenizer = new JackTokenizer(inputStream);
                tokenizer.Advance();
                Assert.That(tokenizer.Symbol(), Is.EqualTo('('));
                tokenizer.Advance();
                Assert.That(tokenizer.Identifier, Is.EqualTo("y"));
                using (var _ = tokenizer.LookAhead())
                {
                    tokenizer.Advance();
                    Assert.That(tokenizer.Symbol(), Is.EqualTo('+'));
                }
                Assert.That(tokenizer.Identifier, Is.EqualTo("y"));
                tokenizer.Advance();
                Assert.That(tokenizer.Symbol(), Is.EqualTo('+'));
                tokenizer.Advance();
                Assert.That(tokenizer.Identifier, Is.EqualTo("size"));
                using (var _ = tokenizer.LookAhead())
                {
                    tokenizer.Advance();
                    Assert.That(tokenizer.Symbol(), Is.EqualTo(')'));
                }
                Assert.That(tokenizer.Identifier, Is.EqualTo("size"));
                tokenizer.Advance();
                Assert.That(tokenizer.Symbol(), Is.EqualTo(')'));
            }
        }

        [Test]
        public void TestDisposableLookAheadWithMultiLineComments()
        {
            var input = @"
                // This file is part of the materials accompanying the book 
                // File name: projects/10/ExpressionlessSquare/Main.jack

                // Expressionless version of Main.jack.

                /**
                 * The Main class initializes a new Square Dance game and starts it.
                 */
                (y + size) < 254";
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            {
                var tokenizer = new JackTokenizer(inputStream);
                tokenizer.Advance();
                Assert.That(tokenizer.Symbol(), Is.EqualTo('('));
                tokenizer.Advance();
                Assert.That(tokenizer.Identifier, Is.EqualTo("y"));
                using (var _ = tokenizer.LookAhead())
                {
                    tokenizer.Advance();
                    Assert.That(tokenizer.Symbol(), Is.EqualTo('+'));
                }
                Assert.That(tokenizer.Identifier, Is.EqualTo("y"));
                tokenizer.Advance();
                Assert.That(tokenizer.Symbol(), Is.EqualTo('+'));
                tokenizer.Advance();
                Assert.That(tokenizer.Identifier, Is.EqualTo("size"));
                using (var _ = tokenizer.LookAhead())
                {
                    tokenizer.Advance();
                    Assert.That(tokenizer.Symbol(), Is.EqualTo(')'));
                }
                Assert.That(tokenizer.Identifier, Is.EqualTo("size"));
                tokenizer.Advance();
                Assert.That(tokenizer.Symbol(), Is.EqualTo(')'));
            }
        }
    }
}
