using JackCompiler.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Jack
{
    internal class NullJackSymbolTable : ISymbolTable
    {
        public void Define(string name, string type, SymbolKind kind)
        {
            return;
        }

        public int IndexOf(string name)
        {
            return 0;
        }

        public SymbolKind KindOf(string name)
        {
            return SymbolKind.NONE;
        }

        public void StartSubroutine()
        {
            return;
        }

        public string TypeOf(string name)
        {
            return string.Empty;
        }

        public int VarCount(SymbolKind kind)
        {
            return 0;
        }
    }
}
