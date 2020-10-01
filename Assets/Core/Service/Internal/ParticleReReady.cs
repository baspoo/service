using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleReReady : MonoBehaviour {
	public ParticleSystem particlesystem;
	public bool isLoop;
	public float MinTime;
	public float MaxTime;
	public float autoDes;


	public static void RePlayAllParent(Transform root){
		foreach (GameObject p in Service.GameObj.GetAllParent(root))
		{
			RePlay(p.transform);
		}
	}
	public static void RePlay(ParticleReReady particlem)
	{
		if (particlem.particlesystem != null)
		{
			particlem.particlesystem.Stop();
			particlem.particlesystem.Play();
			if (particlem.autoDes != 0.0f)
			{
				Destroy(particlem.gameObject, particlem.autoDes);
			}
		}

	}
	public static void RePlay(Transform particlesystem)
	{
		RePlay(particlesystem.GetComponent<ParticleReReady>());
	}

	public static void StopLoopAllParent(Transform root)
	{
		foreach (GameObject p in Service.GameObj.GetAllParent(root))
		{
			ParticleSystem particlesystem = p.GetComponent<ParticleSystem>();
			if(particlesystem!=null)
				particlesystem.loop = false;
		}
	}

	void Start()
	{
		RePlay(this);
	}
	float time = 0.0f;
	float maxtime = 0.0f;
	void Update(){
		if (isLoop) {
			if (time < maxtime) {
				time += Time.deltaTime;
			} else {
				time = 0.0f;
				maxtime = Random.Range (MinTime,MaxTime);
				Start ();
			}
		}
	}

	public void Active(){
		gameObject.SetActive (true);
		Start ();
	}
	void OnEnable(){
		RePlay(this);
	}
}
