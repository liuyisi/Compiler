using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Scanner;
using Compiler.Common;

namespace Compiler.Parser
{
    public class parser
    {
        public parser()
        {
            SNLScanner = new scanner() ;
            SNLPredict = new Predict("../../Predict.txt") ;
            SNLProduct = new Product("../../Product.txt") ;
            error      = new ErrorType() ;
            cur = 0;
        }

        Predict  SNLPredict;
        Product  SNLProduct;
        scanner  SNLScanner;
        List<TokenType> TokenList;
        int cur ;
        static String output;
        static TreeNode root = new TreeNode();

        public ErrorType error { get; set; }

        public TreeNode getTree(String filePath)
        {
            TokenList = SNLScanner.getTokenList(filePath) ;
            if (SNLScanner.error.isError == true)
            {
                if (SNLScanner.error.Type == ErrorType.errorType.LexicalError)
                    output = SNLScanner.lexString(filePath);
                return null;
            }
            else
            {
                TreeNode root = new TreeNode();
                cur = 0;
                root = match(nonTerminals.Program, null);
                //Console.WriteLine(TokenList[cur].LexType);
                if (TokenList[cur].LexType != LexType.DOT)
                {
                    error.Type = ErrorType.errorType.SyntaxError;
                    error.Line = TokenList[cur].Line;
                    error.Row = TokenList[cur].Row;
                }
                if (error.isError == true)
                {
                    int line;
                    line = error.Line / 2;
                    if (error.Line / 2 == 0) line = 1;
                    if (error.Line == 3) line = 2;
                    output = "行: " + line + " 列： " + error.Row + "			" + "语法错误";
                    return null;
                }
                return root;
            }
        }

        public TreeNode match( nonTerminals NonTerminal , TreeNode father )
        {
            TreeNode root = new TreeNode();
            int  choose = -1;

            root.IsTerminal = false;
            root.NonTerminal = NonTerminal;
            root.father = father;
            root.Line = TokenList[cur].Line;
            root.Row  = TokenList[cur].Row ;

            for (int i = 0; i < SNLPredict.predicts.Count; i++)
            {
                bool flag = false;
                for (int j = 0; j < SNLPredict.predicts[i].Count; j++)
                {
                    if (TokenList[cur].LexType == SNLPredict.predicts[i][j])
                    {
                        flag = true;
                        break;
                    }
                }
               // Console.WriteLine(SNLProduct.product[i][0].NonTerminal);
                if (flag == true && SNLProduct.product[i][0].NonTerminal == NonTerminal)
                {
                    choose = i;
                    //Console.WriteLine(choose);
                    break;
                }
            }
            //Console.WriteLine(choose);
            if (choose == -1 )
            {
                error.isError = true;
                error.Type = ErrorType.errorType.SyntaxError;
                error.Line = TokenList[cur].Line;
                error.Row = TokenList[cur].Row;
                return null;
            }
            else
            {
                for (int i = 1 ; i < SNLProduct.product[choose].Count; i++)
                {
                    if (SNLProduct.product[choose][i].IsTerminal == true)
                    {
                        TreeNode leaf = new TreeNode();
                        leaf.IsTerminal = true;
                        leaf.father = root;
                        leaf.Terminal = TokenList[cur].LexType;
                        leaf.Data = TokenList[cur].Data;
                        leaf.Line = TokenList[cur].Line;
                        leaf.Row  = TokenList[cur].Row;
                        root.childs.Add(leaf); 
                        cur ++ ;
                    }
                    else
                    {
                        TreeNode child;
                        child = match( SNLProduct.product[choose][i].NonTerminal , root );
                        root.childs.Add( child );
                    }
                }
            }
            return root;
        }

        public string getOutput( String filePath ) {
		output = "" ;
		error.isError = false ;
		root = getTree( filePath ) ;
		return output ;
	}

    }
}