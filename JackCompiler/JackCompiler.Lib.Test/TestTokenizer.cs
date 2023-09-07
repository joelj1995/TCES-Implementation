using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Test
{
    internal class TestTokenizer
    {
        [Test]
        public void TestExpressionlessSquare()
        {
            Helpers.CompareTokens("ExpressionlessSquare/Main");
            Helpers.CompareTokens("ExpressionlessSquare/Square");
            Helpers.CompareTokens("ExpressionlessSquare/SquareGame");
        }

        [Test]
        public void TestSquareDance()
        {
            Helpers.CompareTokens("Square/Main");
            Helpers.CompareTokens("Square/Square");
            Helpers.CompareTokens("Square/SquareGame");
        }

        [Test]
        public void TestArray()
        {
            Helpers.CompareTokens("ArrayTest/Main");
        }
    }
}
