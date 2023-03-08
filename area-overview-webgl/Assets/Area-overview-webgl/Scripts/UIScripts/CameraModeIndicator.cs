using System;
using Area_overview_webgl.Scripts.CameraModeScripts;
using Area_overview_webgl.Scripts.Controllers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace Area_overview_webgl.Scripts.UIScripts
{
    /**
 * Paint image on click
 * Indicate current camera mode in UI 
 */
    public class CameraModeIndicator : SampleUIView
    {
        [Header("Button background")]
        [SerializeField] private Image firstPerson;
        [SerializeField] private Image orbit;
        [SerializeField] private Color32 standardColor;
        [SerializeField] private Color32 pressedColor;
    
        [Header("Button icon")]
        [SerializeField] private Image firstPersonIcon;
        [SerializeField] private Image orbitIcon;
        [SerializeField] private Color32 standardColorBorder;
        [SerializeField] private Color32 pressedColorBorder;

        [Header("Buttons")]
        [SerializeField] private SampleEventTrigger firstPersonEventTrigger;
        [SerializeField] private SampleEventTrigger orbitalEventTrigger;
        
        public Action OnFirstPersonModeClick;
        public Action OnOrbitalModeClick;

        private CameraModeController cameraModeController;
        public void Init(CameraMode startCameraMode, CameraModeController cameraModeController)
        {
            AddEventsOnButton();
            PaintButtonOnStart(startCameraMode);
            this.cameraModeController = cameraModeController;
            cameraModeController.OnCameraModeChange += OnChangeCameraMode;
        }

       private void PaintButtonOnStart(CameraMode startCameraMode)
        {
            switch (startCameraMode)
            {
                case CameraMode.firstPerson: PaintFirstPersonColorButton();
                    break;
                case CameraMode.orbital: PaintOrbitalColorButton();
                    break;
                default:
                    throw new ArgumentException($"Check CameraMode enum for this value {startCameraMode}");
            }
        }

        // Add and subscribe on events
        private void AddEventsOnButton()
        {
            firstPersonEventTrigger.InitClick();
            firstPersonEventTrigger.OnClick += ClickFirstPersonButton;
            
            orbitalEventTrigger.InitClick();
            orbitalEventTrigger.OnClick += ClickOrbitalButton;
        }
        
        private void ClickFirstPersonButton()
        {
           
            OnFirstPersonModeClick?.Invoke();
        }
        
        private void ClickOrbitalButton()
        {
            
            OnOrbitalModeClick?.Invoke();
        }

        public void OnChangeCameraMode(CameraMode cameraMode)
        {
            switch (cameraMode)
            {
                case CameraMode.orbital: PaintOrbitalColorButton();;
                    break;
                case CameraMode.firstPerson:  PaintFirstPersonColorButton();; 
                    break;
                default:
                    throw new ArgumentException($"Check CameraMode enum for this value {cameraMode}");
            }
        }
        
        // Change indicator color for first person button -> active, orbital - inactive
        private void PaintFirstPersonColorButton()
        {
            firstPerson.color = pressedColor;
            orbit.color = standardColor;
        
            firstPersonIcon.color = pressedColorBorder;
            orbitIcon.color = standardColorBorder;
        }

        // Change indicator color for orbital -> active, first person button - inactive
        private void PaintOrbitalColorButton()
        {
            firstPerson.color = standardColor;
            orbit.color = pressedColor;
            
            firstPersonIcon.color = standardColorBorder;
            orbitIcon.color = pressedColorBorder;
        }

        private void OnDestroy()
        {
            firstPersonEventTrigger.OnClick -= ClickFirstPersonButton;
            orbitalEventTrigger.OnClick -= ClickOrbitalButton;
        }
    }
}

