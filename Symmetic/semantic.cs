using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Common;
using Compiler.Parser;
using System.Collections;

namespace Compiler.Semantic
{

    class semantic {
	private TreeNode root ;
	public SymTable symTable = new SymTable() ;
	public Scope myScope = new Scope() ;
	public List<semError> ErrorList = new  List<semError>() ;
	public int ErrorNum ;
    public const int OFFSET = 0;  //所在层数
	public parser parser ;
	public static String output ; 

    public int Line { get; set; }  //源程序字符所在行
    public int Row { get; set; }  //源程序字符所在列
    public int Cur { get; set; }  //当前字符
    public ErrorType Error { get; set; }  //报错错误对象声明
    public String Buffer { get; set; }

    public semantic()
        {
            Line = 1;
            Row  = 1;
            Cur  = 0;
            Error = new ErrorType();
            parser = new parser();
        }
	
	public static typeIR IntTy , CharTy , BoolTy ;
	
	private void initTy() {
		IntTy = new typeIR() ;
		CharTy = new typeIR() ;
		BoolTy = new typeIR() ;
		
		IntTy.Kind = TypeKind.intTy ;
		IntTy.Count.value = 2 ;
		
		CharTy.Kind = TypeKind.charTy ;
		CharTy.Count.value = 1 ;
		
		BoolTy.Kind = TypeKind.boolTy ;
		BoolTy.Count.value = 1 ;
	}

    private void error(String str, int Line, int Row)
    {
		if( Error.isError == true ) {
            // 错误处理
            Error.Line = Line;
            Error.Row = Row;
            Error.Type = ErrorType.errorType.SemanticError;
		}
		ErrorNum ++ ;
		
		semError seError = new semError() ;
        seError.line = Line;
		seError.row = Row ;
		seError.type = str ;
		
		ErrorList.Add( seError ) ;
	}
	
	public bool OutErrorCmd() {
		if( ErrorNum == 0 ) return false ;
		for( int i = 0 ; i < ErrorList.Count() ; i ++ ){
			int line ; 
			line = ErrorList[i].line / 2 ;
			if( ErrorList[i].line / 2 == 0 ) line = 1 ; 
			if( ErrorList[i].line == 3 ) line = 2 ; 
			output += "行： " + line  + "   列：  " + ErrorList[i].row + "		" + ErrorList[i].type + "\n" ;
		}
		return true ;
	}
	
	private TreeNode Search( TreeNode Root , TreeNode ptr , int ntype ) {
		/*********************/
		/*ntype=0时，仅遍历同一层**/
		/*ntype=1时，仅遍历最左分支*/
		/*ntype=2时，遍历所有子树**/
		/*ntype=3时，遍历所有子树**/
		/*********************/
		
		if( Root == null ) return null ;
        if (Root.ChildNum == 0) return null;
		int  i ;
        if (Root.IsTerminal == ptr.IsTerminal)
        {
			if( Root.IsTerminal == false && Root.Terminal.Equals( ptr.Terminal )  ) return Root ;
			if( Root.IsTerminal == true && Root.NonTerminal.Equals( ptr.NonTerminal )  ) return Root ;
		}
		if( ntype == 0 ) {
			for( i = 0 ; i < Root.ChildNum ; i ++ ) {
				TreeNode temp = Root.childs[i] ;
				if( temp.IsTerminal == false && temp.Terminal.Equals(ptr.Terminal)) return temp ;
				if( temp.IsTerminal == true && temp.NonTerminal.Equals(ptr.NonTerminal)) return temp ;
			} 
		}
		else if( ntype == 1 ) {
			return Search( Root.childs[0] , ptr , ntype ) ;
		}
		else if( ntype == 2 ) {
			for( i = 0 ; i < Root.ChildNum ; i ++ ) {
				TreeNode temp = Search( Root.childs[i] , ptr , ntype );
				if( temp != null ) return temp ;
			}
		}
		else if( ntype == 3 ) {
			for( i = Root.ChildNum - 1 ; i >= 0 ; i -- ) {
				TreeNode temp = Search( Root.childs[i] , ptr , ntype );
				if( temp != null ) return temp ;
			}
		}
		return null ;
	}
	
	public void OutToCmd() {
		output += symTable.OutToCmd() ;
	}
	
	public SymTable OutSymTable() {
		return symTable ;
	}
	
	public String SemScanner( String filePath ){
		output = "" ;
		root = parser.getTree( filePath ) ;
       // output += parser.getOutput( filePath ) ;
        //Console.WriteLine(output);
        if( root == null )
		return output ;
		initTy() ;
		ErrorNum = 0 ;
		program( root ) ;
		OutErrorCmd() ;
        if (ErrorList.Count() == 0)
        {
            Console.WriteLine("ceshi");
            OutToCmd();
        }

		return output ;
	}
	
	private void program( TreeNode Root ) {
		TreeNode p ;
		TreeNode tar = new TreeNode() ;
		tar.IsTerminal=true ; 	tar.NonTerminal= nonTerminals.DeclarePart ;
		
		p = Search( Root , tar , 0 ) ;
		if( p != null ) {
			SymTableNode ptr = new SymTableNode() ;
			ptr.EOFL = true ;
			myScope.newLayer( null ) ;
			integer offset = new integer( OFFSET ) ;
			declarePart( p , 0 , offset ) ;
		}
		
		tar.NonTerminal = nonTerminals.ProgramBody  ;
		p = Search( Root , tar , 0 ) ;
		if( p != null ) {
			programBody( p ) ;
		}
	}
	
	private void declarePart( TreeNode ptr , int layer , integer offset ) {
		TreeNode p ; 
		TreeNode tar = new TreeNode() ;
		tar.IsTerminal=true ;  tar.NonTerminal = nonTerminals.TypeDecpart  ;
		p = Search( ptr , tar , 1 ) ;
		if( p != null ) typeDecpart( p , layer ) ;
		
		tar.NonTerminal = nonTerminals.VarDecpart  ;
		
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) varDecpart( p , layer , offset ) ;
		
		ptr.symtPtr = myScope.scope.Peek().front ;
		
		tar.NonTerminal = nonTerminals.ProcDecpart ;
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) {
			tar.NonTerminal = nonTerminals.ProcDecpart ;
			TreeNode temp ;
			temp = Search( p , tar , 2 ) ;
			if( temp != null ) procDecpart( p , layer + 1 ) ;
		}
	}
	
	private void programBody( TreeNode ptr ) {
		TreeNode p ; 
		TreeNode tar = new TreeNode() ;
		tar.IsTerminal=true ;  tar.NonTerminal = nonTerminals.StmList  ;
		
		p = Search( ptr , tar , 0 ) ;
		
		if( p != null ) stmList( p ) ;
	}
	
	/***************/
	/*声明部分的各分块部分*/
	/***************/
	private void typeDecpart( TreeNode ptr , int layer ) {
		TreeNode p ; 
		TreeNode tar = new TreeNode() ;
		
		tar.IsTerminal=true ; tar.NonTerminal =nonTerminals.TypeDecList  ;
		if( ptr.ChildNum > 0 && ptr.childs[0].ChildNum == 0 ) return ;
		
		p = Search( ptr , tar , 2 ) ;
		if( p != null ) typeDecList( p , layer ) ;
	}
	
	private void varDecpart( TreeNode ptr , int layer , integer offset ) {
		TreeNode p ; 
		TreeNode tar = new TreeNode() ;
		
		tar.IsTerminal=true ;  tar.NonTerminal = nonTerminals.VarDecList  ;
		
		if( ptr.ChildNum != 0 && ptr.childs[0].ChildNum == 0 ) return ;
		
		p = Search( ptr , tar , 2 ) ;
		
		if( p != null ) varDecList( p , layer , offset ) ;
		
	}
	
	private void procDecpart( TreeNode ptr , int layer ) {
		TreeNode p ;
		TreeNode tar = new TreeNode() ;
		
		tar.IsTerminal=true ;  tar.NonTerminal = nonTerminals.ProcDec ;
		
		p = Search( ptr , tar , 1 ) ;
		
		if( p != null ) {
			procDec( p , layer , false ) ;
			tar.NonTerminal = nonTerminals.ProcDecMore ;
			p = Search( p , tar , 0 ) ;
			if( p != null ) {
				tar.NonTerminal = nonTerminals.ProcDec ;
				p = Search( p , tar , 1 ) ;
				if( p != null ) procDec( p , layer , true ) ;
			}
		}
	}
	/**************/
	/*****类型声明****/
	/**************/
	private void typeDecList( TreeNode ptr , int layer ) {
		TreeNode q ; 
		TreeNode tar = new TreeNode() ;
		if( ptr.ChildNum != 0 ) {
			SymTableNode p = new SymTableNode() ; 
			p.name = ptr.childs[0].childs[0].Data ;
			p.attrIR.kind = IdKind.typekind ;
			p.attrIR.level = layer ;
			p.next = new SymTableNode() ; 
			
			tar.IsTerminal=true ;  tar.NonTerminal =  nonTerminals.TypeDef  ;
			q = Search( ptr , tar , 0 ) ;
			if( q != null ) {
				typeDef( q , p.attrIR.idtype , layer ) ;
			}
            
			if( p.attrIR.idtype != null ) {	
				symTable.insertNode( p ) ;
				if( myScope.scope.Peek().front == null ) {
					ptr.symtPtr = p ;
					myScope.scope.Peek().front = p ; 
				}
			}
			else {
				String str ;
				str = "语义错误：		类型标识符" + p.name + "未定义" ;
				error( str , ptr.Line , ptr.Row ) ;
				p = null ; 
			}
			
		}
		
		tar.NonTerminal = nonTerminals.TypeDecMore ;
		q = Search( ptr , tar , 0 ) ;
		if( q != null ) {
			tar.NonTerminal = nonTerminals.TypeDecList ;
			q = Search( q , tar , 1 ) ;
			if( q != null ) typeDecList( q , layer ) ;
		}
	}
	
	private void typeDef( TreeNode ptr , typeIR tIR , int layer ) {
		if( ptr.ChildNum == 0 ) {
			tIR = null ; 
			return ;
		}
		if( ptr.childs[0].IsTerminal == true ) {
			if( ptr.childs[0].NonTerminal.Equals(nonTerminals.BaseType ) ) {
				if( ptr.childs[0].childs[0].Data.Equals( "integer" ) ) tIR.copy( IntTy ) ;
				else if( ptr.childs[0].childs[0].Data.Equals( "char" ) ) tIR.copy( CharTy ) ;
			}
			else if( ptr.childs[0].NonTerminal.Equals(nonTerminals.StructureType ) ) 
				structureType( ptr.childs[0] , tIR , layer ) ;
		}
		else if( ptr.childs[0].Terminal.Equals( LexType.ID ) ) {
			SymTableNode p = myScope.FindID( ptr.childs[0].Data , true ) ;
			
			if( p == null ) {
				String str ; 
				str = "语义错误：		类型标识符" + ptr.childs[0].Data + "未定义" ;
				error( str , ptr.childs[0].Line , ptr.childs[0].Row ) ;
				tIR = null ; 
			}
			else {
				if( p.attrIR.kind != IdKind.typekind ) {
					String str ;
					str = "语义错误：		" + ptr.childs[0].Data + "非类型标识符" ;
					error( str , ptr.childs[0].Line , ptr.childs[0].Row ) ;
					tIR = null ;
				}
				else tIR.copy( p.attrIR.idtype ) ;
			}
		}
	} 
	
	private void structureType( TreeNode ptr , typeIR tIR , int layer ) {
		TreeNode p ; 
		TreeNode tar = new TreeNode() ;

        tar.IsTerminal = true; tar.NonTerminal = nonTerminals.ArrayType;
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) {
			arrayType( p , tIR , layer ) ;
			return ;
		}
		
		tar.NonTerminal = nonTerminals.RecType ;
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) {
			recType( p , tIR , layer ) ;
			return ;
		}
		tIR = null ; 
	}
	
	private void arrayType( TreeNode ptr , typeIR tIR , int layer ) {
		tIR.Kind = TypeKind.arrayTy ;
		
		TreeNode p = ptr.childs[2] ;
		if( p.NonTerminal.Equals(nonTerminals.Low) )
            tIR.More.ArrayAttr.low = int.Parse(p.childs[0].Data);

        p = ptr.childs[4];
		if( p.NonTerminal.Equals(nonTerminals.Top) )
            tIR.More.ArrayAttr.top = int.Parse(p.childs[0].Data);
		
		tIR.More.ArrayAttr.indexTy = IntTy ;

        p = ptr.childs[7];
		if( p.childs[0].Terminal.Equals( LexType.INTEGER ) ) 
			tIR.More.ArrayAttr.elemTy = IntTy ;
		else tIR.More.ArrayAttr.elemTy = CharTy ;
		
		if( tIR.More.ArrayAttr.top <= tIR.More.ArrayAttr.low ) {
			String str = "语义错误：		数组上界小于了下界" ;
            error(str, ptr.childs[2].Line, ptr.childs[2].Row);
			return ;
		}
		
		tIR.Count.value = tIR.More.ArrayAttr.top - tIR.More.ArrayAttr.low + 1 ;
		tIR.Count.value *= tIR.More.ArrayAttr.elemTy.Count.value ;
	}
	
	private void recType( TreeNode ptr , typeIR tIR , int layer ) {
		tIR.Kind = TypeKind.fieldTy ;
		tIR.Count.value = 0 ;
		
		TreeNode p ;
		TreeNode tar = new TreeNode() ;
		tar.IsTerminal=true ;  tar.NonTerminal=nonTerminals.FieldDecList  ;
		
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) fieldDecList( ptr , tIR.More.body , tIR.Count , layer ) ;
	}
	
	private void fieldDecList( TreeNode ptr , fieldChain body , integer Count , int layer ) {
		TreeNode p ; 
		TreeNode tar = new TreeNode() ;
		tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.BaseType  ;
		
		p = Search( ptr , tar , 1 ) ;
		if( p != null ) {
			if( p.childs[0].Terminal.Equals( LexType.INTEGER ) ) 
				idList( p.father.childs[1] , p.father.childs[1] , body , layer , Count , IntTy ) ;
			else if( p.childs[0].Terminal.Equals( LexType.CHAR ) )
				idList( p.father.childs[1] , p.father.childs[1] , body , layer , Count , CharTy ) ;
			return ;
		}
		
		tar.NonTerminal = nonTerminals.ArrayType ;
		p = Search( ptr , tar , 1 ) ;
		if( p != null ) {
			typeIR tIR = new typeIR() ;
			arrayType( p , tIR , layer ) ;
			idList( p.father.childs[1] , p.father.childs[1] , body , layer , Count , tIR ) ;
		}
	}
	
	private void idList( TreeNode ptr , TreeNode parent , fieldChain body , int layer , integer Count , typeIR tIR ) {
		body = new fieldChain() ;
		TreeNode p ; 
		body.idname = ptr.childs[0].Data ;
		body.offset.value = Count.value ;
		body.unitType = tIR ;
		body.next = new fieldChain() ;
		Count.value += tIR.Count.value ;
		
		TreeNode tar = new TreeNode() ;
		tar.IsTerminal=true ;  tar.NonTerminal = nonTerminals.IdMore  ;
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) {
			idList( ptr.childs[1].childs[1] , parent , body.next , layer , Count , tIR ) ;
			return ;
		}
		
		tar.NonTerminal = nonTerminals.FieldDecList  ;
		p = Search( parent.father.childs[4] , tar , 0 ) ;
		if( p != null ) {
			fieldDecList( p , body.next , Count , layer ) ;
		}
	}
	
	/************/
	/****变量声明***/
	/************/
	
	private void varDecList( TreeNode ptr , int layer , integer offset ) {
		TreeNode p ; 
		TreeNode tar = new TreeNode() ;
		tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.TypeDef  ;
		
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) {
			typeIR tIR = new typeIR() ; 
			typeDef( p , tIR , layer ) ;
			if( tIR != null ) {
				varIdList( p.father.childs[1] , layer , offset , tIR ) ;
			}
		}
		
		tar. NonTerminal = nonTerminals.VarDecMore  ;
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) {
			tar.NonTerminal = nonTerminals.VarDecList ;
			p = Search( p , tar , 1 ) ;
			if( p != null ) {
				varDecList( p , layer , offset ) ;
			}
		}
	}
	
	private void varIdList( TreeNode ptr , int layer , integer offset , typeIR tIR ) {
		SymTableNode p = new SymTableNode() ;
		p.name = ptr.childs[0].Data ;
		p.next = new SymTableNode() ;
		p.attrIR.kind = IdKind.varkind ;
		p.attrIR.idtype.copy( tIR ) ;
		p.attrIR.More.VarAttr.offset.value = offset.value ;
		p.attrIR.level = layer ;
		p.attrIR.More.VarAttr.access = AccessKind.dir ;
		if( myScope.FindID( p.name , false ) == null ) {
			if( p.attrIR.idtype != null ) {
				offset.value += p.attrIR.idtype.Count.value ;
				symTable.insertNode( p ) ;
				if( myScope.scope.Peek().front == null ) {
					myScope.scope.Peek().front = p ; 
					ptr.symtPtr = p ; 
				}
			}
			else return ;
		}
		else {
			error( "语义错误：		变量" + p.name + "重复定义" , ptr.Line , ptr.Row ) ;
			return ;
		}
		
		TreeNode q ; 
		TreeNode tar = new TreeNode() ;
		tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.VarIdMore  ;
		q = Search( ptr , tar , 0 ) ;
		if( q != null ) {
			tar.NonTerminal = nonTerminals.VarIdList  ;
			q = Search( q , tar , 0 ) ;
			if( q != null ) {
				varIdList( q , layer , offset , tIR ) ;
			}
		}
	}
	
	/************/
	/**过程声明部分**/
	/************/
	
	private void procDec( TreeNode ptr , int layer , bool mid  ) {
		TreeNode p ; 
		SymTableNode sym = new SymTableNode() ;
		ptr.childs[1].childs[0].symtPtr = sym ;
		
		sym.name = ptr.childs[1].childs[0].Data ;
		sym.next = new SymTableNode() ; 
		sym.attrIR.idtype = null ;
		sym.attrIR.kind = IdKind.prockind ;
		sym.attrIR.level = layer ;
		sym.attrIR.More.ProcAttr.Count.value = 0 ;
		sym.EOFL = true ;
		
		if( myScope.scope.Peek().front == null ) {
			myScope.scope.Peek().front = sym ;
			symTable.insertNode( sym ) ;
		}
		if( mid == false ) {
			symTable.insertNode( sym ) ;
		}
		else if( mid == true ) {
			SymTableNode s0 = myScope.GetRear() ;
			s0.EOFL = false ; 
			symTable.insertMid( s0 , sym ) ;
		}
		
		myScope.newLayer( null ) ;
		p = ptr.childs[3] ;
		paramList( p , layer , sym.attrIR.More.ProcAttr.Count , sym.attrIR.More.ProcAttr.param ) ;
		p = ptr.childs[6].childs[0] ;
		declarePart( p , layer , sym.attrIR.More.ProcAttr.Count ) ;
		p = ptr.childs[7].childs[0] ; 
		programBody( p ) ;
		
		myScope.DropLayer() ;
	} 
	
	private void paramList( TreeNode ptr , int layer , integer Count , ParamTable param ) {
		TreeNode p ; 
		TreeNode tar = new TreeNode() ;
		tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.ParamDecList  ;
		p = Search( ptr , tar , 1 ) ;
		if( p != null ) {
			paramDecList( p , layer , Count , param ) ;
		}
	}
	
	private void paramDecList( TreeNode ptr , int layer , integer Count , ParamTable param ) {
		TreeNode p ;
		TreeNode tar = new TreeNode() ; 
		tar.IsTerminal=true ;
		
		if( ptr.ChildNum != 0 && ptr.childs[0].NonTerminal.Equals(nonTerminals.Param ) ) {
			typeIR tIR = new typeIR() ;
			
			tar.NonTerminal = nonTerminals.TypeDef  ;
			p = Search( ptr , tar , 1 ) ;
			if( p != null ) {
				typeDef( p , tIR , layer ) ;
				if( tIR != null ) {
					p = p.father.childs[1] ;
					formList( p , layer , Count , param , tIR , false ) ;
				}
			}
			else {
                tar.IsTerminal = false; tar.Terminal = LexType.VAR;
				p = Search( ptr , tar , 1 ) ; 
				if( p != null ) {
					if( p.father.childs[1].NonTerminal.Equals(nonTerminals.TypeDef ) ) {
						typeDef( p.father.childs[1] , tIR , layer ) ;
                        tar.IsTerminal = true; tar.NonTerminal = nonTerminals.FormList;
						p = Search( p.father.childs[1] , tar , 1 ) ;
						if( tIR != null && p != null ) formList( p , layer , Count , param , tIR , true ) ;
					}
				}
			}
		}
		
		tar.NonTerminal = nonTerminals.ParamMore  ;
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) {
			tar.NonTerminal = nonTerminals.ParamDecList  ;
			p = Search( p , tar , 0 ) ;
			if( p != null ) {
				if( param != null ) {
					ParamTable pm = param ;
					while( pm.next != null ) pm = pm.next ; 
					paramDecList( p , layer , Count , pm.next ) ;
				}
				else paramDecList( p , layer , Count , param ) ; 
			}
		}
	}
	
	private void formList( TreeNode ptr , int layer , integer Count , ParamTable param , typeIR tIR , bool ntype ) {
		TreeNode p  ; 
		TreeNode tar = new TreeNode() ;
		
		SymTableNode sym = new SymTableNode() ;
		sym.name = ptr.childs[0].Data ;
		sym.next = new SymTableNode() ;
		sym.attrIR.idtype.copy( tIR ) ;
		sym.attrIR.kind = IdKind.varkind ;
		if( ntype == true ) sym.attrIR.More.VarAttr.access = AccessKind.indir ;
		else sym.attrIR.More.VarAttr.access = AccessKind.dir ; 
		sym.attrIR.level = layer ;
		sym.attrIR.More.VarAttr.offset.value = Count.value ; 
		
		if( myScope.FindID( sym.name , false ) != null ) {
			String str = "语义错误：		参数标识符" + sym.name + "重复定义!" ;
			error( str , ptr.Line , ptr.Row ) ;
			sym = null ;
			tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.FormList  ;
			p = Search( ptr.childs[1] , tar , 2 ) ;
			if( p != null ) formList( p , layer , Count , param , tIR , ntype ) ;
		}
		else {
			Count.value += tIR.Count.value ;
			symTable.insertNode( sym ) ;
			param.type.copy( tIR ) ; 
			param.symPtr = sym ;
			if( myScope.scope.Peek().front == null ) myScope.scope.Peek().front = sym ;
			
			tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.FormList  ;
			p = Search( ptr.childs[1] , tar , 2 ) ;
			if( p != null ) {
				param.next = new ParamTable() ;
				formList( p , layer , Count , param.next , tIR , ntype ) ;
			}
		}
	}
	
	/***********/
	/**程序体部分**/
	/***********/
	
	private void stmList( TreeNode ptr ) {
		TreeNode p ; 
		TreeNode tar = new TreeNode() ;
		if( ptr == null ) return ;
		bool flag = false ; 	
		tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.ConditionalStm   ;
		
		p = Search( ptr , tar , 1 ) ;
		if( p != null ) {
			flag = true ;
			conditionalStm( p ) ;
		}
		
		if( flag == false ) {
			tar.NonTerminal = nonTerminals.LoopStm  ;
			p = Search( ptr , tar , 1 ) ;
			if( p != null ) {
				flag = true ;
				loopStm( p ) ;
			}
		}
		
		if( flag == false ) {
			tar.NonTerminal = nonTerminals.InputStm   ;
			p = Search( ptr , tar , 1 ) ;
			if( p != null ) {
				flag = true ;
				inputStm( p ) ;
			}
		}
		
		if( flag == false ) {
			tar.NonTerminal = nonTerminals.OutputStm  ;
			p = Search( ptr , tar , 1 ) ;
			if( p != null ) {
				flag = true ;
				outputStm( p ) ;
			}
		}
		
		if( flag == false ) {
			tar.NonTerminal = nonTerminals.ReturnStm  ;
			p = Search( ptr , tar , 1 ) ;
			if( p != null ) {
				flag = true ;
				returnStm( p ) ;
			}
		}
		
		if( flag == false ) {
			tar.IsTerminal = false;
			tar.Terminal =  LexType.ID  ;
			p = Search( ptr , tar , 1 ) ;
			if( p != null ) {
				flag = true ;
				otherStm( p ) ;
			}
		}
		
		tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.StmMore  ;
		p = Search( ptr , tar , 0 ) ;
		
		if( p != null ) {
			tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.StmList  ;
			p = Search( p , tar , 0 ) ;
			if( p != null ) stmList( p ) ;
		}		
	}
	
	private void conditionalStm( TreeNode ptr ) {
		TreeNode p ;
		TreeNode tar = new TreeNode() ;
		
		tar.IsTerminal=true ; tar. NonTerminal = nonTerminals.RelExp  ;
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) {
			relExp( p ) ;
		}
		
		tar.NonTerminal = nonTerminals.StmList  ; 
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) {
			stmList( p ) ;
			for( int i = 0 ; i < ptr.ChildNum ; i ++ ){
				if( ptr.childs[i].IsTerminal == true ) {
					if( ptr.childs[i] != p && ptr.childs[i].NonTerminal.Equals(nonTerminals.StmList ) ) {
						p = ptr.childs[i] ; 
						break ;
					}
				}
			}
			stmList( p ) ;
		}
	}
	
	private void loopStm( TreeNode ptr ) {
		TreeNode p ; 
		TreeNode tar = new TreeNode() ;
		tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.RelExp  ;
		
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) {
			relExp( p ) ;
		}
		
		tar.NonTerminal = nonTerminals.StmList  ;
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) {
			stmList( p ) ;
		}
	}
	
	private void inputStm( TreeNode ptr ) {
		TreeNode p ; 
		TreeNode tar = new TreeNode() ;
		tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.Invar  ;
		
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) {
			invar( p ) ;
		}
	}
	
	private void outputStm( TreeNode ptr ) {
		TreeNode p ; 
		TreeNode tar = new TreeNode() ;
		
		typeIR tIR = new typeIR() ;
		integer  ntype = new integer( 0 ) ; 
		
		tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.Exp  ;
		
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) {
			Exp( p , tIR , ntype ) ;
		}
	}
	
	private void returnStm( TreeNode ptr ) {
	}
	
	private void otherStm( TreeNode ptr ) {
		TreeNode p , q , t ; 
		TreeNode tar = new TreeNode() ; tar.IsTerminal=true;
		
		SymTableNode sym = new SymTableNode() ; 
		sym = myScope.FindID( ptr.Data , true ) ;
		
		if( sym == null ) {
			String str ;
			tar.NonTerminal = nonTerminals.AssignmentRest ;
			p = Search( ptr.father.childs[1] , tar , 1 ) ;
			if( p != null ) str = "语义错误：		变量标识符" + ptr.Data + "未定义" ;
			else str = "语义错误：		过程标识符" + ptr.Data + "未定义" ;
			error( str , ptr.Line , ptr.Row ) ;
			return ;
		}
		else {
			ptr.symtPtr = sym ; 
			tar.NonTerminal = nonTerminals.AssignmentRest  ;
			q = Search( ptr.father.childs[1] , tar , 1 ) ;
			if( q != null ) {
				if( sym.attrIR.idtype.Kind.Equals( TypeKind.arrayTy ) ) {
					tar.IsTerminal = false ; tar.Terminal =  LexType.LMIDPAREN  ;
					t = Search( q.childs[0] , tar , 0 ) ;
					if( t == null ) {
						String str ; 
						str = "语法错误：		把数组" + ptr.Data + "当成标识符来使用了" ;
						error( str , ptr.Line , ptr.Row ) ;
					}
				}
				assignmentRest( q , sym ) ;
				return ; 
			}
			tar.NonTerminal = nonTerminals.CallStmRest  ;
			p = Search( ptr.father.childs[1] , tar , 1 ) ;
			
			if( p != null ) {
				callStmRest( p , sym ) ;
				return ; 
			}
		}
	}
	
	private void relExp( TreeNode ptr ) {
		TreeNode p ; 
		TreeNode tar = new TreeNode() ;
		
		typeIR tIR = new typeIR() ;
		integer ntype = new integer( 0 ) ; 
		
		tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.Exp  ;
		p = Search( ptr , tar , 1 ) ;
		if( p != null ) {
			Exp( p , tIR , ntype ) ;
			if( tIR.Equals( CharTy ) ) tIR.copy( IntTy ) ;
			p = Search( ptr.childs[1] , tar , 2 ) ;
			ntype.value = 1 ;
			Exp( p , tIR , ntype ) ;
		}
	}
	
	private void invar( TreeNode ptr ) {
		if( ptr.ChildNum != 0 && ptr.childs[0].Terminal.Equals(LexType.ID) ) {
			SymTableNode sym = new SymTableNode() ;
			sym = myScope.FindID( ptr.childs[0].Data , true ) ;
			if( sym == null ) {
				String str ;
				str = "语义错误：		变量标识符" + ptr.childs[0].Data + "未定义" ;
				error( str , ptr.childs[0].Line , ptr.childs[0].Row ) ;
			}
			else if( sym.attrIR.kind.Equals( IdKind.varkind ) == false ) {
				error( "语义错误：		标识符" + ptr.childs[0].Data + "应为变量类型" , ptr.childs[0].Line , ptr.childs[0].Row ) ;
			}
			else ptr.childs[0].symtPtr = sym ;
		}
	}
	
	private void Exp( TreeNode ptr , typeIR tIR , integer ntype ) {
		TreeNode p ;
		TreeNode tar = new TreeNode() ;
		tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.Term  ;
		
		p = Search( ptr , tar , 1 ) ;
		if( p != null ) {
			term( p , tIR , ntype ) ;
		}
		
		tar.NonTerminal = nonTerminals.OtherTerm  ;
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) {
			otherTerm( p , tIR , ntype ) ;
		}
	}
	
	private void variMore( TreeNode ptr , SymTableNode sym , typeIR tIR ) {
		TreeNode p ; 
		TreeNode tar = new TreeNode() ;
		integer ntype = new integer( 2 ) ; 
		
		tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.Exp  ;
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) {
			tIR.copy( sym.attrIR.idtype.More.ArrayAttr.indexTy ) ;
			Exp( p , tIR , ntype ) ;
		}
		else {
			tar.NonTerminal = nonTerminals.FieldVar  ;
			p = Search( ptr , tar , 0 ) ;
			if( p != null ) {
				if( p.ChildNum != 0 && p.childs[0].Terminal.Equals( LexType.ID ) ) {
					fieldChain body = new fieldChain() ;
					body = sym.attrIR.idtype.More.body ;
					while( body != null ) {
						if( body.idname.Equals( p.childs[0].Data) ) break ; 
						body = body.next ;
					}
					if( body != null ) {
						tIR.copy( IntTy ) ;
						tar.NonTerminal = nonTerminals.FieldVarMore  ;
						p = Search( p , tar , 0 ) ;
						if( p != null ) {
							tar.NonTerminal = nonTerminals.Exp  ;
							p = Search( p , tar , 0 ) ;
							ntype.value = 2 ;
							Exp( p , tIR , ntype ) ;
						}
					}
					else {
						error( "语义错误：		" + p.childs[0].Data+"并非过程标识符，而是"+sym.name+"类型" , p.childs[0].Line , p.childs[0].Row ) ;
					}
				}
			}
		}
	}
	
	private void term( TreeNode ptr , typeIR tIR , integer ntype) {
		TreeNode p , q ;
		TreeNode tar = new TreeNode() ;
		tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.Factor  ;
		
		p = Search( ptr , tar , 1 ) ;
		if( p != null ) {
			tar.NonTerminal = nonTerminals.Exp  ;
			q = Search( p , tar , 0 ) ;
			if( q !=null ) Exp( q , tIR , ntype) ;
			else {
				tar.NonTerminal = nonTerminals.Variable  ;
				q = Search( p , tar , 1 ) ;
				if( q != null ){
					variable( q , tIR , ntype ) ;
				}
				else if( tIR != null && p.ChildNum != 0 && ( q = p.childs[0] ).Terminal.Equals( LexType.INTC)) {
					if( !tIR.Equals( IntTy ) && ntype.value != 2 ){
						error( "语义错误：		类型应该为" + tIR.Kind + ",而不应该是整型" , q.Line , q.Row ) ;
					}
				}
			}
			tar.NonTerminal = nonTerminals.OtherFactor ;
			q = Search( ptr , tar , 0 ) ;
			if( q != null ) {
				tar.NonTerminal = nonTerminals.Term  ;
				q = Search( q , tar , 0 ) ;
				if( q != null ) term( q , tIR , ntype ) ;
			}
		}
	}
	
	private void otherTerm( TreeNode ptr , typeIR tIR , integer ntype ) {
		TreeNode p ; 
		TreeNode tar = new TreeNode() ;
		tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.Exp  ;
		
		p = Search( ptr , tar , 2 ) ;
		if( p != null ) {
			Exp( p , tIR , ntype ) ;
		}
	}
	
	private void variable( TreeNode ptr , typeIR tIR , integer ntype ) {
		TreeNode p , q , t ;
		TreeNode tar = new TreeNode() ;
		integer type = new integer( 0 ) ; 
		
		tar.IsTerminal = false ;  tar.Terminal = LexType.ID  ;
		p = Search( ptr , tar , 2 ) ;
		if( p != null ) {
			SymTableNode sym = myScope.FindID( p.Data , true ) ;
			if( sym != null ) {
				p.symtPtr = sym ;
				if( !sym.attrIR.kind.Equals( IdKind.varkind ) ) {
					error( "语义错误：		标识符"+ptr.childs[0].Data+"应为变量类型" , ptr.childs[0].Line , ptr.childs[0].Row ) ;
				}
				else {
					if( sym.attrIR.idtype.Kind.Equals( TypeKind.arrayTy ) ) {
						TreeNode temp = new TreeNode() ;
						TreeNode tmp ;
						temp.IsTerminal =false ; temp.Terminal =  LexType.LMIDPAREN  ;
						tmp = Search( ptr.childs[1] , temp , 0 ) ;
						if( tmp == null ) {
							String str ; 
							str = "语法错误：		把数组" + ptr.childs[0].Data + "当成标识符来使用了" ;
							error( str , ptr.childs[0].Line , ptr.childs[0].Row ) ;
						}
					}
					if( ptr.childs[1] != null && ptr.childs[1].ChildNum != 0 ) {
						
						tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.Exp  ;
						q = Search( ptr.childs[1] , tar , 0 ) ;
						if( q != null ) {
							if( !sym.attrIR.idtype.Kind.Equals( TypeKind.arrayTy ) ){
								String str ;
								str = "语义错误：		标识符" + ptr.childs[0].Data + "类型应该为" + " arrayTy" ;
								error( str , ptr.childs[0].Line , ptr.childs[0].Row ) ;
								return ;
							}
							if( ntype.value != 0 && !sym.attrIR.idtype.More.ArrayAttr.elemTy.Equals( tIR ) ){
								String str ;
								str = "语义错误：		标识符" + ptr.childs[0].Data + "类型应该为" + tIR.Kind ;
								error( str , ptr.childs[0].Line , ptr.childs[0].Row ) ;
								return ; 
							}
							else {
								typeIR tIR0 = new typeIR() ;
								tIR0.copy( IntTy ) ;
								type.value = 2 ;
								Exp( q , tIR0 , type ) ;
								if( ntype.value == 0 ) {
									ntype.value = 1 ; 
									tIR.copy( sym.attrIR.idtype.More.ArrayAttr.elemTy ) ; 
								}
							}
						}
						tar.NonTerminal = nonTerminals.FieldVar  ;
						q = Search( ptr.childs[1] , tar , 0 ) ;
						if( q != null ) {
							if( !sym.attrIR.idtype.Kind.Equals( TypeKind.fieldTy ) ) {
								String str ; 
								str = "语义错误：		标识符" + ptr.childs[0].Data + "类型应该为" + TypeKind.fieldTy ;
								error( str , ptr.childs[0].Line , ptr.childs[0].Row ) ;
								return ;
							}
							tar.IsTerminal = false ; tar.Terminal =  LexType.ID  ;
							q = Search( q , tar , 1 ) ;
							if( q != null ) {
								String idName = q.Data ;
								fieldChain body = sym.attrIR.idtype.More.body ;
								while( body != null ) {
									if( body.idname.Equals( idName ) ) break ; 
									body = body.next ; 
								}
								if( body == null ) {
									String str ; 
									str = "语义错误：		变量" + idName + "非纪录" + p.Data + "成员变量" ;
									error( str , ptr.childs[0].Line , ptr.childs[0].Row ) ;
									return ;
								}
								else {
									tar.IsTerminal = true ; tar.NonTerminal = nonTerminals.Exp  ;
									t = Search( q.father.childs[1] , tar , 2 ) ;
									if( t != null ) {
										if( !body.unitType.Kind.Equals( TypeKind.arrayTy ) ) {
											String str ; 
											str = "语义错误：		纪录" + p.Data + "成员变量标识符" + idName + "类型并非数组类型" ;
											error( str , q.Line , q.Row ) ;
											return ; 
										}
										if( ntype.value != 0  && !body.unitType.More.ArrayAttr.elemTy.Equals( tIR ) ) {
											String str = "语义错误：		标识符" + idName + "类型应该为" +tIR.Kind ;
											error( str , q.Line , q.Row ) ;
											return ; 
										}
										else {
											typeIR tIR0 = new typeIR() ;
											tIR0.copy( IntTy ) ;
											type.value = 2 ;
											Exp( t , tIR0 , type ) ;
											if( ntype.value == 0 ) {
												ntype.value = 1 ; 
												tIR.copy( body.unitType.More.ArrayAttr.elemTy ) ; 
											}
										}
									}
									else {
										if( ntype.value != 0 && !body.unitType.Equals( tIR ) ){
											String str = "语义错误：		标识符"+ idName + "类型应该为" + tIR.Kind ;
											error( str , q.Line , q.Row ) ;
											return ; 
										}
									}
								}
							}
							else {
								String str = "语义错误：		此处不应该出现纪录类型" ;
								error( str , q.Line , q.Row ) ;
								return ;
							}
						}
					}
					else {
						if( ntype.value == 0 ){
							tIR.copy( sym.attrIR.idtype ) ;
							ntype.value = 1 ;
						}
						else if( ntype.value == 1 ) {
							if( !sym.attrIR.idtype.Equals( tIR ) ) {
								String str = "语义错误：		标识符" + ptr.childs[0].Data + "类型应该为" + tIR.Kind ;
								error( str , ptr.childs[0].Line , ptr.childs[0].Row ) ;
							}
						}
						else if( ntype.value == 2 && !sym.attrIR.idtype.Equals( IntTy ) ) {
							String str = "语义错误：		标识符" + ptr.childs[0].Data + "类型应该为" + "intTy" ;
							error( str , ptr.childs[0].Line , ptr.childs[0].Row ) ;
						}
					}
				}
			}
			else {
				String str = "语义错误：		变量标识符" + ptr.childs[0].Data + "未定义" ;
				error( str , ptr.childs[0].Line , ptr.childs[0].Row ) ;
				return ;
			}
		}
	}
	
	private void assignmentRest( TreeNode ptr , SymTableNode sym ) {
		TreeNode p ; 
		TreeNode tar = new TreeNode() ;
		typeIR tIR = new typeIR() ;
		tIR.copy( sym.attrIR.idtype ) ; 
		integer ntype = new integer( 1 ) ; 
		
		tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.VariMore  ;
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) variMore( p , sym , tIR ) ;
		if( tIR.Equals( CharTy ) ) tIR.copy( IntTy ) ;
		tar.NonTerminal = nonTerminals.Exp  ;
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) {
			Exp( p , tIR , ntype ) ;
		}
	}
	
	private void callStmRest( TreeNode ptr , SymTableNode sym ) {
		TreeNode p ; 
		TreeNode tar = new TreeNode() ;
		tar.IsTerminal=true ; tar.NonTerminal = nonTerminals.ActParamList  ;
		p = Search( ptr , tar , 0 ) ;
		if( p != null ) {
			if( sym.attrIR.kind.Equals( IdKind.prockind ) ) actParamList( p , sym.attrIR.More.ProcAttr.param ) ;
		}
		else {
			error( "语义错误：		" + sym.name + "并非过程标识符，而是" + sym.attrIR.kind + "类型" , p.childs[0].Line , p.childs[0].Row ) ;
		}
	}
	
	private void actParamList( TreeNode ptr , ParamTable param ) {
		TreeNode p ;
		TreeNode tar = new TreeNode() ;
		integer ntype = new integer( 1 ) ; 
		typeIR tIR = new typeIR() ; 
		
		tar.IsTerminal=true ;  tar.NonTerminal = nonTerminals.Exp  ;
		p = Search( ptr , tar , 1 ) ;
		if( p != null ) {
			if( param == null ) {
				error( "语义错误：		过程调用实参数目过多" , ptr.Line , ptr.Row ) ;
				return  ;
			}
			if( param.type.Equals( CharTy ) ) {
				ntype.value = 0 ;
				Exp( p , tIR = IntTy , ntype ) ;
			}
			else Exp( p , tIR = param.type , ntype ) ;
			
			tar.NonTerminal = nonTerminals.ActParamMore  ;
			p = Search( ptr , tar , 0 ) ;
			
			if( p != null ) {
				tar.NonTerminal = nonTerminals.ActParamList  ;
				p = Search( p , tar , 0 ) ;
				if( p != null ) actParamList( p , param.next ) ;
				else if( param.next != null ) {
					error( "语义错误：		过程调用实参数目不完整" , ptr.Line , ptr.Row ) ;
					return ;
				}
			}
			
		}
		else if( param != null ) {
			error( "语义错误：		过程调用实参数目不完整" , ptr.Line , ptr.Row ) ;
			return ;
		}
	}
}

}
