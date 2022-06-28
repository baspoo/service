using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailAwake : MonoBehaviour
{


    public TrailRenderer trail;
    float m_time = -1.0f;
    
    void OnEnable()
    {
        if (m_time == -1.0f)
            m_time = trail.time;

        StartCoroutine(awake());
    }
    IEnumerator awake() {
        trail.Clear();
        trail.enabled = false;
        trail.time = -1.0f;
        yield return new WaitForSeconds(0.05f);
        trail.Clear();
        trail.time = m_time;
        trail.enabled = true;
    }


    void OnDisable()
    {
        trail.Clear();
        trail.enabled = false;
        trail.time = -1.0f;
    }
}
