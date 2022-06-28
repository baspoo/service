using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBubble : UIBase
{
    public static List<UIBubble> UIBubbles => instances;
    static List<UIBubble> instances = new List<UIBubble>();
    public static UIBubble Open( Texture Icon , Transform target3D , Transform page , System.Action action , string group = null )
    {
        //var instance =   UIRootBase.instance.UIPool<UIBubble>( Pages.prefab_bubblePage, page);
        var instance = UIRootBase.instance.UIPool<UIBubble>(  NguiPackage.instance.guiObject.Bubble  , page);
        instance.Init(Icon,target3D, action);
        instances.Add(instance);
        return instance;
    }
    public static List<UIBubble> Find(string groupName)
    {
        return UIBubbles.FindAll(x=>x.group == groupName);
    }
    public static void OnClose(string groupName)
    {
        Find(groupName).ForEach(x => x.OnCloseBubble());
    }
    public static void OnClose(List<UIBubble> UIBubbles)
    {
        UIBubbles.ForEach(x=>x.OnCloseBubble());
    }


    Texture ImgDefault = null;
    public bool isActive => pool != null ? pool.isActive : false;

    public UITexture imgIcon;
    public UITexture imgBg;
    public Transform popup;
    public string group;


    //public Material mat_blink;

    PoolObj pool;
    [SerializeField]Transform target3D;
    System.Action action;
    ScreenPoint.Looking look;
    private void Init(Texture Icon ,Transform target3D, System.Action action, string group = null)
    {

        if(ImgDefault == null)
            ImgDefault = imgIcon.mainTexture;

        pool = GetComponent<PoolObj>();
        this.group = group;
        this.target3D = target3D;
        this.action = action;
        imgIcon.mainTexture = Icon!=null? Icon : ImgDefault;
        look = root.rootsetting.screenPoint.AddLooking(transform, target3D);
    }
    //[SerializeField] float distance;
    //[SerializeField] float size;
    private void Update()
    {
        imgBg.depth = ((int)transform.localPosition.y) * -1;
        imgIcon.depth = imgBg.depth + 1;
        popup.localScale = Vector3.one * root.rootsetting.config.bubbleSize;
    }

    public void OnSubmit() {
        Playlist.let.sfx_click.Play();
        action?.Invoke();
    }
    public void OnCloseBubble()
    {
        if (look != null)
            root.rootsetting.screenPoint.RemoveLooking(look);
        pool.Deactive();
        instances.Remove(this);
    }


}
