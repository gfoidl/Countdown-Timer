using System;
using Countdown_Timer.Abstractions;

namespace Countdown_Timer.Tests
{
    public class TestTimer : ITimer
    {
        public double IntervalInSeconds { get; set; }
        public bool IsEnabled           { get; set; }
        //---------------------------------------------------------------------
        public event EventHandler Tick;
        //---------------------------------------------------------------------
        public void Start()  => this.IsEnabled = true;
        public void Stop()   => this.IsEnabled = false;
        //---------------------------------------------------------------------
        public void OnTick() => this.Tick?.Invoke(this, EventArgs.Empty);
    }
}
