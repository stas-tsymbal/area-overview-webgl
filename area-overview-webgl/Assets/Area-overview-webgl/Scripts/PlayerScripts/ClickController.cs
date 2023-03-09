using System;
using Area_overview_webgl.Scripts.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Area_overview_webgl.Scripts.PlayerScripts
{
    /**
     * Script detect mouse/mobile without moving pointer click and send event OnClick
     */
    public class ClickController : MonoBehaviour
    {
        [Header("Mobile touch status")]
        [SerializeField] private TouchSample uiTouch;
        [SerializeField] private TouchSample rotationTouch;
        
        private bool mouseHold;
        private Vector3 mousePosition;
        private Vector2 touchPosition;
        private GamePlatform currentGamePlatform;
        
        public Action<Vector3> OnClick;
        
        public void Init(GamePlatform currentGamePlatform)
        {
            this.currentGamePlatform = currentGamePlatform;
        }

        private void Update()
        {
            switch (currentGamePlatform)
            {
                case GamePlatform.mobile:
                    DetectClickMobile();
                    break;
                case GamePlatform.PC:
                    DetectClickPC();
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
                            // touch was moved and then released
                        }
                        else
                        {
                            // click
                            OnClick?.Invoke(touch.position);
                            
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
            if (Input.GetMouseButtonDown(0))
            {
                mouseHold = false;
                mousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                mouseHold = true;
            }

            // I separate mouse moving and single click
            if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (mouseHold && Vector3.SqrMagnitude(mousePosition - Input.mousePosition) > 0)
                {
                    // left mouse button was moved and then released
                }
                else
                {
                    // click
                    OnClick?.Invoke(Input.mousePosition);
                }

                mouseHold = false;
            }
        }

        public bool IsMouseOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
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