using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Obj : MonoBehaviour {
	public static Obj GetObj(GameObject target)
	{
		return (target).GetComponent<Obj>();
	}
	[ContextMenu("Custom-Inspector")]
	public void HideComponent()
	{
		IsCustomInspector = !IsCustomInspector;
	}

	public bool IsCustomInspector;
	public List<string> CustomInspectors = new List<string>();



	[Header("Options")]
	[Header("---------------------------------------------")]
	public int index;
	public string data;
	public double value;
	public bool active;

	public UILabel label_header;
	public UILabel label_name;
	public UILabel label_content;
	public UILabel label_count;
	public UITexture icon;
	public UITexture bg;
	public UI2DSprite spr;
	public UIButton btn;
	public Transform t_select;
	public Service.Var.Behaviours behaviours;
	public Service.Var.Transforms transforms;
	public Service.Var.Values values;
	public Service.Var.Textures imgs;
	public Service.Var.Colors colors;
	

	public class Content {

		public Data data = null;
		public class Data
		{
			public int index;
			public string data;
			public double value;
			public bool active;
		}
		public string header;
		public string name;
		public string content;
		public int count;
		public Texture icon;
		public Texture bg;
		public Sprite spr;
		public bool btnEnable = true;
		public Dictionary<string, object> extensions;
		public System.Action<Obj> onClick;
		public System.Action onUpdate;
	}


	[HideInInspector]
	public object dataobject;

	[HideInInspector]
	public System.Action<Obj> OnClick = null;
	[HideInInspector]
	public System.Action OnUpdate = null;



	public void onClick(){
		if (OnClick != null)
			OnClick(this);
	}
	void Update()
	{
		OnUpdate?.Invoke();
	}



	public void Init(Content con)
	{

        if (con.data != null) 
		{ 
			index = con.data.index;
			value = con.data.value;
			data = con.data.data;
			active = con.data.active;
		}

		if (con.onUpdate != null)
			OnUpdate = con.onUpdate;

		if (con.onClick != null)
			OnClick = con.onClick;


		if (label_header != null) label_header.text = con.header;
		if (label_name != null) label_name.text = con.name;
		if (label_content != null) label_content.text = con.content;
		if (label_count != null) label_count.text = con.count.ToString("#,##0");


		if (icon != null) icon.mainTexture = con.icon;
		if (bg != null) bg.mainTexture = con.bg;
		if (spr != null) spr.sprite2D = con.spr;
		if (btn != null) btn.enabled = con.btnEnable;


		if (con.extensions != null)
		{
			foreach (var ex in con.extensions) 
			{
				var ui = behaviours[ex.Key];
				if (ui != null) 
				{
					var val = ex.Value;
					if (val is string || val is int || val is float || val is double || val is long) 
					{
						((UILabel)ui).text = val.ToString();
					}
					if (val is Texture || val is Texture2D)
					{
						((UITexture)ui).mainTexture = (Texture)val;
					}
					if (val is Sprite)
					{
						((UI2DSprite)ui).sprite2D = (Sprite)val;
					}
					if (val is Color)
					{
						((UIWidget)ui).color = (Color)val;
					}
				}
			}
		}


	}



}










#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(Obj))]
[System.Serializable]
public class ObjUI : Editor
{

	


	void oncustom(Obj m_obj , string key) 
	{
		if (m_obj.IsCustomInspector) 
		{
			var isuse = m_obj.CustomInspectors.Contains(key);
			GUI.backgroundColor = (isuse)?Color.green : Color.white;
			if (GUILayout.Button("+", GUILayout.Width(25.0f)))
			{
				if (!isuse)
				{
					m_obj.CustomInspectors.Add(key);
				}
				else 
				{
					m_obj.CustomInspectors.Remove(key);
				}
			}
			GUI.backgroundColor = Color.white;
		}
	}
	bool iscustom(Obj m_obj, string key , System.Action content)
	{

		bool ishave = m_obj.CustomInspectors.Contains(key);
		bool isDisplay = ishave || m_obj.IsCustomInspector;


		if (isDisplay)
		{
			if (m_obj.IsCustomInspector)
			{
				EditorGUILayout.BeginHorizontal();
				oncustom(m_obj, key);
				content?.Invoke();
				EditorGUILayout.EndHorizontal();
			}
			else 
			{
				content?.Invoke();
			}
		}
		return ishave;
	}
	bool isgroup(Obj m_obj, string key)
	{
		if (m_obj.IsCustomInspector) 
		{
			GUI.backgroundColor = Color.yellow;
			return true;
		}
		foreach (var str in m_obj.CustomInspectors) 
		{
			if (str.IndexOf(key) != -1)
				return true;
		}
		return false;
	}





	public override void OnInspectorGUI()
	{


		Obj m_obj = (Obj)target;




		EditorGUILayout.Space();


		if (EditorGUIService.DrawHeader("Var", "ObjUI-var", false, false))
		{
			EditorGUIService.BeginContents(false);
			{

				m_obj.index = EditorGUILayout.IntField("index", m_obj.index);
				m_obj.value = EditorGUILayout.DoubleField("value", m_obj.value);
				m_obj.data = EditorGUILayout.TextField("data", m_obj.data);
				m_obj.active = EditorGUILayout.Toggle("active", m_obj.active);

			}
			EditorGUIService.EndContents();
		}

		
		if (isgroup(m_obj,"ui_") && EditorGUIService.DrawHeader("UI-Standrad", "ObjUI-standrad", false, false))
		{
			EditorGUIService.BeginContents(false);
			{

				iscustom(m_obj, "ui_header", () => {   m_obj.label_header = (UILabel)EditorGUILayout.ObjectField("lb header", m_obj.label_header, typeof(UILabel));  });
				iscustom(m_obj, "ui_name", () => { m_obj.label_name = (UILabel)EditorGUILayout.ObjectField("lb name", m_obj.label_name, typeof(UILabel)); });

				iscustom(m_obj, "ui_content", () => { m_obj.label_content = (UILabel)EditorGUILayout.ObjectField("lb content", m_obj.label_content, typeof(UILabel)); });
				iscustom(m_obj, "ui_count", () => { m_obj.label_count = (UILabel)EditorGUILayout.ObjectField("lb count", m_obj.label_count, typeof(UILabel)); });

				iscustom(m_obj, "ui_icon", () => { m_obj.icon = (UITexture)EditorGUILayout.ObjectField("img icon", m_obj.icon, typeof(UITexture)); });
				iscustom(m_obj, "ui_bg", () => { m_obj.bg = (UITexture)EditorGUILayout.ObjectField("img bg", m_obj.bg, typeof(UITexture)); });

				iscustom(m_obj, "ui_spr", () => { m_obj.spr = (UI2DSprite)EditorGUILayout.ObjectField("spr", m_obj.spr, typeof(UI2DSprite)); });
				iscustom(m_obj, "ui_btn", () => { m_obj.btn = (UIButton)EditorGUILayout.ObjectField("btn", m_obj.btn, typeof(UIButton)); });
				iscustom(m_obj, "ui_select", () => { m_obj.t_select = (Transform)EditorGUILayout.ObjectField("select", m_obj.t_select, typeof(Transform)); });

				iscustom(m_obj, "ui_extension", () => {
					EditorGUIService.ListView.Objects("UI-Extensions", m_obj.behaviours.Count, (i) => {
						m_obj.behaviours.Contents[i].name = EditorGUILayout.TextField(m_obj.behaviours.Contents[i].name);
						m_obj.behaviours.Contents[i].Class = (Behaviour)EditorGUILayout.ObjectField(m_obj.behaviours.Contents[i].Class, typeof(Behaviour));
					}, () => {
						m_obj.behaviours.Add(string.Empty, null);
					}, (i) => {
						m_obj.behaviours.Contents.Remove(m_obj.behaviours.Contents[i]);
					});
				});

			}
			EditorGUIService.EndContents();
		}



		if (isgroup(m_obj, "option_") && EditorGUIService.DrawHeader("Option", "ObjUI-Option", false, false))
		{
			EditorGUIService.BeginContents(false);
			{
				iscustom(m_obj, "option_imgs", () => {
					EditorGUIService.ListView.Objects("Imgs", m_obj.imgs.Count, (i) => {
						m_obj.imgs.Contents[i].name = EditorGUILayout.TextField(m_obj.imgs.Contents[i].name);
						m_obj.imgs.Contents[i].img = (Texture)EditorGUILayout.ObjectField(m_obj.imgs.Contents[i].img, typeof(Texture));
					}, () => {
						m_obj.imgs.Add(string.Empty, null);
					}, (i) => {
						m_obj.imgs.Contents.Remove(m_obj.imgs.Contents[i]);
					});
				});
				iscustom(m_obj, "option_color", () => {
					EditorGUIService.ListView.Objects("Colors", m_obj.colors.Count, (i) => {
						m_obj.colors.Contents[i].name = EditorGUILayout.TextField(m_obj.colors.Contents[i].name);
						m_obj.colors.Contents[i].color = EditorGUILayout.ColorField(m_obj.colors.Contents[i].color);
					}, () => {
						m_obj.colors.Add(string.Empty, Color.white);
					}, (i) => {
						m_obj.colors.Contents.Remove(m_obj.colors.Contents[i]);
					});
				});
				iscustom(m_obj, "option_value", () => {
					EditorGUIService.ListView.Objects("Values", m_obj.values.Count, (i) => {
						m_obj.values.Contents[i].name = EditorGUILayout.TextField(m_obj.values.Contents[i].name);
						m_obj.values.Contents[i].value = EditorGUILayout.DoubleField(m_obj.values.Contents[i].value);
					}, () => {
						m_obj.values.Add(string.Empty, 0);
					}, (i) => {
						m_obj.values.Contents.Remove(m_obj.values.Contents[i]);
					});
				});
				iscustom(m_obj, "option_transforms", () => {
					EditorGUIService.ListView.Objects("Transforms", m_obj.transforms.Count, (i) => {
						m_obj.transforms.Contents[i].name = EditorGUILayout.TextField(m_obj.transforms.Contents[i].name);
						m_obj.transforms.Contents[i].transform = (Transform)EditorGUILayout.ObjectField(m_obj.transforms.Contents[i].transform, typeof(Transform));
					}, () => {
						m_obj.transforms.Add(string.Empty, null);
					}, (i) => {
						m_obj.transforms.Contents.Remove(m_obj.transforms.Contents[i]);
					});
				});

			}
			EditorGUIService.EndContents();
		}

	} 
}
#endif