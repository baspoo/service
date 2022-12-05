using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPS.Utls;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NPS
{
    public class NguiPrintScript : MonoBehaviour
    {



        public static NguiPrintScript Create(string json, Transform transform)
        {
            var ngui = new GameObject("NguiPrintScript").AddComponent<NguiPrintScript>();
            ngui.transform.parent = transform;
            ngui.transform.ResetTransform();
            Service.GameObj.ReLayer(ngui.gameObject, transform.gameObject);
            ngui.OnJsonToNgui(json);
            return ngui;
        }















        [TextArea]
        public string Json;
        public bool isGenerated => transform.childCount > 0;
        public string OnNguiToJson(Transform root)
        {
            guiData = DoConvertToJson(root);
            return guiData.SerializeToJson(SerializeHandle.NullValue, SerializeHandle.FormattingIndented);
        }
        public Transform OnJsonToNgui(string json)
        {
            OnClean();
            return DoConvertToNGUI(json.DeserializeObject<GUIData>(), transform);
        }
        public void OnClose()
        {
            Destroy(gameObject);
        }
        public void OnClean()
        {
            transform.DesAllParent();
        }





















        public GUIData guiData;
        [System.Serializable]
        public class GUIData
        {

            public string name;
            public Vector3 position;
            public Vector3 rotate;
            public Vector3 scale;

            public static string[] AddOnActions = new string[4] {
            AddOnAction.Close,
            AddOnAction.GoTo,
            AddOnAction.GoToAndClose,
            AddOnAction.Reopen
        };
            public class AddOnAction
            {
                public const string Close = "close";
                public const string GoTo = "goto";
                public const string GoToAndClose = "goto&close";
                public const string Reopen = "reopen";
            }
            [System.Serializable]
            public class AddOn
            {
                public string name;
                public string imgURL;
                public string btnDir;
                public string btnAct;
            }

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


        Transform DoConvertToNGUI(GUIData guiData, Transform parent)
        {
            var root = new GameObject(guiData.name);
            root.transform.parent = parent;
            root.transform.localPosition = guiData.position;
            root.transform.localScale = guiData.scale;
            root.transform.localEulerAngles = guiData.rotate;


            GUIData.AddOn addon = null;
            if (Service.String.isStrCropValue(guiData.name, "{", "}"))
            {
                addon = guiData.name.DeserializeObject<GUIData.AddOn>();
            }



            if (guiData.label != null && guiData.label.enable)
            {
                var label = root.AddComponent<UILabel>();
                var font = NPSUtility.instance.Fonts.Find(x => x.name == guiData.label.fontName);
                label.trueTypeFont = font != null ? font : NPSUtility.instance.Fonts[0];
                label.text = guiData.label.text;
                label.fontSize = guiData.label.fontSize;
                label.maxLineCount = guiData.label.maxLine;
                label.fontStyle = (FontStyle)guiData.label.fontStyle;
                label.overflowMethod = (UILabel.Overflow)guiData.label.overflow;
                label.color = guiData.label.color.HexToColor();
                label.spacingX = (int)guiData.label.spacing.x;
                label.spacingY = (int)guiData.label.spacing.y;

                label.effectStyle = (UILabel.Effect)guiData.label.effect;
                label.effectDistance = guiData.label.effectSize;
                label.effectColor = guiData.label.effectColor.HexToColor();
                label.applyGradient = guiData.label.gradient;
                label.gradientTop = guiData.label.colorTop.HexToColor();
                label.gradientBottom = guiData.label.colorBot.HexToColor();

                label.width = (int)guiData.label.widget.size.x;
                label.height = (int)guiData.label.widget.size.y;
                label.pivot = (UIWidget.Pivot)guiData.label.widget.pivot;
                label.depth = guiData.label.widget.depth;

            }
            if (guiData.texture != null && guiData.texture.enable)
            {
                var texture = root.AddComponent<UITexture>();
                texture.mainTexture = NPSUtility.instance.Texture.Find(x => x.name == guiData.texture.imageName);
                texture.uvRect = new Rect()
                {
                    x = (float)guiData.texture.uvRect[0],
                    y = (float)guiData.texture.uvRect[1],
                    width = (float)guiData.texture.uvRect[2],
                    height = (float)guiData.texture.uvRect[3]
                };
                texture.type = (UISprite.Type)guiData.texture.type;
                texture.border = guiData.texture.border;
                texture.flip = (UISprite.Flip)guiData.texture.flip;
                texture.color = guiData.texture.color.HexToColor();
                texture.applyGradient = guiData.texture.gradient;
                texture.gradientTop = guiData.texture.colorTop.HexToColor();
                texture.gradientBottom = guiData.texture.colorBot.HexToColor();

                texture.width = (int)guiData.texture.widget.size.x;
                texture.height = (int)guiData.texture.widget.size.y;
                texture.pivot = (UIWidget.Pivot)guiData.texture.widget.pivot;
                texture.depth = guiData.texture.widget.depth;


                if (addon != null && addon.imgURL.notnull() && Application.isPlaying)
                {
                    Service.Net.LoadImage(addon.imgURL, (tex) =>
                    {
                        texture.mainTexture = tex;
                    });
                }



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
                btn.onClick.Add(new EventDelegate(() =>
                {

                    if (addon != null)
                    {
                        if (addon.btnAct == GUIData.AddOnAction.Close)
                        {
                            OnClose();
                        }
                        if (addon.btnAct == GUIData.AddOnAction.Reopen)
                        {
                            OnClean();
                            OnJsonToNgui(addon.btnDir);
                        }
                        if (addon.btnAct == GUIData.AddOnAction.GoTo)
                        {
                            DoGoto(addon.btnDir);
                        }
                        if (addon.btnAct == GUIData.AddOnAction.GoToAndClose)
                        {
                            DoGoto(addon.btnDir);
                            OnClose();
                        }
                    }
                }));
            }









            if (guiData.Hierarchies != null && guiData.Hierarchies.Count > 0)
            {
                foreach (var c in guiData.Hierarchies)
                {
                    DoConvertToNGUI(c, root.transform);
                }
            }
            return root.transform;
        }


        void DoGoto(string dir)
        {
            if (dir.Contains("http") || dir.Contains(":\\"))
            {
                Application.OpenURL(dir);
            }
            else
            {
                var path = dir.Split('.');
                var find = GameObject.Find(path[0]);
                if (find != null)
                {
                    find.SendMessage(path[1]);
                }
            }

        }



    }

}




namespace NPS.Utls
{
#if UNITY_EDITOR
    [CustomEditor(typeof(NguiPrintScript))]
    public class UINguiPrintScript : Editor
    {
        public NguiPrintScript m_tools { get { return (NguiPrintScript)target; } }
        public override void OnInspectorGUI()
        {

            GUILayout.Label(NPSUtility.instance.Cover, GUILayout.Height(120));

            if (EditorGUIService.DrawHeader("Global", "NguiPrintScript.Global", false, false))
            {
                EditorGUIService.BeginContents(false);


                EditorGUIService.ListView.Print("Fonts", NPSUtility.instance.Fonts.Count, (i)=> {
                    NPSUtility.instance.Fonts[i] = (Font) EditorGUILayout.ObjectField(NPSUtility.instance.Fonts[i],typeof(Font));
                },()=> {
                    NPSUtility.instance.Fonts.Add(null);
                },(i)=> {
                    NPSUtility.instance.Fonts.RemoveAt(i);
                });

                EditorGUIService.ListView.Print("Textures", NPSUtility.instance.Texture.Count, (i) => {
                    NPSUtility.instance.Texture[i] = (Texture)EditorGUILayout.ObjectField(NPSUtility.instance.Texture[i], typeof(Texture));
                }, () => {
                    NPSUtility.instance.Texture.Add(null);
                }, (i) => {
                    NPSUtility.instance.Texture.RemoveAt(i);
                });

                if (GUILayout.Button("Save"))
                {
                    DoSave();
                }
                EditorGUIService.EndContents();
            }




            if (EditorGUIService.DrawHeader("Current", "NguiPrintScript.Current", false, false, new EditorGUIService.Option() {
                Icon = "C",
                Description ="Copy Json",
                Exe = () => {
                    m_tools.Json.Copy();
                }
            }, new EditorGUIService.Option()
            {
                Icon = "✚",
                Description = "Addon",
                Exe = () => {
                    EditorGUIService.Popup.ShowWindow("Addon", DoOpenAddOn );
                }
            }))
            {
                EditorGUIService.BeginContents(false);


                EditorStyles.textField.wordWrap = true;
                m_tools.Json = EditorGUILayout.TextArea(m_tools.Json, GUILayout.Height(120));

                EditorGUILayout.Space(25);

                EditorGUIService.BeginEndnable(m_tools.isGenerated, () => {
                    if (GUILayout.Button("\nNGUI To Json\n"))
                    {
                        if(IsCheckingToJson())
                            m_tools.Json = m_tools.OnNguiToJson(m_tools.transform);
                    }
                });


                if (!m_tools.isGenerated)
                {
                    EditorGUIService.BeginEndnable(m_tools.Json.notnull(), () => {
                        if (GUILayout.Button("\nJson To NGUI\n"))
                        {
                            m_tools.OnJsonToNgui(m_tools.Json);
                        }
                    });
                }
                else 
                {
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("\nClear\n"))
                    {
                        m_tools.OnClean();
                    }
                    GUI.backgroundColor = Color.white;
                }
                EditorGUIService.EndContents();
            }
        }





        void DoSave()
        {
            Undo.RecordObject(NPSUtility.instance, "NPSUtility");
            PrefabUtility.RecordPrefabInstancePropertyModifications(NPSUtility.instance);
            EditorUtility.CopySerialized(NPSUtility.instance, NPSUtility.instance);
            AssetDatabase.SaveAssets();
        }
        void DoOpenAddOn()
        {
            GUI.backgroundColor = Color.green;
            Transform newTransform = null;
            newTransform = (Transform)EditorGUILayout.ObjectField(newTransform, typeof(Transform), GUILayout.Height(40));
            GUI.backgroundColor = Color.white;

            if (newTransform != null)
            {
                NguiPrintScript.GUIData.AddOn addon = new NguiPrintScript.GUIData.AddOn();
                addon.name = newTransform.gameObject.name;
                newTransform.gameObject.name = addon.SerializeToJson(SerializeHandle.NullValue);
            }



            EditorGUILayout.Space(25);


            foreach (var n in m_tools.gameObject.transform.GetAllNode())
            {
                if (Service.String.isStrCropValue(n.name, "{", "}")) 
                {
                    EditorGUIService.BeginContents(false);
                    var addon = n.name.DeserializeObject<NguiPrintScript.GUIData.AddOn>();

                    GUI.backgroundColor = Color.gray;
                    EditorGUILayout.ObjectField(n.transform, typeof(Transform));
                    GUI.backgroundColor = Color.white;



                    if (n.GetComponent<UITexture>())
                    {
                        //EditorGUILayout.LabelField("Texture");
                        addon.imgURL = EditorGUILayout.TextField("▶ Texture URL :", addon.imgURL);
                        if (addon.imgURL == "Texture URL") addon.imgURL = null;
                    }

                    if (n.GetComponent<UIButton>())
                    {
                        //EditorGUILayout.LabelField("Button");
                        EditorGUILayout.BeginHorizontal();
                      
                        var index = 0;
                        var i = 0;
                        foreach (var str in NguiPrintScript.GUIData.AddOnActions)
                        {
                            if (addon.btnAct == str)
                            {
                                index = i;
                            }
                            i++;
                        }
                        index = EditorGUILayout.Popup("▶ Button :", index, NguiPrintScript.GUIData.AddOnActions);
                        addon.btnAct = NguiPrintScript.GUIData.AddOnActions[index];
                        switch (addon.btnAct)
                        {
                            case NguiPrintScript.GUIData.AddOnAction.Close: break;
                            case NguiPrintScript.GUIData.AddOnAction.GoTo:
                            case NguiPrintScript.GUIData.AddOnAction.GoToAndClose:
                            case NguiPrintScript.GUIData.AddOnAction.Reopen:
                                addon.btnDir = EditorGUILayout.TextField(addon.btnDir);
                                break;

                        }
                        EditorGUILayout.EndHorizontal();
                    }



                    if (addon.imgURL.isnull()) addon.imgURL = null;
                    if (addon.btnAct.isnull()) addon.btnAct = null;
                    if (addon.btnDir.isnull()) addon.btnDir = null;


                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Remove"))
                    {
                        n.gameObject.name = addon.name;
                    }
                    else 
                    {
                        n.gameObject.name = addon.SerializeToJson(SerializeHandle.NullValue);
                    }
                    GUI.backgroundColor = Color.white;

                    
                    EditorGUIService.EndContents();

                    EditorGUILayout.Space(6);
                }
            }


           

        }
        bool IsCheckingToJson() 
        {
            if (Verlify())
            {
                return true;
            }
            else 
            {
                EditorGUIService.Popup.ShowWindow("Checking ToJson", () => { Verlify(); });
                return false;
            }
        }
        bool Verlify( )
        {

            
            bool complete = true;
            foreach (var n in m_tools.transform.GetAllNode())
            {

                var texture = n.GetComponent<UITexture>();
                if (texture != null &&
                    texture.mainTexture != null &&
                    NPSUtility.instance.Texture.Find(x=>x == texture.mainTexture) == null )
                {
                    complete = false;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField(texture.mainTexture,typeof(Texture));
                    GUI.backgroundColor = Color.yellow;
                    if (GUILayout.Button("Solve"))
                    {
                        NPSUtility.instance.Texture.Add(texture.mainTexture);
                        DoSave();
                    }
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndHorizontal();
                }


                var label = n.GetComponent<UILabel>();
                if (label != null &&
                    label.trueTypeFont != null &&
                    NPSUtility.instance.Fonts.Find(x => x == label.trueTypeFont) == null)
                {
                    complete = false;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField(label.trueTypeFont, typeof(Font));
                    GUI.backgroundColor = Color.yellow;
                    if (GUILayout.Button("Solve"))
                    {
                        NPSUtility.instance.Fonts.Add(label.trueTypeFont);
                        DoSave();
                    }
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndHorizontal();
                }



            }
            return complete;
        }



    }
#endif





}
