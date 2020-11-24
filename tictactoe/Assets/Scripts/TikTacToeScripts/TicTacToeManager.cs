using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToeManager : MonoBehaviour
{
    //SINGELTON
    public static TicTacToeManager Instance;

    //PLAYER_PREFIXES
    char PlayerPrefix = 'P';
    char PlayerBPrefix = 'S';
    char ComputerPrefix = 'C';


    //BOARDDATA
    [Header ("BoardData")]
    public char[] BoardData;
    public Transform Player1Markers;
    public Transform Player2Markers;
    public Transform Player1Highlights;
    public Transform Player2Highlights;

    //GAME_FUNCTIONS
    bool GameOver;
    int NumberOfTurns;
    int WinningCombination;
    int UniversalCounter;
    bool IsAI;
    int CurrentTurn;
    int CurrentWinner;

    //INPUT DISPATCHING
    [Header("InputDispatching")]
    public Transform BoardTouchPoints;
    public bool[] ButtonInputDispatched;

    //ROBOT
    [Header("Robot")]
    public AIBot Robot;

    [Header("MenuManager")]
    public MasterMenu MasterMenuManager;
    public float WinScreenDelayTime;

    [Header("PlayerBlips")]
    public Animator Player1Label;
    public Animator Player2Label;
    public Animator ComputerLabel;

    [Header("PlayerPanels")]
    public GameObject PanelA;
    public GameObject PanelB;

    [Header("Player Markers")]
    public PlayerMarkerFetch MarkerFetch;

    [Header("Sound Manager")]
    public CustomSoundManager SoundMgr;
 



    void Awake()
    {
        Instance = this; //SINGLETON OBJECT
        Time.timeScale = 1.0f;
        CurrentTurn = Random.Range(1, 3); //TURN RANDOMIZER
        
        if (PlayerPrefs.GetInt("IsAI") == 1) //AI OR PLAYER-2 CHECK
        {
            IsAI = true;
            Robot.gameObject.SetActive(true);
            ComputerLabel.gameObject.SetActive(true);
        }
        else
        {
            IsAI = false;
            Player2Label.gameObject.SetActive(true);
        }
        AssignMarkerImages();
    }

    void Start()
    {
        if ((CurrentTurn == 2) && (IsAI))
        {
            
            ComputerLabel.enabled = true;
            Invoke("DisableComputerGlow", 2f);
            Invoke("RobotMove", 3f);
        }

        else if ((CurrentTurn == 2) && (!IsAI))
        {
            
            Player2Label.enabled = true;
            Invoke("DisablePlayer2Glow", 2f);
        }
        else if (CurrentTurn == 1)
        {
            Player1Label.enabled = true;
            Invoke("DisablePlayer1Glow", 2f);
        }
        if (IsAI)
        {
            ComputerLabel.gameObject.transform.parent.gameObject.SetActive(true);

        }
        else
        {
            Player2Label.gameObject.transform.parent.gameObject.SetActive(true);
        }

        Invoke("ChangePanels", 0.5f);
    }

    void RobotMove()
    {
        Robot.SelectMove(BoardData);
    }

    void DisableComputerGlow()
    {
        ComputerLabel.enabled = false;
    }

    void DisablePlayer1Glow()
    {
        Player1Label.enabled = false;
    }

    void DisablePlayer2Glow()
    {
        Player2Label.enabled = false;
    }

    void ChangePanels ()
    {
        if (CurrentTurn == 1)
        {
            PanelA.SetActive(true);
            PanelB.SetActive(false);
        }
        else
        {
            PanelB.SetActive(true);
            PanelA.SetActive(false);
        }
    }



    public void MarkPosition(int i)
    {
        i = i - 1;

        if ((ButtonInputDispatched[i] == true) || (GameOver))
        {
            return;
        }

        else
        {
            switch (CurrentTurn)
            {
                case 1:
                    DispatchInputButton(i);
                    BoardData[i] = 'P';
                    Player1Markers.GetChild(i).gameObject.SetActive(true);
                    NumberOfTurns++;
                    if (NumberOfTurns >= 5)
                    {
                        CheckPlayerWin (BoardData,true);
                    }
                    else
                    {
                        Invoke("ActivateTouchPoints", 2.0f);
                    }

                    break;

                case 2:
                    DispatchInputButton(i);
                    if (!IsAI)
                    {
                        BoardData[i] = 'S';
                    }
                    else
                    {
                        BoardData[i] = 'C';
                    }
                    Player2Markers.GetChild(i).gameObject.SetActive(true);
                    NumberOfTurns++;
                     if (NumberOfTurns >= 5)
                     {
                        CheckPlayerWin (BoardData,false);
                     }
                    else
                    {
                        Invoke("ActivateTouchPoints", 2.0f);
                    }
                    break;
            }

            ChangeTurns();
            SoundMgr.PlayMarkSound();
        }
    }

    void ChangeTurns()
    {
        if (CurrentTurn == 1)
        {
            CurrentTurn = 2;
            if (IsAI)
            {
                Invoke("RobotMove", 2f);
            }
        }
        else
        {
            CurrentTurn = 1;
        }

        ChangePanels(); 
    }
    public void CheckPlayerWin(char [] ImportedBoardData, bool Player)
    {
        if (Player)
        {
            CheckWinConditons('P');
        }
        else
        {
            if (!IsAI)
            {
                CheckWinConditons('S');
            }
            else
            {
                CheckWinConditons('C');
            }
        }
    }

    public void CheckWinConditons(char X)
    {
        if ((BoardData [0] == X) && (BoardData[1] == X) && (BoardData[2] == X))
        {
            WinningCombination = 1;
            GameOver = true;
        }

        else if ((BoardData[3] == X) && (BoardData[4] == X) && (BoardData[5] == X))
        {
            WinningCombination = 2;
            GameOver = true;
        }

        else if ((BoardData[6] == X) && (BoardData[7] == X) && (BoardData[8] == X))
        {
            WinningCombination = 3;
 
        }

        else if ((BoardData[0] == X) && (BoardData[3] == X) && (BoardData[6] == X))
        {
            WinningCombination = 4;
        }

        else if ((BoardData[1] == X) && (BoardData[4] == X) && (BoardData[7] == X))
        {
            WinningCombination = 5;
           
        }

        else if ((BoardData[2] == X) && (BoardData[5] == X) && (BoardData[8] == X))
        {
            WinningCombination = 6;
            
        }

        else if ((BoardData[0] == X) && (BoardData[4] == X) && (BoardData[8] == X))
        {
            WinningCombination = 7;
        }

        else if ((BoardData[2] == X) && (BoardData[4] == X) && (BoardData[6] == X))
        {
            WinningCombination = 8;
        }

        if (WinningCombination > 0)
        {
            GameOver = true;
            CallWinner(X);
        }
        else
        {
            Invoke("ActivateTouchPoints", 2.0f);
        }
        if ((NumberOfTurns == 9) && (WinningCombination == 0)) 
        {
            CallWinner('T');
        }
    }

    void CallWinner(char X)
    {
        switch (X)
        {
            case 'P':
                Debug.Log("Player 1 WINS");
                if (IsAI)
                {
                    CurrentWinner = 0;
                }
                else
                {
                    CurrentWinner = 3;
                }
                if (!AIBot.Instance)
                {
                    AIBot.Instance.IsGameOver = true;
                }
                break;
            case 'C':
                CurrentWinner = 1;
                Debug.Log("Robot WINS");
                if (AIBot.Instance)
                {
                    AIBot.Instance.IsGameOver = true;
                }
                break;
            case 'S':
                CurrentWinner = 4;
                Debug.Log("Player 2 WINS");
                if (AIBot.Instance)
                {
                    AIBot.Instance.IsGameOver = true;
                }
                break;
            case 'T':
                CurrentWinner = 2;
                if (AIBot.Instance)
                {
                    AIBot.Instance.IsGameOver = true;
                }
                Debug.Log("TIE TIE");
                break;
        }
        PlayWinnerAnimation(X);
        SoundMgr.PlayCompletedSound();

        Invoke("ShowWinnerWindow", WinScreenDelayTime);
    }

    void PlayWinnerAnimation(char X)
    {
        if (X.Equals('P'))
        {
            switch (WinningCombination)
            {
                case 1:
                    Player1Highlights.GetChild(0).gameObject.SetActive(true);
                    Player1Highlights.GetChild(1).gameObject.SetActive(true);
                    Player1Highlights.GetChild(2).gameObject.SetActive(true);
                    Player1Markers.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                    Player1Markers.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
                    Player1Markers.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
                    break;
                case 2:
                    Player1Highlights.GetChild(3).gameObject.SetActive(true);
                    Player1Highlights.GetChild(4).gameObject.SetActive(true);
                    Player1Highlights.GetChild(5).gameObject.SetActive(true);
                    Player1Markers.GetChild(3).gameObject.GetComponent<Image>().enabled = false;
                    Player1Markers.GetChild(4).gameObject.GetComponent<Image>().enabled = false;
                    Player1Markers.GetChild(5).gameObject.GetComponent<Image>().enabled = false;
                    break;
                case 3:
                    Player1Highlights.GetChild(6).gameObject.SetActive(true);
                    Player1Highlights.GetChild(7).gameObject.SetActive(true);
                    Player1Highlights.GetChild(8).gameObject.SetActive(true);
                    Player1Markers.GetChild(6).gameObject.GetComponent<Image>().enabled = false;
                    Player1Markers.GetChild(7).gameObject.GetComponent<Image>().enabled = false;
                    Player1Markers.GetChild(8).gameObject.GetComponent<Image>().enabled = false;
                    break;
                case 4:
                    Player1Highlights.GetChild(0).gameObject.SetActive(true);
                    Player1Highlights.GetChild(3).gameObject.SetActive(true);
                    Player1Highlights.GetChild(6).gameObject.SetActive(true);
                    Player1Markers.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                    Player1Markers.GetChild(3).gameObject.GetComponent<Image>().enabled = false;
                    Player1Markers.GetChild(6).gameObject.GetComponent<Image>().enabled = false;
                    break;
                case 5:
                    Player1Highlights.GetChild(1).gameObject.SetActive(true);
                    Player1Highlights.GetChild(4).gameObject.SetActive(true);
                    Player1Highlights.GetChild(7).gameObject.SetActive(true);
                    Player1Markers.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
                    Player1Markers.GetChild(4).gameObject.GetComponent<Image>().enabled = false;
                    Player1Markers.GetChild(7).gameObject.GetComponent<Image>().enabled = false;
                    break;
                case 6:
                    Player1Highlights.GetChild(2).gameObject.SetActive(true);
                    Player1Highlights.GetChild(5).gameObject.SetActive(true);
                    Player1Highlights.GetChild(8).gameObject.SetActive(true);
                    Player1Markers.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
                    Player1Markers.GetChild(5).gameObject.GetComponent<Image>().enabled = false;
                    Player1Markers.GetChild(8).gameObject.GetComponent<Image>().enabled = false;
                    break;
                case 7:
                    Player1Highlights.GetChild(0).gameObject.SetActive(true);
                    Player1Highlights.GetChild(4).gameObject.SetActive(true);
                    Player1Highlights.GetChild(8).gameObject.SetActive(true);
                    Player1Markers.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                    Player1Markers.GetChild(4).gameObject.GetComponent<Image>().enabled = false;
                    Player1Markers.GetChild(8).gameObject.GetComponent<Image>().enabled = false;
                    break;
                case 8:
                    Player1Highlights.GetChild(2).gameObject.SetActive(true);
                    Player1Highlights.GetChild(4).gameObject.SetActive(true);
                    Player1Highlights.GetChild(6).gameObject.SetActive(true);
                    Player1Markers.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
                    Player1Markers.GetChild(4).gameObject.GetComponent<Image>().enabled = false;
                    Player1Markers.GetChild(6).gameObject.GetComponent<Image>().enabled = false;
                    break;
            }
        }
        else if ((X.Equals('S')) || (X.Equals('C')))
        {
            switch (WinningCombination)
            {
                case 1:
                    Player2Highlights.GetChild(0).gameObject.SetActive(true);
                    Player2Highlights.GetChild(1).gameObject.SetActive(true);
                    Player2Highlights.GetChild(2).gameObject.SetActive(true);
                    Player2Markers.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                    Player2Markers.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
                    Player2Markers.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
                    break;
                case 2:
                    Player2Highlights.GetChild(3).gameObject.SetActive(true);
                    Player2Highlights.GetChild(4).gameObject.SetActive(true);
                    Player2Highlights.GetChild(5).gameObject.SetActive(true);
                    Player2Markers.GetChild(3).gameObject.GetComponent<Image>().enabled = false;
                    Player2Markers.GetChild(4).gameObject.GetComponent<Image>().enabled = false;
                    Player2Markers.GetChild(5).gameObject.GetComponent<Image>().enabled = false;
                    break;
                case 3:
                    Player2Highlights.GetChild(6).gameObject.SetActive(true);
                    Player2Highlights.GetChild(7).gameObject.SetActive(true);
                    Player2Highlights.GetChild(8).gameObject.SetActive(true);
                    Player2Markers.GetChild(6).gameObject.GetComponent<Image>().enabled = false;
                    Player2Markers.GetChild(7).gameObject.GetComponent<Image>().enabled = false;
                    Player2Markers.GetChild(8).gameObject.GetComponent<Image>().enabled = false;
                    break;
                case 4:
                    Player2Highlights.GetChild(0).gameObject.SetActive(true);
                    Player2Highlights.GetChild(3).gameObject.SetActive(true);
                    Player2Highlights.GetChild(6).gameObject.SetActive(true);
                    Player2Markers.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                    Player2Markers.GetChild(3).gameObject.GetComponent<Image>().enabled = false;
                    Player2Markers.GetChild(6).gameObject.GetComponent<Image>().enabled = false;
                    break;
                case 5:
                    Player2Highlights.GetChild(1).gameObject.SetActive(true);
                    Player2Highlights.GetChild(4).gameObject.SetActive(true);
                    Player2Highlights.GetChild(7).gameObject.SetActive(true);
                    Player2Markers.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
                    Player2Markers.GetChild(4).gameObject.GetComponent<Image>().enabled = false;
                    Player2Markers.GetChild(7).gameObject.GetComponent<Image>().enabled = false;
                    break;
                case 6:
                    Player2Highlights.GetChild(2).gameObject.SetActive(true);
                    Player2Highlights.GetChild(5).gameObject.SetActive(true);
                    Player2Highlights.GetChild(8).gameObject.SetActive(true);
                    Player2Markers.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
                    Player2Markers.GetChild(5).gameObject.GetComponent<Image>().enabled = false;
                    Player2Markers.GetChild(8).gameObject.GetComponent<Image>().enabled = false;
                    break;
                case 7:
                    Player2Highlights.GetChild(0).gameObject.SetActive(true);
                    Player2Highlights.GetChild(4).gameObject.SetActive(true);
                    Player2Highlights.GetChild(8).gameObject.SetActive(true);
                    Player2Markers.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                    Player2Markers.GetChild(4).gameObject.GetComponent<Image>().enabled = false;
                    Player2Markers.GetChild(8).gameObject.GetComponent<Image>().enabled = false;
                    break;
                case 8:
                    Player2Highlights.GetChild(2).gameObject.SetActive(true);
                    Player2Highlights.GetChild(4).gameObject.SetActive(true);
                    Player2Highlights.GetChild(6).gameObject.SetActive(true);
                    Player2Markers.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
                    Player2Markers.GetChild(4).gameObject.GetComponent<Image>().enabled = false;
                    Player2Markers.GetChild(6).gameObject.GetComponent<Image>().enabled = false;
                    break;
            }
        }
    }
    void ShowWinnerWindow()
    {
        MasterMenuManager.ShowWinScreen(CurrentWinner);
    }


    void DispatchInputButton(int i)
    {
        ButtonInputDispatched[i] = true;
        DeactivateTouchPoints();
    }

    void DeactivateTouchPoints()
    {
        for (UniversalCounter = 0; UniversalCounter < BoardTouchPoints.childCount; UniversalCounter++)
        {
            BoardTouchPoints.GetChild(UniversalCounter).gameObject.GetComponent<Button>().enabled = false;
        }
    }

    public void ActivateTouchPoints ()
    {
        for (UniversalCounter = 0; UniversalCounter < BoardTouchPoints.childCount; UniversalCounter++)
        {
            if (ButtonInputDispatched[UniversalCounter] == false)
            {
                BoardTouchPoints.GetChild(UniversalCounter).gameObject.GetComponent<Button>().enabled = true;
            }
        }
    }

    void AssignMarkerImages()
    {
        for (int i = 0; i < Player1Markers.childCount; i++)
            {
            Player1Markers.GetChild(i).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = MarkerFetch.MarkerImages[0];
          //  Player1Markers.GetChild(i).transform.GetChild(0).gameObject.GetComponent<Image>().SetNativeSize();
            }
        for (int i = 0; i < Player2Markers.childCount; i++)
        {
            Player2Markers.GetChild(i).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = MarkerFetch.MarkerImages[1];
          //  Player2Markers.GetChild(i).transform.GetChild(0).gameObject.GetComponent<Image>().SetNativeSize();
        }
    }


}
