using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Hack;
using VMtranslator.Core.Hack.CodeGenerators;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForStackPushStatic : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForStackPushStatic(HackCodeWriterContext context, int index) : base(context)
        {
            this.index = index;
        }

        public override HackScript getCode()
        {
            return
                HackCommand.A($"{context.FileName}.{index}") +
                HackCommand.C("D", "M") +
                pushDOntoStack();
        }

        private readonly int index;
    }
}
