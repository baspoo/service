using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RefreshGameObject : MonoBehaviour {



	public float Duration;
	bool isTimeout  = false;
	float run;
	IEnumerator Start () {
		yield return new WaitForEndOfFrame ();
		while (!isTimeout) {
			Refresh (false);
			yield return new WaitForEndOfFrame ();
			Refresh (true);
		}
		yield return new WaitForEndOfFrame ();
		Refresh (true);
	}
	void Update(){
		if (!isTimeout) {
			if (run < Duration)
				run += Time.deltaTime;
			else {
				run = 0.0f;
				isTimeout = true;
				Destroy (this);
				Refresh (true);
			}
		}
	}







	public Cloth cloth;
	public GameObject gameObject;
	public Transform trans;
	public Renderer render;
	void Refresh(bool isActive){
		if(cloth!=null)cloth.enabled = isActive;
		if(gameObject!=null)gameObject.SetActive (isActive);
		if(trans!=null)trans.gameObject.SetActive (isActive);
		if(render!=null)render.enabled = isActive;
	}

}
