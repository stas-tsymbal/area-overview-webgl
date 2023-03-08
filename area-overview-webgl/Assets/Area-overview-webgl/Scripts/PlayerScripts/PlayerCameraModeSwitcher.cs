using System;
using System.Collections;

using Area_overview_webgl.Scripts.LookAtRotatorScripts;
using Area_overview_webgl.Scripts.ParallelAreaScripts;
using UnityEngine;

namespace Area_overview_webgl.Scripts.PlayerScripts
{
    public class PlayerCameraModeSwitcher : MonoBehaviour
    {
        private enum CameraMode
    {
        orbital,
        firstPerson
    }

    [SerializeField] private CameraMode currentCameraMode;
    
    [Header("Camera Moving")]
    [SerializeField] private Transform orbitalPosition; 
    [SerializeField] private Transform orbitalCameraParent; // parent for orbital camera
    [Space]
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
    
    [SerializeField] private Transform rayYHeight; // height of ray for check teleport

    public Action OnCameraModeChanged;

    public void Init()
    {
        
    }
    
    // set orbital mode, call from UI
    public void SetOrbitalMode()
    {
        SetCameraMode(CameraMode.orbital); // set camera mode
       
        RememberFirstPersonPosition();
        myCamera.SetParent(orbitalCameraParent);
        MoveCameraToOrbital(); // copy and set camera position 
        
        // lerp camera
       // StopAllCoroutines();
        StartCoroutine(LerpOrbitalMode());
        
    }
    
    // disable camera rotation and moving scripts while lerp 
    private IEnumerator LerpOrbitalMode()
    {
        while (Vector3.Distance(myCamera.position,targetTransform.position) > lerpMinValue)
        {
            myCamera.position = Vector3.Lerp( myCamera.position, targetTransform.position, Time.deltaTime * lerpSpeed);
            myCamera.rotation = Quaternion.Lerp(myCamera.rotation, orbitalPosition.rotation, Time.deltaTime * lerpSpeed);
            yield return null;
        }
        OnCameraModeChanged?.Invoke();
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
        SetCameraMode(CameraMode.firstPerson);
        // reset parent
        myCamera.SetParent(firstPersonCameraParent.transform);
        // lerp moving 
      //  StopAllCoroutines();
        StartCoroutine(LerpForFirsPersonMode());
    }

    // disable camera rotation and moving scripts while lerp 
    private IEnumerator LerpForFirsPersonMode()
    {
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
        OnCameraModeChanged?.Invoke();
    }
    
    // set camera mode
    private void SetCameraMode(CameraMode _mode)
    {
        currentCameraMode = _mode;
    }

    // set orbital position for camera
    private void MoveCameraToOrbital()
    {
        targetTransform.position = orbitalPosition.position;
        targetTransform.eulerAngles = orbitalPosition.eulerAngles;
    }
    

    // set new position for last person position gameobject
    private void SetLastPersonPosition(Vector3 _pos)
    {
        firstPersonCameraParent.transform.position = new Vector3(_pos.x, _pos.y + firstPersonCameraParent.height/2, _pos.z);
        lastFPPosition.position = new Vector3(_pos.x, lastFPPosition.position .y, _pos.z);
    }
    
    }
}