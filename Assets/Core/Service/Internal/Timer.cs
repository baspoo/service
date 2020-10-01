using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	public int Sec;
	public int Min;
	public int FixSec;
	public int FixMin;
	public EventDelegate OnTick;
	float timer;
	void Update () {
		if (timer < 1.0f)
			timer += Time.deltaTime;
		else
			UpdateTime ();
	}
	void UpdateTime()
	{
		timer = 0.0f;
		Sec++;
		if (Sec == 60) {
			Sec = 0;
			Min++;
		}
		if ((Sec == FixSec) && (Min == FixMin))
		if (OnTick != null)
			OnTick.Execute ();

	}
}
