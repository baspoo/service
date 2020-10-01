using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AspectRatio{
	Height,Width
}
public class NguiService : MonoBehaviour {

	#region Label
	public static void ChangeAndUpdateLabel(UILabel label){
	
	}
	public static string LabelColor( string message ,Color color){
		return "["+Service.Colour.ToRGBHex (color,false)+"]" +message + "[-]";
	}
	#endregion
	#region UITexture
	public static void ChangeAndUpdateTexture(UITexture uiTexture){
		
	}
	public static void UITextureFixSizeByKeepAspectRatio(UITexture uiTexture,AspectRatio aspectRatio){
		if (aspectRatio == AspectRatio.Height) {
			int Hight = uiTexture.height;
			uiTexture.keepAspectRatio = UIWidget.AspectRatioSource.Free;
			uiTexture.MakePixelPerfect ();

			uiTexture.keepAspectRatio = UIWidget.AspectRatioSource.BasedOnHeight;
			uiTexture.height = Hight;
		}
		if (aspectRatio == AspectRatio.Width) {
			int Width = uiTexture.width;
			uiTexture.keepAspectRatio = UIWidget.AspectRatioSource.Free;
			uiTexture.MakePixelPerfect ();

			uiTexture.keepAspectRatio = UIWidget.AspectRatioSource.BasedOnWidth;
			uiTexture.width = Width;
		}
	}
	//#if ngui
	public static void UITextureFixSize( UITexture image , int fixLength ,AspectRatio type ){
		if (image != null) {
			image.MakePixelPerfect ();
			if (type == AspectRatio.Height)
				SetTextureFixHight (image, fixLength);
			else
				SetTextureFixWidth (image, fixLength);
		}
	}
	public static void UITextureFixSize( UITexture image , int fixHight,int fixWidth ,AspectRatio firstfix ){
		if (image != null) {
			image.MakePixelPerfect ();
			if (firstfix == AspectRatio.Height) {
				SetTextureFixHight (image, fixHight);
				if(image.width>fixWidth)SetTextureFixWidth (image, fixWidth);

			} else {
				SetTextureFixWidth (image, fixWidth);
				if(image.height>fixHight)SetTextureFixHight (image, fixHight);
			}
		}
	}
	static void SetTextureFixHight( UITexture image , int fixHight  ){
		int my_width =  image.width;
		int my_height =  image.height;

		float changeper = (float)fixHight / (float)my_height;
		image.height = fixHight;
		image.width = (int)(my_width * changeper);
	}
	static void SetTextureFixWidth( UITexture image , int fixWidth  ){
		int my_width =  image.width;
		int my_height =  image.height;

		float changeper =  (float)fixWidth / (float)my_width;
		image.height = (int)(my_height * changeper);
		image.width = fixWidth; 
	}
	#endregion




	public class Var{
		#region Var-Transforms
		[System.Serializable]
		public class Tweens{
			[SerializeField]
			List<Tween>  _Tween = new List<Tween>();
			[System.Serializable]
			public class Tween{
				public string name;
				public UITweener tween;
				public Service.Callback.callback callback;
				public 	void OnFinish( ){
					if(callback!=null)
						callback ();
				}
			}
			public 	void Replay( string varName){
				UITweener t = Get (varName);
				if (t != null) {
					t.enabled = true;
					t.ResetToBeginning ();
					t.PlayForward ();
				}
			}
			public 	void PlayFoward( string varName, bool isForward = true){
				UITweener t = Get (varName);
				if (t != null) {
					t.enabled = true;
					t.Play (isForward);
				}
			}
			public 	void OnFinish( string varName , Service.Callback.callback callback){
				Tween tween = null;
				foreach (Tween t in _Tween) {
					if (t.name == varName)
						tween = t;
				}
				if (tween != null) {
					EventDelegate onFInish = new EventDelegate ( tween.OnFinish );
					tween.tween.AddOnFinished (  onFInish   );
				}
			}
			public 	List<UITweener> Gets( ){
				List<UITweener> tweens = new List<UITweener> ();
				foreach(Tween t in _Tween){
					tweens.Add (t.tween);
				}
				return tweens;
			}
			public UITweener Get(string varName){
				foreach (Tween t in _Tween) {
					if (t.name == varName)
						return t.tween;
				}
				return null;
			}
		}
		#endregion



		#region Var-UIWidgets
		[System.Serializable]
		public class UIWidgets{

			public enum UI{
				label,texture,sprite,button,unknow
			}

			[SerializeField]
			List<WidgetData>  widgetData = new List<WidgetData>();
			[System.Serializable]
			public class WidgetData{
				public string name;
				public UIWidget widget;
				public UI type;
			}
			public UIWidget Get(string varName){
				foreach (WidgetData t in widgetData) {
					if (t.name == varName)
						return t.widget;
				}
				return null;
			}
		}
		#endregion
	}

	[System.Serializable]
	public class Container
	{
		public Transform trans;
		public GameObject pObject;
		public UIGrid root;
		public List<T> Generate<T>( int count, Service.Callback.callbackgameobject caseout = null)
		{
			List<T> list = new List<T>();
			for (int i = 0; i < count; i++)
			{
				var g = Service.GameObj.Created(pObject, root.transform);
				list.Add(g.GetComponent<T>());
				if(caseout!=null)
					caseout(g);
			}
			root.repositionNow = true;
			return list;
		}
	}

}
