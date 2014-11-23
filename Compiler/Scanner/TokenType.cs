/*
 *author: Louise
 *date  : 21/09/2014
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Common;

namespace Compiler.Scanner
{  
    /*
     *author: Louise
     *date  : 21/09/2014
     *describe : 描述token的数据结构
     */
    class TokenType
    {
        public int Line{ get; set; }
        public int Row { get; set; }
        public LexType lexType { get; set; }
        public string Data { get; set; }
    }
}
