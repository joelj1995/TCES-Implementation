using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Jack
{
    internal class JackCompilationContext
    {
        public int getNextCookie => cookie++;

        private int cookie = 0;
    }
}
