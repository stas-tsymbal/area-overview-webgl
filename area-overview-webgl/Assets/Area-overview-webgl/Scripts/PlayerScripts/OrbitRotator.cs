using Area_overview_webgl.Scripts.Controllers;
using UnityEngine;

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
        private float touchSensitivity = .1f;

        [SerializeField] private float mouseSensitivity = .1f;


        [Header("Invert mobile moving")] [SerializeField]
        private bool m_invertX;

        [SerializeField] private bool m_invertY;

        [Header("Invert PC moving")] [SerializeField]
        private bool pc_invertX;

        [SerializeField] private bool pc_invertY;

        [SerializeField] private Transform orbitSphere; //link to the camera sphere

        private GamePlatform currentGamePlatform;
        private ClickController clickController;
        
        public void Init(GamePlatform currentGamePlatform)
        {
            this.currentGamePlatform = currentGamePlatform;
        }

        private void OnEnable()
        {
            currentRotationSpeedLerpValue = 0;
            h = 0;
            v = 0;
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
            var _curV = v * currentRotationSpeedLerpValue * mouseSensitivity;
            var _curH = h * currentRotationSpeedLerpValue * mouseSensitivity;

            if (orbitSphere.transform.eulerAngles.z + _curV <= 0.1f ||
                orbitSphere.transform.eulerAngles.z + _curV >= 179.9f)
                _curV = 0;

            var eulerAngles = orbitSphere.transform.eulerAngles;
            eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y + _curH, eulerAngles.z + _curV);
            orbitSphere.transform.eulerAngles = eulerAngles;
        }

        //check for the lower camera limit
        private void LateUpdate()
        {
            var _cameraRotation = orbitSphere.transform.eulerAngles;

            if (_cameraRotation.z > yAxisBottomLimit)
                orbitSphere.transform.eulerAngles = new Vector3(_cameraRotation.x, _cameraRotation.y, yAxisBottomLimit);
        }
    }
}