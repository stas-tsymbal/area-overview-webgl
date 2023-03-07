using System;
using UnityEngine;

namespace Area_overview_webgl.Scripts.PlayerScripts
{
    [Serializable]
    public class PlayerBody 
    {
        [SerializeField] private Transform head;
        [SerializeField] private Transform body;
        [SerializeField] private CapsuleCollider collider;
        [SerializeField] private Rigidbody rigidbody;
       
        public Transform GetHead()
        {
            return head;
        }
       
        public Transform GetBody()
        {
            return body;
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