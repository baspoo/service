using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(LogTools))]
public class LogToolsUI : Editor{

	[MenuItem(EditorGUIService.ProjectPath.header + "/Editor/Log/LogSetting")]
	public static void OnSelection() {
		Selection.activeObject = LogTools.log.gameObject;
		AssetDatabase.OpenAsset(Selection.activeObject);
		((GameObject)Selection.activeObject).transform.hideFlags = HideFlags.HideInInspector;
	}


	public LogTools m_tools {get{ return ((GameObject)Selection.activeObject).GetComponent<LogTools>();  }}
	public override void OnInspectorGUI()
	{
		Display( );
	}
	public static void Display( )
	{
		EditorGUILayout.Space();
		if (LogTools.log != null)
		{
			LogTools.log.DisplayGUI();
			((GameObject)Selection.activeObject).gameObject.SetActive(true);
			LogTools.log.gameObject.name = LogTools.log.gameObject.name;
			if (GUILayout.Button("Save Tag"))
			{
				Undo.RecordObject(LogTools.log, "Log");
				PrefabUtility.RecordPrefabInstancePropertyModifications(LogTools.log);
				EditorUtility.CopySerialized(LogTools.log, LogTools.log);
				AssetDatabase.SaveAssets();
			}
		}
	}
}
#endif




[System.Serializable]
public class TagData
{
	public string Tag;
	public Color Color;
	public bool Enable;
}
public class LogTools : MonoBehaviour {
	[Header("Setting")]
	public bool IgnoreNotTag;
	public bool IgnoreAll;
	public bool DisableUnityLog;
	public bool IsReportFile;
	[Range(10, 2000)]
	public int LimitCount;
	public List<TagData> Tags = new List<TagData>();
	static LogTools m_log;
	public static LogTools log
	{
		get
		{
			if (m_log == null)
			{
				GameObject logG = Resources.Load("Log") as GameObject;
				m_log = logG.GetComponent<LogTools>();
				m_log.InitAdject();

			}
			return m_log;
		}
	}



	public void ForceOpenLogDebug()
	{
		IgnoreAll = false;
		DisableUnityLog = false;
		IgnoreNotTag = false;
		IsReportFile = false;
		Debug.unityLogger.logEnabled = true;
	}

    public void InitAdject()
    {
        Debug.unityLogger.logEnabled = !DisableUnityLog;
#if UNITY_EDITOR
        LogEditor.LogEditor.Init();
#else
			IgnoreAll = false;
			IgnoreNotTag = false;
			IsReportFile = false;
			ClearLogStatic();
#endif

    }


    string m_LogTagFind = "";
	public void DisplayGUI() {
#if UNITY_EDITOR
		if (EditorGUIService.DrawHeader("Advance Setting", "Log Advance Setting", false, false))
		{
			EditorGUIService.BeginContents(false);
			IgnoreAll = EditorGUILayout.ToggleLeft("IgnoreAll", IgnoreAll);
			IgnoreNotTag = EditorGUILayout.ToggleLeft("IgnoreNotTag", IgnoreNotTag);
			DisableUnityLog = EditorGUILayout.ToggleLeft("DisableUnityLog", DisableUnityLog);
			IsReportFile = EditorGUILayout.ToggleLeft("IsReportFile", IsReportFile);
			EditorGUIService.EndContents();
		}
		EditorGUILayout.Space();
		EditorGUILayout.Space();


		foreach (TagData tag in log.Tags) {

			EditorGUIService.BeginContents(false);
			EditorGUILayout.BeginHorizontal();
			tag.Enable = EditorGUILayout.Toggle(tag.Enable, GUILayout.Width(40.0f));
			tag.Color = EditorGUILayout.ColorField(tag.Color);
			tag.Tag = EditorGUILayout.TextField(tag.Tag);
			GUI.backgroundColor = Color.red;
			if (GUILayout.Button("X", GUILayout.Width(40.0f)))
			{
				log.Tags.Remove(tag);
				return;
			}
			GUI.backgroundColor = Color.white;
			EditorGUILayout.EndHorizontal();
			EditorGUIService.EndContents();
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		if (GUILayout.Button("Add Tag")) {
			log.Tags.Add(new TagData() { Color = Color.white });
		}

#endif

		if (!Application.isPlaying)
			return;
	}





	public static Formula ToFomula(Log.LogData log)
	{
		Formula F = new Formula();
		string mess = Service.String.EncodeString(log.Message, 123, Service.String.EncodeStringType.linear);
		F.AddFormula("Message", mess);
		F.AddFormula("LogType", log.LogType.ToString());
		F.AddFormula("Date", log.Date);
		F.AddFormula("Tag", (log.Tag == null) ? "" : log.Tag.Tag);
		//** [Local]
		if (log.LocalContent != null)
		{
			if (log.LocalContent.Formula != null)
			{
				string fencode = Service.String.EncodeString(log.LocalContent.Formula.PassToJson(new Formula()), 123, Service.String.EncodeStringType.linear);
				F.AddFormula("Formula", fencode);
			}
		}
		return F;
	}
	public static Log.LogData ToLog(Formula F)
	{
		Log.LogData data = new Log.LogData();
		string tag = F.GetFormula("Tag").Text;
		var tagData = LogTools.log.Tags.Find( x => x.Tag == tag);
		data.Tag = tagData;
		data.Date = (long)F.GetFormula("Date").Value;
		data.LogType = (Log.LogType)Service.String.ToEnum(F.GetFormula("LogType").Text, Log.LogType.write);
		data.Message = Service.String.EncodeString(F.GetFormula("Message").Text, -123, Service.String.EncodeStringType.linear);
		if (F.GetFormula("Formula").isHave) {
			string fencode = Service.String.EncodeString(F.GetFormula("Formula").Text, -123, Service.String.EncodeStringType.linear);
			data.LocalContent = new Log.LogData.Local() { Formula = FormulaToolsService.Json.JsonToJFormula(fencode) };
		}
		return data;
	}
	public static string SaveToString()
	{
		List<Formula> save = new List<Formula>();
		foreach (var l in Log.LogDatas)
		{
			save.Add(LogTools.ToFomula(l));
		}
		return FormulaToolsService.Json.FormulasToJsonArray(save);
	}



	public static void Save(bool isOpenFile = false)
	{
		//Path
		string path = "gamelog";
		if (!System.IO.Directory.Exists(path))
		{
			System.IO.Directory.CreateDirectory(path);
		}
		//FileName
		string dateName = System.DateTime.Now.ToString();
		dateName = dateName.Replace("\\", "-");
		dateName = dateName.Replace('\\', '-');
		dateName = dateName.Replace("/", "-");
		dateName = dateName.Replace(":", "-");

		//Message
		string message = LogTools.SaveToString();
		//Debug.Log(message);

		//Save File
		string fullpath = path + System.IO.Path.DirectorySeparatorChar + dateName + ".txt";
		System.IO.File.WriteAllText(fullpath, message);
		#if UNITY_EDITOR && UNITY_STANDALONE_WIN
		if (isOpenFile) System.Diagnostics.Process.Start("notepad.exe", fullpath);
		#endif

	}






	static bool isInitLogStatic = false;
	static bool m_isLogStatic = false;
	public static bool isLogStatic
	{
		get{
			if (!isInitLogStatic) 
			{
				isInitLogStatic = true;
				m_isLogStatic = PlayerPrefs.GetInt("IsLogStatic") == 1;
				if (!m_isLogStatic)
					ClearLogStatic();
			}
			return m_isLogStatic;
		}
		set 
		{
			m_isLogStatic = value;
			PlayerPrefs.SetInt("IsLogStatic", value ? 1 : 0); 
		}
	}
	public static void SaveLogStatic(string message) 
	{
		#if UNITY_EDITOR
		if (!isLogStatic)
			return;
		string last = PlayerPrefs.GetString("LogStatic");
		PlayerPrefs.SetString("LogStatic", last + "%" + message);
		#endif
	}
	public static string[] GetLogStatic( )
	{
#if UNITY_EDITOR
		if (!isLogStatic)
			return new string[0];
		string last = PlayerPrefs.GetString("LogStatic");
		if (string.IsNullOrEmpty(last))
			return new string[0];
		else
			return last.Split('%');
#endif
        return new string[0];
    }
    public static void ClearLogStatic( )
	{
		PlayerPrefs.DeleteKey("LogStatic");
	}



}













#if UNITY_EDITOR
namespace LogEditor
{
	
	public class LogCenterData {
		public string Name;
		public bool isMeta;
		public Formula Formula;
		public Log.LogData Data;
		public bool isAddMore;
		public string AddMoremssage;
		public bool isSave;
	}
	public class LogEditorPopup : EditorWindow
	{
		static LogCenterData logCenterData;
		public static void ShowWindow(LogCenterData data)
		{
			logCenterData = data;
			EditorWindow.GetWindow(typeof(LogEditorPopup),true,"Log Content");
		}
		Vector2 ScrollView;
		void OnGUI()
		{
			if (logCenterData == null)
				return;

			if (logCenterData.Formula != null) 
			{
				EditorStyles.textField.wordWrap = true;
				logCenterData.isMeta = EditorGUILayout.ToggleLeft("meta", logCenterData.isMeta );
				EditorGUILayout.TextArea( logCenterData.Formula.PassToJson(  (logCenterData.isMeta)?new Formula():null  )  , GUILayout.Height(100.0f));
				FormulaToolsService.OnGUI.GUIFormulaSerialize.FormulaDataDisplay(logCenterData.Formula, FormulaToolsService.OnGUI.display.full, logCenterData.Name);
			}
			if (logCenterData.Data != null) 
			{
				EditorGUILayout.BeginHorizontal();
				if (logCenterData.Data.Tag != null)
				{
					GUI.backgroundColor = logCenterData.Data.Tag.Color;
					EditorGUILayout.TextField("Tag:",logCenterData.Data.Tag.Tag );
					GUI.backgroundColor = Color.white;
				}
				else 
				{
					EditorGUILayout.TextField("Tag:", "None");
				}
				EditorGUILayout.EnumPopup(logCenterData.Data.LogType, GUILayout.Width(100.0f));
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.TextArea(logCenterData.Data.Message, GUILayout.Height(100.0f));

				ScrollView = EditorGUILayout.BeginScrollView(ScrollView);
				EditorGUIService.BeginContents(false);
				foreach (var log in logCenterData.Data.stackTrace.GetFrames())
				{
					var refMethod = log.GetMethod();
					string pathFile = log.GetFileName();
					if (refMethod.Name != "Write" && refMethod.Name != "HandleLog" && !string.IsNullOrEmpty(pathFile) && pathFile.IndexOf("Assets") != -1) {
						EditorGUILayout.BeginHorizontal();
						
						if (GUILayout.Button("view", GUILayout.Width(60.0f)))
						{
							string currentFile = log.GetFileName();
							int currentLine = log.GetFileLineNumber();
							string finalFileName = System.IO.Path.GetFullPath(currentFile);
							string path = $"/edit \"{finalFileName}\" /command \"edit.goto { currentLine.ToString() }\"";
							var ss = System.Diagnostics.Process.Start("devenv", path);
						}
						EditorGUILayout.SelectableLabel($"[{refMethod.Name}] { log.GetFileName() } ({ log.GetFileLineNumber()})({log.GetFileColumnNumber()})");
						EditorGUILayout.EndHorizontal();
					}


				}
				EditorGUIService.EndContents();
				EditorGUILayout.EndScrollView();
			}


			if (logCenterData.isAddMore) {
				logCenterData.AddMoremssage = EditorGUILayout.TextArea( logCenterData.AddMoremssage , GUILayout.Height(100.0f));
				if (GUILayout.Button("Push", GUILayout.Width(60.0f))) {
					List<Formula> load = FormulaToolsService.Json.JsonArrayToJFormulas(logCenterData.AddMoremssage);
					List<Log.LogData> testLog = new List<Log.LogData>();
					foreach (var f in load)
					{
						var log = LogTools.ToLog(f);
						testLog.Add(log);
						Log.LogDatas.Add(log);
					}
				}
			}

		}
	}














	public class LogEditor : EditorWindow
	{

		static List<Log.LogData> logHistory = new List<Log.LogData>();
		public static void Init() {
			EditorApplication.playModeStateChanged += RunGcWhenExitPlayMode;
			Application.logMessageReceived += HandleLog;
			//Application.RegisterLogCallback(HandleLog);
		}
		static void RunGcWhenExitPlayMode(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.EnteredEditMode) {
				//Log.LogDatas = Log.LogDatas;
				//System.GC.Collect();
			}
				
		}
		static void HandleLog(string logString, string stackTrace, LogType type)
		{
			var log = new Log.LogData() { LogType = Log.LogType.unitydebug };
			log.stackTrace = new System.Diagnostics.StackTrace(true);
			log.Message = logString;
			
			log.Tag = new TagData();
			if (type == LogType.Error)
			{
				log.Tag.Color = Color.red;
				log.Tag.Tag = type.ToString();
			}
			else if (type == LogType.Warning)
			{
				log.Tag.Color = Color.yellow;
				log.Tag.Tag = type.ToString();
			}
			else {
				log.Tag.Color = Color.white;
				log.Tag.Tag = type.ToString();
			}
			Log.Write(log);
		}



		[MenuItem(EditorGUIService.ProjectPath.header + "/Editor/Log/LogEditor")]
		public static void ShowWindow()
		{
			EditorWindow.GetWindow(typeof(LogEditor),false, "LogEditor");
			
		}
		void OnGUI()
		{
			LogEditor.Gui();
		}


		void Update() { if(isUpdate==2) Repaint(); }




		static bool isBool( string key , string name ) {
			bool bol = EditorPrefs.GetBool(key);
			bol = EditorGUILayout.ToggleLeft(name, bol,GUILayout.Width(name.Length * 9.0f));
			EditorPrefs.SetBool(key , bol);
			return bol;
		}
		static long DateFrom;
		static long DateTo;
		static bool isDatetime;
		static int isUpdate = 0;
		static bool isOption = false;
		static Vector2 ScrollView;
		public static void Gui()
		{

            #region ToolLayer1
            EditorGUILayout.BeginHorizontal();
			//#Tag
			GUILayout.Label(EditorGUIUtility.FindTexture("d_VisibilityOn"), GUILayout.Width(18.0f));
			string tag = EditorPrefs.GetString("editorlog_tag");
			EditorGUILayout.LabelField("Tag:", GUILayout.Width(40.0f));
			tag = EditorGUILayout.TextField(tag);
			EditorPrefs.SetString("editorlog_tag", tag);

			//#Keyword
			GUILayout.Label(EditorGUIUtility.FindTexture("d_ViewToolZoom"), GUILayout.Width(18.0f));
			string keyword = EditorPrefs.GetString("editorlog_keyword");
			EditorGUILayout.LabelField("Keyword:", GUILayout.Width(60.0f));
			keyword = EditorGUILayout.TextField( keyword );
			EditorPrefs.SetString("editorlog_keyword", keyword);

			GUI.backgroundColor = Color.red;
			if (GUILayout.Button("clear log", GUILayout.Width(80.0f)))
			{
				/*
				var keep = Log.LogDatas.FindAll(x => x.isKeep);
				logHistory.AddRange(Log.LogDatas);
				Log.LogDatas.Clear();
				Log.LogDatas.AddRange(keep);
				*/
				foreach (Log.LogData log in new ArrayList(Log.LogDatas)) {

					if (log.isKeep)
					{

					}
					else 
					{
						Log.LogDatas.Remove(log);
						logHistory.Add(log);
					}
				}
				return;
			}
			GUI.backgroundColor = Color.white;
			if (GUILayout.Button("load", GUILayout.Width(40.0f)))
			{
				if (logHistory.Count > 0)
				{
					//Log.LogDatas.Clear();
					Log.LogDatas.AddRange(logHistory);
					logHistory.Clear();
				}
				return;
			}
			if (GUILayout.Button(EditorGUIUtility.FindTexture("d_P4_AddedRemote"), GUILayout.Width(25.0f)))
			{
				LogCenterData logCenterData = new LogCenterData()
				{
					isAddMore = true
				};
				LogEditorPopup.ShowWindow(logCenterData);
				return;
			}
			EditorGUILayout.EndHorizontal();
			#endregion

			#region ToolLayer2
			EditorGUILayout.BeginHorizontal();

			//isUpdate
			isUpdate = EditorPrefs.GetInt("editorlog_isUpdate");
			string ico = "";
			if (isUpdate == 0) ico = "Animation.Record";
			if (isUpdate == 1) ico = "Animation.Play"; 
			if (isUpdate == 2) ico = "Animation.LastKey"; 
			if (GUILayout.Button(EditorGUIUtility.FindTexture(ico), GUILayout.Width(25.0f)))
			{
				isUpdate++;
				if (isUpdate > 2) isUpdate = 0;
			}
			EditorPrefs.SetInt("editorlog_isUpdate", isUpdate);

			//Option
			isOption = EditorPrefs.GetBool("editorlog_option");
			GUI.backgroundColor = (!isOption)?Color.gray: Color.yellow;
			if (GUILayout.Button("Option", GUILayout.Width(60.0f)))
			{
				isOption = !isOption;
				EditorPrefs.SetBool("editorlog_option", isOption);
			}
			if (isOption) 
			{
				//Time
				isDatetime = EditorPrefs.GetBool("editorlog_istime");
				GUI.backgroundColor = (isDatetime) ? Color.yellow : Color.gray;
				if (GUILayout.Button(EditorGUIUtility.FindTexture("d_UnityEditor.AnimationWindow"), GUILayout.Width(25.0f)))
				{
					isDatetime = !isDatetime;
					EditorPrefs.SetBool("editorlog_istime", isDatetime);
				}
				//Save
				GUI.backgroundColor = Color.white;
				if (GUILayout.Button(EditorGUIUtility.FindTexture("SaveActive"), GUILayout.Width(25.0f)))
				{
					LogTools.Save(true);
				}
			}
			

			//Setting
			GUI.backgroundColor = Color.white;
			if (GUILayout.Button("Setting", GUILayout.Width(65.0f)))
			{
				LogToolsUI.OnSelection();
			}

	
			bool islogwrite = isBool("editorlog_islogwrite", "Log Write");
			bool islogdebug = isBool("editorlog_islogdebug", "Log UnityDebug");
			bool isruntime = isBool("editorlog_isruntime", "Log Runtime");
			bool isbutton = isBool("editorlog_isbutton", "Log Button");


			if (GUILayout.Button("LogStatic", GUILayout.Width(75.0f)))
			{
				LogTools.isLogStatic = !LogTools.isLogStatic;
			}
			if (LogTools.isLogStatic) 
			{
				if (GUILayout.Button("Load", GUILayout.Width(65.0f)))
				{
					foreach (var message in LogTools.GetLogStatic())
						Log.Write(message);
				}
				if (GUILayout.Button("Clear", GUILayout.Width(65.0f)))
				{
					LogTools.ClearLogStatic();
				}
			}




			EditorGUILayout.EndHorizontal();
			#endregion


			if (isUpdate == 0)
				return;



			//time[20:20:20 - 20:20:21]
			DateFrom = 0;
			DateTo = 0;
			if (!string.IsNullOrEmpty(keyword)) {
				if (Service.String.isStrCropValue(keyword, "time[", "]")) {
					string t = Service.String.strCropValue(keyword, "time[", "]");
					if (!string.IsNullOrEmpty(t)) {
						try
						{
							string[] times = t.Split('-');
							if (times.Length > 0)
							{
								System.DateTime d = System.DateTime.Now;
								string iString = $"{d.Year}-{d.Month}-{d.Day} {times[0]}";
								System.DateTime oDate = System.Convert.ToDateTime(iString);
								DateFrom = Service.Time.DateTimeToUnixTimeStamp(oDate);
							}
							if (times.Length > 1)
							{
								System.DateTime d = System.DateTime.Now;
								string iString = $"{d.Year}-{d.Month}-{d.Day} {times[1]}";
								System.DateTime oDate = System.Convert.ToDateTime(iString);
								DateTo = Service.Time.DateTimeToUnixTimeStamp(oDate);
							}
						}
						catch (System.Exception e) { 
						
						}
					}
				}
			}



			List<Log.LogData> logDataFilter = new List<Log.LogData>();
			foreach (var log in Log.LogDatas)
			{
				bool isHaveTag = false;
				if (islogwrite && log.LogType == Log.LogType.write) isHaveTag = true;
				if (islogdebug && log.LogType == Log.LogType.unitydebug) isHaveTag = true;
				if (isruntime && log.LogType == Log.LogType.runtime) isHaveTag = true;
				if (isbutton && log.LogType == Log.LogType.button) isHaveTag = true;
				if (isHaveTag) 
				{
					if (!string.IsNullOrEmpty(tag)) { 
						if(tag.IndexOf(",")!=-1)
							isHaveTag = log.Tag != null && tag.IndexOf(log.Tag.Tag) != -1;
						else
							isHaveTag = log.Tag != null && log.Tag.Tag.IndexOf(tag) != -1;
					}
						
				}
				if (isHaveTag)
				{
					if (!string.IsNullOrEmpty(keyword))
					{


						if (DateFrom != 0 && DateTo != 0)
						{
							if (log.Date >= DateFrom && log.Date <= DateTo)
							{
								isHaveTag = true;
							}
							else isHaveTag = false;
						}
						else 
						{
							if (!string.IsNullOrEmpty(log.Message))
								isHaveTag = log.Message.IndexOf(keyword) != -1;
							else
								isHaveTag = false;
						}
					}
				}
				if (isHaveTag)
					logDataFilter.Add(log);
			}




			if (logDataFilter.Count > 0) 
			{
				ScrollView = EditorGUILayout.BeginScrollView(ScrollView);
				EditorGUIService.BeginContents(false);
				foreach (var log in logDataFilter)
				{
					Line(log);
				}
				EditorGUIService.EndContents();
				EditorGUILayout.EndScrollView();
			}

			
		}





		public static void Line( Log.LogData log) 
		{
			EditorGUILayout.BeginHorizontal();
			LineContent(log);
			EditorGUILayout.EndHorizontal();
		}
		static void LineContent(Log.LogData log) {


			//** [Option]
			if (isOption) 
			{
				GUI.backgroundColor = Color.red;
				if (GUILayout.Button("x", GUILayout.Width(20.0f)))
				{
					Log.LogDatas.Remove(log);
					return;
				}
				GUI.backgroundColor = (log.isKeep)?Color.cyan: Color.black;
				if (GUILayout.Button("★", GUILayout.Width(20.0f)))
				{
					log.isKeep = !log.isKeep;
				}
				GUI.backgroundColor = Color.white;
				if (GUILayout.Button("☰", GUILayout.Width(20.0f)))
				{
					LogCenterData logCenterData = new LogCenterData()
					{
						Data = log
					};
					LogEditorPopup.ShowWindow(logCenterData);
				}
				GUI.backgroundColor = Color.white;
			}


			if(isDatetime)
				EditorGUILayout.TextField(  Service.Time.UnixTimeStampToDateTime(log.Date).ToString("HH:mm:ss") , GUILayout.Width(120.0f) );
			



			//** [Tag]
			GUI.backgroundColor = (log.Tag == null) ? Color.white : log.Tag.Color;
			EditorGUILayout.TextField((log.Tag == null) ? "" : log.Tag.Tag , GUILayout.Width(40.0f));
			GUI.backgroundColor = Color.white;

		

			//** [Local]
			if (log.LocalContent != null) 
			{

				if (log.LocalContent.GameObject != null) 
				{
					EditorGUILayout.ObjectField(log.LocalContent.GameObject,typeof(GameObject));
					//EditorGUIService.GizmosUtils.DrawText( GUIstylePackage.Instant.Header , log.Message, log.LocalContent.GameObject.transform.position);
				}
				if (log.LocalContent.Transform != null)
				{
					EditorGUILayout.ObjectField(log.LocalContent.Transform, typeof(Transform));
					//EditorGUIService.GizmosUtils.DrawText(null, log.Message, log.LocalContent.Transform.position);
				}
				if (log.LocalContent.MonoBehaviour != null)
				{
					//EditorGUILayout.ObjectField(log.LocalContent.MonoBehaviour, typeof(MonoBehaviour));
				}
				if (log.LocalContent.Formula != null)
				{
					//string data = log.LocalContent.Formula.PassToJson();
					//EditorGUILayout.TextArea(data);
					if (GUILayout.Button("formula", GUILayout.Width(80.0f)))
					{
						LogCenterData logCenterData = new LogCenterData() { 
							Name = "formula",
							Formula = log.LocalContent.Formula
						};
						LogEditorPopup.ShowWindow(logCenterData);
					}
				}
				if (log.LocalContent.Object != null)
				{
					//string data = ServiceJson.Json.SerializeObject(log.LocalContent.Object);
					//EditorGUILayout.SelectableLabel(log.LocalContent.Object.ToString());
					//EditorGUILayout.TextArea(data);
					if (GUILayout.Button("class", GUILayout.Width(80.0f)))
					{
						LogCenterData logCenterData = new LogCenterData()
						{
							Name = log.LocalContent.Object.ToString(),
							Formula = FormulaToolsService.Json.JsonToJFormula(log.LocalContent.Object)
						};
						LogEditorPopup.ShowWindow(logCenterData);
					}

				}
			}
			//** [RunTime]
			if(log.LogType == Log.LogType.runtime)
			if (log.LocalRunTimeContent != null)
			{
				if (log.LocalRunTimeContent.RuntimeObject != null)
				{
					if (log.LocalRunTimeContent.Data != null) 
					{
						log.LocalRunTimeContent.RuntimeObject(log.LocalRunTimeContent.Data);
						EditorGUILayout.SelectableLabel(log.LocalRunTimeContent.Data.Message);
					}
				}
				else EditorGUILayout.LabelField("RunTime null");
			}
			//** [Btn]
			if (log.LocalButtonContent != null)
			{
				if (log.LocalButtonContent.LogCallback != null)
				{
					string BtnName = (string.IsNullOrEmpty(log.LocalButtonContent.BtnName))? "click" : log.LocalButtonContent.BtnName;
					if (GUILayout.Button(BtnName, GUILayout.Width(BtnName.Length*9.0f)))
					{
						log.LocalButtonContent.LogCallback();
					}
				}
			}



			//** [Message]
			EditorGUILayout.SelectableLabel(log.Message);
			/*
			if (!string.IsNullOrEmpty(log.Message))
			{
				if (log.LocalContent != null)
					EditorGUILayout.SelectableLabel(log.Message, GUILayout.Width(100.0f));
				else
					EditorGUILayout.SelectableLabel(log.Message);
			}
			else
				EditorGUILayout.SelectableLabel("", GUILayout.Width(1.0f));
			*/





		}










	}

}
#endif







