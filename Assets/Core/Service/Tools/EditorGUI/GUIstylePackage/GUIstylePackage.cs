using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class GUIstylePackage : MonoBehaviour {
	static GUIstylePackage m = null;
	public static GUIstylePackage Instant{
		get{ 	
			if (m == null) 	
				m = (Resources.Load ("GUIstylePackage") as GameObject).GetComponent<GUIstylePackage>(); 
			return m;
		}
	}
	public GUIStyle Header;
	public GUIStyle HeaderBig;
	public float size;
	public List<string> iconsName;

}




#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(GUIstylePackage))]
[System.Serializable]
public class GUIstylePackageUI : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		var gp = (GUIstylePackage)target;

		EditorGUIService.LableBlod("Lable-Blod", Color.yellow);
		EditorGUIService.LableHeader("Lable-Header", Color.yellow, TextAnchor.MiddleCenter);
		//EditorGUIService.LableHeaderHigtlight("Lable-HeaderHigtlight");
		EditorGUIService.LableHeaderBig("Lable-HeaderBig");
		//EditorGUIService.LableHeaderBigCenter("Lable-HeaderBigCenter", Color.red , TextAnchor.MiddleCenter);
		//EditorGUIService.LableStyle("LableStyle", 30 , Color.green , FontStyle.Italic , TextAnchor.MiddleCenter);

		if (EditorGUIService.Button("PlayButton",Color.green)) 
		{

			Debug.Log("xxxx");
		}


		for (int i = 0; i < gp.iconsName.Count; i++) 
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(EditorGUIService.BtnIcon(gp.iconsName[i]), GUILayout.Width(gp.size), GUILayout.Height(gp.size));
			gp.iconsName[i] = EditorGUILayout.TextField(gp.iconsName[i]);
			EditorGUILayout.EndHorizontal();
		}
	} 

}
#endif