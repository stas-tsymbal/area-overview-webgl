using System;
using UnityEngine;

namespace _4_timers.Scripts
{
    public class TimerTestController : MonoBehaviour
    {
        public TimerType type;
        public float seconds = 5f;

        private Timer timer;

        private void Awake()
        {
            timer = new Timer(type, seconds);
        }

        private void OnEnable()
        {
            timer.OnTimerValueChangedEvent += OnValueChange;
            timer.OnTimerFinishedEvent += OnTimerFinished;
        }

        private void OnDisable()
        {
            timer.OnTimerValueChangedEvent -= OnValueChange;
            timer.OnTimerFinishedEvent -= OnTimerFinished;
        }

        void OnValueChange(float second)
        {
            Debug.Log($"seconds left {second}");
        }
        
        void OnTimerFinished()
        {
            Debug.Log($"Finished ");
        }

        [ContextMenu("start")]
        public void StartTimer()
        {
            timer.Start();
        }

        [ContextMenu("pause")]
        public void PauseTimer()
        {
            if(timer.isPaused)  timer.Resume();
            else timer.Pause();
        }

        [ContextMenu("stop")]
        public void StopTimer()
        {
            timer.Stop();
        }

        [ContextMenu("update timer")]
        public void UpdateTime()
        {
            timer.SetTime(seconds);
        }
    }
    
    
}