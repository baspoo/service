using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FollowTransform : MonoBehaviour {


	public DestinationObject Original;
	public List<DestinationObject> Destination;
    public Vector3 DefaultChange = Vector3.zero;

    [Serializable]
	public class DestinationObject
	{
		public Transform transform;
		public TweenPosition tween;
		public bool isCheckTween;
		public Vector3 position = Vector3.zero;

        public DestinationObject(Transform destination,Vector3 position, bool isCheckTween = false, TweenPosition tween = null)
        {
            this.transform = destination;
            this.tween = tween;
            this.isCheckTween = isCheckTween;
            this.position = position;
        }

    }

   

    Vector3 position_original;

	void Start(){
		if (Original.transform == null) {
			Original.transform = gameObject.transform;
			Original.position = gameObject.transform.position;
		}

		position_original = Original.position;

		foreach (DestinationObject obj in Destination) {
			if (obj.isCheckTween) {
				if(obj.tween == null)
					obj.tween = obj.transform.gameObject.GetComponent<TweenPosition> ();
				if(obj.tween!=null){
					obj.tween.AddOnFinished (() => {
						//Debug.Log("obj.tween.direction "+obj.tween.direction.ToString());
						if(obj.tween.direction == AnimationOrTween.Direction.Forward){
							changePosition (obj);
						}
						else
							changePosition ();
					});
				}
			}
		}
	}

	DestinationObject m_focus;
	bool isUp = false;
	void Update(){
		if (Original.transform.gameObject.activeSelf) {
			bool isClose = false;
			foreach (DestinationObject obj in Destination) {
				//Debug.Log(obj.transform.name + " activeInHierarchy " + obj.transform.gameObject.activeInHierarchy.ToString());
				if (obj.isCheckTween) {
					if (m_focus == obj && !obj.transform.gameObject.activeInHierarchy)
						isClose = true;

				}
				else if (obj.transform.gameObject.activeInHierarchy && !isUp) {
					isClose = false;
					changePosition (obj);
				}

			}
			if (isClose) {
				changePosition();
			}

		}
	
			

	}

	void changePosition(DestinationObject obj = null){
		if (obj!=null) {
			m_focus = obj;
		//	Debug.Log ("changePosition " + obj.transform.name);
			Original.transform.localPosition = obj.position;
			isUp = true;
		} else {
		//	Debug.Log ("changePosition ORIGINAL" );
			Original.transform.localPosition = position_original;
			isUp = false;
		}
			
			
	}

    public bool isReferencedAll(){
        bool isHave = Destination.Count > 0;
        foreach (DestinationObject obj in Destination){
            if (obj.transform == null)
                isHave = false;
        }
        return isHave;
    }

    public void ReferenceByList(List<GameObject> gameObjects){
        for (int i = 0; i < gameObjects.Count && i < Destination.Count;i++){
            Destination[i].transform = gameObjects[i].transform;
        }

    }

}
