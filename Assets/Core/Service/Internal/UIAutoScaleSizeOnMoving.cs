using UnityEngine;
using System.Collections;

public class UIAutoScaleSizeOnMoving : MonoBehaviour {
	public float SpeedScale;
	public Vector3 origenScale;
	public bool X;
	public bool Y;
	public bool Z;


	bool isReady = false;
	void Start () {
		if(origenScale==Vector3.zero)origenScale = transform.localScale; 
		isReady = true;
	}
	void Update () {
		if (isReady) {

			float Dir = 0.0f;
			if(X)Dir = transform.position.x;
			else
				if(Y)Dir = transform.position.y;
				else
					if(Z)Dir = transform.position.z;
			
			float Persen = (1.0f - (Mathf.Abs (Dir) * SpeedScale ));
			if (Persen >= 0.0f) {
				Vector3 NewScale = Persen*origenScale;
				transform.localScale = NewScale;
			}
		}
	}
}
