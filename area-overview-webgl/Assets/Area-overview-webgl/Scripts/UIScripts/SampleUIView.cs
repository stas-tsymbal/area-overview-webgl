using UnityEngine;

namespace Area_overview_webgl.Scripts.UIScripts
{
    public class SampleUIView : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        
    }
}