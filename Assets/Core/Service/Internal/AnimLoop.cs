using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimLoop : MonoBehaviour
{
    public Animation anim;
    public bool IsUnloop;
    float savepoint ;
    public void Begin()
    {
        savepoint = anim[anim.clip.name].time;
    }
    public void End()
    {
        if(!IsUnloop)
            anim[anim.clip.name].time = savepoint;
    }




 
}
