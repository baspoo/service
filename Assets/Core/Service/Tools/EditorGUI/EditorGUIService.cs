﻿#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.Linq;


public class EditorGUIService 
{
	


	//Label Style
	static void DoLableStyle(string text, GUIStyle style, Color? color = null ,TextAnchor anc = TextAnchor.MiddleLeft)
	{
		if (color != null)
			GUI.contentColor = (Color)color;
		style.alignment = anc;
		EditorGUILayout.LabelField(text, style);
		if (color != null)
			GUI.contentColor = Color.white;
	}
	//public static void LableStyle(string text, int size = 0 ,Color? color = null, FontStyle font = FontStyle.Normal, TextAnchor anc = TextAnchor.MiddleLeft)
	//{
	//	if (color != null)
	//		GUI.contentColor = (Color)color;

	//	var style = new GUIStyle();
	//	style.alignment = anc;
	//	style.fontStyle = font;
	//	style.fontSize = size;
	//	style.c
	//	EditorGUILayout.LabelField(text, style);
	//	if (color != null)
	//		GUI.contentColor = Color.white;
	//}
	public static void LableBlod(string text, Color? color = null, TextAnchor anc = TextAnchor.MiddleLeft) => DoLableStyle(text, EditorStyles.boldLabel, color, anc); 
	public static void LableHeader(string text , Color? color = null, TextAnchor anc = TextAnchor.MiddleLeft) => DoLableStyle(text, GUIstylePackage.Instant.Header , color, anc);
	public static void LableHeaderBig(string text, Color? color = null, TextAnchor anc = TextAnchor.MiddleLeft) => DoLableStyle(text, GUIstylePackage.Instant.HeaderBig, color, anc);
	//public static void LableHeaderBigCenter(string text, Color? color = null, TextAnchor anc = TextAnchor.MiddleLeft) => DoLableStyle(text, GUIstylePackage.Instant.HeaderBigCenter, color , anc);
	public static bool Button(string TextOrImage, Color? color = null) 
	{
		bool click = false;
		if (color != null)
			GUI.backgroundColor = (Color)color;
		var icon = EditorGUIUtility.FindTexture(TextOrImage);
		if (icon != null) 
			click = GUILayout.Button(icon);
		else 
			click = GUILayout.Button(TextOrImage);

		if (color != null)
			GUI.backgroundColor = Color.white;
		return click;
	}

	public static string StringBuffer => EditorGUIUtility.systemCopyBuffer;







	public class GUIData
	{
		string dataName;
		public enum castTime { realTime, castDelay, unclear }
		public GUIData(string dataName, castTime castTime)
		{
			this.dataName = dataName;
			this.mCastTime = castTime;
		}

		public List<Data> datas = new List<Data>();
		public Data Get(string key)
		{
			var d = datas.Find(x => x.key == key);
			if (d == null)
			{
				d = new Data(key);
				datas.Add(d);
			}
			return d;
		}

		public class Data
		{
			public string key { get; private set; }
			public Data(string key)
			{
				this.key = key;
			}

			string mValue;
			public string value
			{
				get
				{
					return mValue;
				}
				set
				{
					mValue = value;
				}
			}
			public bool boo
			{
				get
				{
					return value != null;
				}
				set
				{
					this.value = value ? string.Empty : null;
				}
			}
		}

		string getkey(string key) => $"{dataName}-{key}";

		public void ClearCache()
		{
			dictJson.Clear();
			dictListObject.Clear();
			dictDictObject.Clear();
		}

		castTime mCastTime;
		float time = 0.0f;

		bool IsGetRealTime()
		{
			if (mCastTime == castTime.realTime)
			{
				return true;
			}
			else
			{
				if (mCastTime == castTime.castDelay)
				{
					if (time < Time.time)
					{
						ClearCache();
						time = Time.time + 5.0f;
					}
				}
				return false;
			}
		}

		Dictionary<object, string> dictJson = new Dictionary<object, string>();
		Dictionary<object, List<object>> dictListObject = new Dictionary<object, List<object>>();
		Dictionary<object, Dictionary<string, object>> dictDictObject = new Dictionary<object, Dictionary<string, object>>();
		string getJson(object obj)
		{
			if (IsGetRealTime())
			{
				return obj.SerializeToJson(SerializeHandle.IgnoreReadonly);
			}
			else
			{
				if (dictJson.ContainsKey(obj))
				{
					return dictJson[obj];
				}
				else
				{
					var json = obj.SerializeToJson(SerializeHandle.IgnoreReadonly);
					dictJson.Add(obj, json);
					return json;
				}
			}
		}
		List<object> getListObject(object obj, string json)
		{
			if (IsGetRealTime())
			{
				List<object> let = json.DeserializeObject<List<object>>();
				return let;
			}
			else
			{
				if (dictListObject.ContainsKey(obj))
				{
					return dictListObject[obj];
				}
				else
				{
					List<object> let = json.DeserializeObject<List<object>>();
					dictListObject.Add(obj, let);
					return let;
				}
			}
		}
		Dictionary<string, object> getDictObject(object obj, string json)
		{
			if (IsGetRealTime())
			{
				Dictionary<string, object> let = json.DeserializeObject<Dictionary<string, object>>();
				return let;
			}
			else
			{
				if (dictDictObject.ContainsKey(obj))
				{
					return dictDictObject[obj];
				}
				else
				{
					Dictionary<string, object> let = json.DeserializeObject<Dictionary<string, object>>();
					dictDictObject.Add(obj, let);
					return let;
				}
			}
		}


		Color ccHeader = Color.white;
		Color ccContent = Color.white;
		public void SetColorHeader(Color cc)
		{
			ccHeader = cc;
		}
		public void SetColorContent(Color cc)
		{
			ccContent = cc;
		}


		public string TextBox(string key, bool permanence = false)
		{
			if (!permanence)
			{
				var k = getkey(key);
				var val = Get(k);
				GUI.color = ccContent;
				val.value = EditorGUILayout.TextField(key, val.value);
				GUI.color = Color.white;
				return val.value;
			}
			else
			{
				var k = getkey(key);
				GUI.color = ccContent;
				var val = EditorGUILayout.TextField(key, EditorPrefs.GetString(k));
				GUI.color = Color.white;
				EditorPrefs.SetString(k, val);
				return val;
			}
		}
		public string TextOnly(string key)
		{
			var k = getkey(key);
			var val = Get(k);
			GUI.color = ccContent;
			val.value = EditorGUILayout.TextField(val.value);
			GUI.color = Color.white;
			return val.value;
		}
		public string TextArea(string key, bool permanence = false)
		{
			if (!permanence)
			{
				var k = getkey(key);
				var val = Get(k);
				EditorStyles.textField.wordWrap = true;
				GUI.color = ccContent;
				val.value = EditorGUILayout.TextArea(val.value, GUILayout.MinHeight(100), GUILayout.MaxHeight(100));
				GUI.color = Color.white;
				return val.value;
			}
			else
			{
				var k = getkey(key);
				EditorStyles.textField.wordWrap = true;
				GUI.color = ccContent;
				var val = EditorGUILayout.TextArea(EditorPrefs.GetString(k), GUILayout.MinHeight(100), GUILayout.MaxHeight(100));
				GUI.color = Color.white;
				EditorPrefs.SetString(k, val);
				return val;
			}
		}
		public double NumberBox(string key, bool permanence = false)
		{
			if (!permanence)
			{
				var k = getkey(key);
				var val = Get(k);
				var num = val.value.ToDouble();
				GUI.color = ccContent;
				num = EditorGUILayout.DoubleField(key, num);
				GUI.color = Color.white;
				val.value = num.ToString();
				return num;
			}
			else
			{
				var k = getkey(key);
				var num = EditorPrefs.GetString(k).ToDouble();
				GUI.color = ccContent;
				num = EditorGUILayout.DoubleField(key, num);
				GUI.color = Color.white;
				EditorPrefs.SetString(k, num.ToString());
				return num;
			}
		}
		public bool Toggle(string key)
		{
			var k = getkey(key);
			var val = Get(k);
			val.boo = EditorGUILayout.Toggle(key, val.boo);
			return val.boo;
		}
		public void Label(string message, Color? color = null)
		{
			GUI.color = color == null ? ccContent : (Color)color;
			EditorGUILayout.LabelField(message);
			GUI.color = Color.white;
		}
		public void LabelBlod(string message, Color? color = null)
		{
			GUI.color = color == null ? ccHeader : (Color)color;
			EditorGUILayout.LabelField(message, EditorStyles.boldLabel);
			GUI.color = Color.white;
		}
		public bool Button(string BtnName, System.Action action)
		{
			if (GUILayout.Button(BtnName))
			{
				action?.Invoke();
				return true;
			}
			else return false;
		}
		public bool Topic(string header, System.Action action)
		{
			bool open = false;
			var k = getkey(header);
			EditorGUILayout.Space();
			if (EditorGUIService.DrawHeader(header, k, true, false))
			{
				open = true;
				EditorGUIService.BeginContents(false);
				action?.Invoke();
				EditorGUIService.EndContents();
			}
			EditorGUILayout.Space();
			return open;
		}
		public void SearchBox<T>(string header, Dictionary<string, T> dict, System.Action<T> action)
		{

			var k = getkey(header);
			var valbool = Get(k + "bool");
			EditorGUILayout.Space();
			if (EditorGUIService.DrawHeader(header, k, true, false, new Option()
			{
				Icon = valbool.boo ? "○" : "●",
				Color = Color.gray,
				Exe = () => { valbool.boo = !valbool.boo; }
			}))
			{
				EditorGUIService.BeginContents(false);
				var val = Get(k + "box");
				val.value = EditorGUILayout.TextField("Search", val.value);
				int index = 0;
				if (dict != null)
				{
					foreach (var data in dict)
					{
						if (val.value.notnull())
						{


							bool match = false;
							if (val.value == "*")
							{
								match = true;
							}
							else
							{
								if (val.value.Contains(","))
								{
									match = val.value.Contains(data.Key);
								}
								else
								{
									match = data.Key.Contains(val.value);
								}
							}



							if (match)
							{
								EditorGUIService.BeginContents(false);
								EditorGUILayout.LabelField(data.Key);
								action?.Invoke(data.Value);
								EditorGUIService.EndContents();
							}
						}
						else
						{
							if (!valbool.boo)
							{
								EditorGUILayout.TextField(index.ToString(), data.Key);
								index++;
							}
						}
					}
				}
				EditorGUIService.EndContents();
			}
			EditorGUILayout.Space();
		}
		public int DropDown(string header, List<string> items)
		{
			var k = getkey(header);
			var val = Get(k);
			var num = val.value.ToInt();
			num = EditorGUILayout.Popup(header, num, items.ToArray());
			val.value = num.ToString();
			return num;
		}
		public int DropDown(string header, params string[] items)
		{
			var k = getkey(header);
			var val = Get(k);
			var num = val.value.ToInt();
			num = EditorGUILayout.Popup(header, num, items);
			val.value = num.ToString();
			return num;
		}
		public void Print(string header, int data)
		{
			DoObjectList(header, data.ToString());
		}
		public void Print(string header, object data)
		{
			if (data != null)
			{
				var json = getJson(data);
				DoObjectList(header, json);
			}
			else DoObjectList(header, string.Empty);
		}
		public void Print(string header, string data)
		{
			DoObjectList(header, data);
		}

		public void PrintList(string header, object data)
		{
			if (data != null)
			{

				var json = getJson(data);
				if (json.IsJsonArray())
				{
					List(header, json, data);
				}
				else
				{
					Dict(header, json, data);
				}
			}
			else PrintList(header, new List<string>());
		}
		void List(string header, string json, object obj)
		{
			var k = getkey(header);
			if (EditorGUIService.DrawHeader(header, k, true, false))
			{
				List<object> let = getListObject(obj, json);// json.DeserializeObject<List<object>>();
				EditorGUIService.BeginContents(false);
				int index = 0;
				foreach (var d in let)
				{
					var nJson = getJson(d);
					DoObjectList(index.ToString(), nJson);
					index++;
				}
				EditorGUIService.EndContents();
			}
		}
		public void PrintList(string header, List<long> list)
		{

			var k = getkey(header);
			if (EditorGUIService.DrawHeader(header, k, true, false))
			{
				EditorGUIService.BeginContents(false);
				int index = 0;
				if (list != null)
					foreach (var d in list)
					{
						DoObjectList(index.ToString(), d.ToString());
						index++;
					}
				EditorGUIService.EndContents();
			}
		}
		public void PrintList(string header, List<int> list)
		{

			var k = getkey(header);
			if (EditorGUIService.DrawHeader(header, k, true, false))
			{
				EditorGUIService.BeginContents(false);
				int index = 0;
				if (list != null)
					foreach (var d in list)
					{
						DoObjectList(index.ToString(), d.ToString());
						index++;
					}
				EditorGUIService.EndContents();
			}
		}
		public void PrintList(string header, List<string> list)
		{

			var k = getkey(header);
			if (EditorGUIService.DrawHeader(header, k, true, false))
			{
				EditorGUIService.BeginContents(false);
				int index = 0;
				if (list != null)
					foreach (var d in list)
					{
						DoObjectList(index.ToString(), d);
						index++;
					}
				EditorGUIService.EndContents();
			}
		}
		void Dict(string header, string json, object obj)
		{
			var dict = getDictObject(obj, json);
			PrintList(header, dict);
		}
		public void PrintList(string header, Dictionary<string, object> dict)
		{
			var k = getkey(header);
			if (EditorGUIService.DrawHeader(header, k, true, false))
			{
				EditorGUIService.BeginContents(false);
				if (dict != null)
					foreach (var d in dict)
					{
						var json = getJson(d.Value);
						DoObjectList(d.Key, json);
					}
				EditorGUIService.EndContents();
			}
		}
		public void PrintList(string header, Dictionary<string, int> dict)
		{
			var k = getkey(header);
			if (EditorGUIService.DrawHeader(header, k, true, false))
			{
				EditorGUIService.BeginContents(false);
				if (dict != null)
					foreach (var d in dict)
					{
						var json = getJson(d.Value);
						DoObjectList(d.Key, json);
					}
				EditorGUIService.EndContents();
			}
		}

		void DoObjectList(string key, string value)
		{

			if (value.isnull())
			{
				GUI.color = ccContent;
				EditorGUILayout.TextField(key, string.Empty);
				GUI.color = Color.white;
				return;
			}


			if (value.Length <= 20)
			{
				GUI.color = ccContent;
				EditorGUILayout.TextField(key, value);
				GUI.color = Color.white;
			}
			else
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.TextField(key, value);
				if (GUILayout.Button(EditorGUIUtility.FindTexture("ViewToolOrbit"), GUILayout.Width(24.0f)))
				{
					string json = value;
					if (json.IsJson() || json.IsJsonArray())
					{
						try
						{
							var obj = json.DeserializeObject<object>();
							json = obj.SerializeToJson(SerializeHandle.FormattingIndented, SerializeHandle.IgnoreReadonly);
						}
						catch { }
					}


					Popup.OpenLargeTextAreaBox(key, json);

				}
				EditorGUILayout.EndHorizontal();
			}
		}
	}




	public class EditorData 
	{
		string m_key;
		public EditorData(string key) {
			m_key = key;
		}
		public int Value
		{
			get { return EditorPrefs.GetInt(m_key); }
			set { EditorPrefs.SetInt(m_key,value); }
		}
		public string Text
		{
			get { return EditorPrefs.GetString(m_key); }
			set { EditorPrefs.SetString(m_key, value); }
		}
		public bool Bool
		{
			get { return EditorPrefs.GetBool(m_key); }
			set { EditorPrefs.SetBool(m_key, value); }
		}
		public bool isHave
		{
			get { return EditorPrefs.HasKey(m_key); }
		}
	}



	public static string Copyed => EditorGUIUtility.systemCopyBuffer;
	public static void Ping(Object ping) => EditorGUIUtility.PingObject(ping);
	public static Texture2D BtnIcon(string str) => EditorGUIUtility.FindTexture(str);





	public static void OpenDialog(string header, string message, System.Action action)
	{
		if (EditorUtility.DisplayDialog(header, message, "yes", "no")) action?.Invoke();
	}
	public static void NativeFilePath(string filter, string oldpath, System.Action<string> done)
	{
		//string path = EditorUtility.OpenFilePanel("Path", "", "png,jpg");
		string path = EditorUtility.OpenFilePanel("Path", oldpath, filter);
		done(path);
	}
	public static void NativeFileByte(string filter, string oldpath, System.Action<byte[]> done)
	{
		//string path = EditorUtility.OpenFilePanel("Path", "", "png,jpg");
		string path = EditorUtility.OpenFilePanel("Path", oldpath, filter);
		if (path.Length != 0)
		{
			var fileContent = System.IO.File.ReadAllBytes(path);
			done(fileContent);
		}
		else done(null);
	}




	public class Popup : EditorWindow
	{

		Vector2 ScrollView;
		public System.Action Delegate = null;

		public static EditorWindow OpenLargeTextAreaBox(string popupname, string message)
		{
			Vector2 sc = Vector2.zero;
			var pop = new Popup(popupname, () => {
				sc = EditorGUILayout.BeginScrollView(sc);
				EditorStyles.textField.wordWrap = true;
				EditorGUILayout.TextArea(message, GUILayout.MinWidth(580));
				EditorGUILayout.EndScrollView();
			});
			pop.title = popupname;
			pop.minSize = new Vector2(600, 600);
			pop.maxSize = new Vector2(600, 600);
			return pop;
		}
		public static EditorWindow ShowWindow(string popupname, System.Action action)
		{
			var pop = new Popup(popupname, action);
			pop.title = popupname;
			return pop;
		}
		public Popup(string popupname, System.Action action)
		{
			var pop = EditorWindow.GetWindow(typeof(Popup), true, popupname);
			((Popup)pop).Delegate = action;
		}
		void OnGUI()
		{
			if (Delegate != null)
			{
				ScrollView = EditorGUILayout.BeginScrollView(ScrollView);
				Delegate?.Invoke();
				EditorGUILayout.EndScrollView();
			}
		}
	}
	public class Corotine : EditorWindow
	{

		public EditorService.EditorCoroutine EditorCoroutine;
		public static EditorWindow Open(string popupname, IEnumerator action, bool autoClose = false)
		{
			var pop = new Corotine("StartCorotine");
			pop.title = "StartCorotine";
			pop.header = popupname;
			pop.autoClose = autoClose;
			pop.minSize = new Vector2(320, 90);
			pop.maxSize = new Vector2(320, 90);
			pop.DoStartCorotine(action);
			return pop;
		}
		public static EditorService.EditorCoroutine StartCorotine(IEnumerator action)
		{
			return new EditorService.EditorCoroutine(action);
		}
		public Corotine(string popupname)
		{
			var pop = EditorWindow.GetWindow(typeof(Corotine), true, popupname);
		}
		void Update()
		{
			Repaint();
		}
		bool autoClose = false;
		string header;
		int index = 0;
		int maxIndex = 35;
		double speed = 50;
		double timeStamp = 0.0f;
		string timeDisplay = "";
		void updateTime()
		{
			var timeSpan = System.DateTime.Now - startTime;
			var time = timeSpan.TotalMilliseconds - timeStamp;
			if (time > speed)
			{
				timeStamp = timeSpan.TotalMilliseconds;
				time = 0.0f;
				index++;
				if (index >= maxIndex)
					index = 0;
			}
			timeDisplay = Service.Time.UnixTimeStampToWatch((long)timeSpan.TotalSeconds, Service.Time.WatchType.HMS);
		}



		void OnGUI()
		{


			EditorGUIService.BeginContents(false);
			EditorGUILayout.Space();
			//EditorGUILayout.LabelField(header);
			if (runing) EditorGUILayout.LabelField($"{header}  -> Runing...  ({timeDisplay})");
			else EditorGUILayout.LabelField($"{header}  -> Complete");
			EditorGUILayout.Space();
			EditorGUIService.EndContents();

			EditorGUILayout.BeginHorizontal();
			for (int i = 0; i < maxIndex; i++)
			{
				GUI.color = (i == index || !runing) ? Color.green : Color.gray;
				EditorGUILayout.LabelField("▉", GUILayout.Width(6));
				GUI.color = Color.white;
			}
			EditorGUILayout.EndHorizontal();

			if (runing)
			{
				updateTime();
			}


			GUI.backgroundColor = runing ? Color.red : Color.white;
			if (GUILayout.Button(runing ? "Stop" : "Close"))
			{
				Stop();
				Close();
			}
			GUI.backgroundColor = Color.white;
		}
		bool runing = false;
		System.DateTime startTime;
		void DoStartCorotine(IEnumerator action)
		{
			startTime = System.DateTime.Now;
			runing = true;

			EditorCoroutine = new EditorService.EditorCoroutine(action, this, () => {
				runing = false;
				if (autoClose)
					Stop();
			});
		}
		void Stop()
		{
			runing = false;
			if (EditorCoroutine != null)
				EditorCoroutine.Stop();
			EditorCoroutine = null;
		}
	}









	public class AdjustClass {

		/// <summary>
		/// Gets the internal class ID of the specified type.
		/// </summary>

		static public int GetClassID(System.Type type)
		{
			var go = EditorUtility.CreateGameObjectWithHideFlags("Temp", HideFlags.HideAndDontSave);
			var uiSprite = go.AddComponent(type);
			var ob = new SerializedObject(uiSprite);
			var classID = ob.FindProperty("m_Script").objectReferenceInstanceIDValue;
			NGUITools.DestroyImmediate(go);
			return classID;
		}

		/// <summary>
		/// Gets the internal class ID of the specified type.
		/// </summary>

		static public int GetClassID<T>() where T : MonoBehaviour
		{
			return GetClassID(typeof(T));
		}

		/// <summary>
		/// Convenience function that replaces the specified MonoBehaviour with one of specified type.
		/// </summary>

		static public SerializedObject ReplaceClass(MonoBehaviour mb, System.Type type)
		{
			var id = GetClassID(type);
			var ob = new SerializedObject(mb);
			ob.Update();
			ob.FindProperty("m_Script").objectReferenceInstanceIDValue = id;
			ob.ApplyModifiedProperties();
			ob.Update();
			return ob;
		}

		/// <summary>
		/// Convenience function that replaces the specified MonoBehaviour with one of specified class ID.
		/// </summary>

		static public SerializedObject ReplaceClass(MonoBehaviour mb, int classID)
		{
			var ob = new SerializedObject(mb);
			ob.Update();
			ob.FindProperty("m_Script").objectReferenceInstanceIDValue = classID;
			ob.ApplyModifiedProperties();
			ob.Update();
			return ob;
		}

		/// <summary>
		/// Convenience function that replaces the specified MonoBehaviour with one of specified class ID.
		/// </summary>

		static public void ReplaceClass(SerializedObject ob, int classID)
		{
			ob.FindProperty("m_Script").objectReferenceInstanceIDValue = classID;
			ob.ApplyModifiedProperties();
			ob.Update();
		}

		/// <summary>
		/// Convenience function that replaces the specified MonoBehaviour with one of specified class ID.
		/// </summary>

		static public void ReplaceClass(SerializedObject ob, System.Type type)
		{
			ob.FindProperty("m_Script").objectReferenceInstanceIDValue = GetClassID(type);
			ob.ApplyModifiedProperties();
			ob.Update();
		}

		/// <summary>
		/// Convenience function that replaces the specified MonoBehaviour with one of specified type.
		/// </summary>

		static public T ReplaceClass<T>(MonoBehaviour mb) where T : MonoBehaviour
		{
			return ReplaceClass(mb, typeof(T)).targetObject as T;
		}

	}
	public class AdjustTexture {
		private static Texture2D mBackdropTex;
		private static Texture2D mContrastTex;
		private static Texture2D mGradientTex;
		private static Object mPrevious;

		/// <summary>
		/// Returns a blank usable 1x1 white texture.
		/// </summary>

		static public Texture2D blankTexture
		{
			get
			{
				return EditorGUIUtility.whiteTexture;
			}
		}

		/// <summary>
		/// Returns a usable texture that looks like a dark checker board.
		/// </summary>

		static public Texture2D backdropTexture
		{
			get
			{
				if (mBackdropTex == null) mBackdropTex = CreateCheckerTex(
					new Color(0.1f, 0.1f, 0.1f, 0.5f),
					new Color(0.2f, 0.2f, 0.2f, 0.5f));
				return mBackdropTex;
			}
		}

		/// <summary>
		/// Returns a usable texture that looks like a high-contrast checker board.
		/// </summary>

		static public Texture2D contrastTexture
		{
			get
			{
				if (mContrastTex == null) mContrastTex = CreateCheckerTex(
					new Color(0f, 0f, 0f, 0.5f),
					new Color(1f, 1f, 1f, 0.5f));
				return mContrastTex;
			}
		}

		/// <summary>
		/// Gradient texture is used for title bars / headers.
		/// </summary>

		static public Texture2D gradientTexture
		{
			get
			{
				if (mGradientTex == null) mGradientTex = CreateGradientTex();
				return mGradientTex;
			}
		}

		/// <summary>
		/// Create a white dummy texture.
		/// </summary>

		private static Texture2D CreateDummyTex()
		{
			Texture2D tex = new Texture2D(1, 1);
			tex.name = "[Generated] Dummy Texture";
			tex.hideFlags = HideFlags.DontSave;
			tex.filterMode = FilterMode.Point;
			tex.SetPixel(0, 0, Color.white);
			tex.Apply();
			return tex;
		}

		/// <summary>
		/// Create a checker-background texture
		/// </summary>

		private static Texture2D CreateCheckerTex(Color c0, Color c1)
		{
			Texture2D tex = new Texture2D(16, 16);
			tex.name = "[Generated] Checker Texture";
			tex.hideFlags = HideFlags.DontSave;

			for (int y = 0; y < 8; ++y) for (int x = 0; x < 8; ++x) tex.SetPixel(x, y, c1);
			for (int y = 8; y < 16; ++y) for (int x = 0; x < 8; ++x) tex.SetPixel(x, y, c0);
			for (int y = 0; y < 8; ++y) for (int x = 8; x < 16; ++x) tex.SetPixel(x, y, c0);
			for (int y = 8; y < 16; ++y) for (int x = 8; x < 16; ++x) tex.SetPixel(x, y, c1);

			tex.Apply();
			tex.filterMode = FilterMode.Point;
			return tex;
		}

		/// <summary>
		/// Create a gradient texture
		/// </summary>

		private static Texture2D CreateGradientTex()
		{
			Texture2D tex = new Texture2D(1, 16);
			tex.name = "[Generated] Gradient Texture";
			tex.hideFlags = HideFlags.DontSave;

			Color c0 = new Color(1f, 1f, 1f, 0f);
			Color c1 = new Color(1f, 1f, 1f, 0.4f);

			for (int i = 0; i < 16; ++i)
			{
				float f = Mathf.Abs((i / 15f) * 2f - 1f);
				f *= f;
				tex.SetPixel(0, i, Color.Lerp(c0, c1, f));
			}

			tex.Apply();
			tex.filterMode = FilterMode.Bilinear;
			return tex;
		}
	}

	public static class GizmosUtils
	{
		public static void DrawText(GUISkin guiSkin, string text, Vector3 position, Color? color = null, int fontSize = 0, float yOffset = 0)
		{
#if UNITY_EDITOR
			var prevSkin = GUI.skin;
			if (guiSkin == null) { }
			//Debug.LogWarning("editor warning: guiSkin parameter is null");
			else
				GUI.skin = guiSkin;

			GUIContent textContent = new GUIContent(text);

			GUIStyle style = (guiSkin != null) ? new GUIStyle(guiSkin.GetStyle("Label")) : new GUIStyle();
			if (color != null)
				style.normal.textColor = (Color)color;
			if (fontSize > 0)
				style.fontSize = fontSize;

			if (Camera.current == null)
				return;

			Vector2 textSize = style.CalcSize(textContent);
			Vector3 screenPoint = Camera.current.WorldToScreenPoint(position);

			if (screenPoint.z > 0) // checks necessary to the text is not visible when the camera is pointed in the opposite direction relative to the object
			{
				var worldPosition = Camera.current.ScreenToWorldPoint(new Vector3(screenPoint.x - textSize.x * 0.5f, screenPoint.y + textSize.y * 0.5f + yOffset, screenPoint.z));
				UnityEditor.Handles.Label(worldPosition, textContent, style);
			}
			GUI.skin = prevSkin;
#endif
		}
	}


	static public void RegisterUndo(string name, Object obj) { if (obj != null) UnityEditor.Undo.RecordObject(obj, name); }
	static public void RegisterUndo(string name, params Object[] objects) { if (objects != null && objects.Length > 0) UnityEditor.Undo.RecordObjects(objects, name); }


	public class Option
	{
		public string Icon;
		public string Description;
		public Color Color = Color.white;
		public bool ShowAlway;
		public System.Action Exe;
	}
	public static bool DrawHeader(string text, string key, bool forceOn, bool minimalistic, params Option[] options)
	{
		bool state = EditorPrefs.GetBool(key, false);

		if (!minimalistic) GUILayout.Space(3f);
		if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
		GUILayout.BeginHorizontal();
		GUI.changed = false;

		if (minimalistic)
		{
			if (state) text = "\u25BC" + (char)0x200a + text;
			else text = "\u25BA" + (char)0x200a + text;

			GUILayout.BeginHorizontal();
			GUI.contentColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.7f) : new Color(0f, 0f, 0f, 0.7f);
			if (!GUILayout.Toggle(true, text, "PreToolbar2", GUILayout.MinWidth(20f))) state = !state;
			GUI.contentColor = Color.white;
			GUILayout.EndHorizontal();
		}
		else
		{
			text = "<b><size=11>" + text + "</size></b>";
			if (state) text = "\u25BC " + text;
			else text = "\u25BA " + text;
			if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;
		}

		if (options != null)
		{
			foreach (var opt in options)
			{
				if (state || opt.ShowAlway)
				{
					GUI.backgroundColor = opt.Color;
					if (opt.Description.notnull())
					{
						if (GUILayout.Button(new GUIContent(opt.Icon, opt.Description), GUILayout.MaxWidth(20f), GUILayout.MaxHeight(18f)))
						{
							opt.Exe?.Invoke();
						}
					}
					else
					{
						if (GUILayout.Button(opt.Icon, GUILayout.MaxWidth(20f), GUILayout.MaxHeight(18f)))
						{
							opt.Exe?.Invoke();
						}
					}
					GUI.backgroundColor = Color.white;
				}
			}
		}



		if (GUI.changed) EditorPrefs.SetBool(key, state);

		if (!minimalistic) GUILayout.Space(2f);
		GUILayout.EndHorizontal();
		GUI.backgroundColor = Color.white;
		if (!forceOn && !state) GUILayout.Space(3f);
		return state;
	}


	static bool mEndHorizontal = true;
	static public void BeginContents(bool minimalistic)
	{
		if (!minimalistic)
		{
			mEndHorizontal = true;
			GUILayout.BeginHorizontal();
			EditorGUILayout.BeginHorizontal("TextArea", GUILayout.MinHeight(10f));
		}
		else
		{
			mEndHorizontal = false;
			EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10f));
			GUILayout.Space(10f);
		}
		GUILayout.BeginVertical();
		GUILayout.Space(2f);
	}
	static public void EndContents()
	{
		GUILayout.Space(3f);
		GUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();

		if (mEndHorizontal)
		{
			GUILayout.Space(3f);
			GUILayout.EndHorizontal();
		}

		GUILayout.Space(3f);
	}


	static public void BeginEndnable(bool isEnable , System.Action action)
	{
		EditorGUI.BeginDisabledGroup(!isEnable);
		action?.Invoke();
		EditorGUI.EndDisabledGroup();
	}



	public static void SimpleViewObject(object obj, HashSet<string> onlyfield = null)
	{
		if (obj == null)
			return;

		LableBlod(obj.GetType().Name);

		string json = ServiceJson.Json.SerializeObject(obj);
		//Debug.Log(json);
		Dictionary<string, object> dict = ServiceJson.Json.DeserializeObject<Dictionary<string, object>>(json);
		if (dict != null)
			foreach (var data in dict)
			{
				if (onlyfield == null || onlyfield.Contains(data.Key))
				{
					string value = (data.Value is string) ? value = ((string)data.Value) :
					JsonConvert.SerializeObject(data.Value, Formatting.None);
					EditorGUILayout.TextField(data.Key, value);
				}
			}
	}
	public class SimpleViewObjectEdit : EditorWindow
	{
		static object m_data;
		static string m_raw;
		public static void ShowWindow(ref object data)
		{
			m_data = data;
			EditorWindow.GetWindow(typeof(SimpleViewObjectEdit), true, "Object Edit");
		}
		Vector2 ScrollView;
		void OnGUI()
		{
			m_raw = JsonConvert.SerializeObject(m_data, Formatting.Indented);
			EditorStyles.textField.wordWrap = true;
			EditorGUILayout.TextArea(m_raw, GUILayout.Height(200.0f));
			if (GUILayout.Button("Convert"))
			{
				m_data = JsonConvert.DeserializeObject(m_raw);
				Close();
			}
		}
	}

	public static void Dict(Dictionary<string, int> dict)
	{
		foreach (var d in dict)
			EditorGUILayout.IntField(d.Key, d.Value);
	}
	public static void Dict(Dictionary<string, long> dict)
	{
		foreach (var d in dict)
			EditorGUILayout.LongField(d.Key, d.Value);
	}
	public static void Dict(Dictionary<string, double> dict)
	{
		foreach (var d in dict)
			EditorGUILayout.DoubleField(d.Key, d.Value);
	}
	public static void Dict(Dictionary<string, float> dict)
	{
		foreach (var d in dict)
			EditorGUILayout.FloatField(d.Key, d.Value);
	}
	public static void Dict(Dictionary<string, string> dict)
	{
		foreach (var d in dict)
			EditorGUILayout.TextField(d.Key, d.Value);
	}











	public class ListView 
	{
		public enum LineStyle { 
			Once,Group
		}
		public static void Print(string listName, int count, System.Action<int> view, System.Action add, System.Action<int> des , LineStyle lineStyle = LineStyle.Once)
		{
			if (EditorGUIService.DrawHeader(listName, "Print.List." + listName, false, false, new Option()
			{
				Icon = "✚",
				Description = "New item",
				Color = Color.green,
				ShowAlway = false,
				Exe = () => {
					add?.Invoke();
					return;
				}
			}))
			{

				for (int i = 0; i < count; i++)
				{
					if (lineStyle == LineStyle.Once)
					{
						EditorGUILayout.BeginHorizontal();
						view?.Invoke(i);
						GUI.backgroundColor = Color.red;
						if (GUILayout.Button("✘"))
						{
							des?.Invoke(i);
							return;
						}
						GUI.backgroundColor = Color.white;
						EditorGUILayout.EndHorizontal();
					}
					else 
					{
						EditorGUILayout.Space();
						GUI.backgroundColor = Color.gray;
						EditorGUIService.BeginContents(false);
						view?.Invoke(i);
						GUI.backgroundColor = Color.red;
						if (GUILayout.Button("✘"))
						{
							des?.Invoke(i);
							return;
						}
						GUI.backgroundColor = Color.white;
						EditorGUIService.EndContents();
					}
				}

			}
		}


		static void head(string name,System.Action action , System.Action add) 
		{
			if (EditorGUIService.DrawHeader(name, "ListView-" + name, false, false))
			{
				EditorGUIService.BeginContents(false);
				{
					action?.Invoke();
					if (GUILayout.Button("+"))
					{
						add?.Invoke();
					}
				}
				EditorGUIService.EndContents();
			}
		}
		static void content(System.Action content,System.Action remove)
		{
			EditorGUILayout.BeginHorizontal();
			content?.Invoke();
			GUI.backgroundColor = Color.red;
			if (GUILayout.Button("X", GUILayout.Width(40.0f)))
			{
				remove?.Invoke();
			}
			GUI.backgroundColor = Color.white;
			EditorGUILayout.EndHorizontal();
		}
		//ตัวอย่างการใช้ Other Objects
		static void example() {

			List<GameObject> list = new List<GameObject>();

			EditorGUIService.ListView.Objects("test", list.Count, (i) => {
				//ในช่องแนวนอนนั้นอยากให้เก็บฟิลอะไรบ้าง ได้มากกว่า 1 คอลลั้ม.... 
				list[i] = (GameObject)EditorGUILayout.ObjectField(list[i], typeof(GameObject));
			}, () => {
				list.Add(null);
			}, (i) => {
				list[i] = null;
			});

		}
		public static void Objects( string name , int count , System.Action<int> contentlist, System.Action add, System.Action<int> remove)
		{
			head(name, () => {
				for (int i = 0; i < count ; i++)
				{
					content(() => {
						contentlist?.Invoke(i);
					}, () => { remove?.Invoke(i); });
				}
			}, () => { add?.Invoke(); });
		}


		public static void GameObjects(string name, List<GameObject> list)
		{
			head(name, () => {
				if(list!=null)
				for (int i = 0; i < list.Count; i++)
				{
					content(() => {
						list[i] = (GameObject)EditorGUILayout.ObjectField(list[i], typeof(GameObject));
					}, () => { list.Remove(list[i]); });
				}
			}, () => { list.Add(null); });
		}
		public static void Transforms(string name, List<Transform> list)
		{
			head(name, () => {
				if (list != null) 
				for (int i = 0; i < list.Count; i++)
				{
					content(() => {
						list[i] = (Transform)EditorGUILayout.ObjectField(list[i], typeof(Transform));
					}, () => { list.Remove(list[i]); });
				}
			}, () => { list.Add(null); });
		}
		public static void Ints(string name, List<int> list){
			head(name, () => {
				if (list != null) for (int i = 0; i < list.Count; i++){content(() => {
						list[i] = EditorGUILayout.IntField(list[i]);
					}, () => {list.Remove(list[i]);});}}, () => {list.Add(0);});
		}
		public static void Floats(string name, List<float> list)
		{
			head(name, () => {
				if (list != null) for (int i = 0; i < list.Count; i++)
				{
					content(() => {
						list[i] = EditorGUILayout.FloatField(list[i]);
					}, () => { list.Remove(list[i]); });
				}
			}, () => { list.Add(0); });
		}
		public static void Doubles(string name, List<double> list)
		{
			head(name, () => {
				if (list != null) for (int i = 0; i < list.Count; i++)
				{
					content(() => {
						list[i] = EditorGUILayout.DoubleField(list[i]);
					}, () => { list.Remove(list[i]); });
				}
			}, () => { list.Add(0); });
		}
		public static void Strings(string name,List<string> list)
		{
			head(name, () => {
				if (list != null) for (int i = 0; i < list.Count; i++)
				{
					content(() => {
						list[i] = EditorGUILayout.TextField(list[i]);
					}, () => { list.Remove(list[i]); });
				}
			}, () => { list.Add(string.Empty); });
		}
	}










	static Dictionary<string, string> dict = new Dictionary<string, string>();
	public static string Popuplist(string header, string defaultdata, string[] datas)
	{
		string val = defaultdata;
		if (dict.ContainsKey(header))
			val = dict[header];


		var lit = datas.ToList();
		int indexOld = lit.FindIndex(x => x == val);
		if (indexOld == -1) indexOld = 0;
		indexOld = EditorGUILayout.Popup(indexOld, datas);


		val = datas[indexOld];
		if (dict.ContainsKey(header))
			dict[header] = val;
		else
			dict.Add(header, val);

		return val;
	}
	public static string Popuplist(string header, string defaultdata, List<Object> datas)
	{
		string[] str = new string[datas.Count];
		for (int i = 0; i < str.Length; i++)
		{
			str[i] = (datas[i] == null) ? string.Empty : datas[i].name;
		}
		return Popuplist(header, defaultdata, str);
	}
	public static string Popuplist(string header, string defaultdata, Object[] datas)
	{
		string[] str = new string[datas.Length];
		for (int i = 0; i < str.Length; i++)
		{
			str[i] = (datas[i] == null) ? string.Empty : datas[i].name;
		}
		return Popuplist(header, defaultdata, str);
	}



	//ถ้าจะใช้ UserPopuplist Action ให้มันCall เช่นโหลดของจาก Resource ตลอดเวลามันหนักไป
	//ใช้ Delay ขั้นกลางก่อนสักที จะเบาขึ้นเยอะ / หรือเอาไปขั้นกระบวนการอะไรบางอย่างที่ ไม่อยากให้ทำถี่ๆ
	static Dictionary<string, Object[]> dictobjs = new Dictionary<string, Object[]>();
	static Dictionary<string, System.DateTime> dicttime = new Dictionary<string, System.DateTime>();
	public delegate Object[] callbackObjects();
	public static bool Delay(string header) => Delay(header, 3.0f, null);
	public static bool Delay(string header, System.Action action) => Delay(header, 3.0f, action);
	public static bool Delay(string header, float refresh = 3, System.Action action = null)
	{
		if (!dicttime.ContainsKey(header))
		{
			action?.Invoke();
			dicttime.Add(header, System.DateTime.Now);
			return true;
		}
		else
		{
			var lasttime = dicttime[header];
			bool isTimeout = System.DateTime.Now.AddSeconds(-refresh) > lasttime;
			if (isTimeout)
			{
				action?.Invoke();
				dicttime[header] = System.DateTime.Now;
			}
			return isTimeout;
		}
		return false;
	}

	public static string Popuplist(string header, string defaultdata, callbackObjects action)
	{
		Object[] datas = null;
		if (Delay(header))
		{
			datas = action();

			if (!dictobjs.ContainsKey(header))
				dictobjs.Add(header, datas);
			else
				dictobjs[header] = datas;
		}
		else
		{
			datas = dictobjs[header];
		}

		string[] str = new string[datas.Length];
		for (int i = 0; i < str.Length; i++)
		{
			str[i] = (datas[i] == null) ? string.Empty : datas[i].name;
		}
		return Popuplist(header, defaultdata, str);
	}

	public static void RefreshMonitor()
	{

		Canvas.ForceUpdateCanvases();
		EditorWindow.GetWindow(System.Type.GetType("UnityEditor.GameView,UnityEditor")).Repaint();
		SceneView.RepaintAll();
		EditorWindow.GetWindow<SceneView>().Repaint();
		HandleUtility.Repaint();
	}

}

#endif