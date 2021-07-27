using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class StoreObject
{
	public string name;
	public Object fill;
	public bool isPlay = true;
	public bool isEnable;
	public string path;
	public int instanceID;

	Object m_object;
	public Object getobject
	{
		get
		{

			if (!Application.isPlaying)
				return reload(path);

			if (m_object == null)
			{
				if (!string.IsNullOrEmpty(path))
					m_object = reload(path);
			}
			return m_object;
		}
	}

	public GameObject gameObject => (GameObject)getobject;
	public static Object reload(string path)
	{
		return Resources.Load(path) as Object;
	}



	public GameObject PoolEffectAt(Transform t_position, float destime = 5.0f)
	{
		GameObject effect = PoolManager.Spawn(gameObject, t_position, destime).gameObject;
		return effect;
	}
	public GameObject DrawEffectAt(Transform t_position, float destime = 3.0f)
	{
		GameObject effect = Service.GameObj.Created(gameObject, t_position);
		if (destime > 0.0f) UnityEngine.GameObject.Destroy(effect, destime);
		return effect;
	}
	public GameObject DrawEffect(Transform t_position, float destime = 3.0f)
	{
		GameObject effect = Service.GameObj.Created(gameObject, null);
		Service.GameObj.LookAtTransform(effect.transform, t_position);
		if (destime > 0.0f) UnityEngine.GameObject.Destroy(effect, destime);
		return effect;
	}

	public GameObject CreatedAt(Transform t_position = null)
	{
		GameObject asset = Service.GameObj.Created(gameObject, t_position);
		return asset;
	}

}





#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(StoreObject))]
public class StoreObjectDataDrawer : PropertyDrawer
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


		//GUI
		Reposition();
		var isEnable = property.FindPropertyRelative("isEnable").boolValue;
		var instanceID = property.FindPropertyRelative("instanceID").intValue;

		if (isEnable || instanceID != 0)
		{
			if (GUI.Button(Position(position, 24), EditorGUIUtility.FindTexture("d_Prefab Icon")))
			{
				//PING
				#region Ping
				EditorGUIUtility.PingObject(StoreObject.reload(property.FindPropertyRelative("path").stringValue));
				#endregion

				//Refill
				#region Refill
				var obj = StoreObject.reload(property.FindPropertyRelative("path").stringValue);
				if (obj != null)
				{
					//add instanceID
					property.FindPropertyRelative("instanceID").intValue = obj.GetInstanceID();
					Debug.Log("add instanceID:" + property.FindPropertyRelative("instanceID").intValue);
				}
				else
				{
					//find instanceID & refill
					var iid = property.FindPropertyRelative("instanceID").intValue;
					obj = (UnityEngine.Object)typeof(UnityEngine.Object)
								   .GetMethod("FindObjectFromInstanceID", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
								   .Invoke(null, new object[] { iid });

					if (obj != null)
					{
						property.FindPropertyRelative("fill").objectReferenceValue = obj;
						Debug.Log("refill instanceID:" + property.FindPropertyRelative("instanceID").intValue + " - " + obj.name);
					}
					else Debug.Log("refill instanceID:" + property.FindPropertyRelative("instanceID").intValue + " - null");
				}
				#endregion
			}
		}
		//	EditorGUI.PropertyField(Position(position, 20), property.FindPropertyRelative("isPlay"), GUIContent.none);
		if (!isEnable)
		{
			EditorGUI.HelpBox(Position(position, 20), "", MessageType.Warning);
			if (instanceID != 0)
				EditorGUI.LabelField(Position(position, 50), $"({instanceID})");
		}

		EditorGUI.PropertyField(Position(position, 30), property.FindPropertyRelative("fill"), GUIContent.none);
		EditorGUI.PropertyField(Position(position, 300), property.FindPropertyRelative("path"), GUIContent.none);





		//Varidate Path
		if (property.FindPropertyRelative("path").stringValue != pathKeep)
		{
			var obj = StoreObject.reload(property.FindPropertyRelative("path").stringValue);
			property.FindPropertyRelative("isEnable").boolValue = obj != null;
			pathKeep = property.FindPropertyRelative("path").stringValue;
			property.FindPropertyRelative("name").stringValue = (obj != null) ? obj.name : null;
			if (obj != null) property.FindPropertyRelative("instanceID").intValue = obj.GetInstanceID();
		}




		//if (!property.FindPropertyRelative("isPlay").boolValue)
		//{

		//	property.FindPropertyRelative("isPlay").boolValue = true;




		//	//PING
		//	#region Ping
		//	EditorGUIUtility.PingObject(StoreObject.reload(property.FindPropertyRelative("path").stringValue));
		//	#endregion

		//	//Refill
		//	#region Refill
		//	var obj = StoreObject.reload(property.FindPropertyRelative("path").stringValue);
		//	if (obj != null)
		//	{
		//		//add instanceID
		//		property.FindPropertyRelative("instanceID").intValue = obj.GetInstanceID();
		//		Debug.Log("add instanceID:" + property.FindPropertyRelative("instanceID").intValue);
		//	}
		//	else
		//	{
		//		//find instanceID & refill
		//		var iid = property.FindPropertyRelative("instanceID").intValue;
		//		obj = (UnityEngine.Object)typeof(UnityEngine.Object)
		//					   .GetMethod("FindObjectFromInstanceID", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
		//					   .Invoke(null, new object[] { iid });

		//		if (obj != null) 
		//		{
		//			property.FindPropertyRelative("fill").objectReferenceValue = obj;
		//			Debug.Log("refill instanceID:" + property.FindPropertyRelative("instanceID").intValue + " - " + obj.name);
		//		} 
		//		else Debug.Log("refill instanceID:" + property.FindPropertyRelative("instanceID").intValue + " - null");
		//	}
		//	#endregion
		//}







		//if (!property.FindPropertyRelative("isPlay").boolValue)
		//{

		//	property.FindPropertyRelative("isPlay").boolValue = true;




		//	//PING
		//	#region Ping
		//	EditorGUIUtility.PingObject(StoreObject.reload(property.FindPropertyRelative("path").stringValue));
		//	#endregion

		//	//Refill
		//	#region Refill
		//	var obj = StoreObject.reload(property.FindPropertyRelative("path").stringValue);
		//	if (obj != null)
		//	{
		//		//add instanceID
		//		property.FindPropertyRelative("instanceID").intValue = obj.GetInstanceID();
		//		Debug.Log("add instanceID:" + property.FindPropertyRelative("instanceID").intValue);
		//	}
		//	else
		//	{
		//		//find instanceID & refill
		//		var iid = property.FindPropertyRelative("instanceID").intValue;
		//		obj = (UnityEngine.Object)typeof(UnityEngine.Object)
		//					   .GetMethod("FindObjectFromInstanceID", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
		//					   .Invoke(null, new object[] { iid });
		//		property.FindPropertyRelative("fill").objectReferenceValue = obj;
		//		if (obj != null) Debug.Log("refill instanceID:" + property.FindPropertyRelative("instanceID").intValue + " - " + obj.name);
		//		else Debug.Log("refill instanceID:" + property.FindPropertyRelative("instanceID").intValue + " - null");
		//	}
		//	#endregion
		//}




		//Fill Path
		if (property.FindPropertyRelative("fill").objectReferenceValue != null)
		{
			var path = AssetDatabase.GetAssetPath(property.FindPropertyRelative("fill").objectReferenceValue);
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





		// Set indent back to what it was
		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty();
	}
}
#endif
