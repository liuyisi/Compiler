/*
 *author: Louise
 *date  : 16/11/2014
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Compiler.Common;

namespace Compiler.Parser
{
    /*
     *author: Louise
     *date  : 16/11/2014
     *describe: product集合
     */
    class Product
    {
        /*
         *author: Louise
         *date  : 16/11/2014
         *describe: product产生式中的元素
         */
        public class ProductElement
        {
            public bool IsTerminal;             //true 代表是终极符 false代表是非终极符
            public nonTerminals NonTerminal;    //非终极符
            public LexType Terminal;            //终极符
        }

        public List< List<ProductElement> > product { get; set; }  // product集合

        String buffer;
        int Cur;

        /*
         *author: Louise
         *date  : 16/11/2014
         *describe: 构造函数
         */
        public Product()
        {
            product = new List<List<ProductElement>>();
        }

        /*
         *author: Louise
         *date  : 16/11/2014
         *describe: 构造函数(用filePath来构造)
         */
        public Product(String filePath)
        {
            product = new List<List<ProductElement>>();
            getProduct(filePath);
        }

        /*
         *author: Louise
         *date  : 16/11/2014
         *describe: 取得产生式集合
         */
        public void getProduct( String filePath ) 
        {
            StreamReader reader = new StreamReader(filePath);
            buffer = reader.ReadToEnd();
            reader.Close();
            Cur = 0;

            while (Cur < buffer.Length)
            {
                List<ProductElement> element = getOneProduct();
                if( element.Count > 0 ) product.Add( element );
            }
        }

        /*
         *author: Louise
         *date  : 16/11/2014
         *describe: 取得单条产生式
         */
        private List<ProductElement> getOneProduct()
        {
            List<ProductElement> product = new List<ProductElement>();

            while (true)
            {
                if (Cur >= buffer.Length) break;
                if (buffer[Cur] == '\n' || buffer[Cur] == '\r') break;
                if (buffer[Cur] == ' ')
                {
                    Cur++;
                    continue;
                }

                int start = Cur;
                ProductElement Element = new ProductElement();
                Element.IsTerminal = false ;
                String data ;

                while (buffer[Cur] != ' ' && buffer[Cur] != '\n' && buffer[Cur] != '\r' && Cur < buffer.Length)
                {
                    Cur++;
                    if (Cur >= buffer.Length) break;
                }
                data = buffer.Substring(start, Cur - start);


                if (data.CompareTo( "=" ) == 0 )
                {
                     Cur++;
                     continue;
                }

                Element.NonTerminal = recoNonTerminal( data , Element ) ;
                if( Element.IsTerminal == true ) Element.Terminal = recoTerminal(data);
                product.Add(Element);

                Cur++;
            }
            Cur++;

            return product;
        }

        /*
         *author: Louise
         *date  : 16/11/2014
         *describe: 辨认是哪个非终极符
         */
        private nonTerminals recoNonTerminal( String data , ProductElement Element )
        {
            switch (data)
            {
                case "Program":         return nonTerminals.Program;
                case "ProgramHead":     return nonTerminals.ProgramHead;
                case "ProgramName":     return nonTerminals.ProgramName;
                case "DeclarePart":     return nonTerminals.DeclarePart;
                case "TypeDecpart":     return nonTerminals.TypeDecpart;
                case "TypeDec":         return nonTerminals.TypeDec;
                case "TypeDecList":     return nonTerminals.TypeDecList;
                case "TypeDecMore":     return nonTerminals.TypeDecMore;
                case "TypeId":          return nonTerminals.TypeId;
                case "TypeDef":         return nonTerminals.TypeDef;
                case "BaseType":        return nonTerminals.BaseType;
                case "StructureType":   return nonTerminals.StructureType;
                case "ArrayType":       return nonTerminals.ArrayType;
                case "Low":             return nonTerminals.Low;
                case "Top":             return nonTerminals.Top;
                case "RecType":         return nonTerminals.RecType;
                case "FieldDecList":    return nonTerminals.FieldDecList;
                case "FieldDecMore":    return nonTerminals.FieldDecMore;
                case "IdList":          return nonTerminals.IdList;
                case "IdMore":          return nonTerminals.IdMore;
                case "VarDecpart":      return nonTerminals.VarDecpart;
                case "VarDec":          return nonTerminals.VarDec;
                case "VarDecList":      return nonTerminals.VarDecList;
                case "VarDecMore":      return nonTerminals.VarDecMore;
                case "VarIdList":       return nonTerminals.VarIdList;
                case "VarIdMore":       return nonTerminals.VarIdMore;
                case "ProcDecpart":     return nonTerminals.ProcDecpart;
                case "ProcDec":         return nonTerminals.ProcDec;
                case "ProcDecMore":     return nonTerminals.ProcDecMore;
                case "ProcName":        return nonTerminals.ProcName;
                case "ParamList":       return nonTerminals.ParamList;
                case "ProcDeclaration": return nonTerminals.ProcDeclaration;
                case "ParamDecList":    return nonTerminals.ParamDecList;
                case "ParamMore":       return nonTerminals.ParamMore;
                case "Param":           return nonTerminals.Param;
                case "FormList":        return nonTerminals.FormList;
                case "FidMore":         return nonTerminals.FidMore;
                case "ProcDecPart":     return nonTerminals.ProcDecPart;
                case "ProcBody":        return nonTerminals.ProcBody;
                case "ProgramBody":     return nonTerminals.ProgramBody;
                case "StmList":         return nonTerminals.StmList;
                case "StmMore":         return nonTerminals.StmMore;
                case "Stm":             return nonTerminals.Stm;
                case "AssCall":         return nonTerminals.AssCall;
                case "AssignmentRest":  return nonTerminals.AssignmentRest;
                case "ConditionalStm":  return nonTerminals.ConditionalStm;
                case "LoopStm":         return nonTerminals.LoopStm;
                case "InputStm":        return nonTerminals.InputStm;
                case "Invar":           return nonTerminals.Invar;
                case "OutputStm":       return nonTerminals.OutputStm;
                case "ReturnStm":       return nonTerminals.ReturnStm;
                case "CallStmRest":     return nonTerminals.CallStmRest;
                case "ActParamList":    return nonTerminals.ActParamList;
                case "ActParamMore":    return nonTerminals.ActParamMore;
                case "RelExp":          return nonTerminals.RelExp;
                case "OtherRelE":       return nonTerminals.OtherRelE;
                case "Exp":             return nonTerminals.Exp;
                case "OtherTerm":       return nonTerminals.OtherTerm;
                case "Term":            return nonTerminals.Term;
                case "OtherFactor":     return nonTerminals.OtherFactor;
                case "Factor":          return nonTerminals.Factor;
                case "Variable":        return nonTerminals.Variable;
                case "VariMore":        return nonTerminals.VariMore;
                case "FieldVar":        return nonTerminals.FieldVar;
                case "FieldVarMore":    return nonTerminals.FieldVarMore;
                case "CmpOp":           return nonTerminals.CmpOp;
                case "AddOp":           return nonTerminals.AddOp;
                case "MultOp":          return nonTerminals.MultOp;
            }
            Element.IsTerminal = true;
            return nonTerminals.Program;
        }

        /*
         *author: Louise
         *date  : 16/10/2014
         *describe: 辨认是哪个终极符
         */
        private LexType recoTerminal(String terminal)
        {
            switch (terminal)
            {
                case "ENDFILE":   return LexType.ENDFILE;
                case "ERROR":     return LexType.ERROR;
                case "PROGRAM":   return LexType.PROGRAM;
                case "PROCEDURE": return LexType.PROCEDURE;
                case "TYPE":      return LexType.TYPE;
                case "VAR":       return LexType.VAR;
                case "IF":        return LexType.IF;
                case "THEN":      return LexType.THEN;
                case "ELSE":      return LexType.ELSE;
                case "FI":        return LexType.FI;
                case "WHILE":     return LexType.WHILE;
                case "DO":        return LexType.DO;
                case "ENDWH":     return LexType.ENDWH;
                case "BEGIN":     return LexType.BEGIN;
                case "END":      return LexType.END;
                case "READ":      return LexType.READ;
                case "WRITE":     return LexType.WRITE;
                case "ARRAY":     return LexType.ARRAY;
                case "OF":        return LexType.OF;
                case "RECORD":    return LexType.RECORD;
                case "RETURN":   return LexType.RETURN;
                case "INTEGER":  return LexType.INTEGER;
                case "CHAR":     return LexType.CHAR;
                case "ID":        return LexType.ID;
                case "INTC":      return LexType.INTC;
                case "CHARC":     return LexType.CHARC;
                case "ASSIGN":    return LexType.ASSIGN;
                case "EQ":        return LexType.EQ;
                case "LT":        return LexType.LT;
                case "PLUS":      return LexType.PLUS;
                case "MINUS":     return LexType.MINUS;
                case "TIMES":     return LexType.TIMES;
                case "OVER":      return LexType.OVER;
                case "LPAREN":    return LexType.LPAREN;
                case "RPAREN":    return LexType.RPAREN;
                case "DOT":       return LexType.DOT;
                case "COLON":     return LexType.COLON;
                case "SEMI":      return LexType.SEMI;
                case "COMMA":     return LexType.COMMA;
                case "LMIDPAREN": return LexType.LMIDPAREN;
                case "RMIDPAREN": return LexType.RMIDPAREN;
                case "UNDERANGE": return LexType.UNDERANGE;
            }
            return LexType.ENDFILE;
        }
    }
}
