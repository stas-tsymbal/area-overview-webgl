using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
 * Script calculate and draw indicator over mouse pointer
 * This script send ray from mouse pointer and find normal of object that ray collide
 */
public class NormalDetector : MonoBehaviour
{
    public static NormalDetector Instance;
  //  public static NormalDetector Instance;
    [SerializeField] private Camera camera; // make ray from this camera
    [SerializeField] private Transform hitIndicator; // this object we set when ray collide with some area 

    [Header("Time for disable indicator")] [SerializeField]
    private float disableTime = 3f;
    [SerializeField] private Image indicatorImg;
    private Vector3 currentMousePosition = Vector3.zero; // current mouse position, use for check disable/enable hitIndicator
    private bool timerCoroutineIsRunning = false;
    private Coroutine currentTimerCor;
    private bool isIndicatorDisable = false;
    private bool isMousepressed = false;
    [Range(0, 255)]
    [SerializeField] private byte indicatorAlpha = 50;

    [SerializeField] private bool indicatorDisableForTeleport = false;
    [SerializeField] private LayerMask ignoreLayer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // disable indicator for mobile
        if(Application.isMobilePlatform)
            gameObject.SetActive(false);
    }

    private void FixedUpdate(){
        
        if(isMousepressed) return;

        if(indicatorDisableForTeleport) return;
        // disable/enable hitIndidcator if mouse don't move
        
        if (currentMousePosition == Input.mousePosition) 
        {
            // start check 
            if (!IsTimerCoroutineRunning())
            {
                currentTimerCor =  StartCoroutine(TryDisableHitIndicator(disableTime));
            }
        }
        else
        {
            SetCurrentMoisePosition(Input.mousePosition);
            if (IsTimerCoroutineRunning())
            {
                StopCoroutine(currentTimerCor);
                SetStateCorIsRunning(false);
                if(!EventSystem.current.IsPointerOverGameObject())
                    StartCoroutine(SetIndicatorMaxColor(true));
            }
        }

        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        // make normal to hit and set hitIndidcator
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreLayer)) {
            hitIndicator.position = hit.point+ hit.normal/100; // set indicator over hit point; '/100' - use for correct distance from hit point to hitIndicator(bigger value -> less distance) 
            hitIndicator.LookAt( hit.point);
            
            // DEBUG DRAW RAYCAST
      
        //    Debug.DrawRay(hit.point, camera.transform.position, Color.green); // ray from camera
        //    Debug.DrawLine(vectorNormal, hit.point, Color.red); // normal ray from object
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
            isMousepressed = true;
            SetCurrentMoisePosition(Input.mousePosition);
            if (!isIndicatorDisable)
                StartCoroutine(SetIndicatorMaxColor(false));
        }
        else
            isMousepressed = false;
    }

    // disable hit indicator after timer
    private IEnumerator TryDisableHitIndicator(float _time)
    {
        SetStateCorIsRunning(true);
        yield return new WaitForSeconds(_time);
        StartCoroutine(SetIndicatorMaxColor(false));
        SetStateCorIsRunning(false);
    }
    
    // set state for timer coroutine
    private void SetStateCorIsRunning(bool _val)
    {
        timerCoroutineIsRunning = _val;
    }

    // get state for timer coroutine
    private bool IsTimerCoroutineRunning()
    {
        return timerCoroutineIsRunning;
    }

    // on/off indicator , true - enable indicator, false - disable
    IEnumerator SetIndicatorMaxColor(bool _val)
    {
        if (_val)
        {
            float alpha = 0;
            byte maxAlpha = indicatorAlpha;
            indicatorImg.color = new Color32(255, 255, 255, maxAlpha);
            yield return null;
            isIndicatorDisable = false;
        }
        else
        {
            // disable image
        
            byte minAlpha = 0;
            indicatorImg.color = new Color32(255, 255, 255, minAlpha);
            yield return null;
            isIndicatorDisable = true;
        }
    }

    // set current mouse position
    private void SetCurrentMoisePosition(Vector3 _position)
    {
        currentMousePosition = _position; 
    }

    public void DisableIndicatorForTeleport(bool _val)
    {
        indicatorDisableForTeleport = _val;
        if (_val && gameObject.activeSelf)
            StartCoroutine(SetIndicatorMaxColor(false)); // disable indicator
    }

    public void DisableCursor()
    {
        StartCoroutine(SetIndicatorMaxColor(false));
    }
}
