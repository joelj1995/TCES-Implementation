using JackCompiler.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Jack
{
    internal class JackCompilationEngine : ICompilationEngine
    {
        public enum TermType { VARIABLE, ARRAY_ENTRY, SUBROUTINE_CALL, NONE };

        public JackCompilationEngine(JackConfiguration config, StreamReader inputStream, StreamWriter outputStream)
        {
            this.config = config;
            this.tokenizer = new JackTokenizer(inputStream);
            this.semantics = new JackCompilationSemanticsEngine(outputStream, (JackTokenizer)tokenizer, config);
            this.symbols = config.SkipSymbols ? new NullJackSymbolTable() : new JackSymbolTable(config);
            if (config.OutputXml)
            {
                var vmWriter = new NullJackVMWriter();
                this.writer = vmWriter;
                this.stackLengthTracker = vmWriter;
            }
            else
            {
                var vmWriter = new JackVMWriter(outputStream);
                this.writer = vmWriter;
                this.stackLengthTracker = vmWriter;
            }
            tokenizer.Advance();
        }

        public void CompileClass()
        {
            semantics.BeginScope("class");
            semantics.ExpectKeyword(KeyWord.CLASS);
            var className = semantics.DefineIdentifier("class");
            semantics.SetClassName(className);
            semantics.ExpectSymbol('{');
            while(tokenizer.IsClassVarDec())
            {
                CompileClassVarDec();
            }
            while(tokenizer.IsSubroutineDec())
            {
                CompileSubroutine();
            }
            semantics.ExpectSymbol('}');
            semantics.EndScope("class");
        }

        public void CompileClassVarDec()
        {
            semantics.BeginScope("classVarDec");
            var symbolKind = semantics.ExpectClassSymbolKind();
            var symbolType = semantics.ExpectType(false);
            while (true)
            {
                var symbolName = semantics.DefineIdentifier(symbolKind);
                symbols.Define(symbolName, symbolType, symbolKind);
                if (!tokenizer.OnSymbol(','))
                    break;
                semantics.ExpectSymbol(','); 
            }
            semantics.ExpectSymbol(';');
            semantics.EndScope("classVarDec");
        }

        public void CompileSubroutine()
        {
            stackLengthTracker.NextRoutine();
            whileCookie = 0;
            ifCookie = 0;
            semantics.BeginScope("subroutineDec");
            symbols.StartSubroutine();
            var declarationKeyword = semantics.ExpectKeyword(TokenizerExtensions.SubroutineDecKeywords);
            semantics.ExpectType();
            var routineName = semantics.DefineIdentifier("subroutine");
            if (declarationKeyword == KeyWord.METHOD)
            {
                symbols.Define("", semantics.ClassName, SymbolKind.ARG);
            }
            CompileParameterList();
            semantics.BeginScope("subroutineBody");
            semantics.ExpectSymbol('{');
            while (tokenizer.IsVarDec())
            {
                CompileVarDec();
            }
            writer.WriteFunction($"{semantics.ClassName}.{routineName}", symbols.VarCount(SymbolKind.VAR));
            if (declarationKeyword == KeyWord.CONSTRUCTOR)
            {
                writer.WritePush(Segment.CONST, symbols.VarCount(SymbolKind.FIELD));
                writer.WriteCall("Memory.alloc", 1);
                writer.WritePop(Segment.POINTER, 0);
            }
            else if (declarationKeyword == KeyWord.METHOD)
            {
                writer.WritePush(Segment.ARG, 0);
                writer.WritePop(Segment.POINTER, 0);
            }
            CompileStatements();
            semantics.ExpectSymbol('}');
            semantics.EndScope("subroutineBody");
            semantics.EndScope("subroutineDec");
        }

        public void CompileParameterList()
        {
            semantics.ExpectSymbol('(');
            semantics.BeginScope("parameterList");
            while (true)
            {
                if (tokenizer.TokenType() == TokenType.SYMBOL) break;
                var type = semantics.ExpectType(false);
                var symbolName = semantics.DefineIdentifier(SymbolKind.ARG);
                symbols.Define(symbolName, type, SymbolKind.ARG);
                if (!tokenizer.OnSymbol(','))
                    break;
                semantics.ExpectSymbol(',');
            }
            semantics.EndScope("parameterList");
            semantics.ExpectSymbol(')');
        }

        public void CompileVarDec()
        {
            semantics.BeginScope("varDec");
            semantics.ExpectKeyword(KeyWord.VAR);
            var symbolType = semantics.ExpectType(false);
            while (true)
            {
                var symbolName = semantics.DefineIdentifier(SymbolKind.VAR);
                symbols.Define(symbolName, symbolType, SymbolKind.VAR);
                if (!tokenizer.OnSymbol(','))
                    break;
                semantics.ExpectSymbol(',');
            }
            semantics.ExpectSymbol(';');
            semantics.EndScope("varDec");
        }

        public void CompileStatements()
        {
            semantics.BeginScope("statements");
            while (tokenizer.IsStatement())
            {
                switch (tokenizer.KeyWord())
                {
                    case KeyWord.LET: CompileLet(); break;
                    case KeyWord.IF: CompileIf(); break;
                    case KeyWord.WHILE: CompileWhile(); break;
                    case KeyWord.DO: CompileDo(); break;
                    case KeyWord.RETURN: CompileReturn(); break;
                }
            }
            semantics.EndScope("statements");
        }

        public void  CompileDo()
        {
            semantics.BeginScope("doStatement");
            semantics.ExpectKeyword(KeyWord.DO);
            CompileSubroutineCall();
            writer.WritePop(Segment.TEMP, 0); // not entire clear yet on what this is for -- maybe to support let?
            semantics.ExpectSymbol(';');
            semantics.EndScope("doStatement");
        }

        public void CompileLet()
        {
            semantics.BeginScope("letStatement");
            semantics.ExpectKeyword(KeyWord.LET);
            var symbolName = semantics.PeekIdentifier();
            var arrayOperation = symbols.KindOf(symbolName);
            semantics.ConsumeIdentifier(symbols.KindOf(symbolName));
            var didWildArrayStuff = false;
            if (tokenizer.OnSymbol('['))
            {
                semantics.ExpectSymbol('[');
                CompileExpression();
                semantics.ExpectSymbol(']');
                switch (arrayOperation)
                {
                    case SymbolKind.FIELD: writer.WritePush(Segment.THIS, symbols.IndexOf(symbolName)); break;
                    case SymbolKind.STATIC: writer.WritePush(Segment.STATIC, symbols.IndexOf(symbolName)); break;
                    case SymbolKind.VAR: writer.WritePush(Segment.LOCAL, symbols.IndexOf(symbolName)); break;
                    case SymbolKind.ARG: writer.WritePush(Segment.ARG, symbols.IndexOf(symbolName)); break;
                    default: if (!config.SkipSymbols) { throw new SyntaxException(tokenizer.ToXML(), LineNumber, semantics.ClassName); } break;
                }
                writer.WriteArithmetic(Command.ADD);
                didWildArrayStuff = true;
            }
            semantics.ExpectSymbol('=');
            CompileExpression();
            if (didWildArrayStuff)
            {
                writer.WritePop(Segment.TEMP, 0); // store the result of the expression in TEMP[0]
                writer.WritePop(Segment.POINTER, 1); // THAT points to array element previously calculated
                writer.WritePush(Segment.TEMP, 0); // push result onto the stack
                writer.WritePop(Segment.THAT, 0); // store result in the array
            }
            else
            {
                switch (arrayOperation)
                {
                    case SymbolKind.FIELD: writer.WritePop(Segment.THIS, symbols.IndexOf(symbolName)); break;
                    case SymbolKind.STATIC: writer.WritePop(Segment.STATIC, symbols.IndexOf(symbolName)); break;
                    case SymbolKind.VAR: writer.WritePop(Segment.LOCAL, symbols.IndexOf(symbolName)); break;
                    case SymbolKind.ARG: writer.WritePop(Segment.ARG, symbols.IndexOf(symbolName)); break;
                    default: if (!config.SkipSymbols) { throw new SyntaxException(tokenizer.ToXML(), LineNumber, semantics.ClassName); } break;
                }
            }
            
            semantics.ExpectSymbol(';');
            semantics.EndScope("letStatement");
        }

        public void CompileWhile()
        {
            var cookie = whileCookie++;
            semantics.BeginScope("whileStatement");
            semantics.ExpectKeyword(KeyWord.WHILE);
            writer.WriteLabel($"WHILE_EXP{cookie}");
            semantics.ExpectSymbol('(');
            CompileExpression();
            writer.WriteArithmetic(Command.NOT);
            writer.WriteIf($"WHILE_END{cookie}");
            semantics.ExpectSymbol(')');
            semantics.ExpectSymbol('{');
            CompileStatements();
            writer.WriteGoto($"WHILE_EXP{cookie}");
            semantics.ExpectSymbol('}');
            writer.WriteLabel($"WHILE_END{cookie}");
            semantics.EndScope("whileStatement");
        }

        public void CompileReturn()
        {
            semantics.BeginScope("returnStatement");
            semantics.ExpectKeyword(KeyWord.RETURN);
            if (!tokenizer.OnSymbol(';'))
            {
                CompileExpression();
            }
            else
            {
                // I think the idea is to push zero for null returns?
                writer.WritePush(Segment.CONST, 0);
            }
            writer.WriteReturn();
            semantics.ExpectSymbol(';');
            semantics.EndScope("returnStatement");
        }

        public void CompileIf()
        {
            var cookie = ifCookie++;
            semantics.BeginScope("ifStatement");
            semantics.ExpectKeyword(KeyWord.IF);
            semantics.ExpectSymbol('(');
            CompileExpression();
            semantics.ExpectSymbol(')');
            semantics.ExpectSymbol('{');
            writer.WriteIf($"IF_TRUE{cookie}");
            writer.WriteGoto($"IF_FALSE{cookie}");
            writer.WriteLabel($"IF_TRUE{cookie}");
            CompileStatements();
            semantics.ExpectSymbol('}');
            if (tokenizer.OnKeyWord(KeyWord.ELSE)) 
            {
                writer.WriteGoto($"IF_END{cookie}");
                writer.WriteLabel($"IF_FALSE{cookie}");
                semantics.ExpectKeyword(KeyWord.ELSE);
                semantics.ExpectSymbol('{');
                CompileStatements();
                semantics.ExpectSymbol('}');
                writer.WriteLabel($"IF_END{cookie}");
            }
            else
                writer.WriteLabel($"IF_FALSE{cookie}");
            semantics.EndScope("ifStatement");
        }

        public void CompileExpression()
        {
            semantics.BeginScope("expression");
            CompileTerm();
            while (tokenizer.OnSymbol(TokenizerExtensions.OpSymbols))
            {
                var op = semantics.ExpectSymbol(TokenizerExtensions.OpSymbols);
                CompileTerm();
                switch (op)
                {
                    case '+': writer.WriteArithmetic(Command.ADD); break;
                    case '-': writer.WriteArithmetic(Command.SUB); break;
                    case '=': writer.WriteArithmetic(Command.EQ); break;
                    case '>': writer.WriteArithmetic(Command.GT); break;
                    case '<': writer.WriteArithmetic(Command.LT); break;
                    case '&': writer.WriteArithmetic(Command.AND); break;
                    case '|': writer.WriteArithmetic(Command.OR); break;
                    case '*': writer.WriteCall("Math.multiply", 2); break;
                    case '/': writer.WriteCall("Math.divide", 2); break;
                    default: throw new SyntaxException(tokenizer.ToXML(), LineNumber, semantics.ClassName);
                }
            }
            semantics.EndScope("expression");
        }

        public void CompileTerm()
        {
            semantics.BeginScope("term");
            if (tokenizer.OnType(TokenizerExtensions.ConstantTokenTypes))
            {
                var constant = semantics.ExpectConstant();
                if (constant.intVal is not null)
                    writer.WritePush(Segment.CONST, (int)constant.intVal);
                else if (constant.stringVal is not null)
                {
                    string str = constant.stringVal;
                    writer.WritePush(Segment.CONST, str.Length);
                    writer.WriteCall("String.new", 1);
                    foreach (char c in str)
                    {
                        writer.WritePush(Segment.CONST, c);
                        writer.WriteCall("String.appendChar", 2);
                    }
                }
                else throw new SyntaxException(tokenizer.ToXML(), LineNumber, semantics.ClassName);
            }
            else if (tokenizer.OnType(TokenType.IDENTIFIER))
            {
                var termType = TermType.NONE;
                using (var _ = tokenizer.LookAhead())
                {
                    tokenizer.Advance();
                    if (tokenizer.OnSymbol('.') || tokenizer.OnSymbol('(')) termType = TermType.SUBROUTINE_CALL;
                    else if (tokenizer.OnSymbol('[')) termType = TermType.ARRAY_ENTRY;
                    else termType = TermType.VARIABLE;
                }
                if (termType == TermType.SUBROUTINE_CALL)
                {
                    CompileSubroutineCall();
                }
                else if (termType == TermType.VARIABLE || termType == TermType.ARRAY_ENTRY)
                {
                    var symbolName = semantics.PeekIdentifier();
                    var symbolType = symbols.KindOf(symbolName);
                    semantics.ConsumeIdentifier(symbolType);
                    if (termType == TermType.ARRAY_ENTRY)
                    {
                        semantics.ExpectSymbol('[');
                        CompileExpression();
                        semantics.ExpectSymbol(']');
                        switch (symbolType)
                        {
                            case SymbolKind.FIELD: writer.WritePush(Segment.THIS, symbols.IndexOf(symbolName)); break;
                            case SymbolKind.STATIC: writer.WritePush(Segment.STATIC, symbols.IndexOf(symbolName)); break;
                            case SymbolKind.VAR: writer.WritePush(Segment.LOCAL, symbols.IndexOf(symbolName)); break;
                            case SymbolKind.ARG: writer.WritePush(Segment.ARG, symbols.IndexOf(symbolName)); break;
                            default: if (!config.SkipSymbols) { throw new SyntaxException(tokenizer.ToXML(), LineNumber, semantics.ClassName); } break;
                        }
                        writer.WriteArithmetic(Command.ADD);
                        writer.WritePop(Segment.POINTER, 1);
                        writer.WritePush(Segment.THAT, 0);
                    }
                    else
                    {
                        switch (symbolType)
                        {
                            case SymbolKind.FIELD: writer.WritePush(Segment.THIS, symbols.IndexOf(symbolName)); break;
                            case SymbolKind.STATIC: writer.WritePush(Segment.STATIC, symbols.IndexOf(symbolName)); break;
                            case SymbolKind.VAR: writer.WritePush(Segment.LOCAL, symbols.IndexOf(symbolName)); break;
                            case SymbolKind.ARG: writer.WritePush(Segment.ARG, symbols.IndexOf(symbolName)); break;
                            default: if (!config.SkipSymbols) { throw new SyntaxException(tokenizer.ToXML(), LineNumber, semantics.ClassName); } break;
                        }
                    }
                }
                else throw new SyntaxException(tokenizer.ToXML(), LineNumber, semantics.ClassName);
            }
            else if (tokenizer.OnKeyWord(TokenizerExtensions.ConstantKeywords))
            {
                var keyword = semantics.ExpectKeyword(TokenizerExtensions.ConstantKeywords);
                switch (keyword)
                {
                    case KeyWord.TRUE: writer.WritePush(Segment.CONST, 0); writer.WriteArithmetic(Command.NOT); break;
                    case KeyWord.FALSE: writer.WritePush(Segment.CONST, 0); break;
                    case KeyWord.NULL: writer.WritePush(Segment.CONST, 0); break;
                    case KeyWord.THIS: writer.WritePush(Segment.POINTER, 0); break;
                    default: throw new SyntaxException(tokenizer.ToXML(), LineNumber, semantics.ClassName);
                }
            }
            else if (tokenizer.OnSymbol(TokenizerExtensions.UnaryOpSymbols))
            {
                var op = semantics.ExpectSymbol(TokenizerExtensions.UnaryOpSymbols);
                CompileTerm();
                switch (op)
                {
                    case '-': writer.WriteArithmetic(Command.NEG); break;
                    case '~': writer.WriteArithmetic(Command.NOT); break;
                    default: throw new SyntaxException(tokenizer.ToXML(), LineNumber, semantics.ClassName);
                }
            }
            else if (tokenizer.OnSymbol('('))
            {
                semantics.ExpectSymbol('(');
                CompileExpression();
                semantics.ExpectSymbol(')');
            }
            else
                throw new SyntaxException(tokenizer.ToXML(), LineNumber, semantics.ClassName);
            semantics.EndScope("term");
        }

        public void CompileExpressionList()
        {
            int c = 0;
            semantics.BeginScope("expressionList");
            if (!tokenizer.OnSymbol(')'))
            {
                CompileExpression();
                c++;
                while (tokenizer.OnSymbol(','))
                {
                    semantics.ExpectSymbol(',');
                    CompileExpression();
                    c++;
                }
            }
            nArgs.Push(c);
            semantics.EndScope("expressionList");
        }

        
        public string DebugOutput => semantics.DebugOut;
        public int LineNumber => ((JackTokenizer)tokenizer).LineNumber;
        public int StackLength => stackLengthTracker.StackLength;

        private void CompileSubroutineCall()
        {
            int argAdjustment = 0;
            var symbolName = semantics.PeekIdentifier();
            var symbolKind = symbols.KindOf(symbolName);
            var fullCallIdentifier = string.Empty;
            char nextSymbol;
            using (var _ = tokenizer.LookAhead())
            {
                tokenizer.Advance();
                try
                {
                    nextSymbol = tokenizer.Symbol();
                }
                catch
                {
                    throw new SyntaxException(tokenizer.ToXML(), LineNumber, semantics.ClassName);
                }
            }
            if (symbolKind == SymbolKind.NONE && nextSymbol == '(') // method call
            {
                argAdjustment = 1;
                writer.WritePush(Segment.POINTER, 0); // Not sure why pointer here
                var methodName = semantics.ConsumeIdentifier("subroutine");
                fullCallIdentifier = $"{semantics.ClassName}.{methodName}";
            }
            else if (nextSymbol == '.') // CLASS OR VAR
            {
                var lhs = String.Empty;
                if (symbolKind == SymbolKind.NONE) // CLASS
                {
                    lhs = semantics.ConsumeIdentifier("class");
                }
                else // VAR
                {
                    argAdjustment = 1;
                    lhs = symbols.TypeOf(symbolName);
                    semantics.ConsumeIdentifier("var");
                    switch (symbolKind)
                    {
                        case SymbolKind.FIELD: writer.WritePush(Segment.THIS, symbols.IndexOf(symbolName)); break;
                        case SymbolKind.STATIC: writer.WritePush(Segment.STATIC, symbols.IndexOf(symbolName)); break;
                        case SymbolKind.VAR: writer.WritePush(Segment.LOCAL, symbols.IndexOf(symbolName)); break;
                        case SymbolKind.ARG: writer.WritePush(Segment.ARG, symbols.IndexOf(symbolName)); break;
                        default: if (!config.SkipSymbols) { throw new SyntaxException(tokenizer.ToXML(), LineNumber, semantics.ClassName); } break;
                    }
                }
                semantics.ExpectSymbol('.');
                var routineName = semantics.ConsumeIdentifier("subroutine");
                fullCallIdentifier = $"{lhs}.{routineName}";
            }
            else throw new SyntaxException(tokenizer.ToXML(), LineNumber, semantics.ClassName);
            semantics.ExpectSymbol('(');
            CompileExpressionList();
            semantics.ExpectSymbol(')');
            writer.WriteCall(fullCallIdentifier, nArgs.Pop() + argAdjustment);
        }

        private readonly JackConfiguration config;
        private JackCompilationSemanticsEngine semantics;
        private readonly ISymbolTable symbols;
        private readonly ITokenizer tokenizer;
        private readonly IVMWriter writer;
        private readonly IJackCompilationState stackLengthTracker;
        private int whileCookie = 0;
        private int ifCookie = 0;
        private Stack<int> nArgs = new Stack<int>();
    }
}
