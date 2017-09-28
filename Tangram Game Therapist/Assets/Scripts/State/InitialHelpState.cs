using UnityEngine;
using System.Collections;
using System;

public class InitialHelpState : State
{

    public InitialHelpState()
    {
    }

    public void BeginFirstGame()
    {
        Debug.Log("1 initial help " + SolutionManager.Instance.puzzleNamept);

        UtterancesManager.Instance.CheckUtteranceFinish();

        if (Therapist.Instance.firstTimeFinger == 1)
        {
            /*NEW*/

            UtterancesManager.Instance.GameStart(SolutionManager.Instance.puzzleNamept, false, Therapist.Instance.NiceRobot);
            Therapist.Instance.ShowFormRatings();

            /*NEW*/

            Therapist.Instance.currentState = Therapist.Instance.FingerHelpState;
            Therapist.Instance.BeginFirstGame();
        }
        else if (Therapist.Instance.firstTimeButton == 1)
        {
            /*NEW*/

            UtterancesManager.Instance.GameStart(SolutionManager.Instance.puzzleNamept, false, Therapist.Instance.NiceRobot);
            Therapist.Instance.ShowFormRatings();

            /*NEW*/

            Therapist.Instance.currentState = Therapist.Instance.ButtonHelpState;
            Therapist.Instance.BeginFirstGame();
        }
        else
        {
            /*NEW*/

            UtterancesManager.Instance.GameStart(SolutionManager.Instance.puzzleNamept, true, Therapist.Instance.NiceRobot);
            Therapist.Instance.ShowFormRatings();

            /*NEW*/

            Therapist.Instance.currentState = Therapist.Instance.PlayState;
        }
    }

    public void BeginNextGame()
    {
        Debug.Log("2 initial help " + SolutionManager.Instance.puzzleNamept);

        UtterancesManager.Instance.CheckUtteranceFinish();

        if (Therapist.Instance.firstTimeFinger == 1)
        {
            /*NEW*/

            UtterancesManager.Instance.GameStart(SolutionManager.Instance.puzzleNamept, false, Therapist.Instance.NiceRobot);
            Therapist.Instance.ShowFormRatings();

            /*NEW*/

            Therapist.Instance.currentState = Therapist.Instance.FingerHelpState;
            Therapist.Instance.BeginNextGame();
        }
        else if (Therapist.Instance.firstTimeButton == 1)
        {
            /*NEW*/

            UtterancesManager.Instance.GameStart(SolutionManager.Instance.puzzleNamept, false, Therapist.Instance.NiceRobot);
            Therapist.Instance.ShowFormRatings();

            /*NEW*/

            Therapist.Instance.currentState = Therapist.Instance.ButtonHelpState;
            Therapist.Instance.BeginNextGame();
        }
        else
        {
            /*NEW*/

            UtterancesManager.Instance.GameStart(SolutionManager.Instance.puzzleNamept, true, Therapist.Instance.NiceRobot);
            Therapist.Instance.ShowFormRatings();

            /*NEW*/

            Therapist.Instance.currentState = Therapist.Instance.PlayState;
        }
    }

    public void EndGame()
    {
    }

    public void HelpMotor()
    {
    }

    public void HelpAdjustingPiece()
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
    }

    public void FirstAnglePrompt()
    {
    }

    public void FirstPlacePrompt()
    {
    }

    public void SecondPrompt()
    {
    }

    public void ThirdPrompt()
    {
    }

    public void SecondAnglePrompt()
    {
    }

    public void ThirdAnglePrompt()
    {
    }

    public void Update()
    {
    }
}
