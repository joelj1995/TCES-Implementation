using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForGoto : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForGoto(HackCodeWriterContext context, string label) : base(context)
        {
            this.label = label;
        }

        public override HackScript getCode()
        {
            return HackCommand.A(label) + HackCommand.C(null, "0", "JMP");
        }

        private readonly string label;
    }
}
