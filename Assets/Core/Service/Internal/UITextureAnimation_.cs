using UnityEngine;
using System.Collections;

public class UITextureAnimation_ : MonoBehaviour
{

	public UITexture uitexture;
	public Texture[] textures;
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
				if (index >= textures.Length) {
					if (loop)
						index = 0;
					else {
						play = false;
						index--;
					}
					if (onceDestory)
						Destroy (gameObject);
					if (onEndAnimation != null)
						onEndAnimation.Execute ();
				} 
				if (uitexture == null)
					gameObject.GetComponent<UITexture> ().mainTexture = textures [index];
				else
					uitexture.mainTexture = textures [index];
				runtime = 0.0f;
				index++;
					
			}
		}
	}
}
