using UnityEngine;
using System.Collections;

public class function_string : MonoBehaviour {




	public static int PassInt(string str)
	{
		return System.Convert.ToInt32 (str);
	}
	public static float PassFloat(string str)
	{
		return System.Convert.ToSingle (str);
	}

	public static string strCropValue(string data,char CH_start,char CH_end)
	{
		int indexstart = data.IndexOf(CH_start)+1;
		int indexend = data.LastIndexOf(CH_end);
		indexend  -=indexstart;

		if (indexend == -1)
			return string.Empty;
		if(indexstart == -1)
			return string.Empty;

		string newdata = data.Substring(indexstart,indexend);
		return(newdata);
	}
	//Ex   key:value,key:value
	public static string getValueKey( string _Data,string itemkey)
	{
		string[] items = _Data.Split(',');
		foreach(string item in items)
		{
			string[] Value = item.Split(':');
			if(Value[0] == itemkey) return Value[1];
		}
		return string.Empty;
	}
	public static string desLastIndexStr(string str)
	{
		return  str.Remove(str.Length-1,1);
	}
	public string EncodeString(string str, int key)
	{
		string output_s ="";
		if (str.Length > 0) {
			for (int n =0; n < str.Length; n++) {
				char ch = str [n];
				int asc = System.Convert.ToInt32 (ch);
				asc += key;
				output_s += char.ConvertFromUtf32 (asc);
			}
		}
		return output_s;
	}
	


	public static string NumberConvertToCashString (int number ){
		return NumberConvertToCashString ((ulong)number,0);
	}
	public static string NumberConvertToCashString (int number , int lang) {
		return NumberConvertToCashString ((ulong)number , lang);
	}

	public static string NumberConvertToCashString (ulong number , int lang , ulong maximum ){
		if (number < maximum)
			return number.ToString("#,##0");
		else 
			return NumberConvertToCashString (number, lang );
	}
	public static string NumberConvertToCashString (ulong number ){
		return NumberConvertToCashString (number,0);
	}
	public static string NumberConvertToCashString (ulong number , int lang) {
		if (number >= 1000000000) {
			string[] cash = number.ToString("#,##0").Split(',');
			string output;
			if(lang==0)output = cash[0] + " B";
			else{
				if(lang>=3)lang=3;
				string Last = "."+cash [1].Substring (0, lang);
				if ((lang == 2)&&(Last ==".00")) Last = "";
				output = cash[0]+ Last+ " B";
			}
			return output;
		}
		if (number >= 1000000) {
			string[] cash = number.ToString("#,##0").Split(',');
			string output;
			if(lang==0)output = cash[0] + " M";
			else{
				if(lang>=3)lang=3;
				string Last = "."+cash [1].Substring (0, lang);
				if ((lang == 2)&&(Last ==".00")) Last = "";
				output = cash[0]+ Last + " M";
			}
			return output;
		}
		if (number >= 1000) {
			string[] cash = number.ToString("#,##0").Split(',');
			string output;
			if(lang==0)output = cash[0] + " K";
			else{
				if(lang>=3)lang=3;
				string Last = "."+cash [1].Substring (0, lang);
				if ((lang == 2)&&(Last ==".00")) Last = "";
				output = cash[0]+ Last + " K";
			}
			return output;
		}
		return number.ToString ();
	}






}

