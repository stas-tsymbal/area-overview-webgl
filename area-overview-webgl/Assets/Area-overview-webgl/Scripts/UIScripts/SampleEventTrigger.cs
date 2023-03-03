using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Area_overview_webgl.Scripts.UIScripts
{
    public class SampleEventTrigger : MonoBehaviour
    {
        public Action OnClick;
        public Action OnEnter;
        public Action OnExit;
        public Action OnDown;
        public Action OnUp;
        
        // add on click for button
        public void InitClick()
        {
            var eventTrigger = gameObject.AddComponent<EventTrigger>();
            
            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            
            entry.callback.AddListener((data) => { CLickEventTrigger(); });
            
            eventTrigger.triggers.Add(entry);
        }

        public void InitControlButton()
        {
            var eventTrigger = gameObject.AddComponent<EventTrigger>();

            // Pointer Enter Event
            var pointerEnter = new EventTrigger.Entry();
            pointerEnter.eventID = EventTriggerType.PointerEnter;
            pointerEnter.callback.AddListener((data) => { PointerEnterEventTrigger(); });
            eventTrigger.triggers.Add(pointerEnter);

            // Pointer Exit Event
            var pointerExit = new EventTrigger.Entry();
            pointerExit.eventID = EventTriggerType.PointerExit;
            pointerExit.callback.AddListener((data) => { PointerExitEventTrigger(); });
            eventTrigger.triggers.Add(pointerExit);

            // Pointer Down Event
            var pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((data) => { PointerDownEventTrigger(); });
            eventTrigger.triggers.Add(pointerDown);

            // Pointer Up Event
            var pointerUp = new EventTrigger.Entry();
            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((data) => { PointerUpEventTrigger(); });
            eventTrigger.triggers.Add(pointerUp);
        }

        // Pointer Enter
        private void PointerEnterEventTrigger()
        {
            OnEnter?.Invoke();
        }

        // Pointer Exit
        private void PointerExitEventTrigger()
        {
            OnExit?.Invoke();
        }

        // Pointer Down
        private void PointerDownEventTrigger()
        {
            OnDown?.Invoke();
        }

        // Pointer Up
        private void PointerUpEventTrigger()
        {
            OnUp?.Invoke();
        }
        
        //Button Clicked
        private void CLickEventTrigger()
        {
            OnClick?.Invoke();
        }

        
    }
}