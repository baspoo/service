using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayMouseToWorldPosition : MonoBehaviour
{
    public LayerMask layerMask;
    Ray ray;
    RaycastHit hitData;
    Vector3 worldPosition;
    public delegate void onclick(Vector3 worldposition);
    public onclick OnClick;
    public onclick OnUpdate;
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitData, 1000, layerMask))
        {
            worldPosition = hitData.point;
            if (OnUpdate != null)
                OnUpdate(worldPosition);
            if (Input.GetMouseButtonDown(0)) { 
                if (OnClick != null)
                    OnClick(worldPosition);
            }
        }
    }

}
