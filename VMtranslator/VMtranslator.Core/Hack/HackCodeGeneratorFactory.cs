using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Hack.CodeGenerators;
using VMtranslator.Core.Interface;

namespace VMtranslator.Core.Hack
{
    internal class HackCodeGeneratorFactory
    {
        public HackCodeGeneratorFactory(HackCodeWriterContext context) 
        {
            this.context = context;
        }

        public AbstractHackCodeGenerator arithmetic(string command)
        {
            switch (command)
            {
                case "add": return new HackCodeGeneratorForArithmeticBinaryOperation(context, "+");
                case "sub": return new HackCodeGeneratorForArithmeticBinaryOperation(context, "-");
                case "neg": return new HackCodeGeneratorForArithmeticNeg(context);
                case "eq":  return new HackCodeGeneratorForArithmeticBinaryCompare(context, "JEQ");
                case "gt":  return new HackCodeGeneratorForArithmeticBinaryCompare(context, "JGT");
                case "lt":  return new HackCodeGeneratorForArithmeticBinaryCompare(context, "JLT");
                case "and": return new HackCodeGeneratorForArithmeticBinaryOperation(context, "&");
                case "or":  return new HackCodeGeneratorForArithmeticBinaryOperation(context, "|");
                case "not": return new HackCodeGeneratorForArithmeticNot(context);
                default:
                    throw new NotImplementedException(command);
            }
        }

        public AbstractHackCodeGenerator call(string functionName, int args)
        {
            return new HackCodeGeneratorForFunctionCall(context, functionName, args);
        }

        public AbstractHackCodeGenerator cgoto(string label)
        {
            return new HackCodeGeneratorForGoto(context, label);
        }

        public AbstractHackCodeGenerator cif(string label)
        {
            return new HackCodeGeneratorForIf(context, label);
        }

        public AbstractHackCodeGenerator init() 
        {
            return new HackCodeGeneratorForHeader(context);
        }

        public AbstractHackCodeGenerator function(string functionName, int numLocals)
        {
            return new HackCodeGeneratorForFunction(context, functionName, numLocals);
        }

        public AbstractHackCodeGenerator label(string label)
        {
            return new HackCodeGeneratorForScript(context, HackCommand.L(label));
        }

        public AbstractHackCodeGenerator creturn()
        {
            return new HackCodeGeneratorForReturn(context);
        }

        public AbstractHackCodeGenerator stackOperation( CommandType command, string segment, int index)
        {
            if (command.Equals(CommandType.C_PUSH))
            {
                switch (segment)
                {
                    case "argument": return new HackCodeGeneratorForStackPushSegement(context, HackPlatform.ReservedRegisters.ARG, index);
                    case "constant": return new HackCodeGeneratorForStackPushConstant(context, index);
                    case "local": return new HackCodeGeneratorForStackPushSegement(context, HackPlatform.ReservedRegisters.LCL, index);
                    case "pointer": return new HackCodeGeneratorForStackPushPointer(context, index);
                    case "static": return new HackCodeGeneratorForStackPushStatic(context, index);
                    case "temp": return new HackCodeGeneratorForStackPushTemp(context, index);
                    case "this": return new HackCodeGeneratorForStackPushSegement(context, HackPlatform.PointerRegisters[0], index);
                    case "that": return new HackCodeGeneratorForStackPushSegement(context, HackPlatform.PointerRegisters[1], index);
                }
            }
            else if (command.Equals(CommandType.C_POP))
            {
                switch (segment)
                {
                    case "argument": return new HackCodeGeneratorForStackPopSegment(context, HackPlatform.ReservedRegisters.ARG, index);
                    case "constant": return new HackCodeGeneratorForNoop(context);
                    case "local": return new HackCodeGeneratorForStackPopSegment(context, HackPlatform.ReservedRegisters.LCL, index);
                    case "pointer": return new HackCodeGeneratorForStackPopPointer(context, index);
                    case "temp": return new HackCodeGeneratorForStackPopTemp(context, index);
                    case "static": return new HackCodeGeneratorForStackPopStatic(context, index);
                    case "this": return new HackCodeGeneratorForStackPopSegment(context, HackPlatform.PointerRegisters[0], index);
                    case "that": return new HackCodeGeneratorForStackPopSegment(context, HackPlatform.PointerRegisters[1], index);
                }
            }
            throw new ArgumentException(nameof(command));
        }

        private readonly HackCodeWriterContext context;
    }
}
