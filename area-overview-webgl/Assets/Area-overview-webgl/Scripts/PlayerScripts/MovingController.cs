using System;
using Area_overview_webgl.Scripts.Controllers;
using Area_overview_webgl.Scripts.Interfaces;
using Area_overview_webgl.Scripts.UIScripts;
using UnityEngine;

namespace Area_overview_webgl.Scripts.PlayerScripts
{
    public class MovingController : MonoBehaviour
    { 
        IMove player;
        private GamePlatform currentGamePlatform;
        
        [Header("Moving")]
       [SerializeField] private KeyCode moveForwardKey = KeyCode.W;
       [SerializeField] private KeyCode moveBackSpeedKey = KeyCode.S;
       [SerializeField] private KeyCode moveRightSpeedKey = KeyCode.A;
       [SerializeField] private KeyCode moveLeftSpeedKey = KeyCode.D;
        
       [Header("Boost moving speed")]
       [SerializeField] private KeyCode boostMovingSpeedKey = KeyCode.LeftShift;

       public void Init(IMove player, GamePlatform currentGamePlatform, UIController uiController)
       {
           this.player = player;
           this.currentGamePlatform = currentGamePlatform;

           switch (currentGamePlatform)
           {
               case GamePlatform.PC:
                   break;
               case GamePlatform.mobile: InitMobile(uiController);
                   break;
           }
       }
       

       private void Update()
        {
            switch (currentGamePlatform)
            {
                case GamePlatform.PC: DetectMovingPC();
                    break;
                case GamePlatform.mobile: DetectMovingMobile();;
                    break;
            }
            
        }

       

       #region PC
        private void DetectMovingPC()
        {
            
            if (Input.GetKey(boostMovingSpeedKey))
            {
                player.BoostSpeed();
            }

            if (Input.GetKey(moveForwardKey)) // move forward
            {
                player.MoveForward();
            }

            if (Input.GetKey(moveBackSpeedKey)) // move back
            {
                player.MoveBack();
            }

            if (Input.GetKey(moveRightSpeedKey)) // move left
            {
                player.MoveRight();
            }

            if (Input.GetKey(moveLeftSpeedKey)) // move right
            {
                player.MoveLeft();
            }
            
            
        }

        #endregion

        #region Mobile
        
        private bool mobileForward;
        private bool mobileBack;
        private MobileControlButton mobileMovingButtons;
        
        private void DetectMovingMobile()
        {
            if (mobileForward) // move forward
            {
                player.MoveForward();
            }

            if (mobileBack) // move back
            {
                player.MoveBack();
            }
        }
        
        private void InitMobile( UIController uiController)
        {
            mobileMovingButtons = uiController.GetMobileMovingButtons();

            mobileMovingButtons.OnPointerEnterForwardButton += ActivateMobileForwardMoving;
            mobileMovingButtons.OnPointerExitForwardButton += DeactivateMobileForwardMoving;
            
            mobileMovingButtons.OnPointerEnterBackButton += ActivateMobileBackMoving;
            mobileMovingButtons.OnPointerExitBackButton += DeactivateMobileBackMoving;
            
        }

        private void ActivateMobileForwardMoving()
        {
            SetStateMoveForward(true);
        }

        private void DeactivateMobileForwardMoving()
        {
            SetStateMoveForward(false);
        }

        private void ActivateMobileBackMoving()
        {
            SetStateMoveBack(true);
        }

        private void DeactivateMobileBackMoving()
        {
            SetStateMoveBack(false);
        }
        
        void SetStateMoveForward(bool val)
        {
            mobileForward = val;
        }

        void SetStateMoveBack(bool val)
        {
            mobileBack = val;
        }
        

        #endregion

        private void OnDestroy()
        {
            if (currentGamePlatform == GamePlatform.mobile)
            {
                mobileMovingButtons.OnPointerEnterForwardButton -= ActivateMobileForwardMoving;
                mobileMovingButtons.OnPointerExitForwardButton -= DeactivateMobileForwardMoving;
            
                mobileMovingButtons.OnPointerEnterForwardButton -= ActivateMobileBackMoving;
                mobileMovingButtons.OnPointerExitForwardButton -= DeactivateMobileBackMoving;
            }
        }
    }
}