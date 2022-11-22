using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Networking;
using System.Security.Cryptography;
using System.Text;

/// <summary>
///    ** Game Master Service by 'Baspoo'
/// </summary>
public class Service : MonoBehaviour 
{
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
		public delegate void callback_formula( Formula f );
		public delegate int callback_value_return( int value );
        public delegate double callback_double_return(int value);
        public delegate string callback_data_return( string data );
	}

	








	public class Permanence
	{
		public Permanence(string key) { this.key = key; }
		string key;
		bool m_init = false;

		public void Clear() {
			PlayerPrefs.DeleteKey(key);
		}

		public bool isHas => PlayerPrefs.HasKey(key);

		bool m_bool;
		public bool getBool
		{
			get
			{
				if (!m_init)
				{
					m_bool = save_bool;
					m_init = true;
				}
				return m_bool;
			}
			set
			{
				m_bool = value;
				save_bool = value;
			}
		}
		bool save_bool
		{
			get { return PlayerPrefs.GetInt(key) == 1; }
			set { PlayerPrefs.SetInt(key, value ? 1 : 0); }
		}


		string m_string;
		public string getString
		{
			get
			{
				if (!m_init)
				{
					m_string = save_string;
					m_init = true;
				}
				return m_string;
			}
			set
			{
				m_string = value;
				save_string = value;
			}
		}
		string save_string
		{
			get { return PlayerPrefs.GetString(key); }
			set { PlayerPrefs.SetString(key, value); }
		}


		int m_int;
		public int getInt
		{
			get
			{
				if (!m_init)
				{
					m_int = save_int;
					m_init = true;
				}
				return m_int;
			}
			set
			{
				m_int = value;
				save_int = value;
			}
		}
		int save_int
		{
			get { return PlayerPrefs.GetInt(key); }
			set { PlayerPrefs.SetInt(key, value); }
		}
	}







	public class Var{


		[System.Serializable]
		public class Val
		{
			public string Key;
			public double Value;
		}

		#region Var-Transforms
		[System.Serializable]
		public class Transforms
		{
			[SerializeField]
			List<Trans> _Transforms = new List<Trans>();

			public Transform this[int index] { get { return _Transforms[index].transform; } }
			public Transform this[string key] { get { return Get(key); } }

			[System.Serializable]
			public class Trans
			{
				public string name;
				public Transform transform;
			}
			public void Add(string varName, Transform tran)
			{
				Trans t = new Trans();
				t.name = varName;
				t.transform = tran;
				_Transforms.Add(t);
			}
			public void Remove(string varName, bool isDestoryObjectInTrans = false)
			{
				Trans find = null;
				foreach (Trans t in _Transforms)
				{
					if (t.name == varName)
						find = t;
				}
				if (find != null)
				{
					_Transforms.Remove(find);
					if (isDestoryObjectInTrans)
						Destroy(find.transform.gameObject);
				}
			}
			public void RemoveAll(bool isDestoryObjectInTrans = false)
			{
				if (isDestoryObjectInTrans)
					foreach (Service.Var.Transforms.Trans t in _Transforms)
						Destroy(t.transform.gameObject);
				_Transforms.Clear();
			}
			public Transform Get(string varName)
			{
				foreach (Trans t in _Transforms)
				{
					if (t.name == varName)
					{
						return t.transform;
					}
				}
				return null;
			}
			public List<Trans> Gets
			{
				get { return _Transforms; }
			}
			public Transform Active(string varName, bool isActive)
			{
				foreach (Trans t in _Transforms)
				{
					if (t.name == varName)
					{
						t.transform.gameObject.SetActive(isActive);
						return t.transform;
					}
				}
				return null;
			}
			public int Count => _Transforms.Count;
			public List<Trans> Contents => _Transforms;
		}
		#endregion
		#region Var-Values
		[System.Serializable]
		public class Values
		{
			[SerializeField]
			List<Value> _Values = new List<Value>();
			public double this[int index] { get { return _Values[index].value; } }
			public double this[string key] { get { return Get(key); } }


			[System.Serializable]
			public class Value
			{
				public string name;
				public double value;
			}
			public List<Value> ValueList
			{
				get { return _Values; }
			}
			public void Add(string varName, double value)
			{
				Value newValue = new Value();
				newValue.name = varName;
				newValue.value = value;
				_Values.Add(newValue);
			}
			public double Get(string varName)
			{
				foreach (Value t in _Values)
				{
					if (t.name == varName)
						return t.value;
				}
				return 0;
			}
			public void Set(string varName, double newValue)
			{
				foreach (Value t in _Values)
				{
					if (t.name == varName)
						t.value = newValue;
				}
			}
			public int Count => _Values.Count;
			public List<Value> Contents => _Values;
		}
		#endregion
		#region Var-Texture
		[System.Serializable]
		public class Textures
		{
			[SerializeField]
			List<TextureData> _Texture = new List<TextureData>();
			public Texture this[int index] { get { return _Texture[index].img; } }
			public Texture this[string key] { get { return Get(key); } }

			[System.Serializable]
			public class TextureData
			{
				public string name;
				public Texture img;
			}
			public void Add(string varName, Texture texture)
			{
				_Texture.Add(new TextureData() { name = varName, img = texture });
			}
			public Texture Get(string varName)
			{
				foreach (TextureData t in _Texture)
				{
					if (t.name == varName)
						return t.img;
				}
				return null;
			}
			public int Count => _Texture.Count;
			public List<TextureData> Contents => _Texture;
		}
		#endregion
		#region Var-String
		[System.Serializable]
		public class Strings
		{
			[SerializeField]
			List<String> _Strings = new List<String>();
			public string this[int index] { get { return _Strings[index].text; } }
			public string this[string key] { get { return Get(key); } }


			[System.Serializable]
			public class String
			{
				public string name;
				public string text;
			}
			public string Get(string varName)
			{
				foreach (String t in _Strings)
				{
					if (t.name == varName)
						return t.text;
				}
				return string.Empty;
			}
			public void Add(string varName, string text)
			{
				_Strings.Add(new String() { name = varName, text = text });
			}
			public void Remove(string varName)
			{
				_Strings.RemoveAll(x => x.name == varName);
			}
			public int Count => _Strings.Count;
			public List<String> Contents => _Strings;
		}
		#endregion
		#region Var-Color
		[System.Serializable]
		public class Colors
		{
			[SerializeField]
			List<_Color> _Colors = new List<_Color>();
			public Color this[int index] { get { return _Colors[index].color; } }
			public Color this[string key] { get { return Get(key); } }


			[System.Serializable]
			public class _Color
			{
				public string name;
				public Color color;
			}
			public Color Get(string varName)
			{
				foreach (_Color t in _Colors)
				{
					if (t.name == varName)
						return t.color;
				}
				return Color.white;
			}
			public void Add(string varName, Color color)
			{
				_Colors.Add(new _Color() { name = varName, color = color });
			}
			public int Count => _Colors.Count;
			public List<_Color> Contents => _Colors;
		}
		#endregion
		#region Var-Behaviour
		[System.Serializable]
		public class Behaviours
		{
			[SerializeField]
			List<behaviour> _Behaviours = new List<behaviour>();
			public Behaviour this[int index] { get { return _Behaviours[index].Class; } }
			public Behaviour this[string key] { get { return Get(key); } }


			[System.Serializable]
			public class behaviour
			{
				public string name;
				public Behaviour Class;
			}
			public Behaviour Get(string varName)
			{
				foreach (behaviour t in _Behaviours)
				{
					if (t.name == varName)
						return t.Class;
				}
				return null;
			}
			public void Add(string varName, Behaviour Class)
			{
				_Behaviours.Add(new behaviour() { name = varName, Class = Class });
			}
			public int Count => _Behaviours.Count;
			public List<behaviour> Contents => _Behaviours;
		}
		#endregion
		#region Var-Class
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
				else if (field.FieldType.IsEnum)
				{
					//var val = System.Enum.ToObject(field.FieldType, _value);
					//var val = System.Convert.ChangeType( enumtype , field.FieldType);
					int index = System.Convert.ToInt32(_value);
					string enumname = System.Enum.GetName(field.FieldType, index );
					var enumtype = System.Enum.Parse(field.FieldType, enumname ,false);
					field.SetValue( _class , enumtype );
				}
				else if (field.FieldType == typeof(Dictionary<string,string>) && _value.GetType() == typeof(Formula))
				{
					var f = (Formula)_value;
					var values = ServiceJson.Json.DeserializeObject<Dictionary<string, string>>(f.PassToJson());
					field.SetValue(_class, values);
				}
				else if (field.FieldType == typeof(Dictionary<string, double>) && _value.GetType() == typeof(Formula))
				{
					var f = (Formula)_value;
					var values = ServiceJson.Json.DeserializeObject<Dictionary<string, double>>(f.PassToJson());
					field.SetValue(_class, values);
				}
				else if (field.FieldType == typeof(Dictionary<double, string>) && _value.GetType() == typeof(Formula))
				{
					var f = (Formula)_value;
					var values = ServiceJson.Json.DeserializeObject<Dictionary<double, string>>(f.PassToJson());
					field.SetValue(_class, values);
				}
				else if (field.FieldType == typeof(Dictionary<double, double>) && _value.GetType() == typeof(Formula))
				{
					var f = (Formula)_value;
					var values = ServiceJson.Json.DeserializeObject<Dictionary<double, double>>(f.PassToJson());
					field.SetValue(_class, values);
				}
				else if (field.FieldType == typeof(System.Double[]))
				{
					var listformula = (Formula)_value;
					int index = 0;
					double[] output = new double[listformula.GetFormulaDatas.Count];
					foreach (var d in listformula.GetFormulaDatas) {
						output[index] = d.Value;
						index++;
					}
					field.SetValue(_class, output );
				}
				else 
				{
					if (!field.FieldType.IsGenericType && !field.FieldType.IsArray)
						field.SetValue(_class, _value);
					else 
					{
						Debug.LogError($"{_key} : {field.FieldType} <-> {_value.GetType()} - {field.FieldType.IsGenericType}-{field.FieldType.IsArray}");
					}
				}
			}
		}
		#endregion
	}








	#region Tools
	public class Tools{
		static GameObject m_gameservice;


		static ServiceRunTime m_serviceRunTime;
		public static ServiceRunTime serviceRunTime
		{
			get
			{
				if (gameservice)
					return m_serviceRunTime;
				return null;
			}
			private set { m_serviceRunTime = value; }
		}
		public static GameObject gameservice
		{
			get
			{
				if (m_gameservice == null)
				{
					m_gameservice = new GameObject("#GameService");
					serviceRunTime = m_gameservice.AddComponent<ServiceRunTime>();
					GameObject.DontDestroyOnLoad(m_gameservice);
				}
				return m_gameservice;
			}
		}
		public class ServiceRunTime : MonoBehaviour
		{
			public Coroutine OnStartCorotine(IEnumerator ienumerator)
			{
				return StartCoroutine(ienumerator);
			}
			public void OnStopCorotine(Coroutine coroutine)
			{
				if (coroutine != null)
					StopCoroutine(coroutine);
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
		static AddOn.IEnume IEnume;
		public static AddOn.IEnume AddIEnume()
		{
			if (IEnume == null) 
			{
				IEnume = gameservice.AddComponent<AddOn.IEnume>();
			}
			return IEnume;
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
			public void Open(string contentName)
			{
				if(trans.Count != 0)
				{
					var t = trans.Find(x => x.name == contentName);
					Open(t);
				}
			}
            #endregion


			[SerializeField]
			[Header("Find by Transform")]
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
			public delegate void callback(Formula f = null, Service.Callback.callback onfinish = null );
			public class FunctionCallingData{
				public callback Function;
				public string Name;
				public bool IsCallOnecTime;
				public string Tag;
			}
			public List<FunctionCallingData> 	Functions = new List<FunctionCallingData>();
			public void Add(FunctionCallingData data)
			{
				Functions.Add(data);
			}
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
			public void Call(string functionName , Formula formula = null,Service.Callback.callback onfinish = null){
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
			public void CallAll( Formula formula = null,Service.Callback.callback onfinish = null){
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
			public void CallAll(string tag, Formula formula = null, Service.Callback.callback onfinish = null)
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
			public int Int { get{ if (m_obj == null)	return 0; else return System.Convert.ToInt32(m_obj); } }
			public double Double { get{ if (m_obj == null)	return 0; else return (double) m_obj; } }
			public float Float { get{ if (m_obj == null)	return 0.0f; else return (float) m_obj; } }
			public long Long { get { if (m_obj == null) return 0; else return (long)m_obj; } }
			public bool Bool { get{ if (m_obj == null)	return false; else return (bool)System.Convert.ToBoolean(m_obj); } }
			public  Formula Formula { get { return new Formula(m_obj);  } }
			public object Object { get { return m_obj; } }

			public bool IsHave => m_obj != null;


			public static Formula.FormulaData.datatype DefineType(object obj) {
				if (obj.GetType() == typeof(string)) return Formula.FormulaData.datatype.str;
				else if (Service.Tools.ObjectDefine.IsNumeric(obj)) return Formula.FormulaData.datatype.num;
				else if (obj.GetType() == typeof(bool)) return Formula.FormulaData.datatype.bol;
				else if (obj.GetType() == typeof(Formula)) return Formula.FormulaData.datatype.formula;
				else if (obj.GetType() == typeof(Formula.FormulaData)) return Formula.FormulaData.datatype.formulaData;
				else if (obj.GetType() == typeof(List<Formula.FormulaData>)) return Formula.FormulaData.datatype.list;
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
			NameValidate = NameValidate.Replace(" ", "");
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
		public static string IUnikey(string head ="I", int ch = 4)
		{
			string main = Random.Range(00, 99).ToString();
			string date = Service.Time.DateTimeToUnixTimeStamp(System.DateTime.Now).ToString();
			string mid = "";
			for (int i = 0; i < ch; i++)
				mid += String.RandomString(String.TypeWriter.up);

			string key = $"{head}-{main}-{mid}-{date}";
			return key;
		}
		public static string UniSimple( int count = 6 )
		{
			string key = "";
			for (int i = 0; i < count; i++) 
			{
				string add = "";
				if (Random.RandomRange(0, 100) < 40)
				{
					key += Random.Range(0, 9).ToString();
				}
				else 
				{
					key += String.RandomString(String.TypeWriter.up);
				}
			}
			return key;
		}
		public static string RandomUniKey( int ch ,bool isHeader = true){
			string key = string.Empty;
			if(isHeader)
					key = "unikey_"+
						  "r_" + Random.Range (11111, 99999).ToString () +
			              "d_" + Service.Time.DateTimeToUnixTimeStamp (System.DateTime.Now).ToString () +
			              "c_";
			for (int i = 0; i < ch; i++) 
			{
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
				foreach (char c in str.ToCharArray()) 
				{
					i.Add ((int)c);
				}
			}
			return i;
		}
		public static int StringToTotalASCII(string str)
		{
			int sum = 0;
			if (!string.IsNullOrEmpty(str))
			{
				foreach (char c in str.ToCharArray())
				{
					
					sum += (int)c;
				}
			}
			return sum;
		}
		/// <summary>
		/// Str = (0.0f,0.0f,0.0f)
		/// </summary>
		public static Vector3 PassStringToVector3(string Str){  
			Vector3 IsVec = Vector3.zero;
			if (string.IsNullOrEmpty (Str))
				return IsVec;
			try 
			{
				Str = strCropValue(Str, "(", ")");
				string[] Value = Str.Split(',');
				IsVec.x = Value[0].ToFloat();
				IsVec.y = Value[1].ToFloat();
				IsVec.z = Value[2].ToFloat();
				return IsVec;
			} 
			catch {
				return Vector3.zero;
			}
		}
		public static Vector2 PassStringToVector2(string Str){  
			Vector2 IsVec = Vector2.zero;
			if (string.IsNullOrEmpty (Str))
				return IsVec;
			try 
			{
				Str = strCropValue(Str, "(", ")");
				string[] Value = Str.Split(',');
				IsVec.x = Value[0].ToFloat();
				IsVec.y = Value[1].ToFloat();
				return IsVec;

			} 
			catch { return Vector2.zero; }
		}
		public static Vector2Int PassStringToVector2Int(string Str)
		{
			Vector2Int IsVec = Vector2Int.zero;
			if (string.IsNullOrEmpty(Str))
				return IsVec;
			try
			{
				Str = strCropValue(Str, "(", ")");
				string[] Value = Str.Split(',');
				IsVec.x = Value[0].ToInt();
				IsVec.y = Value[1].ToInt();
				return IsVec;
			}
			catch 
			{
				return Vector2Int.zero;
			}
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

		public static int Parse(string Str, int _default)
		{
			var output = _default;
			return (System.Int32.TryParse(Str, out output)) ? output : _default;
		}
		public static long Parse(string Str, long _default)
		{
			var output = _default;
			return (System.Int64.TryParse(Str, out output)) ? output : _default;
		}
		public static double Parse(string Str, double _default)
		{
			var output = _default;
			return (System.Double.TryParse(Str, out output)) ? output : _default;
		}
		public static float Parse(string Str, float _default)
		{
			var output = _default;
			return (System.Single.TryParse(Str, out output)) ? output : _default;
		}
		public static bool Parse(string Str, bool _default)
		{
			var output = _default;
			return (System.Boolean.TryParse(Str, out output)) ? output : _default;
		}
		public static byte Parse(string Str, byte _default)
		{
			var output = _default;
			return (System.Byte.TryParse(Str, out output)) ? output : _default;
		}
		public static System.DateTime Parse(string Str, System.DateTime _default)
		{
			var output = _default;
			return (System.DateTime.TryParse(Str, out output)) ? output : _default;
		}
		public static string Parse(string Str, string _default)
		{
			if (string.IsNullOrEmpty(Str))
				return _default;
			else
				return Str;
		}
		public enum formulaToType { statdard, json }
		public static Formula Parse(string Str, Formula _default, formulaToType type)
		{
			if (type == formulaToType.statdard)
				return Formula.TextSetup(Str);
			else
				return FormulaToolsService.Json.JsonToJFormula(Str);
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
		public static void To(string Str , out Formula result, formulaToType type = formulaToType.statdard)
		{  
			result = Parse(Str,new Formula(), type);
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
		public static bool isCheckHaveinString(string strFind, string strMessage)
		{
			if (string.IsNullOrEmpty(strMessage)) return false;
			else return (strMessage.IndexOf(strFind) != -1);
		}





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

		public static string ComputeSha256Hash(string rawData)
		{
			// Create a SHA256   
			using (System.Security.Cryptography.SHA256 sha256Hash = System.Security.Cryptography.SHA256.Create())
			{
				// ComputeHash - returns byte array  
				byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawData));

				// Convert byte array to a string   
				System.Text.StringBuilder builder = new System.Text.StringBuilder();
				for (int i = 0; i < bytes.Length; i++)
				{
					builder.Append(bytes[i].ToString("x2"));
				}
				return builder.ToString();
			}
		}


		private static byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
		static byte[] getKey(int keys)
		{
			//Debug.Log(keys);
			byte[] key = new byte[8];
			//var sign = keys/8;
			for (var i = 0; i < 8; i++)
			{
				var dd = keys / (i + 1);
				//Debug.Log(dd);
				key[i] = (byte)(dd);
			}
			return key;
		}
		public static string Encrypt(string text, int key)
		{
			byte[] keyByte = getKey(key);
			SymmetricAlgorithm algorithm = DES.Create();
			ICryptoTransform transform = algorithm.CreateEncryptor(keyByte, iv);
			byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
			byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
			return System.Convert.ToBase64String(outputBuffer);
		}

		public static string Decrypt(string text, int key)
		{
			byte[] keyByte = getKey(key);
			SymmetricAlgorithm algorithm = DES.Create();
			ICryptoTransform transform = algorithm.CreateDecryptor(keyByte, iv);
			byte[] inputbuffer = System.Convert.FromBase64String(text);
			byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
			return Encoding.Unicode.GetString(outputBuffer);
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
			public static Formula JsonToFormula(string json)
			{
				return FormulaToolsService.Json.JsonToJFormula(json);
			}
			public static List<Formula> JsonToFormulas(string json)
			{
				return FormulaToolsService.Json.JsonArrayToJFormulas(json);
			}
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
			public static Formula DictToFormula( Dictionary<string,string> dic ){
				Formula f_dict = new Formula( );
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
		public static Coroutine StartCorotine( IEnumerator corotime ){
			AddOn.IEnume addon = Tools.AddIEnume();
			return addon._StartCorotine (corotime);		
		}
		public static void StopCorotine(Coroutine corotime)
		{
			AddOn.IEnume addon = Tools.AddIEnume();
			if (corotime != null) addon.StopCoroutine(corotime);
		}

		public class CoroutineList { public string name; public Coroutine coro; }
		public static List<CoroutineList> CoroutineLists = new List<CoroutineList>();
		public static Coroutine StartCorotine(string name , IEnumerator corotime)
		{
			AddOn.IEnume addon = Tools.AddIEnume();
			StopCorotine(name);
			var coro = addon._StartCorotine(corotime);
			CoroutineLists.Add( new CoroutineList() { name = name , coro  = coro } );
			return coro;
		}
		public static void StopCorotine(string corotime)
		{
			var find = CoroutineLists.Find(x=>x.name == corotime);
			if (find!=null) 
			{
				StopCorotine(find.coro);
			}
			CoroutineLists.RemoveAll(x => x.coro == null);
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




	//
	public class Task 
	{
		public static void RunJob(System.Action job, System.Action callback) {
			AddOn.IEnume addon = Tools.AddIEnume();
			addon.StartCoroutine(runtime(job, callback));
		}
		static IEnumerator runtime(System.Action job, System.Action callback) 
		{
			bool isDone = false;
			var threadsave = new System.Threading.Thread(() => {
				job?.Invoke();
				isDone = true;
			});
			threadsave.Start();
			while (!isDone)
			{
				yield return null;
			}
			threadsave.Abort();
			callback?.Invoke();
		}
	}
	





	#region Timemer
	public class Timmer  {


		public class Update 
		{
			public bool IsPause = false;
			public float Runtime = 0.0f;
			System.Action m_timeout;
			public void OnUpdate(float max = 1.0f , float speed = 1.0f , System.Action timeout = null ) 
			{
				m_timeout = timeout;
				if (IsPause)
					return;

				if (Runtime < max)
				{
					Runtime += UnityEngine.Time.deltaTime * speed;
				}
				else 
				{
					Done();
				}
			}
			public void Reset( )
			{
				Runtime = 0.0f;
			}
			public void Done()
			{
				m_timeout?.Invoke();
				Runtime = 0.0f;
			}
		}





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
			public TimeServer(bool promoteToMaster = false) { if (promoteToMaster) master = this; }
			//bool thisTimelocal = false;
			public static TimeServer master = new TimeServer();

			long dif;
			// If TimeServerNow == UniversalTime Not LocalTo
			// Pls.. TimeServerNow.ToLocalTime();

			public void Init(System.DateTime TimeServerNow ) {
				//this.thisTimelocal = thisTimelocal;
				//ex1. 50100 - 50000 = 100
				//ex2. 50100 - 50200 = -100

				long unix = DateTimeToUnixTimeStamp(TimeServerNow);
				dif = (unix - DateTimeToUnixTimeStamp(System.DateTime.Now));
			}
			public void Init(long unix)
			{
				//ex1. 50100 - 50000 = 100
				//ex2. 50100 - 50200 = -100
				dif = (unix - DateTimeToUnixTimeStamp(System.DateTime.UtcNow));
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
				get 
				{
					return DateTimeToUnixTimeStamp(Time,true);
				}
			}
		
			public bool isTimeOut(System.DateTime Timeout) {
				return Service.Time.isTimeOut(Time, Timeout);
			}
			public bool isTimeOut(long Timeout) {
				return UnixTime >= Timeout;
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
			int diffyear = Start.Year;
			if (Start.Year - End.Year != 0 || Start.Month > End.Month)
            {
				//** กรณีช่วงคาบเกี่ยวระหว่างปลายปีไปต้นปี
				if (Now.Month <= End.Month)
					diffyear = End.Year;
			}
			
			System.DateTime startThisyear = Start.AddYears(Now.Year - diffyear); 
			System.DateTime endThisyear = End.AddYears(Now.Year - diffyear);
			//Debug.Log("startThisyear " + startThisyear.ToString());
			//Debug.Log("endThisyear " + endThisyear.ToString());

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
			if (unixTimeStamp > 9999999999)
				dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp);
			else
				dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);

            return isLocalTime? dtDateTime.ToLocalTime() : dtDateTime;
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
		public static void For( int round , Service.Callback.callback_value callback )
		{
			for (int i = 0; i < round; i++) 
			{
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
			//AssetsBundleHandle.RefreshMaterial(p);
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
				//AssetsBundleHandle.RefreshMaterial(g);
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

			if(Application.isPlaying)

			for (int n = 0; n < count; n++)
				Destroy (_tran.GetChild(n).gameObject );

			else

				for (int n = 0; n < count; n++)
					DestroyImmediate(_tran.GetChild(n).gameObject);

		}
		public static List<GameObject> GetAllParent( Transform _tran){
			List<GameObject> m_objs = new List<GameObject> ();
			int count = _tran.childCount;
			for (int n = 0; n < count; n++)
				m_objs.Add (_tran.GetChild(n).gameObject );
			return m_objs;
		}
		public static List<T> GetAllParent<T>(Transform _tran)
		{
			return _tran.GetComponentsInChildren<T>().ToList<T>();
		}
		public static GameObject GetTransform( Transform _tran , string name){
			foreach(GameObject g in GetAllParent(_tran) ){
				if (g.name == name)
					return g;
			}
			return null;
		}
		public static List<T> GetAllNode<T>(Transform _tran)
		{
			List<T> list = new List<T>();
			foreach (GameObject g in GetAllParent(_tran))
			{
				var get = g.GetComponent<T>();
				if (get!=null)
					list.Add(get);
			}
			return list;
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
		public static void OnAllNodeActive(Transform root,string name,bool enable)
		{
			foreach (var p in Service.GameObj.GetAllNode(root))
			{
				if (p.name.IndexOf(name) != -1)
				{
					p.gameObject.SetActive(enable);
				}
			}
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
		public static Texture2D TextureToTexture2D( Texture texture)
		{
			Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
			RenderTexture currentRT = RenderTexture.active;
			RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
			Graphics.Blit(texture, renderTexture);

			RenderTexture.active = renderTexture;
			
			texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
			texture2D.Apply();

			RenderTexture.active = currentRT;
			RenderTexture.ReleaseTemporary(renderTexture);
			return texture2D;
		}
		public static Color32 AverageColorFromTexture(Texture tex) {
			return AverageColorFromTexture(TextureToTexture2D(tex));
		}
		public static Color32 AverageColorFromTexture(Texture2D tex)
		{
			Color32[] texColors = tex.GetPixels32();

			int total = texColors.Length;

			float r = 0;
			float g = 0;
			float b = 0;

			for (int i = 0; i < total; i++)
			{

				r += texColors[i].r;

				g += texColors[i].g;

				b += texColors[i].b;

			}

			return new Color32((byte)(r / total), (byte)(g / total), (byte)(b / total), 0);

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
		public static Vector3 RandomVector(Vector3 root, float min, float max)
		{
			return new Vector3(
				root.x + UnityEngine.Random.RandomRange(min, max),
				root.y + UnityEngine.Random.RandomRange(min, max),
				root.z + UnityEngine.Random.RandomRange(min, max)
				);
		}
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
		public static Color HexColor(string hexString)
		{
			Color actColor;
			ColorUtility.TryParseHtmlString(hexString,out actColor);
			return actColor;
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


		public static string CreateTextfile (string path , string data )
		{
			path = Application.dataPath + Path.DirectorySeparatorChar + path;
			Debug.Log(path);
			#if UNITY_EDITOR
			System.IO.File.WriteAllText(path, data);
			#endif
			return path;
		}


	}
	#endregion





	#region Net
	public class Net
	{
		public enum method {
			POST, GET
		}

		static NetBehaviour m_net;
		public static NetBehaviour net
        {
			get {
				if (m_net == null) 
				{
					m_net = Tools.gameservice.AddComponent<NetBehaviour>();
				}
				return m_net;
			}
		}

		public static NetBehaviour ApiRequest(method method, string url, WWWForm form, System.Action<UnityWebRequest> onComplete, System.Action<UnityWebRequestAsyncOperation> onOperation = null)
		{
			net.SendRequest(method, url, form, onComplete , onOperation);
			return net;
		}
		public static NetBehaviour ApiRequest(string url , System.Action<UnityWebRequest> onComplete, System.Action<UnityWebRequestAsyncOperation> onOperation = null)
		{
			net.SendRequest(method.GET , url, null , onComplete , onOperation);
			return net;
		}


		static Dictionary<string, Texture> imageDict = new Dictionary<string, Texture>();
		//public static void LoadImage(string imageUrl, System.Action<Texture> onComplete)
		//{
		//	if (!imageUrl.notnull())
		//	{
		//		onComplete?.Invoke(null);
		//		return;
		//	}


		//	if (imageDict.ContainsKey(imageUrl))
		//	{
		//		onComplete?.Invoke(imageDict[imageUrl]);
		//	}
		//	else 
		//	{
		//		net.SendRequest(method.GET, imageUrl, null, (uwr) => {
		//			if (uwr.isNetworkError || uwr.isHttpError)
		//			{
		//				onComplete?.Invoke(null);
		//			}
		//			else
		//			{
		//				Texture myTexture = DownloadHandlerTexture.GetContent(uwr);
		//				if (myTexture != null)
		//				{
		//					imageDict.Add(imageUrl, myTexture);
		//					onComplete?.Invoke(myTexture);
		//				}
		//				else onComplete?.Invoke(null);
		//			}
		//		});
		//	}
		//}
		public static void LoadImage(string imageUrl, System.Action<Texture> onComplete)
		{
			if (!imageUrl.notnull())
			{
				onComplete?.Invoke(null);
				return;
			}


			if (imageDict.ContainsKey(imageUrl))
			{
				onComplete?.Invoke(imageDict[imageUrl]);
			}
			else
			{
				net.SendWWW(imageUrl, (www) => {
					if (www.error.notnull())
					{
						onComplete?.Invoke(null);
					}
					else
					{
						Texture myTexture = www.texture;
						if (myTexture != null)
						{
							imageDict.Add(imageUrl, myTexture);
							onComplete?.Invoke(myTexture);
						}
						else onComplete?.Invoke(null);
					}
				});
			}
		}



		public class NetBehaviour : MonoBehaviour 
		{

			public void SendRequest(method method, string url, WWWForm form, System.Action<UnityWebRequest> onComplete , System.Action<UnityWebRequestAsyncOperation> onOperation = null)
			{
				StartCoroutine(_SendRequest(method, url, form, onComplete , onOperation));
			}
			IEnumerator _SendRequest(method method, string url, WWWForm form, System.Action<UnityWebRequest> onComplete, System.Action<UnityWebRequestAsyncOperation> onOperation = null)
			{
				string fullURL = url;
				UnityWebRequest request = null;
				switch (method)
				{
					case method.POST:
						request = UnityWebRequest.Post(fullURL, form);
						break;
					case method.GET:
						request = UnityWebRequest.Get(fullURL);
						break;
				}

				UnityWebRequestAsyncOperation op = request.SendWebRequest();
				while (op.isDone == false)
				{
					onOperation?.Invoke(op);
					yield return 0;
				}
				onComplete?.Invoke(request);
				request.Dispose();
			}



			public void SendWWW( string url , System.Action<WWW> onComplete )
			{
				StartCoroutine(_SendWWW(url, onComplete));
			}
			IEnumerator _SendWWW(string url, System.Action<WWW> onComplete)
			{
				WWW www = new WWW(url);
				yield return www;
				onComplete?.Invoke(www);
				www.Dispose();
			}
		}
		

	}
	#endregion

	



	public class Dict
	{
		public static void Add( Dictionary<string,int> dict , string key , int value)
		{
			if (!dict.ContainsKey(key)) dict.Add(key, value);
			else dict[key]+= value;
		}
		public static void Add(Dictionary<string, long> dict, string key, long value)
		{
			if (!dict.ContainsKey(key)) dict.Add(key, value);
			else dict[key] += value;
		}
		public static void Set(Dictionary<string, int> dict, string key, int value)
		{
			if (!dict.ContainsKey(key)) dict.Add(key, value);
			else dict[key] = value;
		}
		public static void Set(Dictionary<string, long> dict, string key, long value)
		{
			if (!dict.ContainsKey(key)) dict.Add(key, value);
			else dict[key] = value;
		}
		public static void Set(Dictionary<string, string> dict, string key, string value)
		{
			if (!dict.ContainsKey(key)) dict.Add(key, value);
			else dict[key] = value;
		}
		public static void Set(Dictionary<string, object> dict, string key, object value)
		{
			if (!dict.ContainsKey(key)) dict.Add(key, value);
			else dict[key] = value;
		}

		public static int Get(Dictionary<string, int> dict, string key)
		{
			if (dict.ContainsKey(key)) return dict[key];
			else return 0;
		}
		public static long Get(Dictionary<string, long> dict, string key)
		{
			if (dict.ContainsKey(key)) return dict[key];
			else return 0;
		}
		public static string Get(Dictionary<string, string> dict, string key)
		{
			if (dict.ContainsKey(key)) return dict[key];
			else return string.Empty;
		}
		public static object Get(Dictionary<string, object> dict, string key)
		{
			if (dict.ContainsKey(key)) return dict[key];
			else return null;
		}







	}
}












