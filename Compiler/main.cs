using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Common;
using Compiler.Scanner;
using Compiler.Parser;

namespace Compiler
{
    class main
    {
        static void Main(string[] args)
        {
            Product products = new Product("../../Product.txt");
            int count = products.product.Count ;
            Console.WriteLine(products.product.Count);
            for (int i = 0; i < products.product.Count; i++)
            {
                String output = "" ;
                for (int j = 0; j < products.product[i].Count; j++)
                {
                    if (products.product[i][j].IsTerminal == true)
                        output += products.product[i][j].Terminal + " ";
                    else output += products.product[i][j].NonTerminal + " "; 
                }
                Console.WriteLine(output); 
            }
            Console.ReadKey();
        }
    }
}
