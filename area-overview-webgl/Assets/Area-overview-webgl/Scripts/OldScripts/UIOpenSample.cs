using System;
using System.Collections;
using System.Collections.Generic;
using Area_overview_webgl.Scripts.Teleport;
using UnityEngine;

/**
 * Open window after click on this game object 
 */
public class UIOpenSample : MonoBehaviour
{
    private Vector2 currentAngle;
    [SerializeField] private int viewID;
    private float angleForIgnoreTeleport = 0.5f;
    private void OnMouseDown()
    {
        RememberAngle();
    }

    void OnMouseUp()
    {
        if(CanOpen())
            ShowView(viewID);
    }

    private void ShowView(int _id)
    {
       IconViewController.Instance.ShowIcon(_id);
    }

    private bool CanOpen()
    {
        var canTeleport = !(Math.Abs(Math.Abs(currentAngle.x) - Math.Abs(Teleport.Instance.GetFirstPersonX().eulerAngles.y)) > angleForIgnoreTeleport || // check X
                            (Math.Abs(Math.Abs(currentAngle.y) - Math.Abs(Teleport.Instance.GetFirstPersonY().eulerAngles.x)) > angleForIgnoreTeleport));
        return canTeleport;
    }

    private void RememberAngle()
    {
        currentAngle = new Vector2( Teleport.Instance.GetFirstPersonX().eulerAngles.y ,Teleport.Instance.GetFirstPersonY().eulerAngles.x);
    }
}
