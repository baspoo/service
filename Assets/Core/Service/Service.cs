using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


/// <summary>
///    ** Game Master Service by 'Baspoo'
/// </summary>
public class Service : MonoBehaviour {
	public const string head_service = "BP-Service";
	public static void Clear(){
		//Clear
		if(Tools.gameservice!=null)
		foreach (Component comp in Tools.gameservice.GetComponents<Component>()){
			if (!(comp is Transform)){
				Destroy(comp);
			}
		}
	}


	public class Callback{
		public delegate void callback();
		public delegate void callbackImg( Texture2D img );
		public delegate void callbackobject( object obj);
		public delegate void callbackgameobject(GameObject gameobj);
		public delegate void callback_value( int value );
		public delegate void callback_fvalue( float value );
		public delegate void callback_bool( bool boo );
		public delegate void callback_data( string data );
		public delegate void callback_formula( Service.Formula f );

		public delegate int callback_value_return( int value );
        public delegate double callback_double_return(int value);

        public delegate string callback_data_return( string data );

	}


	public class Var{

		#region Var-Transforms
		[System.Serializable]
		public class Transforms{
			[SerializeField]
			List<Trans>  _Transforms = new List<Trans>();
			[System.Serializable]
			public class Trans{
				public string name;
				public Transform transform;
			}
			public void Add(string varName , Transform tran){
				Trans t = new Trans ();
				t.name = varName;
				t.transform = tran;
				_Transforms.Add (t);
			}
			public void Remove(string varName, bool isDestoryObjectInTrans = false){
				Trans find = null;
				foreach (Trans t in _Transforms) {
					if (t.name == varName)
						find = t;
				}
				if (find != null) {
					_Transforms.Remove (find);
					if (isDestoryObjectInTrans)
						Destroy (find.transform.gameObject);
				}
			}
			public void RemoveAll(bool isDestoryObjectInTrans = false){
				if(isDestoryObjectInTrans)
					foreach (Service.Var.Transforms.Trans t in _Transforms)
						Destroy (t.transform.gameObject);
				_Transforms.Clear ();
			}
			public Transform Get(string varName){
				foreach (Trans t in _Transforms) {
					if (t.name == varName) {
						return t.transform;
					}
				}
				return null;
			}
			public List<Trans> Gets{
				get{ return _Transforms;}
			}
			public Transform Active(string varName , bool isActive){
				foreach (Trans t in _Transforms) {
					if (t.name == varName) {
						t.transform.gameObject.SetActive (isActive);
						return t.transform;
					}
				}
				return null;
			}
		}
		#endregion
		#region Var-Values
		[System.Serializable]
		public class Values{
			[SerializeField]
			List<Value>  _Values = new List<Value>();
			[System.Serializable]
			public class Value{
				public string name;
				public double value;
			}
			public List<Value> ValueList{
				get { return _Values;}
			}
			public void Add(string varName,double value){
				Value newValue = new Value();
				newValue.name = varName;
				newValue.value = value;
				_Values.Add (newValue);
			}
			public double Get(string varName){
				foreach (Value t in _Values) {
					if (t.name == varName)
						return t.value;
				}
				return 0;
			}
			public void Set(string varName,double newValue){
				foreach (Value t in _Values) {
					if (t.name == varName)
						t.value = newValue;
				}
			}
		}
		#endregion
		#region Var-Texture
		[System.Serializable]
		public class Textures{
			[SerializeField]
			List<TextureData>  _Texture = new List<TextureData>();
			[System.Serializable]
			public class TextureData{
				public string name;
				public Texture img;
			}
			public void Add(string varName , Texture texture)
			{
				_Texture.Add (  new TextureData(){name = varName, img = texture  } );
			}
			public Texture Get(string varName){
				foreach (TextureData t in _Texture) {
					if (t.name == varName)
						return t.img;
				}
				return null;
			}
		}
		#endregion
		#region Var-String
		[System.Serializable]
		public class Strings{
			[SerializeField]
			List<String>  _Strings = new List<String>();
			[System.Serializable]
			public class String{
				public string name;
				public string text;
			}
			public string Get(string varName){
				foreach (String t in _Strings) {
					if (t.name == varName)
						return t.text;
				}
				return string.Empty;
			}
		}
		#endregion
		#region Var-Color
		[System.Serializable]
		public class Colors{
			[SerializeField]
			List<_Color>  _Colors = new List<_Color>();
			[System.Serializable]
			public class _Color{
				public string name;
				public Color color;
			}
			public Color Get(string varName){
				foreach (_Color t in _Colors) {
					if (t.name == varName)
						return t.color;
				}
				return Color.white;
			}
		}
		#endregion
		#region Var-Behaviour
		[System.Serializable]
		public class Behaviours{
			[SerializeField]
			List<behaviour>  _Behaviours = new List<behaviour>();
			[System.Serializable]
			public class behaviour{
				public string name;
				public Behaviour Class;
			}
			public Behaviour Get(string varName){
				foreach (behaviour t in _Behaviours) {
					if (t.name == varName)
						return t.Class;
				}
				return null;
			}
		}
		#endregion
		#region Var-ParticleSystems
		[System.Serializable]
		public class ParticleSystems{
			[SerializeField]
			List<particleSystem>  _ParticleSystems = new List<particleSystem>();
			[System.Serializable]
			public class particleSystem{
				public string name;
				public ParticleSystem particle;
			}
			public ParticleSystem Get(string varName){
				foreach (particleSystem t in _ParticleSystems) {
					if (t.name == varName)
						return t.particle;
				}
				return null;
			}
		}
		#endregion
		#region Var-ParticleSystems
		public static object GetClass( object _class , string _key ){
			System.Reflection.FieldInfo field = _class.GetType ().GetField (_key);
			return field.GetValue (_class);
		}
		public static void ToClass( object _class , string _key , object _value ){
			System.Reflection.FieldInfo field = _class.GetType ().GetField (_key);
			if (field != null) {
				if (field.FieldType == typeof(System.Int32)) 
				{
					field.SetValue (_class, System.Convert.ToInt32 (_value));
				} else if (field.FieldType == typeof(System.Int64)) {
					field.SetValue (_class, System.Convert.ToInt64 (_value));
				} else if (field.FieldType == typeof(System.Single)) {
					field.SetValue (_class, System.Convert.ToSingle (_value));
				} else if (field.FieldType == typeof(System.Double)) {
					field.SetValue (_class, System.Convert.ToDouble (_value));
				} else if (field.FieldType == typeof(System.String)) {
					field.SetValue (_class, System.Convert.ToString (_value));
				} else if (field.FieldType == typeof(System.Boolean)) {
					field.SetValue (_class, System.Convert.ToBoolean (_value));
				}
				else if (field.FieldType == typeof(System.Double[]))
				{
					var listformula = (Service.Formula)_value;
					int index = 0;
					double[] output = new double[listformula.GetFormulaDatas.Count];
					foreach (var d in listformula.GetFormulaDatas) {
						output[index] = d.Value;
						index++;
					}
					field.SetValue(_class, output );
				}
				else {
					field.SetValue (_class, _value);
				}
			}
		}
		#endregion
	}




	public class Tag{
		static List<TagData> tags = new List<TagData>(); 
		class TagData{
			public string Tag{get{ return m_tag;}}
			string m_tag;
			public List<object> Content{get{ return m_tagobj;}}
			List<object> m_tagobj = new List<object>(); 
			public TagData(string tagname ){
				m_tag = tagname;
			}
			public void Add(object obj){
				m_tagobj.Add (obj);
			}
		}
		public static void Put (string tag , object obj){
			TagData NewTag = null;
			foreach (TagData i_tag in tags) {
				if (i_tag.Tag == tag)
					NewTag = i_tag;
			}
			if (NewTag == null) {
				NewTag = new TagData (tag);
				tags.Add (NewTag);
			}
			NewTag.Add (obj);
		}
		static TagData Find(string tag){
			foreach (TagData i_tag in tags) {
				if (i_tag.Tag == tag)
					return i_tag;
			}
			return null;
		}
		public static List<object> Get (string tag , bool notnull = false){
			TagData find = null;
			find = Find (tag);
			if (find == null) {
				return null;
			} else {
				if (notnull) 
				{
					List<object> output = null;
					foreach (object obj in find.Content) {
						if (!object.ReferenceEquals (obj, null)) {
							if (output == null)
								output = new List<object> ();
							output.Add (obj);
						}
					}
					return output;
				} else {
					return find.Content;
				}
			}
		}
		public static object GetLast (string tag , bool notnull = false ){
			List<object> objs = Get(tag,notnull);
			if (objs == null)
				return null;
			else {
				if (objs.Count == 0)
					return null;
				else
					return objs [objs.Count-1];
			}
		}
		public static bool isHave(string tag)
		{
			List<object> objs = Get(tag, false);
			if (objs == null)
				return false;
			else
			{
				if (objs.Count == 0)
					return false;
				else
					return true;
			}
		}
		public static void Clear(string tag) {
			TagData tagData = Find (tag);
			if (tagData != null)
				tags.Remove (tagData);
		}
		public static void ClearAll( ) {
			tags.Clear ();
		}
	}







	#region Tools
	public class Tools{
		static GameObject m_gameservice;
		public static GameObject gameservice{
			get{
				if (m_gameservice == null) {
					m_gameservice = new GameObject ("#GameService");
					GameObject.DontDestroyOnLoad (m_gameservice);  
				}
				return m_gameservice;
			}
		}




		public static class ObjectCopier
		{
			/// <summary>
			/// Perform a deep Copy of the object.
			/// </summary>
			/// <typeparam name="T">The type of object being copied.</typeparam>
			/// <param name="source">The object instance to copy.</param>
			/// <returns>The copied object.</returns>
			public static T Clone<T>(T source)
			{
				if (!typeof(T).IsSerializable)
				{
					  Debug.LogError("The type must be serializable." + nameof(source));
				}

				// Don't serialize a null object, simply return the default for that object
				if (Object.ReferenceEquals(source, null))
				{
					return default(T);
				}
				IFormatter formatter = new BinaryFormatter();
				Stream stream = new MemoryStream();
				using (stream)
				{
					formatter.Serialize(stream, source);
					stream.Seek(0, SeekOrigin.Begin);
					return (T)formatter.Deserialize(stream);
				}
			}
		}







		// Addon
		public static AddOn.IEnume AddIEnume(GameObject addTarget = null){
			GameObject target;
			if (addTarget != null) target = addTarget;
			else target = gameservice;
			AddOn.IEnume addOn = target.AddComponent <AddOn.IEnume>();
			return addOn;
		}
		// Addon
		public static AddOn.Timmer AddTimmer(GameObject addTarget = null){
			GameObject target;
			if (addTarget != null) target = addTarget;
			else target = gameservice;
			AddOn.Timmer addOn = target.AddComponent <AddOn.Timmer>();
			return addOn;
		}



		public class DoubleClick : MonoBehaviour {
			bool one_click = false;
			bool timer_running;
			float timer_for_double_click;
			float delay = 0.25f;
			public void OnEnter ( Service.Callback.callback callback )
			{
				if(one_click)
				{
					if(( UnityEngine.Time.time - timer_for_double_click) > delay)
					{
						one_click = false;
					}
				}
				if(!one_click) 
				{
					one_click = true;
					timer_for_double_click = UnityEngine.Time.time; 
				} 
				else
				{
					//** DoubleClick
					one_click = false; 
					if (callback != null)
						callback ();
				}
			}

		}

		[System.Serializable]
		public class OpenEyes  {

            #region Use-For-Inspecter
            [System.Serializable]
			public class OpenEyeData {
				public string Name;
				public Transform T;
			}
			public List<OpenEyeData> Content = new List<OpenEyeData>();
			public void Open(string contentName)
			{
				if(trans.Count == 0)
				{
					foreach (var ct in Content)
						AddTransform(ct.T);
				}
				var t = Content.Find(x=>x.Name == contentName);
				Open( (t==null)? null: t.T);
			}
            #endregion


			[SerializeField]
            private List<Transform> trans = new List<Transform>();
			public List<Transform> Transforms {
				get{ return trans; }
			}
			public void AddTransform(Transform tran){
				trans.Add (tran);
			}
			public void AddTransform(Transform[] m_trans){
				foreach (Transform t in m_trans) 
					trans.Add (t);
			}
			public void AddTransform(List<Transform> m_trans){
				foreach (Transform t in m_trans) 
					trans.Add (t);
			}
			public void AddTransforms(List<object> Trans){
				foreach (object t in Trans) {
					GameObject obj = (GameObject)t;
					trans.Add (obj.transform);
				}
			}
			public void Open(Transform tran = null){
				foreach (Transform t in trans) {
					if (t != tran)
						t.gameObject.SetActive (false);
					else
						t.gameObject.SetActive (true);
				}
			}
			public void Open(bool active){
				foreach (Transform t in trans) {
					t.gameObject.SetActive (active);
				}
			}
		}

		public class FunctionCalling{
			public delegate void callback(Service.Formula f = null, Service.Callback.callback onfinish = null );
			public class FunctionCallingData{
				public callback Function;
				public string Name;
				public bool IsCallOnecTime;
				public string Tag;
			}
			public List<FunctionCallingData> 	Functions = new List<FunctionCallingData>();
			public FunctionCallingData Add(string functionName, callback function){
				return Add(functionName, string.Empty , false, function);
			}
			public FunctionCallingData Add(string functionName, string tag ,callback function){
				return Add(functionName, tag , false , function);
			}
			public FunctionCallingData Add(string functionName,bool isOnecTime, callback function){
				return Add(functionName, string.Empty, isOnecTime , function);
			}
			public FunctionCallingData Add(string functionName , string tag, bool isOnecTime ,callback function){
				FunctionCallingData f = Get(functionName);
				if (f == null) {
					f = new FunctionCallingData ();
					f.Name = functionName;
					f.Tag = tag;
					f.IsCallOnecTime = isOnecTime;
					f.Function = function;
					Functions.Add (f);
				}
				else
				{
					f.Function = function;
				}
				return f;
			}
			FunctionCallingData Get(string functionName){
				foreach (FunctionCallingData f in Functions) {
					if (f.Name == functionName)
						return f;
				}
				return null;
			}
			public void Call(string functionName , Service.Formula formula = null,Service.Callback.callback onfinish = null){
				FunctionCallingData f = Get (functionName);
				if (f != null) 
				{
					f.Function(formula, onfinish);

					if (f.IsCallOnecTime)
					{
						Des(f.Name);
					}
				}
			}
			public void CallAll( Service.Formula formula = null,Service.Callback.callback onfinish = null){
				if (Functions != null && Functions.Count > 0)
				{
					foreach (Service.Tools.FunctionCalling.FunctionCallingData f in new ArrayList(Functions))
						if (f != null)
						{
							f.Function(formula, onfinish);
							if (f.IsCallOnecTime)
							{
								Des(f.Name);
							}
						}
				}
				else 
				if (onfinish != null)
					onfinish();

			}
			public void CallAll(string tag, Service.Formula formula = null, Service.Callback.callback onfinish = null)
			{
				if (Functions != null && Functions.Count > 0) 
				{ 
					foreach (Service.Tools.FunctionCalling.FunctionCallingData f in new ArrayList(Functions))
						if (f != null && f.Tag == tag)
						{
							f.Function(formula, onfinish);
							if (f.IsCallOnecTime)
							{
								Des(f.Name);
							}
						}
				}
				else
				if (onfinish != null)
					onfinish();
			}
			public void Des(string functionName ){
				FunctionCallingData f = Get (functionName);
				if (f != null)
					Functions.Remove (f);
			}
            public void Clear(string tag = null)
            {
                if (tag == null)
                    Functions.Clear();
                else
                {
                    foreach (Service.Tools.FunctionCalling.FunctionCallingData f in new ArrayList(Functions))
                        if (f != null && f.Tag == tag)
                        {
                            Des(f.Name);

                        }
                }
            }
        }



		public class ObjectDefine
		{
			object m_obj;
			public ObjectDefine(object obj){
				m_obj = obj;
			}
			public string String { get{ if (m_obj == null)	return string.Empty; else return (string) m_obj; } }
			public int Int { get{ if (m_obj == null)	return 0; else return (int) m_obj; } }
			public double Double { get{ if (m_obj == null)	return 0; else return (double) m_obj; } }
			public float Float { get{ if (m_obj == null)	return 0.0f; else return (float) m_obj; } }
			public bool Bool { get{ if (m_obj == null)	return false; else return (bool) m_obj; } }




			public static Service.Formula.FormulaData.datatype DefineType(object obj) {
				if (obj.GetType() == typeof(string)) return Formula.FormulaData.datatype.str;
				else if (Service.Tools.ObjectDefine.IsNumeric(obj)) return Formula.FormulaData.datatype.num;
				else if (obj.GetType() == typeof(bool)) return Formula.FormulaData.datatype.bol;
				else if (obj.GetType() == typeof(Service.Formula)) return Formula.FormulaData.datatype.formula;
				else if (obj.GetType() == typeof(Service.Formula.FormulaData)) return Formula.FormulaData.datatype.formulaData;
				else if (obj.GetType() == typeof(List<Service.Formula.FormulaData>)) return Formula.FormulaData.datatype.list;
				return Formula.FormulaData.datatype.obj;
			}

			public static bool IsNumeric(object obj)
			{
				switch (System.Type.GetTypeCode(obj.GetType()))
				{
					case System.TypeCode.Byte:
					case System.TypeCode.SByte:
					case System.TypeCode.UInt16:
					case System.TypeCode.UInt32:
					case System.TypeCode.UInt64:
					case System.TypeCode.Int16:
					case System.TypeCode.Int32:
					case System.TypeCode.Int64:
					case System.TypeCode.Decimal:
					case System.TypeCode.Double:
					case System.TypeCode.Single:
						return true;
					default:
						return false;
				}
			}
			public static double NumericToDouble(object obj)
			{
				double value = 0;
				switch (System.Type.GetTypeCode(obj.GetType()))
				{
					case System.TypeCode.UInt16: value	= (ushort)obj; break;
					case System.TypeCode.UInt32: value	= (uint)obj; break;
					case System.TypeCode.UInt64: value	= (ulong)obj; break;
					case System.TypeCode.Int16: value	= (short)obj; break;
					case System.TypeCode.Int32: value	= (int)obj; break;
					case System.TypeCode.Int64: value	= (long)obj; break;
					case System.TypeCode.Double: value	= (double)obj; break;
					case System.TypeCode.Single: value	= (float)obj; break;
				}
				return value;
			}
		}


		[System.Serializable]
		public class Container
		{
			public GameObject pObject;
			public Transform root;
			List<GameObject> m_GameObjects = new List<GameObject>();
			public List<GameObject> GameObjects {
				get { return m_GameObjects; }
			}


			public List<T> Generate<T>(int count, Service.Callback.callbackgameobject caseout = null)
			{
				List<T> list = new List<T>();
				for (int i = 0; i < count; i++)
				{
					var g = Service.GameObj.Created(pObject, root);
					list.Add(g.GetComponent<T>());
					if (caseout != null)
						caseout(g);
				}
				return list;
			}
		}





		public class Action {
			public static void LoopCase(int now , int max , Service.Callback.callback_value casein , Service.Callback.callback_value caseout) {
				for (int i = 0; i < max; i++) {
					if (i < now) { if (casein != null) casein(i); }
					else { if (caseout != null) caseout(i); }

				}
			}
		}


		public class BinarySearch
		{
			char Getkey( string key ) {
				if(!string.IsNullOrEmpty(key))
					return key.ToCharArray()[0];
				else 
					return char.MinValue;
			}
			Dictionary<char, Dictionary<string, int>> raw = new Dictionary<char, Dictionary<string, int>>();
			public  void Init( List<string> keys )
			{
				raw = new Dictionary<char, Dictionary<string, int>>();
				int index = 0;
				foreach (var key in keys) 
				{
					char ch = Getkey(key);
					if (!raw.ContainsKey(ch)) 
					{
						raw.Add( ch , new Dictionary<string, int>());
					}
					if (!raw[ch].ContainsKey(key))
						raw[ch].Add(key, index);
					else
						Debug.LogError("ContainsKey:" + key);
					index++;
				}
			}
			public  int Find (string key)
			{
				char ch = Getkey(key);
				if (raw.ContainsKey(ch))
				{
					var r = raw[ch];
					foreach (var i in r)
					{
						if (i.Key == key)
							return i.Value;
					}
				}
				return -1;
			}
		}
	}
	#endregion



	#region String
	public class String  {
		public static void Copy(string messge){
			TextEditor tx = new TextEditor ();
			tx.text = messge;
			tx.SelectAll();
			tx.Copy ();
		}
		public static bool isStrCropValue(string data, string CH_start, string CH_end)
		{
			bool isCanCrop = true;
			int indexstart = data.IndexOf(CH_start);
			int indexend = data.LastIndexOf(CH_end);
			if (indexstart == -1) isCanCrop = false;
			if (indexend == -1) isCanCrop = false;
			return isCanCrop;
		}
		public static string strCropValue(string data,string CH_start,string CH_end)
		{
			bool isCanCrop = true;
			int indexstart = data.IndexOf(CH_start);
			int indexend = data.LastIndexOf(CH_end);
			if (indexstart == -1)	isCanCrop = false;
			if (indexend == -1)	isCanCrop = false;

			if (isCanCrop) {
				indexstart += CH_start.Length;
				indexend -= indexstart;
				string newdata = data.Substring (indexstart, indexend);
				return(newdata);
			} 
			else {
				Debug.LogError("Not Can Crop data Value");
				return data;
			}
		}
		public static List<string> strCropValues (string data,string CH_start,string CH_end){
			string temp = data;
			bool isDo = true;
			List<string> values = new List<string> ();
			while(isDo){
				if (temp.IndexOf (CH_start) == -1)
					isDo = false;
				if (temp.IndexOf (CH_end) == -1)
					isDo = false;
				if (isDo) {
					string content = string.Empty;
					bool isCanCrop = true;
					int indexstart = temp.IndexOf(CH_start);
					string newData = temp.Remove (indexstart,1);
					int indexend = newData.IndexOf(CH_end); indexend++;
					if (indexstart == -1)	isCanCrop = false;
					if (indexend == -1)	isCanCrop = false;
					if (isCanCrop) {
						indexstart += CH_start.Length;
						indexend -= indexstart;
						content = temp.Substring (indexstart, indexend);
						temp = temp.Remove ( indexstart , indexend );
						temp = temp.Replace (CH_start + CH_end, "");
						values.Add (content);
					} 
				}
			}
			return values;
		}
		public static List<string> strCropValuesFormat(string data, char CH_start, char CH_end)
		{
			List<string> values = new List<string>();
			string temp = data;
			bool isCan = true;
			while (isCan)
			{
				isCan = isCanCrop(temp, CH_start, CH_end);
				if (isCan)
				{
					string outout = crop(temp, CH_start, CH_end);
					values.Add(outout);
					int[] index = indexOfCrop(temp, CH_start, CH_end);
					temp = temp.Remove(index[0]+1, index[1]-index[0]-1);
					temp = temp.Replace(CH_start.ToString() + CH_end.ToString(), "");
				}
			}
			return values;
		}
		static bool isCanCrop(string data, char CH_start, char CH_end)
		{
			bool isDo = true;
			int[] indexs = indexOfCrop(data, CH_start, CH_end);
			int start = indexs[0];
			int end = indexs[1];
			if (start == -1)
				isDo = false;
			if (end == -1)
				isDo = false;
			if (start >= end)
				isDo = false;
			return isDo;
		}
		static int[] indexOfCrop(string data, char CH_start, char CH_end )
		{
			string temp = data;
			int end = -1;
			int start = -1;

				end = data.IndexOf(CH_end);
				bool isCan = true;
				while (isCan)
				{
					int find = temp.IndexOf(CH_start);
					if (find != -1)
					{
						char[] chars = temp.ToArray<char>();
						chars[find] = CH_end;
						temp = new string(chars);

						if (find < end)
							start = find;
					}
					else isCan = false;
				}
			
			return new int[2] { start, end };
		}
		static string crop(string data, char CH_start, char CH_end)
		{
			string temp = data;
			string output = "";
			bool isDo = isCanCrop(temp, CH_start, CH_end);
			if (isDo)
			{
				int[] indexs = indexOfCrop(temp, CH_start, CH_end);
				int start = indexs[0];
				int end = indexs[1];
				if ((start < end))
					output = temp.Substring(start + 1, end - start - 1);
			}
			return output;
		}

		public static string strCropRemove(string data , string CH_start, string CH_end) {
			if (isStrCropValue(data, CH_start, CH_end))
			{
				string s = strCropValue(data, CH_start, CH_end);
				return data.Replace(CH_start + s + CH_end, "");
			}
			else return data;
		}

		public static string RemoveSpecialCharater(string str){
			string Text = str;
			Text = Text.Replace("\n","");
			Text = Text.Replace("\r","");
			Text = Text.Replace("\t","");
			Text = Text.Replace("\v","");
			return  Text;
		}
		public static string RemoveSymbolCharater(string str){
			string NameValidate = str;
			NameValidate = NameValidate.Replace ("*", "");
			NameValidate = NameValidate.Replace ("/", "");
			NameValidate = NameValidate.Replace ("\\", "");
			NameValidate = NameValidate.Replace ("#", "");
			NameValidate = NameValidate.Replace ("&", "");
			NameValidate = NameValidate.Replace ("@", "");
			NameValidate = NameValidate.Replace ("(", "");
			NameValidate = NameValidate.Replace (")", "");
			NameValidate = NameValidate.Replace ("<", "");
			NameValidate = NameValidate.Replace (">", "");
			NameValidate = NameValidate.Replace ("[", "");
			NameValidate = NameValidate.Replace ("]", "");
			NameValidate = NameValidate.Replace ("{", "");
			NameValidate = NameValidate.Replace ("}", "");
			NameValidate = NameValidate.Replace ("+", "");
			NameValidate = NameValidate.Replace ("-", "");
			NameValidate = NameValidate.Replace ("!", "");
			NameValidate = NameValidate.Replace ("?", "");
			NameValidate = NameValidate.Replace ("$", "");
			NameValidate = NameValidate.Replace ("%", "");
			NameValidate = NameValidate.Replace ("^", "");
			NameValidate = NameValidate.Replace ("~", "");
			NameValidate = NameValidate.Replace ("'", "");
			NameValidate = NameValidate.Replace ("\"", "");
			NameValidate = NameValidate.Replace (".", "");
			NameValidate = NameValidate.Replace (",", ""); 
			NameValidate = NameValidate.Replace (":", "");
			NameValidate = NameValidate.Replace (";", "");
			NameValidate = NameValidate.Replace ("=", "");
			NameValidate = NameValidate.Replace ("_", "");
			return NameValidate;
		}
		public static bool isEmailValidate(string Str){
            //exm : bas.poogame.dev@gmail.com

            if (validateEmailInput(Str) == false) return false;

			int indexAddress = Str.IndexOf ("@");
			int indexDot= Str.LastIndexOf (".");
			if (indexAddress == -1)	return false;
			if (indexDot == -1)	return false;
			if(indexAddress > indexDot) return false;
			string[] addressSplit = Str.Split ('@');
			if (addressSplit.Length != 2)	return false;
			if (addressSplit[0].Length <= 0)	return false;
			if (addressSplit[1].Length <= 0)	return false;
			string[] dotSplit = Str.Split ('.');
			if (dotSplit[dotSplit.Length-2].Length <= 0)	return false;
			if (dotSplit[dotSplit.Length-1].Length <= 0)	return false;

			string sumString = string.Empty;
			for(int count = 0 ; count<dotSplit.Length-1;count++){
				sumString += dotSplit[count];
			}
			string[] CompanyMailstr = sumString.Split ('@');
			if (CompanyMailstr[0].Length <= 0)	return false;
			if (CompanyMailstr[1].Length <= 0)	return false;
			return true;
		}
        static bool validateEmailInput(string Str)
        {
            foreach(char ch in Str)
            {
                if(validateEmailInput(ch) == false)
                {
                    return false;
                }
            }
            return true;

        }
        static bool validateEmailInput(char ch)
        {
            // All alphanumeric characters
            if (ch >= 'A' && ch <= 'Z') return true;
            if (ch >= 'a' && ch <= 'z') return true;
            if (ch >= '0' && ch <= '9') return true;
            // Email symbol character
            if (ch == '@' || ch == '.') return true;

            return false;
        }
		static string ContentCommaValidate( string master ){
			List<string> added = new List<string> ();
			string new_master = string.Empty;
			foreach(string str in  master.Split (',')){
				if( string.IsNullOrEmpty(new_master))
					new_master = str;
				else 
					if (added.FindIndex (x => x == str) == -1) {
						new_master+= ","+str;
					}
				added.Add (str);
			}
			return new_master;
		}
		public static string AddCommarString(string add , string master , bool isUnique = false){
			if (string.IsNullOrEmpty (master))
				master = add;
			else 
				master  += "," + add;
			if (isUnique)
				master = ContentCommaValidate (master);
			return master;
		}
		public static string AddCommarString(List<string> str , bool isUnique = false)
		{
			string output = "";
			if (str != null) 
				str.ForEach((s) => {
				output = Service.String.AddCommarString(s, output, true);
			});
			return output;
		}
		public static string AddCommarString( string[] str, bool isUnique = false)
		{
			string output = "";
			if(str!=null)
				str.ToList<string>().ForEach((s) => {
				output = Service.String.AddCommarString(s, output, true);
			});
			return output;
		}
		public static string DesCommarString(string des, string master, bool isUnique = false){
			string Text = string.Empty;
			foreach (string str in master.Split(','))
			{
				if (str != des) { 
					if (string.IsNullOrEmpty(Text))
						Text = str;
					else
						Text += "," + str;
				}
			}
			if (isUnique)
				Text = ContentCommaValidate(Text);
			return Text;
		}


		public static string ListStringToStringCommar(string[] strlist , bool isUnique = false ){
			string Text = string.Empty;
			foreach( string str in strlist){
				if( string.IsNullOrEmpty(Text))
					Text = str;
				else 
					Text+= ","+str;
			}
			if (isUnique)
				Text = ContentCommaValidate (Text);
			return  Text;
		}
		public static string ListStringToStringCommar(List<string> strlist ,  bool isUnique = false){
			string Text = string.Empty;
			foreach( string str in strlist){
				if(string.IsNullOrEmpty(Text))
					Text = str;
				else 
					Text+= ","+str;
			}
			if (isUnique)
				Text = ContentCommaValidate (Text);
			return  Text;
		}
		public static List<string> CommarStringToList(string Text){
			if (string.IsNullOrEmpty (Text))
				return new List<string> ();
			return  Text.Split(',').ToList();
		}




		public enum TypeWriter
		{
			up,low,all,number
		}
		public class CharConstans
		{
			public const int A = 65; 
			public const int Z = 90; 
			public const int a = 97; 
			public const int z = 122; 
		}
		public static string RandomString( TypeWriter type ){
			if(type == TypeWriter.number){
				return Random.Range (0, 10).ToString();
			}
			else if (type == TypeWriter.up) {
				return ((char)Random.Range (CharConstans.A, CharConstans.Z + 1)).ToString ();
			} else if (type == TypeWriter.low) {
				return ((char)Random.Range (CharConstans.a, CharConstans.z + 1)).ToString ();
			} else {
				return ((char)Random.Range (CharConstans.A, CharConstans.z + 1)).ToString ();
			}
		}
		public static string RandomUniKey( int ch ,bool isHeader = true){
			string key = string.Empty;
			if(isHeader)
					key = "unikey_"+
						  "r_" + Random.Range (11111, 99999).ToString () +
			              "d_" + Service.Time.DateTimeToUnixTimeStamp (System.DateTime.Now).ToString () +
			              "c_";
			for (int i = 0; i < ch; i++) {
				key += String.RandomString (String.TypeWriter.all);
			}
			return key;
		}
		public static string ASCIItoString ( int ASCII  ){
			return ((char)ASCII).ToString ();
		}
		public static List<int> StringToASCII ( string str  ){
			List<int> i = new List<int> ();
			if (!string.IsNullOrEmpty (str)) {
				foreach (char c in str.ToCharArray()) {
					i.Add ((int)c);
				}
			}
			return i;
		}
		/// <summary>
		/// Str = (0.0f,0.0f,0.0f)
		/// </summary>
		public static Vector3 PassStringToVector3(string Str){  
			Vector3 IsVec = Vector3.zero;
			if (string.IsNullOrEmpty (Str))
				return IsVec;
			
			Str = strCropValue (Str,"(",")");
			string[] Value = Str.Split (',');
			IsVec.x = System.Convert.ToSingle (Value[0]);
			IsVec.y = System.Convert.ToSingle (Value[1]);
			IsVec.z = System.Convert.ToSingle (Value[2]);
			return IsVec;
		}
		public static Vector2 PassStringToVector2(string Str){  
			Vector2 IsVec = Vector2.zero;
			if (string.IsNullOrEmpty (Str))
				return IsVec;

			Str = strCropValue (Str,"(",")");
			string[] Value = Str.Split (',');
			IsVec.x = System.Convert.ToSingle (Value[0]);
			IsVec.y = System.Convert.ToSingle (Value[1]);
			return IsVec;
		}
		public static int PassStringToInt(string Str){  
			if (string.IsNullOrEmpty (Str))
				return 0;
			else 
				return System.Convert.ToInt32 (Str);
		}
		public static double[] PassStringToRange(string Str){  
			double[] output = new double[0];
			string range = Str;
			if (!string.IsNullOrEmpty (range)) {
				if (range.IndexOf ("[") != -1) 
				if (range.IndexOf ("]") != -1) 
					range = String.strCropValue (range, "[", "]");
				
				List<double> doubles = new List<double> ();
				foreach (string val in  range.Split ('/')) 
				{
					doubles.Add (Parse (val,0.0));
				}
				output = doubles.ToArray ();
			}
			return output;
		}
		public static string PassRangeToString(double[] vals){  
			string sum = "[";
			for (int i = 0; i < vals.Length; i++) {
				if (i != 0)
					sum += "/";
				sum += vals [i].ToString ();
			}
			sum += "]";
			return sum;
		}

		public static int Parse(string Str, int _default){
			if (string.IsNullOrEmpty (Str)) return _default;
			else return System.Convert.ToInt32 (Str);
		}
		public static long Parse(string Str,long _default){  
			if (string.IsNullOrEmpty (Str)) return _default;
			else return System.Convert.ToInt64 (Str);
		}
		public static double Parse(string Str ,double _default){  
			if (string.IsNullOrEmpty (Str)) return _default;
			else return System.Convert.ToDouble (Str);
		}
		public static float Parse(string Str ,float _default){  
			if (string.IsNullOrEmpty (Str)) return _default;
			else return System.Convert.ToSingle (Str);
		}
		public static bool Parse(string Str ,bool _default){  
			if (string.IsNullOrEmpty (Str))	return _default;
			else return System.Convert.ToBoolean (Str);
		}
		public static string Parse(string Str ,string _default){  
			if (string.IsNullOrEmpty (Str))
				return _default;
			else
				return Str;
		}
		public static Service.Formula  Parse ( string Str , Service.Formula _default){  
			return Service.Formula.Setup(Str);
		}
		public static Vector2 Parse(string Str ,Vector2 _default  ){  
			return PassStringToVector2 (Str);
		}
		public static Vector3 Parse(string Str ,Vector3 _default){  
			return PassStringToVector3 (Str);
		}
		public static void To(string Str , out int result ){  
			result = Parse(Str,0);
		}
		public static void To(string Str , out long result ){  
			result = Parse(Str,0);
		}
		public static void To(string Str , out double result ){  
			result = Parse(Str,0.0);
		}
		public static void To(string Str , out float result ){  
			result = Parse(Str,0.0f);
		}
		public static void To(string Str , out bool result ){  
			result = Parse(Str,false);
		}
		public static void To(string Str , out string result ){  
			result = Parse(Str,string.Empty);
		}
		public static void To(string Str , out Service.Formula result ){  
			result = Parse(Str,new Service.Formula());
		}
		public static void To(string Str , out Vector2 result ){  
			result = Parse(Str,Vector2.zero);		
		}
		public static void To(string Str , out Vector3 result ){  
			result = Parse(Str,Vector3.zero);
		}
		/// <summary>
		/// Value = (ValueType) Service.String.ToEnum ("str",Value.none);
		/// </summary>
		public static System.Enum ToEnum(string Str, object Default){
			try{
				return (System.Enum)  System.Enum.Parse ( Default.GetType() ,Str);
			}catch{
				//Debug.LogError ("Can't ToEnum = '"+Str+"'");
				return (System.Enum) System.Enum.Parse ( Default.GetType() , Default.ToString());
			}
		}
		public static string RemoveTheLastCharacter(string str)
		{
			return str.Remove(str.Length - 1, 1);
		}
		//public static int key=2533;
		public enum EncodeStringType { linear, curve }
		public static string EncodeString(string str, int key, EncodeStringType encodeStringType)
		{
			string output_s = "";

			if (encodeStringType == EncodeStringType.linear)
			{
				if (str.Length > 0)
				{
					for (int n = 0; n < str.Length; n++)
					{
						char ch = str[n];
						int asc = System.Convert.ToInt32(ch);
						asc += key;
						output_s += char.ConvertFromUtf32(asc);
					}
				}
			}
			else if (encodeStringType == EncodeStringType.curve)
			{

				int maxCurve = 999;
				int currentCurve = 0;

				if (str.Length > 0)
				{
					for (int n = 0; n < str.Length; n++)
					{
						char ch = str[n];
						int asc = System.Convert.ToInt32(ch);

						if (key >= 0) asc += key + currentCurve;
						else asc += key - currentCurve;

						output_s += char.ConvertFromUtf32(asc);
						currentCurve++;
						if (currentCurve >= maxCurve)
							currentCurve = 0;
					}
				}
			}
			return output_s;
		}
		public static bool isCheckHaveinString(string strFind, string strMessage)
		{
			if (string.IsNullOrEmpty(strMessage)) return false;
			else return (strMessage.IndexOf(strFind) != -1);
		}








		public static string ClassToString(object c) {
			return ServiceJson.Json.SerializeObject(c);
		}


		public class Transforms
		{
			public static string TransformToString( Transform transform ) {
				string vect(Vector3 position) {
					return  $"({position.x},{position.y},{position.z})";
				}
				return vect(transform.localPosition) + "/" + vect(transform.localRotation.eulerAngles) + "/" + vect(transform.localScale);
			}
			public static void StringToTransform(string text  ,Transform transform)
			{
				string[] vect = text.Split('/');
				transform.localPosition = Service.String.PassStringToVector3(vect[0]);
				Quaternion rotate = new Quaternion();
				rotate.eulerAngles = Service.String.PassStringToVector3(vect[1]);
				transform.transform.localRotation = rotate;
				transform.localScale = Service.String.PassStringToVector3(vect[2]);
			}
		}

		public class StringArray
		{
			public enum Brackets
			{
				None,           //
				Message,        // ".."
				Brackets,       // (..)
				CurlyBrackets,  // {..}
				SquareBrackets, // [..]
				AngleBrackets   // <..>
			}
			public class BracketsData
			{
				public string header;
				public string footer;
				public BracketsData(Brackets type)
				{
					switch (type)
					{
						case Brackets.Message: header = "\""; footer = "\""; break;
						case Brackets.Brackets: header = "("; footer = ")"; break;
						case Brackets.CurlyBrackets: header = "{"; footer = "}"; break;
						case Brackets.SquareBrackets: header = "["; footer = "]"; break;
						case Brackets.AngleBrackets: header = "<"; footer = ">"; break;
					}
				}
			}
			BracketsData m_master;
			BracketsData m_content;
			public StringArray(Brackets master = Brackets.None, Brackets content = Brackets.None)
			{
				this.m_master = new BracketsData(master);
				this.m_content = new BracketsData(content);
			}
			public StringArray(string group_herader, string group_footer, string content_herader = "", string content_footer = "")
			{
				this.m_master = new BracketsData(Brackets.None) { header = group_herader, footer = group_footer };
				this.m_content = new BracketsData(Brackets.None) { header = content_herader, footer = content_footer };
			}
			public void AddMemeber(string ID, int Count = 1)
			{
				for (int n = 0; n < Count; n++)
				{
					m_memeber.Add(ID);
				}
			}
			List<string> m_memeber = new List<string>();
			public string GetString()
			{
				//["material_0","material_0"]
				string content = string.Empty;
				foreach (string ID in m_memeber)
				{
					if (!string.IsNullOrEmpty(content))
						content += ",";
					content += m_content.header + ID + m_content.footer;
				}
				content = m_master.header + content + m_master.footer;
				Debug.Log(content);
				return content;
			}
		}
		public class StringDefine{
			string m_str;
			public StringDefine(string str){
				m_str = str;
			}
			public string 	String 	{ get{ string 	output; To (m_str, out output); return output;} }
			public int 		Int 	{ get{ int 		output; To (m_str, out output); return output;} }
			public double 	Double 	{ get{ double 	output; To (m_str, out output); return output;} }
			public float 	Float 	{ get{ float 	output; To (m_str, out output); return output;} }
			public bool 	Bool 	{ get{ bool 	output; To (m_str, out output); return output;} }

			public static Formula.FormulaData.datatype StringType(string Str ){
				if (!string.IsNullOrEmpty(Str)){
					double Double;
					bool Bool;
					if (System.Double.TryParse(Str,out Double))
					{ 
						return Formula.FormulaData.datatype.num;
					}
					else if (System.Boolean.TryParse(Str, out Bool))
					{
						return Formula.FormulaData.datatype.bol;
					}
					else if (Json.isJson(Str) )
					{
						Debug.Log(Str);
						return Formula.FormulaData.datatype.json;
					}
					else if(Json.isJsonArray(Str))
					{
						Debug.Log(Str);
						return Formula.FormulaData.datatype.json;
					}
					else return Formula.FormulaData.datatype.str;
				}
				return Formula.FormulaData.datatype.none;
			}
			public static object StringToObject(string Str)
			{
				object output = null;
				if (!string.IsNullOrEmpty(Str))
				{
					double Double;
					bool Bool;
					if (System.Double.TryParse(Str, out Double))
					{
						output = Double;
						return output;
					}
					else if (System.Boolean.TryParse(Str, out Bool))
					{
						output = Bool;
						return output;
					}
					else if (Json.isJson(Str))
					{
						output = FormulaToolsService.Json.JsonToJFormula(Str);
						return output;
					}
					else if (Json.isJsonArray(Str))
					{
						output = FormulaToolsService.Json.JsonArrayToJFormulas(Str);
						return output;
					}
					else return Str;
				}
				return output;
			}
			public static StringDefine ToStringDefine(string Str){
				return new StringDefine(Str);
			}
		}
		public class Json{
			public static Service.Formula JsonToFormula(string json)
			{
				return FormulaToolsService.Json.JsonToJFormula(json);
			}
			public static List<Service.Formula> JsonToFormulas(string json)
			{
				return FormulaToolsService.Json.JsonArrayToJFormulas(json);
			}
            #region OLD
			/*
            public static Service.Formula JsonToFormula(string json) {
				if (string.IsNullOrEmpty(json))
					return new Service.Formula();


				//** CropString{ }
				string keyreplace = "<#key-replace#-";
				Service.Formula strContent = new Service.Formula();
				string NewJson = json;
				if (NewJson.IndexOf("{") != -1)
					if (NewJson.IndexOf("}") != -1)
						NewJson = strCropValue(NewJson, "{", "}");


				//** CropString SubJson to Args { }
				string keyjson = "<#key-json#-";
				string checkType = NewJson;
				Service.Formula strContentJson = new Service.Formula();
				if (String.isStrCropValue(checkType, "{", "}")) {
					string subJson = String.strCropValue(checkType, "{", "}");
					int indexcheck = 0;
					List<string> subJsons = String.strCropValues(checkType, "{","}");
					foreach (string s in subJsons) 
					{
						string KEYJSON = "\"" + keyjson + indexcheck.ToString() + ">\"";
						strContentJson.AddFormula(KEYJSON.Replace("\"",""),s);
						checkType = checkType.Replace(s, KEYJSON);
						indexcheck++;
					}
				}
				NewJson = checkType;



				List<string> values = strCropValues ( NewJson , "\"","\"");
				int index = 0;
				foreach (string dataStr in values) 
				{
					if (!string.IsNullOrEmpty(dataStr))
					{
						string key = keyreplace + index.ToString() + ">";
						NewJson = NewJson.Replace("\"" + dataStr + "\"", key);
						strContent.AddFormula(key, dataStr);
						index++;
					}
				}
				NewJson = NewJson.Replace("\"\"","");
				NewJson = NewJson.Replace("'", Formula.single);


				string formulastring = string.Empty;
				foreach (string str in NewJson.Split(',')) {
					if (!string.IsNullOrEmpty (str)) {
						string[] v = str.Split (':');
						string key 		= v [0];
						string value = v[1];
						//Debug.Log(value);

						key = strContent.GetFormula (  "<" + String.strCropValue(key,"<",">") +">" ).Text;
						if (value.IndexOf (keyreplace) != -1) 
						{
							value =  strContent.GetFormula ("<" + String.strCropValue (value, "<", ">") + ">").Text;
							if ( value.IndexOf ("[")==-1 && value.IndexOf ("]")==-1 ) 
								value = "'" + value + "'";
						}
						string F = key+":"+value;
						if(!string.IsNullOrEmpty(formulastring))
							formulastring += ",";
						formulastring += F;
					}
				}


				///*** Add Args By SubJson{}
				Service.Formula FormulaOutPut = Service.Formula.Setup(formulastring);
				foreach (Service.Formula.FormulaData FD in FormulaOutPut.GetFormulaDatas) {
					Service.Formula.FormulaData FindJson = strContentJson.GetFormula(FD.Text);
					if (FindJson.isHave) 
					{
						//Debug.Log(FindJson.FormulaName);
						Service.Formula j =  JsonToFormula(FindJson.Text);
						foreach (Service.Formula.FormulaData jFD in j.GetFormulaDatas)
						{
							FD.AddArg(jFD.FormulaName, jFD.Text);
						}
					}
				}




				return FormulaOutPut;
			}
			public static List<Service.Formula>  JsonToFormulas( string json  ){
				List<Service.Formula> fomulas = new List<Formula> ();
				if (string.IsNullOrEmpty(json))
					return fomulas;
				json = json.Replace("'", Formula.single);
				List<string> values = strCropValues ( strCropValue(json,"[","]") , "{","}");
				foreach (string dataStr in values) 
				{
					fomulas.Add ( JsonToFormula(dataStr)  );
				}
				return fomulas;
			}
			*/
            #endregion
            public static bool isJsonArray(string raw , bool isDeserializeCheck = true)
			{
				string strInput = raw.Trim();
				if (strInput.StartsWith("[") && strInput.EndsWith("]"))
				{
					if (isDeserializeCheck)
					{
						var obj = ServiceJson.Json.DeserializeObject<object>(raw);
						return (obj.GetType() == typeof(ServiceJson.JsonArray));
					}
					else 
					{
						return true;
					}
				}
				else return false;
			}
			public static bool isJson(string raw , bool isDeserializeCheck = true)
			{
				string strInput = raw.Trim();
				if (strInput.StartsWith("{") && strInput.EndsWith("}"))
				{
					if (isDeserializeCheck)
					{
						var obj = ServiceJson.Json.DeserializeObject<object>(raw);
						return (obj.GetType() == typeof(ServiceJson.JsonObject));
					}
					else
					{
						return true;
					}

				}
				else return false;
			}
		}
		public class Dict{
			public static Service.Formula DictToFormula( Dictionary<string,string> dic ){
				Service.Formula f_dict = new Service.Formula( );
				if (dic != null)
					foreach (string key in dic.Keys) {
						double n; bool b;
						if (double.TryParse (dic [key], out n)) 
						{
							f_dict.AddFormula (key, n);
						}
						else if(bool.TryParse(dic[key], out b))
						{
							f_dict.AddFormula(key, b);
						}
						else 
						{
							f_dict.AddFormula (key, dic [key]);
						}
					}
				return f_dict;
			}
		}

	}
	#endregion












	#region Formula
	
	[System.Serializable]
	public class Formula : FormulaToolsService {
		public Formula() {
		
		}
		public Formula(string key,Formula f){
			AddFormula(key,f);
		}
		public Formula(string key, object obj)
		{
			AddFormula(key, obj);
		}
		public Formula(object serializable)
		{
			var f = Json.JsonToJFormula(serializable);
			AddFormulas(f);
		}
		[System.Serializable]
		public class FormulaData  {

			public enum datatype{
				none,num,nums,str,bol,vector,transform,enums,formula,obj,json,list, formulaData
			}
			public FormulaData(string name)
			{
				this.Name = name;
			}
			public FormulaData(string name,bool uni = false){
				this.Name = name;  
				if(uni)
					m_uniID = String.RandomUniKey(0,true);
			}
			public FormulaData(string name,string uni ){
				this.Name = name;  
				if(!string.IsNullOrEmpty(uni))
					m_uniID = uni;
			}
			public FormulaData(string name , object obj)
			{
				this.Name = name;
				if (obj != null)
				{
					isHave = true;
					var define = new Service.Tools.ObjectDefine(obj);
					var type =  Service.Tools.ObjectDefine.DefineType(obj);
					switch (type) {
						case datatype.str: Text				= define.String; break;
						case datatype.num: Value			= Service.Tools.ObjectDefine.NumericToDouble(obj); break;
						case datatype.bol: Status			= define.Bool; break;
						case datatype.formula: SubFormula	= (Formula) obj; break;
						case datatype.obj: Object			= obj; break;
					}
				}
			}



			public string uniID {get{ return m_uniID;}}
			private string m_uniID;

			public string FormulaName {get{ return Name;}}
			public void Rename (string newName){
				Name = newName;
			}

			[SerializeField]
			private string Name;
			[SerializeField]
			string m_Text;
			[SerializeField]
			double m_Value;
			[SerializeField]
			double[] m_Range;
			[SerializeField]
			bool m_Status;
			[SerializeField]
			object m_object;
			[SerializeField]
			Service.Formula m_formula;
			[SerializeField]
			Service.Formula m_litsformula;
			[SerializeField]
			public FormulaToolsService.LocalPackageContent LocalContent = new LocalPackageContent();


			string m_tag;
			//public string[] args = new string[10];
			public List<argsData> args = new List<argsData> ();
			public class argsData{
				public string argName = string.Empty;
				public string argData = null;
			}
			public string Text{
				get{ return  m_Text;}
				set{ m_datatype = datatype.str; m_Text = value;}
			}
			public string Json
			{
				get { return m_Text; }
				set { m_datatype = datatype.json; m_Text = value; }
			}
			public double Value{
				get{ return  m_Value;}
				set{ m_datatype = datatype.num; m_Value = value;}
			}
			public int ValueInt
			{
				get { return (int)m_Value; }
			}
			public double[] Range{
				get{ 
					if(m_Range==null || m_Range.Length == 0){
						m_Range = String.PassStringToRange(m_Text);
					}
					return  m_Range;
				}
				set{ m_datatype = datatype.nums; m_Range = value;}
			}
			public Service.Formula SubFormula{
				get{ return  m_formula;}
				set{ m_datatype = datatype.formula; m_formula = value;}
			}
			public Service.Formula ListFormulaDatas
			{
				get { return m_litsformula; }
				set { m_datatype = datatype.list; m_litsformula = value; }
			}
			public object Object{
				get{ return  m_object;}
				set{ m_datatype = datatype.obj; m_object = value;}
			}
			public bool Status{
				get{ return  m_Status;}
				set{ m_datatype = datatype.bol; m_Status = value;}
			}
			public object GetDataByType( ){
				if (m_datatype == datatype.str)		return  m_Text;
				if (m_datatype == datatype.num)		return  m_Value;
				if (m_datatype == datatype.bol)		return  m_Status;
				if (m_datatype == datatype.formula) return SubFormula;
				if (m_datatype == datatype.obj)		return m_object;
				if (m_datatype == datatype.list)	return ListFormulaDatas;
				return null;
			}
			public System.Enum Enum(object Default)
			{
				return String.ToEnum(Text, Default);
			}


		   [HideInInspector]
			public bool isHave = false;
			public bool Equals(string findname, bool isSensitive = true)
			{
				if(isSensitive)
					return (Name == findname);
				else
					return (Name.ToLower() == findname.ToLower());
			}
			datatype m_datatype = datatype.none;
			public datatype GetDataType{
				get{ return m_datatype; }
			}
			public FormulaData SetTag( string tagName){
				m_tag = tagName;
				return this;
			}
			public FormulaData SetUniID( string uniID){
				m_uniID = uniID;
				return this;
			}
			public string GetTag( ){
				return  m_tag;
			}

			public argsData AddArg(string argName , object argData){
				return AddArg (argName, (argData == null)?string.Empty: argData.ToString() );
			}
			public argsData AddArg(string argName , int argData){
				return AddArg (argName,argData.ToString());
			}
			public argsData AddArg(string argName , float argData){
				return AddArg (argName,argData.ToString());
			}
			public argsData AddArg(string argName , bool argData){
				return AddArg (argName,argData.ToString());
			}
			public argsData AddArg(string argName , string argData){
				argsData a = new argsData ();
				a.argName = argName;
				a.argData = argData;
				args.Add (a);
				return a;
			}
			public Service.String.StringDefine GetArg(string argName , bool isSensitive = true)
			{
				foreach (argsData arg in args)
				{
					if((isSensitive)?(arg.argName==argName):(arg.argName.ToLower()==argName.ToLower()))
					return Service.String.StringDefine.ToStringDefine(arg.argData);
				}
				return Service.String.StringDefine.ToStringDefine (string.Empty);
			}
			public bool UpdateArg(string argName , string value){
				foreach (argsData arg in args)
					if (arg.argName == argName) {
						arg.argData = value;
						return  true;
					}
				return false;
			}
			public void AddOrUpdateArg(string argName, object value)
			{
				foreach (argsData arg in args)
					if (arg.argName == argName)
					{
						arg.argData = value.ToString();
						return;
					}
				AddArg(argName, value.ToString());
			}
			public void AddArgs( List<argsData> argsDatas ){
				args.AddRange(argsDatas);
			}
			public void RemoveArg(string argName )
			{
				foreach (argsData arg in args)
					if (arg.argName == argName)
					{
						args.Remove(arg);
						return;
					}
			}
			public bool isHaveArg(string argName){
				foreach (argsData arg in args)
					if (arg.argName == argName)
						return  true;
				return false;
			}
			public void UpdateData( object value ){
				if (m_datatype == datatype.str)	 	m_Text = new Service.Tools.ObjectDefine (value).String;
				if(m_datatype == datatype.num)  	m_Value = new Service.Tools.ObjectDefine (value).Double;
				if(m_datatype == datatype.bol)  	m_Status = new Service.Tools.ObjectDefine (value).Bool;
				if (m_datatype == datatype.obj)		m_object = value;
			}
			public void UpdateData(Service.Formula f)
			{
				if (m_datatype == datatype.formula) SubFormula = f;
			}
			public void AddList(object value)
			{
				ListFormulaDatas.AddFormula(null, value);
			}
		}

		




		private string m_Nickname = string.Empty;
		public string Nickname{
			get
			{ 
				return m_Nickname;
			}
			set
			{ 
				m_Nickname = value;
			}
		}

		public FormulaToolsService.LocalPackageContent LocalContent = new LocalPackageContent();

		[SerializeField]
		List<FormulaData> FormulaDatas = new List<FormulaData>();
		public List<FormulaData> GetFormulaDatas{
			get{ 
				return FormulaDatas;
			}
		}
		public void Clear(){
			FormulaDatas.Clear ();
		}
		public FormulaData AddFormula(string f_name ,  string text){
			FormulaData fData = new FormulaData (f_name);
			fData.Text = text;
			fData.isHave = true;
			FormulaDatas.Add (fData);
			return fData;
		}
		public FormulaData AddFormula(string f_name ,  double value){
			FormulaData fData = new FormulaData (f_name);
			fData.Value = value;
			fData.isHave = true;
			FormulaDatas.Add (fData);
			return fData;
		}
		public FormulaData AddFormula(string f_name ,  bool status){
			FormulaData fData = new FormulaData (f_name);
			fData.Status = status;
			fData.isHave = true;
			FormulaDatas.Add (fData);
			return fData;
		}
		public FormulaData AddFormula(string f_name , Service.Formula f ){
			FormulaData fData = new FormulaData (f_name);
			fData.SubFormula = f;
			fData.isHave = true;
			FormulaDatas.Add (fData);
			return fData;
		}
		public FormulaData AddFormula(FormulaData formulaData) {
			formulaData.isHave = true;
			FormulaDatas.Add (formulaData);
			return formulaData;
		}
		public FormulaData AddFormula(string f_name ,  object obj){
			FormulaData fData = new FormulaData (f_name, obj );
			if(fData.isHave) FormulaDatas.Add(fData);
			return fData;
		}
		public FormulaData AddFormula(string f_name, object obj, FormulaData.datatype type)
		{
			FormulaData fData = new FormulaData(f_name);
			if (obj != null)
			{
				switch (type)
				{
					case FormulaData.datatype.str:	fData.Text = (string)obj; break;
					case FormulaData.datatype.json: fData.Json = (string)obj; break;
					case FormulaData.datatype.num:	fData.Value = Tools.ObjectDefine.NumericToDouble(obj); break;
					case FormulaData.datatype.bol:	fData.Status = (bool)obj; break;
					case FormulaData.datatype.obj: fData.Object = obj; break;
				}
				fData.isHave = true;
				FormulaDatas.Add(fData);
			}
			return fData;
		}
		public void AddFormulas(Formula formulaData){
			if (formulaData == null)
				return;
			AddFormulas(formulaData.GetFormulaDatas);
		}
		public void AddFormulas(List<Formula.FormulaData> formulaDatas)
		{
			if (formulaDatas == null)
				return;
			foreach (Service.Formula.FormulaData data in formulaDatas ){
				if (data.isHave){
					FormulaDatas.Add(data);
				}
			}
		}
		public void AddAndUpdateFormulas(Formula formulaData){
			if (formulaData == null)
				return;
			foreach (Service.Formula.FormulaData data in formulaData.GetFormulaDatas) {
				if (data.isHave) {
					Service.Formula.FormulaData FD =  GetFormula (data.FormulaName);
					if (FD.isHave) 
					{
						FD.UpdateData ( data.GetDataByType() );
					}
					else
						FormulaDatas.Add (data);
				}
			}
		}
		public FormulaData AddOrUpdateFormula(FormulaData formulaData){
			for (int i = 0; i < FormulaDatas.Count; i++){
				if (FormulaDatas[i].FormulaName == formulaData.FormulaName)
				{
					FormulaDatas[i] = formulaData;
					return formulaData;
				}
			}
			AddFormula(formulaData);
			return formulaData;
		}
		public FormulaData AddOrUpdateFormula(string f_name ,  object obj){
			FormulaData fData = GetFormula(f_name);
			if (GameObj.isObjectNotNull(obj)) {
				if (fData != null) {
					if (fData.isHave) 
					{
						if (fData.GetDataType == FormulaData.datatype.num) fData.Value = Tools.ObjectDefine.NumericToDouble(obj);
						if (fData.GetDataType == FormulaData.datatype.bol) fData.Status = (bool)obj;
						if (fData.GetDataType == FormulaData.datatype.str) fData.Text = (string)obj;
						if (fData.GetDataType == FormulaData.datatype.formula) fData.SubFormula = (Service.Formula)obj;
						if (fData.GetDataType == FormulaData.datatype.obj) fData.Object = obj;
					}
					else fData = AddFormula (f_name,obj);
				}
				else fData = AddFormula (f_name,obj);
			}
			return fData;
		}
		public FormulaData AddOrUpdateFormula(string f_name, Formula obj)
		{
			FormulaData fData = GetFormula(f_name);
			if (GameObj.isObjectNotNull(obj))
			{
				if (fData != null){
					if (fData.isHave){
						fData.SubFormula = obj;
					}
					else fData = AddFormula(f_name, obj);
				}
				else fData = AddFormula(f_name, obj);
			}
			return fData;
		}
		public FormulaData AddList(string f_name)
		{
			FormulaData fData = new FormulaData(f_name);
			fData.ListFormulaDatas = new Formula();
			fData.isHave = true;
			FormulaDatas.Add(fData);
			return fData;
		}
		public void DesFormula(string f_name){
			FormulaData f = GetFormula(f_name);
			if (f.isHave) {
				FormulaDatas.Remove (f);
			}
		}
		public void DesFormula(FormulaData f){
			if (f.isHave) {
				FormulaDatas.Remove (f);
			}
		}
		public void  DesFormulas(Service.Formula f_remove){
			foreach (FormulaData fd in f_remove.GetFormulaDatas) {
				DesFormula (fd.FormulaName);
			}
		}
		public Service.Formula  DesFormulasAndCopy(Service.Formula f_remove){
			Service.Formula copy = new Formula ();
			foreach (FormulaData fd in FormulaDatas) {
				if(!f_remove.GetFormula(fd.FormulaName).isHave)
					copy.AddFormula (fd);
			}
			return copy;
		}
		public List<FormulaData> GetTag(string TagName){
			List<FormulaData> tag = new List<FormulaData> ();
			foreach (FormulaData formula in FormulaDatas) {
				if(formula.GetTag() == TagName)
					tag.Add(formula);
			}
			return tag;
		}
		public Formula SplitTag(string TagName){
			Service.Formula f = new Formula();
			foreach (FormulaData fd in GetTag(TagName))
			{
				f.AddFormula(fd);
			}
			return f;
		}
		public List<FormulaData> GetAllisHaveArg(string ArgName){
			List<FormulaData> args = new List<FormulaData> ();
			foreach (FormulaData formula in FormulaDatas) {
				if(formula.isHaveArg(ArgName))
					args.Add(formula);
			}
			return args;
		}
		public FormulaData GetFormula(string FormulaName , bool isSensitive = true )
		{
			foreach (FormulaData formula in FormulaDatas) {
				if (formula.Equals (FormulaName, isSensitive)) {
					return formula;
				}
			}
			return new FormulaData(string.Empty);
		}
		public FormulaData GetFormula(string FormulaName,string tag){
			foreach (FormulaData formula in FormulaDatas) {
				if ((formula.Equals (FormulaName))&&(formula.GetTag() == tag )) {
					return formula;
				}
			}
			return new FormulaData(string.Empty);
		}
		public List<FormulaData> GetFormulas(string FormulaName){
			List<FormulaData> fDatas = new List<FormulaData> ();
			foreach (FormulaData formula in FormulaDatas) {
				if (formula.Equals (FormulaName)) {
					fDatas.Add (formula);
				}
			}
			return fDatas;
		}
		public Json Json { get { return new FormulaToolsService.Json(this); } }




		public void Shuffle(  ){
			for (int i = 0; i < FormulaDatas.Count; i++) {
				FormulaData temp = FormulaDatas[i];
				int randomIndex = Random.Range(i, FormulaDatas.Count);
				FormulaDatas[i] = FormulaDatas[randomIndex];
				FormulaDatas[randomIndex] = temp;
			}
		}
		public Formula Clone() {
			string json = FormulaToolsService.Json.FormulaToJson(this);
			return FormulaToolsService.Json.JsonToJFormula(json);
		}

		public enum FormatSetup
		{
			standard,multiargument,multitag
		}
		public static Formula Setup(string Datas , FormatSetup format = FormatSetup.standard){ 
			if (string.IsNullOrEmpty (Datas))
				return new Formula ();
			try
			{
				switch(format)
				{
				case FormatSetup.standard 		: return setup_standard(Datas); 		break;
				case FormatSetup.multiargument 	: return setup_multiargument(Datas); 	break;
				case FormatSetup.multitag 		: return setup_multitag(Datas); 		break;
				}
			}
			catch(System.Exception e)
			{
				Debug.LogError ("Formula Can't Setup : {"+Datas+"}" + "\n\n\n\n" + e.StackTrace );
			}
			return new Formula ();
		}
		#region Setup-Standard
		/// text:'hello world',index:5,enable:false
		/// 
		///		[output]
		///     formuladata.name = text
		/// 	formuladata.text = "hello world";
		/// 
		/// 	formuladata.name = index
		/// 	formuladata.value = 5;
		/// 
		/// 	formuladata.name = enable
		/// 	formuladata.bool = false;

		static Formula setup_standard (string Datas){ 
			Formula formula = new Formula ();
			string NewJson = Datas;
			List<string> values = String.strCropValues ( NewJson , "'","'");
			int index = 0;
			foreach (string dataStr in values) 
			{
				string newValue = dataStr.Replace(",","<&comma>");
				NewJson = NewJson.Replace (   dataStr , newValue );
			}
			string[] splitData = NewJson.Split (',');
			foreach (string KeyValue in splitData) {
				if (!string.IsNullOrEmpty (KeyValue)) {
					Formula.FormulaData FData = ToFormulaData (KeyValue.Replace("<&comma>",","));
					if (FData != null)
						formula.AddFormula (FData);
				}
			}
			return formula;
		}
		#endregion
		#region Setup-MultiArgument
		//Resource={COIN:50,GEM:10};Item={itemID:pocket44}

		///		[output]
		///     formuladata.name = Resource
		/// 	formuladata.value = Count;
		/// 	formuladata.arg[0] = coin:50
		/// 	formuladata.arg[1] = gem:10
		/// 
		/// 	formuladata.name = Item
		/// 	formuladata.value = Count;
		/// 	formuladata.arg[0] = itemID:pocket44

		static Formula setup_multiargument(string Datas){ 
			Formula formula = new Formula ();
			formula.GetFormulaDatas.Clear ();
			if (string.IsNullOrEmpty (Datas))
				return formula;
			try{
				string NewJson = Datas;
				string[] splitData = NewJson.Split (';');
				foreach (string KeyValue in splitData) {
					if (!string.IsNullOrEmpty (KeyValue)) 
					{
						int count = 0;
						string[] formulaDataValue = KeyValue.Split ('=');
						Service.Formula.FormulaData FData = formula.AddFormula(formulaDataValue[0],count);
						foreach (string argument in String.strCropValue ( formulaDataValue[1] , "{","}").Split(',')) {
							string[] argumentData = argument.Split (':');
							FData.AddArg(argumentData[0],argumentData[1]);
							count++;
						}
						FData.Value = count;
					}
				}
			}catch(System.Exception e)
			{
				Debug.LogError ("Formula Can't Setup : " + e.StackTrace + "\n\n\n\n" + "{ "+ Datas +" }");
			}
			return formula;
		}
		#endregion
		#region Setup-MultiTag
		//Resource={COIN:50<percent:50|damage:2>,GEM:10};Item={itemID:'pocket44'}

		///		[output]
		///     formuladata.name 	= coin
		/// 	formuladata.value 	= 50;
		/// 	formuladata.tag 	= Resource
		/// 	formuladata.arg[0]  = percent:50
		/// 	formuladata.arg[1]  = damage:2
		/// 
		/// 	formuladata.name 	= gem
		/// 	formuladata.value 	= 50;
		/// 	formuladata.tag 	= Resource
		/// 
		/// 	formuladata.name 	= itemID
		/// 	formuladata.text 	= pocket44;
		/// 	formuladata.tag 	= Item

		static Formula setup_multitag(string Datas){ 
			Formula formula = new Formula ();
			formula.GetFormulaDatas.Clear ();
			if (string.IsNullOrEmpty (Datas))
				return formula;
			try{
				string NewJson = Datas;
				string[] splitData = NewJson.Split (';');
				foreach (string KeyValue in splitData) {
					if (!string.IsNullOrEmpty (KeyValue)) 
					{
						string[] formulaDataValue = KeyValue.Split ('=');
						string tag = formulaDataValue[0];
						string contents = formulaDataValue[1];
						foreach (string FD in String.strCropValue ( contents , "{","}").Split(',')) 
						{

							//res:True<arg><args_A>111</args_A>&<args_A>111</args_A></arg>,
							//itembag:'pocket44'<percent:5>
							string data = FD;
							string arg = string.Empty;
							if ( data.IndexOf ("<") != -1 && data.IndexOf (">") != -1){
								arg = String.strCropValue (FD, "<", ">");
								if (!string.IsNullOrEmpty (arg))
									data = data.Replace ("<"+arg+">", "");
							}
							
							Service.Formula.FormulaData FData = ToFormulaData( data ) ;
							if (FData != null)
							{
								formula.AddFormula(  FData ).SetTag(tag);
								if (!string.IsNullOrEmpty (arg))
									foreach (string a in arg.Split('|')) 
									{
										string[] argsData = a.Split(':');
										FData.AddArg(argsData[0],argsData[1]);	
									}
							}
						}
					}
				}
			}catch(System.Exception e)
			{
				Debug.LogError ("Formula Can't Setup : " + e.StackTrace + "\n\n\n\n" + "{ "+ Datas +" }");
			}
			return formula;
		}
		#endregion
		public static string single = "<#key-single#>";
		static Formula.FormulaData ToFormulaData(string KeyValue){
			//baspoo:50
			//test:'ccc'

			//**Remove Text 'message' to the Data FirstStep ==> You Can Enter All Char || String to the Formula Ok..!
			string removeText = KeyValue;
			string text = string.Empty;
			if (removeText.IndexOf ("'") != -1) {
				text = String.strCropValue (removeText, "'", "'");
				if (!string.IsNullOrEmpty (text))
					removeText = removeText.Replace ("'"+text+"'", "''");
				else
					text = string.Empty;
			}
			//**-----------------------------------------------------------------------------------------------------
			string args = string.Empty;
			if (removeText.IndexOf ("<arg>") != -1) {
				args = String.strCropValue (removeText, "<arg>", "</arg>");
				if (!string.IsNullOrEmpty (args))
					removeText = removeText.Replace ("<arg>" + args + "</arg>", "");
			}
			//**-----------------------------------------------------------------------------------------------------
			string uniID = string.Empty;
			if (removeText.IndexOf ("<uniID>") != -1) {
				uniID = String.strCropValue (removeText, "<uniID>", "</uniID>");
				if (!string.IsNullOrEmpty (uniID))
					removeText = removeText.Replace ("<uniID>" + uniID + "</uniID>", "");
			}
			//**-----------------------------------------------------------------------------------------------------
			string range = string.Empty;
			if (removeText.IndexOf ("[") != -1) 
			if (removeText.IndexOf ("]") != -1) {
				range = String.strCropValue (removeText, "[", "]");
				if (!string.IsNullOrEmpty (range))
					removeText = removeText.Replace (range, "");
			}
			//**-----------------------------------------------------------------------------------------------------



			//Debug.Log (KeyValue);
			string[] splitKeyValue = removeText.Split (':');
			if (splitKeyValue.Length >= 2)
			if (!string.IsNullOrEmpty (splitKeyValue [0]))
			if (!string.IsNullOrEmpty (splitKeyValue [1])) {

				//CleanKey
				string key = splitKeyValue[0];
				key = String.RemoveSpecialCharater (key);
				FormulaData fData = new FormulaData (key);

				//** String
				if (!string.IsNullOrEmpty(text)) {
					text = text.Replace(single,"'");
					fData.Text = text; //function_string.strCropValue (splitKeyValue [1], '\'', '\'');
					fData.isHave = true;
				} else {
					//** Bool
					if ((splitKeyValue [1].ToLower ().IndexOf ("true") != -1) || (splitKeyValue [1].ToLower ().IndexOf ("false") != -1)) 
					{
								bool valuebool;
								bool isBool = bool.TryParse(splitKeyValue[1], out valuebool);
								if (isBool)
								{
									fData.Status = valuebool;
									fData.isHave = true;
								}
					} 
					//** Double
					else {
						//**Doubles / Rang [0/1] / [2/3/4/5/8/9.....]
						if (!string.IsNullOrEmpty (range)) {

							/*
							string[] r = range.Split ('/');
							fData.Range = new double[r.Length];
							for (int i = 0 ; i < r.Length ; i++) {
								fData.Range[i] = System.Convert.ToDouble (r[i]);
							}
							fData.isHave = true;
							*/

							fData.Range = String.PassStringToRange (range);
							fData.isHave = true;

						} 
						//**Double
						else {
									double valueDouble = 0;
									bool isDouble =	double.TryParse(splitKeyValue[1],out valueDouble);
									if (isDouble) {
										fData.Value = valueDouble;
										fData.isHave = true;
									}
						}
					}
				}
				//** Tag
				if (splitKeyValue.Length >= 3) {
					if (!string.IsNullOrEmpty (splitKeyValue [2]))
						fData.SetTag (splitKeyValue[2]);
				}
				//**Agus
				if (!string.IsNullOrEmpty (args)) {
					string[] args_split = args.Split ('&');
					foreach (string arg in args_split) {
						string argName = String.strCropValue (arg, "</args_", ">");
						string begin = "<args_" + argName + ">";
						string end = "</args_" + argName  + ">";
						if (args.IndexOf (begin) != -1) {
							string argData = String.strCropValue (args, begin , end );
							fData.AddArg (argName,argData);
						}
					}
				}
				//**uniID
				if (!string.IsNullOrEmpty (uniID)) {
					fData.SetUniID (uniID);
				}


				return fData;
			}
			return null;
		}
		public string PassToString()
		{
			string str = "";
			foreach (FormulaData formula in FormulaDatas)
			{
				string data = "";
				//** Main
				if (formula.GetDataType == FormulaData.datatype.str) data = formula.FormulaName + ":'" + formula.Text.Replace("'", single) + "'";
				if (formula.GetDataType == FormulaData.datatype.num) data = formula.FormulaName + ":" + formula.Value.ToString();
				if (formula.GetDataType == FormulaData.datatype.bol) data = formula.FormulaName + ":" + formula.Status.ToString();
				if (formula.GetDataType == FormulaData.datatype.nums)
				{
					/*
					string sum = "[";
					for (int i = 0; i < formula.Range.Length; i++) {
						if (i != 0)
							sum += "/";
						sum += formula.Range [i].ToString ();
					}
					sum += "]";
					*/
					string sum = String.PassRangeToString(formula.Range);
					data = formula.FormulaName + ":" + sum;
				}



				//** Tag
				if (!string.IsNullOrEmpty(formula.GetTag())) data += ":" + formula.GetTag();

				//** Args
				string args = string.Empty;
				if (formula.args.Count != 0)
				{
					foreach (FormulaData.argsData arg in formula.args)
					{
						if (arg.argData != null)
						{
							if (args != string.Empty) args += "&";
							args += "<args_" + arg.argName + ">" + arg.argData + "</args_" + arg.argName + ">";
						}
					}
				}
				if (args != string.Empty)
				{
					data += "<arg>" + args + "</arg>";
				}


				//** uniID
				if (!string.IsNullOrEmpty(formula.uniID))
					data += "<uniID>" + formula.uniID + "</uniID>";



				//**Add data to datas[all].
				if (!string.IsNullOrEmpty(data))
				{
					str += data + ",";
				}
			}
			return str;
		}
		public void PassToClass(  object obj ){
			foreach(Service.Formula.FormulaData fd in GetFormulaDatas){
				Var.ToClass (obj,fd.FormulaName, fd.GetDataByType() );
			}
		}
		public string PassToJson(Service.Formula meta = null)
		{
			return Json.FormulaToJson( this , meta );
		}
		public void SaveToLocal(string StoreName, string FormulaThisName){
			LocalStore local = new LocalStore(StoreName);
			local.Save(FormulaThisName, this);
		}
	}





	#endregion





	#region IEnumerator
	public class IEnume  {
		public delegate void IEnumeratorCallback ( );
		public delegate void IEnumeratorCallbackLoop ( int index);
		public delegate void IEnumeratorWWWCallback ( WWW www = null );

		public static AddOn.IEnume Wait(float waiting , IEnumeratorCallback callback){
			AddOn.IEnume addon = Tools.AddIEnume();
			addon.Waitting (waiting,callback);			
			return addon;
		}
		public static AddOn.IEnume StartCorotine( IEnumerator corotime , GameObject root = null){
			AddOn.IEnume addon = Tools.AddIEnume(root);
			addon._StartCorotine (corotime);		
			return addon;
		}
		public static AddOn.IEnume WaitLoop(float waiting , int Count , IEnumeratorCallbackLoop callback  ){
			return WaitLoop (waiting,Count,callback,0.0f);
		}
		public static AddOn.IEnume WaitLoop(float waiting , int Count , IEnumeratorCallbackLoop callback , float plusTimePerRound){
			AddOn.IEnume addon = Tools.AddIEnume();
			addon.WaittingLoop (waiting,Count,callback,plusTimePerRound);
			return addon;
		}
		public static AddOn.IEnume WWW(string url , IEnumeratorWWWCallback www){
			AddOn.IEnume addon = Tools.AddIEnume();
			addon.internetWWW (url,www);
			return addon;
		}
	}
	#endregion
	 
	#region Timemer
	public class Timmer  {
		public delegate void TimmerCallback (   );
		public delegate void TimmerCallbackLoop ( int index);
		public delegate void TimmerCallbackInfinity ( AddOn.Timmer time);

		public static  AddOn.Timmer Find(string ID){
			foreach (AddOn.Timmer time in FindObjectsOfType<AddOn.Timmer> ().ToList()) {
				if (time.ID == ID)
					return time;
			}
			return null;
		}
		public static  void StopAndDelete(string ID){
			AddOn.Timmer time = Find (ID);
			if (time != null)
				time.StopAndDelete ();
		}
		public static void StopAndDeleteAll(string ID)
		{
			foreach (AddOn.Timmer time in FindObjectsOfType<AddOn.Timmer>().ToList())
			{
				if ( time!=null && time.ID == ID)
				{
					time.StopAndDelete();
				}
			}
		}
		public static  void ForceFinish(string ID){
			AddOn.Timmer time = Find (ID);
			if (time != null)
				time.ForceFinish ();
		}

		public static AddOn.Timmer Wait(float waiting  , TimmerCallback callback = null , bool isIgnoreTimeScale=false){
			AddOn.Timmer addon = Tools.AddTimmer ();
            addon.isIgnoreTimeScale = isIgnoreTimeScale;
            addon.TimmerStart (waiting ,callback);
			return addon;
		}
		public static AddOn.Timmer Wait(float waiting  ,GameObject root , TimmerCallback callback = null, bool isIgnoreTimeScale = false)
        {
			AddOn.Timmer addon = Tools.AddTimmer (root);
            addon.isIgnoreTimeScale = isIgnoreTimeScale;
            addon.TimmerStart (waiting ,callback);
			return addon;
		}
		public static AddOn.Timmer WaitInfinity(float waiting ,GameObject root , TimmerCallbackInfinity callback = null, bool isIgnoreTimeScale = false)
        {
			AddOn.Timmer addon = Tools.AddTimmer (root);
            addon.isIgnoreTimeScale = isIgnoreTimeScale;
            addon.TimmerInfinityStart (waiting,callback);
			return addon;
		}
		public static AddOn.Timmer Wait(float waiting ,int Round , TimmerCallbackLoop callback = null, bool isIgnoreTimeScale = false)
        {
			AddOn.Timmer addon = Tools.AddTimmer ();
            addon.isIgnoreTimeScale = isIgnoreTimeScale;
            addon.TimmerRoundStart (waiting,Round,callback);
			return addon;
		}
		public static AddOn.Timmer Wait(float waiting ,int Round  , GameObject root , TimmerCallbackLoop callback = null, bool isIgnoreTimeScale = false)
        {
			AddOn.Timmer addon = Tools.AddTimmer (root);
            addon.isIgnoreTimeScale = isIgnoreTimeScale;
            addon.TimmerRoundStart (waiting,Round,callback);
			return addon;
		}
		public static AddOn.Timmer WaitDestoryActive ( GameObject gameobjectDestory , TimmerCallback callback = null, bool isIgnoreTimeScale = false)
		{
			return WaitInfinity(0.0f, null , (r)=> {
				r.roundRunning = 0;
				if (!Service.GameObj.isObjectNotNull(gameobjectDestory)) {
					r.StopAndDelete();
					if (callback != null)
						callback();
				}
			});
		}
		public static AddOn.Timmer WaitStep( GameObject root  = null, bool isIgnoreTimeScale = false)
        {
			AddOn.Timmer addon = Tools.AddTimmer (root);
            addon.isIgnoreTimeScale = isIgnoreTimeScale;
            addon.TimmerStep ();
			return addon;
		}
	}
	#endregion



	#region Time
	public class Time {



		public class TimeServer
		{
			public static TimeServer master = new TimeServer();

			long dif;
			public void Init(System.DateTime TimeServerNow) {
				//ex1. 50100 - 50000 = 100
				//ex2. 50100 - 50200 = -100
				dif = (DateTimeToUnixTimeStamp(TimeServerNow) - DateTimeToUnixTimeStamp(System.DateTime.Now));
			}

			public System.DateTime Time {
				get {
					//ex1
					// server - client
					// 50100 - 50000 = 100
					// Dif = 100
					// 100 + 50000 = 50100

					//ex2
					// server - client
					// 50100 - 50200 = -100
					// Dif = -100
					// -100 + 50200 = 50100

					//---- Time Run --------------------
					//Now 50000----->51200

					//ex1
					// Dif = 100
					// 100 + 51200 = 51300
					long t_now = DateTimeToUnixTimeStamp(System.DateTime.Now) + dif;
					System.DateTime dtDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
					return dtDateTime.AddSeconds(t_now);
				}
			}
			public long UnixTime
			{
				get { return DateTimeToUnixTimeStamp(Time); }
			}

			public bool isTimeOut(System.DateTime Timeout) {
				return Service.Time.isTimeOut(Time, Timeout);
			}
			public bool isTimeOut(long Timeout) {
				return DateTimeToUnixTimeStamp(Time) >= Timeout;
			}
			public AddOn.Timmer CountDown(GameObject root, System.DateTime Timeout, Service.Callback.callback_data runtime, Service.Callback.callback timeout = null)
			{
				var timer = Service.Timmer.WaitInfinity(0.0f, root, (r) => {
					r.roundRunning = 0;
					string timetext = Service.Time.DateTimeSubtract(Timeout, Time, WatchType.HMS);
					runtime(timetext);
					if (isTimeOut(Timeout))
					{
						r.StopAndDelete();
						if (timeout != null)
							timeout();
					}
				});
				return timer;
			}

		}


		/// <summary>
		/// Check only day and month (ignore year) by long
        /// Default time is time from server...
		/// </summary>
		public static bool isInPeriodDay(long Start, long End, bool defaultOnNull)
		{
			if (Start == 0 || End == 0)
				return defaultOnNull;
			else
				return isInPeriodDay(TimeServer.master.Time, UnixTimeStampToDateTime(Start), UnixTimeStampToDateTime(End));
		}


		/// <summary>
		/// Check datetime (ignore year)
		/// </summary>
		public static bool isInPeriodDay(System.DateTime Now, System.DateTime Start, System.DateTime End)
        {	
			System.DateTime startThisyear = Start.AddYears(Now.Year - Start.Year);
			System.DateTime endThisyear = End.AddYears(Now.Year - End.Year);

			return isInPeriodTime(Now, startThisyear, endThisyear);


		}

		/// <summary>
		/// Check datetime stamp by long
		/// Default time is time from server...
		/// </summary>
		public static bool isInPeriodTime(long Start,long End, bool defaultOnNull)
		{
			if (Start == 0 || End == 0)
				return defaultOnNull;
			else
				return isInPeriodTime(TimeServer.master.Time, UnixTimeStampToDateTime(Start), UnixTimeStampToDateTime(End));
		}

		/// <summary>
		/// Check datetime stamp
		/// </summary>
		public static bool isInPeriodTime(System.DateTime Now, System.DateTime Start, System.DateTime End)
		{
			if (Now >= Start && Now <= End)
				return true;
			else
				return false;
		}


		public static bool isTimeOut(System.DateTime Now, System.DateTime Timeout) {
			return DateTimeToUnixTimeStamp(Now) >= DateTimeToUnixTimeStamp(Timeout);
		}
		public static HashSet<string> HStr = new HashSet<string>() { "bas", "best", "jom" };


		public static long DateTimeToUnixTimeStamp( System.DateTime datetime , bool isLocalTime = false)
        {
            if (isLocalTime) return (long)(datetime.Subtract(DateTime1970.ToLocalTime())).TotalSeconds;
            else return (long)(datetime.Subtract(DateTime1970)).TotalSeconds;
        }
		public static System.DateTime UnixTimeStampToDateTime( long unixTimeStamp , bool isLocalTime = false)
		{
			// Unix timestamp is seconds past epoch
			System.DateTime dtDateTime = new System.DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
            if (isLocalTime)
                dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            else
                dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);

            return dtDateTime;
		}
		public enum WatchType
		{
			H,HM,HMS,HMSms
		}
		public static string UnixTimeStampToWatch( long unixTimeStamp ,WatchType watchType)
		{
			// Unix timestamp is 00:00:00
			System.TimeSpan t = System.TimeSpan.FromSeconds( (double)unixTimeStamp);
			string timerFormatted = "";
			if( watchType == WatchType.HMSms) 		timerFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", (int)t.TotalHours, t.Minutes, t.Seconds, t.Milliseconds);
			else if( watchType == WatchType.HMS) 	timerFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)t.TotalHours, t.Minutes, t.Seconds, t.Milliseconds);
			else if( watchType == WatchType.HM) 	timerFormatted = string.Format("{0:D2}:{1:D2}", (int)t.TotalHours, t.Minutes, t.Seconds, t.Milliseconds);
			else 									timerFormatted = string.Format("{0:D2}", (int)t.TotalHours, t.Minutes, t.Seconds, t.Milliseconds);
			return timerFormatted;
		}
		public static string DateTimeSubtract( System.DateTime max , System.DateTime min ,WatchType watchType){
			return UnixTimeStampToWatch( DateTimeToUnixTimeStamp(max)-DateTimeToUnixTimeStamp(min) , watchType);
		}
        public static System.TimeSpan DateTimeSubtractTimeSpan(System.DateTime max, System.DateTime min)
        {
            return System.TimeSpan.FromSeconds((double)DateTimeToUnixTimeStamp(max) - DateTimeToUnixTimeStamp(min));
        }


        public static System.DateTime DateTime1970 {
			get {
				return new System.DateTime (1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			}
		}
		public static bool IsDateTime1970 (System.DateTime dateTime) {
			return (System.TimeZoneInfo.ConvertTimeToUtc (dateTime) - DateTime1970).TotalSeconds == 0;
		}

		/// <summary>
		/// ex: 2017-01-14 11:31:44
		/// </summary>
		public static System.DateTime DateTimeFromString (string str) {
			System.DateTime myDate = System.DateTime.ParseExact (str, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
			return myDate;
		}
	}

	#endregion




	#region Loop
	public class Loop  {
		public static void For( int round , Service.Callback.callback_value callback ){
			for (int i = 0; i < round; i++) {
				callback (i);
			}
		}
	}
	#endregion






	#region GameObj
	public class GameObj : MonoBehaviour {
		public static GameObject Created(  GameObject _page ){
			return Created (_page,null);
		}
		public static GameObject Created(  GameObject _page , Transform _parent){
			GameObject p = Instantiate(_page) as GameObject;
			p.transform.parent 			= _parent;
			p.transform.localPosition 	= Vector3.zero;
			p.transform.localScale 		= Vector3.one;
			return p;
		}
		public static Object Created(Object _page, Transform _parent)
		{
			Object obj = Instantiate(_page);
			GameObject p = ((Behaviour)obj).gameObject;
			p.transform.parent = _parent;
			p.transform.localPosition = Vector3.zero;
			p.transform.localScale = Vector3.one;
			return obj;
		}
		public static List<T> Createds <T>(GameObject p , Transform root , int count )
		{
			List<T> list = new List<T>();
			for (int i = 0; i < count; i++)
			{
				var g = Service.GameObj.Created(p , root);
				list.Add(g.GetComponent<T>());
			}
			return list;
		}
		public static void ResetTransform(Transform transform) {
			if (transform != null) {
				transform.localPosition = Vector3.zero;
				transform.localScale 	= Vector3.one;
				transform.localRotation = Quaternion.identity;
			}
		}
		public static void LookAtTransform( Transform self,Transform look){
			if (self != null && look!=null ) {
				self.transform.position = look.transform.position;
				self.transform.localScale = look.transform.localScale;
				self.transform.rotation = look.transform.rotation;
			}
		}
		public static Behaviour CreatedComponent(  GameObject _page  ) {
			return CreatedComponent (_page,null);
		}
		public static Behaviour CreatedComponent(  GameObject _page , Transform _parent) {
			GameObject p = Instantiate(_page) as GameObject;
			p.transform.parent 			= _parent;
			p.transform.localPosition 	= Vector3.zero;
			p.transform.localScale 		= Vector3.one; 

			return p.GetComponent<Behaviour>();
		}
		
		public static GameObject CreatedPerfabReference(  GameObject _page , Transform _parent){
			GameObject p = Instantiate(_page) as GameObject;
			p.transform.parent 			= _parent;
			p.transform.localPosition 	= _page.transform.localPosition;
			p.transform.localScale	 	= _page.transform.localScale;
			p.transform.localRotation 	= _page.transform.localRotation;
			return p;
		}
		public static void DesAllParent( Transform _tran){
			int count = _tran.childCount;
			for (int n = 0; n < count; n++)
				Destroy (_tran.GetChild(n).gameObject );
		}
		public static List<GameObject> GetAllParent( Transform _tran){
			List<GameObject> m_objs = new List<GameObject> ();
			int count = _tran.childCount;
			for (int n = 0; n < count; n++)
				m_objs.Add (_tran.GetChild(n).gameObject );
			return m_objs;
		}
		public static GameObject GetTransform( Transform _tran , string name){
			foreach(GameObject g in GetAllParent(_tran) ){
				if (g.name == name)
					return g;
			}
			return null;
		}
		public static List<GameObject> GetAllNode( Transform _tran){
			List<GameObject> m_objs = new List<GameObject> ();
			int count = _tran.childCount;
			for (int n = 0; n < count; n++) {
				
				m_objs.Add (_tran.GetChild (n).gameObject);
				m_objs.AddRange (GetAllNode(_tran.GetChild (n)));
			}
			return m_objs;
		}
		public static GameObject GetAllNode( Transform _tran , string findname){
        
			return GetAllNode (_tran).Find(x=>x.gameObject.name == findname);
		}
		public static void Parent(  GameObject gameobj , Transform parent_to_transform ){
			gameobj.transform.parent = parent_to_transform;
			ResetTransform(gameobj.transform);
		}

		public static void ReLayer(GameObject root, GameObject toLayer) {
			if (isObjectNotNull(toLayer)) 
				ReLayer(root,toLayer.layer);
		}
		public static void ReLayer( GameObject root , int toLayer)
		{
			if (isObjectNotNull(root))
			{
				root.layer = toLayer;
				foreach (GameObject g in Service.GameObj.GetAllNode(root.transform))
				{
					g.layer = toLayer;
				}
			}
		}


		public static bool isObjectNotNull(object obj) {
			if (obj == null)
				return false; // the reference actually is null --> early exit
			if ((obj is UnityEngine.Object) && (obj.Equals(null)))
				return false;  // the object is a fake-null object --> early exit
			// now savely use "obj"
			return true;
		}



	}
	#endregion









	#region Calculate
	public class Cal
	{
		public static int CountOfFill( int count , int maxline ) 
		{
			int fill = maxline - (count % maxline);
			if (fill == maxline)
			{
				fill = 0;
			}
			return fill;
		}
	}
    #endregion











    #region Image
    public class Image : MonoBehaviour {
		public enum ImageDimensionType{
			Hight,Width
		}
		public static Texture2D SpriteToTexture(Sprite spr){
			return spr.texture;
		}
		public static string ByteToBase64(Texture2D Img){
			byte[] isByte = Img.EncodeToPNG ();
			return  System.Convert.ToBase64String (isByte);
		}
		public static Sprite TextureToSprite(		Texture2D texture2d 	, float pixelsPerunit){
			Rect rec = new Rect(0, 0, texture2d.width, texture2d.height);
			Sprite.Create(texture2d,rec,new Vector2(0,0),1);
			Sprite sprite = Sprite.Create(texture2d,rec,new Vector2(0.5f,0.5f), pixelsPerunit );
			return sprite;
		}

		public static Texture2D AtlasTextures (string name, List<Texture2D> textures)
		{
			// determine your size from sprites
			int width = 0;
			int height = 0;
			int textureWidthCounter = 0;
			foreach (Texture2D t in textures) {
				width += t.width;
				// determine the height
				if (t.height > height)
					height = t.height;
			}
			// make your new texture
			Texture2D atlas = new Texture2D (width, height, TextureFormat.RGBA32, false);
			// loop through your textures
			for (int i = 0; i < textures.Count; i++) {
				int y = 0;
				while (y < atlas.height) {
					int x = 0;
					while (x < textures [i].width) {
						if (y < textures [i].height) {
							// fill your texture
							atlas.SetPixel (x + textureWidthCounter, y, textures [i].GetPixel (x, y));
						} else {
							// add transparency
							atlas.SetPixel (x + textureWidthCounter, y, new Color (0f, 0f, 0f, 0f));
						}
						++x;
					}
					++y;
				}
				atlas.Apply ();
				textureWidthCounter += textures [i].width;
			}
			return atlas;
		}


		static public Texture2D RenderTextureToTexture2D (RenderTexture rt , bool isGenerateMipmap)
		{
			// Remember currently active render texture
			RenderTexture currentActiveRT = RenderTexture.active;

			// Set the supplied RenderTexture as the active one
			RenderTexture.active = rt;

			// Create a new Texture2D and read the RenderTexture image into it
			Texture2D tex = new Texture2D(rt.width, rt.height,TextureFormat.RGBA32,isGenerateMipmap,false);
			tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
			tex.Apply ();
			// Restorie previously active render texture
			RenderTexture.active = currentActiveRT;
			return tex;
		}
		//#endif
		public static Texture2D CameraToTexture2D (Camera camera)
		{
			// The Render Texture in RenderTexture.active is the one
			// that will be read by ReadPixels.
			var currentRT = RenderTexture.active;
			RenderTexture.active = camera.targetTexture;

			// Render the camera's view.
			camera.Render();

			// Make a new texture and read the active Render Texture into it.
			Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
			image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
			image.Apply();

			// Replace the original active Render Texture.
			RenderTexture.active = currentRT;
			return image;
		}

		public static Texture2D ToGray (Texture2D tex2D)
		{
			// Should be moved out of HBallManager since it's also used for Surface Textures
			Texture2D grayTex = new Texture2D (tex2D.width, tex2D.height);
			float grayScale;
			float alpha;
			for (int y = 0; y < tex2D.height; ++y) {
				for (int x = 0; x < tex2D.width; ++x) {
					grayScale = tex2D.GetPixel (x, y).r * 0.21f + tex2D.GetPixel (x, y).g * 0.71f + tex2D.GetPixel (x, y).b * 0.07f;
					alpha = tex2D.GetPixel (x, y).a;
					grayTex.SetPixel (x, y, new Color (grayScale, grayScale, grayScale, alpha));
				}
			}
			grayTex.Apply ();
			return grayTex;
		}


	}
	#endregion



	#region Quaternion
	public class RotateTools  {
		public enum AnglesType
		{
			Clamp360,Euler180
		}
		public static Vector3 ToAngles( Quaternion q , AnglesType anglesType){
			Vector3 r = q.eulerAngles;
			if(anglesType == AnglesType.Euler180) return new Vector3 (C360To180Format(r.x),C360To180Format(r.y),C360To180Format(r.z));
			else return new Vector3 (C180To360Format(r.x),C180To360Format(r.y),C180To360Format(r.z));
		}
		public static float C360To180Format( float input ){
			float output = 0.0f;
			if (input <= 180.0f) 
				output = input;
			else 
				output = (180.0f - (input-180) )*-1.0f;
			return output;
		} 
		public static float C180To360Format( float eulerAngles ){
			float result = eulerAngles - Mathf.CeilToInt(eulerAngles / 360f) * 360f;
			if (result < 0)
				result += 360f;
			return result;
		} 
	}
	#endregion

	public class Vector
	{
		public static Vector3 RandomVector(float min, float max)
		{
			return RandomVector(Vector3.zero, Random.RandomRange(min,max), Random.RandomRange(min, max), Random.RandomRange(min, max));
		}
		public static Vector3 RandomVector(float x , float y , float z)
		{
			return RandomVector(Vector3.zero , x,y,z);
		}
		public static Vector3 BetweenRandomVector(Vector3 a , Vector3 b)
		{
			Vector3 vec = Vector3.zero;
			vec.x = Random.RandomRange(a.x , b.x);
			vec.y = Random.RandomRange(a.y , b.y);
			vec.z = Random.RandomRange(a.z , b.z);
			return vec;
		}
		public static Vector3 RandomVector(Vector3 root ,float x, float y, float z)
		{
			return new Vector3(
				root.x + UnityEngine.Random.RandomRange(-x, x),
				root.y + UnityEngine.Random.RandomRange(-y, y),
				root.z + UnityEngine.Random.RandomRange(-z, z)
				);
		}
	}


	#region Colour
		public class Colour  {
		public static string ToRGBHex(Color c , bool H = true)
		{
			if(H)return string.Format("#{0:X2}{1:X2}{2:X2}", ToByte(c.r), ToByte(c.g), ToByte(c.b));
			else return string.Format("{0:X2}{1:X2}{2:X2}", ToByte(c.r), ToByte(c.g), ToByte(c.b));
		}
		private static byte ToByte(float f)
		{
			f = Mathf.Clamp01(f);
			return (byte)(f * 255);
		}
	}
	#endregion




	#region File
	public class File  {

		//** FullPart = /Doc/imagebachup/baspoo 
		//** filter = ".png"
		public static FileInfo[] GetFileInfo( string FullPart , string filter)
		{
			if (System.IO.Directory.Exists (FullPart)) {
				FullPart = FullPart + Path.DirectorySeparatorChar;
				DirectoryInfo info = new DirectoryInfo(FullPart);

				//** filter cut string  ex =   "*.png"
				string m_filter = string.Empty;
				if (!string.IsNullOrEmpty (filter))
					m_filter = "*" + filter; 


				FileInfo[] files = info.GetFiles(m_filter).OrderByDescending(p => p.CreationTime).ToArray();
				return files;
			}
			return null;
		}
	}
	#endregion



}