using UnityEngine;
using System.Collections;

public class Line_linker : MonoBehaviour {

	public enum LineTyle { 
		normal, projectile
	}



	public LineTyle lineTyle = LineTyle.normal;
	public Transform target;
	public Transform[] targets;
	public float Flcating =0.0f;
	public LineRenderer line;
	void Start(){
		if(line==null)line = gameObject.GetComponent<LineRenderer> ();
	}
	void Update () {


		if (lineTyle == LineTyle.normal) 
		{

			if (target != null)
			{
				Vector3 postTarget = target.position;
				line.SetPosition(0, transform.position);
				for (int n = 2; n <= 5; n++)
				{
					Vector3 post1 = transform.position + (postTarget - transform.position) * (n - 1) / 6;
					post1.x += Random.Range(-Flcating, Flcating);
					post1.y += Random.Range(-Flcating, Flcating);
					line.SetPosition(n - 1, post1);
				}
				line.SetPosition(5, postTarget);
			}
			else
			{
				if (targets.Length > 0)
				{
					int i = 0;
					foreach (Transform t in targets)
					{
						line.SetPosition(i, t.position);
						i++;
					}
				}
			}

		}
		if (lineTyle == LineTyle.projectile) 
		{
			updateLine();
		}
	}











	Vector3 ParametricPos(float progress, float hight, Vector3 origin, Vector3 destination)
	{
		Vector3 pos = Vector3.Lerp(origin, destination, progress);
		pos.y = 4 * (progress - progress * progress) * hight + progress * destination.y + (1 - progress) * origin.y;
		return pos;
	}
	void updateLine()
	{
		for (int x = 0; x < line.positionCount; x++)
		{
			Vector3 Position = ParametricPos((float)x / (float)line.positionCount, Flcating , transform.position, target.position);
			line.SetPosition(x, Position);
		}
	}





}
