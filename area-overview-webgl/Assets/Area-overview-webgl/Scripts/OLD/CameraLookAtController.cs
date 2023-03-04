using System;
using System.Collections;
using System.Collections.Generic;
using Area_overview_webgl.Scripts.CameraModeController;
using Area_overview_webgl.Scripts.Static;
using Area_overview_webgl.Scripts.Teleport;
using UnityEngine;
using UnityEngine.Serialization;

/*
 * Script added to provide camera look at selected object, object need to be in special layer (lookAtLayer)
 */

public class CameraLookAtController : MonoBehaviour
{
    [SerializeField] private Transform fpsCamera;
    [SerializeField] private Camera myCamera;
    
    
    [SerializeField] private Transform fpsBody;
    [SerializeField] private Transform lookAtPointBody;
    [SerializeField] private Transform lookAtPointCamera;
    
    private bool rotationIsActive = false;
    [SerializeField] private float rotationSpeed = 1f;

    [SerializeField] private LayerMask lookAtLayer;
    #region Singleton

    public static CameraLookAtController Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    //after player clicks over some scene obj
    public void CheckLookAtLayer(Vector3 _inputPosition)
    {
        Debug.Log("look at this object");
        if(!CameraModeController.Instance.FPScameraActive())
            return;
        
        RaycastHit hit;
        Ray ray = myCamera.ScreenPointToRay(_inputPosition); // make ray from position
        if (Physics.Raycast(ray, out hit)) 
        {
            GameObject hitObject = hit.transform.gameObject;  // ray hit this object
            if (hitObject.layer == Calculator.GetPowNumber2(lookAtLayer.value))
            {
                //start camera rotation towards the target
                lookAtPointBody.position = fpsBody.position;
                lookAtPointCamera.position = fpsCamera.position;
                
                lookAtPointBody.LookAt(hit.point);
                lookAtPointBody.eulerAngles = new Vector3(0f, lookAtPointBody.eulerAngles.y, 0f);
                
                lookAtPointCamera.LookAt(hit.point);
                lookAtPointCamera.localEulerAngles = new Vector3(lookAtPointCamera.localEulerAngles.x, 0f, 0f);

                rotationIsActive = true;
                //Debug.Log("rotation started" + hit.transform.name);
            }
        }
    }

    private void LateUpdate()
    {
        if(!rotationIsActive)
            return;
            
        //try to lerp camera rotation towards the target
        fpsBody.rotation = Quaternion.Lerp(fpsBody.rotation, lookAtPointBody.rotation, Time.deltaTime * rotationSpeed);
        fpsBody.eulerAngles = new Vector3(0f, fpsBody.eulerAngles.y, 0f);
        
        fpsCamera.rotation = Quaternion.Lerp(fpsCamera.rotation, lookAtPointCamera.rotation,Time.deltaTime * rotationSpeed);
        fpsCamera.localEulerAngles = new Vector3(fpsCamera.localEulerAngles.x, 0f, 0f);
    }

    //stop look at action after player's activity
    public void BreakLookAtRotation()
    {
        rotationIsActive = false;
      //  Debug.Log("rotation stopped");
    }
}
