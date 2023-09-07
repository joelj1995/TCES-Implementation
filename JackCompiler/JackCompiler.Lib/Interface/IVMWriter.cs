using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Interface
{
    internal enum Segment
    {
        CONST, ARG, LOCAL, STATIC, THIS, THAT, POINTER, TEMP
    }

    internal enum Command
    {
        ADD, SUB, NEG, EQ, GT, LT, AND, OR, NOT
    }

    internal interface IVMWriter
    {
        void WritePush(Segment segment, int index);
        void WritePop(Segment segment, int index);
        void WriteArithmetic(Command command);
        void WriteLabel(string label);
        void WriteGoto(string label);
        void WriteIf(string label);
        void WriteCall(string name, int nArgs);
        void WriteFunction(string name, int nLocals);
        void WriteReturn();
    }
}
