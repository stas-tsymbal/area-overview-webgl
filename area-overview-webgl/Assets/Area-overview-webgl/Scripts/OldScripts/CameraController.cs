using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script added to the camera to provide look at ability
 */

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraOrbit;
    [SerializeField] private Transform target;

    private void Start()
    {
        cameraOrbit.position = target.position;
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
        transform.LookAt(target.position);
    }
}
