using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Interface
{
    public enum TokenType
    {
        KEYWORD,
        SYMBOL,
        IDENTIFIER,
        INT_CONST,
        STRING_CONST
    }

    enum KeyWord
    {
        CLASS,
        METHOD,
        FUNCTION,
        CONSTRUCTOR,
        INT,
        BOOLEAN,
        CHAR,
        VOID,
        VAR,
        STATIC,
        FIELD,
        LET,
        DO,
        IF,
        ELSE,
        WHILE,
        RETURN,
        TRUE,
        FALSE,
        NULL,
        THIS
    }

    internal interface ITokenizer
    {
        bool HasMoreTokens();
        void Advance();
        TokenType TokenType();
        KeyWord KeyWord();
        char Symbol();
        string Identifier();
        int IntVal();
        string StringVal();
        string ToXML();
        void Stash();
        void StashPop();
        IDisposable LookAhead();
    }
}
