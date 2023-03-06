using System;
using System.Collections;
using Area_overview_webgl.Scripts.Controllers;
using Area_overview_webgl.Scripts.Interfaces;
using Area_overview_webgl.Scripts.UIScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Area_overview_webgl.Scripts.PlayerScripts
{
    public class Player : MonoBehaviour, IMove, IRotate
    {
        private GamePlatform currentGamePlatform;
        [SerializeField] private MovingInputController movingInputController;
        [SerializeField] private RotationInputController rotationInputController;
        [SerializeField] private ClickController clickController;
        
        [Header("PlayerBody")]
        [SerializeField] private PlayerBody playerBody;
        
        [Header("Player Moving")] [SerializeField]
        private float movingForceSpeed = 10f; // use for correct camera body speed 
        
        [SerializeField] private float boostSpeed = 5f; // speed when press shift
        private bool isBoost;
        
        [Header("Player Rotation")]
        [SerializeField] private float maxRotationSpeed = 8f; //rotation speed
        [SerializeField] private float yAxisTopLimit = 280f; //upper euler angle for the camera rotation
        [SerializeField] private float yAxisBottomLimit = 80f; //lower euler angle for the camera rotation 
       
        [SerializeField] private float lerpSpeed = 10f;

        
        [FormerlySerializedAs("touchSensitivity")]
        [Header("Rotation Sensitivity")]
        [SerializeField] private float mobileSensitivity = .1f;
        [SerializeField] private float mouseSensitivity = .1f;
        
        
        
        
        public void Init(GamePlatform currentGamePlatform, UIController uiController)
        {
            this.currentGamePlatform = currentGamePlatform;
            movingInputController.Init(this, currentGamePlatform, uiController);
            rotationInputController.Init(this, currentGamePlatform);
           
        }

        public PlayerBody GetPlayerBody()
        {
            return playerBody;
        }

        private void FixedUpdate()
        {
            ApplyRotation();
        }
        
        private void LateUpdate()
        {
            CorrectRotationVerticalLimit();
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

        private float currentRotationSpeedLerpValue;
        private float h;
        private float v;
        
        public void Rotate(float horizontalValue, float verticalValue)
        {
            currentRotationSpeedLerpValue = maxRotationSpeed;
            h = horizontalValue;
            v = verticalValue;
        }

        public void DampRotation()
        {
            currentRotationSpeedLerpValue = Mathf.Lerp(currentRotationSpeedLerpValue, 0f, Time.deltaTime * lerpSpeed);
        }

        void ApplyRotation()
        {
            float currentVerticalRotationValue;
            float currentHorizontalRotationValue;

            switch (currentGamePlatform)
            {
                case GamePlatform.PC: 
                    currentVerticalRotationValue = v * currentRotationSpeedLerpValue * mouseSensitivity;
                    currentHorizontalRotationValue = h * currentRotationSpeedLerpValue * mouseSensitivity;
                    break;
                case GamePlatform.mobile:
                    currentVerticalRotationValue = v * currentRotationSpeedLerpValue * mobileSensitivity;
                    currentHorizontalRotationValue = h * currentRotationSpeedLerpValue * mobileSensitivity;
                    break;
                default:  throw new ArgumentException($"Check GamePlatform enum for this value {currentGamePlatform}");
            }
            
            currentVerticalRotationValue = CheckVerticalLimit(currentVerticalRotationValue);

            RotateVertical(currentVerticalRotationValue);

            RotateHorizontal(currentHorizontalRotationValue);
        }

        private float CheckVerticalLimit(float currentVerticalRotationValue)
        {
            var _newYAfterRotation = GetPlayerBody().GetHead().eulerAngles.x + currentVerticalRotationValue;
            if (_newYAfterRotation <= yAxisTopLimit && _newYAfterRotation > 180 ||
                _newYAfterRotation >= yAxisBottomLimit && _newYAfterRotation < 180)
                return 0;
            else
                return currentVerticalRotationValue;
        }

        private void RotateVertical(float currentVerticalRotationValue)
        {
            // rotate vertical
            Vector3 eulerAnglesVertical = GetPlayerBody().GetHead().eulerAngles;
            eulerAnglesVertical = new Vector3(eulerAnglesVertical.x + currentVerticalRotationValue, 
                eulerAnglesVertical.y,
                eulerAnglesVertical.z);
            GetPlayerBody().GetHead().eulerAngles = eulerAnglesVertical;
        }
        private void RotateHorizontal(float currentHorizontalRotationValue)
        {
            // rotate horizontal
            Vector3 eulerAnglesHorizontal = GetPlayerBody().GetBody().transform.eulerAngles;
            eulerAnglesHorizontal = new Vector3(eulerAnglesHorizontal.x, 
                eulerAnglesHorizontal.y + currentHorizontalRotationValue,
                eulerAnglesHorizontal.z);
            GetPlayerBody().GetBody().transform.eulerAngles = eulerAnglesHorizontal;
        }
        private void CorrectRotationVerticalLimit()
        {
            var cameraRotation = GetPlayerBody().GetHead().eulerAngles;
            // top
            if (cameraRotation.x <= yAxisTopLimit && cameraRotation.x > 180)
                GetPlayerBody().GetHead().eulerAngles = new Vector3(yAxisTopLimit, cameraRotation.y, cameraRotation.z);

            // bottom 
            if (cameraRotation.x >= yAxisBottomLimit && cameraRotation.x < 180)
                GetPlayerBody().GetHead().eulerAngles = new Vector3(yAxisBottomLimit, cameraRotation.y, cameraRotation.z);
        }
        
        private void ResetRotationSpeed()
        {
            currentRotationSpeedLerpValue = 0;
            h = 0;
            v = 0;
        }

        #endregion

        
    }

    
}