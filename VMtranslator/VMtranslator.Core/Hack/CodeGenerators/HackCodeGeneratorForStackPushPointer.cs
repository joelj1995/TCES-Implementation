using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForStackPushPointer : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForStackPushPointer(HackCodeWriterContext context, int index) : base(context)
        {
            this.index = index;
        }

        public override HackScript getCode()
        {
            return 
                HackCommand.A(HackPlatform.PointerRegisters[index]) +
                HackCommand.C("D", "M") +
                pushDOntoStack();
        }

        private int index;
    }
}
