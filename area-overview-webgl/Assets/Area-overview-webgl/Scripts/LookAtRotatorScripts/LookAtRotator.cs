﻿using Area_overview_webgl.Scripts.Static;
using UnityEngine;
using UnityEngine.Serialization;

namespace Area_overview_webgl.Scripts.LookAtRotatorScripts
{
    public class LookAtRotator : MonoBehaviour
    {
        [Header("Transform that need to be rotated, setup from init")] 
        [SerializeField] private Transform playerHead; // head
        [SerializeField] private Transform playerBody; // body
        
        [Header("Rotator axis vertical-Y and horizontal-X")] 
        [SerializeField] private Transform lookAtBody;
        [SerializeField] private Transform lookAtHead;

        [Header("Rotation speed")] [SerializeField]
        private float rotationSpeed = 1f;

        private bool rotationIsActive = false;
        private Vector3 lookAtPosition;
        private Vector3 lookAtRotation;
        
        public void Init(Transform playerHead, Transform playerBody)
        {
            this.playerHead = playerHead;
            this.playerBody = playerBody;
            
            lookAtPosition = transform.position;
            lookAtRotation = transform.eulerAngles;
        }

        //Rotate camera to object
        public void LookAtPoint(Vector3 rotateToThisPoint)
        {
            //start camera rotation towards the target
            lookAtBody.position = playerBody.position;
            lookAtHead.position = playerHead.position;

            lookAtBody.LookAt(rotateToThisPoint);
            lookAtBody.eulerAngles = new Vector3(0f, lookAtBody.eulerAngles.y, 0f);

            lookAtHead.LookAt(rotateToThisPoint);
            lookAtHead.localEulerAngles = new Vector3(lookAtHead.localEulerAngles.x, 0f, 0f);

            rotationIsActive = true;
        }

        private void LateUpdate()
        {
            if (!rotationIsActive)
                return;

            KeepRotatorStartPosition();
            
            //try to lerp camera rotation towards the target
            playerBody.rotation = Quaternion.Lerp(playerBody.rotation, lookAtBody.rotation,
                Time.deltaTime * rotationSpeed);
            playerBody.eulerAngles = new Vector3(0f, playerBody.eulerAngles.y, 0f);

            playerHead.rotation = Quaternion.Lerp(playerHead.rotation,
                lookAtHead.rotation, Time.deltaTime * rotationSpeed);
            playerHead.localEulerAngles = new Vector3(playerHead.localEulerAngles.x, 0f, 0f);
        }

        private void KeepRotatorStartPosition()
        {
           transform.position = lookAtPosition;
           transform.eulerAngles = lookAtRotation;
        }

        //Stop look at action after player's activity
        public void StopLookAtRotation()
        {
            if(rotationIsActive) rotationIsActive = false;
        }
    }
}