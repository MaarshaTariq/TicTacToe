using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMarkerFetch : MonoBehaviour
{
    public Sprite [] MarkerImages;
    public static PlayerMarkerFetch Instance;
    public void Start()
    {
        Instance = this;
    }
}


