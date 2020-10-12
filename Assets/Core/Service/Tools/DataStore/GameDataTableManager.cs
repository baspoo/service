using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameDataTableManager : MonoBehaviour {

	public static List<string> DataTables = new  List<string> (){
        "LanguageData",
		"ActionData",				// ==> [0]
		"MergeActionData",			// ==> [1]
		"CostumeData",              // ==> [2]
		"ItemBagData",              // ==> [3]
		"WeaponSkinData" ,          // ==> [4]
		"CharacterData",			// ==> [5]
		"AbilityData",				// ==> [6]
		"BlockData",				// ==> [7]
		"EnvironmentData",			// ==> [8]
		"MapDisplayData",			// ==> [9]
		"PlayModeData",				// ==> [10]
		"BuffData",					// ==> [11]
		"GameActivityData",			// ==> [12]
		"PlayerProgressData",		// ==> [13]
		"CharacterProgressData",	// ==> [14]
        "RampageData",              // ==> [15]
        "TutorialData",             // ==> [16]
		"ConfigData",              	// ==> [17]
        "GachaData",                // ==> [18]
        "ShopItemData",             // ==> [19]
        "DealData",                 // ==> [20]
        //"ExchangeData",           // ==> [21]
        "CashShopItemData",         // ==> [22]
        "CashShopDealData",         // ==> [23]
        "AchievementData",          // ==> [24]
        "MissionData",              // ==> [25]
        "DailyLoginData",           // ==> [26]
		"ManualData",               // ==> [27]
        "AbilityUnlockData",        // ==> [28]
        "FeatureUnlockData",        // ==> [29]
		"DurableData",				// ==> [30]
		"FestivalData"				// ==> [31]

    };

	static bool isInitFinish = false;
	public static bool InitGameDataTable () {

        if (!isInitFinish) {
			try{

				GetDataTable("ActionData");
				GetDataTable("ItemBagData");
				GetDataTable("BlockData");
				/*
                Actions.ActionData.InitDataTable 					(GetDataTable(DataTables[indexstep]));	indexstep++	;
				Actions.MergeActionData.InitDataTable				(GetDataTable(DataTables[indexstep]));	indexstep++	;
				Costume.CostumeData.InitDataTable           		(GetDataTable(DataTables[indexstep]));	indexstep++	;
				ItemBag.ItemBagData.InitDataTable           		(GetDataTable(DataTables[indexstep]));	indexstep++	;
				Actions.WeaponSkinData.InitDataTable				(GetDataTable(DataTables[indexstep]));	indexstep++	;
				Character.CharacterData.InitDataTable				(GetDataTable(DataTables[indexstep]));	indexstep++	;
				Ability.AbilityData.InitDataTable					(GetDataTable(DataTables[indexstep]));	indexstep++	;
				Block.BlockData.InitDataTable						(GetDataTable(DataTables[indexstep]));	indexstep++	;
				Environments.EnvironmentData.InitDataTable			(GetDataTable(DataTables[indexstep]));	indexstep++	;
				Map.MapDisplayData.InitDataTable           		 	(GetDataTable(DataTables[indexstep]));	indexstep++	;
				PlayModeData.InitDataTable							(GetDataTable(DataTables[indexstep]));	indexstep++	;
				Buff.BuffData.InitDataTable							(GetDataTable(DataTables[indexstep]));	indexstep++	;
				GameActivity.GameActivityData.InitDataTable			(GetDataTable(DataTables[indexstep]));	indexstep++	;
				ProgressData.PlayerProgressData.InitDataTable	    (GetDataTable(DataTables[indexstep]));	indexstep++	;
				ProgressData.CharacterProgressData.InitDataTable    (GetDataTable(DataTables[indexstep]));	indexstep++	;
				Rampage.RampageData.InitDataTable           		(GetDataTable(DataTables[indexstep]));	indexstep++	;
				Tutorial.TutorialData.InitDataTable        			(GetDataTable(DataTables[indexstep]));	indexstep++	;
				GameConfigData.InitDataTable						(GetDataTable(DataTables[indexstep]));	indexstep++	;
                Gacha.GachaData.InitDataTable                       (GetDataTable(DataTables[indexstep]));  indexstep++ ;
                Shop.ShopItemData.InitDataTable                     (GetDataTable(DataTables[indexstep]));  indexstep++ ;
                Shop.DealData.InitDataTable                         (GetDataTable(DataTables[indexstep]));  indexstep++ ;
                //Shop.ExchangeData.InitDataTable                   (GetDataTable(DataTables[indexstep]));  indexstep++ ;
                Shop.CashShopItemData.InitDataTable                 (GetDataTable(DataTables[indexstep]));  indexstep++ ;
                Shop.CashShopDealData.InitDataTable                 (GetDataTable(DataTables[indexstep]));  indexstep++ ;
                Mission.AchievementData.InitDataTable               (GetDataTable(DataTables[indexstep]));  indexstep++ ;
                Mission.MissionData.InitDataTable                   (GetDataTable(DataTables[indexstep]));  indexstep++ ;
                DailyLoginData.InitDataTable                        (GetDataTable(DataTables[indexstep]));  indexstep++ ;
				ManualData.InitDataTable							(GetDataTable(DataTables[indexstep]));  indexstep++ ;
                AbilityUnlockData.InitDataTable                     (GetDataTable(DataTables[indexstep]));  indexstep++ ;
                FeatureUnlockData.InitDataTable                     (GetDataTable(DataTables[indexstep]));  indexstep++ ;
				DurableData.InitDataTable							(GetDataTable(DataTables[indexstep]));	indexstep++ ;
				Festival.FestivalData.InitDataTable					(GetDataTable(DataTables[indexstep]));	indexstep++;
				*/

			}
			catch ( System.Exception e ) {
				Debug.LogError ( "[ "+ lastDataName + " ]    "+ e.Message +"\n" + e.StackTrace);
				return false;
			}
		}
		isInitFinish = true;
		return true;
	}
	static string lastDataName;
	static GameDataTable GetDataTable(string DataName){
		lastDataName = DataName;
		string Data = TSVLoaderTools.LoadContent(DataName);
		Debug.Log(DataName + " : " + Data);
		return  GameDataTable.ReadData (Data);
	}



	public static void LoadingAndInit(Service.Callback.callback callback) 
	{

		TSVLoaderTools.loader.Download(()=> {
			InitGameDataTable();
			if (callback != null)
				callback();
		});
		
	}


}
