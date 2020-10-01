using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{




    public List<StepData> StepDatas = new List<StepData>();

    [System.Serializable]
    public class StepData {
        public string stepName;
        public bool active;
        public AnimationClip anim;
        public AudioClip clip;
        public Transform tran;
        public  ParticleSystem paticle;
        public bool stoploop;
        public EventDelegate eventDele;
    }


    public void Event(string stepName)
    {

        StepData SD = StepDatas.Find(x=>x.stepName == stepName);
        if (SD == null) return;

        if (SD.anim != null) 
            GetComponent<Animation>()?.Play(SD.anim.name);
        if (SD.clip != null)
            Sound.Play(SD.clip);
        if (SD.tran != null)
            SD.tran.gameObject.SetActive(SD.active);
        if (SD.paticle != null)
        {
            SD.paticle.Stop();
            if (SD.active) SD.paticle.Play();
            if (SD.stoploop) ParticleReReady.StopLoopAllParent(SD.paticle.transform);
        }
        if (SD.eventDele != null)
            SD.eventDele.Execute();
    }



}
