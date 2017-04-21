using UnityEngine;
using System.Collections;
using System;

public class FitHelpState : State
{
    int nHelpRequests;
    DateTime lastHelpTime;

    public FitHelpState()
    {
        nHelpRequests = 0;
    }

    public void HelpAdjustingPiece()
    {
        nHelpRequests++;

        if (nHelpRequests > 5 || nHelpRequests == 0 || (DateTime.Now - lastHelpTime).TotalSeconds > 100)
        {
            UtterancesManager.Instance.CloseHelp();

            lastHelpTime = DateTime.Now;
            nHelpRequests = 0;
        }
        Therapist.Instance.currentState = Therapist.Instance.previousState;
        Therapist.Instance.previousState = null;
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

        /////////

        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.MotorHelpState;
        Therapist.Instance.HelpMotor();

        /////////

    }

    public void GivePositiveFeedback()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.PositiveFeedState;
        Therapist.Instance.GivePositiveFeedback();

        /////////

    }
    public void GiveNegativeFeedback()
    {

        /////////

        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.NegativeFeedState;
        Therapist.Instance.GiveNegativeFeedback();

        /////////

    }

    public void StartedMoving(bool correctAngle)
    {
    }

    public void FirstIdlePrompt()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.FirstIdlePromptState;
        Therapist.Instance.FirstIdlePrompt();

        /////////

    }
    public void FirstAnglePrompt()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.FirstAnglePromptState;
        Therapist.Instance.FirstAnglePrompt();

        /////////

    }
    public void FirstPlacePrompt()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.FirstPlacePromptState;
        Therapist.Instance.FirstPlacePrompt();

        /////////

    }
    public void SecondPrompt()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.SecondPromptState;
        Therapist.Instance.SecondPrompt();

        /////////

    }
    public void ThirdPrompt()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.ThirdPromptState;
        Therapist.Instance.ThirdPrompt();

        /////////

    }
    public void SecondAnglePrompt()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.SecondAnglePromptState;
        Therapist.Instance.SecondAnglePrompt();

        /////////

    }
    public void ThirdAnglePrompt()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.ThirdAnglePromptState;
        Therapist.Instance.ThirdAnglePrompt();

        /////////

    }


    public void Update()
    {
    }

    public void RepeatPrompt()//não existia acrescentei
    {
    }

    public void HardCluePrompt()
    {
    }
}
