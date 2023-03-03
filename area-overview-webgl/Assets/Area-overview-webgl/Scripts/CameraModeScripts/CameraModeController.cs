using System;
using UnityEngine;

namespace Area_overview_webgl.Scripts.CameraModeScripts
{
    public class CameraModeController : MonoBehaviour
    {
        [SerializeField] private CameraMode currentCameraMode;
        [SerializeField] private FirstPersonRotator firstPersonRotator;
        [SerializeField] private OrbitalRotator orbitalRotator;

        public Action<CameraMode> OnCameraModeChange;

        public void Init(CameraMode currentCameraMode)
        {
            switch (currentCameraMode)
            {
                case CameraMode.firstPerson: SetFirstPersonMode();
                    break;
                case CameraMode.orbital: SetOrbitalMode();
                    break;
                default: throw new ArgumentException($"Check CameraMode enum for this value {currentCameraMode}");
            }
        }
        

        #region First Person Mode
        public void SetFirstPersonMode()
        {
            currentCameraMode = CameraMode.firstPerson;
            OnCameraModeChange?.Invoke(currentCameraMode);
        }
        
        #endregion

        #region Orbital Mode
        
        public void SetOrbitalMode()
        {
            currentCameraMode = CameraMode.orbital;
            OnCameraModeChange?.Invoke(currentCameraMode);
        }
        
        #endregion
        
        
        

        
    }
}