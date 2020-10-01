using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeREUItween : MonoBehaviour {




	public static void ReAllTween(  Transform root )
	{
		foreach (UITweener gettween in root.gameObject.GetComponents<UITweener> ()) {
			ReTween(gettween);
		}
	}
	public static void ReTween(UITweener tween)
	{
		tween.ResetToBeginning();
		tween.PlayForward();
	}
	public static void EnableAll( Transform root , bool isEnable)
	{
		foreach (UITweener gettween in root.gameObject.GetComponents<UITweener>()){
			gettween.enabled = isEnable;
		}
	}



	public UITweener tween;
	public bool isReAllTween;
	void OnEnable()
	{
		if (tween!=null) 
		{
			tween.ResetToBeginning ();
			tween.PlayForward ();
		}
		if (isReAllTween)
		{
			ReAllTween(transform);
			if (tween != null) {
				ReAllTween(tween.transform);
			}
		}
	}

	public void OnBtnRetween() {
		ReAllTween(transform);
	}


}
