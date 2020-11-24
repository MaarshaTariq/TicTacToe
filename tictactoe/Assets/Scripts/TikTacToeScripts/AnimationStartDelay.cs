using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStartDelay : MonoBehaviour
{
    public float DelayTime;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("AnimationStart", DelayTime);
    }

    void AnimationStart()
    {
        GetComponent<Animator>().enabled = true;
    }
}
