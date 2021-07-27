using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif





public class StepByStep : MonoBehaviour
{


	[System.Serializable]
	public class RunData
	{
		public runaction runAction;
		public enum runaction
		{
			enable, create ,destory ,anim, particle, audio, tween, mono
		}
		public GameObject gameobj;
		public UITweener tween;
		public Transform trans;
		public MonoBehaviour mono;
		public bool enable;
		public Animation animation;
		public string clipname;
		public ParticleSystem particleSystem;
		public Color Color;
		public AudioClip audioClip;
		public Material material;
		public float waiting;
		public float value;
		public UnityEngine.UI.Button.ButtonClickedEvent Event;
	}




	bool isRuning = false;
	public List<RunData> Runs = new List<RunData>();
	System.Action<RunData> actiondone;
	System.Action done;
    Coroutine coroutine;
	

	public void OnStart(System.Action done = null)
	{
		OnStart(null, done );
	}
	public void OnStart( System.Action<RunData> actiondone, System.Action done )
	{

		this.actiondone = actiondone;
		this.done = done;
		
        if (!isRuning) {
			isRuning = true;
			coroutine = StartCoroutine(run());
		}
	}
	IEnumerator run()
	{


		foreach (var r in Runs) 
		{
			yield return new WaitForSeconds(r.waiting);
			actiondone?.Invoke(r);
			switch (r.runAction)
			{
				case RunData.runaction.enable:
					r.trans.gameObject.SetActive(r.enable);
					break;
				case RunData.runaction.mono:
					r.mono.enabled = (r.enable);
					break;
				case RunData.runaction.create:
					r.gameobj.Create(r.trans);
					break;
				case RunData.runaction.destory:
					Destroy(r.gameobj);
					break;
				case RunData.runaction.particle:
					ParticleReReady.RePlay(r.particleSystem.transform);
					break;
				case RunData.runaction.anim:
					r.animation.Stop();
					r.animation.Play(r.clipname);
					break;
				case RunData.runaction.tween:
					AwakeREUItween.ReAllTween(r.tween.transform,true);
					break;
				case RunData.runaction.audio:
					Sound.Play(r.audioClip);
					break;
			}

		}
		done?.Invoke();
	}

	




	void Update()
	{

	}
	public void OnStop()
	{
		isRuning = false;
		if(coroutine!=null)
			StopCoroutine(coroutine);
	}
}


























#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(StepByStep))]
[System.Serializable]
public class FinisHimObjUI : Editor
{
	public StepByStep m_tools { get { return ((GameObject)Selection.activeObject).GetComponent<StepByStep>(); } }
	public override void OnInspectorGUI()
	{
		//base.OnInspectorGUI();
		if (!Service.GameObj.isObjectNotNull(m_tools))
		{
			return;
		}

		EditorGUILayout.Space();
		if (EditorGUIService.DrawHeader("Step", "Step", false, false))
		{
			EditorGUIService.BeginContents(false);
			{
				foreach (var r in m_tools.Runs)
				{
					EditorGUILayout.BeginHorizontal();
					r.waiting = EditorGUILayout.FloatField(r.waiting , GUILayout.Width(  35.0f ));
					r.runAction = (StepByStep.RunData.runaction)EditorGUILayout.EnumPopup(r.runAction);


					switch (r.runAction)
					{
						case StepByStep.RunData.runaction.enable:
							r.trans = (Transform)EditorGUILayout.ObjectField(r.trans, typeof(Transform));
							r.enable = EditorGUILayout.Toggle(r.enable);
							break;
						case StepByStep.RunData.runaction.mono:
							r.mono = (MonoBehaviour)EditorGUILayout.ObjectField(r.mono, typeof(MonoBehaviour));
							r.enable = EditorGUILayout.Toggle(r.enable);
							break;
						case StepByStep.RunData.runaction.create:
							r.gameobj = (GameObject)EditorGUILayout.ObjectField(r.gameobj, typeof(GameObject));
							r.trans = (Transform)EditorGUILayout.ObjectField(r.trans, typeof(Transform));
							break;
						case StepByStep.RunData.runaction.destory:
							r.gameobj = (GameObject)EditorGUILayout.ObjectField(r.gameobj, typeof(GameObject));
							break;
						case StepByStep.RunData.runaction.particle:
							r.particleSystem = (ParticleSystem)EditorGUILayout.ObjectField(r.particleSystem, typeof(ParticleSystem));
							break;
						case StepByStep.RunData.runaction.anim:
							r.animation = (Animation)EditorGUILayout.ObjectField(r.animation, typeof(Animation));
							r.clipname = EditorGUILayout.TextField(r.clipname);
							break;
						case StepByStep.RunData.runaction.tween:
							r.tween = (UITweener)EditorGUILayout.ObjectField(r.tween, typeof(UITweener));
							break;
						case StepByStep.RunData.runaction.audio:
							r.audioClip = (AudioClip)EditorGUILayout.ObjectField(r.audioClip, typeof(AudioClip));
							break;
		
					}



					GUI.backgroundColor = Color.red;
					if (GUILayout.Button("X", GUILayout.Width(40.0f)))
					{
						m_tools.Runs.Remove(r);
						return;
					}
					GUI.backgroundColor = Color.white;
					EditorGUILayout.EndHorizontal();
				}

				if (GUILayout.Button("+"))
				{
					m_tools.Runs.Add(new StepByStep.RunData());
				}












			}
			EditorGUIService.EndContents();
		}


		if (Application.isPlaying && GUILayout.Button("Run"))
		{
			m_tools.OnStart(()=> { Debug.Log("Done!"); });
		}
	}



}
#endif



