using System;
using Area_overview_webgl.Scripts.LookAtRotatorScripts;
using Area_overview_webgl.Scripts.Static;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Area_overview_webgl.Scripts.TeleportScripts
{
    public class TeleportController : MonoBehaviour
    {
        [Header("Teleport")] [SerializeField] private Teleport telepot;
        [Header("Layer for teleport")]
        [SerializeField] private LayerMask teleportLayerMask ;
        
        [Space] private Vector2 currentAngle; // use for remember current angle -> X and Y
        private bool isRememberCurrentAngle;
        
        private Camera myCamera;
        private RaycastHit currentHit;
        
        
        public static TeleportController Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void Init(Transform firstPersonCamera, CapsuleCollider firstPersonCollider)
        {
            telepot.Init(firstPersonCamera, firstPersonCollider);
            telepot.OnEndTeleportation += OnEndTeleportation;
        }

        private void OnEndTeleportation()
        {
            Debug.Log("end teleportation");
        }
        
        
        private void Update()
        {
            //TODO remove Update
          /*  if (!Application.isMobilePlatform)
            {
                #region PC teleport detector

                // detect mouse btn down and remember camera angle
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    RememberCurrentCameraAngle();
                }

                // detect mouse btn up and try to make teleport
                if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    if (CanTeleport() && CheckLayer(Input.mousePosition))
                    {
                        LookAtRotatorController.Insctance.StopLookAtRotation();
                       // CameraModeController.CameraModeController.Instance.MoveCameraByClick(GetHit().point);
                        telepot.MakeTeleport(GetHit().point);
                    }
                    else
                    {
                        //added to provide look at mech
                        if (CanTeleport()) LookAtRotatorController.Insctance.TryRotateToObject(Input.mousePosition);
                    }
                }

                #endregion
            }
            else
            {
                #region Mobile teleport detector

                // remember current angle in click detector and try 

                // detect touch down and remember camera angle
                if (Input.touchCount > 0)
                {
                    
                }

                #endregion
            }*/
        }
        
        
    /*    [SerializeField] private Transform firstPersonX;
        [SerializeField] private Transform firstPersonY;
        
        [SerializeField] private Transform orbitalCamera;
        // remember camera angle
        public void RememberCurrentCameraAngle()
        {
            if (isRememberCurrentAngle) return; // don't remember again while mouse btn down

            isRememberCurrentAngle = true;
            if (!CameraModeController.CameraModeController.Instance.IsCurrentModeOrbital())
                currentAngle = new Vector2(firstPersonX.eulerAngles.y, firstPersonY.eulerAngles.x);
            else
                currentAngle = new Vector2(orbitalCamera.eulerAngles.y, orbitalCamera.eulerAngles.z);
        }*/

 /*   [Header("Ignore teleport")]
    [SerializeField] private float angleForIgnoreTeleport;
        private bool CanTeleport()
        {
            isRememberCurrentAngle = false;
            var canTeleport = true;
            if (!CameraModeController.CameraModeController.Instance.IsCurrentModeOrbital())
            {
                if (Math.Abs(Math.Abs(currentAngle.x) - Math.Abs(firstPersonX.eulerAngles.y)) >
                    angleForIgnoreTeleport || // check X
                    (Math.Abs(Math.Abs(currentAngle.y) - Math.Abs(firstPersonY.eulerAngles.x)) >
                     angleForIgnoreTeleport)) // check Y
                    canTeleport = false;
            }
            else
            {
                if (Math.Abs(Math.Abs(currentAngle.x) - Math.Abs(orbitalCamera.eulerAngles.y)) >
                    angleForIgnoreTeleport || // check X
                    (Math.Abs(Math.Abs(currentAngle.y) - Math.Abs(orbitalCamera.eulerAngles.z)) >
                     angleForIgnoreTeleport)) // check Y
                    canTeleport = false;
            }

            return canTeleport;
        }*/
        
      /*  private bool CanTeleport(Transform firstPersonX, Transform firstPersonY)
        {
            isRememberCurrentAngle = false;
            var canTeleport = true;
            if (!CameraModeController.CameraModeController.Instance.IsCurrentModeOrbital())
            {
                if (Math.Abs(Math.Abs(currentAngle.x) - Math.Abs(firstPersonX.eulerAngles.y)) >
                    angleForIgnoreTeleport || // check X
                    (Math.Abs(Math.Abs(currentAngle.y) - Math.Abs(firstPersonY.eulerAngles.x)) >
                     angleForIgnoreTeleport)) // check Y
                    canTeleport = false;
            }
            else
            {
                if (Math.Abs(Math.Abs(currentAngle.x) - Math.Abs(orbitalCamera.eulerAngles.y)) >
                    angleForIgnoreTeleport || // check X
                    (Math.Abs(Math.Abs(currentAngle.y) - Math.Abs(orbitalCamera.eulerAngles.z)) >
                     angleForIgnoreTeleport)) // check Y
                    canTeleport = false;
            }

            return canTeleport;
        }*/
        
        // check layer for teleport
        private bool CheckLayer(Vector3 _rayStartPosition)
        {
            var isLayerForTeleport = false;
            RaycastHit hit;
            var ray = myCamera.ScreenPointToRay(_rayStartPosition); // make ray from position
            if (Physics.Raycast(ray, out hit))
            {
                var hitObject = hit.transform.gameObject; // ray hit this object
                if (hitObject.layer == Calculator.GetPowNumber2(teleportLayerMask.value))
                    isLayerForTeleport = true;
                currentHit = hit;
            }

            return isLayerForTeleport;
        }
        
        // call from TouchPhase.Ended in click detector
        public void TryMakeTeleport(Vector2 _position)
        {
            if (CheckLayer(_position))
                telepot.MakeTeleport(GetHit().point);
              //  CameraModeController.CameraModeController.Instance.MoveCameraByClick(GetHit().point);
        }
        
        private RaycastHit GetHit()
        {
            return currentHit;
        }
        
        
    }
}