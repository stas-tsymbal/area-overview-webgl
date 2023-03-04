using Area_overview_webgl.Scripts.Static;
using Area_overview_webgl.Scripts.Teleport;
using UnityEngine;
using UnityEngine.Serialization;

namespace Area_overview_webgl.Scripts.LookAtRotatorScripts
{
    public class LookAtRotator : MonoBehaviour
    {
        [Header("Transform that need to be rotated, setup from init")] 
        [SerializeField] private Transform playerHorizontalAxis;
        [SerializeField] private Transform playerVerticalAxis;
        
        [Header("Rotator axis vertical-Y and horizontal-X")] 
        [SerializeField] private Transform lookAtVerticalAxis;
        [SerializeField] private Transform lookAtHorizontalAxis;

        [Header("Rotation speed")] [SerializeField]
        private float rotationSpeed = 1f;

        private bool rotationIsActive = false;

        
        public void Init(Transform playerHorizontalAxis, Transform playerVerticalAxis)
        {
            this.playerHorizontalAxis = playerHorizontalAxis;
            this.playerVerticalAxis = playerVerticalAxis;
           
        }

        //Rotate camera to object
        public void LookAtPoint(Vector3 rotateToThisPoint)
        {
            //start camera rotation towards the target
            lookAtVerticalAxis.position = playerVerticalAxis.position;
            lookAtHorizontalAxis.position = playerHorizontalAxis.position;

            lookAtVerticalAxis.LookAt(rotateToThisPoint);
            lookAtVerticalAxis.eulerAngles = new Vector3(0f, lookAtVerticalAxis.eulerAngles.y, 0f);

            lookAtHorizontalAxis.LookAt(rotateToThisPoint);
            lookAtHorizontalAxis.localEulerAngles =
                new Vector3(lookAtHorizontalAxis.localEulerAngles.x, 0f, 0f);

            rotationIsActive = true;
        }

        private void LateUpdate()
        {
            if (!rotationIsActive)
                return;

            //try to lerp camera rotation towards the target
            playerVerticalAxis.rotation = Quaternion.Lerp(playerVerticalAxis.rotation, lookAtVerticalAxis.rotation,
                Time.deltaTime * rotationSpeed);
            playerVerticalAxis.eulerAngles = new Vector3(0f, playerVerticalAxis.eulerAngles.y, 0f);

            playerHorizontalAxis.rotation = Quaternion.Lerp(playerHorizontalAxis.rotation,
                lookAtHorizontalAxis.rotation, Time.deltaTime * rotationSpeed);
            playerHorizontalAxis.localEulerAngles = new Vector3(playerHorizontalAxis.localEulerAngles.x, 0f, 0f);
        }

        //Stop look at action after player's activity
        public void StopLookAtRotation()
        {
            rotationIsActive = false;
        }
    }
}