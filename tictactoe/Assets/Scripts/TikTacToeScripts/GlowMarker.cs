using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlowMarker : MonoBehaviour
{

    Image TargetImage;
    float FillRate = 0.0f;

    private void Start()
    {
        TargetImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        FillRate = FillRate + 0.05f;
        TargetImage.fillAmount = FillRate;
    }
}
