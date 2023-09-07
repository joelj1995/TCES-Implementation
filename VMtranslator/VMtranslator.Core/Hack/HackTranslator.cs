using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Interface;

namespace VMtranslator.Core.Hack
{
    internal class HackTranslator : ITranslator
    {
        public HackTranslator(StreamWriter outputStream)
        {
            codeWriter = new HackCodeWriter(outputStream);
        }

        public void translate(string fileName, StreamReader inputStream)
        {
            var parser = new VMParser(inputStream);
            codeWriter.setFileName(fileName);
            codeWriter.writeInit();
            while (parser.hasMoreCommands())
            {
                parser.advance();
                switch (parser.commandType())
                {
                    case CommandType.EMPTY:
                        continue;
                    case CommandType.C_ARITHMETIC:
                        codeWriter.writeArithmetic(parser.arg1());
                        break;
                    case CommandType.C_CALL:
                        codeWriter.writeCall(parser.arg1(), parser.arg2());
                        break;
                    case CommandType.C_FUNCTION:
                        codeWriter.setSubroutine(parser.arg1());
                        codeWriter.writeFunction(parser.arg1(), parser.arg2());
                        break;
                    case CommandType.C_GOTO:
                        codeWriter.writeGoto(parser.arg1());
                        break;
                    case CommandType.C_IF:
                        codeWriter.writeIf(parser.arg1());
                        break;
                    case CommandType.C_LABEL:
                        codeWriter.writeLabel(parser.arg1());
                        break;
                    case CommandType.C_PUSH:
                        codeWriter.writePushPop(CommandType.C_PUSH, parser.arg1(), parser.arg2());
                        break;
                    case CommandType.C_POP:
                        codeWriter.writePushPop(CommandType.C_POP, parser.arg1(), parser.arg2()); 
                        break;
                    case CommandType.C_RETURN:
                        codeWriter.writeReturn();
                        break;
                    default: throw new ArgumentException(parser.commandType().ToString());
                }
            }
        }

        private readonly ICodeWriter codeWriter;
    }
}
