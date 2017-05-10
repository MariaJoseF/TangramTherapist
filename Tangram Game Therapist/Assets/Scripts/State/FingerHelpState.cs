using UnityEngine;
using System.Collections;
using System;

public class FingerHelpState : State
{

    public FingerHelpState()
    {
    }

    public void BeginFirstGame()
    {
        UtterancesManager.Instance.FingerHelp();
        Debug.Log("1 FingerHelp");

        Therapist.Instance.firstTimeFinger = 2;
        Therapist.Instance.currentState = Therapist.Instance.PlayState;
    }

    public void BeginNextGame()
    {
        UtterancesManager.Instance.FingerHelp();
        Debug.Log("2 FingerHelp");

        Therapist.Instance.firstTimeFinger = 2;
        Therapist.Instance.currentState = Therapist.Instance.PlayState;
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
    public void HelpAdjustingPiece()
    {

        /////////

        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.FitHelpState;
        Therapist.Instance.HelpAdjustingPiece();

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

    public void HardCluePrompt()
    {
    }
}
