using UnityEngine;
using System.Collections;
using System;

public class ThirdAnglePromptState : State
{

    public DateTime lastPromptTime, repeatPromptTime;
    public int nPrompts;
    bool finalPrompt = false, repeatPrompt = false, rightAnglePiece = false;
    Piece currentPiece;

    public ThirdAnglePromptState()
    {
    }

    public void ThirdAnglePrompt()
    {
        lastPromptTime = DateTime.Now;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        nPrompts = 0;

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

        if (nPrompts == 0)
        {
            //if (!UtterancesManager.Instance.ThirdAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name))) {
            if (!UtterancesManager.Instance.ThirdAnglePrompt(GameState.Instance.PieceInformation(currentPiece.name)))
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

        if (!UtterancesManager.Instance.ThirdAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name)))
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


    public void StartedMoving(bool correctAngle)
    {
        //lastPromptTime = DateTime.Now;
        if (finalPrompt)
        {
            finalPrompt = false;
            nPrompts = 1;
        }
        if (correctAngle)
        {
            Debug.Log("BOA!!! não mexas mais, agora só falta coloca-la no sitio certo");
            UtterancesManager.Instance.StopAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));
            Therapist.Instance.previousState = Therapist.Instance.currentState;
            Therapist.Instance.nFailedTries = 0;
            Therapist.Instance.nWrongAngleTries = 0;
            FirstPlacePrompt();
        }
    }

    public void Update()
    {
        if (nPrompts > 0 && !rightAnglePiece)
        {
            if ((repeatPrompt && (DateTime.Now - repeatPromptTime).TotalSeconds > 4)
                || (DateTime.Now - lastPromptTime).TotalSeconds > 25 || Therapist.Instance.nFailedTries >= 2
                || Therapist.Instance.nWrongAngleTries >= 2)
            {
                if (repeatPrompt || nPrompts < 3)
                {
                    UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> RepeatPrompt ThirdAnglePrompt");
                    CallFeedback();
                    //RepeatPrompt();
                    return;
                }
                else if (!rightAnglePiece && nPrompts >= 3)
                {
                    UtterancesManager.Instance.Quit();
                    Therapist.Instance.nFailedTries = 0;
                    Therapist.Instance.nWrongAngleTries = 0;
                    finalPrompt = true;
                    lastPromptTime = DateTime.Now;
                }
            }
        }
        else if ((repeatPrompt && (DateTime.Now - repeatPromptTime).TotalSeconds > 4))
        {
            UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> ThirdAnglePrompt");
            CallFeedback();
            //ThirdAnglePrompt();
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
            ChangedPiece();
        }
        if ((rightAnglePiece && (DateTime.Now - lastPromptTime).TotalSeconds > 5) || Therapist.Instance.nFailedTries > 3)
        {
            UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> SecondPrompt");
            CallFeedback();
            //SecondPrompt();
        }
    }

    private void CallFeedback()
    {
        // Therapist.Instance.AlgorithmEXP3_.RunExp3();.
        Therapist.Instance.AlgorithmUCB_.RunUCB();
        Therapist.Instance.Feedback();
    }

    void ChangedPiece()
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

        lastPromptTime = DateTime.Now;
        PieceSolution currentPlace = GameState.Instance.FindTheCorrectPlace(currentPiece);
        Therapist.Instance.currentPlace = currentPlace;

        if (currentPlace != null)
        {
            Therapist.Instance.nWrongAngleTries = 0;
            repeatPrompt = false;
            rightAnglePiece = true;
        }
        else
            rightAnglePiece = false;
    }

    public void EndGame()
    {
        repeatPrompt = false;
        rightAnglePiece = false;
        nPrompts = 0;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Debug.Log("3rd angle finalPrompt -> quit");
        Therapist.Instance.currentState = Therapist.Instance.FinalState;
        Therapist.Instance.EndGame();
    }

    public void HelpMotor()
    {
        repeatPrompt = false;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        lastPromptTime = DateTime.Now;
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.MotorHelpState;
        Therapist.Instance.HelpMotor();
    }

    public void HelpAdjustingPiece()
    {
        repeatPrompt = false;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        lastPromptTime = DateTime.Now;
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.FitHelpState;
        Therapist.Instance.HelpAdjustingPiece();
    }

    public void GivePositiveFeedback()
    {
        repeatPrompt = false;
        rightAnglePiece = false;
        nPrompts = 0;
        Therapist.Instance.currentState = Therapist.Instance.PositiveFeedState;
        Therapist.Instance.GivePositiveFeedback();
    }

    public void GiveNegativeFeedback()
    {
        Therapist.Instance.previousState = Therapist.Instance.ThirdAnglePromptState;
        Therapist.Instance.currentState = Therapist.Instance.NegativeFeedState;
        Debug.Log("3angle state-> neg feed");
        Therapist.Instance.GiveNegativeFeedback();
    }

    public void FirstAnglePrompt()
    {
        repeatPrompt = false;
        rightAnglePiece = false;
        nPrompts = 0;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.currentState = Therapist.Instance.FirstAnglePromptState;
        Therapist.Instance.FirstAnglePrompt();
    }

    public void FirstPlacePrompt()
    {
        repeatPrompt = false;
        rightAnglePiece = false;
        nPrompts = 0;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.FirstPlacePromptState;
        Therapist.Instance.FirstPlacePrompt();
    }

    public void SecondPrompt()
    {
        repeatPrompt = false;
        rightAnglePiece = false;
        nPrompts = 0;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.currentState = Therapist.Instance.SecondPromptState;
        Therapist.Instance.SecondPrompt();
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

    public void SecondAnglePrompt()
    {
        ///////////////

        Therapist.Instance.currentState = Therapist.Instance.SecondAnglePromptState;
        Therapist.Instance.SecondAnglePrompt();

        ///////////////
    }

    public void ThirdPrompt()
    {
        ///////////////////

        Therapist.Instance.currentState = Therapist.Instance.ThirdPromptState;
        Therapist.Instance.ThirdPrompt();

        ///////////////////
    }

    public void HardCluePrompt()
    {
    }

}
