using UnityEngine;
using System.Collections;
using System;

public class ThirdPromptState : State {

    public DateTime lastPromptTime, incorrectAngleTime, repeatPromptTime, goToSecondAnglePromptTime;
	public int nPrompts, nIncorrectAngle;
    bool finalPrompt, repeatPrompt = false, goToSecondAnglePrompt = false;
    Piece currentPiece;

	public ThirdPromptState () {
		nPrompts = 0;
		nIncorrectAngle = 0;
	}

	public void ThirdPrompt(){
		lastPromptTime = DateTime.Now;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;
        nPrompts = 0;
		if (nPrompts == 0) {
            if (!UtterancesManager.Instance.ThirdPrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name))){
                repeatPrompt = true;
                repeatPromptTime = DateTime.Now;
                nPrompts = 0;
            }
            else
            {
                repeatPrompt = false;
                nPrompts = 1;
            }
		}   
	}

	public void RepeatPrompt(){//não devia ser public
        lastPromptTime = DateTime.Now;
        Therapist.Instance.nFailedTries = 0;
        Therapist.Instance.nWrongAngleTries = 0;

        if (!UtterancesManager.Instance.ThirdPrompt(GameState.Instance.PieceInformation(Therapist.Instance.currentPiece.name))){
            repeatPrompt = true;
            repeatPromptTime = DateTime.Now;
        }
        else
        {
            repeatPrompt = false;
            nPrompts++;
        }
	}

    void InitializeParameters(){
        PieceSolution currentPlace = GameState.Instance.FindTheCorrectPlace(Therapist.Instance.currentPiece);
        Therapist.Instance.currentPlace = currentPlace;

        if (currentPlace == null)
        {
            goToSecondAnglePrompt = true;
            goToSecondAnglePromptTime = DateTime.Now;
        }
    }
	public void StartedMoving (bool correctAngle){
		//lastPromptTime = DateTime.Now;
        if (finalPrompt){
            finalPrompt = false;
            nPrompts = 1;
        }
		if (!GameState.Instance.dragging) {
			if (!correctAngle) {
				nIncorrectAngle++;
				incorrectAngleTime = DateTime.Now;
			} else {
				nIncorrectAngle = 0;
				Therapist.Instance.nWrongAngleTries = 0;
			}
		}
	}
	
	public void Update(){
		if (nPrompts > 0) {
			if (Therapist.Instance.nWrongAngleTries >= 2 || (nIncorrectAngle > 0 && (DateTime.Now - incorrectAngleTime).TotalSeconds > 12)
                || (goToSecondAnglePrompt && (DateTime.Now - goToSecondAnglePromptTime).TotalSeconds > 5)) {
				SecondAnglePrompt ();	
				Debug.Log (Therapist.Instance.nWrongAngleTries + " Third -> 2Angle " + Therapist.Instance.currentPiece + " " + nIncorrectAngle);
                return;
            }
            else if ((repeatPrompt && (DateTime.Now - repeatPromptTime).TotalSeconds > 4) 
                || (DateTime.Now - lastPromptTime).TotalSeconds > 25 || Therapist.Instance.nFailedTries >= 2)
            {
				if (repeatPrompt || nPrompts < 3) {
					RepeatPrompt();
                    return;
				}
				else if (nPrompts >= 3){
                    UtterancesManager.Instance.Quit();
                    nPrompts = 0;
					finalPrompt = true;
					lastPromptTime = DateTime.Now;
				}
			}
		}
        else if ((repeatPrompt && (DateTime.Now - repeatPromptTime).TotalSeconds > 4)) {
            ThirdPrompt();
        }
        else if (finalPrompt && (DateTime.Now - lastPromptTime).TotalSeconds > 20)
        {
            lastPromptTime = DateTime.Now;
            GameState.Instance.quit = true;
            GameManager.Instance.quit = true;
            EndGame();
            return;
        }
        if (Therapist.Instance.currentPiece != currentPiece)
        {
            currentPiece = Therapist.Instance.currentPiece;
            InitializeParameters();
        }
	}

	public void GiveNegativeFeedback() {
		Therapist.Instance.previousState = Therapist.Instance.ThirdPromptState;
		Therapist.Instance.currentState = Therapist.Instance.NegativeFeedState;
		Debug.Log("3promp state-> neg feed");
		Therapist.Instance.GiveNegativeFeedback ();
	}

	public void EndGame(){
        repeatPrompt = false;
		nPrompts = 0;
		Therapist.Instance.nFailedTries = 0;
		Therapist.Instance.nWrongAngleTries = 0;
        Debug.Log("3rd finalPrompt -> quit");
        Therapist.Instance.currentState = Therapist.Instance.FinalState;
		Therapist.Instance.EndGame ();
	}

	public void HelpMotor(){
        repeatPrompt = false;
		Therapist.Instance.nFailedTries = 0;
		lastPromptTime = DateTime.Now;
		Therapist.Instance.previousState = Therapist.Instance.currentState;
		Therapist.Instance.currentState = Therapist.Instance.MotorHelpState;
		Therapist.Instance.HelpMotor ();
	}
	
	public void HelpAdjustingPiece() {
        repeatPrompt = false;
		Therapist.Instance.nFailedTries = 0;
		lastPromptTime = DateTime.Now;
		Therapist.Instance.previousState = Therapist.Instance.currentState;
		Therapist.Instance.currentState = Therapist.Instance.FitHelpState;
		Therapist.Instance.HelpAdjustingPiece ();
	}

	public void GivePositiveFeedback() {
        repeatPrompt = false;
		nPrompts = 0;
		Therapist.Instance.currentState = Therapist.Instance.PositiveFeedState;
		Therapist.Instance.GivePositiveFeedback ();
	}
	
	public void SecondAnglePrompt(){
        repeatPrompt = false;
		nPrompts = 0;
		Therapist.Instance.nFailedTries = 0;
		Therapist.Instance.nWrongAngleTries = 0;
		Therapist.Instance.currentState = Therapist.Instance.SecondAnglePromptState;
		Therapist.Instance.SecondAnglePrompt ();	
	}
	
	public void BeginFirstGame(){
	}
	
	public void BeginNextGame(){
	}

	public void FirstIdlePrompt(){
	}
	
	public void FirstAnglePrompt(){
	}
	
	public void FirstPlacePrompt(){
	}

	public void SecondPrompt(){
	}

	public void ThirdAnglePrompt(){
	}
}
