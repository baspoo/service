using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeREUItween : MonoBehaviour
{




	public static void ReAllTween(Transform root, bool isForceToBegin = false)
	{
		foreach (UITweener gettween in root.gameObject.GetComponents<UITweener>())
		{
			ReTween(gettween, isForceToBegin);
		}
	}
	public static void ReTween(UITweener tween, bool isForceToBegin = false)
	{
		tween.ResetToBeginning();
		tween.PlayForward();
		if (isForceToBegin)
			FoceToBegin(tween);
	}
	public static void EnableAll(Transform root, bool isEnable)
	{
		foreach (UITweener gettween in root.gameObject.GetComponents<UITweener>())
		{
			gettween.enabled = isEnable;
		}
	}


	public static void FoceToBegin(UITweener tween)
	{

		var tweenalpha = tween.GetComponent<TweenAlpha>();
		if (tweenalpha != null)
		{
			if (tween.GetComponent<UIPanel>() != null) tween.GetComponent<UIPanel>().alpha = tweenalpha.from;
			if (tween.GetComponent<UILabel>() != null) tween.GetComponent<UILabel>().alpha = tweenalpha.from;
			if (tween.GetComponent<UITexture>() != null) tween.GetComponent<UITexture>().alpha = tweenalpha.from;
		}

		var tweenposition = tween.GetComponent<TweenPosition>();
		if (tweenposition != null)
		{
			tween.transform.localPosition = tweenposition.from;
		}

		var tweenscale = tween.GetComponent<TweenScale>();
		if (tweenscale != null)
		{
			tween.transform.localScale = tweenscale.from;
		}
	}



	public UITweener tween;
	public bool isReAllTween;
	public bool isForceToBegin;
	void OnEnable()
	{
		if (tween != null)
		{
			ReTween(tween, isForceToBegin);
		}
		if (isReAllTween)
		{
			ReAllTween(transform, isForceToBegin);
			if (tween != null)
			{
				ReAllTween(tween.transform, isForceToBegin);
			}
		}
	}

	public void OnBtnRetween()
	{
		ReAllTween(transform, isForceToBegin);
	}


}
