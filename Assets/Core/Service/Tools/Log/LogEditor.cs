using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using LogService.Utility;
using UnityEditor;
#endif


namespace LogService.LogEditor
{


#if UNITY_EDITOR

    public class LogCenterData
    {
        public string Name;
        // public bool isMeta;
        public Formula Formula;
        public Log.LogData Data;
        //public bool isAddMore;
        //public string AddMoremssage;
        //public bool isSave;
    }
    public class LogEditorPopup : EditorWindow
    {
        static LogCenterData logCenterData;
        public static void ShowWindow(LogCenterData data)
        {
            logCenterData = data;
            EditorWindow.GetWindow(typeof(LogEditorPopup), true, "Log Content");
        }
        Vector2 ScrollView;
        void OnGUI()
        {
            if (logCenterData == null)
                return;

            if (logCenterData.Formula != null)
            {
                EditorStyles.textField.wordWrap = true;
                FormulaToolsService.OnGUI.GUIFormulaSerialize.FormulaDataDisplay(logCenterData.Formula, FormulaToolsService.OnGUI.display.full, logCenterData.Name);
            }
            if (logCenterData.Data != null)
            {
                EditorGUILayout.BeginHorizontal();
                if (logCenterData.Data.Tag != null)
                {
                    GUI.backgroundColor = logCenterData.Data.Tag.Color;
                    EditorGUILayout.TextField("Tag:", logCenterData.Data.Tag.Tag);
                    GUI.backgroundColor = Color.white;
                }
                else
                {
                    EditorGUILayout.TextField("Tag:", "None");
                }
                //EditorGUILayout.EnumPopup(logCenterData.Data.LogType, GUILayout.Width(100.0f));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.TextArea(logCenterData.Data.Message, GUILayout.Height(100.0f));

                ScrollView = EditorGUILayout.BeginScrollView(ScrollView);
                EditorGUIService.BeginContents(false);


                if (logCenterData.Data.stackTrace != null) 
                {
                    foreach (var log in logCenterData.Data.stackTrace.GetFrames())
                    {
                        if (Log.IsValidateStack(log))
                        {
                            EditorGUILayout.BeginHorizontal();
                            if (GUILayout.Button("view", GUILayout.Width(60.0f)))
                            {
                                string currentFile = log.GetFileName();
                                int currentLine = log.GetFileLineNumber();
                                string finalFileName = System.IO.Path.GetFullPath(currentFile);
                                string path = $"/edit \"{finalFileName}\" /command \"edit.goto { currentLine.ToString() }\"";
                                var ss = System.Diagnostics.Process.Start("devenv", path);
                            }
                            EditorGUILayout.SelectableLabel(Log.StackTraceMessage(log), GUILayout.Height(15.0f));
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }

                if (logCenterData.Data.stackTraces != null) 
                {
                    foreach (var log in logCenterData.Data.stackTraces)
                    {
                        EditorGUILayout.SelectableLabel(log, GUILayout.Height(15.0f));
                    }
                }

                EditorGUIService.EndContents();
                EditorGUILayout.EndScrollView();
            }
        }
    }
    public class LogEditor : EditorWindow
    {

        public static List<Log.LogData> logHistory = new List<Log.LogData>();

        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(LogEditor), false, "LogEditor");

        }
        void OnGUI()
        {
            LogEditor.Gui();
        }


        void Update() { if (isUpdate) Repaint(); }




        static bool isBool(string key, string name)
        {
            bool bol = EditorPrefs.GetBool(key);
            bol = EditorGUILayout.ToggleLeft(name, bol, GUILayout.Width(name.Length * 9.0f));
            EditorPrefs.SetBool(key, bol);
            return bol;
        }
        static long DateFrom;
        static long DateTo;
        static bool isDatetime;
        static bool isUpdate = true;
        static bool isOption = false;
        static Vector2 ScrollView;
        public static void Gui()
        {

            #region ToolLayer1
            EditorGUILayout.BeginHorizontal();
            //#Tag
            GUILayout.Label(EditorGUIUtility.FindTexture("d_VisibilityOn"), GUILayout.Width(18.0f));
            string tag = EditorPrefs.GetString("editorlog_tag");
            EditorGUILayout.LabelField("Tag:", GUILayout.Width(40.0f));
            tag = EditorGUILayout.TextField(tag);
            EditorPrefs.SetString("editorlog_tag", tag);

            List<string> let = new List<string>() { "▼" };
            let.AddRange(LogSetting.instance.Tags.Select(x => x.Tag).ToList());
            var seleteTag = EditorGUILayout.Popup(0, let.ToArray() , GUILayout.Width(18));
            if (seleteTag != 0)
            {
                if(tag.isnull()) tag = let[seleteTag];
                else tag += "," + let[seleteTag];
                EditorPrefs.SetString("editorlog_tag", tag);
            }
           

            //#Keyword
            GUILayout.Label(EditorGUIUtility.FindTexture("d_ViewToolZoom"), GUILayout.Width(18.0f));
            string keyword = EditorPrefs.GetString("editorlog_keyword");
            EditorGUILayout.LabelField("Keyword:", GUILayout.Width(60.0f));
            keyword = EditorGUILayout.TextField(keyword);
            EditorPrefs.SetString("editorlog_keyword", keyword);

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Clear", GUILayout.Width(80.0f)))
            {
                logHistory.RemoveAll(x=>!x.isKeep);
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();
            #endregion






            #region ToolLayer2
            EditorGUILayout.BeginHorizontal();

            //isUpdate
            isUpdate = EditorPrefs.GetBool("editorlog_isUpdate");
            if (GUILayout.Button(EditorGUIUtility.FindTexture(isUpdate ? "Animation.Record" : "Animation.Play"), GUILayout.Width(25.0f)))
            {
                isUpdate = !isUpdate;
                EditorPrefs.SetBool("editorlog_isUpdate", isUpdate);
            }
            //Display DateTime
            isDatetime = EditorPrefs.GetBool("editorlog_istime");
            GUI.backgroundColor = (isDatetime) ? Color.yellow : Color.gray;
            if (GUILayout.Button(EditorGUIUtility.FindTexture("d_UnityEditor.AnimationWindow"), GUILayout.Width(25.0f)))
            {
                if (!isDatetime && EditorPrefs.GetString("editorlog_keyword").isnull()) 
                {
                    var dateString = $"[00:00:00-{System.DateTime.Now.ToString("HH:mm:ss")}]";
                    EditorPrefs.SetString("editorlog_keyword", dateString);
                }
                if (isDatetime && (DateFrom != 0 || DateTo != 0))
                {
                    EditorPrefs.SetString("editorlog_keyword", string.Empty);
                }

                isDatetime = !isDatetime;
                EditorPrefs.SetBool("editorlog_istime", isDatetime);
            }
            GUI.backgroundColor = (isOption) ? Color.yellow : Color.gray;
            if (GUILayout.Button("☰", GUILayout.Width(20.0f))) 
            {
                isOption = !isOption;
            }
            //Save
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button(EditorGUIUtility.FindTexture("SaveActive"), GUILayout.Width(25.0f)))
            {
                //LogTools.Save(true);
                LocalSaveHandle.OnForceSave();
            }
            //Load
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button(EditorGUIUtility.FindTexture("d_Project"), GUILayout.Width(25.0f)))
            {
                LocalSaveHandle.OnReload((logs)=> {
                    logHistory.AddRange(logs);
                });
            }
            if (GUILayout.Button(EditorGUIUtility.FindTexture("winbtn_win_close"), GUILayout.Width(25.0f)))
            {
                EditorGUIService.OpenDialog("ลบแน่หรือ?", "Remove all log files and clean&clear tempdata ", () =>
                {
                    LocalSaveHandle.OnClear();
                });
               
            }
            //Push
            //if (GUILayout.Button(EditorGUIUtility.FindTexture("d_P4_AddedRemote"), GUILayout.Width(25.0f)))
            //{
            //    LogCenterData logCenterData = new LogCenterData()
            //    {
            //        isAddMore = true
            //    };
            //    LogEditorPopup.ShowWindow(logCenterData);
            //    return;
            //}
            //Setting
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button("Setting", GUILayout.Width(65.0f)))
            {
                UILogSetting.OnSelection();
            }
            EditorGUILayout.EndHorizontal();
            #endregion





            if (!isUpdate)
                return;



            //time[20:20:20 - 20:20:21]
            DateFrom = 0;
            DateTo = 0;
            if (isDatetime && !string.IsNullOrEmpty(keyword))
            {
                if (Service.String.isStrCropValue(keyword, "[", "]"))
                {
                    string t = Service.String.strCropValue(keyword, "[", "]");
                    if (!string.IsNullOrEmpty(t))
                    {
                        try
                        {
                            string[] times = t.Split('-');
                            if (times.Length > 0)
                            {
                                System.DateTime d = System.DateTime.Now;
                                string iString = $"{d.Year}-{d.Month}-{d.Day} {times[0]}";
                                System.DateTime oDate = System.Convert.ToDateTime(iString);
                                DateFrom = Service.Time.DateTimeToUnixTimeStamp(oDate);
                            }
                            if (times.Length > 1)
                            {
                                System.DateTime d = System.DateTime.Now;
                                string iString = $"{d.Year}-{d.Month}-{d.Day} {times[1]}";
                                System.DateTime oDate = System.Convert.ToDateTime(iString);
                                DateTo = Service.Time.DateTimeToUnixTimeStamp(oDate);
                            }
                        }
                        catch (System.Exception e)
                        {

                        }
                    }
                }
            }



            List<Log.LogData> logDataFilter = new List<Log.LogData>();
            foreach (var log in logHistory)
            {
                bool isHaveTag = true;

                if (!log.IsValidate)
                    isHaveTag = false;

                if (!string.IsNullOrEmpty(tag))
                {
                    if (tag.IndexOf(",") != -1)
                        isHaveTag = log.Tag != null && tag.IndexOf(log.Tag.Tag) != -1;
                    else
                        isHaveTag = log.Tag != null && log.Tag.Tag.IndexOf(tag) != -1;
                }

                if (isHaveTag)
                {
                    if (!string.IsNullOrEmpty(keyword))
                    {


                        if (DateFrom != 0 && DateTo != 0)
                        {
                            if (log.Date >= DateFrom && log.Date <= DateTo)
                            {
                                isHaveTag = true;
                            }
                            else isHaveTag = false;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(log.Message))
                                isHaveTag = log.Message.IndexOf(keyword) != -1;
                            else
                                isHaveTag = false;
                        }
                    }
                }
                if (isHaveTag)
                    logDataFilter.Add(log);
            }


            if (logDataFilter.Count > 0)
            {
                ScrollView = EditorGUILayout.BeginScrollView(ScrollView);
                EditorGUIService.BeginContents(false);
                foreach (var log in logDataFilter)
                {
                    Line(log);
                }
                EditorGUIService.EndContents();
                EditorGUILayout.EndScrollView();
            }

        }













        public static void Line(Log.LogData log)
        {
            EditorGUILayout.BeginHorizontal();
            LineContent(log);
            EditorGUILayout.EndHorizontal();
        }
        static void LineContent(Log.LogData log)
        {


            //** [Option]
            if (isOption)
            {
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("x", GUILayout.Width(20.0f)))
                {
                    logHistory.Remove(log);
                    return;
                }
                GUI.backgroundColor = (log.isKeep) ? Color.cyan : Color.black;
                if (GUILayout.Button("★", GUILayout.Width(20.0f)))
                {
                    log.isKeep = !log.isKeep;
                }
                GUI.backgroundColor = Color.white;
                if (GUILayout.Button("☰", GUILayout.Width(20.0f)))
                {
                    LogCenterData logCenterData = new LogCenterData()
                    {
                        Data = log
                    };
                    LogEditorPopup.ShowWindow(logCenterData);
                }
                GUI.backgroundColor = Color.white;
            }


            if (isDatetime)
                EditorGUILayout.TextField(Service.Time.UnixTimeStampToDateTime(log.Date).ToString("HH:mm:ss"), GUILayout.Width(120.0f));


            //** [Tag]
            GUI.backgroundColor = (log.Tag == null) ? Color.white : log.Tag.Color;
            EditorGUILayout.TextField((log.Tag == null) ? log.TagName : log.Tag.Tag, GUILayout.Width(40.0f));
            GUI.backgroundColor = Color.white;

            //** [Message]
            Message(log.Message, log.Object);

        }
        static void Message(string message, object action = null) 
        {
            if (action == null)
            {
                EditorGUILayout.SelectableLabel(message, GUILayout.Height(15.0f));
            }
            else 
            {
                EditorGUILayout.SelectableLabel(message, GUILayout.Height(15.0f), GUILayout.Width(100.0f));

                if (action is GameObject)
                {
                    var gameobj = (GameObject)action;
                    EditorGUILayout.ObjectField(gameobj, typeof(GameObject), GUILayout.Width(80.0f));
                    //if(gameobj!=null) EditorGUIService.GizmosUtils.DrawText(null, message , gameobj.transform.position );
                }
                else if (action is Transform)
                {
                    var trans = (Transform)action;
                    EditorGUILayout.ObjectField(trans, typeof(Transform), GUILayout.Width(80.0f));
                    //if (trans != null) EditorGUIService.GizmosUtils.DrawText(null, message, trans.transform.position);
                }
                else if (action is AudioClip)
                {
                    var clip = (AudioClip)action;
                    EditorGUILayout.ObjectField(clip, typeof(AudioClip), GUILayout.Width(80.0f));
                }
                else if (action is AnimationClip)
                {
                    var anim = (AnimationClip)action;
                    EditorGUILayout.ObjectField(anim, typeof(AnimationClip), GUILayout.Width(80.0f));
                }
                else if (action is Formula)
                {
                    var formula = (Formula)action;
                    if (GUILayout.Button("formula", GUILayout.Width(80.0f)))
                    {
                        LogCenterData logCenterData = new LogCenterData()
                        {
                            Name = "formula",
                            Formula = formula
                        };
                        LogEditorPopup.ShowWindow(logCenterData);
                    }
                }
                else if (action is System.Action && Application.isPlaying)
                {

                    var act = (System.Action)action;
                    if (GUILayout.Button("Invoke", GUILayout.Width(80.0f)))
                    {
                        act?.Invoke();
                    }
                }
                else if (action is System.Func<object> && Application.isPlaying)
                {
                    var func = (System.Func<object>)action;
                    EditorGUILayout.TextField($"{func.Invoke().SerializeToJson()}");
                }
                else 
                {
                
                
                }

                
            }
        }









    }


#endif


}




