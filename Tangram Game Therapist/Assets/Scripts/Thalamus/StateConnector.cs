using UnityEngine;
using System;
using System.Collections.Generic;

public class StateConnector
{
    private readonly ThalamusConnector _thalamusConnector;

    public StateConnector(UtterancesManager m)
    {
        _thalamusConnector = new ThalamusConnector(m);
    }


    /*public void GlanceAtScreen(double x, double y)
    {
        _thalamusConnector.GlanceAtScreen(x, y);
    }*/

    public void Dispose()
    {
        _thalamusConnector.Dispose();
    }

    public void Greeting(bool robotN)
    {
        _thalamusConnector.Greeting(GameManager.Instance.playerName, robotN);
    }

    public void GameStart(string puzzle, bool robotN)
    {
        _thalamusConnector.GameStart(puzzle, robotN);
    }

    public void NextGame(bool robotN)
    {
        _thalamusConnector.NextGame(GameManager.Instance.playerName, robotN);
    }

    public void FingerHelp(bool robotN)
    {
        _thalamusConnector.FingerHelp(robotN);
    }

    public void ButtonHelp(bool robotN)
    {
        _thalamusConnector.ButtonHelp(robotN);
    }

    public void Win(string puzzle, bool robotN)
    {
        _thalamusConnector.Win(puzzle, GameManager.Instance.playerName, robotN);
    }

    public void FastWin(string puzzle, bool robotN)
    {
        _thalamusConnector.FastWin(puzzle, GameManager.Instance.playerName, robotN);
    }

    //internal void GetUtterances(string category, string subcategory)
    //{
    //    throw new NotImplementedException();
    //}

    //internal void LibraryList(string[] libraries)
    //{
    //    throw new NotImplementedException();
    //}

    //internal void LibraryChanged(string serialized_LibraryContents)
    //{
    //    throw new NotImplementedException();
    //}

    //internal void ChangeLibrary(string newLibrary)
    //{
    //    throw new NotImplementedException();
    //}

    //internal void GetLibraries()
    //{
    //    throw new NotImplementedException();
    //}

    public void Quit(bool robotN)
    {
        _thalamusConnector.Quit(GameManager.Instance.playerName, robotN);
    }

    //internal void Utterances(string library, string category, string subcategory, string[] utterances)
    //{
    //    throw new NotImplementedException();
    //}

    public void MotorHelp(bool robotN)
    {
        _thalamusConnector.MotorHelp(robotN);
    }

    public void CloseHelp(bool robotN)
    {
        _thalamusConnector.CloseHelp(robotN);
    }

    public void PositiveFeedback(string nPieces, bool robotN)
    {
        _thalamusConnector.PositiveFeedback(nPieces, GameManager.Instance.playerName, robotN);
    }

    public void NegativeFeedback(bool robotN)
    {
        _thalamusConnector.NegativeFeedback(robotN);
    }

    public void FirstAnglePrompt(string piece, bool robotN)
    {
        _thalamusConnector.FirstAnglePrompt(piece, GameManager.Instance.playerName, robotN);
    }

    public void FirstAnglePromptFinger(string piece, bool robotN)
    {
        _thalamusConnector.FirstAnglePromptFinger(piece, GameManager.Instance.playerName, robotN);
    }

    public void FirstAnglePromptButton(string piece, bool robotN)
    {
        _thalamusConnector.FirstAnglePromptButton(piece, GameManager.Instance.playerName, robotN);
    }

    public void SecondAnglePrompt(string piece, bool robotN)
    {
        _thalamusConnector.SecondAnglePrompt(piece, GameManager.Instance.playerName, robotN);
    }

    public void SecondAnglePromptFinger(string piece, string direction, bool robotN)
    {
        _thalamusConnector.SecondAnglePromptFinger(piece, direction, GameManager.Instance.playerName, robotN);
    }

    public void SecondAnglePromptButton(string piece, string nClicks, bool robotN)
    {
        _thalamusConnector.SecondAnglePromptButton(piece, nClicks, GameManager.Instance.playerName, robotN);
    }

    public void ThirdAnglePrompt(string piece, bool robotN)
    {
        _thalamusConnector.ThirdAnglePrompt(piece, GameManager.Instance.playerName, robotN);
    }

    public void StopAnglePrompt(string piece, bool robotN)
    {
        _thalamusConnector.StopAnglePrompt(piece, robotN);
    }

    public void FirstIdlePrompt(string piece, bool robotN)
    {
        _thalamusConnector.FirstIdlePrompt(piece, GameManager.Instance.playerName, robotN);
    }

    public void FirstPlacePrompt(string piece, bool robotN)
    {
        _thalamusConnector.FirstPlacePrompt(piece, GameManager.Instance.playerName, robotN);
    }

    public void SecondPrompt1Position(string piece, string pos, bool robotN)
    {
        _thalamusConnector.SecondPrompt1Position(piece, pos, GameManager.Instance.playerName, robotN);
    }

    public void SecondPrompt2Position(string piece, string pos1, string pos2, bool robotN)
    {
        _thalamusConnector.SecondPrompt2Position(piece, pos1, pos2, GameManager.Instance.playerName, robotN);
    }

    public void SecondPromptPlace(string piece, string pos, string relativePiece, bool robotN)
    {
        _thalamusConnector.SecondPromptPlace(piece, pos, relativePiece, GameManager.Instance.playerName, robotN);
    }

    public void ThirdPrompt(string piece, bool robotN)
    {
        _thalamusConnector.ThirdPrompt(piece, GameManager.Instance.playerName, robotN);
    }

    public void HardClue(bool robotN)
    {
        _thalamusConnector.HardClue(GameManager.Instance.playerName, robotN);
    }

    public void PGreeting(bool robotN)
    {
        _thalamusConnector.PGreeting(GameManager.Instance.playerName, robotN);
    }

    public void PButtonHelp(bool robotN)
    {
        _thalamusConnector.PButtonHelp(robotN);
    }

    public void PFingerHelp(bool robotN)
    {
        _thalamusConnector.PFingerHelp(robotN);
    }

    public void PWin(string puzzle, bool robotN)
    {
        _thalamusConnector.PWin(puzzle, GameManager.Instance.playerName, robotN);
    }

    public void PChildTurn(bool robotN)
    {
        _thalamusConnector.PChildTurn(GameManager.Instance.playerName, robotN);
    }

    public void PRobotTurn(bool robotN)
    {
        _thalamusConnector.PRobotTurn(robotN);
    }

    public void PRobotDrag(bool robotN)
    {
        _thalamusConnector.PRobotDrag(robotN);
    }

    public void PRobotRotDrag(bool robotN)
    {
        _thalamusConnector.PRobotRotDrag(robotN);
    }

    public void PRobotWin(string nPieces, bool robotN)
    {
        _thalamusConnector.PRobotWin(nPieces, GameManager.Instance.playerName, robotN);
    }

    public void PRobotReminder(bool robotN)
    {
        _thalamusConnector.PRobotReminder(robotN);
    }

    public void PAskingPlace(string piece, bool robotN)
    {
        _thalamusConnector.PAskingPlace(piece, GameManager.Instance.playerName, robotN);
    }

    public void PAskingRotate(string piece, bool robotN)
    {
        _thalamusConnector.PAskingRotate(piece, GameManager.Instance.playerName, robotN);
    }

    public void PAskingPlaceWin(bool robotN)
    {
        _thalamusConnector.PAskingPlaceWin(GameManager.Instance.playerName, robotN);
    }

    public void PAskingRotateWin(bool robotN)
    {
        _thalamusConnector.PAskingRotateWin(GameManager.Instance.playerName, robotN);
    }

    public void PAskingPlaceWrong(string piece, bool robotN)
    {
        _thalamusConnector.PAskingPlaceWrong(piece, robotN);
    }

    public void PAskingRotateWrong(string piece, bool robotN)
    {
        _thalamusConnector.PAskingRotateWrong(piece, robotN);
    }

    public void PAskingQuit(bool robotN)
    {
        _thalamusConnector.PAskingQuit(robotN);
    }

    public void PGivingPlace(string piece, string pos, bool robotN)
    {
        _thalamusConnector.PGivingPlace(piece, pos, robotN);
    }

    public void PGivingRotate(string piece, bool robotN)
    {
        _thalamusConnector.PGivingRotate(piece, robotN);
    }

    public void WriteJSON(string timestamp, string info)
    {
        _thalamusConnector.WriteJSON(timestamp, info);
    }

    public void CancelUtterance(string id)
    {
        _thalamusConnector.CancelUtterance(id);
    }


}
