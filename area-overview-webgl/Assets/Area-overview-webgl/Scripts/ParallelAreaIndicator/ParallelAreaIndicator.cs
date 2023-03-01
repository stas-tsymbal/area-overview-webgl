using _4_timers.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Area_overview_webgl.Scripts.ParallelAreaIndicator
{
    public class ParallelAreaIndicator : MonoBehaviour
    {
        public static ParallelAreaIndicator Instance;

        [Header("After this time we disable cursor")] [SerializeField]
        private float disablingTime = 3f;

        [Header("Cursor animation duration on turn off/on")] [SerializeField]
        private float cursorAnimationTime = 0.1f;

        [Range(0, 1)] [SerializeField] private float maxIndicatorAlphaColor = 0.4f;

        [Header("Don't apply parallel in this layer")] [SerializeField]
        private LayerMask ignoreLayer;

        private Camera myCamera; // make ray from this camera
        private Transform cursorIndicator; // this object we set when ray collide with some area 
        private Image indicatorImg;

        private Vector3 currentMousePosition = Vector3.zero; // current mouse position, use for check disable/enable cursorIndicator

        private bool timerIsRunning;
        private bool isIndicatorDisable;
        private bool isMousePressed;
        private bool indicatorDisableForTeleport;

        private Timer timer;

        private void Awake()
        {
            Instance = this;

            AddNewTimer();
        }

        private void Start()
        {
            cursorIndicator = transform.GetChild(0);
            indicatorImg = GetComponentInChildren<Image>();
            myCamera = Camera.main;

            // disable indicator for mobile
            if (Application.isMobilePlatform)
                gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            timer.OnTimerFinishedEvent += OnTimerFinishedEvent;
        }

        private void OnDisable()
        {
            timer.OnTimerFinishedEvent -= OnTimerFinishedEvent;
        }
        
        private void FixedUpdate()
        {
            if (isMousePressed) return;

            if (indicatorDisableForTeleport) return;

            TryStartTimerForDisablingCursor();

            DrawParallelAreaIndicator();
        }

        private void Update()
        {
            DetectMouseClick();
        }

        // Draw parallel area indicator
        private void DrawParallelAreaIndicator()
        {
            RaycastHit hit;
            var ray = myCamera.ScreenPointToRay(Input.mousePosition);
            // make normal to hit and set cursorIndidcator
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreLayer))
            {
                cursorIndicator.position =
                    hit.point + hit.normal / 100; // set cursor indicator over hit point(height of cursor indicator);
                // '/100' - use for correct distance from hit point to cursorIndicator(bigger value -> less distance) 
                cursorIndicator.LookAt(hit.point);

                // DEBUG DRAW RAYCAST
                // Debug.DrawRay(hit.point, camera.transform.position, Color.green); // ray from camera
                // Debug.DrawLine(vectorNormal, hit.point, Color.red); // normal ray from object
            }
            else
            {
                // disable cursor
                indicatorImg.color = new Color32(255, 255, 255, 0);
            }
        }

        void DetectMouseClick()
        {
            if (Input.GetMouseButton(0))
            {
                isMousePressed = true;
                SetCurrentMousePosition(Input.mousePosition);
                // stop timer and disable cursor indicator
                if (timerIsRunning) PauseTimer();
                if (!isIndicatorDisable) SetIndicatorMaxColor(false);
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
                    if (!EventSystem.current.IsPointerOverGameObject() && isIndicatorDisable)
                        SetIndicatorMaxColor(true);
                }
            }
        }

        #region EnablingDisablingCursor

        // on/off indicator , true - enable indicator, false - disable
        void SetIndicatorMaxColor(bool _val)
        {
            if (_val)
            {
                // enable image
                LeanTween.value(gameObject, UpdateImageAlpha, 0f, maxIndicatorAlphaColor, cursorAnimationTime)
                    .setOnStart(() => SetStateIndicator(false));
                ;
            }
            else
            {
                // disable image
                LeanTween.value(gameObject, UpdateImageAlpha, maxIndicatorAlphaColor, 0f, cursorAnimationTime)
                    .setOnComplete(() => SetStateIndicator(true));
                ;
            }
        }

        void SetStateIndicator(bool state)
        {
            isIndicatorDisable = state;
        }

        void UpdateImageAlpha(float alpha)
        {
            indicatorImg.color = new Color(indicatorImg.color.r, indicatorImg.color.g, indicatorImg.color.b, alpha);
        }

        public void DisableIndicatorForTeleport(bool _val)
        {
            indicatorDisableForTeleport = _val;
            if (_val && gameObject.activeSelf)
                SetIndicatorMaxColor(false); // disable indicator
        }

        public void DisableCursor()
        {
            SetIndicatorMaxColor(false);
        }

        #endregion


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
            if (!isIndicatorDisable) SetIndicatorMaxColor(false);
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