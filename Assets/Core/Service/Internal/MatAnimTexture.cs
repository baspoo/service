using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatAnimTexture : MonoBehaviour {


	public Material mat;
	public float speed = 2.0f;
	public float time = 0.0f;
	public bool move = false;
	public Texture[] imgs;
	public int index=0;
	public void Change () {
		index++;
		if (index >= imgs.Length)
			index = 0;
		mat.mainTexture = imgs[index];
	}
	void Update () {
		if(move)
			if (time < 1.0f) {
				time += Time.deltaTime * speed;
			} else {
				time = 0.0f;
				Change ();
			}
	}
}
