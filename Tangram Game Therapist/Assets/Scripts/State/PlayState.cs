using UnityEngine;
using System.Collections;
using System;

public class PlayState : State
{
    private string prompt;

    public PlayState()
    {
    }

    public void Update()
    {
        if ((DateTime.Now - GameState.Instance.stopped).TotalSeconds > 15 && !GameState.Instance.dragging)
        {
            Debug.Log("PLAY -> 1st idle");
            // FirstIdlePrompt();
            UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> FirstIdlePrompt");
            CallFeedback();
        }
        if (Therapist.Instance.nFailedTries >= 2)
        {
            Debug.Log(Therapist.Instance.nFailedTries + " tentativas erradas");
            //FirstPlacePrompt();
            UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> FirstPlacePrompt");
            CallFeedback();
        }
        if (Therapist.Instance.nWrongAngleTries >= 2)
        {
            Debug.Log(Therapist.Instance.nWrongAngleTries + " x angulo erradas");
            //FirstAnglePrompt();
            UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> FirstAnglePrompt");
            CallFeedback();
        }
    }

    private void CallFeedback()
    {
        // Therapist.Instance.AlgorithmEXP3_.RunExp3();.

        int previous_ActionRatings = Therapist.Instance.RatingsFeedback.previousAction;
        int previous_ActionsTherapist = Therapist.Instance.previousAction;
        if ((previous_ActionsTherapist != previous_ActionRatings) && previous_ActionsTherapist != -1)
        {
            Therapist.Instance.RatingsFeedback.FileHeader();
            Therapist.Instance.RatingsFeedback.WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), ";" + GameManager.Instance.playerName + ";" + GameManager.Instance.CurrentPuzzle + ";" + GameManager.Instance.Difficulty_ + ";" + GameManager.Instance.RotationMode_ + ";" + GameManager.Instance.DistanceThreshold + ";" + previous_ActionsTherapist + ";" + "3;1");
            Therapist.Instance.RatingsFeedback.header = false;

            Therapist.Instance.AlgorithmUCB_.UpdateReward(previous_ActionsTherapist, 3);
            Therapist.Instance.RatingsFeedback.previousAction = previous_ActionsTherapist;
            Therapist.Instance.RatingsFeedback.previousFeedback = 3;
        }

        Therapist.Instance.AlgorithmUCB_.RunUCB();
        Therapist.Instance.Feedback();
    }

    public void StartedMoving(bool correctAngle)
    {
        Debug.Log("moveu " + correctAngle + " " + Therapist.Instance.currentPlace);

        if (!GameState.Instance.dragging && !correctAngle)
        {
            if (Therapist.Instance.previousState == Therapist.Instance.FirstAnglePromptState)
            {
                Debug.Log("PLAY -> 1stangle");
                FirstAnglePrompt();
                Therapist.Instance.previousState = null;
            }
        }
    }

    public void BeginFirstGame()
    {
    }

    public void BeginNextGame()
    {
    }

    public void EndGame()
    {
        Therapist.Instance.currentState = Therapist.Instance.FinalState;
        Therapist.Instance.EndGame();
    }

    public void HelpMotor()
    {
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.MotorHelpState;
        Therapist.Instance.HelpMotor();
    }

    public void HelpAdjustingPiece()
    {
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.FitHelpState;
        Therapist.Instance.HelpAdjustingPiece();
    }

    public void GivePositiveFeedback()
    {
        Therapist.Instance.currentState = Therapist.Instance.PositiveFeedState;
        Therapist.Instance.GivePositiveFeedback();
    }

    public void GiveNegativeFeedback()
    {
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.NegativeFeedState;
        Therapist.Instance.GiveNegativeFeedback();
    }

    public void FirstIdlePrompt()
    {
        Therapist.Instance.currentState = Therapist.Instance.FirstIdlePromptState;
        Therapist.Instance.FirstIdlePrompt();
    }

    public void FirstAnglePrompt()
    {
        Therapist.Instance.currentState = Therapist.Instance.FirstAnglePromptState;
        Therapist.Instance.FirstAnglePrompt();
    }

    public void FirstPlacePrompt()
    {
        Therapist.Instance.currentState = Therapist.Instance.FirstPlacePromptState;
        Therapist.Instance.FirstPlacePrompt();
    }

    public void SecondPrompt()
    {
        Therapist.Instance.currentState = Therapist.Instance.SecondPromptState;
        Therapist.Instance.SecondPrompt();
    }

    public void ThirdPrompt()
    {
        Therapist.Instance.currentState = Therapist.Instance.ThirdPromptState;
        Therapist.Instance.ThirdPrompt();
    }

    public void SecondAnglePrompt()
    {
        Therapist.Instance.currentState = Therapist.Instance.SecondAnglePromptState;
        Therapist.Instance.SecondAnglePrompt();
    }

    public void ThirdAnglePrompt()
    {
        Therapist.Instance.currentState = Therapist.Instance.ThirdAnglePromptState;
        Therapist.Instance.ThirdAnglePrompt();
    }

    public void HardCluePrompt()
    {
        Therapist.Instance.currentState = Therapist.Instance.HardClueState;
        Therapist.Instance.HardCluePrompt();
    }

    void State.Prompt(string prompt_name)
    {
        prompt = prompt_name;
    }
}
