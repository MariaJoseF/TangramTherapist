using UnityEngine;
using System.Collections;
using System;

public class StartState : State {

	public StartState () {
	}

	public void BeginFirstGame(){
	}

	public void BeginNextGame(){
        if(!GameState.Instance.quit)
        {
            Debug.Log("QUERES JOGAR UM NOVO JOGO??");
            UtterancesManager.Instance.NextGame();
        }

        GameState.Instance.quit = false;
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
