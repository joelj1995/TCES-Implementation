using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack
{
    internal class HackCodeWriterContext
    {
        public int StaticSegmentPointerValue { get; set; } = 100;
        public bool InitWritten { get => initWritten; }
        public string? FileName { get; set; } = null;

        public int nextSymbolCookie()
        {
            return ++symbolCookie;
        }

        public void setInitWritten()
        {
            if (initWritten) throw new InvalidOperationException("init has already been written");
            initWritten = true;
        }

        private int symbolCookie = 0;
        private bool initWritten = false;
    }
}
