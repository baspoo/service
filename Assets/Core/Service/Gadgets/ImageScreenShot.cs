//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using Facebook;
//
//
//public class ImageScreenShot : MonoBehaviour {
//	void Start () {
//
//
//
////	 SizeWidth
////	 SizeHeight
//
//
//		float H = (float)Screen.height / 1136.0f ;
//		SizeWidth = (int) (SizeWidth * H);
//		SizeHeight = (int) (SizeHeight * H);
//
//		#if UNITY_IOS
//			IOSCamera.instance.OnImageSaved += OnImageSaved;
//		#endif
//		#if UNITY_ANDROID
//			AndroidCamera.instance.OnImageSaved += OnImageSaved;
//		#endif
//
//		#region FIND_DIR_destinationPath
//		destinationPath = Application.persistentDataPath + Path.DirectorySeparatorChar +"ImageCapture";
//			if (!System.IO.Directory.Exists (destinationPath)) {
//				System.IO.Directory.CreateDirectory (destinationPath);
//			}
//		destinationPath = destinationPath + Path.DirectorySeparatorChar ;
//		#endregion
//	}
//
//
//	#region Call-Back_SaveTo_Camera_Roll
//	private void OnImageSaved (ISN_Result result) {
//		IOSCamera.instance.OnImageSaved -= OnImageSaved;
//		if(result.IsSucceeded) {
//			//IOSMessage.Create("Success", "Image Successfully saved to Camera Roll");
//		} else {
//			//IOSMessage.Create("Success", "Image Save Failed");
//		}
//	}
//	void OnImageSaved (GallerySaveResult result) {
//		AndroidCamera.instance.OnImageSaved -= OnImageSaved;
//		if(result.IsSucceeded) {
//			//Debug.Log("Image saved to gallery \n" + "Path: " + result.imagePath);
//		} else {
//			//Debug.Log("Image save to gallery failed");
//		}
//	}
//	#endregion
//
//
//
//
//
//
//
//
//
//	#region Take1ShotCamera
//	List <takeshotCamera> myTakeObj = new List<takeshotCamera>();
//	bool initSnap = false;
//	public bool StartTakeImageFocus( )
//	{
//		bool isHaveCat = false;
//		initSnap = true;
//
//		Debug.Log("CatList:"+gamemanager.gm._Catlist.Count);
//		myTakeObj.Clear();
//		foreach( CatObj cat in gamemanager.gm._Catlist )
//		{
//			Debug.Log("Cat:"+cat.name);
//			takeshotCamera tc = gamemanager.gm._loadobjMg.Created_CameraTake (cat.transform,this,cat).GetComponent<takeshotCamera>();
//			myTakeObj.Add(tc);
//			isHaveCat = true;
//		}
//		Debug.Log("StartTakeImageFocus!");
//		return isHaveCat;
//	}
//	public void TakeDown(takeshotCamera takeobj)
//	{
//		isTaking = true;
//		foreach( takeshotCamera take in myTakeObj )
//		{
//			take._closeframe();
//			if (take != takeobj) Destroy(take.gameObject);
//		}
//	}
//	public void TakeUp(takeshotCamera takeobj)
//	{
//		StartCoroutine (take (takeobj._cat));
//		Destroy (takeobj.gameObject);
//	}
//	IEnumerator take( CatObj  cat)
//	{
//		yield return new WaitForEndOfFrame ();
//		StartCoroutine(ScreenshotEncode(cat));
//	}
//	void moveframe()
//	{
//		Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
////		Debug.Log(mousePosition.ToString());
//		if(mousePosition.x < (SizeWidth/2))
//		{
//			pointA = (int) ( 0) ;
//			pointB = (int)mousePosition.y-(SizeHeight/2);
//		}
//		else
//			if(mousePosition.x > ( Screen.width  - (SizeWidth/2)))
//		{
//			pointA = (int) ( Screen.width - (SizeWidth)) ; 
//			pointB = (int)mousePosition.y-(SizeHeight/2);
//		}
//		else
//		{
//			pointA = (int)mousePosition.x-(SizeWidth/2);
//			pointB = (int)mousePosition.y-(SizeHeight/2);
//		}
//	}
//	public UITexture image_load;
//	void Update () {
//		if(initSnap)
//			if(Input.GetMouseButton(0)) 	
//				moveframe();
//	}
//	#endregion
//	
//
//
//
//
//
//
//
//
//
//
//
//	#region FunctionCaptureImage
//	public void ScreenShot_ApplicationCapture()
//	{
//		string name = destinationPath+"ApplicationCapture.png";
//		#if UNITY_EDITOR
//		Application.CaptureScreenshot(name);
//		#endif
//		#if UNITY_IOS
//			IOSCamera.instance.SaveScreenshotToCameraRoll();
//		#endif
//		#if UNITY_ANDROID
//			AndroidCamera.instance.SaveScreenshotToGallery();
//		#endif
//	}
//	public void ScreenShot_ApplicationCapture_return()
//	{
//		StartCoroutine(ScreenshoView());
//	}
//	public void ScreenShot_SaveGallry(Texture2D _image)
//	{
//		#if UNITY_EDITOR
//			byte[] bytes = _image.EncodeToPNG();
//			string Path = destinationPath+"ImageToCameraRoll.png"; 
//		System.IO.File.WriteAllBytes( Path , bytes);
//		#endif
//		#if UNITY_IOS
//			IOSCamera.instance.SaveTextureToCameraRoll(_image);
//		#endif
//		#if UNITY_ANDROID
//			AndroidCamera.instance.SaveImageToGalalry(_image);
//		#endif
//	}
//	public void LoadImage_inUiTexture(UITexture _image_load,string name)
//	{
//		string path =  destinationPath+name;
////		Debug.Log(path);
//		StartCoroutine(WaitForRequest(path,_image_load));
//	}
//	IEnumerator WaitForRequest(string path , UITexture _image_load  ) {
//		WWW www = new WWW("file://"+path); 
//		yield return www;
//		if (www.error == null) {
//			if(www.texture != null)
//				_image_load.mainTexture =   www.texture;
//		}    
//	}
//	public  void UpBestShotImage(  string name , EventDelegate onFinish){
//		StartCoroutine(_upbestimage(name,onFinish));
//	}
//	public static Texture2D image_loader;
//	IEnumerator _upbestimage(     string name ,EventDelegate onFinish ) {
//		string path =  destinationPath + name;
//		WWW www = new WWW("file://"+path); 
//		yield return www;
//		if (www.error == null) {
//			if(www.texture != null)
//				image_loader = www.texture;
//				onFinish.Execute();
//		}    
//	}
//
//
//
////	public  void vdoload( ){	
////		StartCoroutine(_vdoloader());
////	}
////	IEnumerator _vdoloader(    ) {
////		OpenPage_.op.NotClickAll(true);
////		WWW www = new WWW("http://junk.onemoby.com/baspoo/log/vdo.mp4"); 
////		yield return www;
////		if (www.error == null) {
////			byte[] bytes =  www.bytes;
////			string path = Application.streamingAssetsPath+Path.DirectorySeparatorChar;
////			System.IO.File.WriteAllBytes( destinationPath + "vdoGG.mp4", bytes);
////		}    
////		OpenPage_.op.NotClickAll(false);
////
////	}
////	public  void playerVdo( ){
//////		Handheld.PlayFullScreenMovie ("file://"+destinationPath+"vdoGG.mp4", Color.black, FullScreenMovieControlMode.Full,FullScreenMovieScalingMode.AspectFit);
////	}
////
//
//
//
//
//
//
//
//	public static string  destinationPath;
//	//public GameObject target;
//	public int SizeWidth;
//	public int SizeHeight;
//	public int pointA;
//	public int pointB;
//	IEnumerator ScreenshotEncode( CatObj  cat  )
//	{
//		isTaking = false;
//		yield return new WaitForEndOfFrame();
//		Texture2D texture = new Texture2D(SizeWidth,SizeHeight, TextureFormat.ARGB32, false);
//		texture.ReadPixels(new Rect(pointA, pointB, SizeWidth, SizeHeight),0, 0);
//		texture.Apply();
//		yield return 0;
//		if(OpenPage_.op.page!=null)
//			if(OpenPage_.op.page.GetComponent<_CameraPage>()!=null)
//				OpenPage_.op.page.GetComponent<_CameraPage>().OpenPhotoTake_savetoCatBook(texture, cat);
//	}
//	IEnumerator ScreenshoView( )
//	{
//		isTaking = false;
//		yield return new WaitForEndOfFrame();
//		Texture2D texture = new Texture2D(Screen.width,Screen.height, TextureFormat.ARGB32, false);
//		texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height),0, 0);
//		texture.Apply();
//		yield return 0;
//		if(OpenPage_.op.page!=null)
//			if(OpenPage_.op.page.GetComponent<_CameraPage>()!=null)
//				OpenPage_.op.page.GetComponent<_CameraPage>().OpenPhotoTake_savetoCameraRoll(texture);
//	}
//
//	public void SaveFile(string name , Texture2D image)
//	{ 
//		byte[] bytes = image.EncodeToPNG();
//		System.IO.File.WriteAllBytes( destinationPath + name, bytes);
//
//	}
//
//	bool isTaking = false;
//	public Texture aTexture;
//	void OnGUI() {
//		if(isTaking){
//			float newpoint =   Screen.height - pointB;
//			GUI.DrawTexture(new Rect(pointA, newpoint, SizeWidth, -SizeHeight), aTexture, ScaleMode.StretchToFill, true, 0.0F);
//		}
//	}
//	_CameraPage _cp;
//	public void PostScreenshot_UptoFacebook(Texture2D snap,string _FbMessage,_CameraPage cp)
//	{
//		_cp = cp;
//		_cp._Fb_FarmeImage.mainTexture = snap;
//		FbMessage = _FbMessage;
//		StartCoroutine (ScreenshotFarme());
//	}
//	public int exW;
//	public int exH;
//	public UITexture Exsame;
//	IEnumerator ScreenshotFarme()
//	{
//		_cp._Fb_Farme.SetActive (true);
//		yield return new WaitForEndOfFrame();
//		int exWx=exW;
//		int exHx=exH;
//		float H = (float)Screen.height / 1136.0f ;
//		exWx = (int) (exWx * H);
//		exHx = (int) (exHx * H);
//		int exA = (int) (Screen.width / 2)-(exWx/2);
//		int exB = (int) (Screen.height / 2)-(exHx/2);
//		yield return new WaitForEndOfFrame();
//		Texture2D texture = new Texture2D(exWx,exHx, TextureFormat.ARGB32, false);
//		texture.ReadPixels(new Rect(exA, exB, exWx, exHx),0, 0);
//		texture.Apply();
//		yield return new WaitForEndOfFrame();
//		_cp._Fb_Farme.SetActive (false);
//		OpenPage_.op.NotClickAll(true);
//		if(FacebookManager.IsLoggedIn) StartCoroutine(TakeScreenshot_UptoFacebook(texture));
//		else
//		{
//			_snap = texture;
//			FacebookManager.didLogin += didLogin;
//			FacebookManager.didFailToLogin += didFailToLogin;
//			FacebookManager.Login();
//		}
//		yield return 0;
//	}
//	public void PostScreenshot_UptoFacebookView(Texture2D snap,string _FbMessage,_CameraPage cp)
//	{
//		_cp = cp;
//		FbMessage = _FbMessage;
//		if(FacebookManager.IsLoggedIn) StartCoroutine(TakeScreenshot_UptoFacebook(snap));
//		else
//		{
//			_snap = snap;
//			FacebookManager.didLogin += didLogin;
//			FacebookManager.didFailToLogin += didFailToLogin;
//			FacebookManager.Login();
//		}
//	}
//
//
//
//
//
//
//
//	Texture2D _snap;
//	void didLogin()
//	{
//		StartCoroutine(TakeScreenshot_UptoFacebook(_snap));
//	}
//	void didFailToLogin()
//	{
//		OpenPage_.op.NotClickAll(false);
//		Debug.LogError ("didFailToLogin!");
//	}
//	string FbMessage;
//	IEnumerator TakeScreenshot_UptoFacebook( Texture2D snap  )
//	{
//
//
//
////		string FbMessage = "Your friend found a new cat. Find one in your backyard in The Cats Paradise. Available on App Store and Google Play.";
////		FbMessage+= "\nIOS : https://itunes.apple.com/us/app/the-cats-paradise/id1012885971";
////		FbMessage+= "\nAndroid : https://play.google.com/store/apps/details?id=com.onemoby.neko";
//		OpenPage_.op.NotClickAll (true);
//		yield return new WaitForEndOfFrame();
//		yield return 0;
//		byte[] screenshot = snap.EncodeToPNG();
//		WWWForm wwwForm = new WWWForm();
//		wwwForm.AddField("message",FbMessage);
//		wwwForm.AddBinaryData("image", screenshot, "barcrawling.png");
//		FB.API("me/photos", Facebook.HttpMethod.POST, TestCallBack, wwwForm);
//	}
//	private void TestCallBack (FBResult result)
//	{
//		OpenPage_.op.NotClickAll(false);
//		if (result.Error == null) 
//		{
//			Debug.Log ("Post Finish");
//			Debug.Log (result.Text);
//			_cp._PostOnFacebook_CallBack_Ok();
//		} 
//
//		else 
//		{
//			Debug.Log (result.Error);
//		}
//	}
//
//	#endregion
//
//
//
//
//
//
//
//
//
//
//
//
//	
//	//	public void UpdateDataCatPlayer(string CatId,Texture2D snap )
//	//	{
//	//		bool isTreasure = true;
//	//		postimagetoserver(CatId,snap,isTreasure);
//	//	}
//	//	void postimagetoserver (string CatId ,Texture2D snap ,bool isTreasure) {
//	//		byte[] screenshot = snap.EncodeToPNG();
//	//		string result ="";
//	//		if(snap!=null)result = System.Convert.ToBase64String(screenshot);
//	//		string str = 
//	//			FB.UserId+"|"+
//	//			CatId+"|"+
//	//			isTreasure.ToString()+"|"+
//	//			result;
//	//		gamemanager.gm._Postserver.Post(str);
//	//	}
//	
//	public static string Get_Base64StrImage(Texture2D image)
//	{
//		byte[] screenshot = image.EncodeToPNG();
//		string result = System.Convert.ToBase64String(screenshot);
//		//Debug.Log("result:"+result);
//		return result;
//		//return "";
//	}
//	
//	//	public	void postimagetoserver2 (Texture2D snap) {
//	//				string url = "http://junk.onemoby.com/chad/binary_img/convert.php";
//	//				byte[] screenshot = snap.EncodeToPNG();
//	//				string result2 = System.Convert.ToBase64String(screenshot);
//	//				WWWForm form = new WWWForm();
//	//				form.AddField("imgbinary1",result2);
//	//				WWW www = new WWW(url, form);
//	//				StartCoroutine(WaitForRequest(www));
//	//	}
//
//
//
//
//
//
//
//	//
//	//
//	//	public string FBID;
//	//	public string CatID;
//	//	public void POSTTEST ( Texture2D snap  ) {
//	//		byte[] screenshot = snap.EncodeToPNG();
//	//		string result ="";
//	//		bool isTreasure = true;
//	//		if(snap!=null)result = System.Convert.ToBase64String(screenshot);
//	//		string str = 
//	//		FBID+"|"+
//	//		CatID+"|"+
//	//		isTreasure.ToString()+"|"+
//	//		result;
//	//		gamemanager.gm._Postserver.Post(str);
//	//	}
//	
//	
//
//	
//	IEnumerator WaitForRequest(WWW www)
//	{
//		yield return www;
//			// check for errors
//			if (www.error == null)
//		{
//			Debug.Log("WWW Ok!: " + www.data);
//		} else {
//			Debug.Log("WWW Error: "+ www.error);
//		}    
//	}    
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//	Texture2D atlas;
//	int textureWidthCounter = 0;
//	int width,height;
//	public void Imageatlas (string name,List<Texture2D> textures ) {
//		// determine your size from sprites
//		width = 0;
//		height = 0;
//		foreach(Texture2D t in textures)
//		{
//			width += t.width;
//			// determine the height
//			if(t.height > height)height = t.height;
//		}
//		
//		// make your new texture
//		atlas = new Texture2D(width,height,TextureFormat.RGBA32,false);
//		// loop through your textures
//		for(int i= 0; i<textures.Count; i++)
//		{
//			int y = 0;
//			while (y < atlas.height) {
//				int x = 0;
//				while (x < textures[i].width ){
//					if(y < textures[i].height){
//						// fill your texture
//						atlas.SetPixel(x + textureWidthCounter, y, textures[i].GetPixel(x,y));
//					}
//					else {
//						// add transparency
//						atlas.SetPixel(x + textureWidthCounter, y,new Color(0f,0f,0f,0f));
//					}
//					++x;
//				}
//				++y;
//			}
//			atlas.Apply();
//			textureWidthCounter +=  textures[i].width;
//		}
//		byte[] bytes = atlas.EncodeToPNG();
//		System.IO.File.WriteAllBytes( destinationPath + name, bytes);
//	}
//	
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//}
