using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBot : MonoBehaviour
{
    public static AIBot Instance;
    int Randomizer;
    [HideInInspector]
    public bool IsGameOver;

    private void OnEnable()
    {
        Instance = this;
        IsGameOver = false;
    }
    public void SelectMove (char [] BoardData)
    {
        if (!IsGameOver)

        {
            Randomizer = Random.Range(0, 9);

            if (BoardData[Randomizer].Equals('E'))
            {
                PlayMove((Randomizer + 1));
            }
            else
            {
                SelectMove(BoardData);
            }
        }
    
    }

    void PlayMove(int i)
    {
        TicTacToeManager.Instance.MarkPosition(i);
    }

}
