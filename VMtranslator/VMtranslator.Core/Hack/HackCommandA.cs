using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack
{
    internal class HackCommandA : IHackCommand
    {
        public readonly string symbol;

        public HackCommandA(HackSymbol symbol) : this(symbol.ToString()) { }

        private HackCommandA(string symbol)
        {
            this.symbol = symbol;
        }

        public IHackCommand clone()
        {
            return new HackCommandA(symbol);
        }

        public string encode()
        {
            return $"@{symbol}";
        }

        public override bool Equals(object? obj)
        {
            var target = obj as HackCommandA;
            if (target == null) return false;
            return target.symbol == this.symbol;
        }
    }
}
