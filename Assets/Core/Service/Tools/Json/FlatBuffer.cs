using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class FlatBuffer 
{

    const string cHeader = "header";
    const string cValues = "values";
    public class Data {
        public List<string> header;
        public Dictionary<string, List<object>> values;
    }
    static bool _IsFlatBufferFormat(string flatFormat)
    {
        try
        {
            var dict = flatFormat.DeserializeObject<Dictionary<string, object>>();
            if (dict.Count != 2) return false;
            var keys = dict.Keys.Select(x=>x.ToLower()).ToList();
            if (!keys.Contains(cHeader)) return false; 
            if (!keys.Contains(cValues)) return false;
            return true;
        }
        catch 
        {
            return false;
        }
    }
    static string _FlatBufferToJsonObject(string jsonFlatFormat)
    {
        var dict = jsonFlatFormat.DeserializeObject<Data>( SerializeHandle.IgnoreUpperCase );
        var header = dict.header;
        var values = dict.values;
        if (header == null && values == null)
        {
            //not format
            return jsonFlatFormat;
        }
        else 
        {
            var let = new Dictionary<string, Dictionary<string, object>>();
            foreach (var row in values)
            {
                int index = 0;
                var data = new Dictionary<string, object>();
                foreach (var f in row.Value)
                {
                    data.Add(header[index], f);
                    index++;
                }
                let.Add(row.Key, data);
            }
            var output = let.SerializeToJson();
            return output;
        }

    }
    static string _FlatBufferToJsonArray(string jsonFlatFormat)
    {
        var dict = jsonFlatFormat.DeserializeObject<Dictionary<string, object>>();
        var header = dict[cHeader].DeserializeObject<List<string>>();
        var values = dict[cValues].DeserializeObject<Dictionary<string, List<object>>>();
        var let = new List<Dictionary<string, object>>();
        foreach (var row in values.Values)
        {
            int index = 0;
            var data = new Dictionary<string, object>();
            foreach (var f in row)
            {
                data.Add(header[index], f);
                index++;
            }
            let.Add(data);
        }
        var output = let.SerializeToJson();
        return output;
    }
    static string _JsonToFlatBuffer(string json, List<string> header = null) 
    {
        if (json.Length > 2) 
        {
            if (json[0] == '{' && json[json.Length - 1] == '}')
            {
                return _JsonObjectToFlatBuf(json, header);
            }
            if (json[0] == '[' && json[json.Length - 1] == ']')
            {
                return _JsonArrayToFlatBuf(json, header);
            }
        }
        return null;
    }
    static string _JsonObjectToFlatBuf(string json, List<string> header = null) 
    {
        var let = json.DeserializeObject<Dictionary<string, Dictionary<string, object>>>();
        if (header == null)
        {
            header = new List<string>();
            foreach (var d in let.Values)
            {
                foreach (var val in d)
                    if (!header.Contains(val.Key))
                        header.Add(val.Key);
            }
        }
        List<string> keys = let.Keys.ToList();
        int index = 0;
        Dictionary<string, object[]> dict = new Dictionary<string, object[]>();
        foreach (var d in let.Values)
        {
            object[] values = new object[header.Count];
            for (int h = 0; h < header.Count; h++)
            {
                if (d.ContainsKey(header[h]))
                {
                    values[h] = d[header[h]];
                }
                else
                {
                    values[h] = null;
                }
            }
            dict.Add($"{keys[index]}", values);
            index++;
        }
        var final = new Dictionary<string, object>();
        final.Add(cHeader, header);
        final.Add(cValues, dict);
        var output = final.SerializeToJson();
        return output;
    }
    static string _JsonArrayToFlatBuf(string json, List<string> header = null)
    {
        var let = json.DeserializeObject<List<Dictionary<string, object>>>();


        if (header == null)
        {
            header = new List<string>();
            foreach (var d in let)
            {
                foreach (var val in d)
                    if (!header.Contains(val.Key))
                        header.Add(val.Key);
            }
        }

        int index = 0;
        Dictionary<string, object[]> dict = new Dictionary<string, object[]>();
        foreach (var d in let)
        {
            object[] values = new object[header.Count];
            for (int h = 0; h < header.Count; h++)
            {
                if (d.ContainsKey(header[h]))
                {
                    values[h] = d[header[h]];
                }
                else
                {
                    values[h] = null;
                }
            }
            dict.Add($"{index}", values);
            index++;
        }

        var final = new Dictionary<string, object>();
        final.Add(cHeader, header);
        final.Add(cValues, dict);
        var output = final.SerializeToJson();
        return output;
    }








    public static bool IsFlatBufferFormat(this string json)
    {
        return _IsFlatBufferFormat(json);
    }
    public static string ConvertJsToFlat(this string json, List<string> header = null)
    {
        return _JsonToFlatBuffer(json, header);
    }
    public static string ConvertFlatToJsObject(this string json)
    {
        return _FlatBufferToJsonObject(json);
    }
    public static string ConvertFlatToJsArray(this string json)
    {
        return _FlatBufferToJsonArray(json);
    }


}

