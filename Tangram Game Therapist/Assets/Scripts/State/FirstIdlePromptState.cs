using UnityEngine;
using System.Collections;
using System;

public class FirstIdlePromptState : State
{

    public DateTime lastPromptTime, repeatPromptTime;
    public int nPrompts;
    bool vibratePiece = false, repeatPrompt = false, repeatHardClue = false;
    Piece currentPiece;

    private bool firstPrompt = true;
    private bool secondPrompt = true;
    private bool thirdPrompt = true;

    public FirstIdlePromptState()
    {
        nPrompts = 0;
    }

    public void FirstIdlePrompt()
    {
        lastPromptTime = DateTime.Now;
        nPrompts = 0;
        repeatPrompt = false;

        firstPrompt = Therapist.Instance.First_Prompt;
        secondPrompt = Therapist.Instance.Second_Prompt;
        thirdPrompt = Therapist.Instance.Third_Prompt;

        bool utterance = false;
        Therapist.Instance.promt_Type = 1;
        UtterancesManager.Instance.CheckUtteranceFinish();

        Debug.Log("1st prompt -> IdlePrompt");
        if (nPrompts == 0)
        {
            if (Therapist.Instance.currentPiece == null)
            {
                Piece piece = GameState.Instance.FindNewPiece();
                Therapist.Instance.currentPiece = piece;
            }

            utterance = UtterancesManager.Instance.FirstIdlePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), 1);

            if (firstPrompt)
            {
                utterance = UtterancesManager.Instance.FirstIdlePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), 0);
            }

            if (!utterance)
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


                ///
                /// update the average reward if the last feedback wasnt given
                /// Call the form for the next utterance
                ///

                Therapist.Instance.lastActionMade = true;
                Therapist.Instance.utt_count++;
                Therapist.Instance.AVG_Ratings(0);
                Therapist.Instance.ShowFormRatings();

                ///
                ///
            }
        }
    }

    void RepeatPrompt()
    {
        lastPromptTime = DateTime.Now;

        bool utterance = false;
        UtterancesManager.Instance.CheckUtteranceFinish();
        Therapist.Instance.promt_Type = 1;
        Debug.Log("1st prompt -> RepeatPrompt");

        if (repeatHardClue || (Therapist.Instance.currentGame.difficulty == SolutionManager.Difficulty.hard
            && !Therapist.Instance.showedHardClue && UnityEngine.Random.Range(0, 3) == 0))
        {
            repeatPrompt = false;
            repeatHardClue = false;

            utterance = UtterancesManager.Instance.HardClue(2f, 1);

            if (firstPrompt)
            {
                utterance = UtterancesManager.Instance.HardClue(2f, 0);
            }

            if (!utterance)
            {
                repeatHardClue = true;
                repeatPromptTime = DateTime.Now;
            }
            else
            {
                repeatHardClue = false;
                nPrompts++;

                ///
                /// update the average reward if the last feedback wasnt given
                /// Call the form for the next utterance
                ///

                Therapist.Instance.lastActionMade = true;
                Therapist.Instance.utt_count++;
                Therapist.Instance.AVG_Ratings(0);
                Therapist.Instance.ShowFormRatings();

                ///
                ///
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

            utterance = UtterancesManager.Instance.FirstIdlePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), 1);

            if (firstPrompt)
            {
                utterance = UtterancesManager.Instance.FirstIdlePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), 0);
            }

            if (!utterance)
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

                ///
                /// update the average reward if the last feedback wasnt given
                /// Call the form for the next utterance
                ///

                Therapist.Instance.lastActionMade = true;
                Therapist.Instance.utt_count++;
                Therapist.Instance.AVG_Ratings(0);
                Therapist.Instance.ShowFormRatings();

                ///
                ///
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
                    RepeatPrompt();
                }
                else if (nPrompts >= 2)
                {
                    Debug.Log("1st idle -> vai para o segundo estado");
                    SecondPrompt();
                    return;
                }
            }
            if (Therapist.Instance.nFailedTries >= 2)
            {
                FirstPlacePrompt();
                return;
            }
            if (Therapist.Instance.nWrongAngleTries >= 2)
            {
                FirstAnglePrompt();
                return;
            }
        }
        else if (repeatPrompt && (DateTime.Now - repeatPromptTime).TotalSeconds > 4)
        {
            FirstIdlePrompt();
        }

        if (vibratePiece)
        {
            GameState.Instance.VibratePiece(currentPiece);
            vibratePiece = false;
        }
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
    }

    public void SecondAnglePrompt()
    {
    }

    public void ThirdAnglePrompt()
    {
    }
}
