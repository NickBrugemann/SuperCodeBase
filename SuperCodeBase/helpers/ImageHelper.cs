using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCodeBase.helpers
{
    /// <summary>
    /// Contains methods regarding generating and mutating existing images.
    /// </summary>
    static class ImageHelper
    {
        /// <summary>
        /// Enumerator for text/image position in image.
        /// </summary>
        public enum PositionInImage{ TopLeft, TopRight, BottomLeft, BottomRight};

        /// <summary>
        /// Adds a watermark text to the right bottom corner of an image.
        /// </summary>
        /// <param name="filePath">The filepath to the image.</param>
        /// <param name="value">The text to write as watermark.</param>
        /// <param name="position">The position of the text on the image.</param>
        public static void AddWatermarkString(string filePath,
                                              string value, 
                                              string outputPath,
                                              PositionInImage position)
        {
            Image img = Image.FromFile(filePath);
            using (var g = Graphics.FromImage(img))
            {
                Font f = new Font("Calibri", 15);
                Brush b = new SolidBrush(Color.Black);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                int x = 15;
                int y = 15;

                switch (position)
                {
                    case PositionInImage.TopLeft:
                        break;
                    case PositionInImage.TopRight:
                        x = img.Width - (int)Math.Ceiling(g.MeasureString(value, f).Width);
                        break;
                    case PositionInImage.BottomLeft:
                        y = img.Height - 25;
                        break;
                    case PositionInImage.BottomRight:
                        x = img.Width - (int)Math.Ceiling(g.MeasureString(value, f).Width);
                        y = img.Height - 25;
                        break;
                }

                g.DrawString(value, f, b, x, y);
            }
            img.Save(outputPath);
        }

        /// <summary>
        /// Adds a watermark icon to an image and saves it to a new file.
        /// </summary>
        /// <param name="originalImageFilePath">Path to the image that will get a watermark.</param>
        /// <param name="watermarkFilePath">Path to the image with the watermark.</param>
        /// <param name="outputFilePath">Path where the watermarked image will be created.</param>
        /// <param name="position">The position of the watermark on the image.</param>
        public static void AddWatermarkIcon(string originalImageFilePath, 
                                            string watermarkFilePath,
                                            string outputFilePath,
                                            PositionInImage position)
        {
            Image img = Image.FromFile(originalImageFilePath);
            Image watermark = Image.FromFile(watermarkFilePath);

            int x = 0;
            int y = 0;

            switch (position)
            {
                case PositionInImage.TopLeft:
                    x = 0;
                    y = 0;
                    break;
                case PositionInImage.TopRight:
                    x = img.Width - watermark.Width;
                    y = 0;
                    break;
                case PositionInImage.BottomLeft:
                    x = 0;
                    y = img.Height - watermark.Height;
                    break;
                case PositionInImage.BottomRight:
                    x = img.Width - watermark.Width;
                    y = img.Height - watermark.Height;
                    break;
            }

            using (var g = Graphics.FromImage(img))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(watermark, x, y, watermark.Width, watermark.Height);
            }
            img.Save(outputFilePath);
        }

        /// <summary>
        /// Represents a shape.
        /// </summary>
        private enum Shape
        {
            /// <summary>
            /// A square.
            /// </summary>
            SQUARE,

            /// <summary>
            /// A circle.
            /// </summary>
            CIRCLE,

            /// <summary>
            /// A triangle.
            /// </summary>
            TRIANGLE,

            /// <summary>
            /// A rectangle standing up.
            /// </summary>
            RECTANGLE_VERTICAL,

            /// <summary>
            /// A rectangle laying down.
            /// </summary>
            RECTANGLE_HORIZONTAL
        }

        /// <summary>
        /// Generates an image.
        /// </summary>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <returns>Returns a bitmap.</returns>
        public static Bitmap GenerateImage(int width, int height)
        {
            Color[] colors = new Color[] { Color.Red,
                                           Color.Blue,
                                           Color.Green,
                                           Color.Yellow,
                                           Color.Black,
                                           Color.Gray,
                                           Color.White,
                                           Color.Purple,
                                           Color.Pink,
                                           Color.Brown,
                                           Color.Orange};

            Bitmap bmp = new Bitmap(width, height);

            DrawShapes(bmp, new SolidBrush(colors[new Random().Next(0, colors.Length)]));
            DrawShapes(bmp, new SolidBrush(colors[new Random().Next(0, colors.Length)]));
            DrawShapes(bmp, new SolidBrush(colors[new Random().Next(0, colors.Length)]));

            return bmp;
        }

        /// <summary>
        /// Draws random shapes on a bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <param name="b">The brush to draw with.</param>
        private static void DrawShapes(Bitmap bitmap, Brush b)
        {
            Array values = Enum.GetValues(typeof(Shape));
            Random random = new Random();
            Shape currentShape = (Shape)values.GetValue(random.Next(values.Length));

            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int ZERO = 0;
            int QUARTER_WIDTH = bitmap.Width / 4;
            int HALF_WIDTH = bitmap.Width / 2;
            int THREEQUARTER_WIDTH = (int)(bitmap.Width * 0.75);
            int QUARTER_HEIGHT = bitmap.Height / 4;
            int HALF_HEIGHT = bitmap.Height / 2;
            int THREEQUARTER_HEIGHT = (int)(bitmap.Height * 0.75);
            int RIGHTMOST = bitmap.Width;
            int BOTTOMMOST = bitmap.Height;

            switch (currentShape)
            {
                case Shape.SQUARE:
                    graphics.FillRectangle(b, new Rectangle(ZERO, ZERO, HALF_WIDTH, HALF_HEIGHT)); // Top left
                    graphics.FillRectangle(b, new Rectangle(HALF_WIDTH, ZERO, HALF_WIDTH, HALF_HEIGHT)); // Top right
                    graphics.FillRectangle(b, new Rectangle(ZERO, HALF_HEIGHT, HALF_WIDTH, HALF_HEIGHT)); // Bottom left
                    graphics.FillRectangle(b, new Rectangle(HALF_WIDTH, HALF_HEIGHT, HALF_WIDTH, HALF_HEIGHT)); // Bottom right
                    break;

                case Shape.CIRCLE:
                    graphics.FillEllipse(b, new Rectangle(ZERO, ZERO, HALF_WIDTH, HALF_HEIGHT)); // Top left
                    graphics.FillEllipse(b, new Rectangle(HALF_WIDTH, ZERO, HALF_WIDTH, HALF_HEIGHT)); // Top right
                    graphics.FillEllipse(b, new Rectangle(ZERO, HALF_HEIGHT, HALF_WIDTH, HALF_HEIGHT)); // Bottom left
                    graphics.FillEllipse(b, new Rectangle(HALF_WIDTH, HALF_HEIGHT, HALF_WIDTH, HALF_HEIGHT)); // Bottom right
                    break;

                case Shape.TRIANGLE:
                    graphics.FillPolygon(b, new Point[] { new Point(ZERO, HALF_HEIGHT), new Point(QUARTER_WIDTH, ZERO), new Point(HALF_WIDTH, HALF_HEIGHT) }); // Top left
                    graphics.FillPolygon(b, new Point[] { new Point(HALF_WIDTH, HALF_HEIGHT), new Point(THREEQUARTER_WIDTH, ZERO), new Point(RIGHTMOST, HALF_HEIGHT) }); // Top right
                    graphics.FillPolygon(b, new Point[] { new Point(ZERO, BOTTOMMOST), new Point(QUARTER_WIDTH, HALF_HEIGHT), new Point(HALF_WIDTH, BOTTOMMOST)}); // // Bottom left
                    graphics.FillPolygon(b, new Point[] { new Point(HALF_WIDTH, BOTTOMMOST), new Point(THREEQUARTER_WIDTH, HALF_WIDTH), new Point(RIGHTMOST, BOTTOMMOST) }); // Bottom right 
                    break;

                case Shape.RECTANGLE_HORIZONTAL:
                    graphics.FillRectangle(b, new Rectangle(QUARTER_WIDTH, ZERO, QUARTER_WIDTH, BOTTOMMOST));
                    graphics.FillRectangle(b, new Rectangle(THREEQUARTER_WIDTH, ZERO, QUARTER_WIDTH, BOTTOMMOST));
                    break;

                case Shape.RECTANGLE_VERTICAL:
                    graphics.FillRectangle(b, new Rectangle(ZERO, QUARTER_HEIGHT, RIGHTMOST, QUARTER_HEIGHT));
                    graphics.FillRectangle(b, new Rectangle(ZERO, THREEQUARTER_HEIGHT, RIGHTMOST, QUARTER_HEIGHT));
                    break;
            }
            System.Threading.Thread.Sleep(200);
        }
    }
}
