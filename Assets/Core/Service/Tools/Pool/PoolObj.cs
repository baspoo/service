﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BasePool : MonoBehaviour
{
	PoolObj m_pool = null;
	protected PoolObj pool
	{
		get
		{
			if (m_pool == null)
				m_pool = GetComponent<PoolObj>();
			return m_pool;
		}
	}
	public void Destroy()
	{
		if (pool != null)
		{
			pool.Deactive();
		}
		else
		{
			Destroy(gameObject);
		}
	}
}







public class PoolObj : MonoBehaviour {

	public int refID;
	public int ID;
	public bool isActive;
	public System.Action onActive;
	public System.Action onDeactive;
	PoolGroup m_group;
	public List<PoolObj> PoolObjs => m_group.PoolObjs;
	public void init(  PoolGroup group )
	{
		ID = this.GetInstanceID();
		refID = group.pObj.GetInstanceID ();
		m_group = group;
		Deactive ();
	} 


	float m_runtime = 0.0f;
	float m_destime = 0.0f;
	public void Active( float destime = 0.0f ){
		isActive = true;
		gameObject.SetActive (isActive);
		m_destime = destime;
		m_runtime = 0.0f;
		onActive?.Invoke();
	}
	public void Deactive(){
		isActive = false;
		gameObject.SetActive (isActive);

		if (transform.parent != m_group.Transforms) 
		{
			ReturnToOrigin();
		}

		onDeactive?.Invoke();
	}

	public void ReturnToOrigin() 
	{
		gameObject.layer = m_group.Transforms.gameObject.layer;
		transform.parent = m_group.Transforms;
	}




	void Update(){
		if(isActive)
		if (m_destime != 0.0f) {
			m_runtime += Time.deltaTime;
			if (m_runtime >= m_destime)
				Deactive ();
		}
	}



	public void Refresh()
	{
		Service.Timmer.StopAndDelete(ID.ToString());
		gameObject.SetActive(false);
		Service.Timmer.Wait(0.05f, PoolManager.pool.gameObject ,()=> {
			if(Service.GameObj.isObjectNotNull(this))
				gameObject.SetActive(true);
		}).ID = ID.ToString();
	}
	


}
