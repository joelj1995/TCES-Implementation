using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Interface
{
    public interface IParser
    {
        public bool hasMoreCommands();
        public void advance();
        public CommandType commandType();
        public string arg1();
        public int arg2();
    }
}
