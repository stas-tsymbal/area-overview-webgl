using System;
using Area_overview_webgl.Scripts.CameraModeScripts;
using UnityEngine;

namespace Area_overview_webgl.Scripts.PlayerScripts
{
    /**
     * Synchronize orbital and first person camera,
     * orbital and first person camera will be in the same direction of view
     */
    public class PlayerCameraModeSynchronizer : MonoBehaviour
    {
        private CameraMode currentCameraMode;

        [Header("Camera Moving")]
        [SerializeField] private Transform firstPersonTransform;
        [SerializeField] private Transform orbitalTransform; 
        
        private CameraModeController cameraModeController;
        private bool isNeedSync;

        private void OnCameraModeChangeSynchronization(CameraMode cameraMode)
        {
            currentCameraMode = cameraMode;
        }

        public void Init(CameraMode cameraMode, CameraModeController cameraModeController, Transform firstPersonTransform, Transform orbitalTransform)
        {
            currentCameraMode = cameraMode;
            this.cameraModeController = cameraModeController;

            this.firstPersonTransform = firstPersonTransform;
            this.orbitalTransform = orbitalTransform;
            
            cameraModeController.OnCameraModeChange += OnCameraModeChangeSynchronization;
            StartSynchronize();
          
        }

        private void StartSynchronize()
        {
            isNeedSync = true;
        }

        private void StopSynchronize()
        {
            isNeedSync = false;
        }

        private void FixedUpdate()
        {
            if(!isNeedSync) return;
            SynchronizeCameraYAxis();
        }

        private void SynchronizeCameraYAxis()
        {
            switch (currentCameraMode)
            {
                case CameraMode.orbital:
                    // copy angle for firs person from orbital
                    var eulerAnglesFirstPerson = firstPersonTransform.eulerAngles;
                    eulerAnglesFirstPerson = new Vector3(eulerAnglesFirstPerson.x, orbitalTransform.eulerAngles.y, eulerAnglesFirstPerson.z);
                    firstPersonTransform.eulerAngles = eulerAnglesFirstPerson;
                    break;
                case CameraMode.firstPerson: 
                    // copy angle for orbital from person from 
                    var eulerAnglesOrbital = orbitalTransform.eulerAngles;
                    eulerAnglesOrbital = new Vector3(eulerAnglesOrbital.x, firstPersonTransform.eulerAngles.y, eulerAnglesOrbital.z);
                    orbitalTransform.eulerAngles = eulerAnglesOrbital;
                    break;
                default:
                    throw new ArgumentException($"Check GameMode enum for this value {currentCameraMode}");
            }
        }
    }
}