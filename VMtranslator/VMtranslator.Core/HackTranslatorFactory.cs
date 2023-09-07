using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Hack;
using VMtranslator.Core.Interface;

namespace VMtranslator.Core
{
    public class HackTranslatorFactory : ITranslatorFactory
    {
        public ITranslator create(StreamWriter outputStream)
        {
            return new HackTranslator(outputStream);
        }
    }
}
