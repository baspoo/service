using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterScrollViewHightLight : MonoBehaviour
{

	public bool isScale;
	public bool isAlpha;
	public UIWidget widget;
	public Transform center;
	public float force_of_scale;
	public float force_of_alpha;

    void Update()
    {
		float dir = Vector3.Distance (transform.position, center.position);
		if (isScale) 
		{
			float power = 1.0f - ((dir * force_of_scale));
			if (power > 1.0f) 	power = 1.0f;
			if (power < 0)		power = 0.0f;
			Vector3 sc = Vector3.one * power;
			transform.localScale = sc;
		}
		if (isAlpha) 
		{
			float power = 1.0f - ((dir * force_of_alpha));
			widget.alpha = power;
		}
    }






}
