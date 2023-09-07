using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForIf : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForIf(HackCodeWriterContext context, string label) : base(context)
        {
            this.label = label;
        }

        public override HackScript getCode()
        {
            return 
                popStackIntoD() + 
                HackCommand.A(routineLocation(label)) + 
                HackCommand.C(null, "D", "JNE");
        }

        private readonly string label;
    }
}
