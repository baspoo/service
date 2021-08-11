using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Newtonsoft.Json;


public static class VariableService 
{


	#region String
	public static bool notnull(this string str)
	{
		return !string.IsNullOrEmpty(str);
	}
	public static Service.Formula ToFormula(this string str)
	{
		return new Service.Formula(str);
	}
	public static int ToInt(this string str)
	{
		return Service.String.Parse(str, 0);
	}
	public static long ToLong(this string str)
	{
		return Service.String.Parse(str, (long) 0);
	}
	public static float ToFloat(this string str)
	{
		return Service.String.Parse(str, 0.0f);
	}
	public static double ToDouble(this string str)
	{
		return Service.String.Parse(str, 0.0);
	}
	public static bool ToBool(this string str)
	{
		return Service.String.Parse(str, false);
	}
	public static System.Enum ToEnum(this string str,object defaultenum)
	{
		return Service.String.ToEnum(str, defaultenum);
	}
	public static Vector2 ToVector2(this string str)
	{
		return Service.String.PassStringToVector2(str);
	}
	public static Vector3 ToVector3(this string str)
	{
		return Service.String.PassStringToVector3(str);
	}
	public static T DeserializeObject<T>(this string value) 
	{
		return JsonConvert.DeserializeObject<T>(value);
	}
	#endregion





	#region Number
	public static void Loop(this int i, System.Action<int> round)
	{
		Service.Loop.For(i, (r) => {
			round?.Invoke(r);
		});
	}
	public static void Wait(this float i, System.Action done)
	{
		Service.Timmer.Wait(i, () => { done?.Invoke(); });
	}
	public static void Wait(this double i, System.Action done)
	{
		Service.Timmer.Wait((float)i, () => { done?.Invoke(); });
	}
	#endregion















	#region Object
	public static string SerializeToJson(this object obj)
	{
		return ServiceJson.Json.SerializeObject(obj);
	}
	public static T DeserializeObject<T>(this object obj)
	{
		return JsonConvert.DeserializeObject<T>(SerializeToJson(obj));
	}
	public static Service.Formula ToFormula(this object obj)
	{
		return new Service.Formula(obj);
	}
	#endregion


	#region GameObject
	public static GameObject Create(this GameObject gameobject , Transform tranform = null)
	{
		return Service.GameObj.Created(gameobject, tranform);
	}
	public static void ResetTransform(this GameObject gameobject)
	{
		Service.GameObj.ResetTransform(gameobject.transform);
	}
	#endregion

	#region Transform
	public static List<GameObject> GetAllParent(this Transform transform)
	{
		return Service.GameObj.GetAllParent(transform);
	}
	public static void DesAllParent(this Transform transform)
	{
		Service.GameObj.DesAllParent(transform);
	}
	#endregion
}
