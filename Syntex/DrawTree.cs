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
using System.Globalization;
using Compiler.Common;
using Compiler.Scanner;
using Compiler.Parser;

namespace Interface
{
    public static class DrawTree
    {
        static int X  ;
        static int Space = 12;
        static int PanelWidth , PanelHigh;

        /*显示树结构*/
        public static void DisplayTree(TreeNode root, DrawingContext DC , DrawingCanvas canvas)
        {
            X = 20; 
            PanelWidth = 0;
            PanelHigh = 0;
            InitTree(root, 20);
            canvas.Height = PanelHigh + 50 ;
            canvas.Width = PanelWidth + 50 ;
            draw( root , DC );
        }

        /*计算画树所需信息*/
        public static int InitTree(TreeNode root , int Y )
        {
            int temp, Length, width = 0;
            String str = "";

            if (root.IsTerminal == true || root.ChildNum == 0 )
            {
                if (root.IsTerminal == true) str += root.Data;
                else str += root.NonTerminal;
                Length = str.Length * 7;
                width = Length + Space;
                root.Length = Length;
                root.Width = width;
                root.x = X;
                root.y = Y;
                X += width;
                if (Y > PanelHigh) PanelHigh = Y;
                if (X > PanelWidth) PanelWidth = X;
            }
            else
            {
                str += root.NonTerminal;
                Length = str.Length * 7;
                root.Length = Length;
                root.y = Y;
                temp = X;
                for (int i = 0; i < root.ChildNum; i++)
                {
                    width += InitTree(root.childs[i], Y + 200);
                }
                root.x = temp + width / 2 - Length / 2;
                if (width < Length)
                {
                    width = Length / 2 + width / 2;
                    X += Length - width + Space;
                }
                root.Width = width;
            }
            return width;
        }
        
        public static void draw(TreeNode root, DrawingContext DC)
        {
            Pen drawingPen = new Pen(Brushes.Black, 1);

            Point point1 = new Point( root.x , root.y );
            Point point2 = new Point( root.x + root.Length , root.y);
            Point point3 = new Point( root.x + root.Length , root.y + 15);
            Point point4 = new Point( root.x , root.y + 15 );

            DC.DrawLine( drawingPen , point1 , point2 );
            DC.DrawLine( drawingPen , point2 , point3 );
            DC.DrawLine( drawingPen , point3 , point4 );
            DC.DrawLine( drawingPen , point4 , point1 );


            if (root.IsTerminal == true)
            {
                FormattedText text = new FormattedText("" + root.Data, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 9, Brushes.Black);
                DC.DrawText( text , new Point( root.x + 1 , root.y ) ) ;
            }
            else
            {
                FormattedText text = new FormattedText("" + root.NonTerminal, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 9, Brushes.Black);
                DC.DrawText(text, new Point(root.x + 1, root.y));
                for (int i = 0; i < root.ChildNum; i++)
                {
                    DC.DrawLine( drawingPen , new Point( root.x + root.Length/2 , root.y + 15 ) , new Point( root.childs[i].x + root.childs[i].Length / 2 , root.childs[i].y)  );
                    draw(root.childs[i], DC);
                }
            }
        }

    }
}
