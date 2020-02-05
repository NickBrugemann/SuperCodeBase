using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCodeBase.helpers
{
    /// <summary>
    /// Contains methods regarding generating colors.
    /// </summary>
    static class ColorHelper
    {
        /// <summary>
        /// Generates an array of Colors in a rainbow fashion.
        /// </summary>
        /// <param name="length">The spread in amount of colors.</param>
        /// <returns></returns>
        public static Color[] GetRainbowColors(int length)
        {
            List<Color> colors = new List<Color>();
            for (int b = 0; b < length; b++) colors.Add(Color.FromArgb(255, 0, b * 255 / length));
            for (int r = length; r > 0; r--) colors.Add(Color.FromArgb(r * 255 / length, 0, 255));
            for (int g = 0; g < length; g++) colors.Add(Color.FromArgb(0, g * 255 / length, 255));
            for (int b = length; b > 0; b--) colors.Add(Color.FromArgb(0, 255, b * 255 / length));
            for (int r = 0; r < length; r++) colors.Add(Color.FromArgb(r * 255 / length, 255, 0));
            for (int g = length; g > 0; g--) colors.Add(Color.FromArgb(255, g * 255 / length, 0));
            colors.Add(Color.FromArgb(0, 255, 0));
            Color[] colorArray = new Color[length];
            for (int i = 0; i < length; i++)
            {
                colorArray[i] = colors[i * 6];
            }
            return colorArray;
        }

        /// <summary>
        /// Generates an array of Colors with a gradient from the first to the second color.
        /// </summary>
        /// <param name="color1">The color of the begin of the gradient.</param>
        /// <param name="color2">The color of the end of the gradient.</param>
        /// <param name="length">The spread in amount of colors.</param>
        /// <returns></returns>
        public static Color[] CreateGradientArray(Color color1, Color color2, int length)
        {
            Color[] colors = new Color[length];
            int stepRed = (color2.R - color1.R) / length;
            int stepGreen = (color2.G - color1.G) / length;
            int stepBlue = (color2.B - color1.B) / length;

            if (length <= 2)
            {
                return new Color[] { color1, color2 };
            }

            colors[0] = color1;
            colors[length - 1] = color2;

            for (int i = 1; i + 1 < length; i++)
            {
                int valueRed = (i * stepRed) + color1.R;
                int valueGreen = (i * stepGreen) + color1.G;
                int valueBlue = (i * stepBlue) + color1.B;
                Color color = Color.FromArgb(valueRed, valueGreen, valueBlue);
                colors[i] = color;
            }

            return colors;
        }

        /// <summary>
        /// Generates an array of random Colors.
        /// </summary>
        /// <param name="count">The amount of colors.</param>
        /// <returns></returns>
        public static Color[] CreateRandomColors(int length)
        {
            Color[] colors = new Color[length];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = RandomColor();
            }

            return colors;
        }

        /// <summary>
        /// Generates a random color.
        /// </summary>
        /// <returns></returns>
        public static Color RandomColor()
        {
            Random r = new Random();
            int red = r.Next(0, 256);
            int green = r.Next(0, 256);
            int blue = r.Next(0, 256);
            int alpha = r.Next(0, 256);
            System.Threading.Thread.Sleep(20);
            Color c = Color.FromArgb(alpha, red, green, blue);
            return c;
        }
    }
}
