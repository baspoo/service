using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hold : MonoBehaviour
{


    bool isHold = false;
    float timehold = 0.0f;

    public float timehold_toComplete;
    public Service.Callback.callback_fvalue onHolding;
    public Service.Callback.callback onComplete;
    public Service.Callback.callback onCancel;

    UIEventTrigger eventTrigger;

    //** -----------------------------------------------------------
    //** Init
    //** -----------------------------------------------------------

    public void Init(Service.Callback.callback onComplete, Service.Callback.callback onCancel, Service.Callback.callback_fvalue onHolding = null)
    {
        this.onComplete = onComplete;
        this.onCancel = onCancel;
        this.onHolding = onHolding;

        //** Setup UIEventTrigger 
        eventTrigger = gameObject.GetComponent<UIEventTrigger>();
        if(eventTrigger == null)
            eventTrigger = gameObject.AddComponent<UIEventTrigger>();

        eventTrigger.onPress.Add(new EventDelegate(OnHold));
        eventTrigger.onRelease.Add(new EventDelegate(OnExit));
        eventTrigger.onDragOver.Add(new EventDelegate(OnExit));


    }



    //** -----------------------------------------------------------
    //** Trigger from object
    //** -----------------------------------------------------------

    public void OnHold()
    {
        //Debug.Log("OnHold");
        isHold = true;
        timehold = 0.0f;
    }

    public void OnExit()
    {
        //Debug.Log("OnRelease");
        Cancel();
        isHold = false;
        timehold = 0.0f;

    }


    //** -----------------------------------------------------------
    //** Callback from another class
    //** -----------------------------------------------------------

    public void Holding()
    {
        if (onHolding != null)
            onHolding(timehold);
    }

    public void Complete()
    {
        //Debug.Log("Complete");
        isHold = false;
        if (onComplete != null)
        {
            onComplete();
        }

    }

    public void Cancel()
    {
        //Debug.Log("Cancel");
        if (onCancel != null)
        {
            onCancel();
        }
    }


    //** -----------------------------------------------------------
    //** Update
    //** -----------------------------------------------------------

    void Update()
    {
        if (isHold)
        {
            timehold += Time.deltaTime;
            if (timehold >= timehold_toComplete)
            {
                Complete();
            }

        }

    }
}
