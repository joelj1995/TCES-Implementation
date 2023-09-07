using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForStackPushTemp : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForStackPushTemp(HackCodeWriterContext context, int index) : base(context)
        {
            this.index = index;
        }

        public override HackScript getCode()
        {
            var register = HackPlatform.TemporaryRegisters[index];
            return 
                loadSymbolIntoD(register) +
                pushDOntoStack();
        }

        private int index;
    }
}
