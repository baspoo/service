using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NguiPrintScript : MonoBehaviour
{
    #if UNITY_EDITOR
    public RuntimeBtn NguiToJson = new RuntimeBtn((r)=> {
        var ngui = r.Gameobject.GetComponent<NguiPrintScript>();
        ngui.Json = ngui.OnNguiToJson(ngui.transform);
    });

    public RuntimeBtn JsonToNgui = new RuntimeBtn((r) => {
        var ngui = r.Gameobject.GetComponent<NguiPrintScript>();
        ngui.OnJsonToNgui(ngui.Json);
    });
    #endif


    [TextArea]
    public string Json;
    public string OnNguiToJson(Transform root) 
    {
        guiData = DoConvertToJson(root);
        return guiData.SerializeToJson(SerializeHandle.NullValue);
    }
    public Transform OnJsonToNgui(string json)
    {
        return DoConvertToNGUI(json.DeserializeObject<GUIData>() , transform );
    }























    public GUIData guiData;
    [System.Serializable]
    public class GUIData {

        public string name;
        public Vector3 position;
        public Vector3 rotate;
        public Vector3 scale;
        


        [System.Serializable]
        public class Widget
        {
            public int pivot;
            public int depth;
            public Vector2 size;
        }


        public Lebel label;
        [System.Serializable]
        public class Lebel
        {
            public bool enable;
            public string text;
            public int fontSize;
            public int maxLine;
            public string fontName;
            public int fontStyle;
            public int overflow;
            public string color;
            public Vector2 spacing;
            public int effect;
            public Vector2 effectSize;
            public string effectColor;
            public bool gradient;
            public string colorTop;
            public string colorBot;
            public Widget widget;
        }



        public Texture texture;
        [System.Serializable]
        public class Texture
        {
            public bool enable;
            public string imageName;
            public double[] uvRect;
            public string color;
            public int type;
            public Vector4 border;
            public int flip;
            public bool gradient;
            public string colorTop;
            public string colorBot;
            public Widget widget;
        }



        public Btn btn;
        [System.Serializable]
        public class Btn
        {
            public bool enable;
            public string action;
            public float transition;
            public string colorNormal;
            public string colorHover;
            public string colorPressed;
            public string colorDisabled;
            public Vector3 colliderCenter;
            public Vector3 colliderSize;
        }




        public List<GUIData> Hierarchies;
    }





    void Demo()
    {
        Debug.Log("Demo");
    }


    public void Test() 
    {
        var find =  GameObject.Find("GameObject");
        if (find != null) 
        {
            find.SendMessage("Demo");
        }
    }



    GUIData DoConvertToJson(Transform root) 
    {
        var gui = new GUIData();
        gui.name = root.name;
        gui.position = root.localPosition;
        gui.scale = root.localScale;
        gui.rotate = root.localEulerAngles;
        


        var label = root.GetComponent<UILabel>();
        if (label != null)
        {
            gui.label = new GUIData.Lebel() { enable = true };
            gui.label.text = label.text;
            gui.label.fontSize = label.fontSize;
            gui.label.maxLine = label.maxLineCount;
            gui.label.fontName = label.trueTypeFont.name;
            gui.label.fontStyle = (int)label.fontStyle;
            gui.label.overflow = (int)label.overflowMethod;
            gui.label.color = label.color.ToHexString();
            gui.label.spacing = new Vector2(label.spacingX, label.spacingY);
            gui.label.effect = (int)label.effectStyle;
            gui.label.effectSize = label.effectDistance;
            gui.label.effectColor = label.effectColor.ToHexString();
            gui.label.gradient = label.applyGradient;
            gui.label.colorTop = label.gradientTop.ToHexString();
            gui.label.colorBot = label.gradientBottom.ToHexString();


            gui.label.widget = new GUIData.Widget();
            gui.label.widget.size = new Vector2(label.width, label.height);
            gui.label.widget.pivot = (int)label.pivot;
            gui.label.widget.depth = (int)label.depth;

        }


        var texture = root.GetComponent<UITexture>();
        if (texture != null)
        {
            gui.texture = new GUIData.Texture() { enable = true };
            gui.texture.imageName = texture.mainTexture != null ? texture.mainTexture.name : string.Empty; 
            gui.texture.uvRect = new double[4] {
                texture.uvRect.x,texture.uvRect.y,texture.uvRect.width,texture.uvRect.height
            };
            gui.texture.type = (int)texture.type;
            gui.texture.border = texture.border;
            gui.texture.flip = (int)texture.flip;
            gui.texture.color = texture.color.ToHexString();
            gui.texture.gradient = texture.applyGradient;
            gui.texture.colorTop = texture.gradientTop.ToHexString();
            gui.texture.colorBot = texture.gradientBottom.ToHexString();

            gui.texture.widget = new GUIData.Widget();
            gui.texture.widget.size = new Vector2(texture.width, texture.height);
            gui.texture.widget.pivot = (int)texture.pivot;
            gui.texture.widget.depth = (int)texture.depth;
        }

        var btn = root.GetComponent<UIButton>();
        if (btn != null)
        {
            gui.btn = new GUIData.Btn() { enable = true };
            gui.btn.colorNormal = btn.defaultColor.ToHexString();
            gui.btn.colorHover = btn.hover.ToHexString();
            gui.btn.colorPressed = btn.pressed.ToHexString();
            gui.btn.colorDisabled = btn.disabledColor.ToHexString();
            gui.btn.transition = btn.duration;
            var collider = root.GetComponent<BoxCollider>();
            if (collider != null) 
            {
                gui.btn.colliderCenter = collider.center;
                gui.btn.colliderSize = collider.size;
            }
        }



        var gameobjs = root.GetAllParent();
        if (gameobjs.Count > 0) 
        {
            gui.Hierarchies = new List<GUIData>();
            foreach (var g in gameobjs)
            {
                var n = DoConvertToJson(g.transform);
                gui.Hierarchies.Add(n);
            }
        }



        return gui;
    }


    public void OnBtnClick(GameObject btn) { 
    
    }
    public void OnBtnClickx( )
    {

    }
    Transform DoConvertToNGUI(GUIData guiData , Transform parent) 
    {
        var root = new GameObject(guiData.name);
        root.transform.parent = parent;
        root.transform.localPosition = guiData.position;
        root.transform.localScale = guiData.scale;
        root.transform.localEulerAngles = guiData.rotate;


        if (guiData.label != null && guiData.label.enable)
        {
            var label = root.AddComponent<UILabel>();
        }
        if (guiData.texture != null && guiData.texture.enable)
        {
            var texture = root.AddComponent<UITexture>();


            Service.Net.LoadImage("url",(tex)=> { 
            
            });
        }
        if (guiData.btn != null && guiData.btn.enable)
        {
            var btn = root.AddComponent<UIButton>();
            var collider = root.AddComponent<BoxCollider>();
            collider.isTrigger = true;


            btn.defaultColor = guiData.btn.colorNormal.HexToColor();
            btn.hover = guiData.btn.colorHover.HexToColor();
            btn.pressed = guiData.btn.colorPressed.HexToColor();
            btn.disabledColor = guiData.btn.colorDisabled.HexToColor();
            btn.duration = guiData.btn.transition;

            collider.center = guiData.btn.colliderCenter;
            collider.size = guiData.btn.colliderSize;
            btn.onClick.Add(new EventDelegate(()=>{


                Debug.Log(btn.name);
            

            }));
        }









        if (guiData.Hierarchies != null && guiData.Hierarchies.Count > 0) 
        {
            foreach (var c in guiData.Hierarchies)
            {
                DoConvertToNGUI( c, root.transform );
            }
        }
        return root.transform;
    }




}
