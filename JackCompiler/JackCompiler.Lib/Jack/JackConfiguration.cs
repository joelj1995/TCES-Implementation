using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Jack
{
    internal class JackConfiguration
    {
        public bool OutputXml { get; set; } = false;
        public bool ExtendedIdentifierXml { get; set; } = false;
        public bool SkipSymbols { get; set; } = false;
    }
}
