#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System.Runtime.InteropServices;
using PhotoshopFile;
using System.Linq;
#endif






#if UNITY_EDITOR
namespace ExternalEditor
{
    public class ExternalEditor : MonoBehaviour
    {
        public static ExternalEditor externalEditor => (ExternalEditor)Resources.Load("ExternalEditor",typeof(ExternalEditor));
        public Material MaterialDefalut;
    }



   


    public class ExternalEditorInspector : EditorWindow
    {
        [MenuItem(EditorGUIService.ProjectPath.header + "/Editor/ExternalEditor")]
        public static void ShowWindow()
        {
            var wnd = GetWindow<ExternalEditorInspector>();
            wnd.title = "ExternalEditor";
            wnd.Show();
        }
        void Update()
        {
            Repaint();
        }


        public void OnGUI()
        {





            EditorGUILayout.Space(50);
        


            EditorGUILayout.LabelField($"Select Image File ( *jpg , *png , *psd , *fbx )");
            EditorGUILayout.BeginHorizontal();
            path = EditorGUILayout.TextField(path, GUILayout.Height(25));
            if (GUILayout.Button(EditorGUIService.BtnIcon("RotateTool"), GUILayout.Height(25), GUILayout.Width(25)))
            {
                m_imaage = null;
                lastModified = Service.Time.DateTime1970;
            }


            GUI.backgroundColor = (isDelete) ? Color.red : Color.white;
            if (GUILayout.Button(EditorGUIService.BtnIcon("PreMatSphere"), GUILayout.Height(25), GUILayout.Width(25)))
            {
                isDelete = !isDelete;
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();
            if (isDelete)
            {
                EditorGUIService.BeginContents(false);

                if (GUILayout.Button("Clean"))
                {
                    clear();
                    foreach (Object t in new ArrayList(Utility.store))
                    {
                        if (Application.isPlaying) Destroy(t);
                        else DestroyImmediate(t);
                    }
                    Utility.store.Clear();
                    EditorApplication.SaveScene();
                    return;
                }


                Utility.store = Utility.store.FindAll(x => x != null);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));


                if (isDelete && Utility.store != null && Utility.store.Count > 0)
                {
                    foreach (var t in Utility.store)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.ObjectField(t, typeof(GameObject));
                        GUI.backgroundColor = Color.red;
                        if (GUILayout.Button("Remove", GUILayout.Width(90)))
                        {
                            if (Application.isPlaying) Destroy(t);
                            else DestroyImmediate(t);
                            EditorApplication.SaveScene();
                            return;
                        }
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.EndHorizontal();


                    }
                }
                EditorGUILayout.EndScrollView();
                EditorGUIService.EndContents();

                return;
            }














            if (GUILayout.Button(new GUIContent(EditorGUIService.BtnIcon("Collab.FolderUpdated")), GUILayout.Height(60)))
            {
                Utility.NativeFilePath(Utility.alltyps, path, (url)=> {
                    if (!string.IsNullOrEmpty(url)) 
                    {
                        clear();
                        path = url;
                        m_fileinfo = new System.IO.FileInfo(path);
                    } 
                });
            }
            EditorGUILayout.HelpBox("Used for extracting external image files into the project.You can select a target object in the scene to use this image.", MessageType.Info);

            EditorGUILayout.Space(80);





     







            if (!string.IsNullOrEmpty(path))
            {
                if(m_autoupdate)
                    autoupdate();

                var type = Utility.IsFileType(path);



                if (type != Utility.filetype.mesh) focus();

                EditorGUILayout.Space(5);
                EditorGUIService.BeginContents(false);
                EditorGUIService.LableBlod("File Type : " + type.ToString());

                if (type == Utility.filetype.image) image();
                if (type == Utility.filetype.psd) psd();
                if (type == Utility.filetype.mesh) mesh();

                EditorGUIService.EndContents();
            }

        }
 

 



        int mode = 0;
        string path = "";
        static System.DateTime lastModified;
        Texture m_imaage = null;
        bool m_autoupdate = false;
        bool m_allparent = false;
        bool islockFocus = false;
        bool isFocus = false;
        bool isDelete = false;
        List<Texture2D> m_layer = null;
        FileInfo m_fileinfo;



        Object selection 
        {
            get 
            {
                return Selection.activeObject;
            }
        }

        Object current => (islockFocus) ? m_current : selection;
        Object m_current = null;
        Utility.applytype m_applytype = Utility.applytype.none;
        
        private void focus()
        {
          
            bool isview = selection != null;
            if (islockFocus)
            {
                isview = current != null;
            }



            if (isview)
            {

  

                EditorGUIService.BeginContents(false);
                EditorGUILayout.Space(10);


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(3);
                //GUILayout.Label(EditorGUIService.BtnIcon("Occlusion@2x"), GUILayout.Width(20), GUILayout.Height(20));
                GUI.backgroundColor = (islockFocus) ? Color.cyan : Color.white;
                if (GUILayout.Button(EditorGUIService.BtnIcon("AssemblyLock"), GUILayout.Height(25), GUILayout.Width(25)))
                {
                    islockFocus = !islockFocus;
                    if (islockFocus)
                    {
                        m_current = selection;
                    }
                    else 
                    {
                        m_current = null;
                    }
                }
                GUI.backgroundColor = Color.white;

                var select = current;
                EditorGUILayout.ObjectField(select, typeof(Object), GUILayout.Height(25));


                GUI.backgroundColor = (m_allparent) ? Color.cyan : Color.white;
                if (GUILayout.Button(EditorGUIService.BtnIcon("Collab.Build"), GUILayout.Height(25), GUILayout.Width(25)))
                {
                    m_allparent = !m_allparent;
                }
                GUI.backgroundColor = Color.white;



                EditorGUILayout.Space(3);
                EditorGUILayout.EndHorizontal();


                if (!m_allparent)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space(3);
                    var type = Utility.ApplyType(current);
                    isFocus = type != null;
                    if (type != null) EditorGUILayout.HelpBox("Target Type : " + type, MessageType.None);
                    else EditorGUILayout.HelpBox("This gameobject not have a renderer. can't replece texture.", MessageType.Error);
                    EditorGUILayout.Space(3);
                    EditorGUILayout.EndHorizontal();
                }
                else 
                {
                    isFocus = current!=null;
                }

                EditorGUILayout.Space(10);
                EditorGUIService.EndContents();

              
            }
            else 
            {
                isFocus = false;
                islockFocus = false;
            }
        }
        void clear() 
        {
            path = null;
            m_imaage = null;
            m_autoupdate = false;
            lastModified = Service.Time.DateTime1970;
            m_fileinfo = null;
        }
        void autoupdate()
        {
            if (string.IsNullOrEmpty(path))
            {
                clear();
                return;
            }

            var modified = System.IO.File.GetLastWriteTime(path);
            if (modified != lastModified)
            {
                m_imaage = null;
            }
        }










        void image() 
        {
            if (m_imaage == null) 
            {
                m_imaage = Utility.GetImagePath(path);
                lastModified = System.IO.File.GetLastWriteTime(path);

                if (m_autoupdate) 
                {
                    applyTexture(current, m_imaage);
                }
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(m_imaage, GUILayout.Width(60), GUILayout.Height(60));

            EditorGUI.BeginDisabledGroup(!isFocus);
            if (GUILayout.Button( "Apply" , GUILayout.Height(62)))
            {
                applyTexture(current, m_imaage);
            }
            EditorGUI.EndDisabledGroup();
            GUI.backgroundColor = (m_autoupdate)?Color.cyan :Color.white;
            if (GUILayout.Button(EditorGUIService.BtnIcon("Profiler.NetworkMessages@2x"), GUILayout.Height(62), GUILayout.Width(30)))
            {
                m_autoupdate = !m_autoupdate;
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();
        }


        PsdFile psdfile;
        Vector2 scrollPos;
        void psd()
        {
            if (m_imaage == null)
            {
                m_layer = null;
                bool isdone = false;
                var tex = (Texture)Utility.psd(path , out isdone);

                if (tex == null)
                    return;

                //if (isdone || !m_autoupdate)
                //{
                //    m_imaage = tex;
                //    lastModified = System.IO.File.GetLastWriteTime(path);
                //}


                m_imaage = tex;
                lastModified = System.IO.File.GetLastWriteTime(path);

                if (m_autoupdate)
                {
                    applyTexture(current, tex);
                }

            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(m_imaage, GUILayout.Width(60), GUILayout.Height(60));

            EditorGUI.BeginDisabledGroup(!isFocus);
            if (GUILayout.Button("Apply", GUILayout.Height(62)))
            {
                applyTexture(current, m_imaage);
            }
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button(EditorGUIService.BtnIcon("d_SceneViewFX@2x"), GUILayout.Height(62), GUILayout.Width(30)))
            {
                Utility.psds(path, (textures) => {

                    m_layer = textures;

                });
            }
            GUI.backgroundColor = (m_autoupdate) ? Color.cyan : Color.white;
            if (GUILayout.Button(EditorGUIService.BtnIcon("Profiler.NetworkMessages@2x"), GUILayout.Height(62), GUILayout.Width(30)))
            {
                m_autoupdate = !m_autoupdate;
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(25);



            if(m_layer!=null && m_layer.Count > 0) 
            { 
                EditorGUIService.BeginContents(false);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));
                foreach (var t in m_layer) 
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(t, GUILayout.Width(30), GUILayout.Height(30));
                    EditorGUI.BeginDisabledGroup(!isFocus);
                    if (GUILayout.Button(t.name, GUILayout.Height(30)))
                    {
                        applyTexture(current, t);
                    }
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();


                }
                EditorGUILayout.EndScrollView();
                EditorGUIService.EndContents();
            }
        }


        List<GameObject> m_mesh = new List<GameObject>();
        void mesh() 
        {


            if (m_fileinfo != null) 
            {

                EditorGUILayout.TextField("FileName",m_fileinfo.Name);
                EditorGUILayout.TextField("LastWriteTime", m_fileinfo.LastWriteTime.ToString());
                EditorGUILayout.TextField("Extension", m_fileinfo.Extension);
                EditorGUILayout.TextField("Length", (m_fileinfo.Length).ToString());
            }


            if (GUILayout.Button("Apply", GUILayout.Height(62)))
            {
                var root = Utility.mesh(path);
                EditorGUIService.Ping(root);
                Selection.activeObject = root;
            }

            EditorGUILayout.Space(25);

          
           


            //if (m_mesh != null && m_mesh.Count > 0)
            //{
            //    EditorGUIService.BeginContents(false);
            //    m_mesh = m_mesh.FindAll(x => x != null);
            //    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));
            //    foreach (var t in m_mesh)
            //    {
            //        EditorGUILayout.BeginHorizontal();
            //        EditorGUILayout.ObjectField(t, typeof(GameObject));
            //        GUI.backgroundColor = Color.red;
            //        if (GUILayout.Button("Remove", GUILayout.Width(90)))
            //        {
            //            if (Application.isPlaying) Destroy(t);
            //            else DestroyImmediate(t);
            //            return;
            //        }
            //        GUI.backgroundColor = Color.white;
            //        EditorGUILayout.EndHorizontal();


            //    }
            //    EditorGUILayout.EndScrollView();
            //    EditorGUIService.EndContents();
            //}



        }





        void applyTexture(Object obj, Texture tex = null) 
        {
            Utility.ApplyType(obj, tex);
            if (m_allparent && obj is GameObject)
            {
                foreach (var g in Service.GameObj.GetAllNode(((GameObject)obj).transform)) 
                {
                    Utility.ApplyType(g, tex);
                }
            }
            Utility.RefreshMonitor();
        }



    }














    public class Utility
    {
        public enum applytype
        {
            none,renderer,ngui,ugui
        }
        public enum filetype 
        {
            none,unkown,image,psd,mesh
        }

        public static List<Object> store = new List<Object>();
        static string[] imgTyps = new string[3] { "png", "jpg", "jpeg" };
        static string[] meshTyps = new string[2] { "obj", "fbx" };
        public static string alltyps = "psd,png,jpg,jpeg,obj,fbx";

        public static filetype IsFileType(string path )
        {
            if (string.IsNullOrEmpty(path))
                return filetype.none;

            var t = path.Split('.')[1];

            if(imgTyps.Contains(t))
                return filetype.image;

            if (t == "psd")
                return filetype.psd;

            if (meshTyps.Contains(t))
                return filetype.mesh;

            return filetype.unkown;
        }


        public static string ApplyType(Object obj, Texture tex = null)
        {
            if (obj == null)
                return null;


            if (obj as GameObject)
            {

                var gameobject = (GameObject)obj;

                if (gameobject.GetComponent<Renderer>() != null)
                {
                    if (tex != null)
                    {
                        var render = gameobject.GetComponent<Renderer>();
                        Material mat = new Material(render.sharedMaterial);
                        //mat.shader = render.sharedMaterial;
                        mat.mainTexture = tex;
                        render.material = mat;
                    }
                    return "Renderer";
                }
                if (gameobject.GetComponent<UITexture>() != null)
                {
                    if (tex != null)
                    {
                        var ui = gameobject.GetComponent<UITexture>();
                        ui.mainTexture = tex;
                        ui.Update();
                        ui.UpdateAnchors();
                        ui.enabled = true;
                    }
                    return "UITexture";
                }
                if (gameobject.GetComponent<RawImage>() != null)
                {
                    if (tex != null)
                    {
                        gameobject.GetComponent<RawImage>().texture = tex;
                    }
                    return "RawImage";
                }
            }
            if (obj as Material)
            {

                //var material = (Material)obj;
                //material.mainTexture = tex;

                if (tex != null)
                {
                    //Material material = (Material)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(obj), typeof(Material) );
                    var material = (Material)obj;
                    material.mainTexture = tex;
                }

                return "Material";
            }

            return null;
        }

        public static void NativeFilePath(string filter , string oldpath , System.Action<string> done)
        {
            //string path = EditorUtility.OpenFilePanel("Path", "", "png,jpg");
            string path = EditorUtility.OpenFilePanel("Path", oldpath , filter );
            done(path);
        }

        public static Texture GetImagePath(string path)
        {
            Texture2D tex = null;
            byte[] bytes;
            bytes = File.ReadAllBytes(path);
            tex = new Texture2D(2, 2, TextureFormat.ARGB32, true);
            tex.filterMode = FilterMode.Point;
            tex.LoadImage(bytes);
            store.Add(tex);
            return tex;
        }



        public static void RefreshMonitor() 
        {

            Canvas.ForceUpdateCanvases();
            EditorWindow.GetWindow(System.Type.GetType("UnityEditor.GameView,UnityEditor")).Repaint();
            SceneView.RepaintAll();
            EditorWindow.GetWindow<SceneView>().Repaint();
            HandleUtility.Repaint();
        }














        public static Texture2D psd(string path, out bool isdone)
        {
            isdone = false;
            if (string.IsNullOrEmpty(path))
                return null;
            try
            {
                var psd = new PsdFile(path, System.Text.Encoding.Default);
                return CreateTexture(psd.BaseLayer, out isdone);
            }
            catch 
            {
                return null;
            }
        }
        public static void psds(string path, System.Action<List<Texture2D>> done)
        {
            var psd = new PsdFile(path, System.Text.Encoding.Default);
            psds(psd, done);
        }
        public static void psds(PsdFile psd, System.Action<List<Texture2D>> done)
        {
            List<Texture2D> Texture2Ds = new List<Texture2D>();
            foreach (var Layer in psd.Layers)
            {
                bool isdone = false;
                var tex = CreateTexture(Layer, out isdone);
                Texture2Ds.Add(tex);
            }
            done(Texture2Ds);
        }
        private static Texture2D CreateTexture(Layer layer, out bool done)
        {
            done = false;
            if ((int)layer.Rect.width == 0 || (int)layer.Rect.height == 0)
                return null;

            Texture2D tex = new Texture2D((int)layer.Rect.width, (int)layer.Rect.height, TextureFormat.RGBA32, true);
            Color32[] pixels = new Color32[tex.width * tex.height];

            Channel red = (from l in layer.Channels where l.ID == 0 select l).First();
            Channel green = (from l in layer.Channels where l.ID == 1 select l).First();
            Channel blue = (from l in layer.Channels where l.ID == 2 select l).First();
            Channel alpha = layer.AlphaChannel;

            bool isdone = true;
            Dictionary<int, byte> dict = new Dictionary<int, byte>();
            byte load(int i)
            {
                isdone = false;
                if (dict.ContainsKey(i))
                    return dict[i];
                return 0;
            }
            byte getChannel(Channel channel, int i)
            {

                if (channel == null)
                    return load(i);
                if (channel.ImageData == null)
                    return load(i);

                if (channel.ImageData.Length > i)
                {

                    var b = channel.ImageData[i];
                    if (dict.ContainsKey(i))
                        dict[i] = b;
                    else
                        dict.Add(i, b);

                    return b;
                }
                else
                {
                    return load(i);
                }
            }


            for (int i = 0; i < pixels.Length; i++)
            {

                byte r = getChannel(red, i);
                byte g = getChannel(green, i);
                byte b = getChannel(blue, i);

                byte a = 255;

                if (alpha != null && alpha.ImageData!=null )
                    a = alpha.ImageData[i];

                int mod = i % tex.width;
                int n = ((tex.width - mod - 1) + i) - mod;
                pixels[pixels.Length - n - 1] = new Color32(r, g, b, a);
            }

            tex.name = layer.Name;

            tex.SetPixels32(pixels);
            tex.Apply();

            store.Add(tex);

            done = isdone;
            return tex;
        }


        public static GameObject mesh( string path )
        {
            var c = TriLibCore.AssetLoader.LoadModelFromFileNoThread(path, null, null, null);
            store.Add(c.RootGameObject);
            return c.RootGameObject;
        }





    }



}

#endif