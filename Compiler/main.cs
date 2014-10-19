using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Common;

namespace Compiler
{
    class main
    {
        static void Main(string[] args)
        {
            Predict predict = new Predict("../../Predict.txt");
            Console.WriteLine( predict.predicts.Count );
            for (int i = 0; i < predict.predicts.Count; i++)
            {
                String output = "" ;
                for (int j = 0; j < predict.predicts[i].Count; j++)
                {
                    output += predict.predicts[i][j] + " ";
                }
                Console.WriteLine(output); 
            }
            Console.ReadKey();
        }
    }
}
