   
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureGray : MonoBehaviour
{
	static List<Texture2D> Textures = new List<Texture2D> ();

	//** Find In Stock
	static Texture2D Find  ( string nameFile ){
		foreach(Texture2D img in Textures){
			if (img.name == nameFile)
				return img;
		}
		return null;
	}

	//** Clear All In Stock
	static void Clear  (   ){
		foreach(Texture2D img in new ArrayList(Textures)){
			Destroy (img);
		}
		Textures.Clear ();
	}


	public static Texture2D ToGray (Texture texture)
	{
		//** Find In Stock
		// If Have Use Old File ** Not Created New Same File .
		Texture2D tex2D = Find (texture.name);
		if (tex2D != null)
			return tex2D;

		//** Find Not Found File New TextureGray. 
		// Should be moved out of HBallManager since it's also used for Surface Textures
		tex2D = (Texture2D)texture;
		Texture2D grayTex = Service.Image.ToGray (tex2D);

		//** Add New File To List
		grayTex.name = texture.name;
		Textures.Add (grayTex);
		return grayTex;
	}


}
  