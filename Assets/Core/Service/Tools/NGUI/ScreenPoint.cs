using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPoint : MonoBehaviour
{

	#region SERVICE
	public static Vector3 GetWorldPositionToOtherCam(Camera maincam,Transform target, Camera othercam){
		Vector3 position = Vector3.zero;
		position = maincam.WorldToScreenPoint(target.position);
		position = othercam.ScreenToWorldPoint (position);
		return position;
	}
	public static Vector3 GetWorldPositionToNGUI(Camera maincam,Transform target, UIRoot root , UIAnchor anc = null){

		//** Setup Anc
		if (anc != null) {
			anc.side = UIAnchor.Side.BottomLeft;
			anc.runOnlyOnce = false;
			anc.enabled = true;
		}

		//** Position
		Vector3 position = Vector3.zero;
		position = maincam.WorldToScreenPoint(target.position);

		//** Screen Percent
		Vector3 percent = Vector3.zero;
		percent.x = position.x / (float)Screen.width;
		percent.y = position.y / (float)Screen.height;

		//** Canvert to Panel Position
		Vector3 panelscreen = Vector3.zero;
		panelscreen.x  =  (float)Screen.width * root.pixelSizeAdjustment;
		panelscreen.y  =  (float)Screen.height * root.pixelSizeAdjustment;
		position.x = panelscreen.x * percent.x;
		position.y = panelscreen.y * percent.y;

		return position;
	}
	#endregion








	[Header("MainCamera")]
	public Camera cam;
	public Transform target;
	Vector3 screenPos;
	Vector2 screenPercent;
    void Update()
    {
		DupCamUpdate ();
		NguiUpdate ();
    }


	[Header("OtherCamera")]
	public bool isOtherCamera;
	public Camera otherCam;
	public Transform moveOtherObj;
	void DupCamUpdate()
	{
		if (!isOtherCamera) return;
		moveOtherObj.transform.position = GetWorldPositionToOtherCam(cam,target,otherCam);
	}




	[Header("NGUI")]
	public bool isNgui;
	public UIRoot root;
	public UIAnchor anc;
	public Transform move;
	Vector2 panelscreen;
	void NguiUpdate()
	{
		if (!isNgui) return;
		move.transform.localPosition =  GetWorldPositionToNGUI(cam,target,root);
	}








}
