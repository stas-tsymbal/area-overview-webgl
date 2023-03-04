using System;
using System.Collections;
using Area_overview_webgl.Scripts.FPSCamera;
using Area_overview_webgl.Scripts.LookAtRotatorScripts;
using Area_overview_webgl.Scripts.ParallelAreaScripts;
using UnityEngine;

namespace Area_overview_webgl.Scripts.CameraModeController
{
    public class CameraModeController : MonoBehaviour
    {
         public static CameraModeController Instance;
    private enum CameraMode
    {
        orbital,
        firstPerson
    }

    [SerializeField] private CameraMode cameraMode;

    [Header("Control Scripts")]
    [SerializeField] private OrbitRotation.OrbitRotation orbitalLogic;
    [SerializeField] private FirstPersonRotator mobileFirstPerson; // logic PC and mobile

    [Header("Camera Moving")]
    [SerializeField] private Transform orbitalPosition; 
    [SerializeField] private Transform orbitalCameraParent; // parent for orbital camera
    [SerializeField] private CapsuleCollider firstPersonCameraParent; // parent for first person camera
    [SerializeField] private Transform myCamera;
    [Header("Lerp changing mode")]
    [SerializeField] private float lerpSpeed = 10f;
    [SerializeField] private Transform targetTransform; // target  for orbital camera lerp 
    [SerializeField] private float lerpMinValue = 0.1f;

    [Header("Last first person position")] [SerializeField]
    private Transform lastFPPosition;

    [Header("First person angle on ground")] // set this angle when activate FP mode, settings for orbitStandardAngle you can find in mobile and pc first player logic  
    [SerializeField] private float firstPersonStandardAngle = 0; // apply for cam x
    private Coroutine moveCor;
  //  [SerializeField] private CameraModeIndicator cameraModeIndicator;
    [SerializeField] private ParallelAreaIndicatorActivationController normalDetector;
    [SerializeField] private Transform rayYHeight; // height of ray for check teleport
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        // indicate default mode
        if (IsCurrentModeOrbital())
        {
            SetCameraMode(CameraMode.firstPerson); // change mode because void return the same camera mode  --> if(cameraMode.Equals(CameraMode.orbital)) return;
            SetOrbitalMode(); 
        }
        else
        {
            SetCameraMode(CameraMode.orbital);
            SetFirstPersonMode();
        }
    }

    // set orbital mode, call from UI
    public void SetOrbitalMode()
    {
        if(IsCurrentModeOrbital()) return;
      //  cameraModeIndicator.SetOrbitColor();
        SetCameraMode(CameraMode.orbital); // set camera mode
        EnableMobileFirstPersonScript(false);
        
        RememberFirstPersonPosition();
        myCamera.SetParent(orbitalCameraParent);
        MoveCameraToOrbital(); // copy and set camera position 
        
        // lerp camera
        StopAllCoroutines();
        StartCoroutine(LerpOrbitalMode());
        SetStateMobileControlBtn(false);
        
        LookAtRotatorController.Insctance.StopLookAtRotation();
    }
    
    // disable camera rotation and moving scripts while lerp 
    private IEnumerator LerpOrbitalMode()
    {
        normalDetector.DisableIndicatorForTeleport(true);
        while (Vector3.Distance(myCamera.position,targetTransform.position) > lerpMinValue)
        {
            myCamera.position = Vector3.Lerp( myCamera.position, targetTransform.position, Time.deltaTime * lerpSpeed);
            myCamera.rotation = Quaternion.Lerp(myCamera.rotation, orbitalPosition.rotation, Time.deltaTime * lerpSpeed);
            yield return null;
        }
        EnableOrbitalScript(true);
        normalDetector.DisableIndicatorForTeleport(false);
    }

    // Remember first person position
    private void RememberFirstPersonPosition()
    {
        lastFPPosition.position = myCamera.position;
        lastFPPosition.eulerAngles = myCamera.eulerAngles;
    }

    // set first person mode, call from UI
    public void SetFirstPersonMode()
    {
        if(!IsCurrentModeOrbital()) return;
        // set camera mode
   //     cameraModeIndicator.SetWalkColor();
        SetCameraMode(CameraMode.firstPerson);
        // off orbital moving logic 
        EnableOrbitalScript(false);
        // reset parent
        myCamera.SetParent(firstPersonCameraParent.transform);
        // lerp moving 
        StopAllCoroutines();
        StartCoroutine(LerpForFirsPersonMode());
        SetStateMobileControlBtn(true);
    }

    // disable camera rotation and moving scripts while lerp 
    private IEnumerator LerpForFirsPersonMode()
    {
        normalDetector.DisableIndicatorForTeleport(true);
        // set X axis for first person cam -> lastFPPosition.eulerAngles(NEW_value, OLD, OLD)
        var eulerAngles = lastFPPosition.eulerAngles;
        eulerAngles = new Vector3(firstPersonStandardAngle, eulerAngles.y, eulerAngles.z);
        lastFPPosition.eulerAngles = eulerAngles;
        
        // Lerp camera position and rotation from ORBITAL MODE to FIRST PERSON 
        while (Vector3.Distance(myCamera.position,lastFPPosition.position) > lerpMinValue)
        {
            myCamera.position = Vector3.Lerp( myCamera.position, lastFPPosition.position, Time.deltaTime * lerpSpeed);
            myCamera.rotation = Quaternion.Lerp(myCamera.rotation, lastFPPosition.rotation, Time.deltaTime * lerpSpeed);
            yield return null;
        }
       
        // on first person moving logic
        EnableMobileFirstPersonScript(true);
        normalDetector.DisableIndicatorForTeleport(false);
    }
    
    // set camera mode
    private void SetCameraMode(CameraMode _mode)
    {
        cameraMode = _mode;
    }

    // on/off orbital script
    private void EnableOrbitalScript(bool _val)
    {
        orbitalLogic.enabled = _val;
    }
    
    // on/off mobile camera script
    private void EnableMobileFirstPersonScript(bool _val)
    {
        if(mobileFirstPerson != null)
            mobileFirstPerson.enabled = _val;
    }

    // set orbital position for camera
    private void MoveCameraToOrbital()
    {
        targetTransform.position = orbitalPosition.position;
        targetTransform.eulerAngles = orbitalPosition.eulerAngles;
    }

    // on/off mobile control btn for first person mode 
    public void SetStateMobileControlBtn(bool _val)
    {
        mobileFirstPerson.SetStateControlBtn(_val);
    }

    // return is orbital game mode
    public bool IsCurrentModeOrbital()
    {
        return cameraMode.Equals(CameraMode.orbital);
    }

    // set new position for last person position gameobject
    private void SetLastPersonPosition(Vector3 _pos)
    {
        firstPersonCameraParent.transform.position = new Vector3(_pos.x, _pos.y + firstPersonCameraParent.height/2, _pos.z);
        lastFPPosition.position = new Vector3(_pos.x, lastFPPosition.position .y, _pos.z);
    }

    //click 
    public void MoveCameraByClick(Vector3 _newPos)
    {
        if (IsCurrentModeOrbital())
        {
            SetLastPersonPosition(_newPos);
            SetFirstPersonMode();
        }
        else
        {
            if(moveCor != null)
                StopCoroutine(moveCor);
            moveCor = StartCoroutine(MoveByClick(_newPos));
        }
    }
    
    // disable camera rotation and moving scripts while lerp 
    private IEnumerator MoveByClick(Vector3 _newPos)
    {
        normalDetector.DisableIndicatorForTeleport(true);
        _newPos = new Vector3(_newPos.x,_newPos.y + firstPersonCameraParent.height/2, _newPos.z);
        // Lerp camera position 
        while (Vector3.Distance(firstPersonCameraParent.transform.position,_newPos) > lerpMinValue)
        {
            if (CheckDistanceLimit(_newPos)) // check capsule near wall
            {
                _newPos = firstPersonCameraParent.transform.position;
                //Debug.LogError("stop move");
            }
            firstPersonCameraParent.transform.position  = Vector3.Lerp( firstPersonCameraParent.transform.position, _newPos, Time.deltaTime * lerpSpeed);
            yield return null;
        }
        normalDetector.DisableIndicatorForTeleport(false);
        
    }
    
    // lerp can't finish if target point near wall, because capsule collider with this obstacles and newer reach target position 
    private bool CheckDistanceLimit(Vector3 _targetPoint)
    {
        var needStop = false;
        // add 2 vector3 for correct Y 
        var playerPosition = new Vector3(firstPersonCameraParent.transform.position.x, rayYHeight.position.y,
            firstPersonCameraParent.transform.position.z);
        var targetPointFixY = new Vector3(_targetPoint.x, rayYHeight.position.y, _targetPoint.z);

        if (Math.Sqrt((playerPosition - targetPointFixY).sqrMagnitude) < firstPersonCameraParent.radius + 0.6f)
        {
            RaycastHit hit;
            var ray = new Ray(playerPosition, targetPointFixY-playerPosition);
            if (Physics.Raycast(ray, out hit)) {
                var hitObject = hit.transform.gameObject;  // ray hit this object
                if (Math.Sqrt((playerPosition - hit.point).sqrMagnitude) <
                    firstPersonCameraParent.radius + 0.5f)
                    needStop = true;
            }
        }
          
        return needStop;
    }

    //added to control look at mode behaviour
    public bool FPScameraActive()
    {
        return cameraMode == CameraMode.firstPerson;
    }
    }
}