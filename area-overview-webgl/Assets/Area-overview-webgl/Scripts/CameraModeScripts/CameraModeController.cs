using System;
using UnityEngine;
using Area_overview_webgl.Scripts.UIScripts;

namespace Area_overview_webgl.Scripts.CameraModeScripts
{
    public class CameraModeController : MonoBehaviour
    {
        [SerializeField] private CameraMode currentCameraMode;
       // [SerializeField] private FirstPersonRotator firstPersonRotator;
       // [SerializeField] private OrbitalRotator orbitalRotator;

        public Action<CameraMode> OnCameraModeChange;
        private UIController uiController;
        public void Init(CameraMode currentCameraMode, UIController uiController)
        {
            switch (currentCameraMode)
            {
                case CameraMode.firstPerson: SetFirstPersonMode();
                    break;
                case CameraMode.orbital: SetOrbitalMode();
                    break;
                default: throw new ArgumentException($"Check CameraMode enum for this value {currentCameraMode}");
            }

            this.uiController = uiController;
            Subscribe();
        }

        void Subscribe()
        {
            uiController.GetCameraModeIndicator().OnOrbitalModeClick += SetOrbitalMode;
            uiController.GetCameraModeIndicator().OnFirstPersonModeClick += SetFirstPersonMode;
        }

        void UnSubscribe()
        {
            uiController.GetCameraModeIndicator().OnOrbitalModeClick -= SetOrbitalMode;
            uiController.GetCameraModeIndicator().OnFirstPersonModeClick -= SetFirstPersonMode;
        }
        
        #region First Person Mode
        public void SetFirstPersonMode()
        {
            if( currentCameraMode == CameraMode.firstPerson) return;
            currentCameraMode = CameraMode.firstPerson;
            OnCameraModeChange?.Invoke(currentCameraMode);
        }
        
        #endregion

        #region Orbital Mode
        
        public void SetOrbitalMode()
        {
            if( currentCameraMode == CameraMode.orbital) return;
            currentCameraMode = CameraMode.orbital;
            OnCameraModeChange?.Invoke(currentCameraMode);
        }
        
        #endregion
        
    }
}