using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowWorld3D : MonoBehaviour
{


    public Transform Target;
    public Camera cam3D;
    public UIRoot root;
    public UIAnchor anc;

    void Update()
    {
        if (Target != null) 
        {
            transform.localPosition = ScreenPoint.GetWorldPositionToNGUI(cam3D, Target.transform , root, anc);
        }
    }
}
