using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent : MonoBehaviour
{



    public GameObject self;
    public GameObject root;
    public bool isActive;
    public bool isPosition;
    public bool isRotate;
    public bool isLocalScale;

    void Update()
    {
        if (root != null) 
        {
            if (isActive) 
            {
                self.SetActive(root.gameObject.activeSelf);
            }
            if (isPosition)
            {
                self.transform.position = root.transform.position;
            }
            if (isLocalScale)
            {
                self.transform.localScale = root.transform.localScale;
            }
            if (isRotate)
            {
                self.transform.rotation = root.transform.rotation;
            }

        }
    }
}
