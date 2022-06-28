using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineLink : MonoBehaviour{

	public LineRenderer linerender;
	public bool isActive;
	public List<Transform> targets;
	List<Vector3> positions = new List<Vector3>();
	public void Set(List<Vector3> positions)
	{
		this.positions = positions;
	}
	public void Set(List<Transform> targets)
	{
		this.targets = targets;
		this.positions.Clear();
		foreach (Transform t in this.targets)
			this.positions.Add(t.position);
	}
	public void SetCircle(int size, float radius)
	{
		this.positions = GetCircle( size, radius , transform );
	}
	public void Update()
	{
		if (isActive) 
		{

			if (targets.Count != 0)
				Set(targets);

			if (positions.Count > 0)
			{
				int i = 0;
				linerender.positionCount = positions.Count;
				foreach (var pos in positions)
				{
					linerender.SetPosition(i, pos );
					i++;
				}
			}
		}
	}





	//Function คำนวนวงกลม
	public static List<Vector3> GetCircle(int Size, float radius , Transform origin = null )
	{
		Vector3 origin_position = (origin==null)? Vector3.zero : origin.position;
		List<Vector3> position = new List<Vector3>();
		float Theta = 0.0f;
		float ThetaScale = 1.0f / (float)Size;
		for (int i = 0; i < Size; i++)
		{
			Theta += (2.0f * Mathf.PI * ThetaScale);
			float x = radius * Mathf.Cos(Theta);
			float y = radius * Mathf.Sin(Theta);
			position.Add(new Vector3( origin_position.x + x , origin_position.y + y, origin_position.z ));
		}
		return position;
	}

}
