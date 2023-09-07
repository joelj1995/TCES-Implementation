using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Interface
{
    public static class VMConstants
    {
        public static readonly string[] arithmeticAndLogicCommands =
        {
            "add",
            "sub",
            "neg",
            "eq",
            "gt",
            "lt",
            "and",
            "or",
            "not"
        };

        public static readonly IDictionary<string, CommandType> commandTypeMappings = new Dictionary<string, CommandType>()
        {
            { "add", CommandType.C_ARITHMETIC },
            { "sub", CommandType.C_ARITHMETIC },
            { "neg", CommandType.C_ARITHMETIC },
            { "eq", CommandType.C_ARITHMETIC },
            { "gt", CommandType.C_ARITHMETIC },
            { "lt", CommandType.C_ARITHMETIC },
            { "and", CommandType.C_ARITHMETIC },
            { "or", CommandType.C_ARITHMETIC },
            { "not", CommandType.C_ARITHMETIC },
            { "push", CommandType.C_PUSH },
            { "pop", CommandType.C_POP },
            { "label", CommandType.C_LABEL },
            { "goto", CommandType.C_GOTO },
            { "if-goto", CommandType.C_IF },
            { "function", CommandType.C_FUNCTION },
            { "call", CommandType.C_CALL },
            { "return", CommandType.C_RETURN },
        };

        public static string EntryMethod => "Sys.init";
    }
}
