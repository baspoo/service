using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SoundData
{
	public string name;
	public bool isEnable;
	public bool isPlay;
	public AudioClip clip;
	public string message;
	public string tag;

	public static SoundData Find(SoundData[] datas , string find  , int indexDefault = -1){
		if (!string.IsNullOrEmpty (find)) {
			foreach (SoundData s in datas) {
				if (s.tag == find)
					return s;
			}
		}
		if (indexDefault != -1)
			return datas [indexDefault];
		else 
			return null;
	}
}


[System.Serializable]
public class SoundGroupOther
{
	[Header("-------------------------------------------------")]
	public string groupname;
	public List<SoundOther> Refs = new List<SoundOther>();
}
[System.Serializable]
public class SoundOther
{
	public string name;
	public Object refobject;
}



#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SoundData))]
public class IngredientDrawer : PropertyDrawer
{
	int nextwidth=0;
	void Reposition(){ nextwidth = 0; }
	Rect Position( Rect position , int width){
		Rect R = new Rect(position.x	+ nextwidth	, position.y, width	, position.height);
		nextwidth += width + 5;
		return R;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;



		ToolsSounds.SoundPlaylist Playlist =  ((GameObject)Selection.activeObject).GetComponent<ToolsSounds.SoundPlaylist>(); 
		if (Playlist != null) {
			foreach( SoundGroupOther groups in Playlist.SoundOther  ){
				foreach( SoundOther so in groups.Refs  ){
					if (so.refobject != null)
						so.name = so.refobject.name;
				}
			}
		}


		Reposition ();
		EditorGUI.PropertyField(Position(position,10), property.FindPropertyRelative("isPlay"), GUIContent.none);
		EditorGUI.PropertyField(Position(position,200), property.FindPropertyRelative("clip"), GUIContent.none);
		EditorGUI.PropertyField(Position(position,20), property.FindPropertyRelative("isEnable"), GUIContent.none);
		EditorGUI.LabelField (Position(position,300),property.FindPropertyRelative("name").stringValue);


		if (property.FindPropertyRelative ("isEnable").boolValue) {
			property.FindPropertyRelative ("isEnable").boolValue = false;
			SoundEdit.ShowWindow (property);
		}


		if (property.FindPropertyRelative ("isPlay").boolValue) {
			property.FindPropertyRelative ("isPlay").boolValue = false;
			Playlist.Play ( (AudioClip) property.FindPropertyRelative ("clip").objectReferenceValue );
		}


		// Set indent back to what it was
		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty();
	}
}
#endif




















//------------------------------------------------------------------------------------
// Wizard
#if UNITY_EDITOR
public class SoundEdit : EditorWindow
{
	public static void ShowWindow( SerializedProperty property ){
		m_property = property;
		EditorWindow.GetWindow(typeof(SoundEdit));
	}

	static SerializedProperty m_property;
	void OnGUI(){

		if (m_property == null)
			return;
		if ( !Service.GameObj.isObjectNotNull(m_property) )
			return;
		
		if ((GameObject)Selection.activeObject == null)
			return;
		else
			if (((GameObject)Selection.activeObject).GetComponent<ToolsSounds.SoundPlaylist>() == null)
				return;
		
		GUIStyle gs = new GUIStyle (GUIstylePackage.Instant.Header);;
		gs.alignment = TextAnchor.MiddleCenter;
		gs.normal.textColor = Color.white;
		EditorGUILayout.LabelField ("--------------------------------------------------------",gs);
		EditorGUILayout.LabelField ("||                     Note!                       ||", gs);
		EditorGUILayout.LabelField ("--------------------------------------------------------",gs);
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		try{
		EditorGUILayout.LabelField ("Name");
		SerializedProperty nn = m_property.FindPropertyRelative ("name");
		nn.stringValue =  EditorGUILayout.TextField ( nn.stringValue);
		nn.serializedObject.ApplyModifiedProperties ();

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		EditorGUILayout.LabelField ("Message");
		SerializedProperty mm = m_property.FindPropertyRelative ("message");
		mm.stringValue =  EditorGUILayout.TextArea ( mm.stringValue);
		mm.serializedObject.ApplyModifiedProperties ();


		EditorGUILayout.LabelField ("Tag");
		SerializedProperty t = m_property.FindPropertyRelative ("tag");
		t.stringValue =  EditorGUILayout.TextField ( t.stringValue);
		t.serializedObject.ApplyModifiedProperties ();

		} catch{
		
		}
	}



}
#endif




