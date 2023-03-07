using UnityEngine;

namespace Area_overview_webgl.Scripts.Interfaces
{
    public interface ILookAtRotatable
    {
        void TryLookAtObject(Vector3 cursorPosition);
    }
}