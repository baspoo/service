using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCopyView : MonoBehaviour {
	public Camera camera;
	Camera currentcamera;
	void Start(){
		currentcamera = GetComponent<Camera> ();
	}
	void Update () {
		if(camera!=null){
			currentcamera.orthographic 			= camera.orthographic;
			currentcamera.fieldOfView 			= camera.fieldOfView;
			currentcamera.renderingPath 		= camera.renderingPath;
			currentcamera.useOcclusionCulling 	= camera.useOcclusionCulling;
			currentcamera.allowHDR 				= camera.allowHDR;
			currentcamera.allowMSAA 			= camera.allowMSAA;
			currentcamera.nearClipPlane 		= camera.nearClipPlane;
			currentcamera.farClipPlane 			= camera.farClipPlane;
		}
	}
}
