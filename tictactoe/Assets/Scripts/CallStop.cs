using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class CallStop : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void _OnGameStopped();
    [DllImport("__Internal")]
    private static extern void _ExitFullScreen();

    public float WaitingTime = 5f;
    public string SceneName;

    void OnEnable()
    {
        StartCoroutine(WaitToCallStop());
    }

    IEnumerator WaitToCallStop()
    {
        yield return new WaitForSeconds(WaitingTime);
        Debug.Log("Game Stopped Called");
        StopGame();

    }

    public void StopGame()
    {
        if (Screen.fullScreen)
        {
#if !UNITY_EDITOR
            _ExitFullScreen();
#endif
            Screen.fullScreen = !Screen.fullScreen;
        }
        ReloadScene();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }
}
