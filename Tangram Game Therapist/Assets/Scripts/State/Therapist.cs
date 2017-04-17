using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Exp3;
using Assets.Scripts.UCB;

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
    UCB AlgorithmUCB = new UCB(19, rewards);

    // public Piece lastPieceUsed;


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
        currentState.HelpMotor();
    }

    public void HelpAdjustingPiece()
    {
        currentState.HelpAdjustingPiece();
    }

    public void GivePositiveFeedback()
    {
        nFailedTries = 0;
        nWrongAngleTries = 0;
        showedHardClue = false;
        currentPiece = null;
        previousState = null;
        currentState.GivePositiveFeedback();
    }

    public void GiveNegativeFeedback()
    {
        currentState.GiveNegativeFeedback();
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
        currentState.FirstIdlePrompt();

    }

    public void FirstAnglePrompt()
    {
        currentState.FirstAnglePrompt();

    }

    public void FirstPlacePrompt()
    {
        currentState.FirstPlacePrompt();

    }

    public void SecondAnglePrompt()
    {
        currentState.SecondAnglePrompt();

    }

    public void SecondPrompt()
    {
        currentState.SecondPrompt();

    }

    public void ThirdAnglePrompt()
    {
        currentState.ThirdAnglePrompt();

    }

    public void ThirdPrompt()
    {
        currentState.ThirdPrompt();

    }


    public void Feedback()
    {
        try
        {
            //  switch (AlgorithmEXP3_.Action)
            switch (AlgorithmUCB_.Action)

            {
                case 0:// -> motor_help
                    Console.WriteLine("motor_help");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> HelpMotor");
                    HelpMotor();
                    break;
                case 1:// -> close_help
                    Console.WriteLine("close_help");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> HelpAdjustingPiece");

                    HelpAdjustingPiece();
                    break;
                case 2:// -> positive feedback
                    Console.WriteLine("positive feedback");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> PositiveFeedback");

                    GivePositiveFeedback();
                    break;
                case 3:// -> negative feedback
                    Console.WriteLine("negative feedback");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> NegativeFeedback");

                    GiveNegativeFeedback();
                    break;
                case 4:// -> first angle prompt
                    Console.WriteLine("first angle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> FirstAnglePrompt");

                    FirstAnglePrompt();
                    break;
                case 5:// -> first finger angle prompt
                    Console.WriteLine("first finger angle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> FirstFingerAnglePrompt");

                    FirstAnglePrompt();//ver se é mesmo assim, possívelmente tenho que adicionar mais qualquer coisa antes
                    break;
                case 6:// -> first button angle prompt
                    Console.WriteLine("first button angle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> FirstButtonAnglePrompt");

                    FirstAnglePrompt();//ver se é mesmo assim, possívelmente tenho que adicionar mais qualquer coisa antes
                    break;
                case 7:// -> second angle prompt
                    Console.WriteLine("second angle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> SecondAnglePrompt");

                    // tenho que saber qual é a peça que está selecionada neste momento está a enviar null :s


                    SecondAnglePrompt();
                    break;
                case 8:// -> second finger angle prompt
                    Console.WriteLine("second finger angle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> SecondFingerAnglePrompt");

                    SecondAnglePrompt();//ver se é mesmo assim, possívelmente tenho que adicionar mais qualquer coisa antes
                    break;
                case 9:// -> second button angle prompt
                    Console.WriteLine("second button angle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> SecondButtonAnglePrompt");

                    SecondAnglePrompt();//ver se é mesmo assim, possívelmente tenho que adicionar mais qualquer coisa antes
                    break;
                case 10:// -> third angle prompt
                    Console.WriteLine(" third angle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> ThirdAnglePrompt");

                    ThirdAnglePrompt();
                    break;
                case 11:// -> stop angle prompt
                    Console.WriteLine("stop angle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> StopAnglePrompt");


                    //if (currentPiece == null)
                    //{
                    //    UtterancesManager.Instance.StopAnglePrompt(lastPieceUsed.ToString());
                    //}
                    //else
                    //{
                    //    UtterancesManager.Instance.StopAnglePrompt(currentPiece.ToString());
                    //}

                    if (currentPiece == null)
                    {
                        Piece piece = GameState.Instance.FindNewPiece();
                        currentPiece = piece;

                    }

                    UtterancesManager.Instance.StopAnglePrompt(currentPiece.ToString());

                    break;
                case 12:// -> idle prompt
                    Console.WriteLine("idle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> FirstIdlePrompt");

                    FirstIdlePrompt();//ver se é mesmo assim, possívelmente tenho que adicionar mais qualquer coisa antes
                    //nºao sei se é aqui
                    break;
                case 13:// -> place prompt
                    Console.WriteLine("place prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> FirstPlacePrompt");

                    FirstPlacePrompt();
                    break;
                case 14:// -> sec_1position prompt
                    Console.WriteLine("sec_1position prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> Sec_1PositionPrompt");


                    // currentPiece;

                    //tenho que adicionar a peça que estou a usar



                    SecondPrompt();//ver se é mesmo assim, possívelmente tenho que adicionar mais qualquer coisa antes
                    break;
                case 15:// -> sec_2position prompt
                    Console.WriteLine("sec_2position prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> Sec_2PositionPrompt");


                    // currentPiece;

                    //tenho que adicionar a peça que estou a usar


                    SecondPrompt();//ver se é mesmo assim, possívelmente tenho que adicionar mais qualquer coisa antes
                    break;
                case 16:// -> sec_place prompt
                    Console.WriteLine("sec_place prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> SecPlacePrompt");


                    //currentPiece;

                    //tenho que adicionar a peça que estou a usar



                    SecondPrompt();
                    break;
                case 17:// -> third prompt
                    Console.WriteLine("third prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> ThirdPrompt");

                    ThirdPrompt();
                    break;
                case 18:// -> hard_clue prompt
                    Console.WriteLine("hard_clue prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> HardClue");

                    showedHardClue = true;//ver se é mesmo assim, possívelmente tenho que adicionar mais qualquer coisa antes
                    ThirdPrompt();
                    break;

            }

            AlgorithmEXP3_.Action = -1;

        }
        catch (Exception e)
        {
            Debug.Log("Error : " + e.Message);
            UtterancesManager.Instance.WriteJSON("Error : " + e.Message + " action = " + AlgorithmEXP3_.Action);
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

    internal Exp3 AlgorithmEXP3_
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

    internal UCB AlgorithmUCB_
    {
        get
        {
            return AlgorithmUCB;
        }

        set
        {
            AlgorithmUCB = value;
        }
    }
}
