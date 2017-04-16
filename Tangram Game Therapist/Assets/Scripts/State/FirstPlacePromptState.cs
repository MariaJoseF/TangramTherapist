using UnityEngine;
using System.Collections;
using System;

public class FirstPlacePromptState : State
{

    public DateTime lastPromptTime, repeatPromptTime;
    public int nPrompts;
    bool repeatHardClue = false, repeatPrompt = false;
    Piece currentPiece;

    public FirstPlacePromptState()
    {
        nPrompts = 0;
    }

    public void FirstPlacePrompt()
    {
        lastPromptTime = DateTime.Now;
        Therapist.Instance.nFailedTries = 0;
        nPrompts = 0;



        //if (Therapist.Instance.currentPiece == null)
        //{
        //    currentPiece = Therapist.Instance.lastPieceUsed;
        //}
        //else
        //{
        //    currentPiece = Therapist.Instance.currentPiece;
        //}

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

        if (nPrompts == 0 && Therapist.Instance.previousState != Therapist.Instance.FirstIdlePromptState
            && Therapist.Instance.previousState != Therapist.Instance.SecondAnglePromptState
            && Therapist.Instance.previousState != Therapist.Instance.ThirdAnglePromptState)
        {
            if (repeatHardClue || Therapist.Instance.currentGame.difficulty == SolutionManager.Difficulty.hard
                && !Therapist.Instance.showedHardClue && UnityEngine.Random.Range(0, 3) == 0)
            {
                repeatPrompt = false;
                repeatHardClue = false;
                if (!UtterancesManager.Instance.HardClue(2f))
                {
                    repeatHardClue = true;
                    repeatPromptTime = DateTime.Now;
                    nPrompts = 0;
                }
                else
                {
                    nPrompts = 1;
                }
            }
            else
            {
                repeatPrompt = false;
                repeatHardClue = false;
                // if (!UtterancesManager.Instance.FirstPlacePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name)))
                if (!UtterancesManager.Instance.FirstPlacePrompt(GameState.Instance.PieceInformation(currentPiece.name)))
                {
                    repeatPrompt = true;
                    repeatPromptTime = DateTime.Now;
                    nPrompts = 0;
                }
                else
                    nPrompts = 1;
            }
        }
        else
            nPrompts = 1;
        Therapist.Instance.previousState = null;
    }

    public void RepeatPrompt()
    {//não devia ser public
        Therapist.Instance.nFailedTries = 0;
        lastPromptTime = DateTime.Now;
        repeatPrompt = false;
        if (repeatHardClue || (Therapist.Instance.currentGame.difficulty == SolutionManager.Difficulty.hard
            && !Therapist.Instance.showedHardClue && UnityEngine.Random.Range(0, 3) == 0))
        {
            repeatPrompt = false;
            repeatHardClue = false;
            if (!UtterancesManager.Instance.HardClue(2f))
            {
                repeatHardClue = true;
                repeatPromptTime = DateTime.Now;
            }
            else
            {
                nPrompts++;
            }
        }
        else
        {
            repeatPrompt = false;
            repeatHardClue = false;
            //  if (!UtterancesManager.Instance.FirstPlacePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name)))
            if (!UtterancesManager.Instance.FirstPlacePrompt(GameState.Instance.PieceInformation(currentPiece.name)))
            {
                repeatPrompt = true;
                repeatPromptTime = DateTime.Now;
            }
            else
                nPrompts++;
        }
    }

    public void Update()
    {
        if (nPrompts > 0)
        {
            if (Therapist.Instance.nWrongAngleTries >= 2)
            {
                FirstAnglePrompt();
                return;
            }
            else if (((repeatHardClue || repeatPrompt) && (DateTime.Now - repeatPromptTime).TotalSeconds > 4)
                || (DateTime.Now - lastPromptTime).TotalSeconds > 20 || Therapist.Instance.nFailedTries >= 2)
            {
                if (repeatHardClue || repeatPrompt || nPrompts < 1)
                {
                    RepeatPrompt();
                }
                else if (nPrompts >= 1)
                {
                    Debug.Log("1stplace -> vai para o segundo estado");
                    SecondPrompt();
                    return;
                }
            }
        }
        else if ((repeatHardClue || repeatPrompt) && (DateTime.Now - repeatPromptTime).TotalSeconds > 4)
        {
            FirstPlacePrompt();
        }
    }

    public void EndGame()
    {
        nPrompts = 0;
        repeatHardClue = false;
        repeatPrompt = false;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.showedHardClue = false;
        Therapist.Instance.currentState = Therapist.Instance.FinalState;
        Therapist.Instance.EndGame();
    }

    public void HelpMotor()
    {
        repeatHardClue = false;
        repeatPrompt = false;
        Therapist.Instance.nFailedTries = 0;
        lastPromptTime = DateTime.Now;
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.MotorHelpState;
        Therapist.Instance.HelpMotor();
    }

    public void HelpAdjustingPiece()
    {
        repeatHardClue = false;
        repeatPrompt = false;
        Therapist.Instance.nFailedTries = 0;
        lastPromptTime = DateTime.Now;
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.FitHelpState;
        Therapist.Instance.HelpAdjustingPiece();
    }

    public void GivePositiveFeedback()
    {
        nPrompts = 0;
        repeatHardClue = false;
        repeatPrompt = false;
        Therapist.Instance.showedHardClue = false;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.currentState = Therapist.Instance.PositiveFeedState;
        Therapist.Instance.GivePositiveFeedback();
    }

    public void GiveNegativeFeedback()
    {
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.NegativeFeedState;
        Debug.Log("1place state-> neg feed");
        Therapist.Instance.GiveNegativeFeedback();

    }

    public void FirstIdlePrompt()
    {
        Debug.Log("1stplace -> 1stidle");
        nPrompts = 0;
        repeatHardClue = false;
        repeatPrompt = false;
        Therapist.Instance.showedHardClue = false;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.currentState = Therapist.Instance.FirstIdlePromptState;
        Therapist.Instance.FirstIdlePrompt();
    }

    public void FirstAnglePrompt()
    {
        Debug.Log("1stplace -> 1stangle");
        nPrompts = 0;
        repeatHardClue = false;
        repeatPrompt = false;
        Therapist.Instance.showedHardClue = false;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.FirstAnglePromptState;
        Therapist.Instance.FirstAnglePrompt();
    }

    public void SecondPrompt()
    {
        nPrompts = 0;
        repeatHardClue = false;
        repeatPrompt = false;
        Therapist.Instance.showedHardClue = false;
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

    public void StartedMoving(bool correctAngle)
    {
        //lastPromptTime = DateTime.Now;
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

}
