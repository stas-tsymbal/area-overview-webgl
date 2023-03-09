using Area_overview_webgl.Scripts.CameraModeScripts;
using Area_overview_webgl.Scripts.PlayerScripts;
using Area_overview_webgl.Scripts.UIScripts;
using UnityEngine;

namespace Area_overview_webgl.Scripts.Controllers
{
    /**
     * Main controller for this application,
     * Using once on game start for installing  dependencies
     */
    public class GameController : MonoBehaviour
    {
        [Header("Set start camera mode")] 
        [SerializeField] private CameraMode startCameraMode;
        
        [SerializeField] private Camera playerCamera;

        [Header("Camera mode controller")] [SerializeField]
        private CameraModeController cameraModeController;

        [Header("UI")] [SerializeField] private UIController uiController;

        [Header("Player")] [SerializeField] private Player player;

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
            uiController.Init(currentGamePlatform, startCameraMode, cameraModeController);
            cameraModeController.Init(startCameraMode, uiController);
            player.Init(currentGamePlatform, uiController, startCameraMode, cameraModeController, playerCamera);
            
        }
        
    }
}