using Assembler.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Test
{
    public class TestBinaryTool
    {
        [Test]
        public void TestExpandIntegerBinary()
        {
            var sevenExpanded = BinaryTool.ExpandIntegerBinary(7, 15);
            Assert.That(sevenExpanded, Is.EqualTo("000000000000111"));
        }
    }
}
