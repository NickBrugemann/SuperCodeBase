using SuperCodeBase.helpers;
using SuperCodeBase.helpers.api;
using SuperCodeBase.statistics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SuperCodeBase
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        WindowsDrawHelper wdh = new WindowsDrawHelper();

        private void Main_Load(object sender, EventArgs e)
        {
            #region Read txt, to datatable, then to csv
            //string path = AppDomain.CurrentDomain.BaseDirectory + "\\" + "testFile.txt";
            //DataTable testio = FileMaster_v2.GetDataTableFrom(path);
            //FileMaster_v2.DataTableToCsv(testio, ",", AppDomain.CurrentDomain.BaseDirectory + "\\" + "testio.csv");
            #endregion

            #region Start showing latest price of bitcoin
            //ShowPriceHistory();

            //System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            //timer.Interval = 5000;
            //timer.Tick += Timer_Tick;
            //timer.Start();
            #endregion

            #region Add watermark to images
            //string originalPath = @"C:\Users\Nick\Documents\Visual Studio 2017\Projects\SuperCodeBase\SuperCodeBase\bin\Debug\image.jpg";
            //string watermarkPath = @"C:\Users\Nick\Documents\Visual Studio 2017\Projects\SuperCodeBase\SuperCodeBase\bin\Debug\watermark.png";
            //ImageHelper.AddWatermarkIcon(originalPath, watermarkPath, "topright.png", ImageHelper.PositionInImage.TopRight);
            //ImageHelper.AddWatermarkIcon(originalPath, watermarkPath, "topleft.png", ImageHelper.PositionInImage.TopLeft);
            //ImageHelper.AddWatermarkIcon(originalPath, watermarkPath, "bottomright.png", ImageHelper.PositionInImage.BottomRight);
            //ImageHelper.AddWatermarkIcon(originalPath, watermarkPath, "bottomleft.png", ImageHelper.PositionInImage.BottomLeft);
            //ImageHelper.AddWatermarkString(originalPath, "Een hele lange zin om de breedte te checken", "xtopleft.png", ImageHelper.PositionInImage.TopLeft);
            //ImageHelper.AddWatermarkString(originalPath, "Een hele lange zin om de breedte te checken", "xtopright.png", ImageHelper.PositionInImage.TopRight);
            //ImageHelper.AddWatermarkString(originalPath, "Een hele lange zin om de breedte te checken", "xbottomleft.png", ImageHelper.PositionInImage.BottomLeft);
            //ImageHelper.AddWatermarkString(originalPath, "Een hele lange zin om de breedte te checken", "xbottomright.png", ImageHelper.PositionInImage.BottomRight);
            #endregion

            #region Add & start timer for retrieving the latest price
            //System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            //timer.Interval = 5000;
            //client.SetTimedAction(timer, delegate
            //{
            //    double latestPrice = client.GetLatestBtcUsdcPrice();
            //    if (priceHistory.Count > 0)
            //    {
            //        if (priceHistory.Last() == latestPrice)
            //        {
            //            return;
            //        }
            //    }

            //    priceHistory.Add(latestPrice);
            //    lisPriceHistory.Items.Clear();

            //    foreach (double d in priceHistory)
            //    {
            //        lisPriceHistory.Items.Add(d.ToString());
            //    }
            //}, 5000);
            #endregion

            chart1.Series.Add("lucian");
            chart1.Series["lucian"].Points.Add(new DataPoint(0, 2));
            chart1.Series["lucian"].Points.Add(new DataPoint(1, 1));
            chart1.Series["lucian"].Points.Add(new DataPoint(2, 2));
            chart1.Series["lucian"].Points.Add(new DataPoint(3, 3));
            chart1.Series["lucian"].Points.Add(new DataPoint(4, 5));
            chart1.Series["lucian"].ChartType = SeriesChartType.Line;

            wdh.font = new Font(FontFamily.GenericSansSerif, 26);
            wdh.StartPersistentDraw();
            wdh.ChangeValue("rrrrrra");
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            wdh.ChangeValue("test clicked");

            //Graphics g = pictureBox.CreateGraphics();
            ////BarChart barChart = new BarChart();
            ////barChart.title = "Fruits";
            ////barChart.AddBar("Apples", 300, colors[0]);
            ////barChart.AddBar("Bananas", 600, colors[1]);
            ////barChart.AddBar("Oranges", 200, colors[2]);
            ////barChart.AddBar("Grapes", 180, colors[3]);
            ////barChart.AddBar("Pears", 573, colors[4]);
            ////barChart.Create(g);

            //PieChart pieChart = new PieChart();
            //pieChart.SetColorPalette(PieChart.ColorPalette.RAINBOW);
            //pieChart.AddSegment("A", 300);
            //pieChart.AddSegment("B", 600);
            //pieChart.AddSegment("C", 200);
            //pieChart.AddSegment("D", 180);
            //pieChart.AddSegment("E", 10);
            //pieChart.AddSegment("F", 280);
            //pieChart.AddSegment("G", 103);
            //pieChart.AddSegment("H", 94);
            //pieChart.AddSegment("I", 45);
            //pieChart.AddSegment("J", 502);
            //pieChart.Create(g);

            //Dictionary<String, int> extensions = new Dictionary<String, int>();
            //string[] files = Directory.GetFiles(@"C:\Users\Nick\Documents\Visual Studio 2017\Projects\SuperCodeBase", "*.*", SearchOption.AllDirectories);

            //foreach (string file in files)
            //{
            //    string extension = Path.GetExtension(file);
            //    int count = 0;
            //    if (extensions.TryGetValue(extension, out count))
            //    {
            //        extensions[extension]++;
            //    }
            //    else
            //    {
            //        extensions.Add(extension, 1);
            //    }
            //}

            //PieChart pieChart = new PieChart();
            //pieChart.SetColorPalette(PieChart.ColorPalette.RAINBOW);

            //foreach (var kvp in extensions)
            //{
            //    pieChart.AddSegment(kvp.Key, kvp.Value);
            //}

            //pieChart.Create(panel1.CreateGraphics());
            //pieChart.SaveAsImage("schoolmap.png");
        }

        private void Main_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("X " + Cursor.Position.X + " - Y " + Cursor.Position.Y);
        }
    }
}
