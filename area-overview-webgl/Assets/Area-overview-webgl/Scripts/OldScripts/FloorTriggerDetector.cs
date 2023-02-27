using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTriggerDetector : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        LevelLoader.Instance.CheckFloorBtnStatus(transform, other.transform);
    }
}
