using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void _OnGameStarted();
    [DllImport("__Internal")]
    private static extern void _ExitFullScreen();
    public int gameNumber;
    public GameObject[] Panels_List;
    Fade[] mode;
	public int noOfLevels;
    public string SceneName;
    public int levelCounter = 0;
    public static GameManager Instance;
    public float gameSpeed = 1;
    public bool changeGameSpeed;
    public int pauseStopPressedCount;
    void Awake()
    {
        Instance = this;
    }
    void OnEnable()
    {
        PlayerPrefs.SetInt("Click", 0);
#if !UNITY_EDITOR
        _OnGameStarted();
#endif
    }
    void Start()
    {
        mode = new Fade[Panels_List.Length]; // initailizing the mode with repect to the level of panel's length

        for (int i = 0; i < Panels_List.Length; i++)
        {
            mode[i] = Panels_List[i].GetComponent<Fade>();
        }
    }
    void Update()
    {
        if(changeGameSpeed)
        {
            changeGameSpeed = false;
            #if UNITY_EDITOR //for testing purposes in editor
            Time.timeScale = gameSpeed;
            #endif
        }
    }
	public void ClickOn(float delay=0)
	{
		StartCoroutine (ClickHandle(delay,1));
	}
	public void ClickOff(float delay=0)
	{
		StartCoroutine (ClickHandle(delay,0));
	}
	public bool CanClick()
	{
		bool check = false;
		if (PlayerPrefs.GetInt ("Click") == 0)
			check = false;
		else if (PlayerPrefs.GetInt ("Click") == 1)
			check = true;
		return check;
			
	}
	IEnumerator ClickHandle(float delay, int state)
	{
		yield return new WaitForSeconds (delay);
		PlayerPrefs.SetInt ("Click", state);
	}
    public void OnButtonClicked(string BtnName) 
    {
		if (BtnName == "FullScrren")
        {
            if (Screen.fullScreen)
            {
                Screen.fullScreen = false;
                FullScreenBtn.Instance.IMG.sprite = FullScreenBtn.Instance.FullScreenIMG[0];
            }
            else
            {
                FullScreenBtn.Instance.IMG.sprite = FullScreenBtn.Instance.FullScreenIMG[1];
                Screen.fullScreen = true;
            }
        }
    }
	public float levelDelay;
	public IEnumerator StartNewLevel(int Ind, float sec)
	{
		if (sec != 0)
		levelDelay = sec;
		yield return new WaitForSeconds (levelDelay);

		if (levelCounter == noOfLevels) {
			Panels_List [noOfLevels-1].GetComponent<Fade> ().Fadeout = true;
			yield return new WaitForSeconds (1.2f);
			Panels_List [noOfLevels-1].SetActive (false);
			MenuManager.instance.EnableFinalScreen();
		} else {

			for (int i = 0; i < Panels_List.Length; i++) {
				if (i == Ind) {
					Panels_List [i].SetActive (true);
				} else {
					Panels_List [i].SetActive (false);
				}
			}
			levelCounter++;
		}
	}
    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.1f);
        if (Screen.fullScreen)
        {
            _ExitFullScreen();
            Screen.fullScreen = !Screen.fullScreen;
        }
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }
    public bool GameNumber(params int[] gameNumbers)
    {
		for (int i = 0; i < gameNumbers.Length; i++)
        {
            if(gameNumbers[i] == gameNumber)
                return true;
        }
        return false;
    }
}