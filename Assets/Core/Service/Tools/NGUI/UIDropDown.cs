using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDropDown : MonoBehaviour
{
    public enum Pivot { Down , Up }
    public Pivot pivot;
    public List<string> Contents;
    public Transform root;
    public UILabel ui_lbname;
    public UIButton btn;
    public UITexture ui_imgbar;
    public UITexture ui_imgbg;
    GameObject popup;
    public EventDelegate EventChange;
    List<string> m_disble = new List<string>();

    public string value { get { return m_value; } }
    string m_value;
    public int Index { get { return Contents.FindIndex(x => x == m_value); } }

    void Start()
    {
        OnChange(0);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
           if(isActive)
                StartCoroutine(IEClose());
        }
    }







    public void OnAddDisble(string content )
    {
        m_disble.Add(content);
    }
    public void OnClearDisble( )
    {
        m_disble.Clear();
    }







    public void OnChange( int Index  , bool isTriggerEvent = false) 
    {
        m_value = Contents[Index];
        ui_lbname.text = m_value;//language(m_value);
        if(isTriggerEvent)
            if (EventChange != null)
                EventChange.Execute();
    }
    public void OnChange(string content , bool isTriggerEvent = false)
    {
        int index = Contents.FindIndex(x => x == content);
        if (index != -1)
            OnChange( index , isTriggerEvent );
    }
    public void Open( ) 
    {
        if(popup != null)
            Destroy(popup.gameObject);

        float fector = -1.0f;
        var uipivot = UIWidget.Pivot.Top;
        var bgpivot = UIWidget.Pivot.Top;
        if (pivot == Pivot.Up) 
        {
            fector = 1.0f;
            uipivot = UIWidget.Pivot.Bottom;
            bgpivot = UIWidget.Pivot.Bottom;
        }



        popup = new GameObject("popup");
        popup.transform.parent = root;
        popup.transform.localPosition = Vector3.zero;
        popup.transform.localScale = Vector3.one;

        UIGrid grid = (new GameObject("grid")).AddComponent<UIGrid>();
        grid.transform.parent = popup.transform;
        grid.transform.localPosition = Vector3.zero;
        grid.transform.localScale = Vector3.one;
        grid.pivot = uipivot;
        grid.cellHeight = ui_imgbar.height + 5.0f;
        grid.arrangement = UIGrid.Arrangement.Vertical;
        foreach (var c in Contents)
            GenBar(c, grid.transform);
        grid.repositionNow = true;
        btn.isEnabled = false;



        UITexture BG = Instantiate( ui_imgbg, popup.transform);
        Service.GameObj.DesAllParent(BG.transform);
        BG.pivot = bgpivot;
        BG.depth += 1;
        BG.width += 20;
        BG.height = (int) ((grid.cellHeight * Contents.Count) + 20);
        BG.transform.localPosition = new Vector3(0.0f, ((grid.cellHeight + 20) / 2.0f ) * (-1.0f * fector), 0.0f);
        //BG.tr

        UIWidget widget = popup.gameObject.AddComponent<UIWidget>();

        TweenAlpha alpha = popup.gameObject.AddComponent<TweenAlpha>();
        alpha.from = 0.0f;
        alpha.to = 1.0f;
        alpha.duration = 0.2f;

        TweenScale tween = popup.gameObject.AddComponent<TweenScale>();
        tween.from = new Vector3(1.0f,0.0f,1.0f);
        tween.to = new Vector3(1.0f, 1.0f, 1.0f);
        tween.duration = 0.15f;

        TweenPosition position = popup.gameObject.AddComponent<TweenPosition>();
        position.from = new Vector3(0.0f, 0.0f, 0.0f);
        position.to = new Vector3(0.0f, ui_imgbar.height * fector , 0.0f);
        position.duration = 0.15f;

        isActive = true;
    }




    void GenBar( string content , Transform parent)
    {
        UITexture Bar = Instantiate( ui_imgbar , parent ); 
        Service.GameObj.DesAllParent(Bar.transform);
        Bar.depth+=2;
        //Bar.gameObject.AddComponent<BoxCollider>();
        //Bar.autoResizeBoxCollider = true;


        UILabel Text = Instantiate( ui_lbname , Bar.transform); 
        Service.GameObj.DesAllParent(Text.transform);
        Text.depth+=2;
        Text.text = content; //language(content);
        Text.transform.parent = Bar.transform;
        Text.transform.localPosition = Vector3.zero;



        UIButton Btn = Bar.GetComponent<UIButton>();
        Bar.color = Btn.defaultColor;
        Btn.ResetDefaultColor();
        Btn.onClick.Clear();
        Btn.onClick.Add(new EventDelegate(()=> {
            Debug.Log(content);
            Onclose();
            OnChange(content,true);
        })) ;

        Btn.isEnabled = !m_disble.Contains(content);
    }


    bool isActive = false;
    public void Onclose() 
    {
        isActive = false;
        btn.isEnabled = true;
        popup.gameObject.SetActive(false);
    }
    IEnumerator IEClose() 
    {
        yield return new WaitForSeconds(0.15f);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Onclose();
    }




}
