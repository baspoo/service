using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheckObj : MonoBehaviour
{
	public bool isRotate;
	public bool isPosition;
	public bool isScale;

	public void OnSheck( float time = 0.0f )
	{
		lastposition = gameObject.transform.localPosition;
		Clear ();
		if (time == 0.0f)
			m_timeSheck = timeSheck;
		else
			m_timeSheck = time;
		isSheck = true;
	}
	public Vector3 lastposition = Vector3.zero;
	bool isSheck = false;
	public float forceSheck = 0.05f;
	public float timeSheck = 0.25f;
	float m_timeSheck = 0.0f;
	float runSheck = 0.0f;
	void Update(){
		if (isSheck) {
			if (runSheck < m_timeSheck) 
			{
				gameObject.transform.localRotation = Quaternion.identity;
				runSheck += Time.deltaTime;

				if (isRotate) 
				{
					Vector3 shcekpost = Vector3.zero;
					shcekpost.x = Random.Range (-forceSheck,forceSheck);
					shcekpost.y = Random.Range (-forceSheck,forceSheck);
					shcekpost.z = Random.Range (-forceSheck,forceSheck);
					gameObject.transform.Rotate (shcekpost);
				}
				if (isPosition) 
				{
					Vector3 shcekpost = lastposition;
					shcekpost.x += Random.Range (-forceSheck,forceSheck);
					shcekpost.y += Random.Range (-forceSheck,forceSheck);
					shcekpost.z += Random.Range (-forceSheck,forceSheck);
					gameObject.transform.localPosition =  (shcekpost);
				}
				if (isScale) 
				{
					Vector3 shcekpost = Vector3.one;
					shcekpost.x = Random.Range (-forceSheck,forceSheck);
					shcekpost.y = Random.Range (-forceSheck,forceSheck);
					shcekpost.z = Random.Range (-forceSheck,forceSheck);
					gameObject.transform.localScale = (shcekpost);
				}
			} 
			else 
			{
				Clear ();
			}
		}
	}
	void Clear(){
		isSheck = false;
		runSheck = 0.0f;
		if(isRotate)	gameObject.transform.localRotation 		= Quaternion.identity;
		if(isScale)		gameObject.transform.localScale 		= Vector3.one;
		if(isPosition)	gameObject.transform.localPosition 		= lastposition;
	}
}
