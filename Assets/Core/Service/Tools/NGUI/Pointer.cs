using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monitor : MonoBehaviour
{

    public Camera cam;
    public UIRoot root;
    public Transform tran;
    public UIAnchor anc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f;
        var position = cam.ScreenToWorldPoint(screenPoint);

        // tran.position = Input.mou;
        tran.localPosition = ScreenPoint.GetWorldPositionToNGUI(cam, position , root, anc);
    }
}
