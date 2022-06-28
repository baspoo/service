using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Playlist : MonoBehaviour
{


    static Playlist m_instance;
    public static Playlist instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = ((GameObject)Resources.Load("Sound/Playlist")).GetComponent<Playlist>();
            }
            return m_instance;
        }
    }






    public Backgrounds backgroundmusic = new Backgrounds();
    [System.Serializable]
    public class Backgrounds
    {
        public SoundData bgm_mainmenu;
    }


    public static Sfx let => instance.playlist;
    public Sfx playlist = new Sfx();
    [System.Serializable]
    public class Sfx
    {
        [Header("Interface ------------------------------------------")]
        public SoundData sfx_click;
        public SoundData sfx_select;
        public SoundData sfx_submit;
        public SoundData sfx_message;
        public SoundData sfx_notif;
        public SoundData sfx_ping;
        public SoundData sfx_back;
        public SoundData sfx_openbag;
        public SoundData sfx_movepage;
        [Header("Misc ------------------------------------------")]
        public SoundData sfx_get_carbon;
        public SoundData sfx_get_coin;
        public SoundData sfx_purchase;
        public SoundData sfx_get_reward_1;
        public SoundData sfx_get_reward_2;
        public SoundData sfx_placeobject;
        public SoundData sfx_planttree;
        public SoundData sfx_watertree;
        public SoundData sfx_deadtree;
        public SoundData sfx_bufftree;
        public SoundData sfx_win_1;
        public SoundData sfx_win_2;
        public SoundData sfx_onworld;
        public SoundData sfx_warp_1;
        public SoundData sfx_warp_2;
        public SoundData sfx_impactground;
        public SoundData sfx_get_minievent;

        public SoundData sfx_pops => sfx_pop_many[sfx_pop_many.Length.Random()];
        [SerializeField] SoundData[] sfx_pop_many;

        public SoundData sfx_pages => sfx_page_many[sfx_page_many.Length.Random()];
        [SerializeField] SoundData[] sfx_page_many;






    }








}


































[System.Serializable]
public class SoundData
{
    public string path;
    public AudioClip clip;

    public AudioClip getclip
    {
        get
        {
            if (!Application.isPlaying)
                return reload(path);
            if (clip == null)
            {
                if (!string.IsNullOrEmpty(path))
                    clip = reload(path);
            }
            return clip;
        }
    }
    AudioClip reload(string path)
    {
        return Resources.Load($"Sound/" + path) as AudioClip;
    }
    public void Play(bool unmuteBackground = false )
    {
        Sound.Play( getclip , unmuteBackground );
    }
}



#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SoundData))]
public class IngredientDrawer : PropertyDrawer
{
    int nextwidth = 0;
    void Reposition() { nextwidth = 0; }
    Rect Position(Rect position, int width)
    {
        Rect R = new Rect(position.x + nextwidth, position.y, width, position.height);
        nextwidth += width + 5;
        return R;
    }
    public string pathKeep;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        Reposition();
        //EditorGUI.PropertyField(Position(position, 35), property.FindPropertyRelative("clip"), GUIContent.none);
        EditorGUI.PropertyField(Position(position, 180), property.FindPropertyRelative("path"), GUIContent.none);
        var obj = Resources.Load($"Sound/" + property.FindPropertyRelative("path").stringValue) as AudioClip;

        if (obj != null)
        {
            if (GUI.Button(Position(position, 25), EditorGUIUtility.FindTexture("Animation.Play")))
            {
                Sound.sound.Sfx.PlayOneShot(obj);
            }
            if (GUI.Button(Position(position, 25), EditorGUIUtility.FindTexture("animationvisibilitytoggleon@2x")))
            {
                EditorGUIUtility.PingObject(obj);
            }
        }


        // Set indent back to what it was
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
#endif
