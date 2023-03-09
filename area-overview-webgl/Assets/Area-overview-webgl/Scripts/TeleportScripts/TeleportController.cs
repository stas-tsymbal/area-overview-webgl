using Area_overview_webgl.Scripts.Static;
using UnityEngine;

namespace Area_overview_webgl.Scripts.TeleportScripts
{
    /**
     * Control object teleportation and
     * provide checking opportunity for teleporting (LayerMask teleportLayerMask)
     */
    public class TeleportController : MonoBehaviour
    {
        [Header("Teleport")] 
        [SerializeField] private Teleport telepot;

        [Header("Layer for teleport")] 
        [SerializeField] private LayerMask teleportLayerMask;

        [Space] private Vector2 currentAngle; // use for remember current angle -> X and Y
        private bool isRememberCurrentAngle;

        private Camera myCamera;
        private RaycastHit currentHit;
        
        public void Init(Transform firstPersonCamera, CapsuleCollider firstPersonCollider, Camera myCamera)
        {
            telepot.Init(firstPersonCamera, firstPersonCollider);
            telepot.OnEndTeleportation += OnEndTeleportation;
            this.myCamera = myCamera;
        }

        private void OnEndTeleportation()
        {
            //Debug.Log("end teleportation");
        }


        // Check layer for teleportation and update currentHit
        public bool CanMakeTeleport(Vector3 rayStartPosition)
        {
            var isLayerForTeleport = false;
            RaycastHit hit;
            var ray = myCamera.ScreenPointToRay(rayStartPosition); // make ray from position
            if (Physics.Raycast(ray, out hit))
            {
                var hitObject = hit.transform.gameObject; // ray hit this object
                if (hitObject.layer == Calculator.GetPowNumber2(teleportLayerMask.value))
                    isLayerForTeleport = true;
                currentHit = hit;
            }

            return isLayerForTeleport;
        }
        
        // Try make teleport with layer checking
        public void TryMakeTeleportWithCheck(Vector2 _position)
        {
            if (CanMakeTeleport(_position))
            {
                telepot.MakeTeleport(GetHit().point);
            }
            else
            {
                Debug.Log("Don't hit layer for teleport");
            }
        }

        // Make teleport to last hit
        public void MakeTeleport()
        {
            telepot.MakeTeleport(GetHit().point);
        }

        private RaycastHit GetHit()
        {
            return currentHit;
        }
    }
}