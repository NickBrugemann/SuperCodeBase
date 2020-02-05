using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperCodeBase.forms
{
    /// <summary>
    /// Contains various advanced actions regarding System.Windows.Forms.MessageBox.
    /// </summary>
    static class MyMessageBox
    {
        #region stalled custom form
        //static Form formMessageBox = new Form();

        //public static void init()
        //{
        //    formMessageBox.Size = new System.Drawing.Size(450, 250);

        //    Label l = new Label();
        //    l.Location = new System.Drawing.Point(20, formMessageBox.Height);
        //    formMessageBox.Controls.Add(l);


        //}
        #endregion

        /// <summary>
        /// Shows a message box with retry and cancel.
        /// If retry is pressed, returns true.
        /// If cancel is pressed, returns false.
        /// </summary>
        /// <param name="text">The text to be displayed in the message box.</param>
        /// <param name="title">The title of the message box.</param>
        /// <param name="icon">The icon to be displayed in the message box.</param>
        /// <returns>Returns true when retry is pressed.</returns>
        public static bool ShowWithRetry(string text, string title = "", MessageBoxIcon icon = MessageBoxIcon.None)
        {
            for (;;) {
                DialogResult result = MessageBox.Show(text, title, MessageBoxButtons.RetryCancel, icon);
                if (result == DialogResult.Cancel)
                    return false;
            }
        }

        /// <summary>
        /// Shows a message box that disappears over time.
        /// </summary>
        /// <param name="text">The text to be displayed in the message box.</param>
        /// <param name="timeOutTime">The amount of milliseconds this message box is visible.</param>
        /// <param name="title">The title of the message box.</param>
        /// <param name="icon">The icon to be displayed in the message box.</param>
        /// <returns></returns>
        public static bool ShowWithTimeout(string text, int timeOutTime, string title = "", MessageBoxIcon icon = MessageBoxIcon.None)
        {
            DialogResult result = AutoClosingMessageBox.Show(text, title, timeOutTime, MessageBoxButtons.OKCancel);
            return result == DialogResult.OK;
        }

        /// <summary>
        /// Message box that automatically closes.
        /// </summary>
        private class AutoClosingMessageBox
        {
            System.Threading.Timer _timeoutTimer;
            string _caption;
            DialogResult _result;
            DialogResult _timerResult;
            AutoClosingMessageBox(string text, string caption, int timeout, MessageBoxButtons buttons = MessageBoxButtons.OK, DialogResult timerResult = DialogResult.None)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                    null, timeout, System.Threading.Timeout.Infinite);
                _timerResult = timerResult;
                using (_timeoutTimer)
                    _result = MessageBox.Show(text, caption, buttons);
            }
            public static DialogResult Show(string text, string caption, int timeout, MessageBoxButtons buttons = MessageBoxButtons.OK, DialogResult timerResult = DialogResult.None)
            {
                return new AutoClosingMessageBox(text, caption, timeout, buttons, timerResult)._result;
            }
            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow("#32770", _caption); // lpClassName is #32770 for MessageBox
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
                _result = _timerResult;
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }
    }
}
