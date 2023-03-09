using System;
using System.Collections;
using UnityEngine;

namespace Area_overview_webgl.Scripts.TeleportScripts
{
    /**
     * Provide teleport for object 
     */
    public class Teleport : MonoBehaviour
    {
        [Header("Set from init")] 
        [SerializeField] private Transform firstPersonHead; // cam transform
        [SerializeField] private CapsuleCollider firstPersonCollider;

        [Header("Lerp stop value and lerp speed")] [SerializeField]
        private float distanceForStopTeleporting = 0.1f;
        [SerializeField] private float lerpTeleportSpeed = 10f;
        
        private Coroutine teleportation = null;
        public Action OnEndTeleportation; // call when object made teleportation
        
        public void Init(Transform firstPersonHead, CapsuleCollider firstPersonCollider)
        {
            this.firstPersonCollider = firstPersonCollider;
            this.firstPersonHead = firstPersonHead;
        }

        public void MakeTeleport(Vector3 _newPos)
        {
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
            _newPos = new Vector3(_newPos.x, _newPos.y + firstPersonCollider.height / 2, _newPos.z);
            
            // Teleport via lerp position
            while (Vector3.Distance(firstPersonCollider.transform.position, _newPos) > distanceForStopTeleporting)
            {
                if (CheckDistanceLimit(_newPos)) // check capsule near wall
                {
                    _newPos = firstPersonCollider.transform.position;
                }

                firstPersonCollider.transform.position = Vector3.Lerp(firstPersonCollider.transform.position, _newPos,
                    Time.deltaTime * lerpTeleportSpeed);
                yield return null;
            }
            OnEndTeleportation?.Invoke();
        }


        // Lerp can't finish if target point near wall, because capsule collider with this obstacles never reach target position 
        private bool CheckDistanceLimit(Vector3 _targetPoint)
        {
            var needStop = false;
            // add 2 vector3 for correct Y 
            var playerPosition = new Vector3(firstPersonCollider.transform.position.x, firstPersonHead.position.y,
                firstPersonCollider.transform.position.z);
            var targetPointFixY = new Vector3(_targetPoint.x, firstPersonHead.position.y, _targetPoint.z);

            var additionalDistanceToCollider = 0.6f;
            if (Math.Sqrt((playerPosition - targetPointFixY).sqrMagnitude) < firstPersonCollider.radius + additionalDistanceToCollider)
            {
                RaycastHit hit;
                var ray = new Ray(playerPosition, targetPointFixY - playerPosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (Math.Sqrt((playerPosition - hit.point).sqrMagnitude) <
                        firstPersonCollider.radius + 0.5f)
                        needStop = true;
                }
            }

            return needStop;
        }
    }
}