using UnityEngine;
using System.Collections;
using System;

public class PositiveFeedState : State
{
    DateTime lastPositiveFeedTime;

    public PositiveFeedState()
    {
        lastPositiveFeedTime = DateTime.Now;
    }

    public void GivePositiveFeedback()
    {
        if ((DateTime.Now - lastPositiveFeedTime).TotalSeconds > 3)
        {
            lastPositiveFeedTime = DateTime.Now;

            UtterancesManager.Instance.CheckUtteranceFinish();
            /*NEW*/

            UtterancesManager.Instance.PositiveFeedback(StringNumberOfPieces(GameState.Instance.notPlacedPieces.Count), Therapist.Instance.NiceRobot);
            Therapist.Instance.ShowFormRatings();

            /*NEW*/

        }
        Therapist.Instance.currentState = Therapist.Instance.PlayState;
    }

    string StringNumberOfPieces(int nPieces)
    {
        string number;
        if (nPieces == 1)
            number = "uma peça";
        else if (nPieces == 2)
            number = "duas peças";
        else
            number = nPieces + " peças";
        return number;
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

    public void HelpAdjustingPiece()
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
