using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Compiler.Common
{
    class Predict
    {
        public List< List<LexType> > predicts{ get; set; }

        String buffer;
        int Cur ;

        public Predict()
        {
            predicts = new List<List<LexType>>();
        }

        public Predict( String filePath )
        {
            predicts = new List<List<LexType>>();
            getPredict(filePath);
        }

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

        public LexType recoTerminal(String terminal)
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
                case "BEGIN":     return LexType.BEGINI;
                case "ENDI":      return LexType.ENDI;
                case "READ":      return LexType.READ;
                case "WRITE":     return LexType.WRITE;
                case "ARRAY":     return LexType.ARRAY;
                case "OF":        return LexType.OF;
                case "RECORD":    return LexType.RECORD;
                case "RETURN1":   return LexType.RETURN1;
                case "INTERGER":  return LexType.INTERGER;
                case "CHAR1":     return LexType.CHAR1;
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
