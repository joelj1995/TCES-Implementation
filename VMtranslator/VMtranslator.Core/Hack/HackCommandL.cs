using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack
{
    internal class HackCommandL : IHackCommand
    {
        public readonly string symbol;

        public HackCommandL(HackSymbol symbol) : this(symbol.ToString()) { }

        public HackCommandL(string symbol) 
        {
            this.symbol = symbol;
        }

        public IHackCommand clone()
        {
            return new HackCommandL(symbol);
        }

        public string encode()
        {
            return $"({symbol})";
        }

        public override bool Equals(object? obj)
        {
            var target = obj as HackCommandL;
            if (target == null) return false;
            return target.symbol == this.symbol;
        }
    }
}
