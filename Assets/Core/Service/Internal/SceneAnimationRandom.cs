using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAnimationRandom : MonoBehaviour
{

    public Animation anim;
    public Transform root;


    [Header("Animation Clips --------")]
    public bool isRandomAnimClip;
    public AnimationClip[] random_clips;

    [Header("Position --------")]
    public bool isRandomPosition;
    public Vector3[] random_positions;

    [Header("Position --------")]
    public Transform isRandomScale;
    public float[] random_scales;
    Vector3 def_scale = Vector3.zero;

    [Header("Time Refresh --------")]
    public bool isRandomTimeRefresh;
    public float[] random_times;


   

    void Start()
    {
        root.gameObject.SetActive(false);
        anim.Stop();
        float starttime = 0.0f;
        if (isRandomTimeRefresh) 
        {
            starttime =  Random.RandomRange( random_times[0] , random_times[1]) ;
        }
        Service.Timmer.Wait(starttime , gameObject , () => {
            Play();
        }).ID = $"[ Wait ] [{gameObject.name}] - SceneAnimationRandom";

    }


    void Play()
    {
        if (isRandomPosition)
        {
            root.localPosition = Service.Vector.BetweenRandomVector(random_positions[0], random_positions[1]);
        }
        if (isRandomScale) 
        {
            if (def_scale == Vector3.zero)
                def_scale = isRandomScale.localScale;
            isRandomScale.localScale = def_scale * Random.RandomRange(random_scales[0], random_scales[1]);
        }
        root.gameObject.SetActive(true);
        AnimationClip clip = anim.clip;
        if (isRandomAnimClip) 
        {
            clip = random_clips[ Random.RandomRange(0, random_clips.Length) ];
        }
        anim.Stop();
        anim.Play(clip.name);
        Service.Timmer.Wait(clip.length , gameObject ,()=> {
            root.gameObject.SetActive(false);
            Start();
        }).ID = $"[ Playing ] [{gameObject.name}] - SceneAnimationRandom";
    }


}
