using UnityEngine;
using System.Collections;
using System;

public class PositiveFeedState : State
{
    DateTime lastPositiveFeedTime;

    public PositiveFeedState()
    {
        lastPositiveFeedTime = DateTime.Now;
    }

    public void GivePositiveFeedback()
    {
        if ((DateTime.Now - lastPositiveFeedTime).TotalSeconds > 3)
        {
            lastPositiveFeedTime = DateTime.Now;
            UtterancesManager.Instance.PositiveFeedback(StringNumberOfPieces(GameState.Instance.notPlacedPieces.Count));
        }
        Therapist.Instance.currentState = Therapist.Instance.PlayState;
    }

    string StringNumberOfPieces(int nPieces)
    {
        string number;
        if (nPieces == 1)
            number = "uma peça";
        else if (nPieces == 2)
            number = "duas peças";
        else
            number = nPieces + " peças";
        return number;
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

        //////////

        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.MotorHelpState;
        Therapist.Instance.HelpMotor();

        /////////

    }

    public void HelpAdjustingPiece()
    {

        //////////

        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.FitHelpState;
        Therapist.Instance.HelpAdjustingPiece();

        /////////

    }

    public void GiveNegativeFeedback()
    {

        //////////

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

        //////////

        Therapist.Instance.currentState = Therapist.Instance.FirstIdlePromptState;
        Therapist.Instance.FirstIdlePrompt();

        /////////

    }

    public void FirstAnglePrompt()
    {

        //////////

        Therapist.Instance.currentState = Therapist.Instance.FirstAnglePromptState;
        Therapist.Instance.FirstAnglePrompt();

        /////////

    }

    public void FirstPlacePrompt()
    {

        //////////

        Therapist.Instance.currentState = Therapist.Instance.FirstPlacePromptState;
        Therapist.Instance.FirstPlacePrompt();

        /////////

    }

    public void SecondPrompt()
    {

        //////////

        Therapist.Instance.currentState = Therapist.Instance.SecondPromptState;
        Therapist.Instance.SecondPrompt();

        /////////

    }

    public void ThirdPrompt()
    {

        //////////

        Therapist.Instance.currentState = Therapist.Instance.ThirdPromptState;
        Therapist.Instance.ThirdPrompt();

        /////////

    }

    public void SecondAnglePrompt()
    {

        //////////

        Therapist.Instance.currentState = Therapist.Instance.SecondAnglePromptState;
        Therapist.Instance.SecondAnglePrompt();

        /////////

    }

    public void ThirdAnglePrompt()
    {

        //////////

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
