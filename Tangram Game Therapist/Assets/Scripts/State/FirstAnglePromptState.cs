using UnityEngine;
using System.Collections;
using System;

public class FirstAnglePromptState : State
{

    public DateTime lastPromptTime, repeatPromptTime, repeatHardClueTime;
    public int nPrompts;
    bool repeatPrompt = false, repeatHardClue = false, repeatAngleHelp = false, rightAnglePiece = false;
    Piece currentPiece;
    private string prompt;

    public FirstAnglePromptState()
    {
        nPrompts = 0;
    }

    public void FirstAnglePrompt()
    {
        lastPromptTime = DateTime.Now;
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
            if (Therapist.Instance.previousState != Therapist.Instance.FirstPlacePromptState
                && Therapist.Instance.previousState != Therapist.Instance.FirstIdlePromptState)
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
                        Therapist.Instance.previousState = null;
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
                    // if (!UtterancesManager.Instance.FirstAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name)))
                    if (!UtterancesManager.Instance.FirstAnglePrompt(GameState.Instance.PieceInformation(currentPiece.name)))
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
        }

        Therapist.Instance.previousState = null;
    }

    private void RepeatPrompt()
    {
        Therapist.Instance.nWrongAngleTries = 0;
        lastPromptTime = DateTime.Now;

        if (repeatHardClue || (Therapist.Instance.currentGame.difficulty == SolutionManager.Difficulty.hard
            && !Therapist.Instance.showedHardClue && UnityEngine.Random.Range(0, 3) == 0))
        {
            repeatPrompt = false;
            repeatHardClue = false;
            repeatAngleHelp = false;
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
            Debug.Log("dentro do repeat 1st angle");
            if (repeatPrompt || (UnityEngine.Random.Range(0, 2) == 0 && !repeatAngleHelp))
            {
                repeatPrompt = false;
                repeatHardClue = false;
                //  if (!UtterancesManager.Instance.FirstAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name)))
                if (!UtterancesManager.Instance.FirstAnglePrompt(GameState.Instance.PieceInformation(currentPiece.name)))
                {
                    Debug.Log("nao consegui chamar repeat do 1st angle");
                    repeatPrompt = true;
                    repeatPromptTime = DateTime.Now;
                }
                else
                {
                    nPrompts++;
                    repeatPrompt = false;
                }
            }
            else
            {
                repeatAngleHelp = false;
                if (Therapist.Instance.currentGame.rotationMode == SceneProperties.RotationMode.button)
                {
                    if (!UtterancesManager.Instance.FirstAnglePromptButton(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name)))
                    {
                        Debug.Log("nao consegui chamar repeat do 1st angle BUTTON");

                        repeatAngleHelp = true;
                        repeatPromptTime = DateTime.Now;
                    }
                    else
                    {
                        nPrompts++;
                        repeatAngleHelp = false;
                    }
                }
                else
                {
                    if (!UtterancesManager.Instance.FirstAnglePromptFinger(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name)))
                    {
                        Debug.Log("nao consegui chamar repeat do 1st angle FINGER");

                        repeatAngleHelp = true;
                        repeatPromptTime = DateTime.Now;
                    }
                    else
                    {
                        nPrompts++;
                        repeatAngleHelp = false;
                    }
                }
            }
        }
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
            repeatAngleHelp = false;
            repeatHardClue = false;
            repeatPrompt = false;
            rightAnglePiece = true;
        }
        else
            rightAnglePiece = false;
    }

    public void Update()
    {
        if (nPrompts > 0 && !rightAnglePiece)
        {
            if ((DateTime.Now - lastPromptTime).TotalSeconds > 20 || Therapist.Instance.nWrongAngleTries >= 2
                || ((repeatAngleHelp || repeatHardClue || repeatPrompt) && (DateTime.Now - repeatPromptTime).TotalSeconds > 4))
            {
                if (repeatAngleHelp || repeatHardClue || repeatPrompt || nPrompts < 1)
                {
                    UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> RepeatPrompt FirstAnglePrompt");
                    CallFeedback();
                    //RepeatPrompt();
                    return;
                }
                else if (!rightAnglePiece && nPrompts >= 1)
                {
                    Debug.Log("1st angle -> vai para o segundo estado");
                    UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> SecondAnglePrompt");
                    CallFeedback();
                    //SecondAnglePrompt();
                    return;
                }
            }
        }
        else if ((repeatHardClue || repeatPrompt) && (DateTime.Now - repeatPromptTime).TotalSeconds > 4)
        {
            UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> FirstAnglePrompt");
            CallFeedback();
            // FirstAnglePrompt();
        }

        if (Therapist.Instance.currentPiece != currentPiece)
        {
            ChangedPiece();
        }
        if ((rightAnglePiece && (DateTime.Now - lastPromptTime).TotalSeconds > 5) || Therapist.Instance.nFailedTries > 3)
        {
            UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> FirstPlacePrompt");
            CallFeedback();
            //FirstPlacePrompt();
        }

    }


    private void CallFeedback()
    {
        // Therapist.Instance.AlgorithmEXP3_.RunExp3();.
        Therapist.Instance.AlgorithmUCB_.RunUCB();
        Therapist.Instance.Feedback();
    }

    public void StartedMoving(bool correctAngle)
    {
        //lastPromptTime = DateTime.Now;

        if (correctAngle)
        {
            Debug.Log("BOA!!! não mexas mais, agora só falta coloca-la no sitio certo");
            UtterancesManager.Instance.StopAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));
            Therapist.Instance.previousState = Therapist.Instance.currentState;
            Therapist.Instance.nFailedTries = 0;
            Therapist.Instance.nWrongAngleTries = 0;
            Therapist.Instance.showedHardClue = false;
            Therapist.Instance.currentState = Therapist.Instance.PlayState;
        }
    }

    public void GiveNegativeFeedback()
    {
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.NegativeFeedState;
        Debug.Log("1angle state-> neg feed " + Therapist.Instance.nWrongAngleTries);
        Therapist.Instance.GiveNegativeFeedback();
    }

    public void EndGame()
    {
        nPrompts = 0;
        repeatPrompt = false;
        repeatHardClue = false;
        repeatAngleHelp = false;
        rightAnglePiece = false;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.showedHardClue = false;
        Therapist.Instance.currentState = Therapist.Instance.FinalState;
        Therapist.Instance.EndGame();
    }

    public void HelpMotor()
    {
        repeatPrompt = false;
        repeatHardClue = false;
        repeatAngleHelp = false;
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
        repeatHardClue = false;
        repeatAngleHelp = false;
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
        repeatHardClue = false;
        repeatAngleHelp = false;
        rightAnglePiece = false;
        nPrompts = 0;
        Therapist.Instance.currentState = Therapist.Instance.PositiveFeedState;
        Therapist.Instance.GivePositiveFeedback();
    }

    public void SecondAnglePrompt()
    {
        nPrompts = 0;
        repeatPrompt = false;
        repeatHardClue = false;
        repeatAngleHelp = false;
        rightAnglePiece = false;
        Therapist.Instance.showedHardClue = false;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.currentState = Therapist.Instance.SecondAnglePromptState;
        Therapist.Instance.SecondAnglePrompt();
    }

    public void FirstPlacePrompt()
    {
        nPrompts = 0;
        repeatPrompt = false;
        repeatHardClue = false;
        repeatAngleHelp = false;
        rightAnglePiece = false;
        Therapist.Instance.showedHardClue = false;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.currentState = Therapist.Instance.FirstPlacePromptState;
        Therapist.Instance.FirstPlacePrompt();
    }

    public void BeginFirstGame()
    {
    }

    public void BeginNextGame()
    {
    }

    public void FirstIdlePrompt()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.FirstIdlePromptState;
        Therapist.Instance.FirstIdlePrompt();

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
