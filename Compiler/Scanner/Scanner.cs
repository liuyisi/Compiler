/*
 *author: Louise
 *date  : 21/09/2014
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace Compiler.Common
{
    class Scanner
    {
        public int       Line   { get; set; }
        public int       Row    { get; set; }
        public int       Cur    { get; set; }
        public ErrorType error  { get; set; } 
        public String    Buffer { get; set; }
        public List<TokenType> tokenList = new List<TokenType>();

        /*
         *author: Louise
         *date  : 21/09/2014
         *describe: 读取文件到Buffer中
         */
        public Scanner(string filePath)
        {
            StreamReader reader = new StreamReader(filePath);
            Buffer = reader.ReadToEnd();
            reader.Close();
            Line = 1;
            Row  = 1;
            Cur  = 0;
        }

        /*
         *author: Louise
         *date  : 21/09/2014
         *describe: 取得下一个有效字符
         */
        public Char getNextChar()
        {
            if (Cur >= Buffer.Length - 1) return '\0';

            if (Buffer[Cur] == '{')
            {
                Cur ++;
                int level = 1;
                while (level != 0)
                {
                    Row ++;
                    if (Buffer[Cur] == '\n' || Buffer[Cur] == '\r')
                    {
                        Row = 1;
                        Line ++;
                    }
                    if (Buffer[Cur] == '\t') Row += 3;
                    if (Buffer[Cur] == '{') level ++;
                    if (Buffer[Cur] == '}') level --;
                    if (Cur == Buffer.Length - 1 && level != 0)
                    {
                        // 错误处理
                        error.isError = true;
                        error.Line    = Line;
                        error.Row     = Row ;
                        error.Type    = ErrorType.errorType.LexicalError;
                        return '\0';
                    }
                    Cur ++;
                }
                return getNextChar();
            }
            if (Buffer[Cur] == ' ' || Buffer[Cur] == '\n' || Buffer[Cur] == '\r' || Buffer[Cur] == '\t' )
            {
                while (true)
                {
                    if (Buffer[Cur] == ' ') Row++;
                    else if (Buffer[Cur] == '\r' || Buffer[Cur] == '\n')
                    {
                        Line++;
                        Row = 1;
                    }
                    else if (Buffer[Cur] == '\t') Row += 4;
                    else break;
                    Cur ++;
                }
                return getNextChar();
            }
            Char character = Buffer[Cur];
            if (character == '\n') Console.WriteLine( "换行" );
            Cur ++;
            return character;
        }


        /*
         *author: Louise
         *date  : 23/09/2014
         *describe: 识别是当否是数字并且返回这个数字，如果后面跟了字母则报错
         */
        public String recoNumber()
        {
            String Number = "";
            while (true)
            {
                if (Buffer[Cur] >= '0' && Buffer[Cur] <= '9')
                {
                    Number += Buffer[Cur];
                    Cur++;
                    Row++;
                }
                else break;
            }
            if ((Buffer[Cur] >= 'a' && Buffer[Cur] <= 'z') || (Buffer[Cur] >= 'A' && Buffer[Cur] <= 'Z'))
            {
                error.Line = Line;
                error.Row = Row;
                error.isError = true;
                error.Type = ErrorType.errorType.LexicalError;
                return null;
            }
            return Number;
        }


        /*
         *author: Louise
         *date  : 23/09/2014
         *describe: 取得由字母开头的标识符或保留字，此处暂时不区分两者，只是切开
         */
        public String recoName()
        {
            String Name = "";
            while (true)
            {
                if ((Buffer[Cur] >= 'a' && Buffer[Cur] <= 'z')
                    || (Buffer[Cur] >= 'A' && Buffer[Cur] <= 'Z')
                    || (Buffer[Cur] >= '0' && Buffer[Cur] <= '9'))
                {
                    Name += Buffer[Cur];
                    Cur++;
                    Row++;
                }
                else break;
            }
            return Name;
        }

        public LexType isReserved(String name)
        {
            switch (name)
            {
                case "program": 
                    return LexType.PROGRAM;
                case "type":
                    return LexType.TYPE;
                case "var":
                    return LexType.VAR;
                case "procedure":
                    return LexType.PROCEDURE;
                case "begin":
                    return LexType.BEGINI;
                case "end":
                    return LexType.ENDI;
                case "array":
                    return LexType.ARRAY;
                case "of":
                    return LexType.OF;
                case "record":
                    return LexType.RECORD;
                case "if":
                    return LexType.IF;
                case "then":
                    return LexType.THEN;
                case "else":
                    return LexType.ELSE;
                case "fi":
                    return LexType.FI;
                case "while":
                    return LexType.WHILE;
                case "do":
                    return LexType.DO;
                case "endwh":
                    return LexType.ENDWH;
                case "read":
                    return LexType.READ;
                case "write":
                    return LexType.WRITE;
                case "return":
                    return LexType.RETURN1;
                case "integer":
                    return LexType.INTERGER;
                case "char":
                    return LexType.CHAR1;

            }
            return LexType.ID;
        }

        /*
         *author: Louise
         *date  : 23/09/2014
         *describe: 取得下一个token
         */
        public TokenType getNextToken()
        {
            char entrance;
            TokenType Token = new TokenType() ;
            Token.Line = Line;
            Token.Row  = Row ;
            entrance = getNextChar();

            if ((entrance >= 'a' && entrance <='z') || (entrance >= 'A' && entrance <= 'Z'))
            {
                Token.Data = recoName();
                Token.lexType = isReserved(Token.Data);
                
            }
            else if (entrance >= '0' && entrance <= '9')
            {
                Token.Data = recoNumber();
                Token.lexType = LexType.INTC;
            }
            else if (entrance == ':' && Buffer[Cur + 1] == '=' )
            {
                Cur += 2; Row += 2; 
                Token.Data = ":=" ;
                Token.lexType = LexType.ASSIGN ;
            }
            else if (entrance == '.' && Buffer[Cur + 1] == '.')
            {
                Cur += 2; Row += 2;
                Token.Data = "..";
                Token.lexType = LexType.UNDERANGE;
            }
            else
            {
            }

            return Token;
        }
    }
}
