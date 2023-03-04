using System;
using Area_overview_webgl.Scripts.Static;
using UnityEngine;

namespace Area_overview_webgl.Scripts.LookAtRotatorScripts
{
    public class LookAtRotatorController : MonoBehaviour
    {
        [SerializeField] private LookAtRotator lookAtRotator;
        
        [Header("Rotates to an object with this layer")] [SerializeField]
        private LayerMask lookAtLayer;
        
        [SerializeField] private Camera myCamera;

        public static LookAtRotatorController Insctance;

        private void Awake()
        {
            Insctance = this;
        }

        public void Init(Camera myCamera, Transform playerHorizontalAxis, Transform playerVerticalAxis)
        {
            this.myCamera = myCamera;
            lookAtRotator.Init(playerHorizontalAxis,playerVerticalAxis);
        }

        public void TryRotateToObject(Vector3 inputCursorPosition)
        {
            Debug.Log("Try look at object");

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

        public void StopLookAtRotation()
        {
            lookAtRotator.StopLookAtRotation();
        }
    }
}