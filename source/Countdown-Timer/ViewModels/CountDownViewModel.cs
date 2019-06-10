using System;
using System.ComponentModel;
using System.Windows.Input;
using Countdown_Timer.Abstractions;
using Countdown_Timer.Models;
using Countdown_Timer.Services;

namespace Countdown_Timer.ViewModels
{
    public sealed class CountDownViewModel : INotifyPropertyChanged
    {
        private readonly ITimer  _timer;
        private readonly IBeeper _beeper;
        private readonly int     _startSeconds;
        private readonly int     _secondsForYellow;
        //---------------------------------------------------------------------
        public CountDownViewModel() : this(new DispatcherTimerWrapper(), Configuration.Default, new Beeper(Configuration.Default)) { }
        //---------------------------------------------------------------------
        public CountDownViewModel(ITimer timer, IConfiguration config, IBeeper beeper)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            _startSeconds     = config.StartSeconds;
            _secondsForYellow = config.SecondsForYellow;
            this.FontSize     = config.FontSize;

            _timer  = timer  ?? throw new ArgumentNullException(nameof(timer));
            _beeper = beeper ?? throw new ArgumentNullException(nameof(beeper));
        }
        //---------------------------------------------------------------------
        private State _state;
        public State State
        {
            get => _state;
            set
            {
                if (_state == value) return;

                _state = value;

                this.OnPropertyChanged(nameof(this.State));
            }
        }
        //---------------------------------------------------------------------
        private int _seconds;
        public int Seconds
        {
            get => _seconds;
            set
            {
                if (_seconds == value) return;

                _seconds = value;

                this.OnPropertyChanged(nameof(this.Seconds));
            }
        }
        //---------------------------------------------------------------------
        public int FontSize { get; }
        //---------------------------------------------------------------------
        private RelayCommand _exitCommand;
        public ICommand ExitCommand => _exitCommand ?? (_exitCommand = new RelayCommand(_ => App.Current.Shutdown()));
        //---------------------------------------------------------------------
        private RelayCommand _startCommand;
        public ICommand StartCommand => _startCommand ?? (_startCommand = new RelayCommand(
            _ => this.Start(),
            _ => !_timer.IsEnabled));
        //---------------------------------------------------------------------
        private RelayCommand _stopCommand;
        public ICommand StopCommand => _stopCommand ?? (_stopCommand = new RelayCommand(
            _ => this.Stop(),
            _ => _timer.IsEnabled));
        //---------------------------------------------------------------------
        public event PropertyChangedEventHandler PropertyChanged;
        //---------------------------------------------------------------------
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //---------------------------------------------------------------------
        private void Start()
        {
            this.Seconds             = _startSeconds;
            this.State               = State.Red;
            _timer.IntervalInSeconds = 1d;
            _timer.Tick    += this.Timer_Tick;
            _timer.Start();
        }
        //---------------------------------------------------------------------
        private void Stop()
        {
            _timer.Stop();
            _timer.Tick -= this.Timer_Tick;
            this.Seconds = 0;
            this.State   = State.Stopped;
        }
        //---------------------------------------------------------------------
        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Seconds--;
            _beeper.Beep(this.Seconds);

            if (this.Seconds == _secondsForYellow)
                this.State = State.Yellow;
            else if (this.Seconds == 0)
                this.State = State.Green;
            else if (this.Seconds == -1)
            {
                this.Seconds = _startSeconds;
                this.State = State.Red;
            }
        }
    }
}
