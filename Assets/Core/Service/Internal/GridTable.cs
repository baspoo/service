using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif



public class GridTable : MonoBehaviour
{
    public enum Arrangement
    {
        Horizontal,
        Vertical
    }
    public enum Pivot
    {
        Left,
        Center,CenterTop,
        Right
    }


   


    public Arrangement arrangement;
    public Pivot pivot;
    public float CellHight;
    public float CellWidth;
    public int Column;
    public bool  HideInactive;

    List<GameObject> let => Service.GameObj.GetAllParent(transform);

     float padx;
     float linefull;




    [System.Serializable]
    public class Custom {

        public GameObject gameObject;
        public Vector2 offset;

    }


    public  T AddChild<T>(GameObject gameObject)
    {
        var t =  Service.GameObj.Created(gameObject,transform).GetComponent<T>();
        Reposition();
        return t;
    }
    public GameObject AddChild(GameObject gameObject)
    {
        var t = Service.GameObj.Created(gameObject, transform);
        Reposition();
        return t;
    }
    public void DesAll( )
    {
        Service.GameObj.DesAllParent(transform);
        Reposition();
    }
    public void Des(GameObject gameobject)
    {
        Destroy(gameobject);
        Reposition();
    }




    public List<Custom> Customs = new List<Custom>();
    public void AddOffect(GameObject gameObject, Vector2 offset) 
    {
        var find = Customs.Find(x => x.gameObject == gameObject);
        if (find == null)
            Customs.Add(new Custom()
            {
                gameObject = gameObject,
                offset = offset
            });
        else 
        {
            find.offset = offset;
        }
    }


    public void Reposition()
    {

        int index = 0;
        int line = 0;

        var let = this.let;

        if (HideInactive) 
        {
            let.RemoveAll(x => !x.activeSelf);
        }

        var count = (Column != 0) ? Column : let.Count;


        if (Column < 0) Column = 0;

        if (pivot == Pivot.Left)
        {
            padx = 0;
        }
        if (pivot == Pivot.Center)
        {
            padx = (((count) / 2.0f) - 0.5f) * CellWidth * -1.0f;
        }
        if (pivot == Pivot.Right)
        {
            padx = (((count)) - 1) * CellWidth * -1.0f;
        }



        var c1 = ((double)let.Count / (double)Column);
        var c2 = Column == 0 ? 0 : (let.Count / Column);
        if (c1 > c2) linefull = c2 + 1;
        else linefull = c2;

        Dictionary<int, List<GameObject>> dict = new Dictionary<int, List<GameObject>>();
        for (int i = 0; i < linefull; i++)
        {
            dict.Add(i, new List<GameObject>());
        }



        if (arrangement == Arrangement.Horizontal)
        {
            Vector2 offset = Vector2.zero;
            foreach (var t in let)
            {
                if (Column != 0 && index >= Column)
                {
                    offset.x = 0.0f;
                    line++;
                    index = 0;
                }

                var find = Customs.Find(f => f.gameObject == t);


                dict[line].Add(t.gameObject);
                var x = index * CellWidth + padx + offset.x;
                var y = line * CellHight * -1f + offset.y;

                if (find != null && find.offset != Vector2.zero) 
                {
                    x += find.offset.x / 2.0f;
                    y += find.offset.y / 2.0f;
                }


                Vector3 vec = new Vector3(x, y, 0.0f);
                t.transform.localPosition = vec;


               
                if (find != null) offset += find.offset;


                index++;
            }



        }

        if (pivot == Pivot.Center || pivot == Pivot.CenterTop)
        {
            float c = 0.0f;
            if (pivot == Pivot.Center) c = CellHight * ((dict.Count / 2.0f) - 0.5f);

            foreach (var d in dict)
            {
                //Debug.Log(d.Value.Count);
                foreach (var t in d.Value)
                {
                    var resume = count - d.Value.Count;
                    Vector3 vec = t.transform.localPosition;
                    vec.x += (resume * CellWidth) / 2;
                    vec.y += c;
                    t.transform.localPosition = vec;
                }
                //c -= CellHight;
            }
        }

        if (pivot == Pivot.Right)
        {
            foreach (var d in dict)
            {
                //Debug.Log(d.Value.Count);
                foreach (var t in d.Value)
                {
                    var resume = count - d.Value.Count;
                    Vector3 vec = t.transform.localPosition;
                    vec.x += (resume * CellWidth);
                    t.transform.localPosition = vec;
                }
            }
        }







    }


}



#if UNITY_EDITOR
[CustomEditor(typeof(GridTable))]
[CanEditMultipleObjects]
[System.Serializable]
public class GridTableUI : Editor
{

    public GridTable master => (GridTable)target;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(!Application.isPlaying)
            master.Reposition();
        
        if (GUILayout.Button("Snap"))
        {

            master.Reposition();


        }


    }
}
#endif








