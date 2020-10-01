using UnityEngine;
using System.Collections;

public class CollisionCheck2D_ : MonoBehaviour {


	public bool isUseTrigger;
	public GameObject getEnter(){ return Enter_;}
	public GameObject getStay()	{ return Stay_;}
	public GameObject getExit()	{ return Exit_;}
	public EventDelegate onEnter_;
	public EventDelegate onStay_;
	public EventDelegate onExit_;
	GameObject Enter_;
	GameObject Stay_;
	GameObject Exit_;




	void OnCollisionEnter2D(Collision2D coll) {	Hit(coll.gameObject);}
	void OnTriggerEnter2D(Collider2D hit) {	if(isUseTrigger)Hit(hit.gameObject);	}
	void OnCollisionExit2D(Collision2D coll) {Out(coll.gameObject);}
	void OnTriggerExit2D(Collider2D coll) {if(isUseTrigger)Out(coll.gameObject);}
	void OnCollisionStay2D(Collision2D coll) {Stay(coll.gameObject);}
	void OnTriggerStay2D(Collider2D coll){if(isUseTrigger)Stay(coll.gameObject);}





	void Hit(GameObject obj)
	{
		Enter_ = obj;
		if(onEnter_!=null)onEnter_.Execute();
	}
	void Out(GameObject obj)
	{
		Exit_ = obj;
		if(onExit_!=null)onExit_.Execute();
	}
	void Stay(GameObject obj)
	{
		Stay_ = obj;
		if(onStay_!=null)onStay_.Execute();
	}

}
