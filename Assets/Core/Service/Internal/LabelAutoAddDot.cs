using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelAutoAddDot : MonoBehaviour
{
    public UILabel label;
    public bool isdropdown;
    void Start()
    {
        if(label == null) {
            label = gameObject.GetComponent<UILabel>();
        }
        LetsAddDot(label, isdropdown);

    }

    public static bool LetsAddDot(UILabel label , bool isdropdown = false )
    {
        bool isAddDot = false;
        if (label != null)
        {
            // Force change to Clamp
            label.overflowMethod = UILabel.Overflow.ClampContent;

            // Check text and replace with "..."
            string text = label.processedText;
            isAddDot = label.text.Length > label.processedText.Length;
            if (isAddDot)
            {
                if(!isdropdown) text = text.Substring(0, label.processedText.Length - 3) + "...";
                else text = text.Substring(0, label.processedText.Length - 4) + "..▼";

                label.text = text;
            }
        }

        else
        {
            Debug.Log("[ LabelAutoAddDot ] not work - UILabel is null!");
        }

        return isAddDot;
    }
}
