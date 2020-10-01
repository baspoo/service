using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
//using System.Diagnostics;



public class Log {


	public static bool isCanLog{

		get {
			if (Info.IsDev) 
			{
				#if UNITY_EDITOR || UNITY_STANDALONE
					return true;
				#else
					return false;
				#endif
			}
			else 
			{
				return false;
			}
		}
	}


	public enum LogType {
		write, runtime, unitydebug, button
	}
	public static List<LogData> LogDatas = new List<LogData>();
	public class LogData {
		public bool isKeep;
		public long Date;
		public LogType LogType;
		public TagData Tag;
		public string Message;
		public System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
		public Local LocalContent;
		public class Local
		{
			public GameObject GameObject;
			public Transform Transform;
			public MonoBehaviour MonoBehaviour;
			public object Object;
			public Service.Formula Formula;
		}
		public LocalButton LocalButtonContent = null;
		public class LocalButton
		{
			public string BtnName;
			public Service.Callback.callback LogCallback;
		}

		public LocalRunTime LocalRunTimeContent = null;
		public class LocalRunTime
		{
			public delegate void logDataCallback(LogData.LocalRunTime.LocalRunTimeData localRunTime);
			public logDataCallback RuntimeObject;
			public LocalRunTimeData Data = new LocalRunTimeData();
			public class LocalRunTimeData
			{
				public string Message;
			}
		}
	}


	public static LogData Write(object Message) {
		return Write(string.Empty, string.Empty, Message);
	}
	public static LogData Write(string Tag, object Content)
	{
		return Write(Tag, string.Empty, Content);
	}
	public static LogData Write(string Tag, string Message, object Content)
	{
		if (!isCanLog) return null;
		if (Content != null) {

			switch (Service.Tools.ObjectDefine.DefineType(Content)) {
				case Service.Formula.FormulaData.datatype.str:
				case Service.Formula.FormulaData.datatype.num:
				case Service.Formula.FormulaData.datatype.bol:
				case Service.Formula.FormulaData.datatype.enums:
					return Write(Tag, Content.ToString(), null); break;
			}
			if (Content.GetType() == typeof(GameObject)) return Write(Tag, Message, new LogData.Local() { GameObject = (GameObject)Content });
			if (Content.GetType() == typeof(Transform)) return Write(Tag, Message, new LogData.Local() { Transform = (Transform)Content });
			if (Content is MonoBehaviour) return Write(Tag, Message, new LogData.Local() { MonoBehaviour = (MonoBehaviour)Content });
			if (Content.GetType() == typeof(Service.Formula)) return Write(Tag, Message, new LogData.Local() { Formula = (Service.Formula)Content });
			return Write(Tag, Message, new LogData.Local() { Object = Content });
		}
		return Write(Tag, "Null", null);
	}


	public static LogData Write(string Tag,  Service.Formula Formula , bool isBreak) {
		return Write(Tag, string.Empty, Formula , isBreak);
	}
	public static LogData Write(string Tag, string Message, Service.Formula Formula, bool isBreak)
	{
		if (!isCanLog) return null;
		//Only Formula Break RuntimeUpdate
		if (isBreak) return Write(Tag, Message, new LogData.Local() { Formula = Service.String.Json.JsonToFormula(Formula.PassToJson(new Service.Formula())) });
		else return Write(Tag, Message, new LogData.Local() { Formula = Formula });
	}
	public static LogData WriteRuntime(string Tag, LogData.LocalRunTime.logDataCallback Content)
	{
		if (!isCanLog) return null;
		var log = Write(Tag, string.Empty , null);
		log.LocalRunTimeContent = new LogData.LocalRunTime() { RuntimeObject = Content };
		log.LogType = LogType.runtime;
		return log;
	}
	public static LogData WriteButton(string Tag , Service.Callback.callback OnClick) {
		if (!isCanLog) return null; 
		return WriteButton(Tag , string.Empty , OnClick);
	}
	public static LogData WriteButton(string Tag, string BtnName ,Service.Callback.callback OnClick)
	{
		if (!isCanLog) return null;
		var log = Write(Tag, string.Empty, null);
		log.LocalButtonContent = new LogData.LocalButton() { LogCallback = OnClick, BtnName = BtnName };
		log.LogType = LogType.button;
		return log;
	}



	public static LogData Write(LogData log)
	{
		if (!isCanLog) return null;
		log.Date = Service.Time.DateTimeToUnixTimeStamp(System.DateTime.Now);
		LogDatas.Add(log);
		return log;
	}
	public static LogData Write(string Tag ,string Message , LogData.Local local){
		if (!isCanLog) return null;

		LogData logData = new LogData() { LogType = LogType.write };
		logData.stackTrace = new System.Diagnostics.StackTrace(true);
		if (!LogTools.log.IgnoreAll)
            {
				TagData tagSelect = null;
				if (!string.IsNullOrEmpty(Tag))
				{
					foreach (TagData tag in LogTools.log.Tags)
					{
						if (Tag.ToLower() == tag.Tag.ToLower())
						{
							tagSelect = tag;
						}
					}
				}
				if(tagSelect == null) 
				{
					tagSelect = new TagData()
					{
						Color = Color.white,
						Tag = Tag,
						Enable = true
					};
				}
				logData.Tag = tagSelect;
				logData.LocalContent = local;
				//Have Tag & Tag isEnable
				if (tagSelect != null)
				{
					if (tagSelect.Enable)
						logData.Message = Message;
				}
				//Not Tag
				else
				{
					if (Tag == string.Empty)
					{
						if (!LogTools.log.IgnoreNotTag)
							logData.Message = Message;
					}
					else 
						logData.Message = Message;
				}
				 Write(logData);
			}
		return logData;
	}


	public static void Clear() {
		LogDatas.Clear();
	}



}
