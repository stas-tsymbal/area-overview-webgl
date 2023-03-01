using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Area_overview_webgl.Scripts.Teleport
{
    public class Teleport : MonoBehaviour
    {
        public static Teleport Instance;
        [Header("Layer for teleport")]
        [SerializeField] private LayerMask teleportLayerMask ;
        
        [Header("Ignore teleport")]
        [SerializeField] private float angleForIgnoreTeleport;
        
        [Space] private Vector2 currentAngle; // use for remember current angle -> X and Y
        private bool isRememberCurrentAngle;
        
        [SerializeField] private Transform orbitalCamera;
        [SerializeField] private Transform firstPersonX;
        [SerializeField] private Transform firstPersonY;

        private Camera myCamera;
        private RaycastHit currentHit;

        private void Awake()
        {
            Instance = this;
            
            myCamera = Camera.main;
        }

        private void Update()
        {
            if (!Application.isMobilePlatform)
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
                        CameraLookAtController.Instance.BreakLookAtRotation();
                        CameraModeController.CameraModeController.Instance.MoveCameraByClick(GetHit().point);
                    }
                    else
                    {
                        //added to provide look at mech
                        if (CanTeleport()) CameraLookAtController.Instance.CheckLookAtLayer(Input.mousePosition);
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
            }
        }

        // call from TouchPhase.Ended in click detector
        public void TryMakeTeleport(Vector2 _position)
        {
            if (CanTeleport() && CheckLayer(_position))
                CameraModeController.CameraModeController.Instance.MoveCameraByClick(GetHit().point);
        }
        
        
        // remember camera angle
        public void RememberCurrentCameraAngle()
        {
            if (isRememberCurrentAngle) return; // don't remember again while mouse btn down

            isRememberCurrentAngle = true;
            if (!CameraModeController.CameraModeController.Instance.IsCurrentModeOrbital())
                currentAngle = new Vector2(firstPersonX.eulerAngles.y, firstPersonY.eulerAngles.x);
            else
                currentAngle = new Vector2(orbitalCamera.eulerAngles.y, orbitalCamera.eulerAngles.z);
        }
        
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
        }
        
        // check layer for teleport
        private bool CheckLayer(Vector3 _rayStartPosition)
        {
            var isLayerForTeleport = false;
            RaycastHit hit;
            var ray = myCamera.ScreenPointToRay(_rayStartPosition); // make ray from position
            if (Physics.Raycast(ray, out hit))
            {
                var hitObject = hit.transform.gameObject; // ray hit this object
                if (hitObject.layer == MathStat.GetPowNumber2(teleportLayerMask.value))
                    isLayerForTeleport = true;
                currentHit = hit;
            }

            return isLayerForTeleport;
        }

        private RaycastHit GetHit()
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

    public static class MathStat
    {
        public static int GetPowNumber2(int powVal)
        {
            var result = 0;
            while (powVal > 1)
            {
                if (powVal % 2 == 0)
                {
                    powVal /= 2;
                    result++;
                }
                else
                {
                    throw new ArgumentException("Value must be a power of 2.");
                }
            }
            return result;
        }
    }
}