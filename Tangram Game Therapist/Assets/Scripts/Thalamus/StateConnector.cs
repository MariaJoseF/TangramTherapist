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

    public void Greeting()
    {
        _thalamusConnector.Greeting(GameManager.Instance.playerName);
    }

    public void GameStart(string puzzle)
    {
        _thalamusConnector.GameStart(puzzle);
    }

    public void NextGame()
    {
        _thalamusConnector.NextGame(GameManager.Instance.playerName);
    }

    public void FingerHelp()
    {
        _thalamusConnector.FingerHelp();
    }

    public void ButtonHelp()
    {
        _thalamusConnector.ButtonHelp();
    }

    public void Win(string puzzle)
    {
        _thalamusConnector.Win(puzzle, GameManager.Instance.playerName);
    }

    public void FastWin(string puzzle)
    {
        _thalamusConnector.FastWin(puzzle, GameManager.Instance.playerName);
    }

    internal void GetUtterances(string category, string subcategory)
    {
        throw new NotImplementedException();
    }

    internal void LibraryList(string[] libraries)
    {
        throw new NotImplementedException();
    }

    internal void LibraryChanged(string serialized_LibraryContents)
    {
        throw new NotImplementedException();
    }

    internal void ChangeLibrary(string newLibrary)
    {
        throw new NotImplementedException();
    }

    internal void GetLibraries()
    {
        throw new NotImplementedException();
    }

    public void Quit()
    {
        _thalamusConnector.Quit(GameManager.Instance.playerName);
    }

    internal void Utterances(string library, string category, string subcategory, string[] utterances)
    {
        throw new NotImplementedException();
    }

    public void MotorHelp()
    {
        _thalamusConnector.MotorHelp();
    }

    public void CloseHelp()
    {
        _thalamusConnector.CloseHelp();
    }

    public void PositiveFeedback(string nPieces)
    {
        _thalamusConnector.PositiveFeedback(nPieces, GameManager.Instance.playerName);
    }

    public void NegativeFeedback()
    {
        _thalamusConnector.NegativeFeedback();
    }

    public void FirstAnglePrompt(string piece)
    {
        _thalamusConnector.FirstAnglePrompt(piece, GameManager.Instance.playerName);
    }

    public void FirstAnglePromptFinger(string piece)
    {
        _thalamusConnector.FirstAnglePromptFinger(piece, GameManager.Instance.playerName);
    }

    public void FirstAnglePromptButton(string piece)
    {
        _thalamusConnector.FirstAnglePromptButton(piece, GameManager.Instance.playerName);
    }

    public void SecondAnglePrompt(string piece)
    {
        _thalamusConnector.SecondAnglePrompt(piece, GameManager.Instance.playerName);
    }

    public void SecondAnglePromptFinger(string piece, string direction)
    {
        _thalamusConnector.SecondAnglePromptFinger(piece, direction, GameManager.Instance.playerName);
    }

    public void SecondAnglePromptButton(string piece, string nClicks)
    {
        _thalamusConnector.SecondAnglePromptButton(piece, nClicks, GameManager.Instance.playerName);
    }

    public void ThirdAnglePrompt(string piece)
    {
        _thalamusConnector.ThirdAnglePrompt(piece, GameManager.Instance.playerName);
    }

    public void StopAnglePrompt(string piece)
    {
        _thalamusConnector.StopAnglePrompt(piece);
    }

    public void FirstIdlePrompt(string piece)
    {
        _thalamusConnector.FirstIdlePrompt(piece, GameManager.Instance.playerName);
    }

    public void FirstPlacePrompt(string piece)
    {
        _thalamusConnector.FirstPlacePrompt(piece, GameManager.Instance.playerName);
    }

    public void SecondPrompt1Position(string piece, string pos)
    {
        _thalamusConnector.SecondPrompt1Position(piece, pos, GameManager.Instance.playerName);
    }

    public void SecondPrompt2Position(string piece, string pos1, string pos2)
    {
        _thalamusConnector.SecondPrompt2Position(piece, pos1, pos2, GameManager.Instance.playerName);
    }

    public void SecondPromptPlace(string piece, string pos, string relativePiece)
    {
        _thalamusConnector.SecondPromptPlace(piece, pos, relativePiece, GameManager.Instance.playerName);
    }

    public void ThirdPrompt(string piece)
    {
        _thalamusConnector.ThirdPrompt(piece, GameManager.Instance.playerName);
    }

    public void HardClue()
    {
        _thalamusConnector.HardClue(GameManager.Instance.playerName);
    }

    public void PGreeting()
    {
        _thalamusConnector.PGreeting(GameManager.Instance.playerName);
    }

    public void PButtonHelp()
    {
        _thalamusConnector.PButtonHelp();
    }

    public void PFingerHelp()
    {
        _thalamusConnector.PFingerHelp();
    }

    public void PWin(string puzzle)
    {
        _thalamusConnector.PWin(puzzle, GameManager.Instance.playerName);
    }

    public void PChildTurn()
    {
        _thalamusConnector.PChildTurn(GameManager.Instance.playerName);
    }

    public void PRobotTurn()
    {
        _thalamusConnector.PRobotTurn();
    }

    public void PRobotDrag()
    {
        _thalamusConnector.PRobotDrag();
    }

    public void PRobotRotDrag()
    {
        _thalamusConnector.PRobotRotDrag();
    }

    public void PRobotWin(string nPieces)
    {
        _thalamusConnector.PRobotWin(nPieces, GameManager.Instance.playerName);
    }

    public void PRobotReminder()
    {
        _thalamusConnector.PRobotReminder();
    }

    public void PAskingPlace(string piece)
    {
        _thalamusConnector.PAskingPlace(piece, GameManager.Instance.playerName);
    }

    public void PAskingRotate(string piece)
    {
        _thalamusConnector.PAskingRotate(piece, GameManager.Instance.playerName);
    }

    public void PAskingPlaceWin()
    {
        _thalamusConnector.PAskingPlaceWin(GameManager.Instance.playerName);
    }

    public void PAskingRotateWin()
    {
        _thalamusConnector.PAskingRotateWin(GameManager.Instance.playerName);
    }

    public void PAskingPlaceWrong(string piece)
    {
        _thalamusConnector.PAskingPlaceWrong(piece);
    }

    public void PAskingRotateWrong(string piece)
    {
        _thalamusConnector.PAskingRotateWrong(piece);
    }

    public void PAskingQuit()
    {
        _thalamusConnector.PAskingQuit();
    }

    public void PGivingPlace(string piece, string pos)
    {
        _thalamusConnector.PGivingPlace(piece, pos);
    }

    public void PGivingRotate(string piece)
    {
        _thalamusConnector.PGivingRotate(piece);
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
