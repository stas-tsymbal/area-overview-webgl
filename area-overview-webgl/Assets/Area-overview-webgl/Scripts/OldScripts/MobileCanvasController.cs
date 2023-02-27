using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Use for control FPS moving from mobile
 */
public class MobileCanvasController : MonoBehaviour
{
    public void PointerDownForward()
    {
        MobileCameraRotatorNew.Instance.SetStateIsMoveUp(true);
        MobileCameraRotatorNew.Instance.SetStateIsMoveDown(false);
    } 
    
    public void PointerUpForward()
    {
        MobileCameraRotatorNew.Instance.SetStateIsMoveUp(false);
    }
    
    public void PointerDownBack()
    {
        MobileCameraRotatorNew.Instance.SetStateIsMoveUp(false);
        MobileCameraRotatorNew.Instance.SetStateIsMoveDown(true);
    } 
    
    public void PointerUpBack()
    {
        MobileCameraRotatorNew.Instance.SetStateIsMoveDown(false);
    }
 
}
