using Area_overview_webgl.Scripts.Controllers;
using Area_overview_webgl.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Area_overview_webgl.Scripts.PlayerScripts
{
    public class RotationInputController : MonoBehaviour
    {
        IRotatable player;
        private GamePlatform currentGamePlatform;
        private ClickController clickController;
        
        [Header("Invert mobile rotation")] 
        [SerializeField] private bool invertX;
        [SerializeField] private bool invertY;
        public void Init(IRotatable player, GamePlatform currentGamePlatform, ClickController clickController)
        {
            this.player = player;
            this.currentGamePlatform = currentGamePlatform;
            this.clickController = clickController;
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
            if (!clickController.GetRotationTouch().isPressed)
            {
                player.DampRotation();
            }
            
            // block rotation if UI is open
          /*  if (UiController.Instance.SomeOverlayUiIsActive())
            {
                player.DampRotation();
                return;
            }*/

            if ((Input.touchCount > 0))
            {
                if(Input.GetTouch(clickController.GetRotationTouch().touchID).phase == TouchPhase.Began)
                {
                    player.Rotate(0,0);
                }
                
                if (clickController.GetRotationTouch().isPressed &&
                    Input.GetTouch(clickController.GetRotationTouch().touchID).phase == TouchPhase.Moved)
                {
                    // invert control
                    int _invertX = -1;
                    if (invertX) _invertX = 1;

                    int _invertY = 1;
                    if (invertY) _invertY = -1;

                    float _xDelta = Input.GetTouch(clickController.GetRotationTouch().touchID).deltaPosition.x;
                    float _yDelta = Input.GetTouch(clickController.GetRotationTouch().touchID).deltaPosition.y;
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
           /* if (UiController.Instance.SomeOverlayUiIsActive())
            {
                player.DampRotation();
                return;
            }*/

            if ((Input.GetMouseButton(0) && !clickController.IsMouseOverUI()))
            {
                var horizontal = -Input.GetAxis("Mouse X");
                var vertical = Input.GetAxis("Mouse Y");
                player.Rotate(horizontal,vertical);
            }
        }

        
        
        #endregion
        
    }
}