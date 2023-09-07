using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForStackPopTemp : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForStackPopTemp(HackCodeWriterContext context, int index) : base(context)
        {
            this.index = index;
        }

        public override HackScript getCode()
        {
            var register = HackPlatform.TemporaryRegisters[index];
            return
                popStackIntoD() +
                HackCommand.A(register) +
                HackCommand.C("M", "D");
        }

        private int index;
    }
}
