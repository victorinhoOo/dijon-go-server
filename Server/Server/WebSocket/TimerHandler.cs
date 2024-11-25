using GoLogic.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket
{
    public class TimerHandler
    {
        private TimerManager timerManager;

        public TimerHandler()
        {
            this.timerManager = new TimerManager();
        }
    }
}
