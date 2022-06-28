using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOption : MonoBehaviour{
	void Start()
	{
		if (link.isActive)link.Start ();
	}
	void Update()
	{
		if (link.isActive)link.Update ();
	}


















	LineLink link = new LineLink();
	public class LineLink
	{
		public LineRenderer linerender;
		public bool isActive;
		public Transform[] targets;

		public void Start()
		{

		}
		public void Update()
		{
			if (targets.Length > 0) 
			{
				int i = 0;
				foreach (Transform t in targets) {
					linerender.SetPosition (i, t.position);
					i++;
				}
			}
		}
	}










}
