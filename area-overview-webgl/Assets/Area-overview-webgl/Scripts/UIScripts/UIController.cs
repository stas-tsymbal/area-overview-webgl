using Area_overview_webgl.Scripts.CameraModeScripts;
using Area_overview_webgl.Scripts.Controllers;
using UnityEngine;

namespace Area_overview_webgl.Scripts.UIScripts
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private CameraModeIndicator cameraModeIndicator;
        [SerializeField] private MobileControlButton mobileMovingButtons;

        public void Init(GamePlatform currentGamePlatform, CameraMode startCameraMode)
        {
            // init mode indicator
            cameraModeIndicator.Init(startCameraMode);
            
            //init mobile input for moving
            if (currentGamePlatform == GamePlatform.mobile)
            {
                mobileMovingButtons.Show();
                mobileMovingButtons.Init();
            }
            else
                mobileMovingButtons.Hide();
        }

        public CameraModeIndicator GetCameraModeIndicator()
        {
            return cameraModeIndicator;
        }
        
        public MobileControlButton GetMobileMovingButtons()
        {
            return mobileMovingButtons;
        }
    }
}