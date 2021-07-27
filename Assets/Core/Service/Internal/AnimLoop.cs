using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimLoop : MonoBehaviour
{
    public Animation anim;


    [Tooltip("loopclip : ถ้าไม่ใส่จะใช้ตัว default / ถ้าใส่ไว้จะใช้ clip อันนี้")]
    public AnimationClip loopclip;
    public bool IsUnloop;
    float savepoint;


    string playingName
    {
        get
        {
            if (loopclip != null)
                return loopclip.name;
            else
                return anim.clip.name;
        }
    }

    public void Begin()
    {
        savepoint = anim[playingName].time;
    }
    public void End()
    {
        if (!IsUnloop)
            anim[playingName].time = savepoint;
    }




}
