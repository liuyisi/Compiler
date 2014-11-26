/*
 *author: Louise
 *date  : 16/10/2014
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Compiler.Common;
using Compiler.Scanner;

namespace Compiler.Parser
{
    class Predict
    {
        public List< List<LexType> > predicts{ get; set; }  // predict集合

        String buffer;
        int Cur ;

        /*
         *author: Louise
         *date  : 16/10/2014
         *describe: 构造函数
         */
        public Predict()
        {
            predicts = new List<List<LexType>>();
        }

         /*
          *author: Louise
          *date  : 16/10/2014
          *describe: 构造函数(使用filepath来构造)
          */
        public Predict( String filePath )
        {
            predicts = new List<List<LexType>>();
            getPredict(filePath);
        }

        /*
         *author: Louise
         *date  : 16/10/2014
         *describe: 取得predict集合
         */
        public void getPredict(String filePath)
        {
            StreamReader reader = new StreamReader(filePath);
            buffer = reader.ReadToEnd();
            reader.Close();


            Cur = 0 ;
            while( Cur < buffer.Length )
            {
                predicts.Add( getOnePredict() );
            }
        }

        /*
         *author: Louise
         *date  : 16/10/2014
         *describe: 取得一个predict
         */
        private List<LexType> getOnePredict()
        {
            List<LexType> onePredict = new List<LexType>();
            while ( true )
            {
                int start = Cur ;
                String terminal ;
                while (buffer[Cur] != ' ' && buffer[Cur] != '\n' && buffer[Cur] != '\r' && Cur < buffer.Length)
                {
                    Cur++;
                    if (Cur >= buffer.Length) break;
                }
                terminal = buffer.Substring(start, Cur - start );
                onePredict.Add( recoTerminal(terminal) );
                Cur ++;
                if (Cur >= buffer.Length) break;
                if (buffer[Cur] == '\n' || buffer[Cur] == '\r') break;
            }
            Cur++;
            return onePredict;
        }

        /*
         *author: Louise
         *date  : 16/10/2014
         *describe: 辨认是那个终极符
         */
        private LexType recoTerminal(String terminal)
        {   
            switch( terminal )
            {
                case "ENDFILE":   return LexType.ENDFILE ;
                case "ERROR":     return LexType.ERROR ;
                case "PROGRAM":   return LexType.PROGRAM ;
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
                case "END":       return LexType.END;
                case "READ":      return LexType.READ;
                case "WRITE":     return LexType.WRITE;
                case "ARRAY":     return LexType.ARRAY;
                case "OF":        return LexType.OF;
                case "RECORD":    return LexType.RECORD;
                case "RETURN":    return LexType.RETURN;
                case "INTEGER":   return LexType.INTEGER;
                case "CHAR":      return LexType.CHAR;
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
