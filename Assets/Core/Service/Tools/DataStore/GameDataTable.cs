﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;


public class GameData {
	Dictionary<string, string> dict = new Dictionary<string, string>();

	public void SetValue(string key, string value)
	{
		if (dict.ContainsKey(key))
			dict[key] = value;
		else
			dict.Add(key, value);
	}
	public string GetValue(string key)
	{
		if (dict.ContainsKey(key))
			return dict[key];
		else
		{
			Debug.LogError("Find null value : " + key);
			return string.Empty;
		}
	}


	public void GetValue(string key, out bool data) => Service.String.To(GetValue(key), out data);
	public void GetValue(string key, out int data) => Service.String.To(GetValue(key), out data);
	public void GetValue(string key, out float data) => Service.String.To(GetValue(key), out data);
	public void GetValue(string key, out double data) => Service.String.To(GetValue(key), out data);
	public void GetValue(string key, out long data) => Service.String.To(GetValue(key), out data);
	public void GetValue(string key, out string data) => Service.String.To(GetValue(key), out data);


	public void GetValue(string key, out int[] data) 
	{
		var val = GetValue(key);
		data = val.DeserializeObject<int[]>();
	}
	public void GetValue(string key, out double[] data)
	{
		var val = GetValue(key);
		data = val.DeserializeObject<double[]>();
	}
	public void GetValue(string key, out float[] data)
	{
		var val = GetValue(key);
		data = val.DeserializeObject<float[]>();
	}
	public void GetValue(string key, out string[] data)
	{
		var val = GetValue(key);
		data = val.DeserializeObject<string[]>();
	}
	public System.Enum GetEnum(string key, object enum_default) {
		var value = GetValue(key);
		return Service.String.ToEnum(value, enum_default);
	}
	public void GetValue(string key, out Formula data)
	{
		data = GetValue(key).ToFormula();
	}
	void GetValue(object objclass, string key)
	{
		var val = GetValue(key);
		if (!string.IsNullOrEmpty(val)) Service.Var.ToClass(objclass, key, val);
	}
	public void GetValue(object objclass, params string[] keys)
	{
		foreach (var k in keys)
			GetValue(objclass, k);
	}





	#region Obscurd
	//** Obscured Int
	public void GetValue(string key, out ObscuredInt data)
	{
		data = Service.String.Parse(GetValue(key), 0);
	}
	public void GetValue(string key, out ObscuredInt[] data)
	{
		var val = GetValue(key);
		var Ints = val.DeserializeObject<int[]>();
		data = new ObscuredInt[Ints.Length];
		for (int i = 0; i < Ints.Length; i++)
			data[i] = Ints[i];
	}
	//** Obscured Long
	public void GetValue(string key, out ObscuredLong data)
	{
		data = Service.String.Parse(GetValue(key), (long)0);
	}
	public void GetValue(string key, out ObscuredLong[] data)
	{
		var val = GetValue(key);
		var Ints = val.DeserializeObject<long[]>();
		data = new ObscuredLong[Ints.Length];
		for (int i = 0; i < Ints.Length; i++)
			data[i] = Ints[i];
	}
	//** Obscured Double
	public void GetValue(string key, out ObscuredDouble data)
	{
		data = Service.String.Parse(GetValue(key), 0);
	}
	public void GetValue(string key, out ObscuredDouble[] data)
	{
		var val = GetValue(key);
		var Ints = val.DeserializeObject<double[]>();
		data = new ObscuredDouble[Ints.Length];
		for (int i = 0; i < Ints.Length; i++)
			data[i] = Ints[i];
	}
	//** Obscured Float
	public void GetValue(string key, out ObscuredFloat data)
	{
		data = Service.String.Parse(GetValue(key), 0);
	}
	public void GetValue(string key, out ObscuredFloat[] data)
	{
		var val = GetValue(key);
		var Ints = val.DeserializeObject<float[]>();
		data = new ObscuredFloat[Ints.Length];
		for (int i = 0; i < Ints.Length; i++)
			data[i] = Ints[i];
	}
	#endregion





	public bool isHave(string key)
	{
		return dict.ContainsKey(key);
	}
}

public class GameDataTable {
	public void SetTable(List<GameData> Lists){
		GameDatas = Lists;
	}
	public 	List<GameData> GetTable(){
		return GameDatas;
	}
	List<GameData> GameDatas = new List<GameData>();
	public static GameDataTable ReadData(string Text)
	{
		GameDataTable GameDatasMaster= new GameDataTable();

		List<GameData> ThisGameDatas = new List<GameData>();
		string Data = Text;
		string[] Row = Data.Split ('\n');
		string[] Header = Row[0].Split ('\t');

		//Remove Character Special.
		for(int H = 0; H < Header.Length; H++){
			string HeaderText = Header[H];
			HeaderText = HeaderText.Replace("\n","");
			HeaderText = HeaderText.Replace("\r","");
			HeaderText = HeaderText.Replace("\t","");
			HeaderText = HeaderText.Replace("\v","");
			HeaderText = HeaderText.Replace("\b","");
			HeaderText = HeaderText.Replace(" ","");
			HeaderText = HeaderText.Trim ();
			Header[H] = HeaderText;
			//Debug.Log ("HeaderText:"+HeaderText);
		}

		for (int N = 1; N < Row.Length; N++) {
			int InfID = 0;
			if (!string.IsNullOrEmpty (Row [N])) {
				string[] Informations = Row [N].Split ('\t');
				GameData GameData = new GameData ();
				foreach (string keyHeader in Header) {
					string val = Informations [InfID];
					val = val.Replace ("\t", "");
					val = val.Replace ("\n", "");
					val = val.Replace ("\r", "");
					GameData.SetValue (keyHeader, val);
					InfID++;
				}
				ThisGameDatas.Add (GameData);
			}
		}
		GameDatasMaster.SetTable (ThisGameDatas);
		return GameDatasMaster;
	}
}
