using UnityEngine;
using System.Collections;

public class RotaionManager : MonoBehaviour {

	// Motion start point
	public Transform origin;
	// Motion end point
	public Transform destination;

	// Start and en anle - 360 for single full rotation
	public float startAngle = 0;
	public float endAngle = 360;

	void Update () {

		Vector3 op = origin.position; op.y = 0;
		Vector3 dp = destination.position; dp.y = 0;
		// Projection of object on line between origin and destination
		var projection = Vector3.Project(transform.position - origin.position, op - dp) + op;
		// Distance to origin
		var dstOrigin = Vector3.Distance(op, projection);
		// Distance to destination
		var dstDestination = Vector3.Distance(dp, projection);
		// Parameter
		float t = (dstDestination + dstOrigin == 0 ? 0 : (dstOrigin / (dstDestination + dstOrigin) ) );
		// The angle at current t
		float angle = Mathf.Lerp(startAngle, endAngle, t);

		// Rotate the object by the angle, then make sure it also faces destination
		transform.rotation = Quaternion.LookRotation(dp - op) * Quaternion.Euler(angle, 0, 0);


	}

}
