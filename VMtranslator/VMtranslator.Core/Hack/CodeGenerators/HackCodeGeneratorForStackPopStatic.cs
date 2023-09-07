using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForStackPopStatic : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForStackPopStatic(HackCodeWriterContext context, int index) : base(context)
        {
            this.index = index;
        }

        public override HackScript getCode()
        {
            return
                popStackIntoD() +
                HackCommand.A($"{context.FileName}.{index}") +
                HackCommand.C("M", "D");
        }

        private readonly int index;
    }
}
