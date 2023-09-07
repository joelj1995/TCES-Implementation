using JackCompiler.Lib.Interface;
using JackCompiler.Lib.Jack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JackCompiler.Lib.Test.Jack
{
    internal class TestJackSymbolTable
    {
        [SetUp] 
        public void SetUp() 
        {
            symbolTable = new JackSymbolTable(new JackConfiguration());
        }

        [Test]
        public void TestClassScopeAddAndGet()
        {
            var name = "testymctest";
            var type = "int";
            var kind = SymbolKind.STATIC;
            symbolTable.Define(name, type, kind);
            Assert.That(symbolTable.TypeOf(name), Is.EqualTo(type));
            Assert.That(symbolTable.KindOf(name), Is.EqualTo(kind));
            Assert.That(symbolTable.IndexOf(name), Is.EqualTo(0));
        }

        [Test]
        public void TestCannotRedefineSymbol()
        {
            var name = "testymctest";
            var type = "int";
            var kind = SymbolKind.STATIC;
            symbolTable.Define(name, type, kind);
            Assert.Throws<InvalidOperationException>(() =>
            {
                symbolTable.Define(name, type, kind);
            });
        }

        [Test]
        public void TestSubroutineScopeAddAndGet()
        {
            var nameClass = "testymctest";
            var typeClass = "int";
            var kindClass = SymbolKind.STATIC;
            symbolTable.Define(nameClass, typeClass, kindClass);
            symbolTable.StartSubroutine();
            var name = "flootest";
            var type = "string";
            var kind = SymbolKind.VAR;
            symbolTable.Define(name, type, kind);
            Assert.That(symbolTable.TypeOf(name), Is.EqualTo(type));
            Assert.That(symbolTable.KindOf(name), Is.EqualTo(kind));
            Assert.That(symbolTable.IndexOf(name), Is.EqualTo(0));
        }

        [Test]
        public void TestValidateKindForScope()
        {
            var name = "testymctest";
            var type = "int";
            Assert.Throws<ArgumentException>(() =>
            {
                symbolTable.Define(name, type, SymbolKind.ARG);
            });
            Assert.Throws<ArgumentException>(() =>
            {
                symbolTable.Define(name, type, SymbolKind.VAR);
            });
            symbolTable.StartSubroutine();
            Assert.Throws<ArgumentException>(() =>
            {
                symbolTable.Define(name, type, SymbolKind.FIELD);
            });
            Assert.Throws<ArgumentException>(() =>
            {
                symbolTable.Define(name, type, SymbolKind.STATIC);
            });
        }

        [Test]
        public void TestTypeOfUnderfinedReturnsNone()
        {
            var kind = symbolTable.KindOf("floo");
            Assert.That(kind, Is.EqualTo(SymbolKind.NONE));
        }

        [Test]
        public void TestVarCount()
        {
            var nameClass = "testymctest";
            var typeClass = "int";
            var kindClass = SymbolKind.STATIC;
            symbolTable.Define(nameClass, typeClass, kindClass);
            Assert.That(symbolTable.VarCount(SymbolKind.STATIC), Is.EqualTo(1));
            symbolTable.StartSubroutine();
            var type = "string";
            var kind = SymbolKind.VAR;
            symbolTable.Define("var1", type, kind);
            symbolTable.Define("var2", type, kind);
            symbolTable.Define("var3", type, kind);
            kind = SymbolKind.ARG;
            symbolTable.Define("arg1", type, kind);
            symbolTable.Define("arg2", type, kind);
            Assert.That(symbolTable.VarCount(SymbolKind.VAR), Is.EqualTo(3));
            Assert.That(symbolTable.VarCount(SymbolKind.ARG), Is.EqualTo(2));
        }

        private ISymbolTable symbolTable;
    }
}
