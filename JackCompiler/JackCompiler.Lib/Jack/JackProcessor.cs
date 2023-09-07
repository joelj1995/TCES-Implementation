using JackCompiler.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Jack
{
    internal class JackProcessor : IProcessor
    {
        public JackProcessor(JackConfiguration config, StreamReader inputStream, StreamWriter outputStream) 
        { 
            this.engine = new JackCompilationEngine(config, inputStream, outputStream);
            this.outputStream = outputStream;
        }

        public void Process()
        {
            engine.CompileClass();
        }

        private readonly ICompilationEngine engine;
        private readonly StreamWriter outputStream;
    }
}
