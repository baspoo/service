using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeAction : MonoBehaviour
{

	[System.Serializable]
	public class RunData
	{
		public runaction runAction;
		public enum runaction
		{
			enable, parent ,anim, particle, audio, tween
		}
		public Transform tran;
		public Animation anim;
		public AnimationClip clip;
		public ParticleSystem particleSystem;
		public AudioClip audioClip;
		public float waiting;
        #region NGUI
        public UITweener Tween;
        #endregion
    }

	public RunData Run;





	void OnEnable()
    {


		var timrs = GetComponents<AddOn.Timmer>();
		foreach (var t in timrs)
			Destroy(t);


        Debug.Log("OnEnable");
		if (Run.runAction == RunData.runaction.enable) {
			Service.Timmer.Wait(Run.waiting , gameObject , () => {
				Run.tran.gameObject.SetActive(true);
			});
		}
    }
    void OnDisable()
    {
        Debug.Log("OnDisable");
    }
}
