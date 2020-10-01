using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

public class GamePlayerData {
	public bool isReady = false;
	public string isTokenKey;
	XmlNodeList xnList;
	public string GetValue(string Key){
		if(xnList[0][Key] != null)
			return System.Convert.ToString (xnList[0] [Key].InnerText);
		else 
			return string.Empty;
	}
	public void SetData(XmlNodeList p_xnList,string p_isTokenKey){
		isReady = true;
		xnList = p_xnList;
		isTokenKey = p_isTokenKey;
	}

	public static GamePlayerData Select(string TableName , string TokenKey)
	{
		GamePlayerData gamePlayerData = new GamePlayerData ();
		gamePlayerData.isReady = false;
		string Text =  PlayerPrefs.GetString (TokenKey);
		if (Text != string.Empty) {
			XmlDocument doc = new XmlDocument ();
			doc.LoadXml (Text);
			XmlNodeList st_xnList = doc.SelectNodes ("/GamePlayerData/"+TableName);
			gamePlayerData.SetData (st_xnList,TokenKey);
		}
		return gamePlayerData;
	}
	public static List<GamePlayerData> SelectAll(string TableName )
	{
		List<GamePlayerData> GamePlayerDatas = new List<GamePlayerData> ();
		string KeyDataTokenList = "GamePlayerDataTokenList_" + TableName;
		string DataTokenList =  PlayerPrefs.GetString (KeyDataTokenList);
		string[] FindTokenKey = DataTokenList.Split (',');
		foreach (string key in FindTokenKey) {
			if (key != string.Empty) {
				GamePlayerData gamePlayerData = Select (TableName, key);
				if (gamePlayerData.isReady) {
					GamePlayerDatas.Add (gamePlayerData);
				}
			}
		}
		return GamePlayerDatas;
	}

	/// <summary>
	/// Insert NewData to 'GamePlayerData'       Return TokenKey(string)
	/// </summary>
	/// <param name="Data">Data.</param>
	public static string Insert(SaveQurry Data)
	{
		string TokenKey = "GamePlayerData_" + Data.GetTable() + "_" + Random.Range(0,9999999).ToString() + System.DateTime.Now.ToString();
		string KeyDataTokenList = "GamePlayerDataTokenList_" + Data.GetTable();
		string DataTokenList =  PlayerPrefs.GetString (KeyDataTokenList);
		DataTokenList += TokenKey + ",";
		PlayerPrefs.SetString (KeyDataTokenList,DataTokenList);
		PlayerPrefs.SetString (TokenKey,Data.GetHashTable());
		return TokenKey;
	}
	public static void Update(SaveQurry Data)
	{
		string Text =  PlayerPrefs.GetString (Data.GetTokenKey());
		string NewUpdateData = Data.UpdateHashTable (Text);
		PlayerPrefs.SetString (Data.GetTokenKey(),NewUpdateData);
	}
	public static  void Delete(string TableName , string TokenKey)
	{
		string KeyDataTokenList = "GamePlayerDataTokenList_" + TableName;
		string DataTokenList =  PlayerPrefs.GetString (KeyDataTokenList);
		DataTokenList = DataTokenList.Replace (TokenKey+",","");
		PlayerPrefs.SetString (KeyDataTokenList,DataTokenList);
		PlayerPrefs.DeleteKey (TokenKey);
	}
	public static void DeleteAll(string TableName){
		string KeyDataTokenList = "GamePlayerDataTokenList_" + TableName;
		string DataTokenList =  PlayerPrefs.GetString (KeyDataTokenList);
		string[] DesTokenKeys = DataTokenList.Split (',');
		foreach (string key in DesTokenKeys)
			Delete (TableName,key);
	}









	public class SaveQurry {
		string m_TableName;
		string m_Token;

		List<string> Keys = new List<string>();
		List<string> Values = new List<string>();
		public void Add(string Key,string Value)
		{
			Keys.Add (Key);
			Values.Add (Value);
		}
		public void Commit_Insert(string TableName)
		{
			m_TableName = TableName;
		}
		public void Commit_Update(string TokenKey)
		{
			m_Token = TokenKey;
		}
		public string GetHashTable( )
		{
			string Hash = "";
			Hash+="<GamePlayerData>\n";
			Hash+="<"+m_TableName+">\n";
			for (int N = 0; N < Keys.Count; N++) {
				Hash+="<"+Keys[N]+">"+Values[N]+"</"+Keys[N]+">\n";
			}
			Hash+="</"+m_TableName+">\n";
			Hash+="</GamePlayerData>";
			return Hash;
		}


		public string UpdateHashTable( string DataLast )
		{
			string NewDataLast = DataLast;
			string[] Lines = DataLast.Split ('\n');
			for (int N = 0; N < Keys.Count; N++) {
				string StartHash 	= 	"<" + Keys [N] + ">";
				string EndHash 		=	"</"+Keys[N]+">";

				foreach (string OldLine in Lines) { //<key>60</key>
					int StartIndex = OldLine.IndexOf (StartHash);
					int EndIndex = OldLine.IndexOf (EndHash);
					if (StartIndex != -1)
					if (EndIndex != -1) {
						string NewLine = "<"+Keys[N]+">"+Values[N]+"</"+Keys[N]+">";
						NewDataLast = NewDataLast.Replace (OldLine,NewLine);
					}
				}

			}
			return NewDataLast;
		}
		public string GetTable( )
		{
			return m_TableName;
		}
		public string GetTokenKey( )
		{
			return m_Token;
		}
	}








	/*
	<Configs>
		<Gamecenter>
			<TimeMinDisplayMessages>3</TimeMinDisplayMessages>
			<AvatarAnimSpeed>60</AvatarAnimSpeed>
			<MaxAvatarDiaplay>15</MaxAvatarDiaplay>
			<Email>Gamemaster@gmail.com</Email>
		</Gamecenter>
	</Configs>
	
	*/

}



