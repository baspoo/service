using UnityEngine;
using System.Collections;

public class SpriteAnimation_ : MonoBehaviour
{

		public SpriteRenderer SpriteRenderer_;
		public Sprite[] spr;
		public float speed;
		public bool loop;
		public bool onceDestory;
		public EventDelegate onEndAnimation;

		public void PlayStart ()
		{
			play = true;
			runtime = 0.0f;
			index = 0;
		}

		bool play = true;
		int index;
		float runtime = 0.0f;

		void Update ()
		{
				if (play) {
				if (runtime < 1.0f)
					runtime += speed * Time.deltaTime;
				else {
				if (index >= spr.Length)
				{
					if(loop)index=0;
					else {play = false; index--;}
					if(onceDestory)Destroy(gameObject);
					if(onEndAnimation!=null)onEndAnimation.Execute();
				} 
					if(SpriteRenderer_==null)gameObject.GetComponent<SpriteRenderer> ().sprite = spr [index];
					else SpriteRenderer_.sprite = spr [index];
					runtime = 0.0f;
					index++;
					
				}
				}
		}
}
