using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class SplineWalker : MonoBehaviour {


	public Transform locator;
	public BezierSpline spline;
	public BezierCurve curve;

	public float duration;

	public bool lookForward;

	public Bezier.SplineWalkerMode mode;

	[Range(0.0f,1.0f)]
	public float progress;
	private bool goingForward = true;

	private void Update () {
		if (goingForward) {
			progress += Time.deltaTime / duration;
			if (progress > 1f) {
				if (mode == Bezier.SplineWalkerMode.Once) {
					progress = 1f;
				}
				else if (mode == Bezier.SplineWalkerMode.Loop) {
					progress -= 1f;
				}
				else {
					progress = 2f - progress;
					goingForward = false;
				}
			}
		}
		else {
			progress -= Time.deltaTime / duration;
			if (progress < 0f) {
				progress = -progress;
				goingForward = true;
			}
		}




		UpdateLocator();



	}



    public void UpdateLocator()
    {
		if (locator != null)
		{
			if (spline != null)
			{
				Vector3 position = spline.GetPoint(progress);
				locator.localPosition = position;
				if (lookForward)
				{
					locator.LookAt(position + spline.GetDirection(progress));
				}
			}
			if (curve != null)
			{
				Vector3 position = curve.GetPoint(progress);
				locator.localPosition = position;
				if (lookForward)
				{
					locator.LookAt(position + curve.GetDirection(progress));
				}
			}
		}
	}


}














#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(SplineWalker))]
[System.Serializable]
public class SplineWalkerUI : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		var splineWalker = (SplineWalker)target;
		splineWalker.UpdateLocator();
	}
}
#endif