using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIHotKeyBtn : MonoBehaviour
{

    public KeyCode Key;
    public enum PositionType { Down , Top , Center }
    public PositionType Position;
    public float OffsetVolume = 1.0f;

    UIButton btn;
    UITexture texture;
    UILabel HotKeyObj;


    void Start()
    {
        btn = GetComponent<UIButton>();
        texture = GetComponent<UITexture>();
        DoApply();
    }
    void Update()
    {
        if (Input.GetKeyDown(Key))
            DoInput();
    }
    bool IsEnable
    {
        get {
            if (!gameObject.activeSelf)
                return false;
            if (btn == false)
                return false;
            if (!btn.isEnabled)
                return false;

            if (texture != null)
            {
                if (!texture.isVisible)
                    return false;
                if (texture.panel == null)
                    return false;
                if (texture.panel.enabled == false)
                    return false;
                if (texture.panel.alpha == 0.0f)
                    return false;

                Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity);
                int i = 0;
                int maxDepth = 0;
                while (i < hitColliders.Length)
                {
                    var depth = NGUITools.CalculateRaycastDepth(hitColliders[i].gameObject);
                    if (depth > maxDepth) maxDepth = depth;
                    i++;
                }
                if (maxDepth > NGUITools.CalculateRaycastDepth(texture.gameObject))
                    return false;
            }


            return true;
        }
    }
    
    void DoApply() 
    {

        if (HotKeyObj == null)
            HotKeyObj = NguiPackage.instance.guiObject.HotKey.GetComponent<UILabel>();
        HotKeyObj.text = $"({Key})";



        if (texture != null)
        {
            //hide out of screen
            texture.hideIfOffScreen = true;

            //position
            HotKeyObj.transform.localPosition = texture.localCenter;
            var position = 0.0f;
            if (Position == PositionType.Down)
                position = HotKeyObj.transform.localPosition.y - ((texture.height / 2) * OffsetVolume);
            if (Position == PositionType.Top)
                position = HotKeyObj.transform.localPosition.y + ((texture.height / 2) * OffsetVolume);
            if (Position == PositionType.Center)
                position = HotKeyObj.transform.localPosition.y;
            HotKeyObj.transform.ChangeCurrentLocalPosition(null, position, null);

            //depth
            HotKeyObj.depth = texture.depth + 2;
            HotKeyObj.transform.GetChild(0).GetComponent<UITexture>().depth = texture.depth + 1;
        }
    }
    void DoInput()
    {
        if (IsEnable)
        {
            foreach (var eventExec in btn.onClick)
                eventExec.Execute();
        }
    }

    public void DebugText() {
        Debug.Log("XXXX");
    }
    [ContextMenu("Do Update")]
    void DoSomething()
    {
        DoApply();
    }


    public void OnTooltip(bool enter) {
        //Debug.Log(enter);
    }
}
