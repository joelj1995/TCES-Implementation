using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack
{
    internal class HackComment : IHackCommand
    {
        public readonly string comment;

        public HackComment(string comment)
        {
            this.comment = comment;
        }

        public IHackCommand clone()
        {
            return new HackComment(this.comment);
        }

        public string encode()
        {
            return $"//{comment}";
        }

        public override bool Equals(object? obj)
        {
            var target = obj as HackComment;
            if (target == null) return false;
            return target.comment == this.comment;
        }
    }
}
