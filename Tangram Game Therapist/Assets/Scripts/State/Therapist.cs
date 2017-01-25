using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Exp3;

public class Therapist : MonoBehaviour
{
    private static Therapist instance = null;
    public int numberOfGames = 0;
    public int firstTimeButton = 0; //0 - never 1 - first time 2 - already
    public int firstTimeFinger = 0; //0 - never 1 - first time 2 - already
    public int nFailedTries;
    public int nWrongAngleTries;

    static State greetingState = new GreetingState();
    State initialHelpState = new InitialHelpState();
    State fingerHelpState = new FingerHelpState();
    State buttonHelpState = new ButtonHelpState();
    State fitHelpState = new FitHelpState();
    State motorHelpState = new MotorHelpState();
    State playState = new PlayState();
    State startState = new StartState();
    State finalState = new FinalState();
    State positiveFeedState = new PositiveFeedState();
    State negativeFeedState = new NegativeFeedState();
    State firstIdlePromptState = new FirstIdlePromptState();
    State firstAnglePromptState = new FirstAnglePromptState();
    State firstPlacePromptState = new FirstPlacePromptState();
    State secondAnglePromptState = new SecondAnglePromptState();
    State secondPromptState = new SecondPromptState();
    State thirdAnglePromptState = new ThirdAnglePromptState();
    State thirdPromptState = new ThirdPromptState();

    public State currentState = greetingState;
    public State previousState;
    public Piece currentPiece;
    public PieceSolution currentPlace;
    public bool showedHardClue = false;
    DateTime gameEndedTime;



    /// 
    /// //////////////////////
    /// 
    static float[] rewards = { 0.2f, 0.2f,
        0.5f, 0.0f,
        0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f,
        0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f};

    Exp3 AlgorithmEXP3 = new Exp3(19, rewards, 0.07f);



    /// 
    /// ///////////////
    /// 

    public struct GameSettings
    {
        public SolutionManager.Difficulty difficulty;
        public string puzzle;
        public SceneProperties.RotationMode rotationMode;
        public float distanceThreshold;
    }

    public GameSettings currentGame;

    public static Therapist Instance
    {
        get
        {
            return instance;
        }
    }

    void Start()
    {
    }

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }

    void Update()
    {
        if (currentState == null)
            currentState = PlayState;
        currentState.Update();
    }

    public void BeginFirstGame()
    {
        currentState.BeginFirstGame();
    }

    public void BeginNextGame()
    {
        currentState.BeginNextGame();
    }

    public void EndGame()
    {
        currentState.EndGame();
    }

    public void HelpMotor()
    {
        //currentState.HelpMotor ();
        RunEXP_Action();
    }

    public void HelpAdjustingPiece()
    {
        //currentState.HelpAdjustingPiece ();
        RunEXP_Action();
    }

    public void GivePositiveFeedback()
    {
        nFailedTries = 0;
        nWrongAngleTries = 0;
        showedHardClue = false;
        currentPiece = null;
        previousState = null;
        //currentState.GivePositiveFeedback ();
        RunEXP_Action();
    }

    public void GiveNegativeFeedback()
    {
        //currentState.GiveNegativeFeedback ();
        RunEXP_Action();
        if (previousState != null)
            currentState = previousState;
        else currentState = PlayState;
    }

    public void StartedMoving(bool correctAngle)
    {
        currentState.StartedMoving(correctAngle);
    }

    public void FirstIdlePrompt()
    {
        //currentState.FirstIdlePrompt();
        RunEXP_Action();
    }

    public void FirstAnglePrompt()
    {
        //currentState.FirstAnglePrompt();
        RunEXP_Action();
    }

    public void FirstPlacePrompt()
    {
        //currentState.FirstPlacePrompt();
        RunEXP_Action();
    }

    public void SecondAnglePrompt()
    {
        //currentState.SecondAnglePrompt();
        RunEXP_Action();
    }

    public void SecondPrompt()
    {
        //currentState.SecondPrompt();
        RunEXP_Action();
    }

    public void ThirdAnglePrompt()
    {
        //currentState.ThirdAnglePrompt();
        RunEXP_Action();
    }

    public void ThirdPrompt()
    {
        //currentState.ThirdPrompt();
        RunEXP_Action();
    }


    public void RunEXP_Action()
    {
        try
        {
            switch (AlgorithmEXP31.Action)
            {
                case 0:// -> motor_help
                    MotorHelpState.HelpMotor();
                    UtterancesManager.Instance.MotorHelp();
                    break;
                case 1:// -> close_help
                       FitHelpState.HelpAdjustingPiece();
                    UtterancesManager.Instance.CloseHelp();
                    break;
                case 2:// -> positive feedback
                      PositiveFeedState.GivePositiveFeedback();
                   // UtterancesManager.Instance.PositiveFeedback(StringNumberOfPieces(GameState.Instance.notPlacedPieces.Count));
                    break;
                case 3:// -> negative feedback
                       NegativeFeedState.GiveNegativeFeedback();
                    UtterancesManager.Instance.NegativeFeedback();
                    break;
                case 4:// -> first angle prompt
                     FirstAnglePromptState.FirstAnglePrompt();
                    //UtterancesManager.Instance.FirstAnglePrompt(GameState.Instance.PieceInformation(piece_.name));
                    UtterancesManager.Instance.FirstAnglePrompt(GameState.Instance.PieceInformation(currentPiece.name));

                    break;
                case 5:// -> first finger angle prompt
                       FirstAnglePromptState.RepeatPrompt();
                    UtterancesManager.Instance.FirstAnglePromptFinger(GameState.Instance.PieceInformation(currentPiece.name));
                    //  GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name)
                    break;
                case 6:// -> first button angle prompt
                        FirstAnglePromptState.RepeatPrompt();
                    UtterancesManager.Instance.FirstAnglePromptButton(GameState.Instance.PieceInformation(currentPiece.name));
                    //                    UtterancesManager.Instance.FirstAnglePromptButton(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));

                    break;
                case 7:// -> second angle prompt
                       SecondAnglePromptState.SecondAnglePrompt();
                    UtterancesManager.Instance.SecondAnglePrompt(GameState.Instance.PieceInformation(currentPiece.name));
                    //                    UtterancesManager.Instance.SecondAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));

                    break;
                case 8:// -> second finger angle prompt
                    Therapist.Instance.SecondAnglePromptState.SecondAnglePrompt();

                   // currentPlace = GameState.Instance.FindTheCorrectPlace(currentPiece);

                    //rotationDirection = CalculateDirectionOfRotation(currentPiece, currentPlace);

                    //UtterancesManager.Instance.SecondAnglePromptFinger(GameState.Instance.PieceInformation(currentPiece.name), rotationDirection);
                    break;
                case 9:// -> second button angle prompt
                       SecondAnglePromptState.SecondAnglePrompt();
                    //UtterancesManager.Instance.SecondAnglePromptButton(GameState.Instance.PieceInformation(currentPiece.name), StringNumberOfClicks(GameState.Instance.numberOfClicks));
                    //                    UtterancesManager.Instance.SecondAnglePromptButton(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name), StringNumberOfClicks(GameState.Instance.numberOfClicks));
                    break;
                case 10:// -> third angle prompt
                       ThirdAnglePromptState.ThirdAnglePrompt();
                    UtterancesManager.Instance.ThirdAnglePrompt(GameState.Instance.PieceInformation(currentPiece.name));
                    // UtterancesManager.Instance.ThirdAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));
                    break;
                case 11:// -> stop angle prompt
                       // ThirdAnglePromptState.StartedMoving(true);
                        // UtterancesManager.Instance.StopAnglePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));
                    UtterancesManager.Instance.StopAnglePrompt(GameState.Instance.PieceInformation(currentPiece.name));
                    break;
                case 12:// -> idle prompt
                      FirstIdlePromptState.FirstIdlePrompt();
                    UtterancesManager.Instance.FirstIdlePrompt(GameState.Instance.PieceInformation(currentPiece.name));
                    //UtterancesManager.Instance.FirstIdlePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));
                    break;
                case 13:// -> place prompt
                        FirstPlacePromptState.FirstPlacePrompt();
                    UtterancesManager.Instance.FirstPlacePrompt(GameState.Instance.PieceInformation(currentPiece.name));
                    //                    UtterancesManager.Instance.FirstPlacePrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name));
                    break;
                case 14:// -> sec_1position prompt
                        SecondPromptState.SecondPrompt();
                    // currentPiece = Therapist.Instance.currentPiece;
                    currentPlace = GameState.Instance.FindTheCorrectPlace(currentPiece);


                    string a = currentPlace.relPos.pos2;



                    UtterancesManager.Instance.SecondPrompt1Position(GameState.Instance.PieceInformation(currentPiece.name), currentPlace.relPos.pos2);
                    break;
                case 15:// -> sec_2position prompt
                       SecondPromptState.SecondPrompt();
                    
                    // currentPiece = Therapist.Instance.currentPiece;
                    //currentPlace = GameState.Instance.FindTheCorrectPlace(currentPiece);
                   // UtterancesManager.Instance.SecondPrompt2Position(GameState.Instance.PieceInformation(currentPiece.name), currentPlace.relPos.pos1, currentPlace.relPos.pos2);
                    break;
                case 16:// -> sec_place prompt
                        SecondPromptState.SecondPrompt();
                    //currentPlace = GameState.Instance.FindTheCorrectPlace(currentPiece);
                    //adjacentPieces = new Dictionary<int, String>(currentPlace.relPos.adjacentPieces);
                    //availableAdjacentPieces = new Dictionary<int, string>(IntersectDictionaries(adjacentPieces, GameState.Instance.placedPieces));

                    //int pieceId = GameState.Instance.RandomKeys(availableAdjacentPieces).First();
                    //string piece = SolutionManager.Instance.FindMatchIdName(pieceId);
                    //string relativePosition = availableAdjacentPieces[pieceId];

                    //UtterancesManager.Instance.SecondPromptPlace(GameState.Instance.PieceInformation(currentPiece.name), relativePosition, GameState.Instance.PieceInformation(piece));
                    break;
                case 17:// -> third prompt
                        ThirdPromptState.ThirdPrompt();
                    UtterancesManager.Instance.ThirdPrompt(GameState.Instance.PieceInformation(currentPiece.name));
                    break;
                case 18:// -> hard_clue prompt
                    UtterancesManager.Instance.HardClue(3f);
                    break;

            }

            AlgorithmEXP31.Action = -1;

        }
        catch (Exception e)
        {
            Debug.Log("Error : " + e.Message);
            UtterancesManager.Instance.WriteJSON("Error : " + e.Message + " action = " + AlgorithmEXP31.Action);
        }
        finally
        {
            Debug.Log("Finaly ExpAlgorithm.RunExp3(17, rewards, 0.07f);");
        }
    }

    public void Quit()
    {
        GameManager.Instance.quit = true;
        GameState.Instance.playButtonInteractable = false;
        currentState = startState;
        BeginNextGame();
    }

    public void BeginGame(SolutionManager.Difficulty difficulty, string puzzle, SceneProperties.RotationMode rotationMode, float distanceThreshold)
    {
        currentGame = new GameSettings
        {
            difficulty = difficulty,
            puzzle = puzzle,
            rotationMode = rotationMode,
            distanceThreshold = distanceThreshold
        };

        greetingState = new GreetingState();
        initialHelpState = new InitialHelpState();
        fingerHelpState = new FingerHelpState();
        buttonHelpState = new ButtonHelpState();
        fitHelpState = new FitHelpState();
        motorHelpState = new MotorHelpState();
        playState = new PlayState();
        startState = new StartState();
        finalState = new FinalState();
        positiveFeedState = new PositiveFeedState();
        negativeFeedState = new NegativeFeedState();
        firstIdlePromptState = new FirstIdlePromptState();
        firstAnglePromptState = new FirstAnglePromptState();
        firstPlacePromptState = new FirstPlacePromptState();
        secondAnglePromptState = new SecondAnglePromptState();
        secondPromptState = new SecondPromptState();
        thirdAnglePromptState = new ThirdAnglePromptState();
        thirdPromptState = new ThirdPromptState();

        previousState = null;
        currentPiece = null;
        currentPlace = null;
        showedHardClue = false;
        nFailedTries = 0;
        nWrongAngleTries = 0;

        if (firstTimeButton == 0 && rotationMode == SceneProperties.RotationMode.button)
        {
            firstTimeButton = 1;
            GameState.Instance.initialHelp = true;
        }
        else if (firstTimeFinger == 0 && rotationMode == SceneProperties.RotationMode.finger)
        {
            firstTimeFinger = 1;
            GameState.Instance.initialHelp = true;
        }
        else GameState.Instance.initialHelp = false;

        currentState = initialHelpState;

        numberOfGames++;
        if (numberOfGames > 1)
            BeginNextGame();
        else BeginFirstGame();
    }

    public State CurrentState
    {
        get
        {
            return currentState;
        }

        set
        {
            currentState = value;
        }
    }

    public State GreetingState
    {
        get
        {
            return greetingState;
        }
    }

    public State InitialHelpState
    {
        get
        {
            return initialHelpState;
        }
    }

    public State FingerHelpState
    {
        get
        {
            return fingerHelpState;
        }
    }

    public State ButtonHelpState
    {
        get
        {
            return buttonHelpState;
        }
    }

    public State FitHelpState
    {
        get
        {
            return fitHelpState;
        }
    }

    public State MotorHelpState
    {
        get
        {
            return motorHelpState;
        }
    }

    public State PlayState
    {
        get
        {
            return playState;
        }
    }

    public State StartState
    {
        get
        {
            return startState;
        }
    }

    public State FinalState
    {
        get
        {
            return finalState;
        }
    }

    public State PositiveFeedState
    {
        get
        {
            return positiveFeedState;
        }
    }

    public State NegativeFeedState
    {
        get
        {
            return negativeFeedState;
        }
    }

    public State FirstIdlePromptState
    {
        get
        {
            return firstIdlePromptState;
        }
    }

    public State FirstAnglePromptState
    {
        get
        {
            return firstAnglePromptState;
        }
    }

    public State FirstPlacePromptState
    {
        get
        {
            return firstPlacePromptState;
        }
    }

    public State SecondAnglePromptState
    {
        get
        {
            return secondAnglePromptState;
        }
    }

    public State SecondPromptState
    {
        get
        {
            return secondPromptState;
        }
    }

    public State ThirdAnglePromptState
    {
        get
        {
            return thirdAnglePromptState;
        }
    }

    public State ThirdPromptState
    {
        get
        {
            return thirdPromptState;
        }
    }

    internal Exp3 AlgorithmEXP31
    {
        get
        {
            return AlgorithmEXP3;
        }

        set
        {
            AlgorithmEXP3 = value;
        }
    }
}
