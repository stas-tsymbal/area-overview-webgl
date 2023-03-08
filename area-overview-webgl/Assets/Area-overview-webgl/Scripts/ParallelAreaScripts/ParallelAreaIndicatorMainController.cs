using System;
using Area_overview_webgl.Scripts.Controllers;
using UnityEngine;

namespace Area_overview_webgl.Scripts.ParallelAreaScripts
{
    public class ParallelAreaIndicatorMainController : MonoBehaviour
    {
        [SerializeField] private ParallelAreaIndicator parallelAreaIndicator;
        [SerializeField] private ParallelAreaIndicatorActivationController activationController;

        private GamePlatform currentGamePlatform;
        public void Init(GamePlatform currentGamePlatform, Camera myCamera)
        {
            this.currentGamePlatform = currentGamePlatform;
            switch (this.currentGamePlatform)
            {
                case GamePlatform.mobile: DestroyIndicatorElements();
                    break;
                case GamePlatform.PC: InitPC(myCamera);
                    break;
                default:
                    throw new ArgumentException($"Check GamePlatform enum for this value {currentGamePlatform}");
            }
        }

        public void HideIndicator()
        {
            parallelAreaIndicator.SetIndicatorMaxColor(false);
        }
        
        public void ShowIndicator()
        {
            parallelAreaIndicator.SetIndicatorMaxColor(true);
        }
        
        private void DestroyIndicatorElements()
        {
            Destroy(parallelAreaIndicator.gameObject);
            Destroy(activationController.gameObject);
            Destroy(gameObject);
        }

        private void InitPC(Camera myCamera)
        {
            parallelAreaIndicator.Init(myCamera);
            activationController.Init();
            Subscribe();
        }

        private void OnDestroy()
        {
            if (currentGamePlatform == GamePlatform.PC)
                Unsubscribe();
        }

        private void Subscribe()
        {
            activationController.OnChangeCursorIndicatorState += parallelAreaIndicator.SetIndicatorMaxColor;
        }
        
        private void Unsubscribe()
        {
            activationController.OnChangeCursorIndicatorState -= parallelAreaIndicator.SetIndicatorMaxColor;
        }
    }
}