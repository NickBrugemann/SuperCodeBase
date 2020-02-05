using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCodeBase.abstracts
{
    /// <summary>
    /// Grants functionality where you can add a timer with delegate as tick event.
    /// </summary>
    abstract class TimedActionObject
    {
        /// <summary>
        /// Sets a timed action.
        /// Whenever the timer elapses, the action gets executed and the timer restarts.
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="timerTick"></param>
        /// <param name="repeatTimeMs"></param>
        public void SetTimedAction(System.Windows.Forms.Timer timer,
                                   EventHandler timerTick,
                                   int repeatTimeMs)
        {
            timerTick.Invoke(null, null);
            timer.Tick += timerTick;
            timer.Start();
        }
    }
}
