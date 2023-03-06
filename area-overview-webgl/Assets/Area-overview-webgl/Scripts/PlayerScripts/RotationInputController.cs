using Area_overview_webgl.Scripts.Controllers;
using Area_overview_webgl.Scripts.Interfaces;
using UnityEngine;

namespace Area_overview_webgl.Scripts.PlayerScripts
{
    public class RotationInputController : MonoBehaviour
    {
        IRotate player;
        private GamePlatform currentGamePlatform;
        
        [Header("Invert mobile rotation")] 
        [SerializeField] private bool invertX;
        [SerializeField] private bool invertY;
        public void Init(IRotate player, GamePlatform currentGamePlatform)
        {
            this.player = player;
            this.currentGamePlatform = currentGamePlatform;
            
        }
       
        private void Update()
        {
            switch (currentGamePlatform)
            {
                case GamePlatform.PC: DetectRotationPC();
                    break;
                case GamePlatform.mobile: DetectRotationMobile();;
                    break;
            }
            
        }

        #region Mobile
        private void DetectRotationMobile()
        {
            if (!ClickDetector.Instance.GetRotationTouch().isPressed)
            {
                player.DampRotation();
            }
            
            // block rotation if UI is open
            if (UiController.Instance.SomeOverlayUiIsActive())
            {
                player.DampRotation();
                return;
            }

            if ((Input.touchCount > 0))
            {
                // if (!ClickDetector.Instance.GetUiTouch().isPressed)
                        //ResetCameraMoving();

                    if (ClickDetector.Instance.GetRotationTouch().isPressed &&
                        Input.GetTouch(ClickDetector.Instance.GetRotationTouch().touchID).phase == TouchPhase.Moved)
                    {
                        //  if(Input.GetTouch(0).deltaPosition.magnitude > startMoveTrashold) return;  //added to prevent start camera flip
                        
                        // invert control
                        int _invertX = -1;
                        if (invertX) _invertX = 1;

                        int _invertY = 1;
                        if (invertY) _invertY = -1;

                        float _xDelta = Input.GetTouch(ClickDetector.Instance.GetRotationTouch().touchID).deltaPosition.x;
                        float _yDelta = Input.GetTouch(ClickDetector.Instance.GetRotationTouch().touchID).deltaPosition.y;
                       var horizontal = _xDelta * _invertX;
                       var vertical = _yDelta * _invertY;
                        player.Rotate(horizontal,vertical);
                    }
            }
        }
        

        #endregion

        #region PC
        private void DetectRotationPC()
        {
            if (!Input.GetMouseButton(0))
            {
                //added to smoothly lerp camera rotation speed after lmb unpick
                player.DampRotation();
            }
            
            // block rotation if UI is open
            if (UiController.Instance.SomeOverlayUiIsActive())
            {
                player.DampRotation();
                return;
            }

            if ((Input.GetMouseButton(0) && !ClickDetector.Instance.IsMouseOverUI()))
            {
                //   LookAtRotatorController.Insctance.StopLookAtRotation();
                var horizontal = -Input.GetAxis("Mouse X");
                var vertical = Input.GetAxis("Mouse Y");
                 player.Rotate(horizontal,vertical);
            }
        }
        
        #endregion
        
    }
}