using UnityEngine;
using System.Collections;
public class Ray3d_obj : MonoBehaviour {
    public void reStart() { Start(); }
	void Start () {
        if (TypeIgnoreMask_ == TypeIgnoreMask.only_check_for_collisions_with_layerX)
            layerMask = 1 << LayerMask.NameToLayer(LayerMask_);
        if (TypeIgnoreMask_ == TypeIgnoreMask.ignore_collisions_with_layerX)
            layerMask = ~(1 << LayerMask.NameToLayer(LayerMask_));
	}
    public TypeIgnoreMask TypeIgnoreMask_;
    int layerMask = 0;
    public string LayerMask_;
    public Vector3 Diraction;
	Vector3 ray;
    public Vector3 TargetStart;
	public bool isParentThisTransform;
	RaycastHit hit;
    public float rang ;
	void Update () {

		if (isParentThisTransform) {
			TargetStart = this.transform.position;
			Diraction =  transform.TransformDirection( Vector3.forward);
		}

		ray = transform.TransformDirection(Diraction);
        Debug.DrawRay(TargetStart, Diraction * rang, Color.blue);
        if (Physics.Raycast(TargetStart, Diraction, out hit, rang, layerMask))
        {
          
            if (hit.collider.gameObject != null)
            {
				//Intersec collider
				if (_objTargetHit != null)
					if (_objTargetHit != hit.collider.gameObject)
						isHit = false;
				
                _objTargetHit = hit.collider.gameObject;
				if (!isHit)
				if (OnHit != null) {
					OnHit.Execute ();
				}
				if(OnStay!=null)
					OnStay.Execute();
            }
			isHit = true;
        }
        else
        {
			if (isHit) {
				if (OnOut != null)
					OnOut.Execute ();

		
			}
            _objTargetHit = null;
            isHit = false;
        }
	}
    public GameObject _objTargetHit;
    public EventDelegate OnHit;
	public EventDelegate OnOut;
	public EventDelegate OnStay;
    public bool isHit;




}
