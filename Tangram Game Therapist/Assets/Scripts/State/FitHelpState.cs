using UnityEngine;
using System.Collections;
using System;

public class FitHelpState : State
{
    int nHelpRequests;
    DateTime lastHelpTime;

    public FitHelpState()
    {
        nHelpRequests = 0;
    }

    public void HelpAdjustingPiece()
    {
        nHelpRequests++;

        if (nHelpRequests > 5 || nHelpRequests == 0 || (DateTime.Now - lastHelpTime).TotalSeconds > 100)
        {

            --its needed to put the rude robot here also
            UtterancesManager.Instance.CloseHelp();

            lastHelpTime = DateTime.Now;
            nHelpRequests = 0;
        }
        Therapist.Instance.currentState = Therapist.Instance.previousState;
        Therapist.Instance.previousState = null;
    }

    public void BeginFirstGame()
    {
    }

    public void BeginNextGame()
    {
    }

    public void EndGame()
    {
    }

    public void HelpMotor()
    {
    }

    public void GivePositiveFeedback()
    {
    }

    public void GiveNegativeFeedback()
    {
    }

    public void StartedMoving(bool correctAngle)
    {
    }

    public void FirstIdlePrompt()
    {
        /*Therapist.Instance.currentState = Therapist.Instance.FirstIdlePromptState;
		Therapist.Instance.FirstIdlePrompt();*/
    }

    public void FirstAnglePrompt()
    {
        /*Therapist.Instance.currentState = Therapist.Instance.FirstAnglePromptState;
		Therapist.Instance.FirstAnglePrompt ();	*/
    }

    public void FirstPlacePrompt()
    {
        /*Therapist.Instance.currentState = Therapist.Instance.FirstPlacePromptState;
		Therapist.Instance.FirstPlacePrompt ();	*/
    }

    public void SecondPrompt()
    {
        /*Therapist.Instance.currentState = Therapist.Instance.SecondPromptState;
		Therapist.Instance.SecondPrompt ();	*/
    }

    public void ThirdPrompt()
    {
        /*Therapist.Instance.currentState = Therapist.Instance.ThirdPromptState;
		Therapist.Instance.ThirdPrompt ();	*/
    }

    public void SecondAnglePrompt()
    {
        /*Therapist.Instance.currentState = Therapist.Instance.SecondAnglePromptState;
		Therapist.Instance.SecondAnglePrompt ();	*/
    }

    public void ThirdAnglePrompt()
    {
        /*Therapist.Instance.currentState = Therapist.Instance.ThirdAnglePromptState;
		Therapist.Instance.ThirdAnglePrompt ();	*/
    }

    public void Update()
    {
    }
}
