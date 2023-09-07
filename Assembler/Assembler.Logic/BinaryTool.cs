using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Logic
{
    public static class BinaryTool
    {
        public static string ExpandIntegerBinary(int input, ushort len)
        {
            var binaryText = Convert.ToString(input, 2);
            if (len < binaryText.Length)
                throw new ArgumentException($"Converted length {binaryText.Length} exceededs padding length {len}");
            var padding = string.Concat(Enumerable.Repeat("0", len - binaryText.Length));
            return padding + binaryText;
        }
    }
}
