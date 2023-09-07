namespace JackCompiler.Lib.Test
{
    public class TestsAnalyzer
    {
        [SetUp]
        public void Setup()
        {
        }
        

        [Test]
        public void TestExpressionlessSquare()
        {
            Helpers.TestAnalyzeSample("ExpressionlessSquare/Main");
            Helpers.TestAnalyzeSample("ExpressionlessSquare/Square");
            Helpers.TestAnalyzeSample("ExpressionlessSquare/SquareGame");
        }

        [Test]
        public void TestSquareDance()
        {
            Helpers.TestAnalyzeSample("Square/Main");
            Helpers.TestAnalyzeSample("Square/Square");
            Helpers.TestAnalyzeSample("Square/SquareGame");
        }

        [Test]
        public void TestArray()
        {
            Helpers.TestAnalyzeSample("ArrayTest/Main");
        }
    }
} 