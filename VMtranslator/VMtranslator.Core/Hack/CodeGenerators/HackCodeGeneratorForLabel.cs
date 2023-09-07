using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForLabel : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForLabel(HackCodeWriterContext context, string label) : base(context)
        {
            this.label = label;
        }

        public override HackScript getCode()
        {
            var labelLocation = location(label);
            return HackCommand.L(labelLocation).toScript();
        }

        private readonly string label;
    }
}
