using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHover : MonoBehaviour
{

    public static UIHover Instance;
    public static bool Hover;

    public Vector3 mousePosition => transform.position;
    [SerializeField] Camera uiCamera;
    [SerializeField] bool m_Hover;
    UIEventTrigger et;

    void Awake()
    {
        Instance = this;
        et = gameObject.AddComponent<UIEventTrigger>();
        et.onHoverOver.Add(new EventDelegate(()=> {
            Hover = true;
        }));
        et.onHoverOut.Add(new EventDelegate(() => {
            Hover = false;
        }));
    }

    // Update is called once per frame
    void Update()
    {
        var pos = Input.mousePosition;
        pos.z = 0;
        pos = uiCamera.ScreenToWorldPoint(pos);
        transform.position = pos;
        m_Hover = Hover;
    }
}
