using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForNoop : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForNoop(HackCodeWriterContext context) : base(context)
        {
        }

        public override HackScript getCode()
        {
            return HackScript.create(HackCommand.C(null, "0", null));
        }
    }
}
