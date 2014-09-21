/*
 *author: Louise
 *date  : 21/09/2014
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Common
{
    class ErrorType
    {
        public int Line { get; set; }
        public int Row  { get; set; }
        public bool isError { get; set; }

        /*定义错误类型*/
        public enum errorType 
        {
            LexicalError,    //词法错误
            SyntaxError,     //语法错误
            SemanticError    //语义错误
        } ;
        public errorType Type ;
    }
}
