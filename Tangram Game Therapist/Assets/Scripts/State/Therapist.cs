using UnityEngine;
using System.Collections;
using System;

public class Therapist : MonoBehaviour {
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
	State thirdAnglePromptState = new ThirdAnglePromptState ();
	State thirdPromptState = new ThirdPromptState();

	public State currentState = greetingState;
	public State previousState;
	public Piece currentPiece;
	public PieceSolution currentPlace;
	public bool showedHardClue = false;
    DateTime gameEndedTime;

	public struct GameSettings
	{
		public SolutionManager.Difficulty difficulty;
		public string puzzle;
		public SceneProperties.RotationMode rotationMode;
		public float distanceThreshold;
	}

	public GameSettings currentGame;

	public static Therapist Instance {
		get { 
			return instance; 
		}
	}

    void Start() {
    }
	 
	void Awake () {
		//Check if instance already exists
		if (instance == null)
			instance = this;
		
		else if (instance != this)
			Destroy(gameObject); 
	}

	void Update(){
		if (currentState == null)
			currentState = PlayState;
		currentState.Update ();
	}
	
	public void BeginFirstGame(){
		currentState.BeginFirstGame ();
	}
	
	public void BeginNextGame(){
		currentState.BeginNextGame ();
	}

	public void EndGame(){
		currentState.EndGame ();
	}

	public void HelpMotor(){
		currentState.HelpMotor ();
	}

	public void HelpAdjustingPiece() {
		currentState.HelpAdjustingPiece ();
	}

	public void GivePositiveFeedback() {
		nFailedTries = 0;
		nWrongAngleTries = 0;
		showedHardClue = false;
		currentPiece = null;
		previousState = null;
		currentState.GivePositiveFeedback ();
	}

	public void GiveNegativeFeedback() {
		currentState.GiveNegativeFeedback ();
		if(previousState != null)
			currentState = previousState;
		else currentState = PlayState;
	}

	public void StartedMoving (bool correctAngle){
		currentState.StartedMoving (correctAngle);
	}

	public void FirstIdlePrompt(){
		currentState.FirstIdlePrompt();
	}

	public void FirstAnglePrompt(){
		currentState.FirstAnglePrompt();
	}

	public void FirstPlacePrompt(){
		currentState.FirstPlacePrompt();
	}

	public void SecondAnglePrompt(){
		currentState.SecondAnglePrompt();
	}

	public void SecondPrompt(){
		currentState.SecondPrompt();
	}

	public void ThirdAnglePrompt(){
		currentState.ThirdAnglePrompt();
	}

	public void ThirdPrompt(){
		currentState.ThirdPrompt();
	}

    public void Quit()
    {
        GameManager.Instance.quit = true;
        GameState.Instance.playButtonInteractable = false;
        currentState = startState;
        BeginNextGame();
    }

	public void BeginGame(SolutionManager.Difficulty difficulty, string puzzle, SceneProperties.RotationMode rotationMode, float distanceThreshold) {
		currentGame = new GameSettings {
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
		thirdAnglePromptState = new ThirdAnglePromptState ();
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
			BeginNextGame ();
		else BeginFirstGame ();
	}

	public State CurrentState {
		get {
			return currentState;
		}

		set {
			currentState = value;
		}
	}

	public State GreetingState {
		get {
			return greetingState;
		}
	}

	public State InitialHelpState {
		get {
			return initialHelpState;
		}
	}

	public State FingerHelpState {
		get {
			return fingerHelpState;
		}
	}

	public State ButtonHelpState {
		get {
			return buttonHelpState;
		}
	}

	public State FitHelpState {
		get {
			return fitHelpState;
		}
	}

	public State MotorHelpState {
		get {
			return motorHelpState;
		}
	}

	public State PlayState {
		get {
			return playState;
		}
	}

	public State StartState {
		get {
			return startState;
		}
	}

	public State FinalState {
		get {
			return finalState;
		}
	}

	public State PositiveFeedState {
		get {
			return positiveFeedState;
		}
	}

	public State NegativeFeedState {
		get {
			return negativeFeedState;
		}
	}

	public State FirstIdlePromptState {
		get {
			return firstIdlePromptState;
		}
	}

	public State FirstAnglePromptState {
		get {
			return firstAnglePromptState;
		}
	}

	public State FirstPlacePromptState {
		get {
			return firstPlacePromptState;
		}
	}

	public State SecondAnglePromptState {
		get {
			return secondAnglePromptState;
		}
	}

	public State SecondPromptState {
		get {
			return secondPromptState;
		}
	}

	public State ThirdAnglePromptState {
		get {
			return thirdAnglePromptState;
		}
	}

	public State ThirdPromptState {
		get {
			return thirdPromptState;
		}
	}

}
