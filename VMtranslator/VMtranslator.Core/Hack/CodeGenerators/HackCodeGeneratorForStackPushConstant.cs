using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForStackPushConstant : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForStackPushConstant(HackCodeWriterContext context, int index) : base(context)
        {
            this.index = index;
        }

        public override HackScript getCode()
        {
            return 
                HackCommand.A(index) +
                HackCommand.C("D", "A") + 
                pushDOntoStack();
        }

        private int index;
    }
}
