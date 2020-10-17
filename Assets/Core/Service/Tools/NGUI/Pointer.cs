using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{

    // สามารถหาตำแหน่งหน้าจอ และ ตำแหน่ง world ตลอดเวลาได้

    public Vector3 WorldPosition => (tranWorld==null) ? Vector3.zero : tranWorld.position;
    public Vector3 ScreenPosition => (tranUI == null) ? Vector3.zero : tranUI.position;

    public Camera camUI;
    public Camera camWorld;
    public UIRoot root;
    public Transform tranUI;
    public Transform tranWorld;
    public UIAnchor anc;
    void Update()
    {
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f;
        var position = camUI.ScreenToWorldPoint(screenPoint);
        tranUI.localPosition = ScreenPoint.GetWorldPositionToNGUI(camUI, position , root, anc);
        tranWorld.position = (camWorld == null)? Vector3.zero : camWorld.ScreenToWorldPoint(screenPoint);
    }
}
