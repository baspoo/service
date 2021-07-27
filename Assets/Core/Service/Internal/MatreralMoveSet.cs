using UnityEngine;
using System.Collections;


public enum MoveMatType
{
	x=0,
	y=1
}
public class MatreralMoveSet : MonoBehaviour {

	public MoveMatType MoveMatType_;
	public Material mat;
	public float speed = 2.0f;
	public bool move = false;
	float offset;
	float x=0.0f;
	float y=0.0f;

	public bool isRandom = false;
	public bool isOnce = false;
	Renderer render;

	void Start () {
		if(MoveMatType_ == MoveMatType.x)x=1.0f;
		if(MoveMatType_ == MoveMatType.y)y=1.0f;


		render = gameObject.GetComponent<Renderer>();


		if (mat == null) {
			mat = render.material;
		}


		if (isRandom) {
			speed = Random.Range (-speed, speed);
		}

	}
	void Update () {
		if(move){

			if(render!=null)
			if (render.material != mat)
				mat = render.material;

			offset += Time.deltaTime *speed;
			if(offset>=1.0f) { offset= offset-1.0f;  }
			mat.SetTextureOffset(GetOffset, new Vector2(offset*x, offset*y));

			if (isOnce) 
			{
				move = false;
			}

		}
	}


	


	public void Set (	float _x , float _y) {
		if (mat == null)
			mat = gameObject.GetComponent<Renderer> ().material;
		mat.SetTextureOffset(GetOffset, new Vector2(_x,_y));
	}


	public string OffsetKey;
	string GetOffset {

		get { return (string.IsNullOrEmpty(OffsetKey) ? "_MainTex" : OffsetKey); }
	}

}
