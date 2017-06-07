﻿using UnityEngine;
using System.Collections;
using System;

public class ThirdPromptState : State
{

    public DateTime lastPromptTime, incorrectAngleTime, repeatPromptTime, goToSecondAnglePromptTime;
    public int nPrompts, nIncorrectAngle;
    bool finalPrompt, repeatPrompt = false, goToSecondAnglePrompt = false;
    Piece currentPiece;

    public ThirdPromptState()
    {
        nPrompts = 0;
        nIncorrectAngle = 0;
    }

    public void ThirdPrompt()
    {
        lastPromptTime = DateTime.Now;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        nPrompts = 0;
        if (nPrompts == 0)
        {

            /////////////////////////////////////////////////////

            if (Therapist.Instance.currentPiece == null)
            {
                Piece piece = GameState.Instance.FindNewPiece();
                Therapist.Instance.currentPiece = piece;

                currentPiece = piece;
            }
            else
            {
                currentPiece = Therapist.Instance.currentPiece;
            }

            /////////////////////////////////////////////////////

            //if (!UtterancesManager.Instance.ThirdPrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name))){
            if (!UtterancesManager.Instance.ThirdPrompt(GameState.Instance.PieceInformation(currentPiece.name)))
            {

                repeatPrompt = true;
                repeatPromptTime = DateTime.Now;
                nPrompts = 0;
            }
            else
            {
                repeatPrompt = false;
                nPrompts = 1;
            }
        }
    }

    private void RepeatPrompt()
    {
        lastPromptTime = DateTime.Now;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;

        if (!UtterancesManager.Instance.ThirdPrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name)))
        {
            repeatPrompt = true;
            repeatPromptTime = DateTime.Now;
        }
        else
        {
            repeatPrompt = false;
            nPrompts++;
        }
    }

    void InitializeParameters()
    {
        PieceSolution currentPlace;
        if (Therapist.Instance.currentPiece == null)
        {
            Piece piece = GameState.Instance.FindNewPiece();
            Therapist.Instance.currentPiece = piece;
            currentPlace = GameState.Instance.FindTheCorrectPlace(Therapist.Instance.currentPiece);

        }
        else
        {
            currentPlace = GameState.Instance.FindTheCorrectPlace(Therapist.Instance.currentPiece);

        }

        Therapist.Instance.currentPlace = currentPlace;

        if (currentPlace == null)
        {
            goToSecondAnglePrompt = true;
            goToSecondAnglePromptTime = DateTime.Now;
        }
    }
    public void StartedMoving(bool correctAngle)
    {
        //lastPromptTime = DateTime.Now;
        if (finalPrompt)
        {
            finalPrompt = false;
            nPrompts = 1;
        }
        if (!GameState.Instance.dragging)
        {
            if (!correctAngle)
            {
                nIncorrectAngle++;
                incorrectAngleTime = DateTime.Now;
            }
            else
            {
                nIncorrectAngle = 0;
                Therapist.Instance.nWrongAngleTries = 0;
            }
        }
    }

    public void Update()
    {
        if (nPrompts > 0)
        {
            if (Therapist.Instance.nWrongAngleTries >= 2 || (nIncorrectAngle > 0 && (DateTime.Now - incorrectAngleTime).TotalSeconds > 12)
                || (goToSecondAnglePrompt && (DateTime.Now - goToSecondAnglePromptTime).TotalSeconds > 5))
            {
                UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> SecondAnglePrompt");
                CallFeedback();
                //SecondAnglePrompt();
                Debug.Log(Therapist.Instance.nWrongAngleTries + " Third -> 2Angle " + Therapist.Instance.currentPiece + " " + nIncorrectAngle);
                return;
            }
            else if ((repeatPrompt && (DateTime.Now - repeatPromptTime).TotalSeconds > 4)
                || (DateTime.Now - lastPromptTime).TotalSeconds > 25 || Therapist.Instance.nFailedTries >= 2)
            {
                if (repeatPrompt || nPrompts < 3)
                {
                    UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> RepeatPrompt ThirdPrompt");
                    CallFeedback();
                    //RepeatPrompt();
                    return;
                }
                else if (nPrompts >= 3)
                {
                    UtterancesManager.Instance.Quit();
                    nPrompts = 0;
                    finalPrompt = true;
                    lastPromptTime = DateTime.Now;
                }
            }
        }
        else if ((repeatPrompt && (DateTime.Now - repeatPromptTime).TotalSeconds > 4))
        {
            UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> ThirdPrompt");
            CallFeedback();
            //ThirdPrompt();
        }
        else if (finalPrompt && (DateTime.Now - lastPromptTime).TotalSeconds > 20)
        {
            lastPromptTime = DateTime.Now;
            GameState.Instance.quit = true;
            GameManager.Instance.quit = true;
            EndGame();
            return;
        }
        if (Therapist.Instance.currentPiece != currentPiece)
        {
            currentPiece = Therapist.Instance.currentPiece;
            InitializeParameters();
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

    public void GiveNegativeFeedback()
    {
        Therapist.Instance.previousState = Therapist.Instance.ThirdPromptState;
        Therapist.Instance.currentState = Therapist.Instance.NegativeFeedState;
        Debug.Log("3promp state-> neg feed");
        Therapist.Instance.GiveNegativeFeedback();
    }

    public void EndGame()
    {
        repeatPrompt = false;
        nPrompts = 0;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Debug.Log("3rd finalPrompt -> quit");
        Therapist.Instance.currentState = Therapist.Instance.FinalState;
        Therapist.Instance.EndGame();
    }

    public void HelpMotor()
    {
        repeatPrompt = false;
        Therapist.Instance.nFailedTries = 0;
        lastPromptTime = DateTime.Now;
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.MotorHelpState;
        Therapist.Instance.HelpMotor();
    }

    public void HelpAdjustingPiece()
    {
        repeatPrompt = false;
        Therapist.Instance.nFailedTries = 0;
        lastPromptTime = DateTime.Now;
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.FitHelpState;
        Therapist.Instance.HelpAdjustingPiece();
    }

    public void GivePositiveFeedback()
    {
        repeatPrompt = false;
        nPrompts = 0;
        Therapist.Instance.currentState = Therapist.Instance.PositiveFeedState;
        Therapist.Instance.GivePositiveFeedback();
    }

    public void SecondAnglePrompt()
    {
        repeatPrompt = false;
        nPrompts = 0;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.currentState = Therapist.Instance.SecondAnglePromptState;
        Therapist.Instance.SecondAnglePrompt();
    }

    public void BeginFirstGame()
    {
    }

    public void BeginNextGame()
    {
    }

    public void FirstIdlePrompt()
    {

        //////////////////

        Therapist.Instance.currentState = Therapist.Instance.FirstIdlePromptState;
        Therapist.Instance.FirstIdlePrompt();

        /////////////
    }

    public void FirstAnglePrompt()
    {

        ////////////////

        Therapist.Instance.currentState = Therapist.Instance.FirstAnglePromptState;
        Therapist.Instance.FirstAnglePrompt();

        ////////////////
    }

    public void FirstPlacePrompt()
    {

        /////////////

        Therapist.Instance.currentState = Therapist.Instance.FirstPlacePromptState;
        Therapist.Instance.FirstPlacePrompt();

        /////////////
    }

    public void SecondPrompt()
    {
        ////////

        Therapist.Instance.currentState = Therapist.Instance.SecondPromptState;
        Therapist.Instance.SecondPrompt();

        ////////

    }

    public void ThirdAnglePrompt()
    {

        //////

        Therapist.Instance.currentState = Therapist.Instance.ThirdAnglePromptState;
        Therapist.Instance.ThirdAnglePrompt();

        //////

    }

    public void HardCluePrompt()
    {
    }

    public void Prompt(string prompt_name)
    {
        
    }
}
