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
        private enum DefaultMode
        {
            walk,
            orbit
        }
        [SerializeField] private DefaultMode defaultMode;
        [Space]
        [SerializeField] private Image walk;
        [SerializeField] private Image orbit;
        [SerializeField] private Color32 standardColor;
        [SerializeField] private Color32 pressedColor;
    
        [Space]
        [SerializeField] private Image walkBorder;
        [SerializeField] private Image orbitBorder;
        [SerializeField] private Color32 standardColorBorder;
        [SerializeField] private Color32 pressedColorBorder;

        [SerializeField] private SampleEventTrigger firstPersonEventTrigger;
        [SerializeField] private SampleEventTrigger orbitalEventTrigger;
        
        public Action OnFirstPersonModeClick;
        public Action OnOrbitalModeClick;


        public void Init(CameraMode startCameraMode)
        {
            AddEventsOnButton();
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
            PaintFirstPersonColorButton();
            OnFirstPersonModeClick?.Invoke();
        }
        
        private void ClickOrbitalButton()
        {
            PaintOrbitalColorButton();
            OnOrbitalModeClick?.Invoke();
        }
        
        
        // Change indicator color for first person button -> active, orbital - inactive
        public void PaintFirstPersonColorButton()
        {
            walk.color = pressedColor;
            orbit.color = standardColor;
        
            walkBorder.color = pressedColorBorder;
            orbitBorder.color = standardColorBorder;
        }

        // Change indicator color for orbital -> active, first person button - inactive
        public void PaintOrbitalColorButton()
        {
            orbit.color = pressedColor;
            walk.color = standardColor;
        
            orbitBorder.color = pressedColorBorder;
            walkBorder.color = standardColorBorder;
        }

        private void OnDestroy()
        {
            firstPersonEventTrigger.OnClick -= ClickFirstPersonButton;
            orbitalEventTrigger.OnClick -= ClickOrbitalButton;
        }
    }
}

