using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHex : MonoBehaviour
{
	void Start(){
		Reposition ();
	}
	void Update(){
		if(isRunUpdate)
			Reposition ();
	}



	public float Side; 			// 200
	public float HightSide; 	// 100
	public float HightDown;		// 200
	public bool isRunUpdate;


	public void Reposition ()
    {
		int i = 0;
		foreach( GameObject g in Service.GameObj.GetAllParent (transform) )
		{
			Vector3 position = Vector3.zero;
			if (i % 2 == 0) 
			{
				position.y = -HightDown * i / 2;
			} 
			else 
			{
				position.x = Side;
				position.y = -HightSide * (i);
			}
			i++;
			g.transform.localPosition = position;
		}
    }

}
