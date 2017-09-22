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
        UtterancesManager.Instance.FingerHelp(Therapist.Instance.NiceRobot);
        Debug.Log("1 FingerHelp");

        Therapist.Instance.firstTimeFinger = 2;
        Therapist.Instance.currentState = Therapist.Instance.PlayState;
    }

    public void BeginNextGame()
    {
        UtterancesManager.Instance.FingerHelp(Therapist.Instance.NiceRobot);
        Debug.Log("2 FingerHelp");

        Therapist.Instance.firstTimeFinger = 2;
        Therapist.Instance.currentState = Therapist.Instance.PlayState;
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

    public void Update()
    {
    }
}
