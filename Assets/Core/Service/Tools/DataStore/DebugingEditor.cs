#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
public class DebugingEditor : EditorWindow
{
	[MenuItem(Service.head_service+"/Data/Update Tsv")]
	public static void ShowWindow(){
		EditorWindow.GetWindow(typeof(DebugingEditor));
	}
	void Update() 
	{
		Repaint();
	}

		 
	
	void OnGUI(){
		DebugingEditorGui.OnGUI ();
	}
}
#endif




#if UNITY_EDITOR
public class DebugingEditorGui : MonoBehaviour
{
	static Vector2 ScrollView;
	public static void OnGUI()
	{
		ScrollView = EditorGUILayout.BeginScrollView(ScrollView);
		GUILayout.Label("TSV ---------", EditorStyles.boldLabel);
		string btnTextTsvLoad = "Reload All - Tsv";
		if (TSVLoaderTools.isHave)
		{
			btnTextTsvLoad = "Loading....";
			if (GUILayout.Button("Stop"))
			{
				TSVLoaderTools.loader.Stop();
			}
		}
		if (GUILayout.Button(btnTextTsvLoad))
		{
			TSVLoaderTools.loader.Download();
		}
		foreach (FileData f in (Resources.Load("Database/_loader") as GameObject).GetComponent<TSVLoaderTools>().FileDatas)
		{
			if (GUILayout.Button(f.Name))
			{
				TSVLoaderTools.loader.Download(f);
			}
		}
		EditorGUILayout.EndScrollView();
	}
}
#endif



