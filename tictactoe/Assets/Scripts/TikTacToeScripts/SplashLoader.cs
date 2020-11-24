using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashLoader : MonoBehaviour
{

    private AsyncOperation Loader;
    public Image LoadingBar;

    // Start is called before the first frame update
    
    IEnumerator Start ()
    {
        Loader = SceneManager.LoadSceneAsync(1);
        yield return Loader;
    }

    private void Update()
    {
        LoadingBar.fillAmount = Loader.progress;
    }
}
