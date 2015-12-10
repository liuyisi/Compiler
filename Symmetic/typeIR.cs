using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Common;
using Compiler.Semantic;

namespace Compiler.Semantic
{
    public class typeIR
    {
        public integer Count = new integer(0);
        public TypeKind Kind;
        public more More = new more();			//	表示数组和域
        public class more
        {
            public arrayAttr ArrayAttr = new arrayAttr();
            public class arrayAttr
            {
                public typeIR indexTy;
                public typeIR elemTy;
                public int low, top;
            }
            public fieldChain body;
        }
        public void copy(typeIR b)
        {
            this.Count.value = b.Count.value;
            this.Kind = b.Kind;
            this.More.ArrayAttr.low = b.More.ArrayAttr.low;
            this.More.ArrayAttr.top = b.More.ArrayAttr.top;
            this.More.ArrayAttr.elemTy = b.More.ArrayAttr.elemTy;
            this.More.ArrayAttr.indexTy = b.More.ArrayAttr.indexTy;
            this.More.body = b.More.body;
        }
        public bool equals(typeIR b)
        {
            if (this.Count.value != b.Count.value) return false;
            if (this.Kind != b.Kind) return false;
            if (this.More.ArrayAttr.low != b.More.ArrayAttr.low) return false;
            if (this.More.ArrayAttr.top != b.More.ArrayAttr.top) return false;
            if (this.More.ArrayAttr.elemTy != b.More.ArrayAttr.elemTy) return false;
            if (this.More.ArrayAttr.indexTy != b.More.ArrayAttr.indexTy) return false;
            if (this.More.body != b.More.body) return false;
            return true;
        }
    }

}
