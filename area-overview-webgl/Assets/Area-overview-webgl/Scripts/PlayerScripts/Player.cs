using System;
using Area_overview_webgl.Scripts.Controllers;
using Area_overview_webgl.Scripts.Interfaces;
using Area_overview_webgl.Scripts.UIScripts;
using UnityEngine;

namespace Area_overview_webgl.Scripts.PlayerScripts
{
    public class Player : MonoBehaviour, IMove
    {
        [SerializeField] private MovingController movingController;
        [SerializeField] private PlayerMoving playerMoving;
        [SerializeField] private ClickController clickController;
        
        [Header("PlayerBody")]
        [SerializeField] private PlayerBody playerBody;
        
        

        public void Init(GamePlatform currentGamePlatform, UIController uiController)
        {
            movingController.Init(this, currentGamePlatform, uiController);
        }

        public PlayerBody GetPlayerBody()
        {
            return playerBody;
        }

        public void MoveForward()
        {
            Debug.Log("Move forward");
        }

        public void MoveBack()
        {
            Debug.Log("Move back");
        }

        public void MoveLeft()
        {
            Debug.Log("Move left");
        }

        public void MoveRight()
        {
            Debug.Log("Move right");
        }

        public void BoostSpeed()
        {
            Debug.Log("Boost");
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