using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CanEditMultipleObjects]
[CustomEditor(typeof(LayoutStyle))]
[System.Serializable]
public class LayoutUI : Editor
{
    //public LayoutStyle m_tools { get { return ((GameObject)Selection.activeObject).GetComponent<LayoutStyle>(); } }
    public LayoutStyle m_tools { get { return (LayoutStyle)target; } }
    bool isSetting = false;
    string ID;
    public override void OnInspectorGUI()
    {

        if (!Service.GameObj.isObjectNotNull(m_tools))
        {
            return;
        }

        if (Application.isPlaying)
        {
            m_tools.iTap = 1;
        }
        else
        {
            string[] taps = new string[] { "Register", "Layout Style" };
            m_tools.iTap = GUILayout.Toolbar(m_tools.iTap, taps);
            isSetting = EditorGUILayout.ToggleLeft("Setting", isSetting);
            EditorGUILayout.Space();
            if (isSetting)
            {
                Setting();
                return;
            }
        }



        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (m_tools.iTap == 0)
        {
            if (Application.isPlaying)
            {

            }
            else
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("----------------------------------");
                foreach (LayoutStyle.LayoutData LD in new ArrayList(m_tools.LayoutDatas))
                {
                    LayoutView(LD);
                }


                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Add Object"))
                {
                    m_tools.LayoutDatas.Add(new LayoutStyle.LayoutData());
                }
                GUI.backgroundColor = Color.white;
            }
        }
        if (m_tools.iTap == 1)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("----------------------------------");
            foreach (LayoutStyle.StyleData SD in new ArrayList(m_tools.StyleDatas))
            {
                StyleView(SD);
            }

            if (!Application.isPlaying)
            {
                GUI.backgroundColor = Color.green;
                ID = EditorGUILayout.TextField("ID", ID);
                if (GUILayout.Button("Add Style"))
                {
                    LayoutStyle.Tools.Save(m_tools, ID);
                    ID = "";
                }
                GUI.backgroundColor = Color.white;
            }
        }
        EditorGUILayout.Space(30);

        if (GUILayout.Button("Save"))
        {
            Save();
        }


    }

    void Save()
    {
        m_tools.gameObject.name = m_tools.gameObject.name;
        serializedObject.ApplyModifiedProperties();
        Undo.RecordObject(m_tools, m_tools.name);
    }

    void LayoutView(LayoutStyle.LayoutData data)
    {
        data.ID = EditorGUILayout.TextField("ID : ", data.ID);
        data.Transform = (Transform)EditorGUILayout.ObjectField("Transform : ", data.Transform, typeof(Transform));

        int del = -1;
        GUI.backgroundColor = Color.red;
        del = GUILayout.SelectionGrid(del, new string[] { "Remove" }, 5);
        GUI.backgroundColor = Color.white;
        if (del != -1)
        {

            if (EditorUtility.DisplayDialog("LayoutStyle!", "Remove '" + data.ID + "' Object", "Yes", "No"))
            {
                m_tools.LayoutDatas.Remove(data);
            }
        }

        EditorGUILayout.LabelField("----------------------------------");
        EditorGUILayout.Space();
    }
    void StyleView(LayoutStyle.StyleData data)
    {
        GUIstylePackage.Instant.HeaderBigBack.normal.textColor = Color.black;
        GUIStyle gs = new GUIStyle(GUIstylePackage.Instant.HeaderBigBack);
        if (m_tools.lastcurrent != null)
        {
            if (m_tools.lastcurrent.ID == data.ID)
                gs.normal.textColor = Color.yellow;
        }
        EditorGUILayout.LabelField(data.ID, gs);

        int del = -1;
        if (!Application.isPlaying)
        {
            //string a = ((data.isAnim) ? "✔ " : "✘ ") + " Anim";
            del = GUILayout.SelectionGrid(del, new string[] { "Load", "Update", "Remove" }, 3);
        }
        else
        {
            if (GUILayout.Button("Load"))
            {
                del = 0;
            }
        }
        if (del == 0)
        {
            m_tools.LoadLayout(data.ID);
        }
        if (del == 1)
        {
            LayoutStyle.Tools.Update(m_tools, data.ID);
        }
        if (del == 2)
        {
            if (EditorUtility.DisplayDialog("LayoutStyle!", "Remove '" + data.ID + "' Style", "Yes", "No"))
            {
                LayoutStyle.Tools.RemoveStyle(m_tools, data);
            }
        }
        EditorGUILayout.LabelField("----------------------------------");
        EditorGUILayout.Space();
    }

    void Setting()
    {
        var s = m_tools.setting;
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Transforms");
        s.position = EditorGUILayout.ToggleLeft("position", s.position);
        s.scale = EditorGUILayout.ToggleLeft("scale", s.scale);
        s.rotate = EditorGUILayout.ToggleLeft("rotate", s.rotate);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Component Enable");
        s.color = EditorGUILayout.ToggleLeft("color", s.color);
        s.tween = EditorGUILayout.ToggleLeft("tween", s.tween);
        s.animation = EditorGUILayout.ToggleLeft("animation", s.animation);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("NGUI Enable");
        s.lable = EditorGUILayout.ToggleLeft("lable", s.lable);
        s.texture = EditorGUILayout.ToggleLeft("texture", s.texture);
        s.btn = EditorGUILayout.ToggleLeft("btn", s.btn);
        s.grid = EditorGUILayout.ToggleLeft("grid", s.grid);
        s.scrollview = EditorGUILayout.ToggleLeft("scrollview", s.scrollview);
        s.panel = EditorGUILayout.ToggleLeft("panel", s.panel);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("NGUI Worry Properties");
        s.lb_text = EditorGUILayout.ToggleLeft("lb_text", s.lb_text);
        s.texture_img = EditorGUILayout.ToggleLeft("texture_img", s.texture_img);
        s.texture_size = EditorGUILayout.ToggleLeft("texture_size", s.texture_size);
        s.texture_border = EditorGUILayout.ToggleLeft("texture_border", s.texture_border);
        s.texture_rect = EditorGUILayout.ToggleLeft("texture_rect", s.texture_rect);
        s.depth = EditorGUILayout.ToggleLeft("depth", s.depth);
        s.pivot = EditorGUILayout.ToggleLeft("pivot", s.pivot);
        s.keepAspectRatio = EditorGUILayout.ToggleLeft("keepAspectRatio", s.keepAspectRatio);


        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        var list = serializedObject.FindProperty("setting.ignore");
        EditorGUILayout.PropertyField(list, new GUIContent("ignore"), true);
        serializedObject.ApplyModifiedProperties();
    }



}
#endif
public class LayoutStylex : MonoBehaviour
{
}
























public class LayoutStyle : MonoBehaviour
{
    public int iTap = 0;
    public bool isAnim;
    public float Speed;
    public List<LayoutData> LayoutDatas = new List<LayoutData>();
    [System.Serializable]
    public class LayoutData
    {
        public string ID;
        public Transform Transform;
        public Service.Formula Formula;
    }
    public StyleData lastcurrent = null;
    public List<StyleData> StyleDatas = new List<StyleData>();
    [System.Serializable]
    public class StyleData
    {
        public string ID;
        public bool isAnim;
    }
    public Setting setting = new Setting();

    [System.Serializable]
    public class Setting
    {
        public bool position = true;
        public bool scale = true;
        public bool rotate = true;
        public bool lable = true;
        public bool lb_text = true;
        public bool texture = true;
        public bool texture_img = true;
        public bool texture_size = true;
        public bool texture_border = true;
        public bool texture_rect = false;
        public bool btn = true;
        public bool color = true;
        public bool tween = true;
        public bool depth = true;
        public bool pivot = true;
        public bool keepAspectRatio = true;
        public bool grid = true;
        public bool scrollview = true;
        public bool panel = true;
        public bool animation = true;

        public List<string> ignore = new List<string>();
    }
    public void LoadLayout(string StyleID)
    {
        Tools.Load(this, StyleID);
    }







    public class Tools
    {
        public static void Save(LayoutStyle m, string StyleID)
        {
            StyleData style = m.StyleDatas.Find(x => x.ID == StyleID);
            if (style == null)
            {
                style = new StyleData() { ID = StyleID };
                m.StyleDatas.Add(style);
                save(m, style);
            }
        }
        public static void Load(LayoutStyle m, string StyleID)
        {
            read(m, StyleID);
        }
        public static void Update(LayoutStyle m, string StyleID)
        {
            StyleData style = m.StyleDatas.Find(x => x.ID == StyleID);
            if (style != null)
            {
                save(m, style);
            }
        }
        public static void RemoveStyle(LayoutStyle m, StyleData Style)
        {
            m.StyleDatas.Remove(Style);
            foreach (var obj in m.LayoutDatas)
            {
                obj.Formula.DesFormula(Style.ID);
            }
        }














        static void save(LayoutStyle m, StyleData style)
        {
            Debug.Log("save : " + style.ID);
            foreach (var obj in m.LayoutDatas)
            {
                Service.Formula f_style = new Service.Formula();

                // Transform
                f_style.AddFormula("activeSelf", 0).LocalContent.Bol = obj.Transform.gameObject.activeSelf;
                f_style.AddFormula("localPosition", 0).LocalContent.vecter3 = obj.Transform.localPosition;
                f_style.AddFormula("localRotation", 0).LocalContent.quaternion = obj.Transform.localRotation;
                f_style.AddFormula("localScale", 0).LocalContent.vecter3 = obj.Transform.localScale;

                // Animation
                var animation = obj.Transform.GetComponent<Animation>();
                if (animation)
                {
                    f_style.AddFormula("animation.clip", 0).LocalContent.animclip = animation.clip;
                }

                var UILabel = obj.Transform.GetComponent<UILabel>();
                if (UILabel)
                {
                    f_style.AddFormula("UILabel.text", 0).LocalContent.String = UILabel.text;
                    f_style.AddFormula("UILabel.width", 0).LocalContent.Int = UILabel.width;
                    f_style.AddFormula("UILabel.height", 0).LocalContent.Int = UILabel.height;
                    f_style.AddFormula("UILabel.color", 0).LocalContent.color = UILabel.color;
                    f_style.AddFormula("UILabel.overflowMethod", 0).LocalContent.Int = (int)UILabel.overflowMethod;
                    f_style.AddFormula("UILabel.pivot", 0).LocalContent.Int = (int)UILabel.pivot;
                    f_style.AddFormula("UILabel.depth", 0).LocalContent.Int = UILabel.depth;
                    f_style.AddFormula("UILabel.fontSize", 0).LocalContent.Int = UILabel.fontSize;
                    f_style.AddFormula("UILabel.fontStyle", 0).LocalContent.Int = (int)UILabel.fontStyle;
                    f_style.AddFormula("UILabel.effectStyle", 0).LocalContent.Int = (int)UILabel.effectStyle;
                    f_style.AddFormula("UILabel.effectColor", 0).LocalContent.color = UILabel.effectColor;
                    f_style.AddFormula("UILabel.effectDistance", 0).LocalContent.vecter2 = UILabel.effectDistance;
                    f_style.AddFormula("UILabel.spacingX", 0).LocalContent.Int = UILabel.spacingX;
                    f_style.AddFormula("UILabel.spacingX", 0).LocalContent.Int = UILabel.spacingX;
                }

                var UITexture = obj.Transform.GetComponent<UITexture>();
                if (UITexture)
                {
                    f_style.AddFormula("UITexture.mainTexture", 0).LocalContent.texture = UITexture.mainTexture;
                    f_style.AddFormula("UITexture.color", 0).LocalContent.color = UITexture.color;
                    f_style.AddFormula("UITexture.pivot", 0).LocalContent.Int = (int)UITexture.pivot;
                    f_style.AddFormula("UITexture.width", 0).LocalContent.Int = UITexture.width;
                    f_style.AddFormula("UITexture.height", 0).LocalContent.Int = UITexture.height;
                    f_style.AddFormula("UITexture.keepAspectRatio", 0).LocalContent.Int = (int)UITexture.keepAspectRatio;
                    f_style.AddFormula("UITexture.depth", 0).LocalContent.Int = UITexture.depth;
                    f_style.AddFormula("UITexture.border", 0).LocalContent.vecter4 = UITexture.border;
                    f_style.AddFormula("UITexture.uvRect", 0).LocalContent.rect = UITexture.uvRect;
                }

                var UIButton = obj.Transform.GetComponent<UIButton>();
                if (UIButton)
                {
                    f_style.AddFormula("UIButton.isEnabled", 0).LocalContent.Bol = UIButton.isEnabled;
                }

                var UIGrid = obj.Transform.GetComponent<UIGrid>();
                if (UIGrid)
                {
                    f_style.AddFormula("UIGrid.arrangement", 0).LocalContent.Int = (int)UIGrid.arrangement;
                    f_style.AddFormula("UIGrid.cellWidth", 0).LocalContent.Float = UIGrid.cellWidth;
                    f_style.AddFormula("UIGrid.cellHeight", 0).LocalContent.Float = UIGrid.cellHeight;
                    f_style.AddFormula("UIGrid.sorting", 0).LocalContent.Int = (int)UIGrid.sorting;
                    f_style.AddFormula("UIGrid.pivot", 0).LocalContent.Int = (int)UIGrid.pivot;
                    f_style.AddFormula("UIGrid.maxPerLine", 0).LocalContent.Int = (int)UIGrid.maxPerLine;
                    f_style.AddFormula("UIGrid.animateSmoothly", 0).LocalContent.Bol = UIGrid.animateSmoothly;
                    f_style.AddFormula("UIGrid.hideInactive", 0).LocalContent.Bol = UIGrid.hideInactive;
                    f_style.AddFormula("UIGrid.keepWithinPanel", 0).LocalContent.Bol = UIGrid.keepWithinPanel;
                }

                var UIScrollView = obj.Transform.GetComponent<UIScrollView>();
                if (UIScrollView)
                {
                    f_style.AddFormula("UIScrollView.contentPivot", 0).LocalContent.Int = (int)UIScrollView.contentPivot;
                    f_style.AddFormula("UIScrollView.movement", 0).LocalContent.Int = (int)UIScrollView.movement;
                }

                var UIPanel = obj.Transform.GetComponent<UIPanel>();
                if (UIPanel)
                {
                    f_style.AddFormula("UIPanel.depth", 0).LocalContent.Int = UIPanel.depth;
                    f_style.AddFormula("UIPanel.clipping", 0).LocalContent.Int = (int)UIPanel.clipping;
                    f_style.AddFormula("UIPanel.clipOffset", 0).LocalContent.vecter2 = UIPanel.clipOffset;
                    f_style.AddFormula("UIPanel.clipSoftness", 0).LocalContent.vecter2 = UIPanel.clipSoftness;
                    f_style.AddFormula("UIPanel.clipTexture", 0).LocalContent.texture2D = UIPanel.clipTexture;
                    f_style.AddFormula("UIPanel.width", 0).LocalContent.Float = UIPanel.width;
                    f_style.AddFormula("UIPanel.height", 0).LocalContent.Float = UIPanel.height;
                    f_style.AddFormula("UIPanel.alpha", 0).LocalContent.Float = UIPanel.alpha;
                    f_style.AddFormula("UIPanel.clipRange", 0).LocalContent.vecter4 = UIPanel.clipRange;
                }

                var TweenAlpha = obj.Transform.GetComponent<TweenAlpha>();
                if (TweenAlpha)
                {
                    f_style.AddFormula("TweenAlpha.from", 0).LocalContent.Float = TweenAlpha.from;
                    f_style.AddFormula("TweenAlpha.to", 0).LocalContent.Float = TweenAlpha.to;

                    f_style.AddFormula("TweenAlpha.animationCurve", 0).LocalContent.animationCurve = TweenAlpha.animationCurve;
                    f_style.AddFormula("TweenAlpha.style", 0).LocalContent.Int = (int)TweenAlpha.style;
                    f_style.AddFormula("TweenAlpha.duration", 0).LocalContent.Float = TweenAlpha.duration;
                    f_style.AddFormula("TweenAlpha.delay", 0).LocalContent.Float = TweenAlpha.delay;
                }

                var TweenColor = obj.Transform.GetComponent<TweenColor>();
                if (TweenColor)
                {
                    f_style.AddFormula("TweenColor.from", 0).LocalContent.color = TweenColor.from;
                    f_style.AddFormula("TweenColor.to", 0).LocalContent.color = TweenColor.to;

                    f_style.AddFormula("TweenColor.animationCurve", 0).LocalContent.animationCurve = TweenColor.animationCurve;
                    f_style.AddFormula("TweenColor.style", 0).LocalContent.Int = (int)TweenColor.style;
                    f_style.AddFormula("TweenColor.duration", 0).LocalContent.Float = TweenColor.duration;
                    f_style.AddFormula("TweenColor.delay", 0).LocalContent.Float = TweenColor.delay;
                }

                var TweenPosition = obj.Transform.GetComponent<TweenPosition>();
                if (TweenPosition)
                {
                    f_style.AddFormula("TweenPosition.from", 0).LocalContent.vecter3 = TweenPosition.from;
                    f_style.AddFormula("TweenPosition.to", 0).LocalContent.vecter3 = TweenPosition.to;

                    f_style.AddFormula("TweenPosition.animationCurve", 0).LocalContent.animationCurve = TweenPosition.animationCurve;
                    f_style.AddFormula("TweenPosition.style", 0).LocalContent.Int = (int)TweenPosition.style;
                    f_style.AddFormula("TweenPosition.duration", 0).LocalContent.Float = TweenPosition.duration;
                    f_style.AddFormula("TweenPosition.delay", 0).LocalContent.Float = TweenPosition.delay;
                }

                var TweenScale = obj.Transform.GetComponent<TweenScale>();
                if (TweenScale)
                {
                    f_style.AddFormula("TweenScale.from", 0).LocalContent.vecter3 = TweenScale.from;
                    f_style.AddFormula("TweenScale.to", 0).LocalContent.vecter3 = TweenScale.to;

                    f_style.AddFormula("TweenScale.animationCurve", 0).LocalContent.animationCurve = TweenScale.animationCurve;
                    f_style.AddFormula("TweenScale.style", 0).LocalContent.Int = (int)TweenScale.style;
                    f_style.AddFormula("TweenScale.duration", 0).LocalContent.Float = TweenScale.duration;
                    f_style.AddFormula("TweenScale.delay", 0).LocalContent.Float = TweenScale.delay;
                }

                var TweenRotation = obj.Transform.GetComponent<TweenRotation>();
                if (TweenRotation)
                {
                    f_style.AddFormula("TweenRotation.from", 0).LocalContent.vecter3 = TweenRotation.from;
                    f_style.AddFormula("TweenRotation.to", 0).LocalContent.vecter3 = TweenRotation.to;

                    f_style.AddFormula("TweenRotation.animationCurve", 0).LocalContent.animationCurve = TweenRotation.animationCurve;
                    f_style.AddFormula("TweenRotation.style", 0).LocalContent.Int = (int)TweenRotation.style;
                    f_style.AddFormula("TweenRotation.duration", 0).LocalContent.Float = TweenRotation.duration;
                    f_style.AddFormula("TweenRotation.delay", 0).LocalContent.Float = TweenRotation.delay;
                }

                obj.Formula.DesFormula(style.ID);
                obj.Formula.AddFormula(style.ID, f_style);
            }
        }
        static void read(LayoutStyle m, string StyleID)
        {


            void push(string key, Service.Formula formula, System.Action<Service.Formula.LocalPackageContent> action)
            {
                // ป้องกัน หากมีการ set key ignore เอาไว้
                if (m.setting.ignore.Contains(key))
                    return;

                // ป้องกัน หากมีไม่มีการ set key เอาไว้ใน formula จะไม่ใส่ค่าลงไปใน ui
                var data = formula.GetFormula(key);
                if (data.isHave)
                {
                    action?.Invoke(data.LocalContent);
                }
            }


            m.lastcurrent = m.StyleDatas.Find(x => x.ID == StyleID);
            //Debug.Log("read : " + StyleID);
            foreach (var obj in m.LayoutDatas)
            {
                if (obj.Transform != null)
                    if (obj.Formula.GetFormulaDatas.FindIndex(x => x.FormulaName == StyleID) != -1)
                    {
                        Service.Formula f_style = obj.Formula.GetFormula(StyleID).SubFormula;

                        if (m.setting.animation)
                        {
                            // Animation
                            var animation = obj.Transform.GetComponent<Animation>();
                            if (animation)
                            {
                                push("animation.clip", f_style, (con) => {
                                    animation.clip = con.animclip;
                                    animation.Play();
                                });
                            }
                        }
                        if (m.setting.lable)
                        {
                            var UILabel = obj.Transform.GetComponent<UILabel>();
                            if (UILabel)
                            {
                                if (m.setting.lb_text) push("UILabel.lb_text", f_style, (con) => { UILabel.text = con.String; });
                                if (m.setting.pivot) push("UILabel.pivot", f_style, (con) => { UILabel.pivot = (UIWidget.Pivot)con.Int; });
                                if (m.setting.depth) push("UILabel.depth", f_style, (con) => { UILabel.depth = con.Int; });
                                if (m.setting.color) push("UILabel.color", f_style, (con) => { UILabel.color = con.color; });

                                push("UILabel.width", f_style, (con) => { UILabel.width = con.Int; });
                                push("UILabel.height", f_style, (con) => { UILabel.height = con.Int; });
                                push("UILabel.overflowMethod", f_style, (con) => { UILabel.overflowMethod = (UILabel.Overflow)con.Int; });
                                push("UILabel.fontSize", f_style, (con) => { UILabel.fontSize = con.Int; });
                                push("UILabel.fontStyle", f_style, (con) => { UILabel.fontStyle = (FontStyle)con.Int; });
                                push("UILabel.effectStyle", f_style, (con) => { UILabel.effectStyle = (UILabel.Effect)con.Int; });
                                push("UILabel.effectColor", f_style, (con) => { UILabel.effectColor = con.color; });
                                push("UILabel.effectDistance", f_style, (con) => { UILabel.effectDistance = con.vecter2; });
                                push("UILabel.spacingX", f_style, (con) => { UILabel.spacingX = con.Int; });
                                push("UILabel.spacingY", f_style, (con) => { UILabel.spacingY = con.Int; });



                            }

                        }
                        if (m.setting.texture)
                        {
                            var UITexture = obj.Transform.GetComponent<UITexture>();
                            if (UITexture)
                            {
                                if (m.setting.texture_img) push("UITexture.mainTexture", f_style, (con) => { UITexture.mainTexture = con.texture; });
                                if (m.setting.pivot) push("UITexture.pivot", f_style, (con) => { UITexture.pivot = (UIWidget.Pivot)con.Int; });
                                if (m.setting.keepAspectRatio) push("UITexture.keepAspectRatio", f_style, (con) => { UITexture.keepAspectRatio = (UIWidget.AspectRatioSource)con.Int; });
                                if (m.setting.color) push("UITexture.color", f_style, (con) => { UITexture.color = con.color; });
                                if (m.setting.depth) push("UITexture.depth", f_style, (con) => { UITexture.depth = con.Int; });
                                if (m.setting.texture_size) push("UITexture.width", f_style, (con) => { UITexture.width = con.Int; });
                                if (m.setting.texture_size) push("UITexture.height", f_style, (con) => { UITexture.height = con.Int; });
                                if (m.setting.texture_border) push("UITexture.border", f_style, (con) => { UITexture.border = con.vecter4; });
                                if (m.setting.texture_rect) push("UITexture.uvRect", f_style, (con) => { UITexture.uvRect = con.rect; });

                            }
                        }
                        UIButton UIButton = null;
                        if (m.setting.btn)
                        {
                            UIButton = obj.Transform.GetComponent<UIButton>();
                            if (UIButton)
                            {
                                push("UIButton.isEnabled", f_style, (con) => { UIButton.isEnabled = con.Bol; });
                            }
                        }
                        if (m.setting.grid)
                        {
                            var UIGrid = obj.Transform.GetComponent<UIGrid>();
                            if (UIGrid)
                            {


                                //วิธีใส่ค่าเข้าไปในตัวแปรแบบใหม่ <ค่อยๆ ถทอยๆแก้>
                                //มันช่วยเช็คด้วยว่า formula key นั้นมีค่า ถ้า !isHave ก็จะไม่ใส่ค่า 0 ลงไปในฟิวนั้นๆ เพราะว่าไม่ได้ set มา เป็นการป้องกันอีกชั้น
                                //อนาคตหากเปลี่ยนมาใช้แบบใหม่นี้หมด สามารถสร้าง list string เพื่อ ignore การ read ได้ เพราะมัน read ไปที่เดียวกันหมด กรณีอย่าง ignore การ read ค่าใดๆใน style
                                // +++++!! หรือ การเพิ่ม ตัวแปลใหม่ๆ เข้าไปในอนาคต แล้วเป็น style ที่ถูก set ไปแล้ว พอload มันจะได้ 0 หรือ ค่าว่าง การใช้ push จะช่วยป้องกันได้


                                if (m.setting.pivot) push("UIGrid.pivot", f_style, (con) => { UIGrid.pivot = (UIWidget.Pivot)con.Int; });
                                push("UIGrid.sorting", f_style, (con) => { UIGrid.sorting = (UIGrid.Sorting)con.Int; });
                                push("UIGrid.arrangement", f_style, (con) => { UIGrid.arrangement = (UIGrid.Arrangement)con.Int; });
                                push("UIGrid.cellWidth", f_style, (con) => { UIGrid.cellWidth = con.Float; });
                                push("UIGrid.cellHeight", f_style, (con) => { UIGrid.cellHeight = con.Float; });
                                push("UIGrid.animateSmoothly", f_style, (con) => { UIGrid.animateSmoothly = con.Bol; });
                                push("UIGrid.hideInactive", f_style, (con) => { UIGrid.hideInactive = con.Bol; });
                                push("UIGrid.keepWithinPanel", f_style, (con) => { UIGrid.keepWithinPanel = con.Bol; });
                                push("UIGrid.maxPerLine", f_style, (con) => { UIGrid.maxPerLine = con.Int; });

                            }
                        }
                        if (m.setting.scrollview)
                        {
                            var UIScrollView = obj.Transform.GetComponent<UIScrollView>();
                            if (UIScrollView)
                            {
                                push("UIScrollView.contentPivot", f_style, (con) => { UIScrollView.contentPivot = (UIWidget.Pivot)con.Int; });
                                push("UIScrollView.movement", f_style, (con) => { UIScrollView.movement = (UIScrollView.Movement)con.Int; });
                            }
                        }
                        if (m.setting.panel)
                        {
                            var UIPanel = obj.Transform.GetComponent<UIPanel>();
                            if (UIPanel)
                            {
                                if (m.setting.depth) push("UIPanel.depth", f_style, (con) => { UIPanel.depth = con.Int; });
                                if (m.setting.color) push("UIPanel.alpha", f_style, (con) => { UIPanel.alpha = con.Float; });

                                push("UIPanel.clipping", f_style, (con) => { UIPanel.clipping = (UIDrawCall.Clipping)con.Int; });
                                push("UIPanel.clipOffset", f_style, (con) => { UIPanel.clipOffset = con.vecter2; });
                                push("UIPanel.clipSoftness", f_style, (con) => { UIPanel.clipSoftness = con.vecter2; });
                                push("UIPanel.clipTexture", f_style, (con) => { UIPanel.clipTexture = con.texture2D; });
                                push("UIPanel.clipRange", f_style, (con) => { UIPanel.clipRange = con.vecter4; });

                            }
                        }
                        if (m.setting.tween)
                        {
                            var TweenAlpha = obj.Transform.GetComponent<TweenAlpha>();
                            if (TweenAlpha)
                            {
                                push("TweenAlpha.from", f_style, (con) => { TweenAlpha.from = con.Float; });
                                push("TweenAlpha.to", f_style, (con) => { TweenAlpha.to = con.Float; });
                                push("TweenAlpha.animationCurve", f_style, (con) => { TweenAlpha.animationCurve = con.animationCurve; });
                                push("TweenAlpha.style", f_style, (con) => { TweenAlpha.style = (UITweener.Style)con.Int; });
                                push("TweenAlpha.duration", f_style, (con) => { TweenAlpha.duration = con.Float; });
                                push("TweenAlpha.delay", f_style, (con) => { TweenAlpha.delay = con.Float; });
                            }

                            var TweenColor = obj.Transform.GetComponent<TweenColor>();
                            if (TweenColor && !UIButton)
                            {
                                push("TweenColor.from", f_style, (con) => { TweenColor.from = con.color; });
                                push("TweenColor.to", f_style, (con) => { TweenColor.to = con.color; });
                                push("TweenColor.animationCurve", f_style, (con) => { TweenColor.animationCurve = con.animationCurve; });
                                push("TweenColor.style", f_style, (con) => { TweenColor.style = (UITweener.Style)con.Int; });
                                push("TweenColor.duration", f_style, (con) => { TweenColor.duration = con.Float; });
                                push("TweenColor.delay", f_style, (con) => { TweenColor.delay = con.Float; });
                            }

                            var TweenPosition = obj.Transform.GetComponent<TweenPosition>();
                            if (TweenPosition)
                            {
                                push("TweenPosition.from", f_style, (con) => { TweenPosition.from = con.vecter3; });
                                push("TweenPosition.to", f_style, (con) => { TweenPosition.to = con.vecter3; });
                                push("TweenPosition.animationCurve", f_style, (con) => { TweenPosition.animationCurve = con.animationCurve; });
                                push("TweenPosition.style", f_style, (con) => { TweenPosition.style = (UITweener.Style)con.Int; });
                                push("TweenPosition.duration", f_style, (con) => { TweenPosition.duration = con.Float; });
                                push("TweenPosition.delay", f_style, (con) => { TweenPosition.delay = con.Float; });
                            }

                            var TweenScale = obj.Transform.GetComponent<TweenScale>();
                            if (TweenScale)
                            {
                                push("TweenScale.from", f_style, (con) => { TweenScale.from = con.vecter3; });
                                push("TweenScale.to", f_style, (con) => { TweenScale.to = con.vecter3; });
                                push("TweenScale.animationCurve", f_style, (con) => { TweenScale.animationCurve = con.animationCurve; });
                                push("TweenScale.style", f_style, (con) => { TweenScale.style = (UITweener.Style)con.Int; });
                                push("TweenScale.duration", f_style, (con) => { TweenScale.duration = con.Float; });
                                push("TweenScale.delay", f_style, (con) => { TweenScale.delay = con.Float; });
                            }

                            var TweenRotation = obj.Transform.GetComponent<TweenRotation>();
                            if (TweenRotation)
                            {
                                push("TweenRotation.from", f_style, (con) => { TweenRotation.from = con.vecter3; });
                                push("TweenRotation.to", f_style, (con) => { TweenRotation.to = con.vecter3; });
                                push("TweenRotation.animationCurve", f_style, (con) => { TweenRotation.animationCurve = con.animationCurve; });
                                push("TweenRotation.style", f_style, (con) => { TweenRotation.style = (UITweener.Style)con.Int; });
                                push("TweenRotation.duration", f_style, (con) => { TweenRotation.duration = con.Float; });
                                push("TweenRotation.delay", f_style, (con) => { TweenRotation.delay = con.Float; });
                            }
                        }

                        // Transform
                        push("activeSelf", f_style, (con) => { obj.Transform.gameObject.SetActive(con.Bol); });
                        if (m.setting.position) push("localPosition", f_style, (con) => { obj.Transform.localPosition = con.vecter3; });
                        if (m.setting.rotate) push("localRotation", f_style, (con) => { obj.Transform.localRotation = con.quaternion; });
                        if (m.setting.scale) push("localScale", f_style, (con) => { obj.Transform.localScale = con.vecter3; });
                    }
                    else
                    {
                        Debug.LogError("Not Found StyleID : " + StyleID);
                    }
            }
        }

    }


}
