using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Interface
{
    public interface ITranslatorFactory
    {
        public ITranslator create(StreamWriter outputStream);
    }
}
