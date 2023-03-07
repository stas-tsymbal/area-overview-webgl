using System;
using Area_overview_webgl.Scripts.Controllers;
using Area_overview_webgl.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Area_overview_webgl.Scripts.PlayerScripts
{
    public class ClickController : MonoBehaviour
    {
        public bool mouseHeld = false;
        private Vector3 mousePosition;
        private GamePlatform currentGamePlatform;
        
        ITeleportable playerTeleport;
        ILookAtRotatable playerLookAt;
        public void Init(ITeleportable playerTeleport, ILookAtRotatable playerLookAt, GamePlatform currentGamePlatform)
        {
           this.playerTeleport = playerTeleport;
           this.playerLookAt = playerLookAt;
           this.currentGamePlatform = currentGamePlatform;

        }
        
        private void Update()
        {
            
            if (Input.GetMouseButtonDown(0)) {
                mouseHeld = false;
                mousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0)) {
                mouseHeld = true;
            }

            // I separate mouse moving and single click
            if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()) {
                if (mouseHeld && Vector3.SqrMagnitude(mousePosition - Input.mousePosition) > 0)
                {
                    // left mouse button is released after being held and moved
                } else  {
                    Debug.Log("it is teleport or rotate click");
                    playerTeleport.TryMakeTeleport();
                    playerLookAt.TryLookAtObject();
                }
                mouseHeld = false;
            }
            
           
        }
    }
}