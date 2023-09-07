using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack
{
    internal class HackCommandC : IHackCommand
    {
        public readonly string? dest;
        public readonly string comp;
        public readonly string? jump;

        public HackCommandC(string? dest, string comp, string? jump)
        {
            if (!HackPlatform.Dests.Contains(dest)) throw new ArgumentException(dest, nameof(dest));
            if (!HackPlatform.Comps.Contains(comp)) throw new ArgumentException(comp, nameof(comp));
            if (!HackPlatform.Jumps.Contains(jump)) throw new ArgumentException(jump, nameof(jump));
            this.dest = dest; this.comp = comp; this.jump = jump;
        }

        public IHackCommand clone()
        {
            return new HackCommandC(dest, comp, jump);
        }

        public string encode()
        {
            var sb = new StringBuilder();
            if (dest != null) sb.Append($"{dest}=");
            sb.Append(comp);
            if (jump != null) sb.Append($";{jump}");
            return sb.ToString();
        }

        public override bool Equals(object? obj)
        {
            var target = obj as HackCommandC;
            if (target == null) return false;
            return target.dest.Equals(dest) &&
                target.comp.Equals(this.comp) &&
                target.jump.Equals(this.jump);
        }
    }
}
