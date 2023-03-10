using System;
using Area_overview_webgl.Scripts.Controllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace  Area_overview_webgl.Scripts.PlayerScripts
{
    public class OrbitRotator : MonoBehaviour
    {
        [Header("Maximum rotation speed")] [SerializeField]
        private float maxRotationSpeed = 8f; //rotation speed

        [Header("Lower angle for the camera rotation")] [SerializeField]
        private float yAxisBottomLimit = 80f; //lower angle for the camera rotation

        private float currentRotationSpeedLerpValue;

        [Header("Damping after user stop rotation")] [SerializeField]
        private float lerpSpeed = 10f;

        private float h;
        private float v;

        [Header("Sensitivity")] [SerializeField]
        private float mobileSensitivity = .1f;

        [SerializeField] private float mouseSensitivity = .1f;
        
        [Header("Invert mobile moving")] 
        [SerializeField] private bool m_invertX;
        [SerializeField] private bool m_invertY;

        [Header("Invert PC moving")] 
        [SerializeField] private bool pc_invertX;
        [SerializeField] private bool pc_invertY;
        
        [Header("Object for rotation")]
        [SerializeField] private Transform orbitalSphere; //link to the camera sphere
        [SerializeField] private Transform lastOrbitCameraPosition; //link to the camera sphere

        private GamePlatform currentGamePlatform;

        public void Init(GamePlatform currentGamePlatform, Transform centralPointForOrbitalRotator)
        {
            this.currentGamePlatform = currentGamePlatform;

            transform.position = centralPointForOrbitalRotator.position;
            transform.SetParent(null);
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
        
        public void ResetRotationSpeed()
        {
            currentRotationSpeedLerpValue = 0;
            h = 0;
            v = 0;
        }
        
        private void FixedUpdate()
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
            
            if (orbitalSphere.transform.eulerAngles.x + currentVerticalRotationValue <= 0.1f ||
                orbitalSphere.transform.eulerAngles.x + currentVerticalRotationValue >= 179.9f)
                currentVerticalRotationValue = 0;

            var eulerAngles = orbitalSphere.transform.eulerAngles;
            eulerAngles = new Vector3(eulerAngles.x + currentVerticalRotationValue, eulerAngles.y + currentHorizontalRotationValue, eulerAngles.z);
            orbitalSphere.transform.eulerAngles = eulerAngles;
        }

       
        private void LateUpdate()
        {
            CheckLoverCameraLimit();
        }

        // Check lower camera limit
        private void CheckLoverCameraLimit()
        {
            var _cameraRotation = orbitalSphere.transform.eulerAngles;
            if (_cameraRotation.x > yAxisBottomLimit)
                orbitalSphere.transform.eulerAngles = new Vector3(yAxisBottomLimit, _cameraRotation.y, _cameraRotation.z);
        }

        public Transform GetSphereRotator()
        {
            return orbitalSphere;
        }
        
        public Transform GetLastOrbitCameraPosition()
        {
            return lastOrbitCameraPosition;
        }
    }
}