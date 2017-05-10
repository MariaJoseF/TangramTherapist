using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Exp3;
using Assets.Scripts.UCB;
using Assets.Scripts.Learning;

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
    State hardClueState = new HardClueState();

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
        0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f,
        0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f};

    //Exp3 AlgorithmEXP3 = new Exp3(17, rewards, 0.07f);
    UCB AlgorithmUCB = new UCB(17, rewards);
    Ratings ratingsFeedback = new Ratings();
    String action_name = "";

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


    public void HardCluePrompt()
    {
        currentState.HardCluePrompt();

    }

    public void Feedback()
    {
        bool give_Feedback = false;
        try
        {

            showedHardClue = false;
            //  switch (AlgorithmEXP3_.Action)
            switch (AlgorithmUCB_.Action)
            {
                case 0:// -> motor_help
                    Console.WriteLine("motor_help");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> HelpMotor");
                    HelpMotor();
                    give_Feedback = true;
                    action_name = "Motor Help prompt";
                    break;
                case 1:// -> close_help
                    Console.WriteLine("close_help");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> HelpAdjustingPiece");

                    HelpAdjustingPiece();
                    give_Feedback = true;
                    action_name = "Close Help prompt";
                    break;
                //case 2:// -> positive feedback
                //    Console.WriteLine("positive feedback");
                //    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> PositiveFeedback");

                //    GivePositiveFeedback();
                //    give_Feedback = true;
                //    action_name = "Positive prompt";
                //    break;
                //case 3:// -> negative feedback
                //    Console.WriteLine("negative feedback");
                //    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> NegativeFeedback");

                //    GiveNegativeFeedback();
                //    give_Feedback = true;
                //    action_name = "Negative prompt";
                //    break;
                case 2:// -> first angle prompt
                    Console.WriteLine("first angle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> FirstAnglePrompt");

                    FirstAnglePrompt();
                    give_Feedback = true;
                    action_name = "First Angle prompt";
                    break;
                case 3:// -> first finger angle prompt
                    Console.WriteLine("first finger angle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> FirstFingerAnglePrompt");

                    FirstAnglePrompt();//ver se é mesmo assim, possívelmente tenho que adicionar mais qualquer coisa antes
                    give_Feedback = true;
                    action_name = "First Finger Angle prompt";
                    break;
                case 4:// -> first button angle prompt
                    Console.WriteLine("first button angle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> FirstButtonAnglePrompt");

                    FirstAnglePrompt();//ver se é mesmo assim, possívelmente tenho que adicionar mais qualquer coisa antes
                    give_Feedback = true;
                    action_name = "First Button Angle prompt";
                    break;
                case 5:// -> second angle prompt
                    Console.WriteLine("second angle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> SecondAnglePrompt");

                    // tenho que saber qual é a peça que está selecionada neste momento está a enviar null :s


                    SecondAnglePrompt();
                    give_Feedback = true;
                    action_name = "Second Angle prompt";
                    break;
                case 6:// -> second finger angle prompt
                    Console.WriteLine("second finger angle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> SecondFingerAnglePrompt");

                    SecondAnglePrompt();//ver se é mesmo assim, possívelmente tenho que adicionar mais qualquer coisa antes
                    give_Feedback = true;
                    action_name = "Second Finger Angle prompt";
                    break;
                case 7:// -> second button angle prompt
                    Console.WriteLine("second button angle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> SecondButtonAnglePrompt");

                    SecondAnglePrompt();//ver se é mesmo assim, possívelmente tenho que adicionar mais qualquer coisa antes
                    give_Feedback = true;
                    action_name = "Second Button Angle prompt";
                    break;
                case 8:// -> third angle prompt
                    Console.WriteLine(" third angle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> ThirdAnglePrompt");

                    ThirdAnglePrompt();
                    give_Feedback = true;
                    action_name = "Third Angle prompt";
                    break;
                case 9:// -> stop angle prompt
                    Console.WriteLine("stop angle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> StopAnglePrompt");

                    if (currentPiece == null)
                    {
                        Piece piece = GameState.Instance.FindNewPiece();
                        currentPiece = piece;
                    }

                    UtterancesManager.Instance.StopAnglePrompt(currentPiece.ToString());
                    give_Feedback = true;
                    action_name = "Stop Angle prompt";
                    break;
                case 10:// -> idle prompt
                    Console.WriteLine("idle prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> FirstIdlePrompt");

                    FirstIdlePrompt();//ver se é mesmo assim, possívelmente tenho que adicionar mais qualquer coisa antes
                    give_Feedback = true;
                    action_name = "Idle prompt";
                    break;
                case 11:// -> place prompt
                    Console.WriteLine("place prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> FirstPlacePrompt");

                    FirstPlacePrompt();
                    give_Feedback = true;
                    action_name = "Place prompt";
                    break;
                case 12:// -> sec_1position prompt
                    Console.WriteLine("sec_1position prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> Sec_1PositionPrompt");


                    // currentPiece;

                    //tenho que adicionar a peça que estou a usar



                    SecondPrompt();//ver se é mesmo assim, possívelmente tenho que adicionar mais qualquer coisa antes
                    give_Feedback = true;
                    action_name = "Second 1 Position prompt";
                    break;
                case 13:// -> sec_2position prompt
                    Console.WriteLine("sec_2position prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> Sec_2PositionPrompt");


                    // currentPiece;

                    //tenho que adicionar a peça que estou a usar


                    SecondPrompt();//ver se é mesmo assim, possívelmente tenho que adicionar mais qualquer coisa antes
                    give_Feedback = true;
                    action_name = "Second 2 Position prompt";
                    break;
                case 14:// -> sec_place prompt
                    Console.WriteLine("sec_place prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> SecPlacePrompt");

                    SecondPrompt();
                    give_Feedback = true;
                    action_name = "Second Place prompt";
                    break;
                case 15:// -> third prompt
                    Console.WriteLine("third prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> ThirdPrompt");

                    if (currentPiece == null)
                    {
                        Piece piece = GameState.Instance.FindNewPiece();
                        currentPiece = piece;
                    }

                    currentPlace = GameState.Instance.FindTheCorrectPlace(currentPiece);

                    ThirdPrompt();
                    give_Feedback = true;
                    action_name = "Third prompt";
                    break;
                case 16:// -> hard_clue prompt
                    Console.WriteLine("hard_clue prompt");
                    UtterancesManager.Instance.WriteJSON("--- NEW FEEDBACK -> HardClue");


                    if (currentPiece == null)
                    {
                        Piece piece = GameState.Instance.FindNewPiece();
                        currentPiece = piece;
                    }

                    currentPlace = GameState.Instance.FindTheCorrectPlace(currentPiece);

                    ThirdPrompt();
                    give_Feedback = true;
                    action_name = "Hard Clue prompt";

                    //    UtterancesManager.Instance.HardClue(0.4f);
                    //   HardCluePrompt();
                    //  repeatHardClue = true;

                    // repeatPromptTime = DateTime.Now;

                    break;

            }

            if (give_Feedback)
            {

                ratingsFeedback.Button_1.Enabled = true;
                ratingsFeedback.Button_2.Enabled = true;
                ratingsFeedback.Button_3.Enabled = true;
                ratingsFeedback.Button_4.Enabled = true;
                ratingsFeedback.Button_5.Enabled = true;

                ratingsFeedback.Label1.Text = "Feedback " + action_name;

                ratingsFeedback.Button_1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
                ratingsFeedback.Button_2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
                ratingsFeedback.Button_3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
                ratingsFeedback.Button_4.BackColor = System.Drawing.SystemColors.ControlDarkDark;
                ratingsFeedback.Button_5.BackColor = System.Drawing.SystemColors.ControlDarkDark;

                ratingsFeedback.ActionNumber1 = AlgorithmUCB_.Action;

                ratingsFeedback.form_Feedback.Show();
                give_Feedback = false;
            }
            else
            {
                ratingsFeedback.Button_1.Enabled = false;
                ratingsFeedback.Button_2.Enabled = false;
                ratingsFeedback.Button_3.Enabled = false;
                ratingsFeedback.Button_4.Enabled = false;
                ratingsFeedback.Button_5.Enabled = false;

                ratingsFeedback.Button_1.BackColor = System.Drawing.SystemColors.Control;
                ratingsFeedback.Button_2.BackColor = System.Drawing.SystemColors.Control;
                ratingsFeedback.Button_3.BackColor = System.Drawing.SystemColors.Control;
                ratingsFeedback.Button_4.BackColor = System.Drawing.SystemColors.Control;
                ratingsFeedback.Button_5.BackColor = System.Drawing.SystemColors.Control;
            }

            AlgorithmUCB_.Action = -1;
            action_name = "";

        }
        catch (Exception e)
        {
            Debug.Log("Error : " + e.Message);
            UtterancesManager.Instance.WriteJSON("Error : " + e.Message + " action = " + AlgorithmUCB_.Action);
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

    //internal Exp3 AlgorithmEXP3_
    //{
    //    get
    //    {
    //        return AlgorithmEXP3;
    //    }

    //    set
    //    {
    //        AlgorithmEXP3 = value;
    //    }
    //}

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

    public State HardClueState
    {
        get
        {
            return hardClueState;
        }

        set
        {
            hardClueState = value;
        }
    }

    public Ratings RatingsFeedback
    {
        get
        {
            return ratingsFeedback;
        }

        set
        {
            ratingsFeedback = value;
        }
    }
}
