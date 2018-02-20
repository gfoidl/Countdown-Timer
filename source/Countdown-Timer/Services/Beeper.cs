using System;
using System.Threading.Tasks;
using Countdown_Timer.Contracts;

namespace Countdown_Timer.Services
{
    public class Beeper : IBeeper
    {
        private readonly int _startSeconds;
        private readonly int _secondsToStartBeeping;
        //---------------------------------------------------------------------
        public Beeper(IConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            _startSeconds          = config.StartSeconds;
            _secondsToStartBeeping = config.StartBeeping;
        }
        //---------------------------------------------------------------------
        public void Beep(int seconds)
        {
            if (seconds == 0)
                this.FinalBeepAsync();
            else if (seconds % 10 == 0 && seconds != _startSeconds)
                this.ShortBeepAsync();
            else if ((uint)seconds <= (uint)_secondsToStartBeeping)
                this.ShortBeepAsync();
        }
        //---------------------------------------------------------------------
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async void ShortBeepAsync() => await Task.Run(() => Console.Beep(1250, 250));
        private async void FinalBeepAsync() => await Task.Run(() => Console.Beep(2000, 600));
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
