using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditions : MonoBehaviour
{
    public enum CompletionAmount { No_Condition, Percentage, All }
    public enum Player { No_Condition, Human_Win, Candle_Win }

    public CompletionAmount tasksTillCompletion;
    public CompletionAmount burnTillCompletion;
    public Player timeOutWinner;

    public float taskCompletion;
    public float burnCompletion;
    public int timeCompletion;

    bool gameEnded = false;
    HouseHealth house;
    float time = 0;

    private void Start()
    {
        if (FindObjectOfType<HouseHealth>() != null)
        {
            house = FindObjectOfType<HouseHealth>();
        }
        else
        {
            Debug.LogWarning("No object of type " + typeof(HouseHealth) + " could be found.");
        }
        if (burnTillCompletion == CompletionAmount.All)
        {
            burnCompletion = 100;
        }
        if (tasksTillCompletion == CompletionAmount.All)
        {
            taskCompletion = 100;
        }
    }

    private void Update()
    {
        if (!gameEnded)
        {
            if (burnTillCompletion != CompletionAmount.No_Condition && house != null)
            {
                if (1 - (house.currentHealth / house.totalHealth) >= burnCompletion)
                {
                    PlayerWin("Candle");
                }
            }

            if (tasksTillCompletion != CompletionAmount.No_Condition)
            {
                //Code for when tasks are done goes here
                //PlayerWin("Human");
            }

            if (timeOutWinner != Player.No_Condition)
            {
                if (time >= timeCompletion)
                {
                    if (timeOutWinner == Player.Human_Win)
                    {
                        PlayerWin("Human");
                    }
                    else
                    {
                        PlayerWin("Candle");
                    }
                }
                else
                {
                    time += Time.deltaTime;
                }
            }
        }
    }

    void PlayerWin(string player)
    {
        //Perform any code for when a player wins here
        gameEnded = true;
        Debug.Log(player + " wins!");
    }
}
