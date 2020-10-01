using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatreralMoveSet_Dissolve : MonoBehaviour
{


	public Material mat;
	public float test;
	void Start () {
		if (mat == null) {
			mat = gameObject.GetComponent<Renderer> ().material;
		}
	}
    void Update()
    {
		if (mat != null) 
		{



			mat.SetFloat ("_DissolveAmount", test);






		}
    }




}
