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
	public string path;
	public string message;
	public string tag;
	public AudioClip fill;
	//public Avatar this[int index] { get => Avatars[index]; }
	//public AudioClip this.clip;

	public static SoundData Find(SoundData[] datas, string find, int indexDefault = -1)
	{
		if (!string.IsNullOrEmpty(find))
		{
			foreach (SoundData s in datas)
			{
				if (s.tag == find)
					return s;
			}
		}
		if (indexDefault != -1)
			return datas[indexDefault];
		else
			return null;
	}

	AudioClip m_clip;
	public AudioClip getclip
	{
		get
		{

			if (!Application.isPlaying)
				return reload(path);

			if (m_clip == null)
			{
				if (!string.IsNullOrEmpty(path))
					m_clip = reload(path);
			}
			return m_clip;
		}
	}

	public static AudioClip reload(string path)
	{
		return Resources.Load(path) as AudioClip;
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
	int nextwidth = 0;
	void Reposition() { nextwidth = 0; }
	Rect Position(Rect position, int width)
	{
		Rect R = new Rect(position.x + nextwidth, position.y, width, position.height);
		nextwidth += width + 5;
		return R;
	}
	public string pathKeep;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;



		ToolsSounds.SoundPlaylist Playlist = ((GameObject)Selection.activeObject).GetComponent<ToolsSounds.SoundPlaylist>();






		Reposition();



		if ((property.FindPropertyRelative("isEnable").boolValue))
		{
			if (GUI.Button(Position(position, 22), EditorGUIUtility.FindTexture("AudioSource Gizmo")))
			{
				Playlist.Play(SoundData.reload(property.FindPropertyRelative("path").stringValue));
				EditorGUIUtility.PingObject(SoundData.reload(property.FindPropertyRelative("path").stringValue));
			}
		}
		//EditorGUI.PropertyField(Position(position, 20), property.FindPropertyRelative("isPlay"), GUIContent.none);
		else
			EditorGUI.HelpBox(Position(position, 20), "", MessageType.Warning);

		//EditorGUI.PropertyField(Position(position,10), property.FindPropertyRelative("isPlay"), GUIContent.none);
		//EditorGUI.HelpBox(Position(position, 20), "", (property.FindPropertyRelative("isEnable").boolValue)?MessageType.None: MessageType.Warning);

		EditorGUI.PropertyField(Position(position, 20), property.FindPropertyRelative("fill"), GUIContent.none);
		EditorGUI.PropertyField(Position(position, 300), property.FindPropertyRelative("path"), GUIContent.none);
		EditorGUI.PropertyField(Position(position, 100), property.FindPropertyRelative("tag"), GUIContent.none);

		if (property.FindPropertyRelative("path").stringValue != pathKeep)
		{
			var clip = SoundData.reload(property.FindPropertyRelative("path").stringValue);
			property.FindPropertyRelative("isEnable").boolValue = clip != null;
			pathKeep = property.FindPropertyRelative("path").stringValue;
		}


		if (property.FindPropertyRelative("fill").objectReferenceValue != null)
		{
			var path = AssetDatabase.GetAssetPath(property.FindPropertyRelative("fill").objectReferenceValue);
			//path = path.Replace("Assets/TheLastBug/MediaStore/Sound/Resources/sfxplaylist/", "");
			//path = path.Split('.')[0];
			//property.FindPropertyRelative("path").stringValue = path;
			//property.FindPropertyRelative("fill").objectReferenceValue = null;

			//var path = AssetDatabase.GetAssetPath(property.FindPropertyRelative("fill").objectReferenceValue);

			string[] stringSeparators = new string[] { "Resources/" };
			var split = path.Split(stringSeparators, System.StringSplitOptions.None);
			if (split.Length > 1)
				path = split[1];
			else
				path = string.Empty;
			path = path.Split('.')[0];
			property.FindPropertyRelative("path").stringValue = path;
			property.FindPropertyRelative("fill").objectReferenceValue = null;


		}




		//	if (property.FindPropertyRelative ("isPlay").boolValue) 
		//{
		//	property.FindPropertyRelative ("isPlay").boolValue = false;
		//	Playlist.Play(SoundData.reload(property.FindPropertyRelative("path").stringValue));
		//	EditorGUIUtility.PingObject(SoundData.reload(property.FindPropertyRelative("path").stringValue));
		//}


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
	public static void ShowWindow(SerializedProperty property)
	{
		m_property = property;
		EditorWindow.GetWindow(typeof(SoundEdit));
	}

	static SerializedProperty m_property;
	void OnGUI()
	{

		if (m_property == null)
			return;
		if (!Service.GameObj.isObjectNotNull(m_property))
			return;

		if ((GameObject)Selection.activeObject == null)
			return;
		else
			if (((GameObject)Selection.activeObject).GetComponent<ToolsSounds.SoundPlaylist>() == null)
			return;

		GUIStyle gs = new GUIStyle(GUIstylePackage.Instant.Header); ;
		gs.alignment = TextAnchor.MiddleCenter;
		gs.normal.textColor = Color.white;
		EditorGUILayout.LabelField("--------------------------------------------------------", gs);
		EditorGUILayout.LabelField("||                     Note!                       ||", gs);
		EditorGUILayout.LabelField("--------------------------------------------------------", gs);
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		try
		{
			EditorGUILayout.LabelField("Name");
			SerializedProperty nn = m_property.FindPropertyRelative("name");
			nn.stringValue = EditorGUILayout.TextField(nn.stringValue);
			nn.serializedObject.ApplyModifiedProperties();

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Message");
			SerializedProperty mm = m_property.FindPropertyRelative("message");
			mm.stringValue = EditorGUILayout.TextArea(mm.stringValue);
			mm.serializedObject.ApplyModifiedProperties();


			EditorGUILayout.LabelField("Tag");
			SerializedProperty t = m_property.FindPropertyRelative("tag");
			t.stringValue = EditorGUILayout.TextField(t.stringValue);
			t.serializedObject.ApplyModifiedProperties();

		}
		catch
		{

		}
	}



}
#endif




