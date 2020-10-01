using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledOffset : MonoBehaviour
{
	UITexture img;
	Transform currentposition;
	int x,y,max;
	int perfect_max;

	int m_current;
	public int GetCurrent
	{
		get
		{ 
			return m_current;
		}
	}
	public void Init( int max = 0 , int perfect_max = 0)
	{
		img = GetComponent<UITexture> ();
		if (img == null) {
			Debug.LogError ("TiledOffset : Not Find UITexture!");
			return;
		}
		this.max = max;
		this.perfect_max = perfect_max;
		x = img.width;
		y = img.height;

		//** Init perfect Scale **
		// ((33.33*5) - (33.33*3))
		if (perfect_max != 0 && max != perfect_max && max> perfect_max)
		{

			var div = (100.0f / perfect_max);
			var size = ((div * max) - (div * perfect_max)) / 100.0f;

			Debug.LogError("----------------------------------- div:" + div + " size:"+ size);

			var scale = img.transform.localScale;
			scale.x = size;
			img.transform.localScale = scale;

		}


		if (currentposition == null) {
			currentposition = new GameObject ("position").transform;
			currentposition.parent = transform;
			currentposition.localScale = Vector3.one;
		}


		if(max!=0)
		if (img.pivot == UIWidget.Pivot.Center) 
		{
			img.pivot = UIWidget.Pivot.Left;
			Vector3 position = img.transform.transform.localPosition;
			position.x = ((max * x) / 2.0f)*-1.0f;
			img.transform.localPosition = position;
		}






	}
	public Vector3 GetPosition( int current ){
		if (img == null)Init ();
		if (img == null)return Vector3.zero;
		if(max!=0)if(current > max) current = max;

		Vector3 position = Vector3.zero;
		position.x =  x*current   - (int)((x/2));
		return position;
	}
	public Transform GetTransform( int current){
		if (img == null)Init ();
		if (img == null)return null;
		if(max!=0)if(current > max) current = max;

		currentposition.localPosition = GetPosition(current);
		return currentposition.transform;
	}
	public void RefreshTiled( int current){
		if (img == null)Init ();
		if (img == null)return;
		if(max!=0)if(current > max) current = max;
		img.width = x * current;
		m_current = current;
		GetTransform (current);
	}

}
