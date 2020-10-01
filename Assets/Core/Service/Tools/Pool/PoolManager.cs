using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PoolManager : MonoBehaviour {
	static PoolManager m = null;
	public static PoolManager pool{
		get{ 	
			if (m == null) {
				m = FindObjectOfType<PoolManager> ();
			}
			return m;
		}
	}
	public static Transform getTransform{
		get{ 
			return pool.gameObject.transform;
		}
	}





	public static void Init(){
		foreach(GameObject g_pool in Service.GameObj.GetAllParent(pool.transform)){
			PoolGroup poolgroup = g_pool.GetComponent<PoolGroup> ();
			poolgroup.init ();
			pool.PoolGroups.Add (poolgroup);
			for (int n = 0; n < poolgroup.Amount; n++) {
				pool.Create ( poolgroup );
			}
		}
	}





	//**  Create
	PoolObj Create(  PoolGroup poolgroup  ){
		GameObject g =	Service.GameObj.Created ( poolgroup.pObj , poolgroup.Transforms  );
		PoolObj poolObj = g.AddComponent<  PoolObj > ();
		poolObj.init ( poolgroup.pObj );
		poolgroup.PoolObjs.Add ( poolObj );
		return poolObj;
	} 
	//**  Group
	//** Dev Self CreateNewPoolGroup For Preload Asset Count Only
	//** IF Not Need Preload You Not Self CreateNewPoolGroup..... Use Spawn ==> He Auto CreateNewPoolGroup OK!
	List<PoolGroup> m_PoolGroups = new List<PoolGroup>();
	public List<PoolGroup> PoolGroups {get{return m_PoolGroups;}}
	public static PoolGroup CreateNewPoolGroup(  GameObject p  ){
		GameObject g = new GameObject( ) ;
		PoolGroup pg = g.AddComponent <PoolGroup>();
		pg.pObj = p;
		pg.init ();
		pool.PoolGroups.Add (pg);
		return pg;
	}
	public static PoolGroup FindPoolGroup( GameObject p ){
		foreach (PoolGroup pg in pool.PoolGroups) {
			if (p.GetInstanceID () == pg.ID) {
				return pg;
			}
		}
		return null;
	}


	//** Function
	public static PoolObj Spawn(  GameObject newObj , Transform tran , float destime = 0.0f){
		PoolGroup pg = FindPoolGroup (newObj);
		if (pg == null) {
			pg = CreateNewPoolGroup (newObj);
		}
		PoolObj p = pg.FindAvalible ();
		if (p == null)
			p = pool.Create ( pg );
		if(tran!=null)
			p.transform.position = tran.position;
		p.transform.localScale = Vector3.one;
		p.Active (destime);
		return p;
	}
	public static PoolObj SpawParent(  GameObject newObj , Transform tran , float destime = 0.0f){
		PoolObj p = Spawn (newObj,tran,destime);
		p.transform.parent = tran;
		p.transform.localPosition = Vector3.zero;
		p.transform.localScale = Vector3.one;
		return p;
	}

	public static void Dispose (  GameObject g ){
		if (!Service.GameObj.isObjectNotNull ((object)g))	return;
		PoolObj p = g.GetComponent<PoolObj> ();
		if (p != null)
			Dispose (p);
	} 
	public static void Dispose (  Behaviour script ){
		if (!Service.GameObj.isObjectNotNull ((object)script))	return;
		PoolObj p = script.gameObject.GetComponent<PoolObj> ();
		if (p != null)
			Dispose (p);
	}
	public static void Dispose(  PoolObj p ){
		if (!Service.GameObj.isObjectNotNull ((object)p))	return;
		p.Deactive ();
	} 
	public static void Clean( ){
		pool.PoolGroups.Clear ();
		Service.GameObj.DesAllParent (pool.transform);
	} 

}
