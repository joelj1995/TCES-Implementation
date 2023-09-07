using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Interface
{
    internal enum SymbolKind
    {
        STATIC, FIELD, ARG, VAR, NONE
    }

    internal interface ISymbolTable
    {
        public void StartSubroutine();
        public void Define(string name, string type, SymbolKind kind);
        public int VarCount(SymbolKind kind);
        public SymbolKind KindOf(string name);
        public string TypeOf(string name);
        public int IndexOf(string name);
    }
}
