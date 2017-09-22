using CookComputing.XmlRpc;

public interface ITMessagesRpc : ITMessages, IXmlRpcProxy
{
    //void LibraryChanged(string serialized_LibraryContents);
    void Utterances(string library, string category, string subcategory, string[] utterances);
    //void LibraryList(string[] libraries);
}

public interface ITMessages
{
    void Dispose();

    [XmlRpcMethod]
    void Greeting(string player, bool robotN);

    [XmlRpcMethod]
    void GameStart(string puzzle, bool robotN);

    [XmlRpcMethod]
    void NextGame(string player, bool robotN);

    [XmlRpcMethod]
    void FingerHelp(bool robotN);

    [XmlRpcMethod]
    void ButtonHelp(bool robotN);

    [XmlRpcMethod]
    void Win(string puzzle, string player, bool robotN);

    [XmlRpcMethod]
    void FastWin(string puzzle, string player, bool robotN);

    [XmlRpcMethod]
    void Quit(string player, bool robotN);

    [XmlRpcMethod]
    void MotorHelp(bool robotN);

    [XmlRpcMethod]
    void CloseHelp(bool robotN);

    [XmlRpcMethod]
    void PositiveFeedback(string nPieces, string player, bool robotN);

    [XmlRpcMethod]
    void NegativeFeedback(bool robotN);

    [XmlRpcMethod]
    void FirstAnglePrompt(string piece, string player, bool robotN);

    [XmlRpcMethod]
    void FirstAnglePromptFinger(string piece, string player, bool robotN);

    [XmlRpcMethod]
    void FirstAnglePromptButton(string piece, string player, bool robotN);

    [XmlRpcMethod]
    void SecondAnglePrompt(string piece, string player, bool robotN);

    [XmlRpcMethod]
    void SecondAnglePromptFinger(string piece, string direction, string player, bool robotN);

    [XmlRpcMethod]
    void SecondAnglePromptButton(string piece, string nClicks, string player, bool robotN);

    [XmlRpcMethod]
    void ThirdAnglePrompt(string piece, string player, bool robotN);

    [XmlRpcMethod]
    void StopAnglePrompt(string piece, bool robotN);

    [XmlRpcMethod]
    void FirstIdlePrompt(string piece, string player, bool robotN);

    [XmlRpcMethod]
    void FirstPlacePrompt(string piece, string player, bool robotN);

    [XmlRpcMethod]
    void SecondPrompt1Position(string piece, string pos, string player, bool robotN);

    [XmlRpcMethod]
    void SecondPrompt2Position(string piece, string pos1, string pos2, string player, bool robotN);

    [XmlRpcMethod]
    void SecondPromptPlace(string piece, string pos, string relativePiece, string player, bool robotN);

    [XmlRpcMethod]
    void ThirdPrompt(string piece, string player, bool robotN);

    [XmlRpcMethod]
    void HardClue(string player, bool robotN);

    [XmlRpcMethod]
    void PGreeting(string player, bool robotN);

    [XmlRpcMethod]
    void PButtonHelp(bool robotN);

    [XmlRpcMethod]
    void PFingerHelp(bool robotN);

    [XmlRpcMethod]
    void PWin(string puzzle, string player, bool robotN);

    [XmlRpcMethod]
    void PChildTurn(string player, bool robotN);

    [XmlRpcMethod]
    void PRobotTurn(bool robotN);

    [XmlRpcMethod]
    void PRobotDrag(bool robotN);

    [XmlRpcMethod]
    void PRobotRotDrag(bool robotN);

    [XmlRpcMethod]
    void PRobotWin(string nPieces, string player, bool robotN);

    [XmlRpcMethod]
    void PRobotReminder(bool robotN);

    [XmlRpcMethod]
    void PAskingPlace(string piece, string player, bool robotN);

    [XmlRpcMethod]
    void PAskingRotate(string piece, string player, bool robotN);

    [XmlRpcMethod]
    void PAskingPlaceWin(string player, bool robotN);

    [XmlRpcMethod]
    void PAskingRotateWin(string player, bool robotN);

    [XmlRpcMethod]
    void PAskingPlaceWrong(string piece, bool robotN);

    [XmlRpcMethod]
    void PAskingRotateWrong(string piece, bool robotN);

    [XmlRpcMethod]
    void PAskingQuit(bool robotN);

    [XmlRpcMethod]
    void PGivingPlace(string piece, string pos, bool robotN);

    [XmlRpcMethod]
    void PGivingRotate(string piece, bool robotN);

    [XmlRpcMethod]
    void WriteJSON(string timestamp, string info);

    [XmlRpcMethod]
    void CancelUtterance(string id);
}



///* NEW */

//public interface ILibraryActionsRPC
//{
//    [XmlRpcMethod]
//    void ChangeLibrary(string newLibrary);

//    [XmlRpcMethod]
//    void GetLibraries();

//    [XmlRpcMethod]
//    void GetUtterances(string category, string subcategory);
//}

//public interface ILibraryEventsRPC
//{
//    [XmlRpcMethod]
//    void LibraryList(string[] libraries);

//    [XmlRpcMethod]
//    void LibraryChanged(string serialized_LibraryContents);

//    [XmlRpcMethod]
//    void Utterances(string library, string category, string subcategory, string[] utterances);
//}

///* NEW */
