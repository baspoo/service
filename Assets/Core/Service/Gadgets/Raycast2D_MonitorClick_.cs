using UnityEngine;
using System.Collections;


public enum TypeIgnoreMask
{
	only_check_for_collisions_with_layerX	=0,
	ignore_collisions_with_layerX=1
}

public enum Input_Monitor_Type
{
	Click	=0,
	ClickDown=1,
	ClickUp =2
}
public class Raycast2D_MonitorClick_ : MonoBehaviour {
	void Start () {
		_instan = this;
		_ObjRaycasyMover2D = new GameObject("Ray_");
		if(TypeIgnoreMask_ == TypeIgnoreMask.only_check_for_collisions_with_layerX)
			layerMask = 1 << LayerMask.NameToLayer (LayerMask_);
		if(TypeIgnoreMask_ == TypeIgnoreMask.ignore_collisions_with_layerX)
			layerMask = ~(1 << LayerMask.NameToLayer (LayerMask_));
	}
	public static Vector3 CustomCameraVectorPoint;
	public static Raycast2D_MonitorClick_ _instan;
	public static GameObject _ObjRaycasyMover2D; 
	public static GameObject _ObjRaycasyHit; 
	public TypeIgnoreMask TypeIgnoreMask_;
	public  string LayerMask_ ;
	public Camera camera_;
	public Input_Monitor_Type InputMonitor_Type_;
	public EventDelegate Hit_Enter;
	Ray ray;
	Vector3 pos;
	int layerMask=0;
	void Update () { 
		if(Input_(0)){
			Vector3 picker = Input.mousePosition;
			picker.x += CustomCameraVectorPoint.x;
			picker.y += CustomCameraVectorPoint.y;
			picker.z += CustomCameraVectorPoint.z;
			RaycastHit2D hit = Physics2D.Raycast( camera_.ScreenToWorldPoint(picker), Vector2.zero,1000,layerMask);
			Vector3 post = Vector3.zero;
			post.x = hit.point.x;
			post.y = hit.point.y;
			_ObjRaycasyMover2D.transform.position = post;
			if(hit.collider != null)click(hit.collider.gameObject);
		}
	}
	bool Input_(int input)
	{
		bool Click = false;
		if(InputMonitor_Type_ == Input_Monitor_Type.Click) 		if(Input.GetMouseButton(input))Click = true;
		if(InputMonitor_Type_ == Input_Monitor_Type.ClickDown) 	if(Input.GetMouseButtonDown(input))Click = true;
		if(InputMonitor_Type_ == Input_Monitor_Type.ClickUp) 	if(Input.GetMouseButtonUp(input))Click = true;
		return  Click;
	}
	void click(GameObject obj)
	{
		_ObjRaycasyHit = obj;
		if(Hit_Enter!=null) Hit_Enter.Execute();
	}

}
