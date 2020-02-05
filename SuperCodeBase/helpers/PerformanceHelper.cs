using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCodeBase.helpers
{
    /// <summary>
    /// Contains methods regarding measuring performance
    /// </summary>
    static class PerformanceHelper
    {
        /// <summary>
        /// Executes code, measures the time elapsed in milliseconds and returns a long.
        /// </summary>
        /// <param name="codeToTime">The code or method to time. NOTE: Encapsulate the code with in a delegate expression.</param>
        /// <returns>Returns the amount of milliseconds elapsed to run this code.</returns>
        public static long CodeToTime(Action codeToTime)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            codeToTime();
            stopWatch.Stop();
            return stopWatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Executes in background.
        /// </summary>
        /// <param name="mainCode">The code to execute.</param>
        public static void DoInBackground(Action mainCode)
        {
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += delegate { mainCode(); };
            bgw.RunWorkerAsync();
        }

        /// <summary>
        /// Executes code in background and executes code when completed.
        /// </summary>
        /// <param name="mainCode">The code to execute.</param>
        /// <param name="onComplete">Code to execute when completed.</param>
        public static void DoInBackground(Action mainCode, Action onComplete)
        {
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += delegate { mainCode(); };
            bgw.RunWorkerCompleted += delegate{ onComplete(); };
            bgw.RunWorkerAsync(); 
        }
    }
}
