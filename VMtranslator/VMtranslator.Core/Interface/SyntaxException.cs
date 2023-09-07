using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Interface
{
    public class SyntaxException : Exception
    {
        public int lineNumber { get; }
        public string reason { get; }
        public SyntaxException(int lineNumber, string reason) : base($"Error on line {lineNumber}: {reason}")
        {
            this.lineNumber = lineNumber;
            this.reason = reason;
        }
    }
}
