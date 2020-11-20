using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;


public class InitializeGameData 
{


	static bool isInitFinish = false;
	public static bool InitGameDataTable()
	{

		if (!isInitFinish)
		{
			try
			{

				
				//UnitData.InitDataTable(GetDataTable("UnitData"));
				//ItemData.InitDataTable(GetDataTable("ItemData"));
				//AbilityData.InitDataTable(GetDataTable("AbilityData"));
				//IslandData.InitDataTable(GetDataTable("IslandData"));
				//ChapterData.InitDataTable(GetDataTable("ChapterData"));
				//ConfigData.InitDataTable(GetDataTable("ConfigData"));

			}
			catch (System.Exception e)
			{
				Debug.LogError("[ " + lastDataName + " ]    " + e.Message + "\n" + e.StackTrace);
				return false;
			}
		}
		isInitFinish = true;
		return true;
	}
	static string lastDataName;
	static GameDataTable GetDataTable(string DataName)
	{
		lastDataName = DataName;
		string Data = TSVLoaderTools.LoadContent(DataName);
		Debug.Log(DataName + " : " + Data);
		return GameDataTable.ReadData(Data);
	}
	public static void LoadingAndInit( bool isDownloadUpdate , Service.Callback.callback callback)
	{
		Debug.Log("IsDownloadUpdate : " + isDownloadUpdate);
		if (isDownloadUpdate)
		{
			TSVLoaderTools.loader.Download(() =>
			{
				InitGameDataTable();
				if (callback != null)
					callback();
			});
		}
		else
		{
			InitGameDataTable();
			if (callback != null)
				callback();
			
		}
	}


}



public class DataTools 
{







}