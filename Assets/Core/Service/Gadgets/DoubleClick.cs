using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DoubleClick : MonoBehaviour {
	int click = 0;
	public double delay;
	System.DateTime lastclick;
	void Clear(){
		click = 0;
	}
	public void Click(){
		if (System.DateTime.Now > lastclick.AddSeconds (delay))
			Clear ();
		if (click == 0) {
			lastclick = System.DateTime.Now;
			click = 1;
		}
		else if (click == 1) {
			if (System.DateTime.Now < lastclick.AddSeconds (delay)) {
				if (OnDoubleClick != null)
					OnDoubleClick.Execute ();
			}
			Clear ();
		}
	}
	public EventDelegate OnDoubleClick;

}
