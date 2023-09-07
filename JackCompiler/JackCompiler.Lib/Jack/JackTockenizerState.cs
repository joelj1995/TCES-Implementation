using JackCompiler.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Jack
{
    internal class JackTockenizerState 
    {
        public JackTockenizerState(StreamReader inputStream)
        {
            this.Stream = new StashableStreamReader(inputStream);
        }

        public void NextKeyword(string keyword)
        {
            this.curType = Interface.TokenType.KEYWORD;
            this.curVal = keyword;
        }

        public void NextSymbol(string symbol)
        {
            if (symbol.Length > 1) throw new ArgumentException(symbol);
            this.curType = Interface.TokenType.SYMBOL;
            this.curVal = symbol;
        }

        public void NextIdentifier(string identifier)
        {
            this.curType = Interface.TokenType.IDENTIFIER;
            this.curVal = identifier;
        }

        public void NextIntVal(int intVal)
        {
            this.curType = Interface.TokenType.INT_CONST;
            this.curVal = intVal.ToString();
        }

        public void NextStringVal(string strVal)
        {
            this.curType = Interface.TokenType.STRING_CONST;
            this.curVal = strVal;
        }

        public void End()
        {
            ended = true;
        }

        public TokenType TokenType()
        {
            if (curType is null) throw new InvalidOperationException("No token has been read.");
            if (ended) throw new InvalidOperationException("No more tokens to read");
            return (TokenType)curType;
        }

        public KeyWord KeyWord()
        {
            if (curType is null) throw new InvalidOperationException("No token has been read.");
            if (ended) throw new InvalidOperationException("No more tokens to read");
            if (curType != Interface.TokenType.KEYWORD) throw new InvalidOperationException();
            return Enum.Parse<KeyWord>(curVal.ToUpper());
        }

        public char Symbol()
        {
            if (curType is null) throw new InvalidOperationException("No token has been read.");
            if (ended) throw new InvalidOperationException("No more tokens to read");
            if (curType != Interface.TokenType.SYMBOL) throw new InvalidOperationException();
            return curVal[0];
        }

        public string Identifier()
        {
            if (curType is null) throw new InvalidOperationException("No token has been read.");
            if (ended) throw new InvalidOperationException("No more tokens to read");
            if (curType != Interface.TokenType.IDENTIFIER) throw new InvalidOperationException();
            return curVal;
        }

        public int IntVal()
        {
            if (curType is null) throw new InvalidOperationException("No token has been read.");
            if (ended) throw new InvalidOperationException("No more tokens to read");
            if (curType != Interface.TokenType.INT_CONST) throw new InvalidOperationException();
            return int.Parse(curVal);
        }

        public string StringVal()
        {
            if (curType is null) throw new InvalidOperationException("No token has been read.");
            if (ended) throw new InvalidOperationException("No more tokens to read");
            if (curType != Interface.TokenType.STRING_CONST) throw new InvalidOperationException();
            return curVal;
        }

        public string ToXML()
        {
            if (curType is null) throw new InvalidOperationException("No token has been read.");
            if (ended) return String.Empty;
            switch (curType)
            {
                case Interface.TokenType.KEYWORD:
                    return $"<keyword> {curVal} </keyword>";
                case Interface.TokenType.SYMBOL:
                    return $"<symbol> {CleanSymbol(curVal)} </symbol>";
                case Interface.TokenType.IDENTIFIER:
                    return $"<identifier> {curVal} </identifier>";
                case Interface.TokenType.INT_CONST:
                    return $"<integerConstant> {curVal} </integerConstant>";
                case Interface.TokenType.STRING_CONST:
                    return $"<stringConstant> {curVal} </stringConstant>";
            }
            throw new NotImplementedException(curType.ToString());
        }

        public void Stash()
        {
            if (!Stream.CanSeek) throw new InvalidOperationException("Base stream does not support seek");
            if (stashedPosition is not null) throw new InvalidOperationException("There is already a stashed position.");
            stashedPosition = Stream.Position;
            stashedype = curType;
            stashedVal = curVal;
            stashedEnded = ended;
        }

        public void StashPop()
        {
            if (stashedPosition is null) throw new InvalidOperationException("Stash has not been called.");
            Stream.SeekToPosition((long)stashedPosition);
            stashedPosition = null;
            curType = stashedype;
            curVal = stashedVal;
            ended = stashedEnded;
        }

        private static string CleanSymbol(string symbol)
        {
            var result = symbol.Replace("&", "&amp;");
            result = result.Replace("<", "&lt;");
            result = result.Replace(">", "&gt;");
            return result;
        }

        public bool Ended => ended;

        public StashableStreamReader Stream { get; private set; }

        private TokenType? curType = null;
        private string curVal = String.Empty;
        private bool ended = false;

        private long? stashedPosition = null;
        private TokenType? stashedype = null;
        private string stashedVal = String.Empty;
        private bool stashedEnded;
        

        internal class StashableStreamReader
        {
            public StashableStreamReader(StreamReader inputStream)
            {
                this.inputStream = inputStream;
            }

            public char Peek()
            {
                return (char)inputStream.Peek();
            }

            public char Read()
            {
                position++;
                return (char)inputStream.Read();
            }

            public string? ReadLine()
            {
                var result = inputStream.ReadLine();
                if (result != null)
                    position += result.Length + 2; // probably going to cause issues if the file is not CRLF
                return result;
            }

            public void SeekToPosition(long position)
            {
                inputStream.BaseStream.Seek((long)position, SeekOrigin.Begin);
                inputStream.DiscardBufferedData();
                this.position = position;
            }

            public bool CanSeek => inputStream.BaseStream.CanSeek;
            public bool EndOfStream => inputStream.EndOfStream;
            public long Position => position;

            private readonly StreamReader inputStream;
            private long position = 0;
        }
    }
}
