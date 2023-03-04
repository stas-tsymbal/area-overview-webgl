using System;
using UnityEngine;
using _4_timers.Scripts;
using UnityEngine.EventSystems;

namespace Area_overview_webgl.Scripts.ParallelAreaScripts
{
    public class ParallelAreaIndicatorActivationController : MonoBehaviour
    {
        public static ParallelAreaIndicatorActivationController Instance;
        
        private Timer timer;
        
        [Header("After this time we disable cursor")] [SerializeField]
        private float disablingTime = 3f;
        
        private bool timerIsRunning;
        private bool isMousePressed;
        private bool indicatorDisableForTeleport;

        private Vector3 currentMousePosition = Vector3.zero; // current mouse position, use for check disable/enable cursorIndicator
        
        public Action<bool> OnChangeCursorIndicatorState;
        
        private void Awake()
        {
            Instance = this;
        }
        
        public void Init()
        {
            AddNewTimer();
            Subscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
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

            if (indicatorDisableForTeleport) return;

            TryStartTimerForDisablingCursor();
        }

        private void Update()
        {
            DetectMouseClick();
        }
        
        void DetectMouseClick()
        {
            if (Input.GetMouseButton(0))
            {
                isMousePressed = true;
                SetCurrentMousePosition(Input.mousePosition);
                // stop timer and disable cursor indicator
                if (timerIsRunning) PauseTimer();
                OnChangeCursorIndicatorState?.Invoke(false);
            }
            else
                isMousePressed = false;
        }
        
        
        // set current mouse position
        private void SetCurrentMousePosition(Vector3 _position)
        {
            currentMousePosition = _position;
        }

        void TryStartTimerForDisablingCursor()
        {
            // disable/enable cursorIndicator if mouse don't move
            if (currentMousePosition == Input.mousePosition)
            {
                // start check 
                if (!IsTimerRunning())
                    StartTimer();
            }
            else
            {
                SetCurrentMousePosition(Input.mousePosition);
                if (IsTimerRunning())
                {
                    PauseTimer();
                    if (!EventSystem.current.IsPointerOverGameObject())
                        OnChangeCursorIndicatorState?.Invoke(true);
                }
            }
        }
        
        public void DisableIndicatorForTeleport(bool _val)
        {
            indicatorDisableForTeleport = _val;
            if (_val && gameObject.activeSelf)
                OnChangeCursorIndicatorState?.Invoke(false); // disable indicator
        }

        public void DisableCursor()
        {
            OnChangeCursorIndicatorState?.Invoke(false);
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