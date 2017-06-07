using UnityEngine;
using System.Collections;
using System;

public class FirstIdlePromptState : State
{

    public DateTime lastPromptTime, repeatPromptTime;
    public int nPrompts;
    bool vibratePiece = false, repeatPrompt = false, repeatHardClue = false;
    Piece currentPiece;
    private string prompt;

    public FirstIdlePromptState()
    {
        nPrompts = 0;
    }

    public void FirstIdlePrompt()
    {
        lastPromptTime = DateTime.Now;
        nPrompts = 0;
        repeatPrompt = false;

        if (nPrompts == 0)
        {
            if (Therapist.Instance.currentPiece == null)
            {
                Piece piece = GameState.Instance.FindNewPiece();
                Therapist.Instance.currentPiece = piece;
            }
            else
            {
                currentPiece = Therapist.Instance.currentPiece;
            }

            if (!UtterancesManager.Instance.FirstIdlePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name)))
            {
                repeatPrompt = true;
                repeatPromptTime = DateTime.Now;
                nPrompts = 0;
            }
            else
            {
                nPrompts = 1;
                vibratePiece = true;
                currentPiece = Therapist.Instance.currentPiece;
            }
        }
    }

    private void RepeatPrompt()
    {
        lastPromptTime = DateTime.Now;

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
                repeatHardClue = false;
                nPrompts++;
            }
        }
        else
        {
            repeatPrompt = false;
            repeatHardClue = false;

            if (Therapist.Instance.currentPiece == null)
            {
                Piece piece = GameState.Instance.FindNewPiece();
                Therapist.Instance.currentPiece = piece;
            }
            else
            {
                currentPiece = Therapist.Instance.currentPiece;
            }

            if (!UtterancesManager.Instance.FirstIdlePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name)))
            {
                repeatPrompt = true;
                repeatPromptTime = DateTime.Now;
            }
            else
            {
                nPrompts++;
                vibratePiece = true;
                currentPiece = Therapist.Instance.currentPiece;
                repeatPrompt = false;
            }
        }
    }

    public void Update()
    {
        if (nPrompts > 0)
        {
            if (((repeatHardClue || repeatPrompt) && (DateTime.Now - repeatPromptTime).TotalSeconds > 4)
                || (DateTime.Now - lastPromptTime).TotalSeconds > 20)
            {
                if (repeatHardClue || repeatPrompt || nPrompts < 2)
                {
                    UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> RepeatPrompt FristIdlePrompt");
                    CallFeedback();
                    // RepeatPrompt();
                }
                else if (nPrompts >= 2)
                {
                    Debug.Log("1st idle -> vai para o segundo estado");
                    UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> SecondPrompt");
                    CallFeedback();
                    // SecondPrompt();
                    return;
                }
            }
            if (Therapist.Instance.nFailedTries >= 2)
            {
                UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> FirstPlacePrompt");
                CallFeedback();
                //FirstPlacePrompt();
                return;
            }
            if (Therapist.Instance.nWrongAngleTries >= 2)
            {
                UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> FirstAnglePrompt");
                CallFeedback();
                // FirstAnglePrompt();
                return;
            }
        }
        else if (repeatPrompt && (DateTime.Now - repeatPromptTime).TotalSeconds > 4)
        {
            UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> FirstIdlePrompt");
            CallFeedback();
            // FirstIdlePrompt();
        }

        if (vibratePiece)
        {
            GameState.Instance.VibratePiece(currentPiece);
            vibratePiece = false;
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

    public void GivePositiveFeedback()
    {
        nPrompts = 0;
        Therapist.Instance.currentState = Therapist.Instance.PositiveFeedState;
        Therapist.Instance.GivePositiveFeedback();
    }

    public void GiveNegativeFeedback()
    {
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.NegativeFeedState;
        Debug.Log("1idle state-> neg feed");
        Therapist.Instance.GiveNegativeFeedback();
    }

    public void StartedMoving(bool correctAngle)
    {
        //lastPromptTime = DateTime.Now;
        Debug.Log("moveu " + GameState.Instance.dragging);

        if (!GameState.Instance.dragging)
        {
            nPrompts = 0;
            Therapist.Instance.currentState = Therapist.Instance.PlayState;
            Debug.Log("rodou chamou o playstate");
        }
    }

    public void EndGame()
    {
        nPrompts = 0;
        Therapist.Instance.showedHardClue = false;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.currentState = Therapist.Instance.FinalState;
        Therapist.Instance.EndGame();
    }

    public void HelpMotor()
    {
        Therapist.Instance.nFailedTries = 0;
        lastPromptTime = DateTime.Now;
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.MotorHelpState;
        Therapist.Instance.HelpMotor();
    }

    public void HelpAdjustingPiece()
    {
        Therapist.Instance.nFailedTries = 0;
        lastPromptTime = DateTime.Now;
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.FitHelpState;
        Therapist.Instance.HelpAdjustingPiece();
    }

    public void FirstPlacePrompt()
    {
        nPrompts = 0;
        Therapist.Instance.showedHardClue = false;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.FirstPlacePromptState;
        Therapist.Instance.FirstPlacePrompt();
    }

    public void FirstAnglePrompt()
    {
        nPrompts = 0;
        Debug.Log("1stidle -> 1stangle");
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

    public void HardCluePrompt()
    {
    }

    void State.Prompt(string prompt_name)
    {
        prompt = prompt_name;
    }
}
