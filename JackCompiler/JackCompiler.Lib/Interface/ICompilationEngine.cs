using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Interface
{
    internal interface ICompilationEngine
    {
        void CompileClass();
        void CompileClassVarDec();
        void CompileSubroutine();
        void CompileParameterList();
        void CompileVarDec();
        void CompileStatements();

        void CompileDo();
        void CompileLet();
        void CompileWhile();
        void CompileReturn();
        void CompileIf();
        void CompileExpression();
        void CompileTerm();
        void CompileExpressionList();
    }
}
