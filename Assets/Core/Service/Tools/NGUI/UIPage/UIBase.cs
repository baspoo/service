using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif





//Sub-Class On UIRoot Main Interface
public class UIRootBase : UIBase
{
    public UIRootSetting rootsetting;
    [System.Serializable]
    public class UIRootSetting
    {
        public Camera mainCamera;
        public Camera uiCamera;
        public UIRoot uiRoot;
        public UIPanel uiPanel;
        public ScreenPoint screenPoint;
        public Transform transMask;
        public Transform transCloseScreen;
        public Transform rootPage;
        public Transform rootPool;


        public UIConfig config;
        [System.Serializable]
        public class UIConfig
        {
            public float bubbleSize = 1.0f;
        }
    }
    public static UIRootBase instance;

    public void Init()
    {
        instance = this;
    }
    bool isMask => rootsetting.transMask.gameObject.activeSelf;
    List<Transform> notClickRequire = new List<Transform>();
    public void OpenMask(bool visible, Transform root = null)
    {
        if (root != null)
        {
            if (visible)
            {
                if (!notClickRequire.Contains(root))
                    notClickRequire.Add(root);
            }
            else
                notClickRequire.Remove(root);
        }
        if (!visible)
        {
            if (notClickRequire.Count != 0)
                return;
        }
        rootsetting.transMask.gameObject.SetActive(visible);
    }

    public void OnCreateEffect(GameObject effect, Transform position)
    {
        PoolManager.CreateNewPoolGroup(effect, rootsetting.rootPool);
        var g = PoolManager.SpawParent(effect, rootsetting.rootPool, 3.0f).gameObject;
        g.SetActive(false);
        g.transform.position = position.position;
    }




}



//Sub-Class On All Page
public class UIBase : MonoBehaviour
{
#if UNITY_EDITOR
    public RuntimeBtn FindPanel = new RuntimeBtn("Find", (r) => {

        UIBase m_base = ((GameObject)Selection.activeObject).GetComponent<UIBase>();
        if (m_base)
        {
            m_base.settingpage.UIPanels.Clear();

            m_base.panel = m_base.GetComponent<UIPanel>();
            if(m_base.panel!=null)
                m_base.settingpage.UIPanels.Add(m_base.panel);
            foreach (var g in m_base.gameObject.transform.GetAllNode())
            {
                var panel = g.GetComponent<UIPanel>();
                if (panel != null)
                    m_base.settingpage.UIPanels.Add(panel);
            }

            //RootUIBubble
            if (m_base.settingpage.RootUIBubble == null)
                m_base.settingpage.RootUIBubble = m_base.transform;

            m_base.FindPanel.BtnName = $"Find [{m_base.settingpage.UIPanels.Count}]";
        }
        else Debug.LogError("Not Found UIBase!");

    });
#endif
    [System.Serializable]
    public class UISettingPage 
    {
        public Transform RootUIBubble;
        public PageLayer pageLayer;
        public List<UIPanel> UIPanels = new List<UIPanel>();
        public enum PageLayer
        {
            None , Background , Console , Mask , Page , Popup , Notif , System
        }
        public enum Sfx
        {
            none, page , movepage
        }

        public bool DontDestoryOnClose;
        [Header("On Open")]
        public bool OpenMask;
        public Sfx OpenSfx;

        [Header("On Close")]
        public bool CloseMask;
        public Sfx CloseSfx;

        public TaskService.Function EventOnClose = new TaskService.Function();
    }


    public UIRootBase root => UIRootBase.instance;
    //public static Store.Pages Pages => Store.instance.page;
    bool IsActive = false;

    static Dictionary<GameObject, GameObject> m_stock = new Dictionary<GameObject, GameObject>();
    public static T CreatePage<T>(GameObject page) 
    {
        GameObject Create() 
        {
            var np = page.Create(UIRootBase.instance.rootsetting.rootPage);
            return np;
        } 
        GameObject newpage = null;
        if (!m_stock.ContainsKey(page))
            newpage = Create();
        else
        {
            newpage = m_stock[page];
            if (newpage != null) newpage.SetActive(true);
            else 
            {
                m_stock.Remove(page);
                newpage = Create();
            }
        }

        //UIBase
        var uibase = newpage.GetComponent<UIBase>();
        uibase.name = $"{page.name} [depth - { ((uibase.panel!=null)? uibase.panel.depth : 0) }]";
        if (uibase.settingpage.OpenMask)
            uibase.root.OpenMask(true, uibase.transform);
        uibase.HandleSound(uibase.settingpage.OpenSfx);
        if (!m_stock.ContainsKey(page) && uibase.settingpage.DontDestoryOnClose)
            m_stock.Add(page, newpage);

        uibase.DoPanel();
        uibase.IsActive = true;
        return newpage.GetComponent<T>();
    }





    Dictionary<UISettingPage.PageLayer, int> pagePanelDepth = new Dictionary<UISettingPage.PageLayer, int> {
        { UISettingPage.PageLayer.None, 0 },
         { UISettingPage.PageLayer.Background, 50 },
          { UISettingPage.PageLayer.Console, 100 },
           { UISettingPage.PageLayer.Mask, 150 },
            { UISettingPage.PageLayer.Page, 200 },
             { UISettingPage.PageLayer.Popup, 250 },
              { UISettingPage.PageLayer.Notif, 300 },
              { UISettingPage.PageLayer.System, 350 }
    };

    //[HideInInspector]
    bool AdjustPanels = false;
    void DoPanel() {
        if (AdjustPanels)
            return;

        //Debug.LogError($"DoPanel [{gameObject.name}] : " + settingpage.pageLayer);
        AdjustPanels = true;
        foreach (var panel in settingpage.UIPanels) 
        {
            //Debug.LogError($"Depth [{panel.depth}] + [{ pagePanelDepth[settingpage.pageLayer]}]");
            panel.depth += pagePanelDepth[settingpage.pageLayer];
        }
    }

     

   
    public UISettingPage settingpage;
    public UIPanel panel;
    public void OnVisible(bool visible)
    {
        if(panel!=null) panel.alpha = visible ? 1.0f : 0.0f;
    }

    


    protected TaskService.Function EventOnClose => settingpage.EventOnClose;


    public void OnClose()
    {
        
        HandleClose();

        if (settingpage.DontDestoryOnClose)
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
        IsActive = false;
    }


    void HandleClose() {

        if (!IsActive) return;

        UIBubble.OnClose(gameObject.name);

        if (settingpage.CloseMask)
            root.OpenMask(false, transform);
        HandleSound(settingpage.CloseSfx);
        EventOnClose?.callall();
    }


    public void OnHide(bool Hide)
    {
        if (!IsActive) return;

        gameObject.SetActive(!Hide);

        if (!Hide && settingpage.OpenMask)
            root.OpenMask(true, transform);
        if (Hide && settingpage.CloseMask)
            root.OpenMask(false, transform);
    }

    public T UIPool<T>(GameObject obj, Transform trans , float wait = 0.0f )
    {
        return UIPool(obj, trans, wait).GetComponent<T>();
    }
    public GameObject UIPool(GameObject obj, Transform trans, float wait = 0.0f)
    {
        PoolManager.CreateNewPoolGroup(obj, root.rootsetting.rootPool);
        var g = PoolManager.SpawParent(obj, trans, wait).gameObject;
        g.SetActive(false);
        RefreshTime(() => { g.SetActive(true); });
        return g;
    }
    public GameObject UIPoolPosition(GameObject obj, Transform trans, float wait = 0.0f)
    {
        PoolManager.CreateNewPoolGroup(obj, root.rootsetting.rootPool);
        var g = PoolManager.SpawParent(obj, trans, wait).gameObject;
        g.transform.parent = root.rootsetting.rootPool;
        g.SetActive(false);
        RefreshTime(() => { g.SetActive(true); });
        return g;
    }


    public void RefreshTime( System.Action actionToRefresh) => StartCoroutine(IERefresh(1, (i)=> { actionToRefresh?.Invoke(); }));
    public void RefreshTime(int time, System.Action<int> actionToRefresh) => StartCoroutine(IERefresh(time, actionToRefresh));
    IEnumerator IERefresh(int time, System.Action<int> actionToRefresh) {
        for (int i = 0; i < time; i++) 
        {
            yield return new WaitForEndOfFrame();
            actionToRefresh?.Invoke(i);
        }
    }

    public void SnapAtPanel(Transform target) {
        RefreshTime(3, (i) => {
            var mBounds = NGUIMath.CalculateRelativeWidgetBounds(panel.cachedTransform, target);
            panel.ConstrainTargetToBounds(target, ref mBounds, true);
        });
    }


    List<UIBubble> UIBubbles = new List<UIBubble>();
    public UIBubble AddUIBubble(Texture Icon , Transform target3D , System.Action action ) 
    {
        var Bubble =  UIBubble.Open( Icon , target3D , settingpage.RootUIBubble != null ? settingpage.RootUIBubble : transform, action , gameObject.name );
        UIBubbles.Add(Bubble);
        return Bubble;
    }


    public UIBubble AddUIBubble(Texture Icon, Transform target3D, System.Action action, string name)
    {
        var Bubble = UIBubble.Open(Icon, target3D, transform, action, name);
        UIBubbles.Add(Bubble);
        return Bubble;
    }

    public void DesAllUIBubble( )
    {
        UIBubble.OnClose(UIBubbles);
        UIBubbles = new List<UIBubble>();
    }

    void HandleSound(UISettingPage.Sfx sfx)
    {
        switch (sfx)
        {
            case UISettingPage.Sfx.movepage: Playlist.let.sfx_movepage.Play(); break;
            case UISettingPage.Sfx.page: Playlist.let.sfx_pages.Play(); break;
        }
    }





}














