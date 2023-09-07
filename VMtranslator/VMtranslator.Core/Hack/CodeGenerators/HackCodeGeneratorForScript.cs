using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForScript : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForScript(HackCodeWriterContext context, HackScript script) : base(context)
        {
            this.script = script;
        }

        public HackCodeGeneratorForScript(HackCodeWriterContext context, IHackCommand command) : base(context)
        {
            this.script = HackScript.create(command);
        }

        public override HackScript getCode()
        {
            return script;
        }

        private readonly HackScript script;
    }
}
