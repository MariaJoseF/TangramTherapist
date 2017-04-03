using UnityEngine;
using System.Collections;
using System;

public class NegativeFeedState : State {
	DateTime lastNegativeFeedTime;
    int nNegativeFeed;

	public NegativeFeedState () {
		lastNegativeFeedTime = DateTime.Now;
        nNegativeFeed = 0;
	}

	public void GiveNegativeFeedback() {
        nNegativeFeed++;
		if (nNegativeFeed >= 3 && (DateTime.Now - lastNegativeFeedTime).TotalSeconds > 15) {
            if(UtterancesManager.Instance.NegativeFeedback()){
                nNegativeFeed = 0;
			    lastNegativeFeedTime = DateTime.Now;
            }
        }
        else//ver se é preciso acrescentar alguma condiçºao para esta situação ou se assim funciona
        {
            UtterancesManager.Instance.NegativeFeedback();
        }
	}

	public void BeginFirstGame(){
	}

	public void BeginNextGame(){
	}

	public void EndGame(){
	}

    public void HelpMotor()
    {

        /////////

        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.MotorHelpState;
        Therapist.Instance.HelpMotor();

        /////////

    }
    public void HelpAdjustingPiece()
    {

        /////////

        Therapist.Instance.previousState = Therapist.Instance.currentState;
        Therapist.Instance.currentState = Therapist.Instance.FitHelpState;
        Therapist.Instance.HelpAdjustingPiece();

        /////////

    }
    public void GivePositiveFeedback()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.PositiveFeedState;
        Therapist.Instance.GivePositiveFeedback();

        /////////

    }

    public void StartedMoving (bool correctAngle){
	}

    public void FirstIdlePrompt()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.FirstIdlePromptState;
        Therapist.Instance.FirstIdlePrompt();

        /////////

    }

    public void FirstAnglePrompt()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.FirstAnglePromptState;
        Therapist.Instance.FirstAnglePrompt();

        /////////

    }

    public void FirstPlacePrompt()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.FirstPlacePromptState;
        Therapist.Instance.FirstPlacePrompt();

        /////////

    }
    public void SecondPrompt()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.SecondPromptState;
        Therapist.Instance.SecondPrompt();

        /////////

    }

    public void ThirdPrompt()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.ThirdPromptState;
        Therapist.Instance.ThirdPrompt();

        /////////

    }

    public void SecondAnglePrompt()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.SecondAnglePromptState;
        Therapist.Instance.SecondAnglePrompt();

        /////////

    }

    public void ThirdAnglePrompt()
    {

        /////////

        Therapist.Instance.currentState = Therapist.Instance.ThirdAnglePromptState;
        Therapist.Instance.ThirdAnglePrompt();

        /////////

    }
    public void Update(){		
	}

    public void RepeatPrompt()//não existia acrescentei
    {
    }
}
