using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObj : MonoBehaviour {

	public int refID;
	public int ID;
	public bool isActive;

	public Service.Callback.callback onActive;
	public Service.Callback.callback onDeactive;

	public void init(  GameObject p ){
		refID = p.GetInstanceID ();
		Deactive ();
	} 


	float m_runtime = 0.0f;
	float m_destime = 0.0f;
	public void Active( float destime = 0.0f ){
		isActive = true;
		gameObject.SetActive (isActive);
		m_destime = destime;
		m_runtime = 0.0f;
		if (onActive != null)
			onActive ();
	}
	public void Deactive(){
		isActive = false;
		gameObject.SetActive (isActive);
		if (onDeactive != null)
			onDeactive ();
	}


	void Update(){
		if(isActive)
		if (m_destime != 0.0f) {
			m_runtime += Time.deltaTime;
			if (m_runtime >= m_destime)
				Deactive ();
		}
	}

}
