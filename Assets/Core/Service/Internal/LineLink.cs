using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineLink : MonoBehaviour{

	public LineRenderer linerender;
	public bool isActive;
	public List<Transform> targets;
	public void Set(List<Transform> targets)
	{
		this.targets = targets;
	}
	public void Update()
	{
		if (isActive) 
		{
			if (targets.Count > 0)
			{
				int i = 0;
				linerender.positionCount = targets.Count;
				foreach (Transform t in targets)
				{
					linerender.SetPosition(i, t.position);
					i++;
				}
			}
		}
	}



}
