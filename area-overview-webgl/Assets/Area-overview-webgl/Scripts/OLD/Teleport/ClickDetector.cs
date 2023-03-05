using System;
using System.Collections;
using System.Collections.Generic;
using Area_overview_webgl.Scripts.TeleportScripts;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Script detect touch and write touch ID and type(over UI or not)
 * Can check mouse over UI in void IsMouseOverUI()
 */
public class ClickDetector : MonoBehaviour
{
    public static ClickDetector Instance;
    
    [Header("Touch status")]
    [SerializeField] private TouchSample uiTouch;
    [SerializeField] private TouchSample rotationTouch;
    [SerializeField] private bool isMousePressOverUI = false;
    [Serializable]
    public class TouchSample
    {
        public int touchID = -1;
        public bool isPressed;

        public void SetTouchState(int _id, bool _state)
        {
            touchID = _id;
            isPressed = _state;
        }
    }
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Application.isMobilePlatform)
            CheckTouch(); // detect touch -> UI or rotation
    }

    // return mouse status 
    public bool IsMouseOverUI()
    {
        return isMousePressOverUI = EventSystem.current.IsPointerOverGameObject();
    }
    
    // detect current touch -> UI or rotation 
    private void CheckTouch()
    {
        foreach (Touch touch in Input.touches)
        {
            int id = touch.fingerId;
            if (touch.phase == TouchPhase.Began) // start touch screen
            {
                // remember touch UI
                if (!uiTouch.isPressed && EventSystem.current.IsPointerOverGameObject(id))
                {
                    uiTouch.SetTouchState(id, true);
                }

                // remember touch rotation
                if (!rotationTouch.isPressed && !EventSystem.current.IsPointerOverGameObject(id))
                {
                    rotationTouch.SetTouchState(id, true);
                   // TeleportController.Instance.RememberCurrentCameraAngle(); // remember angle for teleport detector
                    
                }
            }
            
            if (touch.phase == TouchPhase.Ended) // remove finger
            {
                // end touch UI
                if (uiTouch.isPressed && uiTouch.touchID == id)
                {
                    uiTouch.SetTouchState(-1, false);
                }

                // end touch rotation
                if (rotationTouch.isPressed && rotationTouch.touchID == id)
                {
                    rotationTouch.SetTouchState(-1, false);
                    TeleportController.Instance.TryMakeTeleport(touch.position); // try to make teleport
                }
            }
        }
    }

    // get ui touch
    public TouchSample GetUiTouch()
    {
        return uiTouch;
    }
    
    // get rotation touch
    public TouchSample GetRotationTouch()
    {
        return rotationTouch;
    }
}
