using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallBack : MonoBehaviour
{
	public Animation anim;
	public string AnimClip;
	public List<SimpleCallBack> SimpleCallBacks;
	public EventDelegate OnFinish;


	//**Option #1
	public void OnAnimFinish()
	{
		if (anim != null)
			foreach (AnimationState AS in anim)
			{
				if (AS.enabled && AS.weight > -1.0)
				{
					AnimClip = AS.name;
				}
			}
		if (OnFinish != null)
			OnFinish.Execute();
	}



	//**Option #2
	public System.Action<string> CallbackState = null;
	public void TakeState(string state)
	{
		CallbackState?.Invoke(state);
	}



	//**Option #3
	public TaskService.Function FunctionCalling = new TaskService.Function();
	public void OnFunctionCalling(string callback)
	{
		FunctionCalling.call(callback);
	}






	//**Option #4
	[System.Serializable]
	public class SimpleCallBack
	{
		public string callbackname;
		public AnimationClip anim;
		public AudioClip audio;
		public Transform enable;
		public Transform disable;
	}
	public void OnSimpleCallback(string callbackname)
	{
		var callback = SimpleCallBacks.Find(x => x.callbackname == callbackname);
		if (callback != null)
		{
			if (callback.enable != null) callback.enable.gameObject.SetActive(true);
			if (callback.disable != null) callback.disable.gameObject.SetActive(false);
			if (callback.anim != null) anim.Play(callback.anim.name);
			if (callback.audio != null) Sound.Play(callback.audio);
		}
	}





}
