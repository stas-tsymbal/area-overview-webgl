using System;
using UnityEngine;
using _4_timers.Scripts;
using UnityEngine.EventSystems;

namespace Area_overview_webgl.Scripts.ParallelAreaScripts
{
    /**
     * This class control Timer for enabling/disabling ParallelAreaIndicator
     */
    public class ParallelAreaIndicatorActivationController : MonoBehaviour
    {
        [Header("After this time we disable cursor")] [SerializeField]
        private float disablingTime = 3f;

        private Timer timer;
        
        private bool timerIsRunning;
        private bool isMousePressed;

        private Vector3 currentMousePosition = Vector3.zero; // current mouse position, use for checking enabling/disabling ParallelAreaIndicator

        public Action<bool> OnChangeCursorIndicatorState; // call when cursor change state (enabling/disabling)
        
        public void Init()
        {
            AddNewTimer();
            Subscribe();
        }

        private void Subscribe()
        {
            timer.OnTimerFinishedEvent += OnTimerFinishedEvent;
        }

        private void Unsubscribe()
        {
            timer.OnTimerFinishedEvent -= OnTimerFinishedEvent;
        }

        private void FixedUpdate()
        {
            if (isMousePressed) return;
            
            TryStartTimerForDisablingCursor();
        }

        private void Update()
        {
            DetectMouseClick();
        }

        private void DetectMouseClick()
        {
            if (Input.GetMouseButton(0))
            {
                isMousePressed = true;
                UpdateCurrentMousePosition(Input.mousePosition);
                // stop timer and disable cursor indicator
                if (timerIsRunning) PauseTimer();
                OnChangeCursorIndicatorState?.Invoke(false);
            }
            else
                isMousePressed = false;
        }


        // Set new current mouse position
        private void UpdateCurrentMousePosition(Vector3 _position)
        {
            currentMousePosition = _position;
        }

        // Try start Timer if we don't moving or clicking mouse
        void TryStartTimerForDisablingCursor()
        {
            if (currentMousePosition == Input.mousePosition)
            {
                // start check 
                if (!IsTimerRunning())
                    StartTimer();
            }
            else
            {
                UpdateCurrentMousePosition(Input.mousePosition);
                if (IsTimerRunning())
                {
                    PauseTimer();
                    if (!EventSystem.current.IsPointerOverGameObject())
                        OnChangeCursorIndicatorState?.Invoke(true);
                }
            }
        }
        
        #region Timer

        private void AddNewTimer()
        {
            timer = new Timer(TimerType.OneSecTick, disablingTime);
            StartTimer();
        }

        private void StartTimer()
        {
            SetStateTimerIsRunning(true);
            timer.Start(disablingTime);
        }

        private void PauseTimer()
        {
            SetStateTimerIsRunning(false);
            timer.Pause();
        }

        // TIMER FINISH
        private void OnTimerFinishedEvent()
        {
            OnChangeCursorIndicatorState?.Invoke(false);
        }

        private void SetStateTimerIsRunning(bool _val)
        {
            timerIsRunning = _val;
        }

        private bool IsTimerRunning()
        {
            return timerIsRunning;
        }

        #endregion
    }
}