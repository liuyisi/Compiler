using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Semantic;

namespace Compiler.Semantic
{
    public class Scope
    {
        public Stack<ScopeNode> scope = new Stack<ScopeNode>();

        public SymTableNode FindID(String idname, bool ntype)
        {
            /***********************/
            /*从符号表栈中查找已经声明的标示符*/
            /****ntype=1时遍历所有层****/
            /****ntype=0时遍历当前层****/
            /***********************/
            ScopeNode ptr = scope.Peek();
            if (0 == scope.Count) return null;

            while (ptr != null)
            {
                SymTableNode nptr = new SymTableNode();
                nptr = ptr.front;
                while (nptr != null)
                {
                    if (nptr.name == null) { }
                    else if (nptr.name.Equals(idname))
                    {
                        return nptr;
                    }
                    if (nptr.EOFL == true) break;
                    nptr = nptr.next;
                }
                if (ntype == true) ptr = ptr.parent;
                else break;
            }
            return null;
        }

        public SymTableNode GetRear()
        {
            SymTableNode p = scope.Peek().front;
            while (p != null)
            {
                if (p.EOFL == true || p.next == null) return p;
                p = p.next;
            }
            return null;
        }

        public bool newLayer(SymTableNode ptr)
        {
            /***************/
            /******新建一层****/
            /***************/
            ScopeNode nptr = new ScopeNode();
            nptr.front = ptr;
            if (0 == scope.Count) nptr.parent = null;
            else nptr.parent = scope.Peek();
            scope.Push(nptr);
            return true;
        }

        public bool DropLayer()
        {
            /***************/
            /******删除一层****/
            /***************/
            if (0 == scope.Count)  return false;
            scope.Pop();
            return true;
        }

    }

}
