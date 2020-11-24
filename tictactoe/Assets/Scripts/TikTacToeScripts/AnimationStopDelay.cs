using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStopDelay : MonoBehaviour
{

    public float DelayTime;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("StopAnimation", DelayTime);
    }

    // Update is called once per frame
    void StopAnimation()
    {
        GetComponent<Animator>().enabled = false;
    }
}
