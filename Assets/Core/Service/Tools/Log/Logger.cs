using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace LogService
{
    public class DebugLogger : MonoBehaviour
    {
        static DebugLogger debug;
        public static void Init()
        {

            if (debug == null)
            {
                LogSetting.Init();
                if (LogSetting.instance.IsEnable) 
                {
                    debug = new GameObject("DebugLogger").AddComponent<DebugLogger>();
                    DontDestroyOnLoad(debug.gameObject);
                    Utility.LogHandle.Init();
                    Utility.LocalSaveHandle.Init();
                }
            }
        }


        private void OnApplicationFocus(bool focus)
        {

        }
        private void OnApplicationPause(bool pause)
        {
            if (pause) Utility.LocalSaveHandle.OnPause();
            else Utility.LocalSaveHandle.OnResume();
        }
        private void OnApplicationQuit()
        {
            Utility.LocalSaveHandle.OnQuit();
        }
    }

}










namespace LogService.Utility
{
    [System.Serializable]
    public class LogTag
    {
        public string Tag;
        public Color Color;
        public bool Enable;

        public static  LogTag Find (string Tag) { return LogSetting.instance.Tags.Find(x => x.Tag == Tag); } 
    }
    public class Log
    {
        public static List<LogData> LogDatas = new List<LogData>();
        public class LogData
        {

            public LogData(bool add, string message, string tag, Exception e = null)
            {

                Message = message;
                TagName = tag;
                stackTrace = new System.Diagnostics.StackTrace(1, true);
                Date = Service.Time.DateTimeToUnixTimeStamp(System.DateTime.Now);
                exception = e;

                if (add)
                {
                    LogDatas.Add(this);

#if UNITY_EDITOR
                    LogEditor.LogEditor.logHistory.Add(this);
#endif
                }
                ReloadTag();
            }
            public bool isKeep;
            public long Date;
            public LogTag Tag;
            public string TagName;
            public string Message;
            public List<string> stackTraces;
            public System.Diagnostics.StackTrace stackTrace;
            public System.Exception exception;
            public object Object;


            public bool IsValidate
            {
                get
                {
                    if (TagName.isnull())
                    {
                        return LogSetting.instance.IsNonTag;
                    }
                    else
                    {
                        if (Tag == null)
                            return LogSetting.instance.IsUnkownTag;
                        else
                        {
                            if (LogSetting.instance.IsHasTag)
                            {
                                return Tag.Enable;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            public void ReloadTag()
            {

                if (TagName.notnull())
                    Tag = LogSetting.instance.Tags.Find(x => x.Tag == TagName);
                else
                    Tag = null;
            }

           


        }


        static HashSet<string> MethodNames = new HashSet<string> {
            "_LogFormat","LogFormat","Log", "HandleEvent",
            "HandleIMGUIEvent","DoOnGUI","OnGUI","OnGUISafe","SendEventToIMGUI","HandleEventAtTargetPhase",
            "DispatchEvent","ApplyDispatchingStrategies","Dispose","Dispatch","DoDispatch","ProcessEvent"

        };
        public static bool IsValidateStack(System.Diagnostics.StackFrame StackFrame) 
        {
            var refMethod = StackFrame.GetMethod();
            string pathFile = StackFrame.GetFileName();
            return (
                !MethodNames.Contains(refMethod.Name)
                && !string.IsNullOrEmpty(pathFile) 
                && pathFile.IndexOf("Assets") != -1) ;
        }
        public static string StackTraceMessage(System.Diagnostics.StackFrame StackFrame)
        {
            return $"[{StackFrame.GetMethod().Name}] { StackFrame.GetFileName() } ({ StackFrame.GetFileLineNumber()})({StackFrame.GetFileColumnNumber()})";
        }

        public static void Clear()
        {
            LogDatas.Clear();
        }
    }








    public class LogHandle
    {
        static bool IsCanLog => LogSetting.IsCanLogger;
        static ILogHandler unityLogHandler = Debug.unityLogger.logHandler;
        static ILogHandler logOriginal;
        public class LogHandler : ILogHandler
        {
            public LogHandler() { }
            public void LogException(System.Exception exception, UnityEngine.Object context)
            {
                _LogException(exception, context);
            }
            public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
            {
                _LogFormat(logType, context, format, args);
            }
        }

















        public static void Init()
        {
            logOriginal = Debug.unityLogger.logHandler;
            Debug.unityLogger.logHandler = new LogHandler();
        }
        public static void Restore()
        {
            Debug.unityLogger.logHandler = logOriginal;
        }


      
        static void _LogException(System.Exception exception, UnityEngine.Object context)
        {
            if (IsCanLog)
            {
                Log.LogData logData = new Log.LogData(true, exception.Message , string.Empty , exception);
                LocalSaveHandle.OnUpdateLog(logData);
                unityLogHandler.LogException(exception, context);
            }
        }
        static void _LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            void editorDebug(string message) 
            {
                unityLogHandler.LogFormat(logType, context, message , args);
            }


            if (IsCanLog)
            {
                Log.LogData logData = new Log.LogData( true , format , string.Empty );


                if (args.Length == 0) 
                {
                    if (validate(logData))
                        unitylog(logType, logData, context, args);
                }
                else if (args[0] is string)
                {
                    var tag = ((string)args[0]).ToString();
                    if (format == "{0}")
                    {
                        logData.Message = tag;
                        if (validate(logData))
                            unitylog(logType, logData, context, args);
                    }
                    else
                    {
                        logData.TagName = tag;
                        if (validate(logData))
                        {
                            unitylog(logType, logData, context, args);
                        }
                    }
                }
                else if (args[0] is bool)
                {
                    var tag = string.Empty;
                    if (args.Length > 1 && args[1] is string)
                    {
                        tag = (string)args[1];
                    }

                    logData.TagName = tag;
                    if (validate(logData))
                    {
                        if ((bool)args[0])
                        {
                            unitylog(logType, logData, context, args);
                        }
                    }

                   // editorDebug($"bool - {args.Length} {tag} {(bool)args[0]}");
                }
                else
                {
                    var tag = string.Empty;
                    if (args.Length > 1 && args[1] is string)
                    {
                        tag = (string)args[1];
                    }

                    logData.TagName = tag;
                    logData.Object = args[0];
                    if (validate(logData))
                    {
                        unitylog(logType, logData, context, args);
                    }
                }
            }
        }
        static bool validate(Log.LogData logData)
        {
            //** Find Tag
            logData?.ReloadTag();

            //** OnUpdateLog
            LocalSaveHandle.OnUpdateLog(logData);

            //** Validate
            return logData.IsValidate;
        }
        static void unitylog(LogType logType, Log.LogData logData, UnityEngine.Object context , params object[] args)
        {
            var message = logData.Message;
            if (logData.TagName != string.Empty)
            {
                if (logData.Tag == null) message = $"({logData.TagName}) {logData.Message}";
                else message = $"<color={Service.Colour.ToRGBHex(logData.Tag.Color)}>({logData.Tag.Tag})</color> {logData.Message}";
            }
            //unityLogHandler.LogFormat(logType, context , message, args);
            //unityLogHandler.Log(message);
            //Debug.unityLogger.Log(message);
            //Debug.Log(message);
            unityLogHandler.LogFormat(logType, new UnityEngine.Object(), "{0}", message);
        }
    }













    public class LocalSaveHandle
    {

      


        public static void Init()
        {
            if (LogSetting.instance.IsEnable)
            {
                DoAwake();
            }
        }
        public static void OnUpdateLog(Log.LogData logData)
        {
            DoUpdate(logData);
        }
        public static void OnPause()
        {
            DoSaveToLocal();
        }
        public static void OnResume()
        {
            DoSaveToLocal();
        }
        public static void OnQuit()
        {
            DoSaveToLocal();
        }
        public static void OnForceSave()
        {
            DoForceSaveToFile();
        }
        public static void OnReload(System.Action<List<Log.LogData>> callback)
        {
            ToRead((json) => {callback?.Invoke(ToLogData(json));});
        }
        public static void OnClear()
        {
            ToClear();
        }











        static bool enable = false;
        static List<Log.LogData> logDatas = new List<Log.LogData>();
        static void DoAwake()
        {
            enable = true;
            ToDelete();
            DoSaveToFile();
            logDatas = new List<Log.LogData>();
        }
        static void DoUpdate(Log.LogData logData) 
        {
            if (!enable) return;

            //** Filter Tag
            if (LogSetting.instance.IsLocalTagFilter) 
            {
                if (!logData.IsValidate)
                    return;
            }

            logDatas.Add(logData);


            //** Max Of Lenght
            if (logDatas.Count > LogSetting.instance.CountOfSaveToLocal) 
            {
                DoSaveToLocal();
            }
        }
        static void DoSaveToLocal() 
        {
            if (!enable) return;

            var str = ToStringLocal(logDatas);
            setLocalSave(str);
            logDatas.Clear();

            if (CountOflocalSave > LogSetting.instance.MaxLocalToFile)
            {
                DoSaveToFile();
            }
        }
        static void DoSaveToFile(bool force = false)
        {
            if (!enable || !force) return;

            if (isHasLocalSave) 
            {
                if (CountOflocalSave == 1)
                {
                    ToFile(string.Empty, getLocalSave(0));
                }
                else 
                {
                    CountOflocalSave.Loop((i) => {
                        ToFile(i.ToString(), getLocalSave(i));
                    });
                }
                RemoveLocalSave();
            }
        }
        static void DoForceSaveToFile()
        {
            var str = ToStringLocal(logDatas);
            setLocalSave(str);
            logDatas.Clear();
            DoSaveToFile(true);
        }













        public class DataFile 
        {
            public long DateTime;
            public string Log;
            public string Tag;
            public List<string> StackTrace;
            public List<string> Exception;
        }
      
        static int CountOflocalSave
        {
            get
            {
                return PlayerPrefs.GetInt("logger.count");
            }
            set
            {
                PlayerPrefs.SetInt("logger.count", value);
            }
        }
        static string getLocalSave(int count)
        {
            return PlayerPrefs.GetString($"logger.save.{count}");
        }
        static void setLocalSave(string value)
        {
            PlayerPrefs.SetString($"logger.save.{CountOflocalSave}", value);
            CountOflocalSave = CountOflocalSave + 1;
        }
        static bool isHasLocalSave => CountOflocalSave > 0;
        static void RemoveLocalSave( ) 
        {
            CountOflocalSave.Loop((i) => { PlayerPrefs.DeleteKey($"logger.save.{i}"); });
            CountOflocalSave = 0;
        } 
        static string ToStringLocal(List<Log.LogData> logs) 
        {
            var let = new List<DataFile>();
            foreach (var log in logs) 
            {
                var data = new DataFile();

                data.Log = log.Message;
                data.Tag = log.TagName;
                data.DateTime = log.Date;
                if (LogSetting.instance.IsLocalSaveFullLog) 
                {
                    if (log.stackTrace != null)
                    {
                        data.StackTrace = new List<string>();
                        foreach (var s in log.stackTrace.GetFrames())
                        {
                            if (Log.IsValidateStack(s))
                                data.StackTrace.Add(Log.StackTraceMessage(s));
                        }
                    }
                    if (log.exception != null)
                    {
                        data.Exception = new List<string>();
                        data.Exception.Add(log.exception.Message);
                        data.Exception.Add(log.exception.StackTrace);
                    }
                }
                let.Add(data);
            }
            string logger = let.SerializeToJson(SerializeHandle.NullValue);
            return logger;
        }
        static List<Log.LogData> ToLogData(string json)
        {
            var logs = new List<Log.LogData>();
            if (json.notnull())
            {
                var let = json.DeserializeObject<List<DataFile>>();
                foreach (var data in let) 
                {
                    var log = new Log.LogData(false, data.Log, data.Tag);
                    log.Date = data.DateTime;
                    log.stackTrace = null;
                    log.stackTraces = data.StackTrace;
                    log.exception = new Exception();
                    logs.Add(log);
                }
            }
            return logs;
        }
        public static string DirPath => Application.persistentDataPath + System.IO.Path.DirectorySeparatorChar + "Logs";
        const string fileType = ".log";
        static void ToFile( string index ,  string message ) 
        {
            if (message.isnull())
                return;

            //Path
            if (!System.IO.Directory.Exists(DirPath))
            {
                System.IO.Directory.CreateDirectory(DirPath);
            }

            //FileName
            string dateName = $"{System.DateTime.Now.ToUnix()}";
            if (index.notnull())
                dateName += $"({index})";


            //Save File
            string fullpath = DirPath + System.IO.Path.DirectorySeparatorChar + dateName +"."+ fileType;
            System.IO.File.WriteAllText(fullpath, message);
        }
        static void ToRead( System.Action<string> done)
        {
            EditorGUIService.NativeFilePath( fileType , DirPath, (path) => {
                if (path.notnull())
                {
                    var json = System.IO.File.ReadAllText(path);
                    done?.Invoke(json);
                }
            });
        }
        static void ToDelete( )
        {
            //Path
            if (!System.IO.Directory.Exists(DirPath))
            {
                System.IO.Directory.CreateDirectory(DirPath);
            }
            var unixNow = System.DateTime.Now.ToUnix();
            var info = new DirectoryInfo(DirPath);
            foreach (var file in info.GetFiles()) 
            {
                var fileName = file.Name;
                fileName = Service.String.strCropRemove(fileName,"(",")");
                var date = fileName.ToLong();
                if((unixNow - date) > LogSetting.instance.DaysExprid)
                    file.Delete();
            }
        }
        static void ToClear()
        {
            //Clear PlayerPrefs.
            100.Loop((i) => { PlayerPrefs.DeleteKey($"logger.save.{i}"); });
            PlayerPrefs.DeleteKey("logger.count");

            //Clear File.
            if (!System.IO.Directory.Exists(DirPath))
            {
                System.IO.Directory.CreateDirectory(DirPath);
            }
            var info = new DirectoryInfo(DirPath);
            foreach (var file in info.GetFiles())
            {
                file.Delete();
            }
        }


    }


}
