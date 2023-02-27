using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// open link
public class OpenLink : MonoBehaviour
{
    //call from ui
    public void OpenURL()
    {
        Application.OpenURL("http://www.karmuseum.ru/");
    }

  
}
