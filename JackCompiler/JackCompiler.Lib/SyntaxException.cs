using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib
{
    public class SyntaxException : Exception
    {
        public int LineNumber { get; private set; }
        public SyntaxException(string message, int lineNumber, string theClass) : base($"Class: {theClass} Line {lineNumber}: {message}") 
        {
            LineNumber = lineNumber;
        }
    }
}
