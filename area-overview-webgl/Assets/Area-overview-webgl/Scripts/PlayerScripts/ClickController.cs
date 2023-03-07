using System;
using Area_overview_webgl.Scripts.Controllers;
using Area_overview_webgl.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Area_overview_webgl.Scripts.PlayerScripts
{
    public class ClickController : MonoBehaviour
    {
        [Header("Mobile touch status")] 
        [SerializeField] private TouchSample uiTouch;
        [SerializeField] private TouchSample rotationTouch;
        
        
        public bool mouseHeld = false;
        private Vector3 mousePosition;
        private Vector2 touchPosition;
        private GamePlatform currentGamePlatform;
        
        ITeleportable playerTeleport;
        ILookAtRotatable playerLookAt;
        public void Init(ITeleportable playerTeleport, ILookAtRotatable playerLookAt, GamePlatform currentGamePlatform)
        {
           this.playerTeleport = playerTeleport;
           this.playerLookAt = playerLookAt;
           this.currentGamePlatform = currentGamePlatform;

        }
        
        private void Update()
        {
            switch (currentGamePlatform)
            {
                case GamePlatform.mobile:
                    DetectClickMobile();
                    break;
                case GamePlatform.PC: DetectClickPC();
                    break;
                default:
                    throw new ArgumentException($"Check GamePlatform enum for this value {currentGamePlatform}");
            }
            
        }

        #region Mobile

        private void DetectClickMobile()
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
                        touchPosition = touch.position;
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
                        if (Vector3.SqrMagnitude(touchPosition - touch.position) > 0)
                        {
                            // 
                        } else  {
                            Debug.Log("it is teleport or rotate click");
                            playerTeleport.TryMakeTeleport(touch.position);
                            playerLookAt.TryLookAtObject(touch.position);
                            //TeleportController.Instance.TryMakeTeleport(touch.position); // try to make teleport
                        }
                       
                    }
                }
            }
        }

        public TouchSample GetUiTouch()
        {
            return uiTouch;
        }

        // get rotation touch
        public TouchSample GetRotationTouch()
        {
            return rotationTouch;
        }
        
        #endregion

        #region PC

        private void DetectClickPC()
        {
            if (Input.GetMouseButtonDown(0)) {
                mouseHeld = false;
                mousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0)) {
                mouseHeld = true;
            }

            // I separate mouse moving and single click
            if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()) {
                if (mouseHeld && Vector3.SqrMagnitude(mousePosition - Input.mousePosition) > 0)
                {
                    // left mouse button is released after being held and moved
                } else  {
                    Debug.Log("it is teleport or rotate click");
                    playerTeleport.TryMakeTeleport(Input.mousePosition);
                    playerLookAt.TryLookAtObject(Input.mousePosition);
                }
                mouseHeld = false;
            }
        }

        #endregion
    }
    
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
}