using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Newtonsoft.Json;

namespace Data
{
    [System.Serializable]
    public class Plist
    {
        public static Plist Instance = new Plist();
        public static IEnumerator GetConfig(System.Action<Plist> done = null)
        {
            var url = $"{Application.streamingAssetsPath}/interactive/plist.json";
            WWW www = new WWW(url);
            yield return www;
            if (www.error.isnull())
            {
                //-> Complete
                var Json = www.text;
                //Debug.Log(Json);
                if (Json.notnull())
                {
                    Instance = Json.DeserializeObject<Plist>();
                }
                else Debug.LogError($"GetConfig : Json == null");
            }
            else
            {
                //-> Error
                Debug.LogError(www.error);
            }
            www.Dispose();
            done?.Invoke(Instance);
        }






















        //--> Distance
        public PlayerDistance playerDistance = new PlayerDistance();
        [System.Serializable]
        public class PlayerDistance
        {
            public float distanceOfFar = 50;
            public float distanceActive = 100;
            public float distanceObject = 50;
            public float distanceInteractive = 5;
            public float distanceFollow = 20;
            public float distanceNear = 2.5f;
        }
        public Input input = new Input();
        [System.Serializable]
        public class Input
        {
            public DeviceData web = new DeviceData();
            public DeviceData mobile = new DeviceData();
            [System.Serializable]
            public class DeviceData
            {
                public float spinSensitivity = 0.1f;
                public float zoomSensitivity = 1.0f;
            }
        }
    }

    



}