using JackCompiler.Lib.Interface;
using JackCompiler.Lib.Jack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Test.Jack
{
    internal class TestXmlGen
    {
        [Test]
        public void TestIdentifierDefinition()
        {
            var expected = @"
                <identifierDefinition>
	                <name> foo </name>
	                <category> var </category>
	                <kind> VAR </kind>
                </identifierDefinition>
            ";
            var actual = XmlGen.IdentifierDefinition("foo", "var", SymbolKind.VAR);
            Helpers.AssertXmlEqual(expected, actual);
        }

        [Test]
        public void TestIdentifierOperation()
        {
            var expected = @"
                <identifierOperation>
	                <name> foo </name>
	                <category> var </category>
	                <kind> VAR </kind>
                </identifierOperation>
            ";
            var actual = XmlGen.IdentifierOperation("foo", "var", SymbolKind.VAR);
            Helpers.AssertXmlEqual(expected, actual);
        }
    }
}
