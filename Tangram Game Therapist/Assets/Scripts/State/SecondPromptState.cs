using UnityEngine;
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
        currentPiece = Therapist.Instance.currentPiece;

        bool utterance = false;
        UtterancesManager.Instance.CheckUtteranceFinish();

        if (nPrompts == 0)
        {
            random = UnityEngine.Random.Range(0, 3);
            if (repeatHardClue || random == 0 && Therapist.Instance.currentGame.difficulty == SolutionManager.Difficulty.hard
                && !Therapist.Instance.showedHardClue)
            {
                repeatHardClue = false;
                repeatPrompt = false;
                repeatRelativePosition = false;

                Debug.Log("2nd prompt ");

                /*NEW*/

                utterance = UtterancesManager.Instance.HardClue(4.0f, Therapist.Instance.NiceRobot);

                /*NEW*/


                if (!utterance)
                {
                    repeatHardClue = true;
                    repeatPromptTime = DateTime.Now;
                    nPrompts = 0;
                }
                else
                {
                    repeatHardClue = false;
                    nPrompts = 1;

                    ///
                    Therapist.Instance.ShowFormRatings();
                    ///
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

    void RepeatPrompt()
    {
        lastPromptTime = DateTime.Now;
        Therapist.Instance.nWrongAngleTries = 0;
        Therapist.Instance.nFailedTries = 0;
        random = UnityEngine.Random.Range(0, 3);

        bool utterance = false;
        UtterancesManager.Instance.CheckUtteranceFinish();

        if (repeatHardClue || (random == 0 && Therapist.Instance.currentGame.difficulty == SolutionManager.Difficulty.hard
            && !Therapist.Instance.showedHardClue))
        {
            repeatHardClue = false;
            repeatPrompt = false;
            repeatRelativePosition = false;

            Debug.Log("2nd prompt -> RepeatPrompt ");

            /*NEW*/

            utterance = UtterancesManager.Instance.HardClue(4.0f, Therapist.Instance.NiceRobot);

            /*NEW*/


            if (!utterance)
            {
                repeatHardClue = true;
                repeatPromptTime = DateTime.Now;
            }
            else
            {
                nPrompts++;
                repeatHardClue = false;

                ///
                Therapist.Instance.ShowFormRatings();
                ///
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
        currentPlace = GameState.Instance.FindTheCorrectPlace(Therapist.Instance.currentPiece);
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

        bool utterance = false;
        UtterancesManager.Instance.CheckUtteranceFinish();

        if (!alreadyPromped)
        {
            InitializeParameters();
        }

        if (currentPlace != null)
        {
            if (repeatPrompt || (!repeatRelativePosition && availableAdjacentPieces.Count > 0 && ((prompedRelativePosition && !prompedRelativePieces) || random == 1)))
            {
                repeatRelativePosition = false;
                int pieceId = GameState.Instance.RandomKeys(availableAdjacentPieces).First();
                string piece = SolutionManager.Instance.FindMatchIdName(pieceId);
                string relativePosition = availableAdjacentPieces[pieceId];
                Debug.Log("2nd prompt -> HelpWithRelativePosition -> id " + pieceId + " relative position " + relativePosition + " piece " + piece);

                /*NEW*/

                utterance = UtterancesManager.Instance.SecondPromptPlace(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), relativePosition, GameState.Instance.PieceInformation(piece), Therapist.Instance.NiceRobot);

                /*NEW*/

                if (!utterance)
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

                    ///
                    Therapist.Instance.ShowFormRatings();
                    ///
                }
            }
            else
            {
                repeatPrompt = false;
                if (currentPlace.relPos.pos1 == null)
                {
                    Debug.Log("2nd prompt -> id " + currentPlace.name + " position " + currentPlace.relPos.pos2);

                    /*NEW*/

                    utterance = UtterancesManager.Instance.SecondPrompt1Position(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), currentPlace.relPos.pos2, Therapist.Instance.NiceRobot);

                    /*NEW*/


                    if (!utterance)
                    {
                        repeatRelativePosition = true;
                        repeatPromptTime = DateTime.Now;
                        return;
                    }
                    else
                    {
                        repeatRelativePosition = false;

                        ///
                        Therapist.Instance.ShowFormRatings();
                        ///
                    }

                }
                else if (currentPlace.relPos.pos2 == null)
                {
                    Debug.Log("2nd prompt -> id " + currentPlace.name + " position " + currentPlace.relPos.pos1);

                    /*NEW*/

                    utterance = UtterancesManager.Instance.SecondPrompt1Position(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), currentPlace.relPos.pos1, Therapist.Instance.NiceRobot);

                    /*NEW*/


                    if (!utterance)
                    {
                        repeatRelativePosition = true;
                        repeatPromptTime = DateTime.Now;
                        return;
                    }
                    else
                    {
                        repeatRelativePosition = false;

                        ///
                        Therapist.Instance.ShowFormRatings();
                        ///
                    }

                }
                else
                {
                    Debug.Log("2nd prompt -> id " + currentPlace.name + " position1 " + currentPlace.relPos.pos1 + " position2 " + currentPlace.relPos.pos2);

                    /*NEW*/

                    utterance = UtterancesManager.Instance.SecondPrompt2Position(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), currentPlace.relPos.pos1, currentPlace.relPos.pos2, Therapist.Instance.NiceRobot);

                    /*NEW*/

                    if (!utterance)
                    {
                        repeatRelativePosition = true;
                        repeatPromptTime = DateTime.Now;
                        return;
                    }
                    else
                    {
                        repeatRelativePosition = false;

                        ///
                        Therapist.Instance.ShowFormRatings();
                        ///
                    }

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
        if (nPrompts > 0)
        {
            if (Therapist.Instance.nWrongAngleTries >= 2 || (nIncorrectAngle > 0 && (DateTime.Now - incorrectAngleTime).TotalSeconds > 12)
                || (goToSecondAnglePrompt && (DateTime.Now - goToSecondAnglePromptTime).TotalSeconds > 5))
            {
                SecondAnglePrompt();
                Debug.Log("2nd prompt -> 2ndAngle");
                return;
            }
            else if (((repeatRelativePosition || repeatHardClue || repeatPrompt) && (DateTime.Now - repeatPromptTime).TotalSeconds > 4)
                || (DateTime.Now - lastPromptTime).TotalSeconds > 20 || Therapist.Instance.nFailedTries >= 2)
            {
                if (repeatRelativePosition || repeatHardClue || repeatPrompt || nPrompts < 3)
                {
                    RepeatPrompt();
                    return;
                }
                else if (nPrompts >= 2)
                {
                    Debug.Log("2nd prompt -> vai para o terceiro estado");
                    ThirdPrompt();
                    return;
                }
            }
        }
        else if (((repeatRelativePosition || repeatHardClue || repeatPrompt) && (DateTime.Now - repeatPromptTime).TotalSeconds > 4))
        {
            SecondPrompt();
        }
        if (Therapist.Instance.currentPiece != currentPiece)
        {
            currentPiece = Therapist.Instance.currentPiece;
            InitializeParameters();
        }
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
    }

    public void FirstAnglePrompt()
    {
    }

    public void FirstPlacePrompt()
    {
    }

    public void ThirdAnglePrompt()
    {
    }
}
