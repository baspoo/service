using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif




#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(DebugMessageBox))]
[System.Serializable]
public class DebugMessageBoxUI : Editor{
	public DebugMessageBox m_tools {get{ return ((GameObject)Selection.activeObject).GetComponent<DebugMessageBox>();  }}
	public override void OnInspectorGUI()
	{
		if (!Service.GameObj.isObjectNotNull(m_tools)) {
			return;
		}
		if (Service.GameObj.isObjectNotNull(m_tools))
			if (m_tools.OnUpdate != null) {
				m_tools.OnUpdate ();
		}
		if (Service.GameObj.isObjectNotNull(m_tools))
			EditorGUILayout.TextArea ( m_tools.Message );
	}
}
#endif






public class DebugMessageBox : MonoBehaviour
{
	public string Head;
	public string Message;
	public Service.Callback.callback OnUpdate;
}
