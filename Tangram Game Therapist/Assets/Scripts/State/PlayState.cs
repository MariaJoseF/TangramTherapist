using UnityEngine;
using System.Collections;
using System;

public class PlayState : State {

	public PlayState () {
	}
	
	public void Update(){
		if ((DateTime.Now - GameState.Instance.stopped).TotalSeconds > 15 && !GameState.Instance.dragging) {
			Debug.Log("PLAY -> 1st idle");
			FirstIdlePrompt ();
		}
		if (Therapist.Instance.nFailedTries >= 2) {
			Debug.Log(Therapist.Instance.nFailedTries + " tentativas erradas");
			FirstPlacePrompt();
		}
		if (Therapist.Instance.nWrongAngleTries >= 2) {
			Debug.Log(Therapist.Instance.nWrongAngleTries + " x angulo erradas");
			FirstAnglePrompt();
		}
	}

	public void StartedMoving (bool correctAngle){
		Debug.Log ("moveu " + correctAngle + " " + Therapist.Instance.currentPlace);

		if (!GameState.Instance.dragging && !correctAngle) {
			if (Therapist.Instance.previousState == Therapist.Instance.FirstAnglePromptState) {
				Debug.Log("PLAY -> 1stangle");
				FirstAnglePrompt ();
				Therapist.Instance.previousState = null;
			}
		}
	}

	public void BeginFirstGame(){
	}

	public void BeginNextGame(){
	}

	public void EndGame(){
		Therapist.Instance.currentState = Therapist.Instance.FinalState;
		Therapist.Instance.EndGame ();
	}

	public void HelpMotor(){
		Therapist.Instance.previousState = Therapist.Instance.currentState;
		Therapist.Instance.currentState = Therapist.Instance.MotorHelpState;

        Therapist.Instance.AlgorithmEXP31.RunExp3();
        Therapist.Instance.HelpMotor ();
	}
	
	public void HelpAdjustingPiece() {
		Therapist.Instance.previousState = Therapist.Instance.currentState;
		Therapist.Instance.currentState = Therapist.Instance.FitHelpState;

        Therapist.Instance.AlgorithmEXP31.RunExp3();
        Therapist.Instance.HelpAdjustingPiece ();
	}

	public void GivePositiveFeedback() {
		Therapist.Instance.currentState = Therapist.Instance.PositiveFeedState;

        Therapist.Instance.AlgorithmEXP31.RunExp3();
        Therapist.Instance.GivePositiveFeedback ();
	}
	
	public void GiveNegativeFeedback() {
		Therapist.Instance.previousState = Therapist.Instance.currentState;
		Therapist.Instance.currentState = Therapist.Instance.NegativeFeedState;

        Therapist.Instance.AlgorithmEXP31.RunExp3();
        Therapist.Instance.GiveNegativeFeedback ();
	}

	public void FirstIdlePrompt(){
		Therapist.Instance.currentState = Therapist.Instance.FirstIdlePromptState;

        Therapist.Instance.AlgorithmEXP31.RunExp3();
        Therapist.Instance.FirstIdlePrompt();
	}
	
	public void FirstAnglePrompt(){
		Therapist.Instance.currentState = Therapist.Instance.FirstAnglePromptState;

        Therapist.Instance.AlgorithmEXP31.RunExp3();
        Therapist.Instance.FirstAnglePrompt ();	
	}
	
	public void FirstPlacePrompt(){
		Therapist.Instance.currentState = Therapist.Instance.FirstPlacePromptState;

        Therapist.Instance.AlgorithmEXP31.RunExp3();
        Therapist.Instance.FirstPlacePrompt ();	
	}

	public void SecondPrompt(){
		Therapist.Instance.currentState = Therapist.Instance.SecondPromptState;

        Therapist.Instance.AlgorithmEXP31.RunExp3();
        Therapist.Instance.SecondPrompt ();	
	}
	
	public void ThirdPrompt(){
		Therapist.Instance.currentState = Therapist.Instance.ThirdPromptState;

        Therapist.Instance.AlgorithmEXP31.RunExp3();
        Therapist.Instance.ThirdPrompt ();	
	}

	public void SecondAnglePrompt(){
		Therapist.Instance.currentState = Therapist.Instance.SecondAnglePromptState;

        Therapist.Instance.AlgorithmEXP31.RunExp3();
        Therapist.Instance.SecondAnglePrompt ();	
	}
	
	public void ThirdAnglePrompt(){
		Therapist.Instance.currentState = Therapist.Instance.ThirdAnglePromptState;

        Therapist.Instance.AlgorithmEXP31.RunExp3();
        Therapist.Instance.ThirdAnglePrompt ();	
	}

    public void RepeatPrompt()//não existia acrescentei
    {
    }

}
