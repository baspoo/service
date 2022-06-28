using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LookAtCamera : MonoBehaviour
{
    public Transform lookAt;
    public Vector3 Offset;
    void Update()
    {
        if (lookAt != null)
        {
            transform.LookAt(transform.position + lookAt.forward);
            return;
        }
    }

    

}
