using Assembler.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Test
{
    public class TestAssemblerForProgramWithSymbols
    {
        IAssembler assembler;

        [SetUp]
        public void Setup()
        {
            assembler = AssemblerFactory.CreateAssembler("Hack");
        }

        [Test]
        public void TestMax()
        {
            Helpers.TestAssemble("Max", assembler);
        }

        [Test]
        public void TestPong()
        {
            Helpers.TestAssemble("Pong", assembler);
        }

        [Test]
        public void TestRect()
        {
            Helpers.TestAssemble("Rect", assembler);
        }
    }
}
