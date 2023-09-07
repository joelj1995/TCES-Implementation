using JackCompiler.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace JackCompiler.Lib.Jack
{
    internal class JackTokenizer : ITokenizer
    {
        public JackTokenizer(StreamReader inputStream)
        {
            this.state = new JackTockenizerState(inputStream);
        }

        public void Advance()
        {
            var currentToken = String.Empty;
            while (true)
            {
                if (state.Stream.EndOfStream) break;
                char next = ReadChar();
                char peek = state.Stream.Peek();
                AdvanceReaderPastComments(ref next, ref peek);
                if (Char.IsWhiteSpace(next))
                {
                    if (currentToken.Length == 0)
                        continue;
                    else
                        break;
                }
                else if (JackConstants.Symbols.Contains(next))
                {
                    state.NextSymbol(next.ToString());
                    return;
                }
                else if (next.Equals('"'))
                {
                    var currentString = String.Empty;
                    while (true)
                    {
                        char stringNext = ReadChar();
                        if (stringNext == '"') break;
                        if (state.Stream.EndOfStream) throw new Exception("String not closed");
                        currentString += stringNext;
                    }
                    state.NextStringVal(currentString);
                    return;
                }
                currentToken += next;
                if (JackConstants.Symbols.Contains(peek))
                {
                    if (currentToken.Length != 0)
                    {
                        break;
                    }
                }
            }
            if (JackConstants.Keywords.Contains(currentToken))
            {
                state.NextKeyword(currentToken);
            }
            else if (String.IsNullOrEmpty(currentToken))
            {
                state.End();
            }
            else if (int.TryParse(currentToken[0].ToString(), out _))
            {
                state.NextIntVal(int.Parse(currentToken));
            }
            else
            {
                state.NextIdentifier(currentToken);
            }
            debugCurrentPosition = state.Stream.Position;
        }

        public bool HasMoreTokens()
        {
            return !state.Ended;
        }

        public string Identifier()
        {
            return state.Identifier();
        }

        public int IntVal()
        {
            return state.IntVal();
        }

        public KeyWord KeyWord()
        {
            return state.KeyWord();
        }

        public string StringVal()
        {
            return state.StringVal();
        }

        public char Symbol()
        {
            return state.Symbol();
        }

        public TokenType TokenType()
        {
            return state.TokenType();
        }

        public string ToXML()
        {
            return state.ToXML();
        }

        public int LineNumber { get; private set; } = 1;

        private void AdvanceReaderPastComments(ref char next, ref char peek)
        {
            while (next.Equals('/') && (peek.Equals('/') || peek.Equals('*')))
            {
                while (next.Equals('/') && peek.Equals('/')) // Single line comments
                {
                    state.Stream.ReadLine();
                    LineNumber++;
                    next = ReadChar();
                    peek = state.Stream.Peek();
                }
                while (next.Equals('/') && peek.Equals('*')) // multi line comments
                {
                    while (true)
                    {
                        next = ReadChar();
                        peek = state.Stream.Peek();
                        if (next.Equals('*') && peek.Equals('/'))
                        {
                            state.Stream.Read();
                            next = ReadChar();
                            peek = state.Stream.Peek();
                            break;
                        }
                        if (state.Stream.EndOfStream) throw new Exception("Comment not closed");
                    }
                }
            }
        }

        public IDisposable LookAhead()
        {
            return new JackTokenizerStateManager(state);
        }

        private char ReadChar()
        {
            char c = state.Stream.Read();
            if (c.Equals('\n')) LineNumber++;
            return c;
        }

        public void Stash()
        {
            state.Stash();
        }

        public void StashPop()
        {
            state.StashPop();
        }

        // dependencies
        private readonly JackTockenizerState state;
        private long? stashedPosition;
        private long debugCurrentPosition;

        public class JackTokenizerStateManager : IDisposable
        {
            public JackTokenizerStateManager(JackTockenizerState state)
            {
                this.state = state;
                state.Stash();
            }

            public void Dispose()
            {
                state.StashPop();
            }

            private readonly JackTockenizerState state;
        }
    }
}
