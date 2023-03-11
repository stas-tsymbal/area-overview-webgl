using System;
using System.Collections;
using Area_overview_webgl.Scripts.CameraModeScripts;
using UnityEngine;

namespace Area_overview_webgl.Scripts.PlayerScripts
{
    /**
     * This script provide switching camera mode by moving camera
     * from first person mode to orbital mode
     */
    public class PlayerCameraModeSwitcher : MonoBehaviour
    {
        [SerializeField] private CameraMode currentCameraMode;

        [Header("Camera Moving")] 
        [SerializeField] private Transform orbitalPosition;
        [SerializeField] private Transform orbitalCameraParent; // parent for orbital camera
        
        [Space]
        [SerializeField] private CapsuleCollider firstPersonCameraParent; // parent for first person camera
        [SerializeField] private Transform myCamera;

        [Header("Lerp changing mode")] [SerializeField]
        private float lerpSpeed = 10f;
        [SerializeField] private float lerpMinValue = 0.1f;

        [Header("Last first person position")] [SerializeField]
        private Transform lastFPPosition;

        [Header("First person angle on ground")]   
        [SerializeField]
        private float firstPersonStandardAngle = 0; // apply for cam x

        private Coroutine moveCor;

        [SerializeField] private Transform rayYHeight; // height of ray for check teleport

        public Action OnCameraModeChanged; // call when mode is changed and camera finished moving

        public void Init( CapsuleCollider firstPersonCameraParent, Transform myCamera, Transform orbitalPosition,
            Transform orbitalCameraParent, Transform lastFPPosition, Transform rayYHeight)
        {
            this.firstPersonCameraParent = firstPersonCameraParent;
            this.myCamera = myCamera;
            this.orbitalPosition = orbitalPosition;
            this.orbitalCameraParent = orbitalCameraParent;
            this.lastFPPosition = lastFPPosition;
            this.rayYHeight = rayYHeight;
        }

        // set orbital mode, call from UI
        public void SetOrbitalMode()
        {
            SetCameraMode(CameraMode.orbital); // set camera mode
            RememberFirstPersonPosition();
            myCamera.SetParent(orbitalCameraParent);

            // lerp camera
            if(moveCor != null) StopCoroutine(moveCor);
            moveCor = StartCoroutine(LerpOrbitalMode());
        }

        // disable camera rotation and moving scripts while lerp 
        private IEnumerator LerpOrbitalMode()
        {
            while (Vector3.Distance(myCamera.position, orbitalPosition.position) > lerpMinValue)
            {
                myCamera.position =
                    Vector3.Lerp(myCamera.position, orbitalPosition.position, Time.deltaTime * lerpSpeed);
                myCamera.rotation =
                    Quaternion.Lerp(myCamera.rotation, orbitalPosition.rotation, Time.deltaTime * lerpSpeed);
                yield return null;
            }

            myCamera.position = orbitalPosition.position;
            moveCor = null;
            OnCameraModeChanged?.Invoke();
        }

        // Remember first person position
        private void RememberFirstPersonPosition()
        {
            lastFPPosition.position = rayYHeight.position;
            lastFPPosition.eulerAngles = myCamera.eulerAngles;
        }

        // set first person mode, call from UI
        public void SetFirstPersonMode()
        {
            SetCameraMode(CameraMode.firstPerson);
            myCamera.SetParent(firstPersonCameraParent.transform);
            // lerp moving 
            if(moveCor != null) StopCoroutine(moveCor);
            moveCor = StartCoroutine(LerpForFirsPersonMode());
        }

        // disable camera rotation and moving scripts while lerp 
        private IEnumerator LerpForFirsPersonMode()
        {
            // set X axis for first person cam -> lastFPPosition.eulerAngles(NEW_value, OLD, OLD)
            var eulerAngles = lastFPPosition.eulerAngles;
            eulerAngles = new Vector3(firstPersonStandardAngle, eulerAngles.y, eulerAngles.z);
            lastFPPosition.eulerAngles = eulerAngles;

            // Lerp camera position and rotation from ORBITAL MODE to FIRST PERSON 
            while (Vector3.Distance(myCamera.position, lastFPPosition.position) > lerpMinValue)
            {
                myCamera.position =
                    Vector3.Lerp(myCamera.position, lastFPPosition.position, Time.deltaTime * lerpSpeed);
                myCamera.rotation =
                    Quaternion.Lerp(myCamera.rotation, lastFPPosition.rotation, Time.deltaTime * lerpSpeed);
                yield return null;
            }

            myCamera.position = lastFPPosition.position;
            moveCor = null;
            OnCameraModeChanged?.Invoke();
        }

        // set camera mode
        private void SetCameraMode(CameraMode _mode)
        {
            currentCameraMode = _mode;
        }
        
        // set new position for last person position gameobject
        private void SetLastPersonPosition(Vector3 _pos)
        {
            firstPersonCameraParent.transform.position =
                new Vector3(_pos.x, _pos.y + firstPersonCameraParent.height / 2, _pos.z);
            lastFPPosition.position = new Vector3(_pos.x, lastFPPosition.position.y, _pos.z);
        }
    }
}