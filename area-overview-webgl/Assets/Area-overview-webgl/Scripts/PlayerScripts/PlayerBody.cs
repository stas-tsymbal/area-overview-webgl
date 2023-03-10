using System;
using UnityEngine;

namespace Area_overview_webgl.Scripts.PlayerScripts
{
    /**
     * Script consist information about part of player body
     */
    [Serializable]
    public class PlayerBody
    {
        [SerializeField] private Transform head;
        [SerializeField] private Transform body;
        [SerializeField] private CapsuleCollider collider;
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private Transform firstCamPosition;
        [SerializeField] private Transform cameraYPosition;

        public Transform GetHead()
        {
            return head;
        }

        public Transform GetBody()
        {
            return body;
        }
        public Transform GetFirstCamPosition()
        {
            return firstCamPosition;
        }
        
        public Transform GetCameraYPosition()
        {
            return cameraYPosition;
        }

        public CapsuleCollider GetCapsuleCollider()
        {
            return collider;
        }

        public Rigidbody GetRigidbody()
        {
            return rigidbody;
        }

        public void SetKinematicStateForRigidbody(bool val)
        {
            GetRigidbody().isKinematic = val;
        }
    }
}