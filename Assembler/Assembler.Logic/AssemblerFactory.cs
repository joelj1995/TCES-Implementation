using Assembler.Logic.Hack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Logic
{
    public static class AssemblerFactory
    {
        public static IAssembler CreateAssembler(string language)
        {
            if (language == null)
                throw new ArgumentNullException("language");
            if (language == "Hack")
                return new HackAssembler();
            throw new NotImplementedException($"{language} is not a supported language");
        }
    }
}
