using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject MainBG;
    public GameObject ModeSelection;
    public GameObject ResultBG;
    public GameObject ResultScreen;
    public GameObject GameBoard;
    public GameObject GameFunctions;
    public Transform ResultWindow;
    public GameObject HelpWindow;
    public Button[] ModeButtons;
    bool Once;


    private void Awake()
    {
        Debug.unityLogger.logEnabled = false;
        Time.timeScale = 1.0f;
    }
    public void Play()
    {
        MainMenu.SetActive(false);
        ModeSelection.SetActive(true);
    }

    public void SelectMode(bool i)
    {
        if (!Once)
        {
            Once = true;
            if (i)
            {
                ModeButtons[1].enabled = false;
                PlayerPrefs.SetInt("IsAI", 1);
            }
            else
            {
                ModeButtons[0].enabled = false;
                PlayerPrefs.SetInt("IsAI", 0);
            }

            Invoke("InitiateGamePlay", 0.1f);
        }
    }
    public void Result (string result)
    {
        switch (result)
        {
            case "Win":
                ResultWindow.GetChild(0).transform.gameObject.SetActive(true);
                break;
            case "Lose":
                ResultWindow.GetChild(1).transform.gameObject.SetActive(true);
                break;
            case "Tie":
                ResultWindow.GetChild(2).transform.gameObject.SetActive(true);
                break;
            case "Player1Wins":
                ResultWindow.GetChild(3).transform.gameObject.SetActive(true);
                break;
            case "Player2Wins":
                ResultWindow.GetChild(4).transform.gameObject.SetActive(true);
                break;
        }
    }

    public void InitiateGamePlay ()
    {
        MainBG.SetActive(false);
        GameBoard.SetActive(true);
        GameFunctions.SetActive(true);
        ModeSelection.SetActive(false);
    }

    public void ShowWinScreen(int i)
    {
        ResultBG.SetActive(true);
        MainBG.SetActive(false);
        ResultScreen.SetActive(true);
        GameBoard.SetActive(false);
        switch (i)
        {
            case 0:
                Result("Win");
                break;
            case 1:
                Result("Lose");
                break;
            case 2:
                Result("Tie");
                break;
            case 3:
                Result("Player1Wins");
                break;
            case 4:
                Result("Player2Wins");
                break;
        }
    }

    public void SwitchHelpWindow(bool Switch)
    {
        if (Switch)
        {
            HelpWindow.SetActive(true);
        }
        else
        {
            HelpWindow.SetActive(false);
        }
    }
}
