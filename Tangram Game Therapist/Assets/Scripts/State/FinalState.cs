using UnityEngine;
using System.Collections;
using System;

public class FinalState : State
{
    DateTime endGameTime;

    public FinalState()
    {
    }

    public void EndGame()
    {
        endGameTime = DateTime.Now;

        UtterancesManager.Instance.CheckUtteranceFinish();

        if (!GameState.Instance.quit)
        {
            if ((float)(DateTime.Now - GameManager.Instance.beginGameTime).TotalSeconds < 80)
                UtterancesManager.Instance.FastWin(SolutionManager.Instance.puzzleNamept, Therapist.Instance.NiceRobot);
            else
                UtterancesManager.Instance.Win(SolutionManager.Instance.puzzleNamept, Therapist.Instance.NiceRobot);

            Therapist.Instance.ShowFormRatings();

            GameState.Instance.playButtonInteractable = false;
        }
        else
        {
            UtterancesManager.Instance.WriteJSON("QUIT after " + (float)(DateTime.Now - GameManager.Instance.beginGameTime).TotalSeconds + " seconds");
            
            /*NEW*/

            if (Therapist.Instance.ratingsFeedback.feedback_val == -2.0f)
            {
                Therapist.Instance.ratingsFeedback.FileHeader();
                Therapist.Instance.ratingsFeedback.WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), ";" + GameManager.Instance.playerName + ";" + GameManager.Instance.CurrentPuzzle + ";" + GameManager.Instance.Difficulty_ + ";" + GameManager.Instance.RotationMode_ + ";" + GameManager.Instance.DistanceThreshold + ";" + Therapist.Instance.AlgorithmEXP3.Action + ";" + "-");
            }

            /*NEW*/

            GameState.Instance.playButtonInteractable = true;
            Therapist.Instance.currentState = Therapist.Instance.StartState;
            Application.LoadLevel(1);
            Therapist.Instance.BeginNextGame();
        }
    }

    public void Update()
    {
        if ((DateTime.Now - endGameTime).TotalSeconds > 8)
        {
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

}
