using System;
using System.Collections;
using Area_overview_webgl.Scripts.CameraModeScripts;
using Area_overview_webgl.Scripts.Controllers;
using Area_overview_webgl.Scripts.Interfaces;
using Area_overview_webgl.Scripts.LookAtRotatorScripts;
using Area_overview_webgl.Scripts.ParallelAreaScripts;
using Area_overview_webgl.Scripts.TeleportScripts;
using Area_overview_webgl.Scripts.UIScripts;
using UnityEngine;

namespace Area_overview_webgl.Scripts.PlayerScripts
{
    public class Player : MonoBehaviour, IMove, IRotatable, ITeleportable, ILookAtRotatable
    {
        private GamePlatform currentGamePlatform;
        private CameraMode cameraMode;
        [Header("Input")]
        [SerializeField] private MovingInputController movingInputController;
        [SerializeField] private RotationInputController rotationInputController;
        [SerializeField] private ClickController clickController;

        
        [Header("Mechanicks controllers")] 
        [SerializeField] private LookAtRotatorController lookAtController;
        [SerializeField] private TeleportController teleportController;
        [SerializeField] private ParallelAreaIndicatorMainController parallelAreaController;
        [Header("Camera mode rotators")]
        [SerializeField] private FirstPersonRotator firstPersonRotator;
        [SerializeField] private OrbitRotator orbitRotator;

        [Header("Camera mode switcher")]
        [SerializeField] private PlayerCameraModeSwitcher cameraModeSwitcher;
        [Header("Camera mode synchronizer")]
        [SerializeField] private PlayerCameraModeSynchronizer cameraModeSynchronizer;
        
        [Header("PlayerBody")]
        [SerializeField] private PlayerBody playerBody;
        
        [Header("Player Moving")] 
        [SerializeField] private float movingForceSpeed = 10f;
        [SerializeField] private float boostSpeed = 5f;
        private bool isBoost;

        private CameraModeController cameraModeController;
        
        public void Init(GamePlatform currentGamePlatform, UIController uiController, CameraMode cameraMode, 
            CameraModeController cameraModeController, Camera playerCamera, Transform centralPointForOrbitalRotator, float canvasScaleFactor)
        {
            // set start setting for camera mode, platform, 
            this.currentGamePlatform = currentGamePlatform;
            this.cameraMode = cameraMode;
            this.cameraModeController = cameraModeController;
            
            cameraModeSwitcher.Init(GetPlayerBody().GetCapsuleCollider(), playerCamera.transform, orbitRotator.GetLastOrbitCameraPosition(),
                orbitRotator.GetSphereRotator(),GetPlayerBody().GetFirstCamPosition(),GetPlayerBody().GetCameraYPosition() );
            
            // init look at logic
            lookAtController.Init(playerCamera,GetPlayerBody().GetHead(), GetPlayerBody().GetBody()); 
            
            // init teleportation logic
            teleportController.Init(GetPlayerBody().GetHead(), GetPlayerBody().GetCapsuleCollider(), playerCamera);
            
            // init input controllers
            movingInputController.Init(this, currentGamePlatform, uiController);
            rotationInputController.Init(this, currentGamePlatform, clickController, canvasScaleFactor);
            clickController.Init(currentGamePlatform);
            clickController.OnClick += OnInputClick;
            
            // init parallel area indicator
            parallelAreaController.Init( currentGamePlatform, playerCamera, uiController);
            
            // init rotation (first person and orbital)
            firstPersonRotator.Init(GetPlayerBody(), currentGamePlatform);
            orbitRotator.Init(currentGamePlatform, centralPointForOrbitalRotator);
            
            // detect camera mode changing 
            cameraModeController.OnCameraModeChange += OnCameraModeChange;
            
            // init camera mode synchronizer
            cameraModeSynchronizer.Init(cameraMode, cameraModeController, transform, orbitRotator.transform);
            
        }
        
        private PlayerBody GetPlayerBody()
        {
            return playerBody;
        }

        #region Camera Mode changing
        private void OnCameraModeChange(CameraMode cameraMode)
        {
            //if(this.cameraMode == cameraMode) return;
            
            this.cameraMode = cameraMode;
            lookAtController.StopLookAtRotation();
        
            orbitRotator.ResetRotationSpeed();
            firstPersonRotator.ResetRotationSpeed();
            
            if (currentGamePlatform == GamePlatform.PC) parallelAreaController.HideIndicator();

            cameraModeSwitcher.OnCameraModeChanged += OnCameraModeFinalChanged;
            
            switch (cameraMode)
            {
                case CameraMode.orbital:  cameraModeSwitcher.SetOrbitalMode();
                    break;
                case CameraMode.firstPerson: cameraModeSwitcher.SetFirstPersonMode();
                    break;
                default:
                    throw new ArgumentException($"Check GameMode enum for this value {cameraMode}");
            }
            //Debug.Log("Change camera mode + " + cameraMode);
        }

        // Void will call after all action that call in 
        private void OnCameraModeFinalChanged()
        {
            cameraModeSwitcher.OnCameraModeChanged -= OnCameraModeFinalChanged;
            if (currentGamePlatform == GamePlatform.PC) parallelAreaController.ShowIndicator();
        }

        private void OnDestroy()
        {
            cameraModeController.OnCameraModeChange -= OnCameraModeChange;
        }
        
        #endregion
        

        #region Click from input

        private void OnInputClick(Vector3 inputPosition)
        {
            switch (cameraMode)
            {
                case CameraMode.orbital: TryMakeTeleport(Input.mousePosition);
                    break;
                case CameraMode.firstPerson: 
                    TryMakeTeleport(Input.mousePosition);
                    TryLookAtObject(Input.mousePosition);
                    break;
                default:
                    throw new ArgumentException($"Check GameMode enum for this value {cameraMode}");
            }
            
        }
        
        #endregion
        
        #region PlayerMoving
        public void MoveForward()
        {
            ApplyForceToTheBody( GetPlayerBody().GetBody().forward);
        }

        public void MoveBack()
        {
            ApplyForceToTheBody(-GetPlayerBody().GetBody().forward);
        }

        public void MoveLeft()
        {
            ApplyForceToTheBody(-GetPlayerBody().GetBody().right);
        }

        public void MoveRight()
        {
            ApplyForceToTheBody(GetPlayerBody().GetBody().right);
        }

        public void BoostSpeed(bool val)
        {
            isBoost = val;
        }
        
        private void ApplyForceToTheBody(Vector3 forceHeading)
        {
            var forceForMoving = GetForceForMoving(forceHeading);
            var finalForce = forceForMoving * movingForceSpeed;
            
            GetPlayerBody().GetRigidbody().AddForce(finalForce);
        }
        
        private Vector3 GetForceForMoving(Vector3 forceHeading)
        {
            var movingForce =  forceHeading;
            return isBoost ? movingForce * boostSpeed : movingForce ;
        }
        
        #endregion

        #region Rotation
        
        public void Rotate(float horizontalValue, float verticalValue)
        {
            lookAtController.StopLookAtRotation();
            switch (cameraMode)
            {
                case CameraMode.orbital: orbitRotator.Rotate(horizontalValue, verticalValue);
                    break;
                case CameraMode.firstPerson:
                    firstPersonRotator.Rotate(horizontalValue, verticalValue);
                    break;
                default:
                    throw new ArgumentException($"Check GameMode enum for this value {cameraMode}");
            }
            
        }

        public void DampRotation()
        {
            switch (cameraMode)
            {
                case CameraMode.orbital:  orbitRotator.DampRotation();
                    break;
                case CameraMode.firstPerson: firstPersonRotator.DampRotation();
                    break;
                default:
                    throw new ArgumentException($"Check GameMode enum for this value {cameraMode}");
            }
        }

        #endregion


        #region Teleport

        public void TryMakeTeleport(Vector3 cursorPosition)
        {
            switch (cameraMode)
            {
                case CameraMode.orbital: orbitRotator.ResetRotationSpeed();
                    break;
                case CameraMode.firstPerson: firstPersonRotator.ResetRotationSpeed();
                    break;
                default:
                    throw new ArgumentException($"Check GameMode enum for this value {cameraMode}");
            }
            lookAtController.StopLookAtRotation();

            if (teleportController.CanMakeTeleport(cursorPosition))
            {
                cameraModeController.SetFirstPersonMode();
                teleportController.MakeTeleport();
            }
        }

        #endregion

        #region LookAtObject

        public void TryLookAtObject(Vector3 cursorPosition)
        {
            switch (cameraMode)
            {
                case CameraMode.orbital:  orbitRotator.ResetRotationSpeed();
                    break;
                case CameraMode.firstPerson: firstPersonRotator.ResetRotationSpeed();
                    break;
                default:
                    throw new ArgumentException($"Check GameMode enum for this value {cameraMode}");
            }
            lookAtController.TryRotateToObject(cursorPosition);
        }

        #endregion

        
    }

    
}