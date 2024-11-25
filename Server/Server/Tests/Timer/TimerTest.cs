using GoLogic.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Timer
{
    public class TimerTest
    {
        [Fact]
        public void TestHasTimeRemaining()
        {
            BasicTimer timer = new BasicTimer(TimeSpan.FromSeconds(10));
            timer.Start();
            Thread.Sleep(5000);
            timer.Pause();
            Assert.True(timer.HasTimeRemaining());
        }


        [Fact]
        public void TestNoTimeRemaining()
        {
            BasicTimer timer = new BasicTimer(TimeSpan.FromSeconds(3));
            timer.Start();
            Thread.Sleep(4000);
            timer.Pause();
            Assert.False(timer.HasTimeRemaining());
        }

        [Fact]
        public void TestResume()
        {
            BasicTimer timer = new BasicTimer(TimeSpan.FromSeconds(5));
            timer.Start();
            Thread.Sleep(1000);
            timer.Pause();
            timer.Resume();
            Thread.Sleep(1000);
            Assert.True(timer.HasTimeRemaining());
        }
    }
}
