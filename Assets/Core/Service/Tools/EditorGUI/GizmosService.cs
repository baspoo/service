using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class GizmosService : MonoBehaviour
{

    [Header("DrawPoint")]
    public bool DrawPoint;
    public Color PointColor = Color.blue;
    public float radius = 1.0f;


    [Header("DrawLine")]
    public bool DrawLine;
    public Color LineColor = Color.blue;
    public Vector3[] LinePosition = new Vector3[2];
    public Transform[] LineTransform = new Transform[2];


    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        if (DrawPoint)
        {
            Gizmos.color = PointColor;
            Gizmos.DrawSphere(transform.position, radius);
        }

        if (DrawLine)
        {
            Gizmos.color = LineColor;
            if (LineTransform[0] != null) LinePosition[0] = LineTransform[0].position;
            if (LineTransform[1] != null) LinePosition[1] = LineTransform[1].position;
            Gizmos.DrawLine(LinePosition[0], LinePosition[1]);
        }
    }

    public RuntimeBtn ddd = new RuntimeBtn((r)=> {

        SceneGUI.Enable();
    });

}

