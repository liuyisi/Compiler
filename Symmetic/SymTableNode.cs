using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Semantic
{
    public class SymTableNode
    {
        public String name;							// 符号表项的名称
        public AttributeIR attrIR = new AttributeIR(); // 符号表项的信息
        public bool EOFL;							// 是否是本层符号表结束
        public SymTableNode next;							// 指向下一个符号表项
    }
}
