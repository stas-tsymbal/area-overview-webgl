using UnityEngine;

namespace Area_overview_webgl.Scripts.Interfaces
{
    public interface ITeleportable
    {
        void TryMakeTeleport(Vector3 cursorPosition);
    }
}