using UnityEngine;
using System.Collections;
using System;

public class FirstAnglePromptState : State
{

    public DateTime lastPromptTime, repeatPromptTime, repeatHardClueTime;
    public int nPrompts;
    bool repeatPrompt = false, repeatHardClue = false, repeatAngleHelp = false, rightAnglePiece = false;
    Piece currentPiece;

    public FirstAnglePromptState()
    {
        nPrompts = 0;
    }

    public void FirstAnglePrompt()
    {
        lastPromptTime = DateTime.Now;
        Therapist.Instance.nWrongAngleTries = 0;
        nPrompts = 0;
        currentPiece = Therapist.Instance.currentPiece;

        bool utterance = false;

        UtterancesManager.Instance.CheckUtteranceFinish();

        Debug.Log("1st prompt -> AnglePrompt");
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

                    /*NEW*/

                    utterance = UtterancesManager.Instance.HardClue(2f, Therapist.Instance.NiceRobot("HardClue"));

                    /*NEW*/


                    if (!utterance)
                    {
                        repeatHardClue = true;
                        repeatPromptTime = DateTime.Now;
                        Therapist.Instance.previousState = null;
                        nPrompts = 0;
                    }
                    else
                    {
                        nPrompts = 1;

                        ///
                        Therapist.Instance.ShowFormRatings();
                        ///

                    }
                }
                else
                {
                    repeatPrompt = false;
                    repeatHardClue = false;

                    /*NEW*/

                    utterance = UtterancesManager.Instance.FirstAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), Therapist.Instance.NiceRobot("FirstAnglePrompt"));

                    /*NEW*/


                    if (!utterance)
                    {
                        repeatPrompt = true;
                        repeatPromptTime = DateTime.Now;
                        nPrompts = 0;
                    }
                    else
                        nPrompts = 1;

                    ///
                    Therapist.Instance.ShowFormRatings();
                    ///
                }
            }
            else
                nPrompts = 1;
        }

        Therapist.Instance.previousState = null;
    }

    void RepeatPrompt()
    {
        Therapist.Instance.nWrongAngleTries = 0;
        lastPromptTime = DateTime.Now;

        bool utterance = false;
        UtterancesManager.Instance.CheckUtteranceFinish();

        Debug.Log("1st prompt -> RepeatPrompt");
        if (repeatHardClue || (Therapist.Instance.currentGame.difficulty == SolutionManager.Difficulty.hard
            && !Therapist.Instance.showedHardClue && UnityEngine.Random.Range(0, 3) == 0))
        {
            repeatPrompt = false;
            repeatHardClue = false;
            repeatAngleHelp = false;

            /*NEW*/

            utterance = UtterancesManager.Instance.HardClue(2f, Therapist.Instance.NiceRobot("HardClue"));

            /*NEW*/


            if (!utterance)
            {
                repeatHardClue = true;
                repeatPromptTime = DateTime.Now;
            }
            else
            {
                nPrompts++;

                ///
                Therapist.Instance.ShowFormRatings();
                ///
            }
        }
        else
        {
            Debug.Log("dentro do repeat 1st angle");
            if (repeatPrompt || (UnityEngine.Random.Range(0, 2) == 0 && !repeatAngleHelp))
            {
                repeatPrompt = false;
                repeatHardClue = false;


                /*NEW*/

                utterance = UtterancesManager.Instance.FirstAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), Therapist.Instance.NiceRobot("FirstAnglePrompt"));

                /*NEW*/


                if (!utterance)
                {
                    Debug.Log("nao consegui chamar repeat do 1st angle");
                    repeatPrompt = true;
                    repeatPromptTime = DateTime.Now;
                }
                else
                {
                    nPrompts++;
                    repeatPrompt = false;

                    ///
                    Therapist.Instance.ShowFormRatings();
                    ///
                }
            }
            else
            {
                repeatAngleHelp = false;
                if (Therapist.Instance.currentGame.rotationMode == SceneProperties.RotationMode.button)
                {

                    /*NEW*/

                    utterance = UtterancesManager.Instance.FirstAnglePromptButton(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), Therapist.Instance.NiceRobot("FirstAnglePromptButton"));

                    /*NEW*/


                    if (!utterance)
                    {
                        Debug.Log("nao consegui chamar repeat do 1st angle BUTTON");

                        repeatAngleHelp = true;
                        repeatPromptTime = DateTime.Now;
                    }
                    else
                    {
                        nPrompts++;
                        repeatAngleHelp = false;

                        ///
                        Therapist.Instance.ShowFormRatings();
                        ///
                    }
                }
                else
                {

                    /*NEW*/

                    utterance = UtterancesManager.Instance.FirstAnglePromptFinger(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), Therapist.Instance.NiceRobot("FirstAnglePromptFinger"));

                    /*NEW*/


                    if (!utterance)
                    {
                        Debug.Log("nao consegui chamar repeat do 1st angle FINGER");

                        repeatAngleHelp = true;
                        repeatPromptTime = DateTime.Now;
                    }
                    else
                    {
                        nPrompts++;
                        repeatAngleHelp = false;

                        ///
                        Therapist.Instance.ShowFormRatings();
                        ///
                    }
                }
            }
        }
    }

    void ChangedPiece()
    {
        currentPiece = Therapist.Instance.currentPiece;
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
                    RepeatPrompt();
                    return;
                }
                else if (!rightAnglePiece && nPrompts >= 1)
                {
                    Debug.Log("1st angle -> vai para o segundo estado");
                    SecondAnglePrompt();
                    return;
                }
            }
        }
        else if ((repeatHardClue || repeatPrompt) && (DateTime.Now - repeatPromptTime).TotalSeconds > 4)
        {
            FirstAnglePrompt();
        }

        if (Therapist.Instance.currentPiece != currentPiece)
        {
            ChangedPiece();
        }
        if ((rightAnglePiece && (DateTime.Now - lastPromptTime).TotalSeconds > 5) || Therapist.Instance.nFailedTries > 3)
            FirstPlacePrompt();
    }

    public void StartedMoving(bool correctAngle)
    {
        //lastPromptTime = DateTime.Now;

        if (correctAngle)
        {

            Debug.Log("BOA!!! não mexas mais, agora só falta coloca-la no sitio certo");
            UtterancesManager.Instance.CheckUtteranceFinish();
            bool utterance = false;

            /*NEW*/

            utterance = UtterancesManager.Instance.StopAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), Therapist.Instance.NiceRobot("StopAnglePrompt"));
            Therapist.Instance.ShowFormRatings();

            /*NEW*/

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
    }

    public void SecondPrompt()
    {
    }

    public void ThirdPrompt()
    {
    }

    public void ThirdAnglePrompt()
    {
    }
}
