using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Logic
{
    public interface IAssembler
    {
        byte[] assemble(StreamReader assembly);
        void BuildSymbolTable(StreamReader assembly);
        string DumpSymbolTable();
    }
}
