using System;
using System.Collections;
using Area_overview_webgl.Scripts.CameraModeScripts;
using Area_overview_webgl.Scripts.LookAtRotatorScripts;
using Area_overview_webgl.Scripts.ParallelAreaScripts;
using UnityEngine;

namespace Area_overview_webgl.Scripts.PlayerScripts
{
    public class PlayerCameraModeSynchronizer : MonoBehaviour
    {

        private CameraMode currentCameraMode;

        [Header("Camera Moving")]
        [SerializeField] private Transform firstPersonTransform;
        [SerializeField] private Transform orbitalTransform; 
        
        
        private CameraModeController cameraModeController;
        private bool isNeedSync;
        public void OnCameraModeChangeSynchronization(CameraMode cameraMode)
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
                    var eulerAnglesFirstPerson = firstPersonTransform.eulerAngles;
                    eulerAnglesFirstPerson = new Vector3(eulerAnglesFirstPerson.x, orbitalTransform.eulerAngles.y, eulerAnglesFirstPerson.z);
                    firstPersonTransform.eulerAngles = eulerAnglesFirstPerson;
                    break;
                case CameraMode.firstPerson: 
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