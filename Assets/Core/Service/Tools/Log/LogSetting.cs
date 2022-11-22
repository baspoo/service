using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LogService
{
	[CreateAssetMenu(fileName = "LogSetting", menuName = "Service/Editor/Log/LogSetting", order = 1)]
	public class LogSetting : ScriptableObject
	{

		public static LogSetting instance
		{
			get {
				if (m_instance == null)
				{
					m_instance = (LogSetting)Resources.Load("LogSetting");
				}
				return m_instance;
			}
		}
		static LogSetting m_instance;
		public static void Init()
		{

		}



		public static bool IsCanLogger 
		{
			get
			{
				#if UNITY_EDITOR || DEVELOPMENT_BUILD
				return instance.IsEnable;
				#endif
				return false;
			}
		}



		public bool IsEnable;

		//[Header("Setting")]
		//public bool IgnoreNotTag;
		//public bool IgnoreAll;
		//public bool DisableUnityLog;
		//public bool IsReportFile;

		public bool IsNonTag;
		public bool IsUnkownTag;
		public bool IsHasTag;

		[Header("LocalSave")]
		public bool IsLocalSaveEnable;
		public bool IsLocalSaveFullLog;
		public bool IsLocalTagFilter;
		public int DaysExprid;
		public int CountOfSaveToLocal;
		public int MaxLocalToFile;

		public List<Utility.LogTag> Tags = new List<Utility.LogTag>();

	}

}






namespace LogService.LogEditor
{
#if UNITY_EDITOR
	[CustomEditor(typeof(LogSetting))]
	public class UILogSetting : Editor
	{
		public static void OnSelection()
		{
			Selection.activeObject = LogSetting.instance;
		}
		public LogSetting m_tools { get { return (LogSetting)target; } }
		public override void OnInspectorGUI()
		{
			EditorGUILayout.Space();
			if (m_tools == null) return;


			if (BtnEnable()) 
			{
				IsgnoreSetting();
				TagList();
				localSave();
				BtnSave();
			}

		}



        bool BtnEnable()
        {
			GUI.backgroundColor = m_tools.IsEnable ? Color.green : Color.white;
			if (GUILayout.Button("\nOnEnable\n"))
			{
				m_tools.IsEnable = !m_tools.IsEnable;
				DoSave();
			}
			GUI.backgroundColor = Color.white;
			return m_tools.IsEnable;
		}
        void IsgnoreSetting() 
		{
			if (EditorGUIService.DrawHeader("Setting", "DebugSetting.AdvanceSetting", false, false))
			{
				EditorGUIService.BeginContents(false);
				//m_tools.IgnoreAll = EditorGUILayout.ToggleLeft("IgnoreAll", m_tools.IgnoreAll);
				//m_tools.IgnoreNotTag = EditorGUILayout.ToggleLeft("IgnoreNotTag", m_tools.IgnoreNotTag);
				//m_tools.DisableUnityLog = EditorGUILayout.ToggleLeft("DisableUnityLog", m_tools.DisableUnityLog);
				//m_tools.IsReportFile = EditorGUILayout.ToggleLeft("IsReportFile", m_tools.IsReportFile);

				m_tools.IsNonTag = EditorGUILayout.ToggleLeft("None-Tag", m_tools.IsNonTag);
				m_tools.IsUnkownTag = EditorGUILayout.ToggleLeft("Unkown-Tag", m_tools.IsUnkownTag);
				m_tools.IsHasTag = EditorGUILayout.ToggleLeft("Has-Tag", m_tools.IsHasTag);
				EditorGUIService.EndContents();
			}
		}
		void localSave()
		{
			if (EditorGUIService.DrawHeader("LocalSave", "DebugSetting.LocalSave", false, false))
			{
				EditorGUIService.BeginContents(false);

				GUI.backgroundColor = m_tools.IsLocalSaveEnable ? Color.green : Color.white;
				if (GUILayout.Button("Enable LocalSave"))
				{
					m_tools.IsLocalSaveEnable = !m_tools.IsLocalSaveEnable;
				}
				GUI.backgroundColor = Color.white;
				EditorGUIService.BeginEndnable(m_tools.IsLocalSaveEnable,()=> {

					m_tools.IsLocalSaveFullLog = EditorGUILayout.Toggle("FullLog", m_tools.IsLocalSaveFullLog);
					m_tools.IsLocalTagFilter = EditorGUILayout.Toggle("Using Tag Filter", m_tools.IsLocalTagFilter);
					m_tools.CountOfSaveToLocal = EditorGUILayout.IntField("End of Line", m_tools.CountOfSaveToLocal);
					m_tools.MaxLocalToFile = EditorGUILayout.IntField("MaxSave ToFile", m_tools.MaxLocalToFile);
					m_tools.DaysExprid = EditorGUILayout.IntField("Exprid (Unix)", m_tools.DaysExprid);
					EditorGUILayout.TextField( "Path" , Utility.LocalSaveHandle.DirPath );
					

				});
				EditorGUIService.EndContents();
			}
		}
		void TagList()
		{
			#if UNITY_EDITOR
			if (EditorGUIService.DrawHeader("Tag Filter", "DebugSetting.TagFilter", false, false))
			{
				EditorGUIService.BeginContents(false);


				//EditorGUILayout.Space();
				//GUI.backgroundColor = Color.gray;
				//EditorGUIService.BeginContents(false);
				//m_tools.IsNonTag = EditorGUILayout.ToggleLeft("None-Tag", m_tools.IsNonTag);
				//m_tools.IsUnkownTag = EditorGUILayout.ToggleLeft("Unkown-Tag", m_tools.IsUnkownTag);
				//m_tools.IsHasTag = EditorGUILayout.ToggleLeft("Has-Tag", m_tools.IsHasTag);
				//EditorGUIService.EndContents();
				//GUI.backgroundColor = Color.white;
				//EditorGUILayout.Space();
				//EditorGUILayout.Space();
				//EditorGUILayout.Space();



				bool displayTag(Utility.LogTag tag , bool isCanRemove ) 
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup(!isCanRemove);
					//if (m_tools.tagFilter == DebugSetting.TagFilter.OnlyTag)
					tag.Enable = EditorGUILayout.Toggle(tag.Enable, GUILayout.Width(40.0f));
					tag.Color = EditorGUILayout.ColorField(tag.Color);
					tag.Tag = EditorGUILayout.TextField(tag.Tag);
					

					
					GUI.backgroundColor = isCanRemove?Color.red : Color.black;
					if (GUILayout.Button("X", GUILayout.Width(40.0f)))
					{
						m_tools.Tags.Remove(tag);
						return false;
					}
					EditorGUI.EndDisabledGroup();

					GUI.backgroundColor = Color.white;
					EditorGUILayout.EndHorizontal();
					return true;
				}


				if (m_tools.IsNonTag)
				{
					displayTag(new Utility.LogTag()
					{
						Tag = "non-Tag",
						Color = Color.white,
						Enable = true
					},false);
				}
				if (m_tools.IsUnkownTag)
				{
					displayTag(new Utility.LogTag()
					{
						Tag = "unkown-Tag",
						Color = Color.white,
						Enable = true
					}, false);
				}
				if (m_tools.IsHasTag)
				{
					foreach (var tag in m_tools.Tags)
					{
						if (!displayTag(tag,true))
							return;
					}
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					if (GUILayout.Button("Add Tag"))
					{
						m_tools.Tags.Add(new Utility.LogTag() { Color = Color.white });
					}
				}
	

				EditorGUIService.EndContents();
			}
			#endif
		}


		void BtnSave() 
		{
			//EditorGUILayout.Space();
			//EditorGUILayout.Space();
			//EditorGUILayout.Space();
			//EditorGUILayout.Space();
			if (GUILayout.Button("\nSave\n"))
			{
				DoSave();
			}
		}
		void DoSave() {
			Undo.RecordObject(LogSetting.instance, "DebugSetting");
			PrefabUtility.RecordPrefabInstancePropertyModifications(LogSetting.instance);
			EditorUtility.CopySerialized(LogSetting.instance, LogSetting.instance);
			AssetDatabase.SaveAssets();
		}
	}
#endif





}
