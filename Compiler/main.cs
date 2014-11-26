using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Compiler.Common;
using Compiler.Scanner;
using Compiler.Parser;

namespace Compiler
{
    class main
    {
        static void Main(string[] args)
        {
            parser Parser = new parser();

            TreeNode root = Parser.getTree( "../../source.txt" );
            Console.WriteLine( "EROOR? " + Parser.error.isError );
            //Display( root );
            Console.ReadKey();
            
        }
        static public void Display(TreeNode root)
        {
            Console.Write( root.NonTerminal + ":  " );
            for (int i = 0; i < root.ChildNum; i++)
            {
                if (root.childs[i].IsTerminal) Console.Write(root.childs[i].Terminal + "    ");
                else Console.Write(root.childs[i].NonTerminal + " " );
            }
            Console.WriteLine( "" );
            for (int i = 0; i < root.ChildNum; i++)
            {
                if (root.childs[i].IsTerminal == false) Display( root.childs[i] );
            }
        }
    }
}
