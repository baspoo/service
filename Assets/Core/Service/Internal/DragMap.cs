using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMap : MonoBehaviour {
	public static DragMap Map;
	public Transform Root;
	public TouchZoom_ zoom;
	public float delta;
	private float dist;
	public Vector2 vertical, horizontal;
	private Vector3 MouseStart, MouseMove;
	private Vector3 derp;

	private Vector3 def;
	void Start () {
		Map = this;
		dist = transform.position.z;  // Distance camera is above map
		def = transform.position;
	}
	public void ResetPosition()
	{
		var tweenposition = GetComponent<TweenPosition>();
		tweenposition.from = transform.localPosition;
		AwakeREUItween.ReAllTween(Root);

		//Root.localPosition = Vector3.zero;
	}
	bool isCanDrag = true;
	void Update () {
		if (Input.touchCount == 0)
			isCanDrag = true;

		if (Input.touchCount == 2) {
			isCanDrag = false;
		}




	



		if (isCanDrag) 
		{
			if (Input.GetMouseButtonDown (2)) 
			{
				MouseStart = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, dist);
			} 
			else if (Input.GetMouseButton (2)) 
			{
				MouseMove = new Vector3 (Input.mousePosition.x - MouseStart.x, Input.mousePosition.y - MouseStart.y, dist);
				MouseStart = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, dist);


				Vector3 masterPosition = Root.localPosition;
				//Vector3 position = new Vector3(masterPosition.x - MouseMove.x * Time.deltaTime * delta, dist, masterPosition.z - MouseMove.y * Time.deltaTime * delta);
				Vector3 position = new Vector3(masterPosition.x - MouseMove.x * Time.deltaTime * delta, masterPosition.y - MouseMove.y * Time.deltaTime * delta , masterPosition.z );

				//horizontal
				if (position.x > horizontal.x)
				{
					position.x = horizontal.x;
				}
				if (position.x < horizontal.y)
				{
					position.x = horizontal.y;
				}
				//vertical
				if (position.y > vertical.x)
				{
					position.y = vertical.x;
				}
				if (position.y < vertical.y)
				{
					position.y = vertical.y;
				}



				Root.localPosition = position;




			}
		}
	}

	//public void ResetPosition () {
	//	transform.position = def;
	//	zoom.OnReset ();
	//}

}