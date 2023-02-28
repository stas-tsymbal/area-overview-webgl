using System.Collections;
using System.Collections.Generic;
using Area_overview_webgl.Scripts.FPSCamera;
using UnityEngine;

/**
 * Use for control FPS moving from mobile
 */
public class MobileCanvasController : MonoBehaviour
{
    public void PointerDownForward()
    {
        FirstPersonRotator.Instance.SetStateIsMoveUp(true);
        FirstPersonRotator.Instance.SetStateIsMoveDown(false);
    } 
    
    public void PointerUpForward()
    {
        FirstPersonRotator.Instance.SetStateIsMoveUp(false);
    }
    
    public void PointerDownBack()
    {
        FirstPersonRotator.Instance.SetStateIsMoveUp(false);
        FirstPersonRotator.Instance.SetStateIsMoveDown(true);
    } 
    
    public void PointerUpBack()
    {
        FirstPersonRotator.Instance.SetStateIsMoveDown(false);
    }
 
}
