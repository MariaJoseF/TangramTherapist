using UnityEngine;
using System.Collections;
using System;

public class MotorHelpState : State
{
    int nHelpRequests;

    public MotorHelpState()
    {
        nHelpRequests = 0;
    }

    public void HelpMotor()
    {
        nHelpRequests++;
        if (nHelpRequests >= 3)
        {
            UtterancesManager.Instance.CheckUtteranceFinish();

            /*NEW*/

            if (Therapist.Instance.NiceRobot)
            {
                UtterancesManager.Instance.ChangeLibrary("Tangram");
            }
            else
            {
                UtterancesManager.Instance.ChangeLibrary("Tangram_Rude");
            }

            /*NEW*/

            UtterancesManager.Instance.MotorHelp();

            nHelpRequests = 0;
        }
        Therapist.Instance.currentState = Therapist.Instance.previousState;
        Therapist.Instance.previousState = null;
    }

    public void Update()
    {
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

    public void HelpAdjustingPiece()
    {
    }

    public void GivePositiveFeedback()
    {
    }

    public void GiveNegativeFeedback()
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

}
