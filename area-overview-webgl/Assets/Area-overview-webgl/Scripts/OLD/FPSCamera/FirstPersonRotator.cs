using System.Collections;
using Area_overview_webgl.Scripts.LookAtRotatorScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Area_overview_webgl.Scripts.FPSCamera
{
    public class FirstPersonRotator : MonoBehaviour
    {
        public static FirstPersonRotator Instance;
        
        private GameObject player; // main camera,  use for rotate vertical
        private Rigidbody cameraBody; // camera body,  use for rotate horizontal and moving
        
        [Header("Rotation speed")]
        [SerializeField] private float maxRotationSpeed = 8f; //rotation speed

        [Header("Vertical limit and damping")] [SerializeField]
        private float yAxisTopLimit = 280f; //upper euler angle for the camera rotation

        [SerializeField] private float yAxisBottomLimit = 80f; //lower euler angle for the camera rotation 

        private float currentRotationSpeed;
        [SerializeField] private float lerpSpeed = 10f;

        private float h;
        private float v;

        [Header("Camera Sensitivity")]
        [SerializeField] private float touchSensitivity = .1f;
        [SerializeField] private float mouseSensitivity = .1f;

        [Header("Invert mobile moving")] 
        [SerializeField] private bool invertX;
        [SerializeField] private bool invertY;

        [Header("Camera Moving")] [SerializeField]
        private float movingForceSpeed = 10f; // use for correct camera body speed 

        private bool isMoveUp;
        private bool isMoveDown;
        [SerializeField] private float cameraRotationFromKeySpeed = 10f; // correct rotation speed for key rotation QE
        [SerializeField] private float shiftBoostSpeed = 5f; // speed when press shift

        [Header("Mobile control btn")] [SerializeField]
        private GameObject controlBtn;

        [Header("Copy Y for orbital camera")] [SerializeField]
        private Transform orbitalCamera;
        

        private void Awake()
        {
            Instance = this;
            player = Camera.main.gameObject;
            cameraBody = gameObject.GetComponentInChildren<Rigidbody>();
        }

        private void OnEnable()
        {
            currentRotationSpeed = 0;
            h = 0;
            v = 0;
        }

        private void Update()
        {
            if ((!Application.isMobilePlatform && !Input.GetMouseButton(0)) || (Application.isMobilePlatform &&
                    !ClickDetector.Instance.GetRotationTouch().isPressed))
            {
                //added to smoothly lerp camera rotation speed after lmb unpick
                currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0f, Time.deltaTime * lerpSpeed);
            }

            if (!ClickDetector.Instance.GetUiTouch().isPressed) // reset moving if don't touch ui 
                ResetCameraMoving();

            // block rotation if UI is open
            if (UiController.Instance.SomeOverlayUiIsActive())
            {
                currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0f, Time.deltaTime * lerpSpeed);
                return;
            }

            if ((Input.GetMouseButton(0) && !ClickDetector.Instance.IsMouseOverUI()) || (Input.touchCount > 0))
            {
                if (!Application.isMobilePlatform)
                    currentRotationSpeed = maxRotationSpeed;

                if (ClickDetector.Instance.GetRotationTouch().isPressed || Input.GetMouseButton(0))
                    LookAtRotatorController.Insctance.StopLookAtRotation();

                if (Input.touchCount > 0)
                {
                    if (!ClickDetector.Instance.GetUiTouch().isPressed)
                        ResetCameraMoving();

                    if (ClickDetector.Instance.GetRotationTouch().isPressed &&
                        Input.GetTouch(ClickDetector.Instance.GetRotationTouch().touchID).phase == TouchPhase.Moved)
                    {
                        currentRotationSpeed = maxRotationSpeed;
                        //  if(Input.GetTouch(0).deltaPosition.magnitude > startMoveTrashold) return;  //added to prevent start camera flip
                        // invert control
                        int _invertX = -1;
                        if (invertX) _invertX = 1;

                        int _invertY = 1;
                        if (invertY) _invertY = -1;

                        float _xDelta = Input.GetTouch(ClickDetector.Instance.GetRotationTouch().touchID).deltaPosition
                            .x;
                        float _yDelta = Input.GetTouch(ClickDetector.Instance.GetRotationTouch().touchID).deltaPosition
                            .y;
                        h = _xDelta * _invertX;
                        v = _yDelta * _invertY;
                    }
                }
                else
                {
                    h = -Input.GetAxis("Mouse X");
                    v = Input.GetAxis("Mouse Y");
                }
            }

            // detect PC key WASD + QE + mouse wheel
            if (!Application.isMobilePlatform)
                DetectPCKayMoving();
        }

        private void FixedUpdate()
        {
            float _curV;
            float _curH;
            if (Application.isMobilePlatform)
            {
                _curV = v * currentRotationSpeed * touchSensitivity;
                _curH = h * currentRotationSpeed * touchSensitivity;
            }
            else // correct sensitivity for PC mouse
            {
                _curV = v * currentRotationSpeed * mouseSensitivity;
                _curH = h * currentRotationSpeed * mouseSensitivity;
            }


            // don't rotate vertical, if camera in top or bottom limit 
            float _newYAfterRotation = player.transform.eulerAngles.x + _curV;
            if (_newYAfterRotation <= yAxisTopLimit && _newYAfterRotation > 180 ||
                _newYAfterRotation >= yAxisBottomLimit && _newYAfterRotation < 180)
                _curV = 0;

            // rotate vertical
            Vector3 eulerAnglesVertical = player.transform.eulerAngles;
            eulerAnglesVertical =
                new Vector3(eulerAnglesVertical.x + _curV, eulerAnglesVertical.y, eulerAnglesVertical.z);
            player.transform.eulerAngles = eulerAnglesVertical;

            // rotate horizontal
            Vector3 eulerAnglesHorizontal = cameraBody.transform.eulerAngles;
            eulerAnglesHorizontal = new Vector3(eulerAnglesHorizontal.x, eulerAnglesHorizontal.y + _curH,
                eulerAnglesHorizontal.z);
            cameraBody.transform.eulerAngles = eulerAnglesHorizontal;

            // move camera from screen buttons
            if ((isMoveUp || isMoveDown))
                MoveUpDown();
        }

        //check for the lower camera limit
        private void LateUpdate()
        {
            Vector3 _cameraRotation = player.transform.eulerAngles;
            // top
            if (_cameraRotation.x <= yAxisTopLimit && _cameraRotation.x > 180)
                player.transform.eulerAngles = new Vector3(yAxisTopLimit, _cameraRotation.y, _cameraRotation.z);

            // bottom 
            if (_cameraRotation.x >= yAxisBottomLimit && _cameraRotation.x < 180)
                player.transform.eulerAngles = new Vector3(yAxisBottomLimit, _cameraRotation.y, _cameraRotation.z);
        }


        #region CameraMoving

        // call from event trigger on button  
        public void SetStateIsMoveUp(bool _val)
        {
            isMoveUp = _val;
        }

        // call from event trigger on button  
        public void SetStateIsMoveDown(bool _val)
        {
            isMoveDown = _val;
        }

        // move camera from ui
        private void MoveUpDown()
        {
            if (isMoveUp)
            {
                Vector3 forwardForce = Time.deltaTime * cameraBody.transform.forward;
                ApplyForceToTheBody(forwardForce * movingForceSpeed);
            }
            else if (isMoveDown)
            {
                Vector3 backForce = Time.deltaTime * -cameraBody.transform.forward;
                ApplyForceToTheBody(backForce * movingForceSpeed);
            }
        }

        private void SetKinematicForRB(bool _val)
        {
            cameraBody.isKinematic = _val;
        }

        // Detect PC kay for control moving WASD + QE
        private void DetectPCKayMoving()
        {
            float boost = 1f;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                boost *= shiftBoostSpeed;
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0) // move forward
            {
                Vector3 forwardForce = Time.fixedDeltaTime * cameraBody.transform.forward;
                forwardForce *= boost;
                ApplyForceToTheBody(forwardForce * movingForceSpeed);
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0) // move back
            {
                Vector3 backForce = Time.fixedDeltaTime * -cameraBody.transform.forward;
                backForce *= boost;
                ApplyForceToTheBody(backForce * movingForceSpeed);
            }

            if (Input.GetKey(KeyCode.W)) // move forward
            {
                Vector3 forwardForce = Time.fixedDeltaTime * cameraBody.transform.forward;
                forwardForce *= boost;
                ApplyForceToTheBody(forwardForce * movingForceSpeed);
            }

            if (Input.GetKey(KeyCode.S)) // move back
            {
                Vector3 backForce = Time.fixedDeltaTime * -cameraBody.transform.forward;
                backForce *= boost;
                ApplyForceToTheBody(backForce * movingForceSpeed);
            }

            if (Input.GetKey(KeyCode.A)) // move left
            {
                Vector3 leftForce = Time.fixedDeltaTime * -cameraBody.transform.right;
                leftForce *= boost;
                ApplyForceToTheBody(leftForce * movingForceSpeed);
            }

            if (Input.GetKey(KeyCode.D)) // move right
            {
                Vector3 rightForce = Time.fixedDeltaTime * cameraBody.transform.right;
                rightForce *= boost;
                ApplyForceToTheBody(rightForce * movingForceSpeed);
            }

            if (Input.GetKey(KeyCode.Q)) // rotate left
            {
                Vector3 eulerAnglesHorizontal = cameraBody.transform.eulerAngles;
                eulerAnglesHorizontal = new Vector3(eulerAnglesHorizontal.x,
                    eulerAnglesHorizontal.y - cameraRotationFromKeySpeed, eulerAnglesHorizontal.z);
                cameraBody.transform.eulerAngles = eulerAnglesHorizontal;
                LookAtRotatorController.Insctance.StopLookAtRotation();
            }

            if (Input.GetKey(KeyCode.E)) // rotate right
            {
                Vector3 eulerAnglesHorizontal = cameraBody.transform.eulerAngles;
                eulerAnglesHorizontal = new Vector3(eulerAnglesHorizontal.x,
                    eulerAnglesHorizontal.y + cameraRotationFromKeySpeed, eulerAnglesHorizontal.z);
                cameraBody.transform.eulerAngles = eulerAnglesHorizontal;
                LookAtRotatorController.Insctance.StopLookAtRotation();
            }
        }

        private void ApplyForceToTheBody(Vector3 _newForce)
        {
            SetKinematicForRB(false);
            cameraBody.AddForce(_newForce);
            LookAtRotatorController.Insctance.StopLookAtRotation();
            StopAllCoroutines();
            StartCoroutine(FreezRb()); // try freez RB  
        }

        // freez rigidbody after stop moving 
        IEnumerator FreezRb()
        {
            yield return new WaitForSeconds(0.1f);
            SetKinematicForRB(true);
        }

        #endregion

        // enable/disable control btn for mobile
        public void SetStateControlBtn(bool _val)
        {
            if (Application.isMobilePlatform)
                controlBtn.SetActive(_val);
        }

        // reset camera moving
        private void ResetCameraMoving()
        {
            if (!isMoveUp && !isMoveDown) return;
            SetStateIsMoveUp(false);
            SetStateIsMoveDown(false);
            SetKinematicForRB(true);
        }
    }
}