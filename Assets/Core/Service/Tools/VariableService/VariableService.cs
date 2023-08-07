﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using CodeStage.AntiCheat.ObscuredTypes;























public enum SerializeHandle
{
	NullValue,          //--> skip null value on serialize.
	IgnoreUpperCase,    //--> ignore upper or lower variable name.
	IgnoreReadonly,    //--> ignore upper or lower variable name.
	ReplaceAll,        //--> replace default data in class.
	FormattingIndented  //--> adjust beautiful json format.

}
public static class SerializeService
{
	public class JsonPropertyNameResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
	{
		public bool ignoreUpercase = false;
		public bool ignoreReadonly = false;

		protected override string ResolvePropertyName(string propertyName)
		{
			//Change the incoming property name into Title case
			var name = ignoreUpercase ? string.Concat(propertyName[0].ToString().ToUpper(), propertyName.Substring(1).ToLower()) : propertyName;
			return base.ResolvePropertyName(name);
		}
		protected override IList<Newtonsoft.Json.Serialization.JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			IList<Newtonsoft.Json.Serialization.JsonProperty> props = base.CreateProperties(type, memberSerialization);
			return ignoreReadonly ? props.Where(p => p.Writable).ToList() : props;
		}
	}

	public static Newtonsoft.Json.JsonSerializerSettings gethandle(params SerializeHandle[] SerializeHandles)
	{
		var handle = new Newtonsoft.Json.JsonSerializerSettings();
		handle.Converters.Add(new ObscuredValueConverter());

		var resolver = new JsonPropertyNameResolver();
		handle.ContractResolver = resolver;

		foreach (var key in SerializeHandles)
		{
			if (key == SerializeHandle.NullValue) handle.NullValueHandling = NullValueHandling.Ignore;
			if (key == SerializeHandle.IgnoreUpperCase) resolver.ignoreUpercase = true;
			if (key == SerializeHandle.IgnoreReadonly) resolver.ignoreReadonly = true;
			if (key == SerializeHandle.ReplaceAll) handle.ObjectCreationHandling = ObjectCreationHandling.Replace;
			if (key == SerializeHandle.FormattingIndented) handle.Formatting = Formatting.Indented;
		}
		return handle;
	}

	public static string SerializeToJson(this object obj, params SerializeHandle[] SerializeHandles)
	{
		var handle = gethandle(SerializeHandles);
		return JsonConvert.SerializeObject(obj, handle);
	}
    public static T DeserializeObject<T>(this object obj, params SerializeHandle[] SerializeHandles)
    {
        var json = obj is string ? (string)obj : SerializeToJson(obj, SerializeHandles);
        return DeserializeObject<T>(json, SerializeHandles);
    }
    public static T DeserializeObject<T>(this string json, params SerializeHandle[] SerializeHandles)
	{
		var handle = gethandle(SerializeHandles);
		return JsonConvert.DeserializeObject<T>(json, handle);
	}
	public static object DeserializeObject(this string json, params SerializeHandle[] SerializeHandles)
	{
		var handle = gethandle(SerializeHandles);
		return JsonConvert.DeserializeObject(json, handle);
	}
	public static void DeserializeObject<T>(this object obj, System.Action<T> done, bool flatbufferFormatHandle = false)
	{
		if (obj != null)
		{
			var json = obj.SerializeToJson();
			if (json.notnull())
			{
				if (flatbufferFormatHandle && json.IsFlatBufferFormat())
				{
					json = json.ConvertFlatToJsObject();
				}
				var result = json.DeserializeObject<T>(SerializeHandle.IgnoreUpperCase);
				done?.Invoke(result);
			}
		}
	}
}













public static class VariableService
{


	#region String
	public static bool notnull(this string value)
	{
		return !string.IsNullOrEmpty(value);
	}
	public static bool isnull(this string value)
	{
		return string.IsNullOrEmpty(value);
	}
	public static int ToInt(this string str)
	{
		var output = 0;
		int.TryParse(str, out output);
		return output;
	}
	public static long ToLong(this string str)
	{
		var output = 0l;
		long.TryParse(str, out output);
		return output;
	}
	public static float ToFloat(this string str)
	{
		var output = 0f;
		float.TryParse(str, out output);
		return output;
	}
	public static double ToDouble(this string str)
	{
		var output = 0d;
		double.TryParse(str, out output);
		return output;
	}
	public static bool ToBool(this string str)
	{
		var output = false;
		bool.TryParse(str, out output);
		return output;
	}
	public static char GetFirst(this string str)
	{
		if (str.notnull() && str.Length > 0)
			return str[0];
		else
			return char.MinValue;
	}
	public static char GetLast(this string str)
	{
		if (str.notnull() && str.Length > 0)
			return str[str.Length - 1];
		else
			return char.MinValue;
	}
	public static bool IsJson(this string str)
	{
		return str.GetFirst() == '{' && str.GetLast() == '}';
	}
	public static bool IsJsonArray(this string str)
	{
		return str.GetFirst() == '[' && str.GetLast() == ']';
	}

	public static Vector2 ToVector2(this string str)
	{
		return Service.String.PassStringToVector2(str);
	}
	public static Vector2Int ToVector2Int(this string str)
	{
		return Service.String.PassStringToVector2Int(str);
	}
	public static Vector3 ToVector3(this string str)
	{
		return Service.String.PassStringToVector3(str);
	}
	public static Formula ToFormula(this string json)
	{
		return  Formula.Json.JsonToJFormula(json);
	}


	public static string ToHexString(this Color c) => Service.Colour.ToRGBHex(c);
	public static Color HexToColor(this string hexstring) => Service.Colour.HexColor(hexstring);
	public static Color ToBrightness(this Color color, float percent)
	{
		//** percent = -1 to Drak.
		//** percent = +1 to Bright.

		var colorOutput = color + (Color.white * percent);
		colorOutput.a = color.a;
		return colorOutput;
	}



	public static string ToStringComma(this string[] values)
	{
		return ToStringComma(values.ToList());
	}
	public static string ToStringComma(this List<string> values)
	{
		string str = string.Empty;
		values.ForEach(x => {
			if (string.IsNullOrEmpty(str)) str = x;
			else str += "," + x;
		});
		return str;
	}
	public static void SaveToLocal(this string str, string key)
	{
		PlayerPrefs.SetString(key, str);
	}
	public static string GetByLocal(this string key)
	{
		return PlayerPrefs.GetString(key);
	}
	public static void Copy(this string messge)
	{
		#if !UNITY_EDITOR && UNITY_WEBGL
				HtmlCallback.Copy(messge);
		#else
				TextEditor tx = new TextEditor();
				tx.text = messge;
				tx.SelectAll();
				tx.Copy();
		
		#endif
	}
	public static bool IsValidEmail(this string email)
	{
		var trimmedEmail = email.Trim();

		if (trimmedEmail.EndsWith("."))
		{
			return false; // suggested by @TK-421
		}
		try
		{
			var addr = new System.Net.Mail.MailAddress(email);
			return addr.Address == trimmedEmail;
		}
		catch
		{
			return false;
		}
	}
	#endregion



	#region ObscuredTypes
	public static int[] ToInt(this ObscuredInt[] datas)
	{
		int[] output = new int[datas.Length];
		for (int i = 0; i < datas.Length; i++)
			output[i] = datas[i];
		return output;
	}
	#endregion




	#region object
	public static int ToInt(this object obj)
	{
		if (obj == null) return 0;
		else return obj is int ? (int)obj : 0;
	}
	public static long ToLong(this object obj)
	{
		if (obj == null) return 0;
		else return obj is long ? (long)obj : 0;
	}
	public static float ToFloat(this object obj)
	{
		if (obj == null) return 0;
		else return obj is float ? (float)obj : 0;
	}
	public static double ToDouble(this object obj)
	{
		if (obj == null) return 0;
		else return obj is double ? (double)obj : 0;
	}
	public static bool ToBool(this object obj)
	{
		if (obj == null) return false;
		else return obj is bool ? (bool)obj : false;
	}
	public static string ToStr(this object obj)
	{
		if (obj == null) return string.Empty;
		else return obj is string ? (string)obj : string.Empty;
	}
	public static T ToEnum<T>(this object obj, object _default = null)
	{
		return ToEnum<T>(obj.ToStr(), _default);
	}
	public static T ToEnum<T>(this string obj, object _default = null)
	{
		try
		{
			var result = System.Enum.Parse(typeof(T), obj);
			return (T)result;
		}
		catch
		{
			return _default != null ? (T)_default : default;
		}
	}




	public static T To<T>(this object obj)
	{
		return obj.DeserializeObject<T>();
	}
	#endregion



	#region Number


	public static void Wait(this double time, System.Action done)
	{
		Service.Timmer.Wait((float)time, () => { done?.Invoke(); });
	}
	public static void IEWait(this double time, System.Action done)
	{
		Service.IEnume.Wait((float)time, () => { done?.Invoke(); });
	}
	public static AddOn.Timmer Wait(this int time, System.Action done)
	{
		return Service.Timmer.Wait(time, () => { done?.Invoke(); });
	}
	public static void IEWait(this int time, System.Action done)
	{
		Service.IEnume.Wait(time, () => { done?.Invoke(); });
	}
	public static AddOn.Timmer Wait(this float time, System.Action done)
	{
		return Service.Timmer.Wait(time, () => { done?.Invoke(); });
	}
	public static AddOn.Timmer Wait(this float time, GameObject root, System.Action done)
	{
		return Service.Timmer.Wait(time, root, () => { done?.Invoke(); });
	}
	public static void IEWait(this float time, System.Action done)
	{
		Service.IEnume.Wait(time, () => { done?.Invoke(); });
	}

	public static void Loop(this int max, System.Action<int> round)
	{
		for (int i = 0; i < max; i++)
		{
			round?.Invoke(i);
		}
	}
	public static void Loop(this int max, System.Action round)
	{
		for (int i = 0; i < max; i++)
		{
			round?.Invoke();
		}
	}
	public static int Min(this int i, int min) => (i < min) ? min : i;
	public static int Max(this int i, int max) => (i > max) ? max : i;
	public static double Max(this double i, double max) => (i > max) ? max : i;
	public static float Max(this float i, float max) => (i > max) ? max : i;
	public static bool IsEnough(this int value, int cost, System.Action action = null)
	{
		if (value >= cost)
		{
			action?.Invoke();
			return true;
		}
		else return false;
	}


	//
	static System.DateTime Date => Service.Time.TimeServer.master.Time;
	static long UnixTime => Service.Time.TimeServer.master.UnixTime;
	public static System.DateTime ToDateTime(this long unix, bool ToLocalTime = true) => Service.Time.UnixTimeStampToDateTime(unix, ToLocalTime);
	public static bool IsTimeout(this System.DateTime dateTime, long sec) => dateTime.AddMinutes(sec) < Date;
	public static bool IsTimeout(this long dateTime, long sec) => (dateTime + sec) < UnixTime;
	public static bool IsNewDay(this System.DateTime dateTime) => dateTime.Day != Date.Day;
	public static long ToUnix(this System.DateTime dateTime) => Service.Time.DateTimeToUnixTimeStamp(dateTime, true);

	public static int Random(this int i) => UnityEngine.Random.RandomRange(0, i);
	public static bool IsPercent(this int i) => UnityEngine.Random.RandomRange(0, 100) <= i;
	public static float Random(this float i) => UnityEngine.Random.RandomRange(0, i);

	public static int Random(this int[] i) => UnityEngine.Random.RandomRange(i[0], i[1]);
	public static float Random(this float[] i) => UnityEngine.Random.RandomRange(i[0], i[1]);


	public static string KiloFormat(this int num) => KiloFormat((long)num);
	public static string KiloFormat(this long num)
	{
		if (num >= 100000000)
			return (num / 1000000).ToString("#,0M");

		if (num >= 10000000)
			return (num / 1000000).ToString("0.#") + "M";

		if (num >= 100000)
			return (num / 1000).ToString("#,0K");

		if (num >= 10000)
			return (num / 1000).ToString("0.#") + "K";

		return num.ToString("#,0");
	}
	#endregion


	#region Tag
	static Dictionary<string, Dictionary<string, object>> tags = new Dictionary<string, Dictionary<string, object>>();
	public static void Tag(this object tag , string tagName) 
	{
		Tag(tag, "", tagName);
	}
	public static void Tag(this object tag, string category, string tagName)
	{
		if (tags.ContainsKey(category))
		{
			var currentCategory = tags[category];
			if (currentCategory.ContainsKey(tagName))
				currentCategory[tagName] = tag;
			else
				currentCategory.Add(tagName, tag);
		}
		else 
		{
			var newCategory = new Dictionary<string, object>();
			newCategory.Add(tagName, tag);
			tags.Add(category, newCategory);
		}
	}
	public static bool HasTag(this string tagName, string category = "")
	{
		if (tags.ContainsKey(category))
		{
			var currentCatagoy = tags[category];
			if (currentCatagoy.ContainsKey(tagName))
				return true;
		}
		return false;
	}
	public static object GetTag(this string tagName, string category = "")
	{
		if (tags.ContainsKey(category))
		{
			var currentCatagoy = tags[category];
			if (currentCatagoy.ContainsKey(tagName))
				return currentCatagoy[tagName];
		}
		return null;
	}
	public static bool RemoveTag(this string tagName, string category = "")
	{
		if (tags.ContainsKey(category))
		{
			var currentCatagoy = tags[category];
			if (currentCatagoy.ContainsKey(tagName))
			{
				currentCatagoy.Remove(tagName);
				return true;
			}
		}
		return false;
	}
	public static bool RemoveTagCategory(this string category)
	{
		if (tags.ContainsKey(category))
		{
			tags.Remove(category);
			return true;
		}
		return false;
	}
	#endregion




	#region AudioClip
	public static void Play(this AudioClip audioClip)
	{
		Sound.Play(audioClip);
	}
	#endregion






	#region GameObject
	public static PoolObj Pool(this GameObject gameobject, Transform tranform , float des = 0) {
		var pool = PoolManager.SpawParent(gameobject,tranform, des);
		pool.transform.ResetTransform();
		return pool;
	}
	public static PoolObj PoolPosition(this GameObject gameobject, Transform position, float des = 0)
	{
		var pool = PoolManager.Spawn(gameobject, position , des);
		pool.transform.rotation = position.rotation;
		return pool;
	}
	public static GameObject Create(this GameObject gameobject , Transform tranform = null)
	{
		var g = UnityEngine.GameObject.Instantiate(gameobject, tranform);
		//AssetsBundleHandle.RefreshMaterial(g);
		if (tranform != null) 
		{
			ResetTransform(g);
		}
		return g;
	}
	public static List<GameObject> Overlap(this GameObject gameobject , string tag = null)
	{
		var hits = new List<GameObject>();
		foreach (var hit in Physics.OverlapBox(gameobject.transform.position, gameobject.transform.localScale / 2, Quaternion.identity)) 
		{
			//Debug.Log(hit.gameObject.name);
			if (tag == null || hit.gameObject.tag == tag) 
			{
				hits.Add(hit.gameObject);
			}
		}
		return hits;
	}
	
	public static T Create<T>(this GameObject gameobject, Transform tranform = null)
	{
		var g = UnityEngine.GameObject.Instantiate(gameobject, tranform);
		//AssetsBundleHandle.RefreshMaterial(g);
		if (tranform != null)
		{
			ResetTransform(g);
		}
		return g.GetComponent<T>();
	}
	public static List<T> Creates<T>(this GameObject gameobject, Transform tranform , int count)
	{
		return Service.GameObj.Createds<T>(gameobject, tranform, count);
	}
	public static void ResetTransform(this GameObject gameobject)
	{
		gameobject.transform.localPosition = Vector3.zero;
		gameobject.transform.localScale = Vector3.one;
		gameobject.transform.localRotation = Quaternion.identity;
	}
	#endregion

	#region Transform
	public static List<GameObject> GetAllParent(this Transform transform)
	{
		return Service.GameObj.GetAllParent(transform);
	}
	public static List<T> GetAllParent<T>(this Transform transform)
	{
		return Service.GameObj.GetAllParent<T>(transform);
	}
	public static List<T> GetAllNode<T>(this Transform transform)
	{
		return Service.GameObj.GetAllNode<T>(transform);
	}
	public static List<GameObject> GetAllNode(this Transform transform)
	{
		return Service.GameObj.GetAllNode(transform);
	}
	public static GameObject GetChild(this Transform transform , string childName)
	{
		return Service.GameObj.GetTransform(transform, childName);
	}
	public static void DesAllParent(this Transform transform)
	{
		Service.GameObj.DesAllParent(transform);
	}
	public static void ResetTransform(this Transform transform)
	{
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
		transform.localRotation = Quaternion.identity;
	}
	public static void ChangeCurrentLocalPosition(this Transform transform,float? x , float?y , float? z)
	{
		var mvec = transform.localPosition;
		if (x != null) mvec.x = (float)x;
		if (y != null) mvec.y = (float)y;
		if (z != null) mvec.z = (float)z;
		transform.localPosition = mvec;
	}
	public static Vector3 RandomPointOnXZCircle(this Transform transform , float radius)
	{
		float angle = UnityEngine.Random.Range(0, 2f * Mathf.PI);
		return transform.localPosition + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
	}

	public static void SetActive(this Transform transform,bool active)
	{
		transform.gameObject.SetActive(active);
	}
	public static bool IsAactive (this Transform transform)
	{
		return transform.gameObject.activeSelf;
	}
	public static bool IsNotNull(this Transform transform)
	{
		return transform != null;
	}

	//Vector
	public static Vector3 Random(this Vector3 vector , float min , float max ) => Service.Vector.RandomVector(vector,min, max);


	//List
	public static Transform Find(this List<Transform> transforms , string name)
	{
		return transforms.Find(x=>x.name == name);
	}
	public static void Open(this List<Transform> transforms, string name)
	{
		var t = transforms.Find(x => x.name == name);
		transforms.ForEach(x => {
			if (x != null) 
				x.SetActive(t == null ? false : x == t);
		});
	}
	public static void Close(this List<Transform> transforms )
	{
		transforms.ForEach(x => x.SetActive(false));
	}
	public static void Open(this List<Transform> transforms, Transform target)
	{
		transforms.ForEach(x => x.SetActive( x == target));
	}
	public static void Open(this List<Transform> transforms, List<Transform> targets)
	{
		transforms.ForEach(x => x?.SetActive(targets.Contains(x)));
	}
	public static bool IsHas(this List<Transform> transforms, string name)
	{
		return transforms.Find(name) != null;
	}
	#endregion

	#region MonoBehaviour
	public static bool notnull(this MonoBehaviour value)
	{
		if (value == null)
			return false;
		return Service.GameObj.isObjectNotNull(value);
	}
	#endregion


	#region Coroutine

	public static Coroutine StartCorotine(this IEnumerator Iienumerator)
	{
		return Service.Tools.serviceRunTime.OnStartCorotine(Iienumerator);
	}
	public static void StopCorotine(this Coroutine corotine)
	{
		Service.Tools.serviceRunTime.OnStopCorotine(corotine);
	}

	#endregion




	#region Var
	// var value =========================================================================================
	public static void Update(this List<Service.Var.Val> dict, string key, double value)
	{
		var val = dict.Find(x => x.Key == key);
		if (val!=null) 
		{ 
			val.Value = value;  
		}
		else 
		{ 
			dict.Add(new Service.Var.Val() { Key = key , Value = value });
		}
	}
	public static void Increase(this List<Service.Var.Val> dict, string key, double value)
	{
		var val = dict.Find(x => x.Key == key);
		if (val != null)
		{
			val.Increase(value);
		}
	}
	public static void Increase(this Service.Var.Val val , double value)
	{
		if (val != null)
			val.Value += value;
	}
	public static bool IsHas(this List<Service.Var.Val> dict, string key)
	{
		return dict.Find(x => x.Key == key) != null;
	}
	public static double Find(this List<Service.Var.Val> dict , string key)
	{
		var val = dict.Find(x => x.Key == key);
		return val != null? val.Value : 0;
	}
	public static List<double> ToList(this List<Service.Var.Val> dict)
	{
		List<double> let = new List<double>();
		foreach (var d in dict)
			let.Add(d.Value);
		return let;
	}
	public static List<int> ToListInt(this List<Service.Var.Val> dict)
	{
		List<int> let = new List<int>();
		foreach (var d in dict)
			let.Add((int)d.Value);
		return let;
	}
	#endregion



	#region Log
	public static void Log(this object message)
	{
		Debug.Log(message);
	}
	public static void Log(this object message , string tag)
	{
		Debug.LogFormat($"{message}", tag );
	}
	public static void Log(this object message, Color color)
	{
		Debug.Log($"<color={Service.Colour.ToRGBHex(color)}>{message}</color>");
	}
	#endregion




















	#region List
	public static int[] Shuffle(this int count)
	{
		var index = new int[count];
		for (int i = 0; i < count; i++)
			index[i] = i;

		for (int i = 0; i < count; i++)
		{
			var temp = index[i];
			int randomIndex = UnityEngine.Random.Range(i, count);
			index[i] = index[randomIndex];
			index[randomIndex] = temp;
		}
		return index;
	}
	public static List<T> Shuffle<T>(this List<T> let)
	{
		for (int i = 0; i < let.Count; i++)
		{
			var temp = let[i];
			int randomIndex = UnityEngine.Random.Range(i, let.Count);
			let[i] = let[randomIndex];
			let[randomIndex] = temp;
		}
		return let;
	}
	public static List<T> If<T>(this List<T> let , System.Func<T, bool> func )
	{
		List<T> output = new List<T>();
		foreach (var l in let) 
		{
			if (func.Invoke(l)) 
			{
				output.Add(l);
			}
		}
		return output;
	}
	public static List<T> Clone<T>(this List<T> let)
	{
		return new List<T>(let);
	}
	public static void Modify<T>(this List<T> list, System.Func<T, bool> condition, System.Action<T> action)
	{
		foreach (var data in list)
		{
			if (condition.Invoke(data))
			{
				action.Invoke(data);
			}
		}
	}
	#endregion




	#region Dict
	// type =========================================================================================
	public static bool Update<T>(this Dictionary<string, T> dict, string key, T value, bool replace = true)
	{
		if (dict.ContainsKey(key))
		{
			if (replace)
				dict[key] = value;
			return false;
		}
		else
		{
			dict.Add(key, value);
			return true;
		}
	}
	public static void UpdateRange<T>(this Dictionary<string, T> origin, Dictionary<string, T> adds, bool replace = true)
	{
		foreach (var data in adds)
		{
			if (origin.ContainsKey(data.Key))
			{
				if (replace)
					origin[data.Key] = data.Value;
			}
			else
			{
				origin.Add(data.Key, data.Value);
			}
		}
	}
	public static bool Modify<T>(this Dictionary<string, T> dict, string key, System.Action<T> action, System.Action notfound = null)
	{
		if (dict.ContainsKey(key))
		{
			action?.Invoke(dict[key]);
			return true;
		}
		else
		{
			notfound?.Invoke();
			return false;
		}
	}
	public static T Find<T>(this Dictionary<string, T> dict, string key)
	{
		if (dict.ContainsKey(key)) { return dict[key]; }
		else { return default; }
	}
	public static List<T> FindAll<T>(this Dictionary<string, T> dict, System.Func<T, bool> condition)
	{
		List<T> result = new List<T>();
		foreach (var data in dict.Values)
		{
			if (condition.Invoke(data))
			{
				result.Add(data);
			}
		}
		return result;
	}

	public static int CountByCondition<T>(this Dictionary<string, T> dict, System.Func<T, bool> condition)
	{
		int count = 0;
		foreach (var data in dict.Values)
		{
			if (condition.Invoke(data))
			{
				count++;
			}
		}
		return count;
	}


	public static Dictionary<string, T> Clone<T>(this Dictionary<string, T> dict)
	{
		return new Dictionary<string, T>(dict);
	}
	public static List<T> ToList<T>(this Dictionary<string, T> dict)
	{
		return dict.Values.ToList();
	}
	public static Dictionary<string, T> ToDict<T>(this List<T> list, System.Func<T, string> condition)
	{
		Dictionary<string, T> result = new Dictionary<string, T>();
		foreach (var data in list)
		{
			var key = condition.Invoke(data);
			result.Update(key, data);
		}
		return result;
	}
	// object =========================================================================================
	public static bool Update(this Dictionary<string, object> dict, string key, object value)
	{
		if (dict.ContainsKey(key)) { dict[key] = value; return false; }
		else { dict.Add(key, value); return true; }
	}
	public static object Find(this Dictionary<string, object> dict, string key)
	{
		if (dict.ContainsKey(key)) { return dict[key]; }
		else { return null; }
	}
	public static List<object> ToList(this Dictionary<string, object> dict)
	{
		return dict.Values.ToList();
	}
	// string =========================================================================================
	public static bool Update(this Dictionary<string, string> dict, string key, string value)
	{
		if (dict.ContainsKey(key)) { dict[key] = value; return false; }
		else { dict.Add(key, value); return true; }
	}
	public static string Find(this Dictionary<string, string> dict, string key)
	{
		if (dict.ContainsKey(key)) { return dict[key]; }
		else { return null; }
	}
	public static List<string> ToList(this Dictionary<string, string> dict)
	{
		return dict.Values.ToList();
	}
	// int =========================================================================================
	public static int AddValue(this Dictionary<string, int> dict, string key, int value)
	{
		if (dict.ContainsKey(key))
		{
			dict[key] += value;
			return dict[key];
		}
		else
		{
			dict.Add(key, value);
			return value;
		}
	}
	public static bool Update(this Dictionary<string, int> dict, string key, int value)
	{
		if (dict.ContainsKey(key)) { dict[key] = value; return false; }
		else { dict.Add(key, value); return true; }
	}
	public static int Find(this Dictionary<string, int> dict, string key)
	{
		if (dict.ContainsKey(key)) { return dict[key]; }
		else { return 0; }
	}
	public static List<int> ToList(this Dictionary<string, int> dict)
	{
		return dict.Values.ToList();
	}
	// double =========================================================================================
	public static double AddValue(this Dictionary<string, double> dict, string key, double value)
	{
		if (dict.ContainsKey(key))
		{
			dict[key] += value;
			return dict[key];
		}
		else
		{
			dict.Add(key, value);
			return value;
		}
	}
	public static bool Update(this Dictionary<string, double> dict, string key, double value)
	{
		if (dict.ContainsKey(key)) { dict[key] = value; return false; }
		else { dict.Add(key, value); return true; }
	}
	public static double Find(this Dictionary<string, double> dict, string key)
	{
		if (dict.ContainsKey(key)) { return dict[key]; }
		else { return 0; }
	}
	public static List<double> ToList(this Dictionary<string, double> dict)
	{
		return dict.Values.ToList();
	}
	// long =========================================================================================
	public static long AddValue(this Dictionary<string, long> dict, string key, long value)
	{
		if (dict.ContainsKey(key))
		{
			dict[key] += value;
			return dict[key];
		}
		else
		{
			dict.Add(key, value);
			return value;
		}
	}
	public static bool Update(this Dictionary<string, long> dict, string key, long value)
	{
		if (dict.ContainsKey(key)) { dict[key] = value; return false; }
		else { dict.Add(key, value); return true; }
	}
	public static long Find(this Dictionary<string, long> dict, string key)
	{
		if (dict.ContainsKey(key)) { return dict[key]; }
		else { return 0; }
	}
	public static List<long> ToList(this Dictionary<string, long> dict)
	{
		return dict.Values.ToList();
	}
	// texture =========================================================================================
	public static bool Update(this Dictionary<string, Texture> dict, string key, Texture value)
	{
		if (dict.ContainsKey(key)) { dict[key] = value; return false; }
		else { dict.Add(key, value); return true; }
	}
	public static Texture Find(this Dictionary<string, Texture> dict, string key)
	{
		if (dict.ContainsKey(key)) { return dict[key]; }
		else { return null; }
	}
	// transform =========================================================================================
	public static bool Update(this Dictionary<string, Transform> dict, string key, Transform value)
	{
		if (dict.ContainsKey(key)) { dict[key] = value; return false; }
		else { dict.Add(key, value); return true; }
	}
	public static Transform Find(this Dictionary<string, Transform> dict, string key)
	{
		if (dict.ContainsKey(key)) { return dict[key]; }
		else { return null; }
	}
	// gameObject =========================================================================================
	public static bool Update(this Dictionary<string, GameObject> dict, string key, GameObject value)
	{
		if (dict.ContainsKey(key)) { dict[key] = value; return false; }
		else { dict.Add(key, value); return true; }
	}
	public static GameObject Find(this Dictionary<string, GameObject> dict, string key)
	{
		if (dict.ContainsKey(key)) { return dict[key]; }
		else { return null; }
	}
	#endregion

}
