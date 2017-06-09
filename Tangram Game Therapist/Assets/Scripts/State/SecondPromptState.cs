﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class SecondPromptState : State
{

    public DateTime lastPromptTime, incorrectAngleTime, repeatPromptTime, goToSecondAnglePromptTime;
    public int nPrompts, nIncorrectAngle;
    bool prompedRelativePosition = false, prompedRelativePieces = false, alreadyPromped = false,
        repeatHardClue = false, repeatPrompt = false, repeatRelativePosition = false, goToSecondAnglePrompt = false;
    public Dictionary<int, string> adjacentPieces = new Dictionary<int, string>();
    public Dictionary<int, string> availableAdjacentPieces = new Dictionary<int, string>();
    PieceSolution currentPlace;
    Piece currentPiece;
    int random;

    private static SecondPromptState instance = null;

    private string prompt;

    public string Prompt
    {
        get
        {
            return prompt;
        }

        set
        {
            prompt = value;
        }
    }

    public static SecondPromptState Instance
    {
        get
        {
            return instance;
        }

    }

    public SecondPromptState()
    {
        nPrompts = 0;
        nIncorrectAngle = 0;
    }

    public void SecondPrompt()
    {
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.nFailedTries = 0;
        lastPromptTime = DateTime.Now;
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
            random = UnityEngine.Random.Range(0, 3);
            if (repeatHardClue || random == 0 && Therapist.Instance.currentGame.difficulty == SolutionManager.Difficulty.hard
                && !Therapist.Instance.showedHardClue)
            {
                repeatHardClue = false;
                repeatPrompt = false;
                repeatRelativePosition = false;
                if (!UtterancesManager.Instance.HardClue(4.0f))
                {
                    repeatHardClue = true;
                    repeatPromptTime = DateTime.Now;
                    nPrompts = 0;
                }
                else
                {
                    repeatHardClue = false;
                    nPrompts = 1;
                }
            }
            else
            {
                nPrompts = 1;
                InitializeParameters();
                HelpWithRelativePosition();
            }
        }
    }

    private void RepeatPrompt()
    {
        lastPromptTime = DateTime.Now;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.nFailedTries = 0;
        random = UnityEngine.Random.Range(0, 3);

        if (repeatHardClue || (random == 0 && Therapist.Instance.currentGame.difficulty == SolutionManager.Difficulty.hard
            && !Therapist.Instance.showedHardClue))
        {
            repeatHardClue = false;
            repeatPrompt = false;
            repeatRelativePosition = false;
            if (!UtterancesManager.Instance.HardClue(4.0f))
            {
                repeatHardClue = true;
                repeatPromptTime = DateTime.Now;
            }
            else
            {
                nPrompts++;
                repeatHardClue = false;
            }
        }
        else
        {
            nPrompts++;
            HelpWithRelativePosition();
        }
    }

    void InitializeParameters()
    {

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
        else
        {
            goToSecondAnglePrompt = false;
            adjacentPieces = new Dictionary<int, String>(currentPlace.relPos.adjacentPieces);
            availableAdjacentPieces = new Dictionary<int, string>(IntersectDictionaries(adjacentPieces, GameState.Instance.placedPieces));
            alreadyPromped = true;
        }
    }

    void HelpWithRelativePosition()
    {
        repeatHardClue = false;
        if (!alreadyPromped)
        {
            InitializeParameters();
        }

        if (currentPlace != null)
        {
            if (repeatPrompt || (!repeatRelativePosition && availableAdjacentPieces.Count > 0 && ((prompedRelativePosition && !prompedRelativePieces) || random == 1)) || (prompt == "SecondPromptPlace"))
            {
                repeatRelativePosition = false;

                //if there is no pieces already in the right place availableAdjacentPieces.Count = 0
                if (availableAdjacentPieces.Count > 0)
                {
                    int pieceId = GameState.Instance.RandomKeys(availableAdjacentPieces).First();
                    string piece = SolutionManager.Instance.FindMatchIdName(pieceId);
                    string relativePosition = availableAdjacentPieces[pieceId];
                    Debug.Log("id " + pieceId + " relative position " + relativePosition + " piece " + piece);
                    //   if (!UtterancesManager.Instance.SecondPromptPlace(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), relativePosition, GameState.Instance.PieceInformation(piece)))
                    if (!UtterancesManager.Instance.SecondPromptPlace(GameState.Instance.PieceInformation(currentPiece.name), relativePosition, GameState.Instance.PieceInformation(piece)))
                    {
                        repeatPrompt = true;
                        repeatPromptTime = DateTime.Now;
                        return;
                    }
                    else
                    {
                        repeatPrompt = false;
                        adjacentPieces.Remove(pieceId);
                        availableAdjacentPieces.Remove(pieceId);
                        prompedRelativePieces = true;
                    }
                }
            }
            else
            {
                repeatPrompt = false;
                if (prompt == "SecondPrompt1Position")
                {
                    if (currentPlace.relPos.pos1 == null)
                    {
                        Debug.Log("id " + currentPlace.name + " position " + currentPlace.relPos.pos2);
                        // if (!UtterancesManager.Instance.SecondPrompt1Position(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), currentPlace.relPos.pos2))
                        if (!UtterancesManager.Instance.SecondPrompt1Position(GameState.Instance.PieceInformation(currentPiece.name), currentPlace.relPos.pos2))
                        {
                            repeatRelativePosition = true;
                            repeatPromptTime = DateTime.Now;
                            return;
                        }
                        else
                            repeatRelativePosition = false;
                    }
                    else if (currentPlace.relPos.pos2 == null)
                    {
                        Debug.Log("id " + currentPlace.name + " position " + currentPlace.relPos.pos1);
                        // if (!UtterancesManager.Instance.SecondPrompt1Position(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), currentPlace.relPos.pos1))
                        if (!UtterancesManager.Instance.SecondPrompt1Position(GameState.Instance.PieceInformation(currentPiece.name), currentPlace.relPos.pos1))
                        {
                            repeatRelativePosition = true;
                            repeatPromptTime = DateTime.Now;
                            return;
                        }
                        else
                            repeatRelativePosition = false;
                    }
                }
                else // prompt == "SecondPrompt2Position"
                {
                    Debug.Log("id " + currentPlace.name + " position1 " + currentPlace.relPos.pos1 + " position2 " + currentPlace.relPos.pos2);
                    //  if (!UtterancesManager.Instance.SecondPrompt2Position(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), currentPlace.relPos.pos1, currentPlace.relPos.pos2))
                    if (!UtterancesManager.Instance.SecondPrompt2Position(GameState.Instance.PieceInformation(currentPiece.name), currentPlace.relPos.pos1, currentPlace.relPos.pos2))
                    {
                        repeatRelativePosition = true;
                        repeatPromptTime = DateTime.Now;
                        return;
                    }
                    else
                        repeatRelativePosition = false;
                }

                prompedRelativePosition = true;
            }
        }
    }

    Dictionary<int, string> IntersectDictionaries(Dictionary<int, string> dic1, Dictionary<int, Piece> dic2)
    {
        Dictionary<int, string> result = new Dictionary<int, string>();
        IEnumerable<int> commonKeys = dic1.Keys.Intersect(dic2.Keys);
        foreach (int i in commonKeys)
        {
            result.Add(i, dic1[i]);
        }
        return result;
    }

    public void StartedMoving(bool correctAngle)
    {
        //lastPromptTime = DateTime.Now;

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

        //Check if instance already exists
        if (instance == null)
            instance = this;

        if (nPrompts > 0)
        {
            if (Therapist.Instance.nWrongAngleTries >= 2 || (nIncorrectAngle > 0 && (DateTime.Now - incorrectAngleTime).TotalSeconds > 12)
                || (goToSecondAnglePrompt && (DateTime.Now - goToSecondAnglePromptTime).TotalSeconds > 5))
            {
                UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> SecondAnglePrompt");
                CallFeedback();
                //SecondAnglePrompt();
                Debug.Log("2nd prompt -> 2ndAngle");
                return;
            }
            else if (((repeatRelativePosition || repeatHardClue || repeatPrompt) && (DateTime.Now - repeatPromptTime).TotalSeconds > 4)
                || (DateTime.Now - lastPromptTime).TotalSeconds > 20 || Therapist.Instance.nFailedTries >= 2)
            {
                if (repeatRelativePosition || repeatHardClue || repeatPrompt || nPrompts < 3)
                {
                    UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> RepeatPrompt SecondPrompt");
                    CallFeedback();
                    //RepeatPrompt();
                    return;
                }
                else if (nPrompts >= 2)
                {
                    Debug.Log("2nd prompt -> vai para o terceiro estado");
                    UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> ThirdPrompt");
                    CallFeedback();
                    //ThirdPrompt();
                    return;
                }
            }
        }
        else if (((repeatRelativePosition || repeatHardClue || repeatPrompt) && (DateTime.Now - repeatPromptTime).TotalSeconds > 4))
        {
            UtterancesManager.Instance.WriteJSON("--- OLD FEEDBACK -> SecondPrompt");
            CallFeedback();
            //SecondPrompt();
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
            Therapist.Instance.RatingsFeedback.ButtonsDesactivation();

            Therapist.Instance.AlgorithmUCB_.UpdateReward(previous_ActionsTherapist, 3);
            Therapist.Instance.RatingsFeedback.previousAction = previous_ActionsTherapist;
            Therapist.Instance.RatingsFeedback.previousFeedback = 3;
        }

        Therapist.Instance.AlgorithmUCB_.RunUCB();
        Therapist.Instance.Feedback();
    }

    public void EndGame()
    {
        nPrompts = 0;
        alreadyPromped = false;
        prompedRelativePosition = false;
        prompedRelativePieces = false;
        repeatHardClue = false;
        repeatPrompt = false;
        repeatRelativePosition = false;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.showedHardClue = false; ;
        Therapist.Instance.currentState = Therapist.Instance.FinalState;
        Therapist.Instance.EndGame();
    }

    public void HelpMotor()
    {
        repeatHardClue = false;
        repeatPrompt = false;
        repeatRelativePosition = false;
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
        repeatRelativePosition = false;
        Therapist.Instance.nFailedTries = 0;
        lastPromptTime = DateTime.Now;
        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.FitHelpState;
        Therapist.Instance.HelpAdjustingPiece();
    }

    public void GivePositiveFeedback()
    {
        repeatHardClue = false;
        repeatPrompt = false;
        repeatRelativePosition = false;
        nPrompts = 0;
        alreadyPromped = false;
        prompedRelativePosition = false;
        prompedRelativePieces = false;
        Therapist.Instance.currentState = Therapist.Instance.PositiveFeedState;
        Therapist.Instance.GivePositiveFeedback();
    }

    public void GiveNegativeFeedback()
    {
        Therapist.Instance.previousState = Therapist.Instance.SecondPromptState;
        Therapist.Instance.currentState = Therapist.Instance.NegativeFeedState;
        Debug.Log("2promp state-> neg feed");
        Therapist.Instance.GiveNegativeFeedback();
    }

    public void ThirdPrompt()
    {
        repeatHardClue = false;
        repeatPrompt = false;
        repeatRelativePosition = false;
        nPrompts = 0;
        alreadyPromped = false;
        prompedRelativePosition = false;
        prompedRelativePieces = false;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.showedHardClue = false;
        Therapist.Instance.currentState = Therapist.Instance.ThirdPromptState;
        Therapist.Instance.ThirdPrompt();
    }

    public void SecondAnglePrompt()
    {
        repeatHardClue = false;
        repeatPrompt = false;
        repeatRelativePosition = false;
        nPrompts = 0;
        alreadyPromped = false;
        prompedRelativePosition = false;
        prompedRelativePieces = false;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.showedHardClue = false;
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

        //////

        Therapist.Instance.currentState = Therapist.Instance.FirstIdlePromptState;
        Therapist.Instance.FirstIdlePrompt();

        /////

    }

    public void FirstAnglePrompt()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.FirstAnglePromptState;
        Therapist.Instance.FirstAnglePrompt();

        /////////

    }

    public void FirstPlacePrompt()
    {
        /////////

        Therapist.Instance.currentState = Therapist.Instance.FirstPlacePromptState;
        Therapist.Instance.FirstPlacePrompt();

        /////////

    }

    public void ThirdAnglePrompt()
    {

        ///////////

        Therapist.Instance.currentState = Therapist.Instance.ThirdAnglePromptState;
        Therapist.Instance.ThirdAnglePrompt();

        //////////

    }

    public void HardCluePrompt()
    {
    }

    void State.Prompt(string prompt_name)
    {
        prompt = prompt_name;
    }
}
