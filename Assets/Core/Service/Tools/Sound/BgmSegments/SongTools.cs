using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(SongTools))]
public class SongToolsUI : Editor{
	public SongTools tool{get{return ((GameObject)Selection.activeObject).GetComponent<SongTools> ();}}
	public override void OnInspectorGUI()
	{
		if (tool != null) {
			tool.DisplayGUI ();
			((GameObject)Selection.activeObject).transform.hideFlags = HideFlags.HideInInspector;
		}
	}
}
#endif




public class SongTools : MonoBehaviour {

	//[Range(0.0f,1.0f)]
	public float volume = 1.0f;

	SongPart nextSongPart = null;
	SongPart currentSongPart;
	SongPart.Segment currentSegment;
	SongPart.Segment nextSegment;

	float current_time;
	public string PartName;


	AudioSource m_audioSource;
	AudioSource audioSource{
		get { 
			if(m_audioSource==null)
				m_audioSource = transform.GetChild (0).GetComponent<AudioSource> ();
			return m_audioSource;
		}
	}
	public SongPart[] Parts{
		get { 
			return transform.GetComponents<SongPart> ();
		}
	}
	public SongPart Find( string ID ){
		foreach(SongPart a in Parts){
			if (a.PartName == ID)
				return a;
		}
		Debug.LogError ("SongID == null");
		return null;
	}








	public void ChangeAlbum(SongPart  part){
		if (currentSongPart == null) {
			current_time = 0.0f;
			nextSongPart = null;
			LetgoPart (part);
		} 
		else {
			nextSongPart = part;

			nextSongPart.Init ();
			currentSongPart.OnEnding ();

			nextSegment = currentSongPart.NextAudioSegment;
			if (nextSegment == null)
				nextSegment = nextSongPart.AudioSegment;
			if (nextSegment == null)
				nextSegment = nextSongPart.NextAudioSegment;
			if (nextSegment == null)
				Debug.LogError ("nextSegment == null");
			
		}
	}
	void LetgoPart( SongPart  part){
		//Debug.Log (" Part Start : " + part.PartName);
		currentSongPart = part;
		currentSongPart.Init ();
		nextSongPart = null;
		Play ();
	}





	public void Stop(){
		currentSongPart = null;
		audioSource.Stop ();
	}

	public void Play(){
		current_time = 0.0f;
		if (currentSongPart != null) {

			if (currentSongPart.AudioSegment != null) {
				currentSegment = currentSongPart.AudioSegment;
				//Debug.Log ("Play : " + currentSegment.AudioClip.name);
				audioSource.PlayOneShot (currentSegment.AudioClip);

				if (currentSongPart.IsEnding) {
					currentSongPart.OnFinishingPart ();
					nextSegment = nextSongPart.AudioSegment;
				} else {
					nextSegment = currentSongPart.NextAudioSegment;
				}

			} else {
				
				if (!currentSongPart.IsEnding) {
					Next ();
				} else {

					if (nextSongPart != null)
						LetgoPart (nextSongPart);
					else
						Debug.Log ("IsEnding");
	
				}


			}

		} else {
			//** First Time Play
			if (Parts.Length > 0)
				ChangeAlbum (Parts [0]);
			else
				Debug.LogError ("! Parts.Length == 0");
		}
	}
	void Next(){
		currentSongPart.Next ();
		Play ();
	}





	void Update(){

		if (audioSource != null)
			audioSource.volume = volume;

		if (currentSongPart == null)
			return;
		current_time += Time.deltaTime;
		if ((current_time / currentSegment.AudioClip.length) >= currentSegment.PercentFade(nextSegment)) {
			Next ();
		}
	}








	public void DisplayGUI(  ){
		#if UNITY_EDITOR
		gameObject.name = gameObject.name;
		//source = (Transform)EditorGUILayout.ObjectField(source, typeof(Transform), true);


		if (nextSongPart != null) {
			string text = "";
			text += "\n" + "Ready! Next Part ====> " + nextSongPart.PartName;
			EditorGUILayout.HelpBox (text, MessageType.Warning);
		}
		if (currentSongPart != null) {
			string text = "";
			text += "\n" + "PartName : " + currentSongPart.PartName;
			text += "\n" + "SegmentIndex : " + currentSongPart.GetSegmentIndex.ToString();
			text += "\n" + "Segment : " + currentSegment.AudioClip.name;
			text += "\n" + "OnFade : " +  ((int)(currentSegment.PercentFade(nextSegment)*100)).ToString() + "%";
			text += "\n" + "Current Time : " + current_time;
			text += "\n" + "Source Time : " + currentSegment.AudioClip.length;
			if(nextSegment!=null)
				text += "\n\n\n\n" + "Next Segment : " + nextSegment.AudioClip.name;
			EditorGUILayout.HelpBox (text, MessageType.Info);
		}

		//PartName = EditorGUILayout.TextField ( "PartName " , PartName );
		volume = EditorGUILayout.Slider("Volume:",volume, 0.0f, 1.0f);

		if (currentSongPart == null) {
			if (GUILayout.Button ("Play")) {
				Play ();
			}
		} 
		else 
		{
			foreach (SongPart part in Parts) {
				if (GUILayout.Button ("Change Part = ["+part.PartName+"]")) {
					ChangeAlbum ( part );
				}
			}

			GUILayout.Space (10);
			if (GUILayout.Button ("Stop")) {
				Stop ();
			}
		}

		#endif


	}
}
