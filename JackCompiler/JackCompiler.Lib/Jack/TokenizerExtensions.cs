using JackCompiler.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Jack
{
    internal static class TokenizerExtensions
    {
        public static bool IsClassVarDec(this ITokenizer tokenizer)
        {
            return 
                tokenizer.TokenType() == TokenType.KEYWORD && (
                    tokenizer.KeyWord() == KeyWord.STATIC || tokenizer.KeyWord() == KeyWord.FIELD);
        }

        public static bool IsStatement(this ITokenizer tokenizer)
        {
            return tokenizer.TokenType() == TokenType.KEYWORD && StatementKeywords.Contains(tokenizer.KeyWord());
        }

        public static bool IsSubroutineDec(this ITokenizer tokenizer)
        {
            return
                tokenizer.TokenType() == TokenType.KEYWORD &&
                    SubroutineDecKeywords.Contains(tokenizer.KeyWord());
        }

        public static bool IsVarDec(this ITokenizer tokenizer)
        {
            return
                tokenizer.TokenType() == TokenType.KEYWORD &&
                    tokenizer.KeyWord() == KeyWord.VAR;
        }

        public static bool OnKeyWord(this ITokenizer tokenizer, ICollection<KeyWord> keyWords)
        {
            if (!tokenizer.HasMoreTokens()) return false;
            return
                tokenizer.TokenType() == TokenType.KEYWORD &&
                    keyWords.Contains(tokenizer.KeyWord());
        }

        public static bool OnKeyWord(this ITokenizer tokenizer, KeyWord keyWord)
        {
            if (!tokenizer.HasMoreTokens()) return false;
            return
                tokenizer.TokenType() == TokenType.KEYWORD &&
                    tokenizer.KeyWord() == keyWord;
        }

        public static bool OnSymbol(this ITokenizer tokenizer, ICollection<char> symbols)
        {
            if (!tokenizer.HasMoreTokens()) return false;
            return
                tokenizer.TokenType() == TokenType.SYMBOL &&
                    symbols.Contains(tokenizer.Symbol());
        }

        public static bool OnSymbol(this ITokenizer tokenizer, char symbol)
        {
            if (!tokenizer.HasMoreTokens()) return false;
            return
                tokenizer.TokenType() == TokenType.SYMBOL &&
                    tokenizer.Symbol() == symbol;
        }

        public static bool OnType(this ITokenizer tokenizer, ICollection<TokenType> types)
        {
            if (!tokenizer.HasMoreTokens()) return false;
            return types.Contains(tokenizer.TokenType());
        }

        public static bool OnType(this ITokenizer tokenizer, TokenType type)
        {
            if (!tokenizer.HasMoreTokens()) return false;
            return tokenizer.TokenType() == type;
        }

        public static bool OnTerm(this ITokenizer tokenizer)
        {
            return
                tokenizer.OnType(ConstantTokenTypes) ||
                tokenizer.OnKeyWord(ConstantKeywords) ||
                tokenizer.OnType(TokenType.IDENTIFIER) || // varname[] or subroutine call
                tokenizer.OnSymbol(UnaryOpSymbols) ||
                tokenizer.OnSymbol('('); // '(' expression ')'
        }

        // Types
        public static readonly ICollection<TokenType> ConstantTokenTypes = new TokenType[] { TokenType.INT_CONST, TokenType.STRING_CONST };
        // KeyWords
        public static readonly ICollection<KeyWord> ClassVarDecKeywords = new KeyWord[] { KeyWord.STATIC, KeyWord.FIELD };
        public static readonly ICollection<KeyWord> ConstantKeywords = new KeyWord[] { KeyWord.TRUE, KeyWord.FALSE, KeyWord.NULL, KeyWord.THIS };
        public static readonly ICollection<KeyWord> StatementKeywords = new KeyWord[] { KeyWord.LET, KeyWord.IF, KeyWord.WHILE, KeyWord.DO, KeyWord.RETURN };
        public static readonly ICollection<KeyWord> SubroutineDecKeywords = new KeyWord[] { KeyWord.CONSTRUCTOR, KeyWord.METHOD, KeyWord.FUNCTION };
        public static readonly ICollection<KeyWord> SubroutineTypes = new KeyWord[] { KeyWord.VOID, KeyWord.INT, KeyWord.BOOLEAN, KeyWord.CHAR };
        // Symbols
        public static readonly ICollection<char> OpSymbols = new char[] { '+', '-', '*', '/', '&', '|', '<', '>', '=' };
        public static readonly ICollection<char> UnaryOpSymbols = new char[] { '-', '~' };
    }
}
