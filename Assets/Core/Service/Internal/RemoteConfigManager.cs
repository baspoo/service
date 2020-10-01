using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*

using Unity.RemoteConfig;
public class RemoteConfigManager : MonoBehaviour
{
    public struct userAttributes {
    }
    public struct appAttributes {
    }
    void OnEnable()
    {
        ConfigManager.FetchCompleted += ApplyRemoteSettings;
        ConfigManager.SetCustomUserID("Data");
        ConfigManager.FetchConfigs<userAttributes, appAttributes>( new userAttributes (), new appAttributes ());
    }
    void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        switch (configResponse.requestOrigin)
        {
            case ConfigOrigin.Default:
                Debug.Log("No settings loaded this session; using default values.");
                break;
            case ConfigOrigin.Cached:
                Debug.Log("No settings loaded this session; using cached values from a previous session.");
                break;
            case ConfigOrigin.Remote:
                Debug.Log("New settings loaded this session; update values accordingly.");
                Debug.Log(ConfigManager.appConfig.assignmentID);
                Debug.Log("gacha:" + ConfigManager.appConfig.GetString("gacha"));
                Debug.Log("gamedataconfig:" + ConfigManager.appConfig.GetString("gamedataconfig"));
                break;
        }




    }
}

*/
