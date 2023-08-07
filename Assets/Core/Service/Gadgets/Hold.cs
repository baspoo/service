using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hold : MonoBehaviour
{


    bool isHold = false;
    float timehold = 0.0f;

    public float timehold_toComplete;
    public System.Action<float> onHolding;
    public System.Action onComplete;
    public System.Action onCancel;

    UIEventTrigger eventTrigger;

    //** -----------------------------------------------------------
    //** Init
    //** -----------------------------------------------------------

    public void Init(System.Action onComplete, System.Action onCancel, System.Action<float> onHolding = null)
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
        onHolding?.Invoke(timehold);
    }

    public void Complete()
    {
        //Debug.Log("Complete");
        isHold = false;
        onComplete?.Invoke();
    }

    public void Cancel()
    {
        //Debug.Log("Cancel");
        onCancel?.Invoke();
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
