using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCodeBase
{
    class BarChart
    {
        public string title { get; set; }
        private string horizontalTitle { get; set; }
        private string verticalTitle { get; set; }
        private List<BarChartSegment> bars = new List<BarChartSegment>();

        /// <summary>
        /// Adds a bar to the chart.
        /// </summary>
        /// <param name="name">The name of this bar.</param>
        /// <param name="value">The value of this bar.</param>
        public void AddBar(string name, float value)
        {
            bars.Add(new BarChartSegment(name, value, RandomColor()));
        }

        /// <summary>
        /// Adds a bar to the chart.
        /// </summary>
        /// <param name="name">The name this bar of data represents.</param>
        /// <param name="value">The value of the data.</param>
        /// <param name="color">The color of the bar.</param>
        public void AddBar(string name, float value, Color color)
        {
            bars.Add(new BarChartSegment(name, value, color));
        }

        /// <summary>
        /// Creates a bar chart.
        /// </summary>
        /// <param name="g"></param>
        public void Create(Graphics g)
        {
            int chartStartY = 60;
            int chartHeight = 500-chartStartY;
            int chartWidth = 500;
            int lineStartX = 50;
            int lineStartY = 50;
            int barMargin = 10;
            int amountOfLabels = 10;
            float labelSpacing = (chartHeight) / amountOfLabels;

            float highestValue = 0;

            foreach (BarChartSegment bar in bars)
            {
                if (bar.value > highestValue)
                    highestValue = bar.value;
            }

            int barWidth = (chartWidth / bars.Count) - barMargin;

            #region Create canvas and draw tools
            Rectangle rect = new Rectangle(0, 0, chartHeight, chartWidth);
            Brush brush = new SolidBrush(Color.Black);
            Font font = new Font("Calibri", 12.0f, FontStyle.Regular, GraphicsUnit.Pixel);
            Pen pen = new Pen(brush);
            #endregion

            #region Calculate and draw bar heights
            for (int i = 0; bars.Count > i; i++)
            {
                BarChartSegment bar = bars[i];
                float barHeight = (bar.value / highestValue) * (chartHeight-lineStartY);
                float x = lineStartX+(i * barWidth) + barMargin;
                g.FillRectangle(new SolidBrush(bar.color),            x, chartStartY + chartHeight - barHeight - lineStartY, barWidth, barHeight + lineStartY);
                g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), x, chartStartY + chartHeight - barHeight - lineStartY, barWidth, barHeight + lineStartY);
                g.DrawString(bar.name, font, brush, x + 10, chartHeight+chartStartY);
            }
            #endregion

            #region Draw both axes
            g.DrawString(title, font, brush, chartWidth/2, 0);
            g.DrawLine(pen, lineStartX, chartStartY, lineStartX, chartHeight+chartStartY);
            g.DrawLine(pen, lineStartX, chartHeight+chartStartY, chartWidth-lineStartX, chartHeight+chartStartY);

            for (int labelIndex = 0; labelIndex < amountOfLabels + 1; labelIndex++)
            {
                float y = ((amountOfLabels - labelIndex) * labelSpacing) + chartStartY;
                float value = labelIndex * (highestValue / amountOfLabels);
                g.DrawLine(pen, lineStartX-5, y, lineStartX+5, y);
                g.DrawString(value.ToString(), font, brush, lineStartX-25, y);
            }
            #endregion
        }

        /// <summary>
        /// Saves the barchart as an image.
        /// </summary>
        /// <param name="path"></param>
        public void SaveAsImage(string path)
        {
            Bitmap bitmap = new Bitmap(Convert.ToInt32(500), Convert.ToInt32(500), System.Drawing.Imaging.PixelFormat.Format64bppPArgb);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Create(g);
            bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        /// <summary>
        /// Generates a random color.
        /// </summary>
        /// <returns></returns>
        private Color RandomColor()
        {
            Random r = new Random();
            int red = r.Next(0, 256);
            int blue = r.Next(0, 256);
            int green = r.Next(0, 256);
            System.Threading.Thread.Sleep(20);
            Color c = Color.FromArgb(red, green, blue);
            return c;
        }

        /// <summary>
        /// A bar representing an amount of data.
        /// </summary>
        private class BarChartSegment
        {
            /// <summary>
            /// The title representing the data.
            /// </summary>
            public string name;

            /// <summary>
            /// The value of the data.
            /// </summary>
            public float value;

            /// <summary>
            /// The color of the bar.
            /// </summary>
            public Color color;

            /// <summary>
            /// Constructs a BarChartSegment.
            /// </summary>
            /// <param name="name">The title representing the data.</param>
            /// <param name="value">The value of the data.</param>
            /// <param name="color">THe color of the bar.</param>
            public BarChartSegment(string name, float value, Color color)
            {
                this.name = name;
                this.value = value;
                this.color = color;
            }
        }
    }
}
