using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallBack : MonoBehaviour {
	public Animation anim;
	public string AnimClip;
	public EventDelegate OnFinish;


	//**Option #1
	public void OnAnimFinish(){
		if(anim!=null)
			foreach (AnimationState AS in anim) {
			if (AS.enabled && AS.weight > -1.0) {
				AnimClip = AS.name;
			}
		}
		if (OnFinish != null)
			OnFinish.Execute ();
	}


	//**Option #2
	public Service.Callback.callback[] CallbackIndex = new Service.Callback.callback[10];
	public void Take ( int index ) {
		if(index<CallbackIndex.Length)
		if(CallbackIndex [index] !=null)
			CallbackIndex [index] ();
	}



	//**Option #3
	public Service.Callback.callback_data CallbackState = null;
	public void TakeState ( string state ) {
		if (CallbackState != null)
			CallbackState (state);
	}



	//**Option #4
	public Service.Tools.FunctionCalling FunctionCalling = new Service.Tools.FunctionCalling();
	public void OnFunctionCalling ( string callback ) {
		//Debug.LogError("OnFunctionCalling:" + callback);
		FunctionCalling.Call (callback);
	}

}
