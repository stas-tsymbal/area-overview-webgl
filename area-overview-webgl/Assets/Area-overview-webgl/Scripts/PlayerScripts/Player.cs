using System;
using System.Collections;
using Area_overview_webgl.Scripts.Controllers;
using Area_overview_webgl.Scripts.Interfaces;
using Area_overview_webgl.Scripts.UIScripts;
using UnityEngine;

namespace Area_overview_webgl.Scripts.PlayerScripts
{
    public class Player : MonoBehaviour, IMove
    {
        [SerializeField] private MovingController movingController;
        [SerializeField] private ClickController clickController;
        
        [Header("PlayerBody")]
        [SerializeField] private PlayerBody playerBody;
        
        [Header("Camera Moving")] [SerializeField]
        private float movingForceSpeed = 10f; // use for correct camera body speed 
        
        [SerializeField] private float boostSpeed = 5f; // speed when press shift
        private bool isBoost;
        
        
        public void Init(GamePlatform currentGamePlatform, UIController uiController)
        {
            movingController.Init(this, currentGamePlatform, uiController);
        }

        public PlayerBody GetPlayerBody()
        {
            return playerBody;
        }

        #region PlayerMoving
        public void MoveForward()
        {
            Debug.Log("Move forward");
            ApplyForceToTheBody( GetPlayerBody().GetHead().forward);
        }

        public void MoveBack()
        {
            Debug.Log("Move back");
            ApplyForceToTheBody(-GetPlayerBody().GetHead().forward);
        }

        public void MoveLeft()
        {
            Debug.Log("Move left");
            ApplyForceToTheBody(-GetPlayerBody().GetHead().right);
        }

        public void MoveRight()
        {
            Debug.Log("Move right");
            ApplyForceToTheBody(GetPlayerBody().GetHead().right);
        }

        public void BoostSpeed(bool val)
        {
            Debug.Log($"Boost {val}");
            isBoost = val;
        }
        
        private void ApplyForceToTheBody(Vector3 forceHeading)
        {
            
            var forceForMoving = GetForceForMoving(forceHeading);
            var finalForce = forceForMoving * movingForceSpeed;
           
            SetKinematicStateForRigidbody(false);
            GetPlayerBody().GetRigidbody().AddForce(finalForce);
            
            // LookAtRotatorController.Insctance.StopLookAtRotation();
            StopCoroutine(freezingRigidbodyCor);
            freezingRigidbodyCor = StartCoroutine(FreezingRigidbody()); // try freez RB  
        }

        private Coroutine freezingRigidbodyCor;
        private IEnumerator FreezingRigidbody()
        {
            yield return new WaitForSeconds(0.1f);
            SetKinematicStateForRigidbody(true);
        }

        private Vector3 GetForceForMoving(Vector3 forceHeading)
        {
            var movingForce = Time.fixedDeltaTime * forceHeading;
            return isBoost ? movingForce * boostSpeed : movingForce ;
        }
        
        private void SetKinematicStateForRigidbody(bool val)
        {
            GetPlayerBody().SetKinematicStateForRigidbody(val);
        }
        

        #endregion
       

        
    }

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
           return head;
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