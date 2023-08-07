using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discord : MonoBehaviour
{
    string url = "https://discord.com/api/webhooks/1132009325175505018/x4jk9FQyukHl4bs-Wzwf9WHJCFxhnu26zkxuo70H_URj-pvY6bHe9bWAnK0nN3zzxSel";
    static Discord m_discord;
    static Discord discord
    {
        get {
            if (m_discord == null) 
            {
                m_discord = (new GameObject("Discord")).AddComponent<Discord>();
                DontDestroyOnLoad(m_discord.gameObject);
            }
            return m_discord;
        }
    }
    public static void Send(string message) 
    {
      
        if (Application.isPlaying)
        {
            discord.StartCoroutine(discord.DoSendMessage(message));
        }
        else
        {
            #if UNITY_EDITOR
            EditorGUIService.Corotine.StartCorotine(discord.DoSendMessage(message));
            #endif
        }
    }
    IEnumerator DoSendMessage(string message)
    {
        WWWForm field = new WWWForm();
        field.AddField("content", message);
        UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.Post(url, field);
        yield return request.SendWebRequest();
    }
}
