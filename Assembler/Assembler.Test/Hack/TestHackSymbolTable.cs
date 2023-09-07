using Assembler.Logic.Hack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Test.Hack
{
    public class TestHackSymbolTable
    {
        [Test]
        public void TestHackSymbolTableEndToEnd()
        {
            var symbols = new HackSymbolTable();
            var symbolsToAdd = new KeyValuePair<string, int>[]
            {
                new KeyValuePair<string, int>("floo", 3),
                new KeyValuePair<string, int>("biff", 300),
                new KeyValuePair<string, int>("buzz", 9)
            };
            foreach (var symbolToAdd in symbolsToAdd)
            {
                Assert.IsFalse(symbols.contains(symbolToAdd.Key));
            }
            foreach (var symbolToAdd in symbolsToAdd)
            {
                symbols.addEntry(symbolToAdd.Key, symbolToAdd.Value);
            }
            foreach (var symbolToAdd in symbolsToAdd)
            {
                Assert.IsTrue(symbols.contains(symbolToAdd.Key));
                Assert.That(symbols.getAddress(symbolToAdd.Key), Is.EqualTo(symbolToAdd.Value));
            }
        }

        [Test]
        public void TestCannotAddSymbolTwice()
        {
            var symbols = new HackSymbolTable();
            Assert.Throws<InvalidOperationException>(() =>
            {
                symbols.addEntry("floo", 1);
                symbols.addEntry("floo", 2);
            });
        }

        [Test]
        public void TestPreDefinedSymbol()
        {
            var symbols = new HackSymbolTable();
            var preDefinedSymbol = "SP";
            Assert.True(symbols.contains(preDefinedSymbol));
            Assert.That(symbols.getAddress(preDefinedSymbol), Is.EqualTo(0));
        }
    }
}
