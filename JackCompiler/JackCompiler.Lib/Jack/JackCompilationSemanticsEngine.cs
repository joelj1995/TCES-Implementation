using JackCompiler.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Jack
{
    internal class JackCompilationSemanticsEngine
    {
        public JackCompilationSemanticsEngine(StreamWriter outputStream, JackTokenizer tokenizer, JackConfiguration config)
        {
            this.outputStream = outputStream;
            this.tokenizer = tokenizer;
            this.config = config;
        }

        public void BeginScope(string newScope)
        {
            WriteForDepth($"<{newScope}>");
            scope.Push(newScope);
        }

        public void EndScope(string oldScope)
        {
            var prevScope = scope.Pop();
            if (prevScope != oldScope) throw new ArgumentException(prevScope);
            WriteForDepth($"</{prevScope}>");
        }

        public SymbolKind ExpectClassSymbolKind()
        {
            var keyword = ExpectKeyword(TokenizerExtensions.ClassVarDecKeywords);
            switch (keyword)
            {
                case KeyWord.FIELD: return SymbolKind.FIELD;
                case KeyWord.STATIC: return SymbolKind.STATIC;
                default: throw new Exception(keyword.ToString());
            }
        }

        public (int? intVal, string? stringVal) ExpectConstant()
        {
            int? intVal = null;
            string? stringVal = null;
            switch (tokenizer.TokenType())
            {
                case TokenType.INT_CONST:
                    intVal = tokenizer.IntVal();
                    WriteForDepth(tokenizer.ToXML());
                    break;
                case TokenType.STRING_CONST:
                    stringVal = tokenizer.StringVal();
                    WriteForDepth(tokenizer.ToXML());
                    break;
                default: throw new SyntaxException(tokenizer.ToXML(), LineNumber, ClassName);
            }
            tokenizer.Advance();
            return (intVal, stringVal);
        }

        public string PeekIdentifier()
        {
            if (tokenizer.TokenType() != TokenType.IDENTIFIER)
                throw new SyntaxException(tokenizer.ToXML(), LineNumber, ClassName);
            return tokenizer.Identifier();
        }

        public string ConsumeIdentifier(string category)
        {
            return ExpectIdentifier(false, category, SymbolKind.NONE);
        }

        public string ConsumeIdentifier(SymbolKind kind)
        {
            return ExpectIdentifier(false, kind.ToString().ToLower(), kind);
        }

        public string DefineIdentifier(SymbolKind kind)
        {
            return DefineIdentifier(kind.ToString().ToLower(), kind);
        }

        public string DefineIdentifier(string category)
        {
            return DefineIdentifier(category, SymbolKind.NONE);
        }

        public string DefineIdentifier(string category, SymbolKind kind)
        {
            return ExpectIdentifier(true, category, kind);
        }

        public string ExpectIdentifier(bool defined, string category, SymbolKind kind)
        {
            if (tokenizer.TokenType() != TokenType.IDENTIFIER)
                throw new SyntaxException(tokenizer.ToXML(), LineNumber, ClassName);
            if (config.ExtendedIdentifierXml)
            {
                if (defined)
                    WriteForDepth(XmlGen.IdentifierDefinition(tokenizer.Identifier(), category, kind));
                else
                    WriteForDepth(XmlGen.IdentifierOperation(tokenizer.Identifier(), category, kind));
            }
            else
                WriteForDepth(tokenizer.ToXML());
            var result = tokenizer.Identifier();
            tokenizer.Advance();
            return result;
        }

        public string ExpectType(bool allowVoid=true)
        {
            if (tokenizer.TokenType() == TokenType.KEYWORD && tokenizer.KeyWord() == KeyWord.VOID && !allowVoid)
                throw new SyntaxException("void", LineNumber, ClassName);
            if (tokenizer.TokenType() != TokenType.IDENTIFIER && (
                tokenizer.TokenType() != TokenType.KEYWORD || 
                !TokenizerExtensions.SubroutineTypes.Contains(tokenizer.KeyWord())))
            {
                if (tokenizer.TokenType() == TokenType.KEYWORD)
                    throw new SyntaxException(tokenizer.ToXML(), LineNumber, ClassName);
                else
                    throw new SyntaxException(tokenizer.ToXML(), LineNumber, ClassName);
            }
            var result = string.Empty;
            if (tokenizer.OnType(TokenType.KEYWORD))
                result = tokenizer.KeyWord().ToString().ToLower();
            else
                result = tokenizer.Identifier().ToString();
            WriteForDepth(tokenizer.ToXML());
            tokenizer.Advance();
            return result;
        }

        public KeyWord ExpectKeyword(ICollection<KeyWord> expected)
        {
            if (tokenizer.TokenType() != TokenType.KEYWORD)
                throw new SyntaxException(tokenizer.ToXML(), LineNumber, ClassName);
            if (!expected.Contains(tokenizer.KeyWord()))
                throw new SyntaxException(tokenizer.ToXML(), LineNumber, ClassName);
            WriteForDepth(tokenizer.ToXML());
            var result = tokenizer.KeyWord();
            tokenizer.Advance();
            return result;
        }

        public void ExpectKeyword(KeyWord expected)
        {
            if (tokenizer.TokenType() != TokenType.KEYWORD)
                throw new SyntaxException(tokenizer.ToXML(), LineNumber, ClassName);
            if (tokenizer.KeyWord() != expected) 
                throw new SyntaxException(tokenizer.ToXML(), LineNumber, ClassName);
            WriteForDepth(tokenizer.ToXML());
            tokenizer.Advance();
        }

        public char ExpectSymbol(ICollection<char> expected)
        {
            if (tokenizer.TokenType() != TokenType.SYMBOL)
                throw new SyntaxException(tokenizer.ToXML(), LineNumber, ClassName);
            if (!expected.Contains(tokenizer.Symbol()))
                throw new SyntaxException(tokenizer.ToXML(), LineNumber, ClassName);
            var result = tokenizer.Symbol();
            WriteForDepth(tokenizer.ToXML());
            tokenizer.Advance();
            return result;
        }

        public void ExpectSymbol(char expected)
        {
            if (tokenizer.TokenType() != TokenType.SYMBOL)
                throw new SyntaxException(tokenizer.ToXML(), LineNumber, ClassName);
            if (tokenizer.Symbol() != expected)
                throw new SyntaxException(tokenizer.ToXML(), LineNumber, ClassName);
            WriteForDepth(tokenizer.ToXML());
            tokenizer.Advance();
        }

        public void SetClassName(string className)
        {
            if (ClassName != String.Empty) throw new InvalidOperationException("Class name already set");
            ClassName = className;
        }

        public int Depth => scope.Count;
        public int LineNumber => ((JackTokenizer)tokenizer).LineNumber;

        private void WriteForDepth(string line)
        {
            if (!XmlMode)
            {
                return;
            }
            var tabs = Enumerable.Repeat("  ", Depth);
            string tabString = String.Join("", tabs);
            outputStream.WriteLine(tabString + line);
            DebugOut += tabString + line + Environment.NewLine;
        }

        private JackConfiguration config;
        public string ClassName { get; private set; } = String.Empty;
        public string DebugOut { get; private set; } = String.Empty;
        
        private readonly StreamWriter outputStream;
        private readonly Stack<string> scope = new();
        private readonly JackTokenizer tokenizer;
        private bool XmlMode => config.OutputXml;
    }
}
