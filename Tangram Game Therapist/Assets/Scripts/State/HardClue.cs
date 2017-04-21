using UnityEngine;
using System.Collections;
using System;


public class HardClueState : State
{
    public DateTime lastPromptTime, incorrectAngleTime, repeatPromptTime, goToSecondAnglePromptTime;
    public int nPrompts, nIncorrectAngle;


    public HardClueState()
    {
       
    }

    public void HardCluePrompt()
    {

        if ((DateTime.Now - lastPromptTime).TotalSeconds > 3)
        {
            lastPromptTime = DateTime.Now;
            UtterancesManager.Instance.HardClue(0.4f);
        }

        Therapist.Instance.currentState = null;

    }


    public void RepeatPrompt()
    {//não devia ser public

    }

    public void EndGame()
    {

    }

    public void HelpMotor()
    {

    }

    public void HelpAdjustingPiece()
    {
;
    }

    public void GivePositiveFeedback()
    {

    }

    public void SecondAnglePrompt()
    {

    }

    public void BeginFirstGame()
    {
    }

    public void BeginNextGame()
    {
    }

    public void FirstIdlePrompt()
    {

        //////////////////

        Therapist.Instance.currentState = Therapist.Instance.FirstIdlePromptState;
        Therapist.Instance.FirstIdlePrompt();

        /////////////
    }

    public void FirstAnglePrompt()
    {

        ////////////////

        Therapist.Instance.currentState = Therapist.Instance.FirstAnglePromptState;
        Therapist.Instance.FirstAnglePrompt();

        ////////////////
    }

    public void FirstPlacePrompt()
    {

        /////////////

        Therapist.Instance.currentState = Therapist.Instance.FirstPlacePromptState;
        Therapist.Instance.FirstPlacePrompt();

        /////////////
    }

    public void SecondPrompt()
    {
        ////////

        Therapist.Instance.currentState = Therapist.Instance.SecondPromptState;
        Therapist.Instance.SecondPrompt();

        ////////

    }

    public void ThirdAnglePrompt()
    {

        //////

        Therapist.Instance.currentState = Therapist.Instance.ThirdAnglePromptState;
        Therapist.Instance.ThirdAnglePrompt();

        //////

    }

    public void GiveNegativeFeedback()
    {
    }

    public void ThirdPrompt()
    {
    }

    public void Update()
    {
    }

    public void StartedMoving(bool correctAngle)
    {
    }
}
