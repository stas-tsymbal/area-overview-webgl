using System;
using Area_overview_webgl.Scripts.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Area_overview_webgl.Scripts.OrbitRotation
{
    public class OrbitRotation : MonoBehaviour
    {
        [Header("Maximum rotation speed")] [SerializeField]
        private float maxRotationSpeed = 8f; //rotation speed

        [Header("Lower angle for the camera rotation")] [SerializeField]
        private float yAxisBottomLimit = 80f; //lower angle for the camera rotation

        private float currentRotationSpeed;

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

        private Transform orbitSphere; //link to the camera sphere

        private void Awake()
        {
            orbitSphere = gameObject.transform;
        }

        private void OnEnable()
        {
            currentRotationSpeed = 0;
            h = 0;
            v = 0;
        }

        private void Update()
        {
            // ( (pick up LMB || mouse under ui)) ||
            //  ((touch count == 0)  || (don't move on mobile) )
            if ((!Application.isMobilePlatform &&
                 (!Input.GetMouseButton(0) || ClickDetector.Instance.IsMouseOverUI())) ||
                ((Application.isMobilePlatform && Input.touchCount == 0) ||
                 (Application.isMobilePlatform && Input.GetTouch(0).phase != TouchPhase.Moved)))
            {
                //added to smoothly lerp camera rotation speed after lmb unpick
                currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0f, Time.deltaTime * lerpSpeed);
            }


            if ((Input.GetMouseButton(0) && !ClickDetector.Instance.IsMouseOverUI()) ||
                (Input.touchCount > 0 && !ClickDetector.Instance.GetUiTouch().isPressed))
            {
                if (!Application.isMobilePlatform) // for PC
                    currentRotationSpeed = maxRotationSpeed;

                if (Input.touchCount > 0)
                {
                    if (Input.GetTouch(ClickDetector.Instance.GetRotationTouch().touchID).phase == TouchPhase.Began)
                    {
                        h = 0;
                        v = 0;
                    }

                    if (Input.GetTouch(ClickDetector.Instance.GetRotationTouch().touchID).phase == TouchPhase.Moved)
                    {
                        currentRotationSpeed = maxRotationSpeed;

                        var _invertX = 1;
                        if (m_invertX) _invertX = -1;
                        var _invertY = 1;
                        if (m_invertY) _invertY = -1;

                        var _xDelta = Input.GetTouch(0).deltaPosition.x;
                        var _yDelta = Input.GetTouch(0).deltaPosition.y;
                        h = _xDelta * touchSensitivity * _invertX;
                        v = _yDelta * touchSensitivity * _invertY;
                    }
                }
                else
                {
                    var _invertX = -1;
                    if (pc_invertX) _invertX = 1;
                    var _invertY = -1;
                    if (pc_invertY) _invertY = 1;
                    h = Input.GetAxis("Mouse X") * _invertX;
                    v = Input.GetAxis("Mouse Y") * _invertY;
                }
            }
        }

        private void FixedUpdate()
        {
            var _curV = v * currentRotationSpeed * mouseSensitivity;
            var _curH = h * currentRotationSpeed * mouseSensitivity;

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