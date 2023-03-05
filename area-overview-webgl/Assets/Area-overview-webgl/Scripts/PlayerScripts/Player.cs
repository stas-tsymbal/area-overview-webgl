using System;
using UnityEngine;

namespace Area_overview_webgl.Scripts.PlayerScripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private MovingController movingController;
        [SerializeField] private PlayerMoving playerMoving;
        [SerializeField] private ClickController clickController;
        
        [Header("PlayerBody")]
        [SerializeField] private PlayerBody playerBody;
        
        

        public void Init()
        {
            
        }

        public PlayerBody GetPlayerBody()
        {
            return playerBody;
        }
    }

    [Serializable]
    public class PlayerBody
    {
       [SerializeField] private Transform head;
       [SerializeField] private Transform body;
       [SerializeField] private CapsuleCollider collider;

       public Transform GetHead()
       {
           return head;
       }
       
       public Transform GetBody()
       {
           return head;
       }
       
       public CapsuleCollider GetCapsuleCollider()
       {
           return collider;
       }
    }
    
    
}