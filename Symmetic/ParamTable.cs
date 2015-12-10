using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Semantic
{
    public class ParamTable
    {
        public int a;
        public typeIR type = new typeIR();
        public ParamTable next;
        public SymTableNode symPtr;
    }
}
