using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPadding : MonoBehaviour {

	public ScreenPaddingStyle screenPaddingStyle;

	public enum ScreenPaddingStyle { 
		LeftNRightPadding,
		TransformPadding
	}

	public List<CostumePadding> CostumePaddings = new List<CostumePadding>();
	[System.Serializable]
	public class CostumePadding {
		public ScreenSystem.Ratio.RatioType RatioType;
		public Vector3 Position;
		public Vector3 Rotate;
		public Vector3 Sacle;
	}



	void Start(){
		if(screenPaddingStyle == ScreenPaddingStyle.LeftNRightPadding) StartCoroutine (IELeftNRightPadding());
		if (screenPaddingStyle == ScreenPaddingStyle.TransformPadding) StartCoroutine(IETransformPadding());
	}








	IEnumerator IELeftNRightPadding(){
		yield return new WaitForEndOfFrame ();
		yield return new WaitForEndOfFrame ();
		yield return new WaitForEndOfFrame ();
		UIAnchor Anchor = GetComponent<UIAnchor> ();
		if(Anchor!=null)
		{
			Anchor.runOnlyOnce = true;
			yield return new WaitForEndOfFrame ();
			if(ScreenSystem.Ratio.GetRatio == ScreenSystem.Ratio.RatioType.phoneX)
				loopspadding (Anchor);
		}
		enabled = false;
	}
	UIAnchor.Side DIR( UIAnchor.Side side){
		switch(side){
		case UIAnchor.Side.Left:
		case UIAnchor.Side.TopLeft:
		case UIAnchor.Side.BottomLeft:
			return UIAnchor.Side.Left;
			break;
		case UIAnchor.Side.Right:
		case UIAnchor.Side.TopRight:
		case UIAnchor.Side.BottomRight:
			return UIAnchor.Side.Right;
			break;
		}
		return UIAnchor.Side.Center;
	}
	void loopspadding(UIAnchor Anchor){
		int x = 0;
		foreach(UIAnchor anc in GetComponentsInParent<UIAnchor> ()){
			if (x == 0) 
			{
				//** Self Padding
				padding (Anchor.side);
			} 
			else 
			{
				//** In Parent Re-Padding
				if (DIR (anc.side) == UIAnchor.Side.Left)
					padding (UIAnchor.Side.Right);
				if (DIR (anc.side) == UIAnchor.Side.Right)
					padding (UIAnchor.Side.Left);
			}
			x++;
		}
	}
	void padding(UIAnchor.Side side){
		if (DIR (side) == UIAnchor.Side.Left) 
		{
			transform.localPosition = new Vector3 ( transform.localPosition.x + ScreenSystem.Setting.left_padding , transform.localPosition.y , transform.localPosition .z );
		}
		if (DIR (side) == UIAnchor.Side.Right) 
		{
			transform.localPosition = new Vector3 ( transform.localPosition.x + ScreenSystem.Setting.right_padding , transform.localPosition.y , transform.localPosition .z );
		}
	}





	IEnumerator IETransformPadding()
	{
		var Padding = CostumePaddings.Find(x=>x.RatioType == ScreenSystem.Ratio.GetRatio);
		if (Padding != null) {
			transform.localPosition = Padding.Position;
			transform.localRotation = Quaternion.Euler(Padding.Rotate);
			transform.localScale = Padding.Sacle;
		}
		yield return new WaitForEndOfFrame();
	}


}
