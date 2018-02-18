using System;

namespace Countdown_Timer.Contracts
{
    public interface ITimer
    {
        event EventHandler Tick;
        //---------------------------------------------------------------------
        double IntervalInSeconds { get; set; }
        bool IsEnabled           { get; }
        //---------------------------------------------------------------------
        void Start();
        void Stop();
    }
}
