using UnityEngine;
using System;
using Assets.Scripts.Learning;
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
    static float[] rewards = { };

    //Exp3 AlgorithmEXP3 = new Exp3(17, rewards, 0.07f);
    internal UCB AlgorithmUCB = new UCB(5, rewards);
    internal Ratings ratingsFeedback = new Ratings();
    String action_name = "";


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
    private bool positive_feedback = false;
    private bool firstPrompt = true;
    private bool secondPrompt = true;
    private bool thirdPrompt = true;
    private int previousAction;

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

        if (positive_feedback)
        {
            positive_feedback = false;
            SetPrompts();
            // Debug.Log("Positive = " + poaitive_feedback);
        }
        else
        {
            positive_feedback = true;
            // Debug.Log("Positive = " + poaitive_feedback);

        }

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

        SetPrompts();
    }

    internal void SetPrompts()
    {
        AlgorithmUCB.RunUCB();

        switch (AlgorithmUCB.Action)
        {
            case 0://all prompts
                firstPrompt = true;
                secondPrompt = true;
                thirdPrompt = true;
                action_name = "All prompts";
                break;
            case 1://only first and second prompts
                firstPrompt = true;
                secondPrompt = true;
                thirdPrompt = false;
                action_name = "First and Second prompts";
                break;
            case 2://only first and third prompts
                firstPrompt = true;
                secondPrompt = false;
                thirdPrompt = true;
                action_name = "First and Third prompts";
                break;
            case 3://only second and third prompts
                firstPrompt = false;
                secondPrompt = true;
                thirdPrompt = true;
                action_name = "Second and Third prompts";
                break;
            case 4://no prompts
                firstPrompt = false;
                secondPrompt = false;
                thirdPrompt = false;
                action_name = "No prompts";
                break;
        }


        previousAction = AlgorithmUCB.Action;

        //if (give_Feedback)
        //{

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

            ratingsFeedback.ActionNumber1 = AlgorithmUCB.Action;

            ratingsFeedback.form_Feedback.Show();
            //give_Feedback = false;
        //}
        //else
        //{
        //    ratingsFeedback.Button_1.Enabled = false;
        //    ratingsFeedback.Button_2.Enabled = false;
        //    ratingsFeedback.Button_3.Enabled = false;
        //    ratingsFeedback.Button_4.Enabled = false;
        //    ratingsFeedback.Button_5.Enabled = false;

        //    ratingsFeedback.Button_1.BackColor = System.Drawing.SystemColors.Control;
        //    ratingsFeedback.Button_2.BackColor = System.Drawing.SystemColors.Control;
        //    ratingsFeedback.Button_3.BackColor = System.Drawing.SystemColors.Control;
        //    ratingsFeedback.Button_4.BackColor = System.Drawing.SystemColors.Control;
        //    ratingsFeedback.Button_5.BackColor = System.Drawing.SystemColors.Control;
        //}

        AlgorithmUCB.Action = -1;
        action_name = "";

        Console.WriteLine("firstPrompt = " + firstPrompt + " secondPrompt = " + secondPrompt + " thirdPrompt = " + thirdPrompt);
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

    public bool First_Prompt
    {
        get
        {
            return firstPrompt;
        }

        set
        {
            firstPrompt = value;
        }
    }

    public bool Second_Prompt
    {
        get
        {
            return secondPrompt;
        }

        set
        {
            secondPrompt = value;
        }
    }

    public bool Third_Prompt
    {
        get
        {
            return thirdPrompt;
        }

        set
        {
            thirdPrompt = value;
        }
    }
}
