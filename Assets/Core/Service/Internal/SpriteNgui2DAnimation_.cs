using UnityEngine;
using System.Collections;

public class SpriteNgui2DAnimation_ : MonoBehaviour
{
		public UI2DSprite SpriteRenderer_;
		public Sprite[] spr;
		public float speed;
		public bool loop;
		public bool NotMakePixelPerfect;

		public bool onceDestory;
		public EventDelegate onEndAnimation;

		public void PlayStart ()
		{
			play = true;
			runtime = 0.0f;
			index = 0;
		}

		public bool play = true;
		int index;
		float runtime = 0.0f;
		void Update ()
		{
			if (spr.Length == 0)
				return;
		
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
					if(SpriteRenderer_==null)gameObject.GetComponent<UI2DSprite> ().sprite2D = spr [index];
					else SpriteRenderer_.sprite2D = spr [index];
					if(!NotMakePixelPerfect)SpriteRenderer_.MakePixelPerfect ();
					runtime = 0.0f;
					index++;
					
				}
				}
		}
}
