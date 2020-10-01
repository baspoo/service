using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLookAt : MonoBehaviour {
	public Transform Target;
	public float RotationSpeed;
	public Quaternion ViewQuaternionDebug;
	public bool LockX;
	public bool LockY;
	public bool LockZ;
	public bool LockW;
	//values for internal use
	private Quaternion _lookRotation;
	private Vector3 _direction;




	Quaternion GetRotate(Quaternion lookRotation){
		Quaternion p_lookRotation = lookRotation;
		if(LockX)p_lookRotation.x = transform.rotation.x;
		if(LockY)p_lookRotation.y = transform.rotation.y;
		if(LockZ)p_lookRotation.z = transform.rotation.z;
		if(LockW)p_lookRotation.w = transform.rotation.w;
		return p_lookRotation;
	}

	public void LootAtTo (Transform target){
		_direction = (target.position - transform.position).normalized;
		if(_direction!=Vector3.zero)
			_lookRotation = Quaternion.LookRotation (_direction);
		transform.rotation = GetRotate(_lookRotation);
	}
	// Update is called once per frame
	void Update () {
		ViewQuaternionDebug = transform.rotation;
		if (Target != null) {
			//find the vector pointing from our position to the target
			_direction = (Target.position - transform.position).normalized;

			//create the rotation we need to be in to look at the target
			_lookRotation = Quaternion.LookRotation (_direction);

			//rotate us over time according to speed until we are in the required rotation
			transform.rotation = Quaternion.Slerp (transform.rotation, GetRotate(_lookRotation), Time.deltaTime * RotationSpeed);
		}
	}
}
