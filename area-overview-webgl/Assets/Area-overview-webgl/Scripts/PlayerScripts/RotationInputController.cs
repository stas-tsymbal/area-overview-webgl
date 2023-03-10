using Area_overview_webgl.Scripts.Controllers;
using Area_overview_webgl.Scripts.Interfaces;
using UnityEngine;

namespace Area_overview_webgl.Scripts.PlayerScripts
{
    /**
     * Script detect rotation input and call rotate for player 
     */
    public class RotationInputController : MonoBehaviour
    {
        IRotatable player;
        private GamePlatform currentGamePlatform;
        private ClickController clickController;

        private float canvasScaleFactor;
        
        public void Init(IRotatable player, GamePlatform currentGamePlatform, ClickController clickController, float canvasScaleFactor)
        {
            this.player = player;
            this.currentGamePlatform = currentGamePlatform;
            this.clickController = clickController;
            this.canvasScaleFactor = canvasScaleFactor;
        }

        private void Update()
        {
            switch (currentGamePlatform)
            {
                case GamePlatform.PC:
                    DetectRotationPC();
                    break;
                case GamePlatform.mobile:
                    DetectRotationMobile();
                    ;
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
            
            if ((Input.touchCount > 0))
            {
                if (Input.GetTouch(clickController.GetRotationTouch().touchID).phase == TouchPhase.Began)
                {
                    player.Rotate(0, 0);
                }

                if (clickController.GetRotationTouch().isPressed &&
                    Input.GetTouch(clickController.GetRotationTouch().touchID).phase == TouchPhase.Moved)
                {
                    float _xDelta = Input.GetTouch(clickController.GetRotationTouch().touchID).deltaPosition.x / canvasScaleFactor;
                    float _yDelta = Input.GetTouch(clickController.GetRotationTouch().touchID).deltaPosition.y / canvasScaleFactor;
                    var horizontal = _xDelta;
                    var vertical = _yDelta;
                    player.Rotate(horizontal, vertical);
                }
            }
        }

        #endregion

        #region PC

        private void DetectRotationPC()
        {
            if (!Input.GetMouseButton(0))
            {
                player.DampRotation();
            }

            if ((Input.GetMouseButton(0) && !clickController.IsMouseOverUI()))
            {
                var horizontal = -Input.GetAxis("Mouse X") / canvasScaleFactor;
                var vertical = Input.GetAxis("Mouse Y") / canvasScaleFactor;
                player.Rotate(horizontal, vertical);
            }
        }

        #endregion
    }
}