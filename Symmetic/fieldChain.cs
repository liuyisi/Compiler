using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Semantic
{
    public class fieldChain
    {
        public String idname;
        public typeIR unitType;
        public integer offset;
        public fieldChain next;
    }
}
