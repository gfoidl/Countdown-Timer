using System;
using System.Windows.Threading;
using Countdown_Timer.Abstractions;

namespace Countdown_Timer.Services
{
    public class DispatcherTimerWrapper : ITimer
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.DataBind);
        //---------------------------------------------------------------------
        public event EventHandler Tick
        {
            add    { _timer.Tick += value; }
            remove { _timer.Tick -= value; }
        }
        //---------------------------------------------------------------------
        public double IntervalInSeconds
        {
            get => _timer.Interval.TotalSeconds;
            set => _timer.Interval = TimeSpan.FromSeconds(value);
        }
        //---------------------------------------------------------------------
        public bool IsEnabled => _timer.IsEnabled;
        public void Start()   => _timer.Start();
        public void Stop()    => _timer.Stop();
    }
}
