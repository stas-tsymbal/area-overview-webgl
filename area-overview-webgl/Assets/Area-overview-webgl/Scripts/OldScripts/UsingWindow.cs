using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingWindow : MonoBehaviour
{
    public void SetStateObject(bool Val)
    {
        if(!gameObject.activeSelf)
            gameObject.SetActive(Val);
        else
            gameObject.SetActive(!Val);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
         //   gameObject.SetActive(false);
        }
    }
}
