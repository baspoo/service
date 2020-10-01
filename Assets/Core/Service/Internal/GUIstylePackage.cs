using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIstylePackage : MonoBehaviour {
	static GUIstylePackage m = null;
	public static GUIstylePackage Instant{
		get{ 	
			if (m == null) 	
				m = (Resources.Load ("GUIstylePackage") as GameObject).GetComponent<GUIstylePackage>(); 
			return m;
		}
	}
	public GUIStyle Header;
	public GUIStyle HeaderBigBack;
	public GUIStyle Normal;


	public GUIStyle FindTextBox;
	public GUIStyle FindBtn;
}
