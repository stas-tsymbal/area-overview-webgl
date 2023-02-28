using System;
using UnityEngine;

namespace _4_timers.Scripts
{
    public class TimeInvoker : MonoBehaviour
    {
        public event Action<float> OnUpdateTimeTickedEvent;
        public event Action<float> OnUpdateTimeUnscaleTickedEvent;
        public event Action OnOneSecondTickedEvent;
        public event Action OnOneSecondUnscaledTickedEvent;

        public static TimeInvoker Instance
        {
            get
            {
                if (instance != null) return instance;
                var obj = new GameObject("[TIME INVOKER]");
                instance = obj.AddComponent<TimeInvoker>();
                DontDestroyOnLoad(obj);
                return instance;
            }
        }

        private static TimeInvoker instance;

        private float oneSecTimer;
        private float oneSecUnscaleTimer;

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            OnUpdateTimeTickedEvent?.Invoke(deltaTime);

            oneSecTimer += deltaTime;
            if (oneSecTimer >= 1f)
            {
                oneSecTimer -= 1;
                OnOneSecondTickedEvent?.Invoke();
            }

            var unscaledDeltaTime = Time.unscaledDeltaTime;
            OnUpdateTimeUnscaleTickedEvent?.Invoke(unscaledDeltaTime);

            oneSecUnscaleTimer += unscaledDeltaTime;
            if (oneSecUnscaleTimer >= 1f)
            {
                oneSecUnscaleTimer -= 1;
                OnOneSecondUnscaledTickedEvent?.Invoke();
            }
        }
    }
}
