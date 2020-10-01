using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif




public class CameraRecord : MonoBehaviour
{



	public Camera cam;
	// Start is called before the first frame update
	public void StartCap(string name , float duration)
    {
		StartCoroutine(F(name, duration)) ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public Color A;
	public Color B;
	IEnumerator F(string m_name , float duration) {
		Camera cam = GetComponent<Camera>();
		int sqNumber = 0;
		bool isRun = true;
		yield return new WaitForEndOfFrame();
		while (isRun) {
			duration -= Time.deltaTime;
			if (duration <= 0.0f)
				isRun = false;
#if UNITY_EDITOR
            CameraRecordUI.OnCaptureDisplay(m_name + "_" + sqNumber.ToString(), cam);
#endif
			sqNumber++;
			yield return new WaitForEndOfFrame();
		}
	}


}




#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(CameraRecord))]
[System.Serializable]
public class CameraRecordUI : Editor
{

	static bool isAnimCap = false;
	public static int F = 30;
	public static bool isAlpha = false;
	public static float captime;
	public static string ImageName;
	int sqNumber = 0;

	public Transform t_openMe;
	void Update() { if (isAnimCap) Repaint(); }

	public CameraRecord m_tools { get { return ((GameObject)Selection.activeObject).GetComponent<CameraRecord>(); } }
	public Camera cam { get { return ((GameObject)Selection.activeObject).GetComponent<Camera>(); } }
	public override void OnInspectorGUI()
	{

		void Capture() {

			if (captime > 0.0f)
			{
#if UNITY_EDITOR
				EditorApplication.isPaused = false;
#endif
				//sqNumber = 0;
				//isAnimCap = true;
				m_tools.StartCap(ImageName, captime);
				Application.targetFrameRate = F;
			}
			else
			{
				OnCaptureDisplay(ImageName, cam);
			}

		}



		if(GUILayout.Button((Texture)cam.targetTexture , GUILayout.Width(300.0f) , GUILayout.Height(300.0f))){
			Capture();
		}

		ImageName = EditorGUILayout.TextField("PathName",ImageName);
		captime = EditorGUILayout.FloatField("Duration", captime);
		F = EditorGUILayout.IntField("FrameRate", F);
		isAlpha = EditorGUILayout.Toggle("isAlpha", isAlpha);
		cam.backgroundColor = (isAlpha) ? m_tools.A : m_tools.B;





		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("-" , GUILayout.Width(40.0f)) )
		{
			float s = Time.timeScale;
			s -= 0.1f;
			if (s < 0.0f) s = 0.0f;
			Time.timeScale = s;
		}
		EditorGUILayout.FloatField("TimeScale", Time.timeScale);
		if (GUILayout.Button("+", GUILayout.Width(40.0f)))
		{
			Time.timeScale += 0.1f;
		}
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button("\nCapture\n"))
		{
			Capture();
		}
		if (GUILayout.Button("Open Directory")) {
			string path = "CameraRecord";
			if (!System.IO.Directory.Exists(path))
			{
				System.IO.Directory.CreateDirectory(path);
			}
			System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
			{
				FileName = path,
				UseShellExecute = true,
				Verb = "open"
			});
		}
	}







	public static void OnCaptureDisplay( string ImageName , Camera cam)
	{
#if UNITY_EDITOR
		//Path
		string path = "CameraRecord";
		if (!System.IO.Directory.Exists(path))
		{
			System.IO.Directory.CreateDirectory(path);
		}
		string fullpath = path + System.IO.Path.DirectorySeparatorChar + ImageName + ".png";
		Debug.Log("fullpath:" + fullpath);
		Texture2D texture2D = Service.Image.RenderTextureToTexture2D(cam.targetTexture, false);
		texture2D.alphaIsTransparency = true;
		texture2D.Apply(false);
		byte[] bytes = texture2D.EncodeToPNG();
		System.IO.File.WriteAllBytes(fullpath, bytes);
		texture2D.alphaIsTransparency = true;
		texture2D.Apply(false);
		AssetDatabase.Refresh();
#endif
	}



}
#endif

