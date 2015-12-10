using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Semantic
{
    public class ScopeNode
    {
        public SymTableNode front;
        public ScopeNode parent;
    }
}
