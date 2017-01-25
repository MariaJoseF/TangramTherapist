using UnityEngine;
using System.Collections;
using System;

public class GreetingState : State {

	public GreetingState () {
	}
	
	public void BeginFirstGame(){
        UtterancesManager.Instance.Greeting();
	}

	public void BeginNextGame(){
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

    public void RepeatPrompt()//não existia acrescentei
    {
    }
}
