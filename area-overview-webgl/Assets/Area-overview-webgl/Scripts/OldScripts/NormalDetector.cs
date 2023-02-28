using System;
using System.Collections;
using System.Collections.Generic;
using _4_timers.Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
 * Script draw image that parallel to object that hit ray from mouse cursor
 */
public class NormalDetector : MonoBehaviour
{
    [Header("Time for disable indicator")] [SerializeField]
    private float disableTime = 3f;
    
    public static NormalDetector Instance;
    
    [SerializeField] private Camera camera; // make ray from this camera
    
    private Transform cursorIndicator; // this object we set when ray collide with some area 
    private Image indicatorImg;
    
    private Vector3 currentMousePosition = Vector3.zero; // current mouse position, use for check disable/enable cursorIndicator
    private bool timerCoroutineIsRunning = false;

    private bool isIndicatorDisable = false;
    private bool isMousePressed = false;
    [Range(0, 255)]
    [SerializeField] private byte indicatorAlpha = 50;

    [SerializeField] private bool indicatorDisableForTeleport = false;
    [SerializeField] private LayerMask ignoreLayer;

    private Timer timer;
    private void Awake()
    {
        Instance = this;
        
        timer = new Timer(TimerType.OneSecTick, disableTime);
        StartTimer();
    }

    #region timer

    public void StartTimer()
    {
        SetStateTimerIsRunning(true);
        timer.Start(disableTime);
    }
    
    public void StopTimer()
    {
        SetStateTimerIsRunning(false);
        timer.Stop();
    }
    
    private void OnEnable()
    {
        timer.OnTimerFinishedEvent += OnTimerFinishedEvent;
    }
    
    public void OnTimerFinishedEvent()
    {
        SetIndicatorMaxColor(false);
    }

    #endregion
   

    private void Start()
    {
        cursorIndicator = transform.GetChild(0);
        indicatorImg = GetComponentInChildren<Image>();
        
        // disable indicator for mobile
        if(Application.isMobilePlatform)
            gameObject.SetActive(false);
    }

    private void FixedUpdate(){
        
        if(isMousePressed) return;

        if(indicatorDisableForTeleport) return;
        
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
                StopTimer();
                if(!EventSystem.current.IsPointerOverGameObject()) SetIndicatorMaxColor(true);
            }
        }

        RaycastHit hit;
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        // make normal to hit and set cursorIndidcator
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreLayer)) {
            cursorIndicator.position = hit.point + hit.normal/100; // set indicator over hit point; '/100' - use for correct distance from hit point to cursorIndicator(bigger value -> less distance) 
            cursorIndicator.LookAt( hit.point);
            
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

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isMousePressed = true;
            StopTimer();
            SetCurrentMousePosition(Input.mousePosition);
            if (!isIndicatorDisable)
                SetIndicatorMaxColor(false);
        }
        else
            isMousePressed = false;
    }

    // disable hit indicator after timer
    private IEnumerator TryDisableCursorIndicator(float _time)
    {
       
        yield return new WaitForSeconds(_time);
        
        
    }
    
    // set state for timer coroutine
    private void SetStateTimerIsRunning(bool _val)
    {
        timerCoroutineIsRunning = _val;
    }

    // get state for timer coroutine
    private bool IsTimerRunning()
    {
        return timerCoroutineIsRunning;
    }

    // on/off indicator , true - enable indicator, false - disable
    void SetIndicatorMaxColor(bool _val)
    {
        if (_val)
        {
            byte maxAlpha = indicatorAlpha;
            indicatorImg.color = new Color32(255, 255, 255, maxAlpha);
            isIndicatorDisable = false;
        }
        else
        {
            // disable image
            byte minAlpha = 0;
            indicatorImg.color = new Color32(255, 255, 255, minAlpha);
            isIndicatorDisable = true;
        }
    }

    // set current mouse position
    private void SetCurrentMousePosition(Vector3 _position)
    {
        currentMousePosition = _position; 
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
}
