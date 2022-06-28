using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesService 
{
    public static GameObject LoadGameObject(string path)
    {
        return Resources.Load(path) as GameObject;
    }
    public static Texture LoadTexture(string path)
    {
        return Resources.Load(path) as Texture;
    }
    public static Sprite LoadSptire(string path)
    {
        return Resources.Load(path) as Sprite;
    }
    public static Sprite[] LoadSptires(string path)
    {
        object[] loadedsprite = Resources.LoadAll(path, typeof(Sprite));
        var sptires = new Sprite[loadedsprite.Length];
        //this
        for (int x = 0; x < loadedsprite.Length; x++)
        {
            sptires[x] = (Sprite)loadedsprite[x];
        }
        return sptires;
    }
    public static Object[] LoadAllObjects(string path)
    {
        Object[] list = (Object[])Resources.LoadAll(path);
        return list;
    }
    public static UnityEngine.Video.VideoClip LoadVideo(string path)
    {
        return Resources.Load(path) as UnityEngine.Video.VideoClip;
    }
    public static AudioClip LoadAudioClip(string path)
    {
        return Resources.Load(path) as AudioClip;
    }
    public static AnimationClip LoadAnimationClip(string path)
    {
        return Resources.Load(path) as AnimationClip;
    }
}
