using System;
using Area_overview_webgl.Scripts.Controllers;
using UnityEngine;

namespace Area_overview_webgl.Scripts.PlayerScripts
{
    /**
     * Script provide rotation for player in First Camera mode
     */
    public class FirstPersonRotator : MonoBehaviour
    {
        [Header("Maximal rotation speed")] [SerializeField]
        private float maxRotationSpeed = 8f; //rotation speed

        [Header("Vertical rotation limit")]
        [SerializeField] private float yAxisTopLimit = 280f; //upper euler angle for the camera rotation
        [SerializeField] private float yAxisBottomLimit = 80f; //lower euler angle for the camera rotation
        
        [Header("Damp rotation")]
        [SerializeField] private float lerpSpeed = 10f;

        [Header("Rotation Sensitivity")] 
        [SerializeField] private float mobileSensitivity = .1f;
        [SerializeField] private float mouseSensitivity = .1f;

        private float currentRotationSpeedLerpValue;
        private float h;
        private float v;

        private PlayerBody playerBody;
        private GamePlatform currentGamePlatform;

        public void Init(PlayerBody playerBody, GamePlatform currentGamePlatform)
        {
            this.playerBody = playerBody;
            this.currentGamePlatform = currentGamePlatform;
        }

        private void FixedUpdate()
        {
            ApplyRotation();
        }

        private void LateUpdate()
        {
            CorrectRotationVerticalLimit();
        }

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
                default: throw new ArgumentException($"Check GamePlatform enum for this value {currentGamePlatform}");
            }

            currentVerticalRotationValue = CheckVerticalLimit(currentVerticalRotationValue);

            RotateVertical(currentVerticalRotationValue);

            RotateHorizontal(currentHorizontalRotationValue);
        }

        private float CheckVerticalLimit(float currentVerticalRotationValue)
        {
            var _newYAfterRotation = playerBody.GetHead().eulerAngles.x + currentVerticalRotationValue;
            if (_newYAfterRotation <= yAxisTopLimit && _newYAfterRotation > 180 ||
                _newYAfterRotation >= yAxisBottomLimit && _newYAfterRotation < 180)
                return 0;
            else
                return currentVerticalRotationValue;
        }

        private void RotateVertical(float currentVerticalRotationValue)
        {
            // rotate vertical
            Vector3 eulerAnglesVertical = playerBody.GetHead().eulerAngles;
            eulerAnglesVertical = new Vector3(eulerAnglesVertical.x + currentVerticalRotationValue,
                eulerAnglesVertical.y,
                eulerAnglesVertical.z);
            playerBody.GetHead().eulerAngles = eulerAnglesVertical;
        }

        private void RotateHorizontal(float currentHorizontalRotationValue)
        {
            // rotate horizontal
            Vector3 eulerAnglesHorizontal = playerBody.GetBody().transform.eulerAngles;
            eulerAnglesHorizontal = new Vector3(eulerAnglesHorizontal.x,
                eulerAnglesHorizontal.y + currentHorizontalRotationValue,
                eulerAnglesHorizontal.z);
            playerBody.GetBody().transform.eulerAngles = eulerAnglesHorizontal;
        }

        private void CorrectRotationVerticalLimit()
        {
            var cameraRotation = playerBody.GetHead().eulerAngles;
            // top
            if (cameraRotation.x <= yAxisTopLimit && cameraRotation.x > 180)
                playerBody.GetHead().eulerAngles = new Vector3(yAxisTopLimit, cameraRotation.y, cameraRotation.z);

            // bottom 
            if (cameraRotation.x >= yAxisBottomLimit && cameraRotation.x < 180)
                playerBody.GetHead().eulerAngles = new Vector3(yAxisBottomLimit, cameraRotation.y, cameraRotation.z);
        }

        public void ResetRotationSpeed()
        {
            currentRotationSpeedLerpValue = 0;
            h = 0;
            v = 0;
        }
    }
}