using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularGrid : MonoBehaviour
{

    public Transform[] Slices;
    public float gridRadius;

    void Start()
    {
        for (int slice = 0; slice < Slices.Length; slice++)
        {
            float angle = slice * Mathf.PI * 2 / Slices.Length;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * (gridRadius);

            Vector3 currentPos = transform.localPosition;
            Vector3 desiredPos = new Vector3(currentPos.x + pos.x, currentPos.y + pos.y, currentPos.z + pos.z);

            //Gizmos.DrawSphere(desiredPos, 1.0f);

            Slices[slice].transform.localPosition = desiredPos;

        }
    }

    // Update is called once per frame
    void Update()
    {
        Start();
    }
}
