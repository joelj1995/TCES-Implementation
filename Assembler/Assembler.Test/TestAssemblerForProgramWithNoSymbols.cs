using Assembler.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Test
{
    public class TestAssemblerForProgramWithNoSymbols
    {
        
        IAssembler assembler;

        [SetUp]
        public void Setup()
        {
            assembler = AssemblerFactory.CreateAssembler("Hack");
        }

        [Test]
        public void TestAdd()
        {
            Helpers.TestAssemble("Add", assembler);
        }

        [Test]
        public void TestMax()
        {
            Helpers.TestAssemble("MaxL", assembler);
        }

        [Test]
        public void TestPong()
        {
            Helpers.TestAssemble("PongL", assembler);
        }

        [Test]
        public void TestRect()
        {
            Helpers.TestAssemble("RectL", assembler);
        }
    }
}
