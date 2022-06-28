using UnityEngine;
using System.Collections;

public class SpriteRendererAnimation : MonoBehaviour
{

    public SpriteRenderer SpriteRenderer_;
    public Sprite[] spr;
    public float speed;
    public float taketime;
    public bool loop;
    public bool onceDestory;
    public EventDelegate onEndAnimation;


    Service.Callback.callback m_onFinish;
    Service.Callback.callback m_onTake;
    public void PlayStart(Service.Callback.callback onTake = null,Service.Callback.callback onFinish = null)
    {
        m_onTake = onTake;
        m_onFinish = onFinish;
        play = true;
        isTake = false;
        runtime = 0.0f;
        index = 0;
    }

    bool play = true;
    bool isTake = false;
    int index;
    float runtime = 0.0f;

    void Update()
    {
        if (play)
        {
            if (runtime < 1.0f)
                runtime += speed * Time.deltaTime;
            else
            {
                if (index >= spr.Length)
                {
                    isTake = false;
                    if (loop) index = 0;
                    else { play = false; index--; }
                    if (onceDestory) Destroy(gameObject);
                    if (onEndAnimation != null) onEndAnimation.Execute();
                    if (m_onFinish != null) 
                        m_onFinish();
                }
                if (SpriteRenderer_ == null) 
                    gameObject.GetComponent<SpriteRenderer>().sprite = spr[index];
                else 
                    SpriteRenderer_.sprite = spr[index];


                if (!isTake && taketime > 0 ) 
                {
                    if ( ((float)index / (float)spr.Length) >= taketime) 
                    {
                        if (m_onTake != null)
                            m_onTake();
                        isTake = true;
                    }
                }
                
                runtime = 0.0f;
                index++;

            }
        }
    }
}
