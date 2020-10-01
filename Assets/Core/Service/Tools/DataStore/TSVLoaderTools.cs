#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;


[System.Serializable]
public class FileData{
	[SerializeField]
	public string Name;
	public string GID;
	public bool finish;

}

public class TSVLoaderTools : MonoBehaviour {
	static TSVLoaderTools m_loader;
	public  static bool isHave {
		get
		{
			return (m_loader != null);
		}
	}
	public  static TSVLoaderTools loader
	{
		get{
			finish = false;
			if(m_loader!=null)
				DestroyImmediate (m_loader.gameObject);
			m_loader = null;

			m_loader = Instantiate ( Resources.Load("Database/_loader") as GameObject ).GetComponent<TSVLoaderTools>();
			return m_loader;
		}
	}

	public string path;
	public string GoogleSheetID;
	public List<FileData> FileDatas = new List<FileData>();
	int count ;
	int maxcount ;

	public static bool finish;
	public static bool isLoading;
	public void Download(){
		count = 0;
		maxcount = FileDatas.Count;
		foreach (FileData f in FileDatas) {
			StartCoroutine (load(f));
		}
	}
	public void Download( FileData fileData ){
		count = 0;
		maxcount = 1;
		StartCoroutine (load(fileData));
	}
	public void Stop(   ){
		if(m_loader!=null)
			DestroyImmediate (m_loader.gameObject);
		//DebugingEditor.ShowWindow ();
	}
	IEnumerator load (FileData file){



#if !UNITY_EDITOR_WIN
		string chPath = Path.DirectorySeparatorChar.ToString();
		path = path.Replace ("\\",chPath);
		Debug.Log ("path" + path);
#endif
		
		isLoading = true;
		file.finish = false;
		string filePath = path+file.Name+".txt";
		string URL = "https://docs.google.com/spreadsheets/d/" +  GoogleSheetID  +  "/export?gid="+  file.GID  +"&exportFormat=tsv";
		WWW www = new WWW (URL);
		yield return www;
		count++;
		if (www.isDone) 
		{
			if (www.error == null) {
				FileUtil.DeleteFileOrDirectory (filePath);
				File.WriteAllText (filePath,www.text);
				AssetDatabase.ImportAsset (filePath); 
				Debug.Log (file.Name + " : <color=green> Success </color>");
				file.finish = true;
			} else {
				Debug.Log (  file.Name + " : <color=red> Fail => </color>" + www.error);
			}
		}
		else 
		{
			Debug.Log (  file.Name + " : <color=red> Fail </color>");
		}

		if (count == maxcount) {
			isLoading = false;
			finish = true;
			Debug.Log ("<color=yellow> ----------- FINISH -------------</color>");
			m_loader = null;
			DestroyImmediate (gameObject);
			//DebugingEditor.ShowWindow ();
		}
	}

}
#endif