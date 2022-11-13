using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class NGUIservice 
{

}






public static class NGUIserviceFunction
{



    public static UIButton DisableActive(this List<UIButton> btns, string btnName) {
        btns.ForEach(x => x.isEnabled = true);
        var btn = btns.Find(x => x.name == btnName);
        if (btn != null) btn.isEnabled = false;
        return btn;
    }
    public static UIButton DisableActive(this List<UIButton> btns, UIButton btnName)
    {
        btns.ForEach(x => x.isEnabled = true);
        var btn = btns.Find(x => x == btnName);
        if (btn != null) btn.isEnabled = false;
        return btn;
    }


    public static void OnChangeColor( this UIButton btn , Color? normal , Color? hover, Color? pressed, Color? disabledColor)
    {
        if(normal!=null) btn.defaultColor = (Color)normal;
        if (hover != null) btn.hover = (Color)hover;
        if (pressed != null) btn.pressed = (Color)pressed;
        if (disabledColor != null) btn.disabledColor = (Color)disabledColor;
        btn.UpdateColor(true);
    }
    public static void OnChangeColor(this UIButton btn, Color[] colors )
    {
        if (colors.Length > 0) btn.defaultColor = colors[0];
        if (colors.Length > 1) btn.hover = colors[1];
        if (colors.Length > 2) btn.pressed = colors[2];
        if (colors.Length > 3) btn.disabledColor = colors[3];
        btn.UpdateColor(true);
    }

}