using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin3D : MonoBehaviour
{
    public Transform target;
    public float mouseSpeedMultiplier = 8;
    public float smoothSpeed = 0.04f;
    private float mouseX;
    void OnMouseDrag()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSpeedMultiplier;
    }

    void LateUpdate() //Cause we are using Lerp function
    {
        if (target == null)
            target = transform;
        target.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -mouseX, 0), smoothSpeed);
    }
}
