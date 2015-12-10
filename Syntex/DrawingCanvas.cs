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
using System.Drawing;

namespace Interface
{
    public class DrawingCanvas: Canvas 
    {
        private List<Visual> visuals = new List<Visual>();
        protected override int VisualChildrenCount
        {
            get {return visuals.Count;}
        }

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        public void AddVisual(Visual v)
        {
            visuals.Add(v);
            base.AddVisualChild(v);
            base.AddLogicalChild(v);
        }

        public void DeleteVisual(Visual v)
        {
            visuals.Remove(v);
            base.RemoveVisualChild(v);
            base.RemoveLogicalChild(v);
        }
    }
}
