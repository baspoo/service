using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NguiPackage : MonoBehaviour
{
    static NguiPackage minstance;
    public static NguiPackage instance 
    {
        get {
            if (minstance == null)
                minstance = ((GameObject)Resources.Load("NGUIservice/NguiPackage", typeof(GameObject))).GetComponent<NguiPackage>();
            return minstance;
        }
    }



    public GUIObject guiObject;
    [System.Serializable]
    public class GUIObject 
    {
        public GameObject HotKey;
        public GameObject Bubble;
        public GameObject TopicToolTips;
    }



}
