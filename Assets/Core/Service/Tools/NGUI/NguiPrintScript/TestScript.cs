using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace NPS.Demo 
{ 
    public class TestScript : MonoBehaviour
    {
       
        void Demo()
        {
            var json = "{  }";
            NPS.NguiPrintScript.Create( json , transform);
        }


        void Hello()
        {
            Debug.Log("Hello Baspoo!");
        }

        void PBaspoo()
        {
            Debug.Log("Hello PBaspoo!");
        }
    }
}