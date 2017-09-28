using UnityEngine;
using System.Collections;
using System;

public class NegativeFeedState : State
{
    DateTime lastNegativeFeedTime;
    int nNegativeFeed;

    public NegativeFeedState()
    {
        lastNegativeFeedTime = DateTime.Now;
        nNegativeFeed = 0;
    }

    public void GiveNegativeFeedback()
    {
        nNegativeFeed++;
        //if (nNegativeFeed >= 3 && (DateTime.Now - lastNegativeFeedTime).TotalSeconds > 15) {
        //if (nNegativeFeed >= 1 && (DateTime.Now - lastNegativeFeedTime).TotalSeconds > 15)
        //{

        UtterancesManager.Instance.CheckUtteranceFinish();

        if (UtterancesManager.Instance.NegativeFeedback(Therapist.Instance.NiceRobot))
        {
            Therapist.Instance.ShowFormRatings();

            nNegativeFeed = 0;
            lastNegativeFeedTime = DateTime.Now;
        }
        // }
    }

    public void BeginFirstGame()
    {
    }

    public void BeginNextGame()
    {
    }

    public void EndGame()
    {
    }

    public void HelpMotor()
    {
    }

    public void HelpAdjustingPiece()
    {
    }

    public void GivePositiveFeedback()
    {
    }

    public void StartedMoving(bool correctAngle)
    {
    }

    public void FirstIdlePrompt()
    {
    }

    public void FirstAnglePrompt()
    {
    }

    public void FirstPlacePrompt()
    {
    }

    public void SecondPrompt()
    {
    }

    public void ThirdPrompt()
    {
    }

    public void SecondAnglePrompt()
    {
    }

    public void ThirdAnglePrompt()
    {
    }

    public void Update()
    {
    }
}
