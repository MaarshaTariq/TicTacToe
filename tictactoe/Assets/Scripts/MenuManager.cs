using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour {
	
	[DllImport("__Internal")]
	private static extern void _OnGameStopped();
	[DllImport("__Internal")]
	private static extern void _ExitFullScreen();
	public float startDelay;
	public bool isMenu;
	public string SceneName;
	public static MenuManager instance;
	bool CanClickUp = false;
	public bool isStart;
	public bool isFinal;
	// Use this for initialization
	void Start () {
		instance = this;
		PlayerPrefs.SetInt ("Pause", 0);
	}
	
	// Update is called once per frame
	void Update () {

		foreach (Touch touch in Input.touches)
		{
			if (PlayerPrefs.GetInt ("Pause") == 1 || !GameManager.Instance.CanClick() || (isStart==false && isFinal==false))
				return;
			switch (touch.phase)
			{
			case TouchPhase.Began:
			if (EventSystem.current.currentSelectedGameObject)
				{            //if any button on screen is touched rather than empty spac
					//do nothing when button is touched
				}
				else
				{
					CanClickUp = true;
				}
				break;
			case TouchPhase.Ended:
				if (CanClickUp)
				{
					if (isStart &&  PlayerPrefs.GetInt ("Pause")==0) {
						PlayGame ();
					}
					if (isFinal &&  PlayerPrefs.GetInt ("Pause")==0) {
						isStart = false;
					}
				
					CanClickUp = false;
				}
				break;
			}
		}
		if (Input.GetKeyDown(KeyCode.Space) && (isStart || isFinal))
		{
			CanClickUp = true;
		}
		if (Input.GetKeyUp(KeyCode.Space) && CanClickUp  && (isStart || isFinal))
		{

			if (isStart) {
				CanClickUp = false;
				isStart = false;
				PlayGame ();
			}
			if (isFinal) {
				CanClickUp = false;
				isFinal = false;
				StopGame ("stopFinal");
			}

		}
	}

	public void OpenPause()
	{
		if (GameManager.Instance.CanClick ()) {
			Time.timeScale = 0;
			PlayerPrefs.SetInt ("Pause",1);
		}
	}
	public void StopGame(string state)
	{

		if (state == "stopFinal") {
			if (Screen.fullScreen) {
				_ExitFullScreen ();
				Screen.fullScreen = !Screen.fullScreen;
			}
				_OnGameStopped ();
		} else {
		
			Time.timeScale = 1f;
			if (Screen.fullScreen) {
				_ExitFullScreen ();
				Screen.fullScreen = !Screen.fullScreen;
			}
				_OnGameStopped ();
		}
	}
	bool clickPlay=true;
	public void PlayGame()  
	{
		if (isMenu) {
			if(clickPlay)
				StartCoroutine (Play ());
			clickPlay = false;
		} else {

			PlayerPrefs.SetInt ("Pause",0);
			Time.timeScale = 1f;
		}
	}

	IEnumerator Play()
	{
		SoundManager.instance.PlaySound (11);
		GameManager.Instance.ClickOff();
		StartCoroutine (GameManager.Instance.StartNewLevel(0,startDelay+1.2f));
		yield return new WaitForSeconds (6.5f);
		isMenu = false;
		isStart = false;
	}
	public void EnableFinalScreen()
	{
		isFinal = true;
	}
	public void AgainGame()
	{
		//again game
		PlayerPrefs.SetInt("FullScreen", 0);
		Time.timeScale = 1f;
		StartCoroutine(GameManager.Instance.LoadScene());
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
	public IEnumerator CallStop()
	{
		yield return new WaitUntil(() => RestAPIHandler.Instance.UploadedSuccessfully==true);
		_OnGameStopped();
	}
}
