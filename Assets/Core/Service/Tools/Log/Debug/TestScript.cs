using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogService.Test
{
    public class TestScript : MonoBehaviour
    {

        private void Start()
        {
            LogService.DebugLogger.Init();
        }

        [SerializeField] AudioClip clip;
        [SerializeField] GameObject gameobj;



        public void TestList() {


            List<int> let = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };


            //let.Shuffle().ForEach(x => Debug.Log(x));


            //let.If(x => x > 5).ForEach(x => Debug.Log(x));

            //let.If((x) => {
            //    return false;
            //});



            bool IsCal(int x) 
            {

                if (x % 2 == 0)
                    return false;
                else
                    return true;
            }

            foreach (var d in let.If(IsCal)) 
            {
                Debug.Log(d);
            }

        }

        public void TestLog() 
        {




            "1.message".Log();
            "2.message".Log(Color.red);
            "3.message".Log("baspoo");
            5.Log("baspoo");


            return;

            Debug.Log("1.message");

            return;

            //Commond
            Debug.Log("1.message");


            //Tag
            Debug.LogFormat("2.message");
            Debug.LogFormat("3.message", "tag");
            Debug.LogFormat("4.message", "baspoo");
            Debug.LogFormat("5.message", "ritichai");

            Debug.LogFormat("แกรี่หัวฟวย","garry");
        }


        public void TestLogAdvance()
        {



            //Option - Condition
            Debug.LogFormat("message 20 > 5", 20 > 5);
            Debug.LogFormat("message 20 < 5", 20 < 5);
            Debug.LogFormat("message 300 = 300", 300 == 300 , "tag");

            //Option - Object Tacking
            Debug.LogFormat("message gameObject", gameObject);
            Debug.LogFormat("message gameObject", gameObject, "tag");

            Debug.LogFormat("message transform", transform);
            Debug.LogFormat("message transform", transform, "tag");

            Debug.LogFormat("message clip", clip);
            Debug.LogFormat("message clip", clip, "tag");



            //Option - Invoke Method
            System.Action act = () => { Debug.Log("Click!"); };
            Debug.LogFormat("message action", act);
            Debug.LogFormat("message action", act, "tag");


            //Option - Realtime Func
            System.Func<object> func = () => { return gameObject.name; };
            Debug.LogFormat("message realtime function", func);
            Debug.LogFormat("message realtime function", func, "tag");

        }












        public RuntimeBtn List = new RuntimeBtn((r) =>
        {
            r.Gameobject.GetComponent<TestScript>().TestList();
        });

        public RuntimeBtn Log = new RuntimeBtn((r) =>
        {
            r.Gameobject.GetComponent<TestScript>().TestLog();
        });
        public RuntimeBtn LogAdvance = new RuntimeBtn((r) =>
        {
            r.Gameobject.GetComponent<TestScript>().TestLogAdvance();
        });
    }
}
