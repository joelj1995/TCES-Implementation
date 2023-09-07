using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Logic
{
    public class SyntaxException : Exception
    {
        public int lineNumber;
        public string reason;
        public SyntaxException(int lineNumber, string reason) : base($"Error on line {lineNumber}: {reason}")
        {
            this.lineNumber = lineNumber;
            this.reason = reason;
        }
    }
}
