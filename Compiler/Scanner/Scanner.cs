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

    }
}
