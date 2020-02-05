using System;
using System.Drawing;
using System.Windows.Forms;

namespace SuperCodeBase.helpers
{
    class WindowsDrawHelper
    {
        private DisplayForm displayForm;
        private Timer timer;
        public Font font;

        public void StartPersistentDraw()
        {
            displayForm = new DisplayForm(font);
            timer = new Timer() { Interval = 500, Enabled = true };
            timer.Tick += new EventHandler(Timer_Tick);
        }

        public void ChangeValue(string value)
        {
            displayForm.DisplayValue = value;
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            if (!displayForm.Visible)
            {
                displayForm.Show();
                displayForm.SetDesktopLocation(Screen.PrimaryScreen.Bounds.Left, Screen.PrimaryScreen.Bounds.Top);
            }

            displayForm.Invalidate();
        }

        public class DisplayForm : Form
        {
            public Font Font;
            public String DisplayValue = "";

            public DisplayForm(Font f)
            {
                TopMost = true;
                ShowInTaskbar = false;
                FormBorderStyle = FormBorderStyle.None;
                BackColor = Color.LightGreen;
                TransparencyKey = Color.LightGreen;
                Width = 500;
                Height = 100;

                Paint += new PaintEventHandler(DisplayForm_Paint);

                Font = f;
            }

            void DisplayForm_Paint(object sender, PaintEventArgs e)
            {
                e.Graphics.DrawString(DisplayValue, Font, Brushes.Black, 10, 10);
            }
        }
    }
}
