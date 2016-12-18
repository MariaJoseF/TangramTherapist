using UnityEngine;
using System.Collections;
using System;

public class FinalState : State {
    DateTime endGameTime;

	public FinalState () {
	}

	public void EndGame(){
        endGameTime = DateTime.Now;

        if (!GameState.Instance.quit)
        {
            if((float)(DateTime.Now - GameManager.Instance.beginGameTime).TotalSeconds < 80)
                UtterancesManager.Instance.FastWin(SolutionManager.Instance.puzzleNamept);
            else
                UtterancesManager.Instance.Win(SolutionManager.Instance.puzzleNamept);
            GameState.Instance.playButtonInteractable = false;
        }
        else
        {
            UtterancesManager.Instance.WriteJSON("QUIT after " + (float)(DateTime.Now - GameManager.Instance.beginGameTime).TotalSeconds + " seconds");
            GameState.Instance.playButtonInteractable = true;
            Therapist.Instance.currentState = Therapist.Instance.StartState;
            Application.LoadLevel(1);
            Therapist.Instance.BeginNextGame();
        }
	}

    public void Update()
    {
        if ((DateTime.Now - endGameTime).TotalSeconds > 8) {
            Application.LoadLevel(1);
            Therapist.Instance.currentState = Therapist.Instance.StartState;
            Therapist.Instance.BeginNextGame();
        }
    }

    public void BeginFirstGame()
    {
    }

    public void BeginNextGame()
    {
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

}
