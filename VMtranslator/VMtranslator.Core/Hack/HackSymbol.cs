using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VMtranslator.Core.Hack.HackPlatform;

namespace VMtranslator.Core.Hack
{
    internal class HackSymbol
    {
        public HackSymbol(string symbol)
        {
            this.symbol = symbol;
        }

        public HackSymbol(int address)
        {
            this.symbol = address.ToString();
        }

        public override string ToString() 
        {
            return symbol;
        }

        private string symbol;
    }
}
