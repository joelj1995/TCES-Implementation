using JackCompiler.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Jack
{
    internal static class XmlGen
    {
        public static string IdentifierDefinition(string name, string category)
        {
            return IdentifierDefinition(name, category, SymbolKind.NONE);
        }

        public static string IdentifierDefinition(string name, string category, SymbolKind kind)
        {
            return Identifier("identifierDefinition", name, category, kind);
        }

        public static string IdentifierOperation(string name, string category)
        {
            return IdentifierOperation(name, category, SymbolKind.NONE);
        }

        public static string IdentifierOperation(string name, string category, SymbolKind kind)
        {
            return Identifier("identifierOperation", name, category, kind);
        }

        public static string Identifier(string tag, string name, string category, SymbolKind kind)
        {
            if (!identifierCatgories.Contains(category)) throw new ArgumentException(category);
            return $"<{tag}><name> {name} </name><category> {category} </category><kind> {kind.ToString()} </kind></{tag}>";
        }

        public static readonly ICollection<string> identifierCatgories = new string[] { "var", "arg", "static", "field", "class", "subroutine" };
    }
}
