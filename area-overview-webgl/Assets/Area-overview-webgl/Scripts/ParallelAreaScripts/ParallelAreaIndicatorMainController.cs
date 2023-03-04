using System;
using Area_overview_webgl.Scripts.Controllers;
using UnityEngine;

namespace Area_overview_webgl.Scripts.ParallelAreaScripts
{
    public class ParallelAreaIndicatorMainController : MonoBehaviour
    {
        [SerializeField] private ParallelAreaIndicator parallelAreaIndicator;
        [SerializeField] private ParallelAreaIndicatorActivationController activationController;
        public void Init(GamePlatform currentGamePlatform)
        {
            switch (currentGamePlatform)
            {
                case GamePlatform.mobile: DestroyIndicator();
                    break;
                case GamePlatform.PC: InitPC();
                    break;
                default:
                    throw new ArgumentException($"Check GamePlatform enum for this value {currentGamePlatform}");
            }
        }

        private void DestroyIndicator()
        {
            
        }

        private void InitPC()
        {
            
        }
    }
}