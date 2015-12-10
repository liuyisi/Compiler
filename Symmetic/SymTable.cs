using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Common;

namespace Compiler.Semantic
{
    public class SymTable
    {
        public SymTableNode front;
        public SymTableNode rear;
        public int maxLayer;

        public bool insertNode(SymTableNode ptr)
        {
            /***************/
            /*向符号表插入一个节点*/
            /***************/
            if (ptr == null) return false;
            if (ptr.attrIR.kind != IdKind.prockind) ptr.EOFL = false;
            ptr.next = null;
            if (ptr.attrIR.kind == IdKind.prockind)
            {
                int l = ptr.attrIR.level;
                if (l > maxLayer) maxLayer = l;
            }
            if (ptr.attrIR.kind == IdKind.varkind)
            {
                int l = ptr.attrIR.level;
                if (l > maxLayer) maxLayer = l;
            }
            if (this.front == null)
            {
                front = rear = ptr;
            }
            else
            {
                rear.next = ptr;
                rear = rear.next;
            }
            return true;
        }

        public bool insertMid(SymTableNode pre, SymTableNode next)
        {
            if (pre.next == null) pre.next = next;
            else
            {
                SymTableNode p = pre.next;
                pre.next = next;
                next.next = p;
            }
            return true;
        }

        public String OutToCmd( )
        {
            String output = "";
            if (front == null) return output;
            SymTableNode ptr;
            for (int i = 0; i <= maxLayer; i++)
            {
                output += "--------------------------SymTable in Layer " + i + "\n";
                ptr = front;
                while (ptr != null)
                {
                    if ((ptr.attrIR.kind != IdKind.prockind && ptr.attrIR.level == i) ||
                            (ptr.attrIR.kind == IdKind.prockind && ptr.attrIR.level - 1 == i))
                    {
                        if (ptr.attrIR.kind == IdKind.typekind)
                        {
                            output += ptr.name + "\t" + ptr.attrIR.idtype.Kind + "\t" + ptr.attrIR.kind + "\n";
                        }
                        else if (ptr.attrIR.kind ==IdKind.prockind)
                        {
                            output += ptr.name + "\t" + ptr.attrIR.kind + "\t level: " + ptr.attrIR.level + "\t Noff: " + ptr.attrIR.More.ProcAttr.Count.value + "\n";
                        }
                        else if (ptr.attrIR.kind ==IdKind.varkind)
                        {
                            output += ptr.name + "\t" + ptr.attrIR.idtype.Kind + "\t" + ptr.attrIR.kind + "\t level: " + ptr.attrIR.level
                                    + "\t offset: " + ptr.attrIR.More.VarAttr.offset.value + "\t" + ptr.attrIR.More.VarAttr.access + "\n";
                        }
                    }
                    ptr = ptr.next;
                }
            }
            return output;
        }
        public SymTableNode getFront()
        {
            return front;
        }
        public SymTableNode getRear()
        {
            return rear;
        }
    }
}
