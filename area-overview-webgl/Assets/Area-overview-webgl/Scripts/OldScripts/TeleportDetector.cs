using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * This script detect mouse or touch moving and decide that we can make teleport or not 
 */
public class TeleportDetector : MonoBehaviour
{
    public static TeleportDetector Instance;
    [Space] 
    [SerializeField] private Vector2 currentAngle; // use for remember current angle -> X and Y 
    [SerializeField] private bool isRememberCurrentAngle = false; 
    [SerializeField] private float angleForIgnoreTeleport;
    [SerializeField] private Transform orbitalCamera;
    [SerializeField] private Transform firstPersonX;
    [SerializeField] private Transform firstPersonY;
    [SerializeField] private int touchIndex = -1;
    [SerializeField] private Camera camera;
    private RaycastHit currentHit;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!Application.isMobilePlatform)
        {
            #region PC teleport detector
            // detect mouse down and remember camera angle
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                RememberCurrentCameraAngle();
            }

            // detect mouse up and try to make teleport
            if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (CanTeleport() && CheckLayer(Input.mousePosition))
                {
                    CameraLookAtController.Instance.BreakLookAtRotation();
                    CameraModeController.Instance.MoveCameraByClick(GetHit().point);
                }
                else
                {
                    //added to provide look at mech
                    if(CanTeleport()) CameraLookAtController.Instance.CheckLookAtLayer(Input.mousePosition);
                }
            }
            #endregion
        }
        else
        {
            #region Mobile teleport detector
            // remember current angle in click detector and try 
            
            // detect touch down and remember camera angle
            if (Input.touchCount > 0 )
            {
               
            }
            #endregion
        }
    }

    // call from TouchPhase.Ended in click detector
    public void TryMakeTeleport(Vector2 _position)
    {
        if (CanTeleport() && CheckLayer(_position))
            CameraModeController.Instance.MoveCameraByClick(GetHit().point);
    }
    private void FixedUpdate()
    {
       // if(Application.isMobilePlatform) return;
      /*  RaycastHit hit;
        var ray = camera.ScreenPointToRay(Input.mousePosition); // make ray from position
        if (Physics.Raycast(ray, out hit))
        {
            currentHit = hit;
        }*/
    }


    // remember camera angle
    public void RememberCurrentCameraAngle()
    {
        if(isRememberCurrentAngle) return; // don't remember again while mouse btn down
        
        isRememberCurrentAngle = true;
        if(!CameraModeController.Instance.IsCurrentModeOrbital())
            currentAngle = new Vector2(firstPersonX.eulerAngles.y ,firstPersonY.eulerAngles.x);
        else
            currentAngle = new Vector2(orbitalCamera.eulerAngles.y, orbitalCamera.eulerAngles.z );
    }

    // check camera rotation -> mouse down camera position and mouse up camera position 
    private bool CanTeleport()
    {
        isRememberCurrentAngle = false;
        var canTeleport = true;
        if (!CameraModeController.Instance.IsCurrentModeOrbital())
        {
            if (Math.Abs(Math.Abs(currentAngle.x) - Math.Abs(firstPersonX.eulerAngles.y)) > angleForIgnoreTeleport || // check X
                (Math.Abs(Math.Abs(currentAngle.y) - Math.Abs(firstPersonY.eulerAngles.x)) > angleForIgnoreTeleport)) // check Y
                canTeleport = false;
        }
        else
        {
            if (Math.Abs(Math.Abs(currentAngle.x) - Math.Abs(orbitalCamera.eulerAngles.y)) > angleForIgnoreTeleport || // check X
                (Math.Abs(Math.Abs(currentAngle.y) - Math.Abs(orbitalCamera.eulerAngles.z)) > angleForIgnoreTeleport)) // check Y
                canTeleport = false;
        }
        return canTeleport;
    }
    
    // get index of touch 
    private void FindTouchOnScreen()
    {
        var _index = -1;
        foreach (var touch in Input.touches)
        {
            var id = touch.fingerId;
            if (EventSystem.current.IsPointerOverGameObject(id)) continue;
            _index = id; // don't touch ui
            break;
        }
        touchIndex = _index;
    }

    // check layer for teleport
    private bool CheckLayer(Vector3 _rayStartPosition)
    {
        var isLayerForTeleport = false;
        RaycastHit hit;
        var ray = camera.ScreenPointToRay(_rayStartPosition); // make ray from position
        if (Physics.Raycast(ray, out hit)) {
            var hitObject = hit.transform.gameObject;  // ray hit this object
            if (hitObject.layer == LayerMask.NameToLayer("Teleport"))
                isLayerForTeleport = true;
            currentHit = hit;
        }
 
        return isLayerForTeleport;
    }

    public RaycastHit GetHit()
    {
        return currentHit;
    }

    public Transform GetFirstPersonX()
    {
        return firstPersonX;
    }
    
    public Transform GetFirstPersonY()
    {
        return firstPersonY;
    }
}
