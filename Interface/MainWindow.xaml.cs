using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.CodeDom.Compiler;
using Compiler.Parser;
using Compiler.Common;
using Compiler.Scanner;

namespace Interface
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.layout();
        }

        public void layout()
        {
            parser Parser = new parser();
            TreeNode root = Parser.getTree("../../source.txt");

            DrawingVisual visual = new DrawingVisual();
            DrawingContext dc = visual.RenderOpen();

            DrawTree.DisplayTree(root, dc , Test );

            Pen drawingPen = new Pen(Brushes.Black, 2);
            //dc.DrawLine(drawingPen, new Point(0, 0), new Point(1000, 1000));

            //Test.Height = 6000;
            //Test.Width = 6000;
            Test.AddVisual(visual);
            dc.Close();
        }
    }
}
