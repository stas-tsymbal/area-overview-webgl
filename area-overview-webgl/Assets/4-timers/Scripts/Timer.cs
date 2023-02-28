using System;
using UnityEngine;

namespace _4_timers.Scripts
{
    public class Timer
    {
        public event Action<float> OnTimerValueChangedEvent;
        public event Action OnTimerFinishedEvent;

        public TimerType type { get; }
        public float remainingSeconds { get; private set; }
        public bool isPaused { get; private set; }
        
        
        public Timer(TimerType type)
        {
            this.type = type;
        }

        public Timer(TimerType type, float seconds)
        {
            this.type = type;
            remainingSeconds = seconds;
        }

        public void SetTime(float seconds)
        {
            remainingSeconds = seconds;
            OnTimerValueChangedEvent?.Invoke(remainingSeconds);
        }

        public void Start()
        {
            if (remainingSeconds == 0)
            {
                Debug.LogError($"timer, remaining time = {remainingSeconds}");
                OnTimerFinishedEvent?.Invoke();
            }

            isPaused = false;
            Subscribe();
            OnTimerValueChangedEvent?.Invoke(remainingSeconds);
        }
        
        public void Start(float seconds)
        {
            SetTime(seconds);
            Start();
        }

        public void Pause()
        {
            isPaused = true;
            Unsubscribe();
            OnTimerValueChangedEvent?.Invoke(remainingSeconds);
        }

        public void Resume()
        {
            isPaused = false;
            Subscribe();
            OnTimerValueChangedEvent?.Invoke(remainingSeconds);
        }

        public void Stop()
        {
            Unsubscribe();
            remainingSeconds = 0;
            OnTimerValueChangedEvent?.Invoke(remainingSeconds);
            OnTimerFinishedEvent?.Invoke();
        }

        public void Subscribe()
        {
            switch (type)
            {
                case TimerType.UpdateTick:
                    TimeInvoker.Instance.OnUpdateTimeTickedEvent += OnTick;
                    break;
                case TimerType.UpdateTickUnscale:
                    TimeInvoker.Instance.OnUpdateTimeUnscaleTickedEvent += OnTick;
                    break;
                case TimerType.OneSecTick:
                    TimeInvoker.Instance.OnOneSecondTickedEvent += OnOneSecondTick;
                    break;
                case TimerType.OneSecTickUnscale:
                    TimeInvoker.Instance.OnOneSecondUnscaledTickedEvent += OnOneSecondTick;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }   
        }
        
        public void Unsubscribe()
        {
            switch (type)
            {
                case TimerType.UpdateTick:
                    TimeInvoker.Instance.OnUpdateTimeTickedEvent -= OnTick;
                    break;
                case TimerType.UpdateTickUnscale:
                    TimeInvoker.Instance.OnUpdateTimeUnscaleTickedEvent -= OnTick;
                    break;
                case TimerType.OneSecTick:
                    TimeInvoker.Instance.OnOneSecondTickedEvent -= OnOneSecondTick;
                    break;
                case TimerType.OneSecTickUnscale:
                    TimeInvoker.Instance.OnOneSecondUnscaledTickedEvent -= OnOneSecondTick;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }  
        }

        public void OnTick(float deltaTime)
        {
            if(isPaused) return;
            remainingSeconds -= deltaTime;
            CheckFinishTimer();
        }
        
        private void OnOneSecondTick()
        {
            if(isPaused) return;
            remainingSeconds -= 1;
            CheckFinishTimer();
        }

        public void CheckFinishTimer()
        {
            if(remainingSeconds <=0) Stop();
            else OnTimerValueChangedEvent?.Invoke(remainingSeconds);
        }
    }
}