#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class TsvDataEditor : EditorWindow
{
	//[MenuItem( "Utility/Data/Update Tsv")]
	public static void ShowWindow()
	{
		GetWindow(typeof(TsvDataEditor));
	}
	private void Update() 
	{
		Repaint();
	}
	
	private void OnGUI()
	{
		TsvDataEditorGui.OnGUI();
	}
}
#endif




#if UNITY_EDITOR
public class TsvDataEditorGui : MonoBehaviour
{
	static Vector2 ScrollView;
	public static void OnGUI()
	{
		ScrollView = EditorGUILayout.BeginScrollView(ScrollView);

		EditorGUILayout.BeginHorizontal();
		TSVLoaderTools.loaderData.isRealTime = EditorGUILayout.Toggle(TSVLoaderTools.loaderData.isRealTime, GUILayout.Width(15.0f));
		GUILayout.Label(EditorGUIUtility.FindTexture("d_CloudConnect@2x"), GUILayout.Width(25.0f));
		TSVLoaderTools.loaderData.loadAssetType = (TSVLoaderTools.LoadAssetType)EditorGUILayout.EnumPopup(TSVLoaderTools.loaderData.loadAssetType);
		if (GUILayout.Button("Go to TsvLoader"))
		{
			Selection.activeObject = TSVLoaderTools.loaderData.gameObject;
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Label("TSV ---------", EditorStyles.boldLabel);
		if (GUILayout.Button("ReloadAll"))
			TSVLoaderTools.loaderData.Downloads();
		EditorGUILayout.Space(20.0f);
		foreach (var fileData in TSVLoaderTools.loaderData.FileDatas)
			if (GUILayout.Button(fileData.Name))
				TSVLoaderTools.loaderData.Download(fileData);
		EditorGUILayout.EndScrollView();
	}
}
#endif



