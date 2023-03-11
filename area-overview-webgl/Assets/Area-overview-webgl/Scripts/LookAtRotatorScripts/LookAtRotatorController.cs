using Area_overview_webgl.Scripts.Static;
using UnityEngine;

namespace Area_overview_webgl.Scripts.LookAtRotatorScripts
{
    /**
     * This script controls LookAtRotator mechanics
     * Raycast need to hit object in special rotation layer for start rotation  (LayerMask lookAtLayer)
     */
    public class LookAtRotatorController : MonoBehaviour
    {
        [SerializeField] private LookAtRotator lookAtRotator;

        [Header("Rotates to an object with this layer")] [SerializeField]
        private LayerMask lookAtLayer;

        private Camera myCamera;

        public void Init(Camera myCamera, Transform playerHead, Transform playerBody)
        {
            this.myCamera = myCamera;
            lookAtRotator.Init(playerHead, playerBody);
        }

        // Start rotation if hit object with lookAtLayer for rotation
        public void TryRotateToObject(Vector3 inputCursorPosition)
        {
            RaycastHit hit;
            var ray = myCamera.ScreenPointToRay(inputCursorPosition); // make ray from position
            if (Physics.Raycast(ray, out hit))
            {
                var hitObject = hit.transform.gameObject; // ray hit this object
                if (hitObject.layer == Calculator.GetPowNumber2(lookAtLayer.value))
                {
                    lookAtRotator.LookAtPoint(hit.point);
                }
                else
                {
                    Debug.Log($"You don't hit required layer {Calculator.GetPowNumber2(lookAtLayer.value)}");
                }
            }
        }

        // Stop look at action 
        public void StopLookAtRotation()
        {
            lookAtRotator.StopLookAtRotation();
        }
    }
}