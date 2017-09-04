using System.Collections.Generic;

public interface ITPerceptions
{
    void Dispose();
    void Greeting();
    void GameStart(string puzzle);
    void NextGame();
    void FingerHelp();
    void ButtonHelp();
    void Win(string puzzle);
    void FastWin(string puzzle);
    void Quit();
    void MotorHelp();
    void CloseHelp();
    void PositiveFeedback(string nPieces);
    void NegativeFeedback();
    void FirstAnglePrompt(string piece);
    void FirstAnglePromptFinger(string piece);
    void FirstAnglePromptButton(string piece);
    void SecondAnglePrompt(string piece);
    void SecondAnglePromptFinger(string piece, string direction);
    void SecondAnglePromptButton(string piece, string nClicks);
    void ThirdAnglePrompt(string piece);
    void StopAnglePrompt(string piece);
    void FirstIdlePrompt(string piece);
    void FirstPlacePrompt(string piece);
    void SecondPrompt1Position(string piece, string pos);
    void SecondPrompt2Position(string piece, string pos1, string pos2);
    void SecondPromptPlace(string piece, string pos, string relativePiece);
    void ThirdPrompt(string piece);
    void HardClue();
    void PGreeting();
    void PButtonHelp();
    void PFingerHelp();
    void PWin(string puzzle);
    void PChildTurn();
    void PRobotTurn();
    void PRobotDrag();
    void PRobotRotDrag();
    void PRobotWin(string nPieces);
    void PRobotReminder();
    void PAskingPlace(string piece);
    void PAskingRotate(string piece);
    void PAskingPlaceWin();
    void PAskingRotateWin();
    void PAskingPlaceWrong(string piece);
    void PAskingRotateWrong(string piece);
    void PAskingQuit();
    void PGivingPlace(string piece, string pos);
    void PGivingRotate(string piece);
    void CancelUtterance(string id);

    void LibraryList(string[] libraries);
    void LibraryChanged(string serialized_LibraryContents);
    void Utterances(string library, string category, string subcategory, string[] utterances);
}


public interface ILibraryActions : IAction
{
    void ChangeLibrary(string newLibrary);
    void GetLibraries();
    void GetUtterances(string category, string subcategory);
}

