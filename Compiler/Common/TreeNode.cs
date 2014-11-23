/*
 *author: Louise
 *date  : 19/11/2014
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Common
{
    /*
     *author: Louise
     *date  : 19/11/2014
     *describe : 语法分析树节点的数据结构
     */
    class TreeNode
    {
        public List<TreeNode> childs    { get; set; }  //一个节点的子节点
        public TreeNode       father    { get; set; }  //父节点
        public int            ChildNum                 //子节点数量
        {
            get
            {
                return childs.Count;
            }
        }
        public int Line                 { get; set; }  //在源文件中行号
        public int Row                  { get; set; }  //在源文件中列号
        

        public bool IsTerminal          { get; set; }  //标识是否是终极符
        public LexType Terminal         { get; set; }  //标识哪个终极符
        public nonTerminals NonTerminal { get; set; }
        public String Data              { get; set; }  //数据域(终极符才有)

        public TreeNode()                              //构造函数
        {
            childs = new List<TreeNode>();
        }
    }
}
