using System;
using Area_overview_webgl.Scripts.CameraModeScripts;
using Area_overview_webgl.Scripts.Controllers;
using UnityEngine;

namespace Area_overview_webgl.Scripts.UIScripts
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private CameraModeIndicator cameraModeIndicator;
        [SerializeField] private MobileControlButton mobileMovingButtons;

        public void Init(GamePlatform currentGamePlatform, CameraMode startCameraMode, CameraModeController cameraModeController)
        {
            // init mode indicator
            cameraModeIndicator.Init(startCameraMode, cameraModeController);
            
            //init mobile input for moving
            if (currentGamePlatform == GamePlatform.mobile)
            {
                mobileMovingButtons.Show();
                mobileMovingButtons.Init();
                cameraModeController.OnCameraModeChange += OnCameraModeChange;

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

        private void OnCameraModeChange(CameraMode cameraMode)
        {
            switch (cameraMode)
            {
                case CameraMode.orbital: mobileMovingButtons.Hide();
                    break;
                case CameraMode.firstPerson: mobileMovingButtons.Show(); 
                    break;
                default:
                    throw new ArgumentException($"Check CameraMode enum for this value {cameraMode}");
            }
        }
    }
}