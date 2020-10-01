using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFinishSelfActive : MonoBehaviour {
	public void ActiveMe(){
		gameObject.SetActive (true);
	}
	public void UnActiveMe(){
		gameObject.SetActive (false);
	}
}
