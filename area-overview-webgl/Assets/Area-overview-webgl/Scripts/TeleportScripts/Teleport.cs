using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Area_overview_webgl.Scripts.TeleportScripts
{
    public class Teleport : MonoBehaviour
    {
        [Header("Set from init")]
        [SerializeField] private Transform firstPersonHead; // cam transform
        [SerializeField] private CapsuleCollider firstPersonCollider; 
        
        [Header("Lerp values, ")]
        [SerializeField] private float distanceForStopTeleporting = 0.1f;
        [SerializeField] private float lertTeleportSpeed = 10f;
        
        
        private Coroutine teleportation = null;
        
        public Action OnEndTeleportation;
        
        
        public void Init(Transform firstPersonHead, CapsuleCollider firstPersonCollider)
        {
            this.firstPersonCollider = firstPersonCollider;
            this.firstPersonHead = firstPersonHead;
        }
        
        public void MakeTeleport(Vector3 _newPos)
        {
            Debug.Log("Start make teleport");
            teleportation = StartCoroutine(MakeTeleportCor(_newPos));
        }

        public void StopTeleportation()
        {
            StopCoroutine(teleportation);
            teleportation = null;
        }
        
        // disable camera rotation and moving scripts while lerp 
        private IEnumerator MakeTeleportCor(Vector3 _newPos)
        {
           // normalDetector.DisableIndicatorForTeleport(true);
            _newPos = new Vector3(_newPos.x,_newPos.y + firstPersonCollider.height/2, _newPos.z);
            // Lerp camera position 
            while (Vector3.Distance(firstPersonCollider.transform.position,_newPos) > distanceForStopTeleporting)
            {
                if (CheckDistanceLimit(_newPos)) // check capsule near wall
                {
                    _newPos = firstPersonCollider.transform.position;
                    //Debug.LogError("stop move");
                }
                firstPersonCollider.transform.position  = Vector3.Lerp( firstPersonCollider.transform.position, _newPos, Time.deltaTime * lertTeleportSpeed);
                yield return null;
            }
            OnEndTeleportation?.Invoke();
           // normalDetector.DisableIndicatorForTeleport(false);
        }

       

        // lerp can't finish if target point near wall, because capsule collider with this obstacles and newer reach target position 
        private bool CheckDistanceLimit(Vector3 _targetPoint)
        {
            var needStop = false;
            // add 2 vector3 for correct Y 
            var playerPosition = new Vector3(firstPersonCollider.transform.position.x, firstPersonHead.position.y,
                firstPersonCollider.transform.position.z);
            var targetPointFixY = new Vector3(_targetPoint.x, firstPersonHead.position.y, _targetPoint.z);

            if (Math.Sqrt((playerPosition - targetPointFixY).sqrMagnitude) < firstPersonCollider.radius + 0.6f)
            {
                RaycastHit hit;
                var ray = new Ray(playerPosition, targetPointFixY-playerPosition);
                if (Physics.Raycast(ray, out hit)) {
                    if (Math.Sqrt((playerPosition - hit.point).sqrMagnitude) <
                        firstPersonCollider.radius + 0.5f)
                        needStop = true;
                }
            }
            return needStop;
        }
       
        
    }
}