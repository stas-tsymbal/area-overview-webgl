using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExitPosition : MonoBehaviour
{
    public static PlayerExitPosition Instance;
    // save position after exit from church
    public void SavePositionAfterExit()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }
}
