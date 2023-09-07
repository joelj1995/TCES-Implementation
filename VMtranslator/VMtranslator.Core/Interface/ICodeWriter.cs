using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Interface
{
    public interface ICodeWriter
    {
        public void close();
        public void setFileName(string fileName);
        public void setSubroutine(string subroutine);
        public void writeArithmetic(string command);
        public void writeCall(string functionName, int args);
        public void writeFunction(string functionName, int numLocals);
        public void writeGoto(string label);
        public void writeIf(string label);
        public void writeInit();
        public void writeLabel(string label);
        public void writePushPop(CommandType command, string segment, int index);
        public void writeReturn();
    }
}
