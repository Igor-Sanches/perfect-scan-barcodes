﻿using System;
using System.ComponentModel;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Perfect_Scan.Tools
{
    public sealed class Timer : INotifyPropertyChanged
    {
        #region Fields
        private DispatcherTimer _ticker = new DispatcherTimer();
        private TimeSpan _descendingTime;
        private TimeSpan _defaultTimerInterval = new TimeSpan(0, 0, 0, 0, 100);
        private TimeSpan _defaultTimerDuration = new TimeSpan(0, 0, 60);
        private TimeSpan _timeLeft;
        private bool _timerHasEnded = false;


        #endregion

        #region Properties

        /// <summary>
        /// Shows the amount of time left for the timer. The interval property determines the accuracy of
        /// this value.
        /// </summary>
        public TimeSpan TimeLeft
        {
            get { return _descendingTime; }
            private set
            {
                _timeLeft = value;
                OnPropertyChanged("TimeLeft");

            }
        }

        /// <summary>
        /// How long the timer will run for. Default Value is 100 milliseconds.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary
        /// <para>
        /// Period of time that the before each timer update
        /// </para> 
        /// <para>Lower values give you more precise values when checking the time remaining</para>
        /// </summary>
        public TimeSpan Interval { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new timer. Its default interval is 100 milliseconds and its default duration is 30 seconds.
        /// </summary>
        ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
        public Timer()
        {
            _defaultTimerDuration = new TimeSpan(0, 0, 0, 1, 100);
            _timerHasEnded = false;
            _ticker.Tick += _ticker_Tick;
            SetTimerDuration();
            SetTimerInterval(_ticker);
        }
        public Timer(long timer)
        {
            _defaultTimerDuration = new TimeSpan(0,0,2,0);
            _timerHasEnded = false;
            _ticker.Tick += _ticker_Tick;
            SetTimerDuration();
            SetTimerInterval(_ticker);
        }


        /// <summary>
        ///  Creates a new timer. Its default interval is 100 milliseconds.
        /// </summary>
        /// <param name="duration"></param>
        public Timer(TimeSpan duration)
        {
            _timerHasEnded = false;
            _ticker.Tick += _ticker_Tick;
            SetTimerDuration(duration);
            SetTimerInterval(_ticker);
        }

        /// <summary>
        /// Creates a new timer.
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="interval"></param>
        public Timer(TimeSpan duration, TimeSpan interval)
        {
            _timerHasEnded = false;
            _ticker.Tick += _ticker_Tick;
            SetTimerDuration(duration);
            SetTimerInterval(_ticker, interval);
        }


        #endregion

        #region Methods


        private void SetTimerDuration()
        {
            Duration = _defaultTimerDuration;
            _descendingTime = _defaultTimerDuration;
        }

        private void SetTimerDuration(TimeSpan duration)
        {
            Duration = duration;
            _descendingTime = duration;
        }

        private void SetTimerInterval(DispatcherTimer ticker)
        {
            ticker.Interval = _defaultTimerInterval;
            Interval = _defaultTimerInterval;
        }

        private void SetTimerInterval(DispatcherTimer ticker, TimeSpan interval)
        {
            ticker.Interval = interval;
            Interval = interval;
        }


        /// <summary>
        /// Starts the timer
        /// </summary>
        public void StartTimer()
        {
            _ticker.Start();
            _timerHasEnded = false;
            OnTimerStartedEvent();
        }


        /// <summary>
        /// <para>Changes the value of TimeLeft to the current value of duration.</para>
        /// <para>Note: The timer does not stop running when this is used. In order for the timer to stop running, you must pause the timer first.</para>
        /// </summary>
        public void ResetTimer()
        {
            _descendingTime = Duration;
            _timerHasEnded = false;
            OnTimerResetEvent();
        }



        /// <summary>
        /// Pauses the timer
        /// </summary>
        public void PauseTimer()
        {
            _ticker.Stop();
            OnTimerPausedEvent();
        }





        /// <summary>
        /// <para>
        /// Change how often the timer updates
        /// </para> 
        /// <para>Lower values give you more precise values when checking the time remaining</para>
        /// </summary>
        /// <param name="interval"></param>
        public void ChangeInterval(TimeSpan interval)
        {
            SetTimerInterval(_ticker, interval);
        }

        #endregion

        #region Events

        /// <summary>
        /// Event that occurs when the timer has finished.
        /// </summary>
        public event TimerEventHandler TimerEnded;

        /// <summary>
        /// Event that occurs when the an interval has passed.
        /// </summary>
        public event TimerEventHandler TimerTicked;

        /// <summary>
        /// Event that occurs when the timer has been reset.
        /// </summary>
        public event TimerEventHandler TimerReset;

        /// <summary>
        /// Event that occurs when the timer has been paused.
        /// </summary>
        public event TimerEventHandler TimerPaused;

        /// <summary>
        /// Event that occurs when the timer starts.
        /// </summary>
        public event TimerEventHandler TimerStarted;


        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region Event Methods

        private void _ticker_Tick(object sender, object e)
        {
            _descendingTime = _descendingTime.Subtract(Interval);
            OnTimerTickedEvent();

            if (_descendingTime.Ticks <= 0)
            {
                _timerHasEnded = true;
                _ticker.Stop();
                OnRaiseTimerEndedEvent();

            }
        }

        private void OnTimerStartedEvent()
        {
            TimerStarted?.Invoke(this, new TimerEventArgs(_timerHasEnded));
        }

        private void OnTimerTickedEvent()
        {
            TimerTicked?.Invoke(this, new TimerEventArgs(false));
        }

        private void OnRaiseTimerEndedEvent()
        {
            TimerEnded?.Invoke(this, new TimerEventArgs(true));

        }

        private void OnTimerResetEvent()
        {
            TimerReset?.Invoke(this, new TimerEventArgs(_timerHasEnded));
        }

        private void OnTimerPausedEvent()
        {
            TimerPaused?.Invoke(this, new TimerEventArgs(_timerHasEnded));
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

    }

    public sealed class TimerEventArgs
    {
        public TimerEventArgs(bool isEnded)
        {
            ended = isEnded;
        }
        private bool ended;

        /// <summary>
        /// Whether or not the timer has ended.
        /// </summary>
        public bool TimerEnded
        {
            get { return ended; }
        }


    }
    public delegate void TimerEventHandler(object sender, TimerEventArgs e);

}
 
