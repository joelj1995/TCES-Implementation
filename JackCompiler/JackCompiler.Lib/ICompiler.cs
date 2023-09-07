using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib
{
    public interface ICompiler
    {
        /// <summary>
        /// Compile an input path.
        /// </summary>
        /// <param name="path">Path to a vm file or a directory containing 1+ jack files</param>
        public void Compile(string path);
    }
}
