using UnityEngine;
using System;
using Assets.Scripts.Learning;
using System.Collections.Generic;
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
    static double[] rewards = { 0.5f, 0.5f };

    internal Exp3 AlgorithmEXP3 = new Exp3(2, rewards, 0.07f);
    internal Ratings ratingsFeedback = new Ratings();
   // internal List<int> vec_ratings = new List<int>();
    internal String action_name = "";
    public GameSettings currentGame;
    private bool positive_feedback = false;
    private bool niceRobot = true;
    private int previousAction = -1;

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

        //if (positive_feedback)
        //{
        //    positive_feedback = false;
        //}
        //else
        //{
        //    UtterancesManager.Instance.CheckUtteranceFinish();
        //    positive_feedback = true;
        //   // ratingsFeedback.ButtonsDesactivation();
        //}

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

    }


    internal void SetPrompts(string utterance)
    {

      //  AVG_Ratings();

        /*AlgorithmEXP3.RunExp3();*/

        //switch (AlgorithmEXP3.Action)
        //{
        //    case 0://not rude
        //        niceRobot = true;
        //        action_name = "Nice Robot";
        //        break;
        //    case 1://rude
        //        niceRobot = false;
        //        action_name = "Rude Robot";
        //        break;
        //}

        previousAction = AlgorithmEXP3.Action;

        //ratingsFeedback.Label1.Text = "Feedback " + action_name;

        ratingsFeedback.form_Feedback.Show();
        ratingsFeedback.ButtonsDesactivation();
        ratingsFeedback.feedback_val = -2;
        ratingsFeedback.default_form = 1;
        ratingsFeedback.name_utterance = utterance;

        //vec_ratings = new List<int>();//empty the previous vector of ratings for the new action prompt

        Console.WriteLine("Rude Robot = " + NiceRobotGet);
    }

    internal void ShowFormRatings()
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

        ratingsFeedback.ActionNumber1 = AlgorithmEXP3.Action;

        ratingsFeedback.feedback_val = -2.0f;
        ratingsFeedback.default_form = 1;

        ratingsFeedback.form_Feedback.Show();
    }

    //internal void AVG_Ratings()
    //{
    //    if (vec_ratings.Count > 0)
    //    {
    //        double aux = 0.0f;
    //        for (int i = 0; i < vec_ratings.Count; i++)
    //        {
    //            aux = aux + vec_ratings[i];
    //        }

    //        aux = (aux / vec_ratings.Count);

    //        AlgorithmEXP3.UpdateReward(previousAction, Math.Round(aux, 2));
    //    }
    //}

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

    public bool NiceRobot(string utterance_name)
    {
       
            if (ratingsFeedback.feedback_val == -2.0f)
            {
                ratingsFeedback.FileHeader();
            ratingsFeedback.WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), ";" + GameManager.Instance.playerName + ";" + GameManager.Instance.CurrentPuzzle + ";" + GameManager.Instance.Difficulty_ + ";" + GameManager.Instance.RotationMode_ + ";" + GameManager.Instance.DistanceThreshold + ";" + AlgorithmEXP3.Action + ";" + "-;" + utterance_name);
            }

            SetPrompts(utterance_name);

            AlgorithmEXP3.RunExp3(utterance_name);

            switch (AlgorithmEXP3.Action)
            {
                case 0://not rude
                    niceRobot = true;
                    action_name = "Nice Robot";
                    break;
                case 1://rude
                    niceRobot = false;
                    action_name = "Rude Robot";
                    break;
            }

            ratingsFeedback.Label1.Text = "Feedback " + action_name;

            return niceRobot;
    }

    public bool NiceRobotGet
    {
        get
        {
            return niceRobot;
        }
    }
}
