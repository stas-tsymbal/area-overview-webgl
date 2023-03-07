﻿using System;
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
               // CameraModeController.CameraModeController.Instance.MoveCameraByClick(GetHit().point);
        }
        
        private RaycastHit GetHit()
        {
            return currentHit;
        }
        
        
    }
}