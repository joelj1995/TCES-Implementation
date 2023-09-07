using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Interface;

namespace VMtranslator.Core.Hack
{
    internal class HackCodeWriter : ICodeWriter
    {
        public HackCodeWriter(StreamWriter outputStream)
        {
            writer = outputStream;
            codeFactory = new HackCodeGeneratorFactory(context);
        }

        public void close()
        {
            // Intentionally chose to deviate from the spec here
            // We got the stream as an input. Why do we have to be concerned with the cleanup?
            throw new InvalidOperationException("The caller should take care of cleaning up stream.");
        }

        public void setFileName(string fileName)
        {
            context.FileName = fileName;
        }

        public void writeArithmetic(string command)
        {
            var generator = codeFactory.arithmetic(command);
            writer.Write(generator.compile());
        }

        public void writeCall(string functionName, int args)
        {
            var generator = codeFactory.call(functionName, args);
            writer.Write(generator.compile());
        }

        public void writeFunction(string functionName, int numLocals)
        {
            var generator = codeFactory.function(functionName, numLocals);
            writer.Write(generator.compile());
        }

        public void writeGoto(string label)
        {
            var generator = codeFactory.cgoto(label);
            writer.Write(generator.compile());
        }

        public void writeIf(string label)
        {
            var generator = codeFactory.cif(label);
            writer.Write(generator.compile());
        }

        public void writeInit()
        {
            if (context.InitWritten) return;
            var generator = codeFactory.init();
            writer.Write(generator.compile());
            context.setInitWritten();
        }

        public void writeLabel(string label)
        {
            var generator = codeFactory.label(label);
            writer.Write(generator.compile());
        }

        public void writePushPop(CommandType command, string segment, int index)
        {
            var generator = codeFactory.stackOperation(command, segment, index);
            writer.Write(generator.compile());
        }

        public void writeReturn()
        {
            var generator = codeFactory.creturn();
            writer.Write(generator.compile());
        }

        private readonly StreamWriter writer;
        private readonly HackScript script = HackScript.create();
        private readonly HackCodeWriterContext context = new HackCodeWriterContext();
        private readonly HackCodeGeneratorFactory codeFactory;
    }
}
