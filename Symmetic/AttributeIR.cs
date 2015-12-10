using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Common;
using Compiler.Semantic;

namespace Compiler.Semantic
{
    public class AttributeIR{
	public typeIR idtype = new typeIR();	// 标识符和类型的内部表示
	public IdKind kind ;				// 符号表 是 类型，标识符，变量，还是过程
	public int level ;						// 层数
	public more More = new more() ;			
	public class more {
		public varAttr VarAttr = new varAttr() ;		// 标识符的信息
		public procAttr ProcAttr = new procAttr() ;		// 过程定义的信息
	}
	public class procAttr {					
		public ParamTable param = new ParamTable() ;
		public int code ;
		public integer Count = new integer(0) ;
	}
	public class varAttr {
		public AccessKind access ;
		public integer offset = new integer(0) ;
		public varAttr(){
		}
	}
}
}
