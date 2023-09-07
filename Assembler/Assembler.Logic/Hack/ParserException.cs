using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Logic.Hack
{
    public class ParserException : Exception
    {
        public string reason;
        public int? lineNumber;
        public ParserException(string reason, int lineNumber) : base(reason)
        {
            this.reason = reason;
            this.lineNumber = lineNumber;
        }
    }
}
