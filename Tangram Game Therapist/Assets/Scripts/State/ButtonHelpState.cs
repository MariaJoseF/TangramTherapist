using UnityEngine;
using System.Collections;
using System;

public class ButtonHelpState : State {

	public ButtonHelpState () {
	}
	
	public void BeginFirstGame(){
        UtterancesManager.Instance.ButtonHelp();
        Debug.Log("1 ButtonHelp");

		Therapist.Instance.firstTimeButton = 2;
		Therapist.Instance.currentState = Therapist.Instance.PlayState;
	}

	public void BeginNextGame(){
        UtterancesManager.Instance.ButtonHelp();
        Debug.Log("2 ButtonHelp");

        Therapist.Instance.firstTimeButton = 2;
		Therapist.Instance.currentState = Therapist.Instance.PlayState;
	}

	public void EndGame(){
	}

	public void HelpMotor(){
	}
	
	public void HelpAdjustingPiece() {
	}

	public void GivePositiveFeedback() {
	}
	
	public void GiveNegativeFeedback() {
	}

	public void StartedMoving (bool correctAngle){
	}

	public void FirstIdlePrompt(){
	}
	
	public void FirstAnglePrompt(){	
	}
	
	public void FirstPlacePrompt(){
	}

	public void SecondPrompt(){
	}
	
	public void ThirdPrompt(){	
	}

	public void SecondAnglePrompt(){	
	}
	
	public void ThirdAnglePrompt(){
	}

	public void Update(){		
	}

    public void HardCluePrompt()
    {
    }
}
