using System.Collections;
using System.Collections.Generic;
using UnityEngine;



#if UNITY_EDITOR
using UnityEditor;
public class SceneGUI : EditorWindow
{
    [MenuItem("Window/Scene GUI/Enable")]
    public static void Enable()
    {
        SceneView.onSceneGUIDelegate += OnScene;
        Debug.Log("Scene GUI : Enabled");
    }

    [MenuItem("Window/Scene GUI/Disable")]
    public static void Disable()
    {
        SceneView.onSceneGUIDelegate -= OnScene;
        Debug.Log("Scene GUI : Disabled");
    }

    private static void OnScene(SceneView sceneview)
    {

        if (Selection.activeObject != null)
        {
            var g = (GameObject)Selection.activeObject;
            Rect rect = new Rect(g.transform.position.x, g.transform.position.y, 100, 50);
            if (GUI.Button(rect, "Button"))
            {
                // Button clicked.
            }

        }
        else
        {
            Disable();
        }

        Handles.BeginGUI();


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Press Me"))
            Debug.Log("Got it to work.");
        if (GUILayout.Button("Press Me"))
            Debug.Log("Got it to work.");
        if (GUILayout.Button("Press Me"))
            Debug.Log("Got it to work.");
        GUILayout.EndHorizontal();



        Handles.EndGUI();
    }
}
#endif