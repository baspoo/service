using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Newtonsoft.Json;


public class TestScript : MonoBehaviour
{

    public class Tk 
    {

        public double value;
        public string type;
        public int index;

       

        public void PushData(GameData row)
        {




            Formula f = new Formula();
            var str = f["", 50].Text;



            row.GetValue("value", out value);
            row.GetValue("type", out type);
            row.GetValue("index", out index);


            //row.GetValue(this, "value", "type", "index");

            //row.GetValue(this,"value");
            //row.GetValue(this, "type");
            //row.GetValue(this, "index");

            //string ss = "ss";
            //var tk = ss.DeserializeObject<Tk>();
            //object data = null;
            //tk = data.DeserializeObject<Tk>();


            var obj = "{}".DeserializeObject();
            var tk = "{}".DeserializeObject<Tk>();
        }
    }



    public RuntimeBtn btn = new RuntimeBtn("Test Btn",(r)=> {



        GameData gd = new GameData();
        gd.SetValue("value","123.75");
        gd.SetValue("type", "game");
        gd.SetValue("index", "5");


        Tk t = new Tk();
        t.PushData(gd);


        Debug.Log(t.value);
        Debug.Log(t.type);
        Debug.Log(t.index);

    });






    public StoreObject StoreObj;

    public SoundData sound;




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
