using SuperCodeBase.helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCodeBase.statistics
{
    /// <summary>
    ///  PieChart display data in a pie chart.
    /// </summary>
    class PieChart
    {
        /// <summary>
        /// The list with pie segments.
        /// </summary>
        private List<PieSegment> segments;

        /// <summary>
        /// The list with degrees per segment.
        /// </summary>
        private List<float> degrees;

        /// <summary>
        /// The rectangle in which the pie chart will be drawn.
        /// </summary>
        private Rectangle rectangle;

        /// <summary>
        /// The width in pixels of the border.
        /// </summary>
        private int borderWidth;

        /// <summary>
        /// The title of the pie chart.
        /// </summary>
        private string title;

        private ColorPalette colorPalette;

        public enum ColorPalette
        {
            /// <summary>
            /// Default. Uses custom defined colors by default and uses random colors otherwise.
            /// </summary>
            CUSTOM,

            /// <summary>
            /// Uses random colors only.
            /// </summary>
            RANDOM,

            /// <summary>
            /// Uses a rainbow gradient.
            /// </summary>
            RAINBOW
        }

        /// <summary>
        /// Constructs a pie chart.
        /// </summary>
        /// <param name="segments">The segments of the pie chart.</param>
        /// <param name="width">The width of the pie chart.</param>
        /// <param name="height">The height of the pie chart.</param>
        /// <param name="title">The title of the pie chart.</param>
        public PieChart()
        {
            colorPalette = ColorPalette.CUSTOM;
            segments = new List<PieSegment>();
            degrees = new List<float>();
            rectangle = new Rectangle(52, 52, 200, 200);
            borderWidth = 2;
        }

        /// <summary>
        /// Adds a segment to the pie chart.
        /// </summary>
        /// <param name="title">The title this segment represents.</param>
        /// <param name="value">The value of the data this segment contains.</param>
        public void AddSegment(String title, float value)
        {
            segments.Add(new PieSegment(title, value, Color.White));
        }

        /// <summary>
        /// Adds a segment to the pie chart.
        /// </summary>
        /// <param name="title">The title this segment represents.</param>
        /// <param name="value">The value of the data this segment contains.</param>
        /// <param name="color">The color of the segment.</param>
        public void AddSegment(String title, float value, Color color)
        {
            segments.Add(new PieSegment(title, value, color));
        }

        /// <summary>
        /// Sets the color palette of that the segments will be drawn with.
        /// </summary>
        /// <param name="colorPalette">The enumeral palette that will be drawn with.</param>
        public void SetColorPalette(ColorPalette colorPalette)
        {
            this.colorPalette = colorPalette;
        }

        /// <summary>
        /// Sets the border width.
        /// </summary>
        /// <param name="pixels">The border width in pixels.</param>
        public void SetBorderWidth(int pixels)
        {
            borderWidth = pixels;
        }

        /// <summary>
        /// Creates the pie chart in a Graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw with.</param>
        public void Create(Graphics g)
        {
            float total = 0;
            for (int i = 0; segments.Count > i; i++)
            {
                total += segments[i].value;
            }

            Rectangle rect = rectangle;// new Rectangle(0, 0, 150, 150);
            Font font = new Font("Calibri", 12.0f, FontStyle.Regular, GraphicsUnit.Pixel);
            float startAngle = 0;
            Brush legendBrush = new SolidBrush(Color.Black);

            Color[] colors = new Color[segments.Count];

            switch (colorPalette)
            {
                case ColorPalette.CUSTOM:
                    for (int i = 0; i < colors.Length; i++)
                    {
                        colors[i] = segments[i].color ?? ColorHelper.RandomColor();
                    }
                    break;
                case ColorPalette.RANDOM:
                    colors = ColorHelper.CreateRandomColors(segments.Count);
                    break;
                case ColorPalette.RAINBOW:
                    colors = ColorHelper.GetRainbowColors(segments.Count);
                    break;
            }

            for (int i = 0; segments.Count > i; i++)
            {
                degrees.Add((segments[i].value / total) * 360);

                Color colorToBeUsed = colors[i];

                string legendText = segments[i].title != null ? segments[i].title : "Data " + (i + 1);
                legendText += " (" + segments[i].value + " - " + Math.Round((segments[i].value / total) * 100, 2, MidpointRounding.AwayFromZero) + "%)";

                Brush b = new SolidBrush(colorToBeUsed);
                g.FillPie(b, rect, startAngle, degrees[i]);
                g.DrawPie(new Pen(legendBrush), rect, startAngle, degrees[i]);

                startAngle += degrees[i];

                PointF pointf = new PointF(rectangle.Width + 100, i * 20);
                g.DrawString(legendText, font, legendBrush, pointf);

                Rectangle r = new Rectangle(rectangle.Width + 70, i * 20, 16, 16);
                g.FillRectangle(b, r);
                g.DrawRectangle(new Pen(legendBrush), r);
            }

            g.DrawEllipse(new Pen(Color.Black, borderWidth), rect);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;

            g.DrawString(title, new Font("Arial", 16), new SolidBrush(Color.Black), new PointF(rect.Width / 2, 10), stringFormat);
        }

        /// <summary>
        /// Saves the pie chart as a JPG.
        /// </summary>
        /// <param name="path"></param>
        public void SaveAsImage(string path)
        {
            Bitmap bitmap = new Bitmap(Convert.ToInt32(650), Convert.ToInt32(650), System.Drawing.Imaging.PixelFormat.Format64bppPArgb);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Create(g);
            bitmap.Save(path, ImageFormat.Jpeg);
        }

        /// <summary>
        /// PieSegment represents data in a segment in a pie chart.
        /// </summary>
        private class PieSegment
        {
            /// <summary>
            /// The color of the segment.
            /// </summary>
            public Color? color;

            /// <summary>
            /// The name of the data this segment represents.
            /// </summary>
            public string title;

            /// <summary>
            /// The value of the data.
            /// </summary>
            public float value;

            /// <summary>
            /// Default constructor.
            /// </summary>
            public PieSegment()
            {
                color = null;
            }

            /// <summary>
            /// Constructs a pie segment with various properties.
            /// </summary>
            /// <param name="color">The color of the pie segment.</param>
            /// <param name="title">The name of the data this segment represents.</param>
            /// <param name="value">The value of the data.</param>
            public PieSegment(string title, float value, Color color)
            {
                this.color = color;
                this.title = title;
                this.value = value;
            }
        }
    }
}
