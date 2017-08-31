using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class SecondAnglePromptState : State
{

    public DateTime lastPromptTime, repeatPromptTime;
    public int nPrompts;
    bool alreadyPromped = false, repeatHardClue = false, repeatPrompt = false, repeatAngleHelp = false, rightAnglePiece = false;
    public Dictionary<int, string> adjacentPieces;
    PieceSolution currentPlace;
    Piece currentPiece;
    SceneProperties.RotationMode rotationMode;
    string rotationDirection;

    private bool niceRobot = true;

    public SecondAnglePromptState()
    {
        nPrompts = 0;
    }

    public void SecondAnglePrompt()
    {
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.nFailedTries = 0;
        lastPromptTime = DateTime.Now;
        nPrompts = 0;
        currentPiece = Therapist.Instance.currentPiece;

        niceRobot = Therapist.Instance.NiceRobot;

        bool utterance = false;
        Therapist.Instance.promt_Type = 2;
        UtterancesManager.Instance.CheckUtteranceFinish();

        if (nPrompts == 0)
        {
            int random = UnityEngine.Random.Range(0, 3);
            if (repeatHardClue || random == 0 && !Therapist.Instance.showedHardClue
                && Therapist.Instance.currentGame.difficulty == SolutionManager.Difficulty.hard)
            {
                repeatHardClue = false;
                repeatPrompt = false;
                repeatAngleHelp = false;
                Debug.Log("2nd prompt -> ");

                if (niceRobot)
                {
                    utterance = UtterancesManager.Instance.HardClue(4.0f);
                }
                else
                {
                    utterance = UtterancesManager.Instance.HardClue(4.0f); --rude robot

                }

                if (!utterance)
                {
                    repeatHardClue = true;
                    repeatPromptTime = DateTime.Now;
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
                InitializeParameters();
                if (repeatPrompt || (UnityEngine.Random.Range(0, 3) == 0 && !repeatAngleHelp))
                {
                    repeatPrompt = false;
                    repeatAngleHelp = false;
                    Debug.Log("2nd prompt -> SecondAnglePrompt");

                    if (niceRobot)
                    {
                        utterance = UtterancesManager.Instance.SecondAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));
                    }
                    else
                    {
                        utterance = UtterancesManager.Instance.SecondAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name)); --rude robot

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

                        ///
                        Therapist.Instance.ShowFormRatings();
                        ///
                    }

                }
                else
                {
                    repeatAngleHelp = false;
                    if (rotationMode == SceneProperties.RotationMode.button)
                    {
                        Debug.Log("2nd prompt -> SecondAnglePromptButton");

                        if (niceRobot)
                        {
                            utterance = UtterancesManager.Instance.SecondAnglePromptButton(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), StringNumberOfClicks(GameState.Instance.numberOfClicks));
                        }
                        else
                        {
                            utterance = UtterancesManager.Instance.SecondAnglePromptButton(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), StringNumberOfClicks(GameState.Instance.numberOfClicks)); --rude robot
                        }

                        if (!utterance)
                        {
                            repeatAngleHelp = true;
                            repeatPromptTime = DateTime.Now;
                            nPrompts = 0;
                        }
                        else
                        {
                            nPrompts = 1;
                            repeatAngleHelp = false;

                            ///
                            Therapist.Instance.ShowFormRatings();
                            ///
                        }
                    }
                    else
                    {
                        Debug.Log("2nd prompt -> SecondAnglePromptFinger");

                        if (niceRobot)
                        {
                            utterance = UtterancesManager.Instance.SecondAnglePromptFinger(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), rotationDirection);
                        }
                        else
                        {
                            utterance = UtterancesManager.Instance.SecondAnglePromptFinger(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), rotationDirection); --rude robot
                        }

                        if (!utterance)
                        {
                            repeatAngleHelp = true;
                            repeatPromptTime = DateTime.Now;
                            nPrompts = 0;
                        }
                        else
                        {
                            nPrompts = 1;
                            repeatAngleHelp = false;

                            ///
                            Therapist.Instance.ShowFormRatings();
                            ///
                        }
                    }
                }
            }
        }
    }

    void RepeatPrompt()
    {
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.nFailedTries = 0;
        lastPromptTime = DateTime.Now;
        int random = UnityEngine.Random.Range(0, 3);

        bool utterance = false;
        Therapist.Instance.promt_Type = 2;
        UtterancesManager.Instance.CheckUtteranceFinish();

        Debug.Log("2nd prompt -> RepeatPrompt");
        if (repeatHardClue || (random == 0 && Therapist.Instance.currentGame.difficulty == SolutionManager.Difficulty.hard
            && !Therapist.Instance.showedHardClue))
        {
            repeatHardClue = false;
            repeatPrompt = false;
            repeatAngleHelp = false;

            if (niceRobot)
            {
                utterance = UtterancesManager.Instance.HardClue(4.0f);
            }
            else
            {
                utterance = UtterancesManager.Instance.HardClue(4.0f); --rude robot

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
                Therapist.Instance.ShowFormRatings();
                ///
            }
        }
        else
        {
            if (!alreadyPromped)
                InitializeParameters();
            if (repeatPrompt || (UnityEngine.Random.Range(0, 3) == 0 && !repeatAngleHelp))
            {
                repeatHardClue = false;
                repeatPrompt = false;
                repeatAngleHelp = false;

                if (niceRobot)
                {
                    utterance = UtterancesManager.Instance.SecondAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));
                }
                else
                {
                    utterance = UtterancesManager.Instance.SecondAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name)); --rude robot
                }

                if (!utterance)
                {
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
                repeatHardClue = false;
                repeatPrompt = false;
                repeatAngleHelp = false;
                if (rotationMode == SceneProperties.RotationMode.button)
                {
                    if (niceRobot)
                    {
                        utterance = UtterancesManager.Instance.SecondAnglePromptButton(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), StringNumberOfClicks(GameState.Instance.numberOfClicks));
                    }
                    else
                    {
                        utterance = UtterancesManager.Instance.SecondAnglePromptButton(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), StringNumberOfClicks(GameState.Instance.numberOfClicks)); --rude robot
                    }

                    if (!utterance)
                    {
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
                    if (niceRobot)
                    {
                        utterance = UtterancesManager.Instance.SecondAnglePromptFinger(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), rotationDirection);
                    }
                    else
                    {
                        utterance = UtterancesManager.Instance.SecondAnglePromptFinger(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), rotationDirection); --rude robot
                    }

                    if (!utterance)
                    {
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

    void InitializeParameters()
    {
        currentPiece = Therapist.Instance.currentPiece;
        rotationMode = Therapist.Instance.currentGame.rotationMode;
        currentPlace = GameState.Instance.FindTheCorrectPlace(currentPiece);

        if (currentPlace != null)
        {
            //Therapist.Instance.nWrongAngleTries = 0;
            repeatAngleHelp = false;
            repeatHardClue = false;
            repeatPrompt = false;
            rightAnglePiece = true;
        }
        else
        {
            rightAnglePiece = false;
            currentPlace = GameState.Instance.FindTheClosestPlace(currentPiece);
            rotationDirection = CalculateDirectionOfRotation(currentPiece, currentPlace);
        }
        Therapist.Instance.currentPlace = currentPlace;
        alreadyPromped = true;
    }

    string CalculateDirectionOfRotation(Piece currentPiece, PieceSolution currentPlace)
    {
        //Isto não funciona mt bem, mas está quase feito. será que faz sentido usar isto?
        /*string currentDirection;
		int variation;
		if (currentPiece.rotation > 180) 
			variation = ((int)currentPiece.rotation - 260) - (int)currentPlace.rotation;
		else 
			variation = (int)currentPiece.rotation - (int)currentPlace.rotation;

		if (variation < 0)
			currentDirection = "esquerda";
		else
			currentDirection = "direita";
		
		return currentDirection;*/
        int rand = UnityEngine.Random.Range(0, 2);
        return (rand == 0 ? "isquerda" : "direita");
    }

    string StringNumberOfClicks(int clicks)
    {
        string number;
        if (clicks == 1)
            number = "uma vez";
        else if (clicks == 2)
            number = "duas vezes";
        else
            number = clicks + " vezes";
        return number;
    }

    public void StartedMoving(bool correctAngle)
    {
        //lastPromptTime = DateTime.Now;

        if (correctAngle)
        {
            Debug.Log("BOA!!! não mexas mais, agora só falta coloca-la no sitio certo");
            UtterancesManager.Instance.CheckUtteranceFinish();
            bool utterance = false;

            if (niceRobot)
            {
                utterance = UtterancesManager.Instance.StopAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));
            }
            else
            {
                utterance = UtterancesManager.Instance.StopAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name)); --rude robot
            }

            ///
            Therapist.Instance.ShowFormRatings();
            ///

            Therapist.Instance.previousState = Therapist.Instance.currentState;
            nPrompts = 0;
            Therapist.Instance.nFailedTries = 0;
            Therapist.Instance.nWrongAngleTries = 0;
            Therapist.Instance.showedHardClue = false;
            FirstPlacePrompt();
        }
    }

    public void Update()
    {
        if (nPrompts > 0 && !rightAnglePiece)
        {
            if ((DateTime.Now - lastPromptTime).TotalSeconds > 20 || Therapist.Instance.nFailedTries >= 2
                || Therapist.Instance.nWrongAngleTries >= 2
                || ((repeatAngleHelp || repeatHardClue || repeatPrompt) && (DateTime.Now - repeatPromptTime).TotalSeconds > 4))
            {
                if (repeatAngleHelp || repeatHardClue || repeatPrompt || nPrompts < 3)
                {
                    RepeatPrompt();
                    return;
                }
                else if (!rightAnglePiece && nPrompts >= 3)
                {
                    Debug.Log("2nd angle prompt -> vai para o terceiro estado");
                    ThirdAnglePrompt();
                    return;
                }
            }
        }
        else if (((repeatAngleHelp || repeatHardClue || repeatPrompt) && (DateTime.Now - repeatPromptTime).TotalSeconds > 4))
        {
            SecondAnglePrompt();
        }
        if (Therapist.Instance.currentPiece != currentPiece)
        {
            InitializeParameters();
        }
        if ((rightAnglePiece && (DateTime.Now - lastPromptTime).TotalSeconds > 5) || Therapist.Instance.nFailedTries > 3)
            FirstPlacePrompt();
    }

    public void EndGame()
    {
        nPrompts = 0;
        repeatHardClue = false;
        repeatPrompt = false;
        repeatAngleHelp = false;
        rightAnglePiece = false;
        Therapist.Instance.showedHardClue = false;
        Therapist.Instance.currentState = Therapist.Instance.FinalState;
        Therapist.Instance.EndGame();
    }

    public void HelpMotor()
    {
        repeatHardClue = false;
        repeatPrompt = false;
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
        repeatHardClue = false;
        repeatPrompt = false;
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
        repeatHardClue = false;
        repeatPrompt = false;
        repeatAngleHelp = false;
        rightAnglePiece = false;
        nPrompts = 0;
        alreadyPromped = false;
        Therapist.Instance.currentState = Therapist.Instance.PositiveFeedState;
        Therapist.Instance.GivePositiveFeedback();
    }

    public void GiveNegativeFeedback()
    {
        Therapist.Instance.previousState = Therapist.Instance.SecondAnglePromptState;
        Therapist.Instance.currentState = Therapist.Instance.NegativeFeedState;
        Debug.Log("2angle state-> neg feed");
        Therapist.Instance.GiveNegativeFeedback();
    }

    public void FirstPlacePrompt()
    {
        repeatHardClue = false;
        repeatPrompt = false;
        repeatAngleHelp = false;
        rightAnglePiece = false;
        nPrompts = 0;
        alreadyPromped = false;
        Therapist.Instance.showedHardClue = false;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.FirstPlacePromptState;
        Therapist.Instance.FirstPlacePrompt();
    }

    public void ThirdAnglePrompt()
    {
        repeatHardClue = false;
        repeatPrompt = false;
        repeatAngleHelp = false;
        rightAnglePiece = false;
        nPrompts = 0;
        alreadyPromped = false;
        Therapist.Instance.showedHardClue = false;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.currentState = Therapist.Instance.ThirdAnglePromptState;
        Therapist.Instance.ThirdAnglePrompt();
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

    public void FirstAnglePrompt()
    {
    }

    public void ThirdPrompt()
    {
    }

    public void SecondPrompt()
    {
    }

}
