using JackCompiler.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Jack
{
    internal class NullJackVMWriter : IVMWriter, IJackCompilationState
    {
        public int StackLength => stackLength;

        public void NextRoutine()
        {
            stackLength = 0;
        }

        public void WriteArithmetic(Command command)
        {
            return;
        }

        public void WriteCall(string name, int nArgs)
        {
            return;
        }

        public void WriteFunction(string name, int nLocals)
        {
            return;
        }

        public void WriteGoto(string label)
        {
            return;
        }

        public void WriteIf(string label)
        {
            return;
        }

        public void WriteLabel(string label)
        {
            return;
        }

        public void WritePop(Segment segment, int index)
        {
            return;
        }

        public void WritePush(Segment segment, int index)
        {
            return;
        }

        public void WriteReturn()
        {
            return;
        }

        private int stackLength = 0;
    }
}
