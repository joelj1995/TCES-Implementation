using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Interface
{
    internal interface IJackCompilationState
    {
        void NextRoutine();
        int StackLength { get; }
    }
}
