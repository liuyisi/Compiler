/*
 *author: Louise
 *date  : 21/09/2014
 *describe : 记录单词的类型枚举
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Common
{
    enum LexType 
    {
        /*簿记单词符号*/
        ENDFILE, ERROR, 
        
        /*保留字*/
        PROGRAM, PROCEDURE, TYPE, VAR, IF, THEN, ELSE, FI, WHILE, DO, ENDWH,
        BEGINI, ENDI, READ, WRITE, ARRAY, OF, RECORD, RETURN1,
        //类型
        INTERGER, CHAR1,

        /*多字符单词符号*/
        ID, INTC, CHARC,

        /*特殊符号*/
        ASSIGN, EQ, LT, PLUS, MINUS, TIMES, OVER, LPAREN, RPAREN, DOT, COLON, SEMI, COMMA, LMIDPAREN,
        RMIDPAREN, UNDERANGE
    };
}
