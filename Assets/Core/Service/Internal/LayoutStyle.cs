using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CanEditMultipleObjects]
[CustomEditor(typeof(LayoutStyle))]
[System.Serializable]
public class LayoutUI : Editor{
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
            isSetting = EditorGUILayout.ToggleLeft("Setting",isSetting);
            EditorGUILayout.Space();
            if (isSetting) 
            {
                Setting();
                return;
            }
        }



        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (m_tools.iTap == 0) {
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



        m_tools.gameObject.name = m_tools.gameObject.name;
        serializedObject.ApplyModifiedProperties();
        Undo.RecordObject(m_tools, m_tools.name);

    }

    void LayoutView(LayoutStyle.LayoutData data)
    {
        data.ID =  EditorGUILayout.TextField("ID : ",data.ID);
        data.Transform = (Transform)EditorGUILayout.ObjectField("Transform : ", data.Transform , typeof(Transform));

        int del = -1;
        GUI.backgroundColor = Color.red;
        del = GUILayout.SelectionGrid(del, new string[] { "Remove"}, 5);
        GUI.backgroundColor = Color.white;
        if (del != -1) {

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
        if (m_tools.lastcurrent != null) {
            if (m_tools.lastcurrent.ID == data.ID)
                gs.normal.textColor = Color.yellow;
        }
        EditorGUILayout.LabelField(data.ID ,gs);

        int del = -1;
        if (!Application.isPlaying)
        {
            //string a = ((data.isAnim) ? "✔ " : "✘ ") + " Anim";
            del = GUILayout.SelectionGrid(del, new string[] { "Load", "Update", "Remove" },3 );
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

    void Setting() {
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
    }
    public void LoadLayout(string StyleID) 
    {
        Tools.Load(this, StyleID);
    }







    public class Tools 
    {
        public static void Save(LayoutStyle m , string StyleID)
        {
            StyleData style = m.StyleDatas.Find(x => x.ID == StyleID);
            if (style == null)
            {
                style = new StyleData() { ID = StyleID };
                m.StyleDatas.Add(style);
                save(m,style);
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
        public static void RemoveStyle(LayoutStyle m, StyleData Style) {
            m.StyleDatas.Remove(Style);
            foreach (var obj in m.LayoutDatas){
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
                f_style.AddFormula("activeSelf", 0).LocalContent.Bol            = obj.Transform.gameObject.activeSelf;
                f_style.AddFormula("localPosition", 0).LocalContent.vecter3     = obj.Transform.localPosition;
                f_style.AddFormula("localRotation", 0).LocalContent.quaternion  = obj.Transform.localRotation;
                f_style.AddFormula("localScale", 0).LocalContent.vecter3        = obj.Transform.localScale;

                // Animation
                var animation = obj.Transform.GetComponent<Animation>();
                if (animation)
                {
                    f_style.AddFormula("animation.clip", 0).LocalContent.animclip = animation.clip;
                }

                var UILabel = obj.Transform.GetComponent<UILabel>();
                if (UILabel) {
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
            m.lastcurrent = m.StyleDatas.Find(x=>x.ID == StyleID);
            //Debug.Log("read : " + StyleID);
            foreach (var obj in m.LayoutDatas){
                if (obj.Transform!=null)
                    if (obj.Formula.GetFormulaDatas.FindIndex(x => x.FormulaName == StyleID) != -1)
                    {
                        Service.Formula f_style = obj.Formula.GetFormula(StyleID).SubFormula;

                        if (m.setting.animation)
                        {
                            // Animation
                            var animation = obj.Transform.GetComponent<Animation>();
                            if (animation)
                            {
                                animation.clip = f_style.GetFormula("localPosition").LocalContent.animclip;
                                animation.Play();
                            }
                        }
                        if (m.setting.lable)
                        {
                            var UILabel = obj.Transform.GetComponent<UILabel>();
                            if (UILabel)
                            {
                                if (m.setting.lb_text) UILabel.text = f_style.GetFormula("UILabel.text").LocalContent.String;
                                if (m.setting.pivot) UILabel.pivot = (UIWidget.Pivot)f_style.GetFormula("UILabel.pivot").LocalContent.Int;
                                if (m.setting.depth) UILabel.depth = f_style.GetFormula("UILabel.depth").LocalContent.Int;
                                if (m.setting.color) UILabel.color = f_style.GetFormula("UILabel.color").LocalContent.color;
                                UILabel.width = f_style.GetFormula("UILabel.width").LocalContent.Int;
                                UILabel.height = f_style.GetFormula("UILabel.height").LocalContent.Int;
                                UILabel.overflowMethod = (UILabel.Overflow)f_style.GetFormula("UILabel.overflowMethod").LocalContent.Int;
                                UILabel.fontSize = f_style.GetFormula("UILabel.fontSize").LocalContent.Int;
                                UILabel.fontStyle = (FontStyle)f_style.GetFormula("UILabel.fontStyle").LocalContent.Int;
                                UILabel.effectStyle = (UILabel.Effect)f_style.GetFormula("UILabel.effectStyle").LocalContent.Int;
                                UILabel.effectColor = f_style.GetFormula("UILabel.effectColor").LocalContent.color;
                                UILabel.effectDistance = f_style.GetFormula("UILabel.effectDistance").LocalContent.vecter2;
                                UILabel.spacingX = f_style.GetFormula("UILabel.spacingX").LocalContent.Int;
                                UILabel.spacingY = f_style.GetFormula("UILabel.spacingY").LocalContent.Int;
                            }

                        }
                        if (m.setting.texture)
                        {
                            var UITexture = obj.Transform.GetComponent<UITexture>();
                            if (UITexture)
                            {
                                if (m.setting.texture_img) UITexture.mainTexture = f_style.GetFormula("UITexture.mainTexture").LocalContent.texture;
                                if (m.setting.pivot) UITexture.pivot = (UIWidget.Pivot)f_style.GetFormula("UITexture.pivot").LocalContent.Int;
                                if (m.setting.keepAspectRatio) UITexture.keepAspectRatio = (UIWidget.AspectRatioSource)f_style.GetFormula("UITexture.keepAspectRatio").LocalContent.Int;
                                if (m.setting.color) UITexture.color = f_style.GetFormula("UITexture.color").LocalContent.color;
                                if (m.setting.depth) UITexture.depth = f_style.GetFormula("UITexture.depth").LocalContent.Int;
                                if (m.setting.texture_size) UITexture.width = f_style.GetFormula("UITexture.width").LocalContent.Int;
                                if (m.setting.texture_size) UITexture.height = f_style.GetFormula("UITexture.height").LocalContent.Int;
                                if (m.setting.texture_border) UITexture.border = f_style.GetFormula("UITexture.border").LocalContent.vecter4;
                                if (m.setting.texture_rect) UITexture.uvRect = f_style.GetFormula("UITexture.uvRect").LocalContent.rect;
                            }
                        }
                        UIButton UIButton = null;
                        if (m.setting.btn)
                        {
                            UIButton = obj.Transform.GetComponent<UIButton>();
                            if (UIButton)
                            {
                                UIButton.isEnabled = f_style.GetFormula("UIButton.isEnabled").LocalContent.Bol;
                            }
                        }
                        if (m.setting.grid)
                        {
                            var UIGrid = obj.Transform.GetComponent<UIGrid>();
                            if (UIGrid)
                            {
                                if (m.setting.pivot) UIGrid.pivot = (UIWidget.Pivot)f_style.GetFormula("UIGrid.pivot").LocalContent.Int;
                                UIGrid.sorting = (UIGrid.Sorting)f_style.GetFormula("UIGrid.sorting").LocalContent.Int;
                                UIGrid.arrangement = (UIGrid.Arrangement)f_style.GetFormula("UIGrid.arrangement").LocalContent.Int;
                                UIGrid.cellWidth = f_style.GetFormula("UIGrid.cellWidth").LocalContent.Float;
                                UIGrid.cellHeight = f_style.GetFormula("UIGrid.cellHeight").LocalContent.Float;
                                UIGrid.animateSmoothly = f_style.GetFormula("UIGrid.animateSmoothly").LocalContent.Bol;
                                UIGrid.hideInactive = f_style.GetFormula("UIGrid.hideInactive").LocalContent.Bol;
                                UIGrid.keepWithinPanel = f_style.GetFormula("UIGrid.keepWithinPanel").LocalContent.Bol;
                            }
                        }
                        if (m.setting.scrollview)
                        {
                            var UIScrollView = obj.Transform.GetComponent<UIScrollView>();
                            if (UIScrollView)
                            {
                                UIScrollView.contentPivot = (UIWidget.Pivot)f_style.GetFormula("UIScrollView.contentPivot").LocalContent.Int;
                                UIScrollView.movement = (UIScrollView.Movement)f_style.GetFormula("UIScrollView.movement").LocalContent.Int;
                            }
                        }
                        if (m.setting.panel)
                        {
                            var UIPanel = obj.Transform.GetComponent<UIPanel>();
                            if (UIPanel)
                            {
                                if (m.setting.depth) UIPanel.depth = f_style.GetFormula("UIPanel.depth").LocalContent.Int;
                                if (m.setting.color) UIPanel.alpha = f_style.GetFormula("UIPanel.alpha").LocalContent.Float;
                                UIPanel.clipping = (UIDrawCall.Clipping)f_style.GetFormula("UIPanel.clipping").LocalContent.Int;
                                UIPanel.clipOffset = f_style.GetFormula("UIPanel.clipOffset").LocalContent.vecter2;
                                UIPanel.clipSoftness = f_style.GetFormula("UIPanel.clipSoftness").LocalContent.vecter2;
                                UIPanel.clipTexture = f_style.GetFormula("UIPanel.clipTexture").LocalContent.texture2D;
                                UIPanel.clipRange = f_style.GetFormula("UIPanel.clipRange").LocalContent.vecter4;
                            }
                        }
                        if (m.setting.tween)
                        {
                            var TweenAlpha = obj.Transform.GetComponent<TweenAlpha>();
                            if (TweenAlpha)
                            {
                                TweenAlpha.from = f_style.GetFormula("TweenAlpha.from").LocalContent.Float;
                                TweenAlpha.to = f_style.GetFormula("TweenAlpha.to").LocalContent.Float;
                                TweenAlpha.animationCurve = f_style.GetFormula("TweenAlpha.animationCurve").LocalContent.animationCurve;
                                TweenAlpha.style = (UITweener.Style)f_style.GetFormula("TweenAlpha.style").LocalContent.Int;
                                TweenAlpha.duration = f_style.GetFormula("TweenAlpha.duration").LocalContent.Float;
                                TweenAlpha.delay = f_style.GetFormula("TweenAlpha.delay").LocalContent.Float;
                            }

                            var TweenColor = obj.Transform.GetComponent<TweenColor>();
                            if (TweenColor  && !UIButton)
                            {
                                TweenColor.from = f_style.GetFormula("TweenColor.from").LocalContent.color;
                                TweenColor.to = f_style.GetFormula("TweenColor.to").LocalContent.color;
                                TweenColor.animationCurve = f_style.GetFormula("TweenColor.animationCurve").LocalContent.animationCurve;
                                TweenColor.style = (UITweener.Style)f_style.GetFormula("TweenColor.style").LocalContent.Int;
                                TweenColor.duration = f_style.GetFormula("TweenColor.duration").LocalContent.Float;
                                TweenColor.delay = f_style.GetFormula("TweenColor.delay").LocalContent.Float;
                            }

                            var TweenPosition = obj.Transform.GetComponent<TweenPosition>();
                            if (TweenPosition)
                            {
                                TweenPosition.from = f_style.GetFormula("TweenPosition.from").LocalContent.vecter3;
                                TweenPosition.to = f_style.GetFormula("TweenPosition.to").LocalContent.vecter3;
                                TweenPosition.animationCurve = f_style.GetFormula("TweenPosition.animationCurve").LocalContent.animationCurve;
                                TweenPosition.style = (UITweener.Style)f_style.GetFormula("TweenPosition.style").LocalContent.Int;
                                TweenPosition.duration = f_style.GetFormula("TweenPosition.duration").LocalContent.Float;
                                TweenPosition.delay = f_style.GetFormula("TweenPosition.delay").LocalContent.Float;
                            }

                            var TweenScale = obj.Transform.GetComponent<TweenScale>();
                            if (TweenScale)
                            {
                                TweenScale.from = f_style.GetFormula("TweenScale.from").LocalContent.vecter3;
                                TweenScale.to = f_style.GetFormula("TweenScale.to").LocalContent.vecter3;
                                TweenScale.animationCurve = f_style.GetFormula("TweenScale.animationCurve").LocalContent.animationCurve;
                                TweenScale.style = (UITweener.Style)f_style.GetFormula("TweenScale.style").LocalContent.Int;
                                TweenScale.duration = f_style.GetFormula("TweenScale.duration").LocalContent.Float;
                                TweenScale.delay = f_style.GetFormula("TweenScale.delay").LocalContent.Float;
                            }

                            var TweenRotation = obj.Transform.GetComponent<TweenRotation>();
                            if (TweenRotation)
                            {
                                TweenRotation.from = f_style.GetFormula("TweenRotation.from").LocalContent.vecter3;
                                TweenRotation.to = f_style.GetFormula("TweenRotation.to").LocalContent.vecter3;
                                TweenRotation.animationCurve = f_style.GetFormula("TweenPositiTweenRotationon.animationCurve").LocalContent.animationCurve;
                                TweenRotation.style = (UITweener.Style)f_style.GetFormula("TweenRotation.style").LocalContent.Int;
                                TweenRotation.duration = f_style.GetFormula("TweenRotation.duration").LocalContent.Float;
                                TweenRotation.delay = f_style.GetFormula("TweenRotation.delay").LocalContent.Float;
                            }
                        }

                        // Transform
                        obj.Transform.gameObject.SetActive(f_style.GetFormula("activeSelf").LocalContent.Bol);
                        if (m.setting.position) obj.Transform.localPosition = f_style.GetFormula("localPosition").LocalContent.vecter3;
                        if (m.setting.rotate)   obj.Transform.localRotation = f_style.GetFormula("localRotation").LocalContent.quaternion;
                        if (m.setting.scale)    obj.Transform.localScale = f_style.GetFormula("localScale").LocalContent.vecter3;
                    }
                    else
                    {
                        Debug.LogError("Not Found StyleID : " + StyleID);
                    }
            }
        }

    }


}
