using System;
using Area_overview_webgl.Scripts.CameraModeScripts;
using Area_overview_webgl.Scripts.LookAtRotatorScripts;
using Area_overview_webgl.Scripts.ParallelAreaScripts;
using Area_overview_webgl.Scripts.PlayerScripts;
using UnityEngine;

namespace Area_overview_webgl.Scripts.Controllers
{
    /**
     * Main controller for this application
     */
    public class GameController : MonoBehaviour
    {
        [SerializeField] private CameraMode startCameraMode;
        [SerializeField] private ParallelAreaIndicatorController parallelAreaIndicatorController;
        [SerializeField] private LookAtRotatorController lookAtRotatorController;
        [SerializeField] private CameraModeScripts.CameraModeController cameraModeController; 
        [SerializeField] private Player player;
        [SerializeField] private UiController uiController;
        
        public void Awake()
        {
            StartInit(GetGamePlatform(), startCameraMode);
        }

        // Get current game platform
        private GamePlatform GetGamePlatform()
        {
            return Application.isMobilePlatform ? GamePlatform.mobile : GamePlatform.PC;
        }
        
        // Initialization parameters for start game 
        private void StartInit(GamePlatform currentGamePlatform, CameraMode startCameraMode)
        {
            switch (currentGamePlatform)
            {
                case GamePlatform.mobile: InitMobileSetup(currentGamePlatform, startCameraMode);
                    break;
                case GamePlatform.PC: InitPCSetup(currentGamePlatform, startCameraMode);
                    break;
                default:
                    throw new ArgumentException($"Check GamePlatform enum for this value {currentGamePlatform}");
            }
        }

        #region PC Init
        // Initialization for Mobile
        private void InitPCSetup(GamePlatform currentGamePlatform, CameraMode startCameraMode)
        {
            
        }
        

        #endregion

        #region Mobile Init
        // Initialization for PC
        private void InitMobileSetup(GamePlatform currentGamePlatform, CameraMode startCameraMode)
        {
            
        }
        

        #endregion
       

        
    }
}