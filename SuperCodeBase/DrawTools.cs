using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCodeBase
{
    static class DrawTools
    {
        public static void DrawProgressBar(Graphics g, int x1, int y1, int x2, int y2, float percentage)
        {
            float barWidth = (x2 - x1) * percentage;

            Brush brush = new LinearGradientBrush(new Point(x1, y1), new Point(x1+Convert.ToInt32(barWidth), y2), Color.CadetBlue, Color.DarkBlue);
            Pen p = new Pen(Color.Black);            

            g.FillRectangle(brush, x1, y1, barWidth, y2 - y1);
            g.DrawRectangle(p, x1, y1, x2 - x1, y2 - y1);
        }
    }
}
