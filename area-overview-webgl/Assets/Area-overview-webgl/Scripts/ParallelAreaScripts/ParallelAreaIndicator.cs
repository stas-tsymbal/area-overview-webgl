using UnityEngine;
using UnityEngine.UI;

namespace Area_overview_webgl.Scripts.ParallelAreaScripts
{
    /**
     * Draw "additional" cursor under mouse cursor
     * This "additional" cursor drawing parallel to object that raycast hit from mouse
     */
    public class ParallelAreaIndicator : MonoBehaviour
    {
        public static ParallelAreaIndicator Instance;

        [Header("Cursor animation duration on turn off/on")] [SerializeField]
        private float cursorAnimationTime = 0.1f;

        [Range(0, 1)] 
        [SerializeField] private float maxIndicatorAlphaColor = 0.4f;

        [Header("Ignore parallel indicator for this layer")] [SerializeField]
        private LayerMask ignoreLayer; // ignore raycast for this layer

        [SerializeField] private Camera myCamera; // make ray from this camera

        [Space] [SerializeField]
        private Transform cursorIndicator; // this object we set when ray collide with some area 

        [SerializeField] private Image indicatorImg;

        private bool isIndicatorDisable;


        public void Init(Camera myCamera)
        {
            this.myCamera = myCamera;
        }

        private void FixedUpdate()
        {
            if (isIndicatorDisable) return;

            DrawParallelAreaIndicator();
        }


        // Draw parallel area indicator
        private void DrawParallelAreaIndicator()
        {
            RaycastHit hit;
            var ray = myCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreLayer))
            {
                cursorIndicator.position =
                    hit.point + hit.normal / 100; // set cursor indicator over hit point(height of cursor indicator);
                // '/100' - use for correct distance from hit point to cursorIndicator(bigger value -> less distance) 
                cursorIndicator.LookAt(hit.point);

                // DEBUG DRAW RAYCAST
                // Debug.DrawRay(hit.point, camera.transform.position, Color.green); // ray from camera
                // Debug.DrawLine(vectorNormal, hit.point, Color.red); // normal ray from object
            }
            else
            {
                // Disable cursor
                indicatorImg.color = new Color32(255, 255, 255, 0);
            }
        }

        #region EnablingDisablingCursor

        // On/off indicator , true - enable indicator, false - disable
        public void SetIndicatorMaxColor(bool _val)
        {
            if (_val)
            {
                // Enable image
                if (isIndicatorDisable)
                    LeanTween.value(gameObject, UpdateImageAlpha, 0f, maxIndicatorAlphaColor, cursorAnimationTime)
                        .setOnStart(() => SetStateIndicator(false));
            }
            else
            {
                // Disable image
                if (isIndicatorDisable) return;
                LeanTween.value(gameObject, UpdateImageAlpha, maxIndicatorAlphaColor, 0f, cursorAnimationTime)
                    .setOnComplete(() => SetStateIndicator(true));
            }
        }

        void SetStateIndicator(bool state)
        {
            isIndicatorDisable = state;
        }

        void UpdateImageAlpha(float alpha)
        {
            indicatorImg.color = new Color(indicatorImg.color.r, indicatorImg.color.g, indicatorImg.color.b, alpha);
        }

        #endregion
    }
}