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
        
        [SerializeField] private FirstPersonRotator firstPersonRotator;
        [SerializeField] private OrbitRotator orbitRotator;
        
        [Header("PlayerBody")]
        [SerializeField] private PlayerBody playerBody;
        
        [Header("Player Moving")] [SerializeField]
        private float movingForceSpeed = 10f; // use for correct camera body speed 
        
        [SerializeField] private float boostSpeed = 5f; // speed when press shift
        private bool isBoost;

        private CameraModeScripts.CameraModeController cameraModeController;
        
      
        public void Init(GamePlatform currentGamePlatform, UIController uiController, CameraMode cameraMode, CameraModeScripts.CameraModeController cameraModeController)
        {
            this.currentGamePlatform = currentGamePlatform;
            this.cameraMode = cameraMode;
            this.cameraModeController = cameraModeController;
            
            movingInputController.Init(this, currentGamePlatform, uiController);
            rotationInputController.Init(this, currentGamePlatform, clickController);
            clickController.Init(this,this, currentGamePlatform);
            
            firstPersonRotator.Init(GetPlayerBody(), currentGamePlatform);
            orbitRotator.Init(currentGamePlatform);
            
            cameraModeController.OnCameraModeChange += OnCameraModeChange;
        }

        public void OnCameraModeChange(CameraMode cameraMode)
        {
            this.cameraMode = cameraMode;
            Debug.Log("Change camera mode + " + cameraMode);
        }

        private void OnDestroy()
        {
            cameraModeController.OnCameraModeChange -= OnCameraModeChange;
        }

        public PlayerBody GetPlayerBody()
        {
            return playerBody;
        }
        
        
        #region PlayerMoving
        public void MoveForward()
        {
            Debug.Log("Move forward");
            ApplyForceToTheBody( GetPlayerBody().GetHead().forward);
        }

        public void MoveBack()
        {
            Debug.Log("Move back");
            ApplyForceToTheBody(-GetPlayerBody().GetHead().forward);
        }

        public void MoveLeft()
        {
            Debug.Log("Move left");
            ApplyForceToTheBody(-GetPlayerBody().GetHead().right);
        }

        public void MoveRight()
        {
            Debug.Log("Move right");
            ApplyForceToTheBody(GetPlayerBody().GetHead().right);
        }

        public void BoostSpeed(bool val)
        {
            Debug.Log($"Boost {val}");
            isBoost = val;
        }
        
        private void ApplyForceToTheBody(Vector3 forceHeading)
        {
            var forceForMoving = GetForceForMoving(forceHeading);
            var finalForce = forceForMoving * movingForceSpeed;
           
            SetKinematicStateForRigidbody(false);
            GetPlayerBody().GetRigidbody().AddForce(finalForce);
            
            // LookAtRotatorController.Insctance.StopLookAtRotation();
            if(freezingRigidbodyCor != null) StopCoroutine(freezingRigidbodyCor);
            freezingRigidbodyCor = StartCoroutine(FreezingRigidbody()); // try freez RB  
        }

        private Coroutine freezingRigidbodyCor;
        private IEnumerator FreezingRigidbody()
        {
            yield return new WaitForSeconds(0.1f);
            SetKinematicStateForRigidbody(true);
        }

        private Vector3 GetForceForMoving(Vector3 forceHeading)
        {
            var movingForce = Time.fixedDeltaTime * forceHeading;
            return isBoost ? movingForce * boostSpeed : movingForce ;
        }
        
        private void SetKinematicStateForRigidbody(bool val)
        {
            GetPlayerBody().SetKinematicStateForRigidbody(val);
        }
        

        #endregion

        #region Rotation
        
        public void Rotate(float horizontalValue, float verticalValue)
        {
            lookAtController.StopLookAtRotation();
            switch (cameraMode)
            {
                case CameraMode.orbital:  orbitRotator.Rotate(horizontalValue, verticalValue);
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
            Debug.Log("Try make teleport in player");
            switch (cameraMode)
            {
                case CameraMode.orbital:  orbitRotator.ResetRotationSpeed();
                    break;
                case CameraMode.firstPerson: firstPersonRotator.ResetRotationSpeed();
                    break;
                default:
                    throw new ArgumentException($"Check GameMode enum for this value {cameraMode}");
            }
            lookAtController.StopLookAtRotation();
            teleportController.TryMakeTeleport(cursorPosition);
        }

        #endregion

        #region LookAtObject

        public void TryLookAtObject(Vector3 cursorPosition)
        {
            Debug.Log("Try make look at object");
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