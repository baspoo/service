using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScrollViewOnAwake : MonoBehaviour
{
	public float durationReset;
	bool isRun = false;
	float runing;
	UIScrollView scrollView;
	void OnEnable()
	{
		scrollView = GetComponent<UIScrollView>();
		scrollView.ResetPosition();
		runing = 0.0f;
		if (durationReset!=0.0f) isRun = true;
	}
	void Update()
	{
		if(isRun)
		if (durationReset != 0.0f) {
			if (runing < durationReset)
			{
				scrollView.ResetPosition();
				runing += Time.deltaTime;
			}
			else isRun = false;
		}
	}
	public void Refresh() {
		OnEnable();
	} 
}
