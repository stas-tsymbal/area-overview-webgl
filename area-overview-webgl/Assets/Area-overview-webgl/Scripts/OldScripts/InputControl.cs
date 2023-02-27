using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * Script added to provide camera orbital rotation around target
 */

public class InputControl : MonoBehaviour
{
     [SerializeField] private GameObject cameraOrbit;    //link to the camera sphere
    [SerializeField] private float maxRotationSpeed = 8f;    //rotation speed
    [SerializeField] private float yAxisBottomLimit = 80f;  //lower angle for the camera rotation
    private float currentRotationSpeed;
    [SerializeField] private float lerpSpeed = 10f;

    private float h;
    private float v;

    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private float touchSensitivity = .1f;
    [SerializeField] private float mouseSensitivity = .1f;
    [SerializeField] private float startMoveTrashold = 10f;

    [SerializeField] private Transform firstPersonCameraRB;
    
    [Header("Invert mobile moving")]
    [SerializeField] private bool invertX;
    [SerializeField] private bool invertY;

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
        if ((!Application.isMobilePlatform && (!Input.GetMouseButton(0) || ClickDetector.Instance.IsMouseOverUI())) ||
            ( (Application.isMobilePlatform && Input.touchCount == 0) || (Application.isMobilePlatform && Input.GetTouch(0).phase != TouchPhase.Moved)))
        {
            //added to smoothly lerp camera rotation speed after lmb unpick
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0f, Time.deltaTime * lerpSpeed);
        }
        
        
        if ( (Input.GetMouseButton(0) && !ClickDetector.Instance.IsMouseOverUI()) || (Input.touchCount > 0 && !ClickDetector.Instance.GetUiTouch().isPressed))
        {
            if(!Application.isMobilePlatform) // for PC
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
                    if (invertX) _invertX = -1;
                    var _invertY = 1;
                    if (invertY) _invertY = -1;
                  
                    var _xDelta = Input.GetTouch(0).deltaPosition.x;
                    var _yDelta = Input.GetTouch(0).deltaPosition.y;
                    h = _xDelta * touchSensitivity * _invertX;
                    v = _yDelta * touchSensitivity * _invertY;
                }
            }
            else
            {
                h = Input.GetAxis("Mouse X");
                v = Input.GetAxis("Mouse Y");
            }
        }

        //ZOOM CONTROLS
        /*var scrollFactor = Input.GetAxis("Mouse ScrollWheel");
        if (scrollFactor != 0)
        {
            cameraOrbit.transform.localScale *= (1f - scrollFactor);
        }*/
    }

    private void FixedUpdate()
    {
        var _curV = v * currentRotationSpeed * mouseSensitivity;
        var _curH = h * currentRotationSpeed * mouseSensitivity;
        
        if (cameraOrbit.transform.eulerAngles.z + _curV <= 0.1f || cameraOrbit.transform.eulerAngles.z + _curV >= 179.9f)
            _curV = 0;
        
        var eulerAngles = cameraOrbit.transform.eulerAngles;
        eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y + _curH, eulerAngles.z + _curV);
        cameraOrbit.transform.eulerAngles = eulerAngles;
        
        firstPersonCameraRB.eulerAngles = new Vector3(firstPersonCameraRB.eulerAngles.x, cameraOrbit.transform.eulerAngles.y+90, firstPersonCameraRB.eulerAngles.z);
    }

    //check for the lower camera limit
    private void LateUpdate()
    {
        var _cameraRotation = cameraOrbit.transform.eulerAngles;

        if (_cameraRotation.z > yAxisBottomLimit)
            cameraOrbit.transform.eulerAngles = new Vector3(_cameraRotation.x, _cameraRotation.y, yAxisBottomLimit);
    }

    // get orbital camera transform
    public Transform GetCameraOrbit()
    {
        return cameraOrbit.transform;
    }
}
