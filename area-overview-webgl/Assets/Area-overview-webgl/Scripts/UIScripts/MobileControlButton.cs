using System;
using UnityEngine;

namespace Area_overview_webgl.Scripts.UIScripts
{
    public class MobileControlButton : SampleUIView
    {
        [SerializeField] private SampleEventTrigger forwardButton;
        [SerializeField] private SampleEventTrigger backButton;
        
        // Forward Button
        public Action OnPointerEnterForwardButton;
        public Action OnPointerExitForwardButton;
        public Action OnPointerDownForwardButton;
        public Action OnPointerUpForwardButton;
        
        // Back Button
        public Action OnPointerEnterBackButton;
        public Action OnPointerExitBackButton;
        public Action OnPointerDownBackButton;
        public Action OnPointerUpBackButton;
        
        public void Init()
        {
            AddEventsOnButton();
        }

        // Init and subscribe on events
        private void AddEventsOnButton()
        {
            forwardButton.InitControlButton();
            forwardButton.OnEnter += () => { OnPointerEnterForwardButton?.Invoke(); };
            forwardButton.OnExit += () => { OnPointerExitForwardButton?.Invoke(); };
            forwardButton.OnDown += () => { OnPointerDownForwardButton?.Invoke(); };
            forwardButton.OnUp += () => { OnPointerUpForwardButton?.Invoke(); };
            
            backButton.InitControlButton();
            backButton.OnEnter += () => { OnPointerEnterBackButton?.Invoke(); };
            backButton.OnExit += () => { OnPointerExitBackButton?.Invoke(); };
            backButton.OnDown += () => { OnPointerDownBackButton?.Invoke(); };
            backButton.OnUp += () => { OnPointerUpBackButton?.Invoke(); };
        }

        private void OnDestroy()
        {
            forwardButton.OnEnter -= () => { OnPointerEnterForwardButton?.Invoke(); };
            forwardButton.OnExit -= () => { OnPointerExitForwardButton?.Invoke(); };
            forwardButton.OnDown -= () => { OnPointerDownForwardButton?.Invoke(); };
            forwardButton.OnUp -= () => { OnPointerUpForwardButton?.Invoke(); };

            backButton.OnEnter -= () => { OnPointerEnterBackButton?.Invoke(); };
            backButton.OnExit -= () => { OnPointerExitBackButton?.Invoke(); };
            backButton.OnDown -= () => { OnPointerDownBackButton?.Invoke(); };
            backButton.OnUp -= () => { OnPointerUpBackButton?.Invoke(); };
        }
        
        
    }
}