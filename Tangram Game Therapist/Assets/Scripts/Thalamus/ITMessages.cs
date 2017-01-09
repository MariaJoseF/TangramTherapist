using CookComputing.XmlRpc;

public interface ITMessagesRpc : ITMessages, IXmlRpcProxy { }

public interface ITMessages
{
    void Dispose();

    [XmlRpcMethod]
    void Greeting(string player);

    [XmlRpcMethod]
    void GameStart(string puzzle);

    [XmlRpcMethod]
    void NextGame(string player);

    [XmlRpcMethod]
    void FingerHelp();

    [XmlRpcMethod]
    void ButtonHelp();

    [XmlRpcMethod]
    void Win(string puzzle, string player);

    [XmlRpcMethod]
    void FastWin(string puzzle, string player);

    [XmlRpcMethod]
    void Quit(string player);

    [XmlRpcMethod]
    void MotorHelp();

    [XmlRpcMethod]
    void CloseHelp();

    [XmlRpcMethod]
    void PositiveFeedback(string nPieces, string player);

    [XmlRpcMethod]
    void NegativeFeedback();

    [XmlRpcMethod]
    void FirstAnglePrompt(string piece, string player);

    [XmlRpcMethod]
    void FirstAnglePromptFinger(string piece, string player);

    [XmlRpcMethod]
    void FirstAnglePromptButton(string piece, string player);

    [XmlRpcMethod]
    void SecondAnglePrompt(string piece, string player);

    [XmlRpcMethod]
    void SecondAnglePromptFinger(string piece, string direction, string player);

    [XmlRpcMethod]
    void SecondAnglePromptButton(string piece, string nClicks, string player);

    [XmlRpcMethod]
    void ThirdAnglePrompt(string piece, string player);

    [XmlRpcMethod]
    void StopAnglePrompt(string piece);

    [XmlRpcMethod]
    void FirstIdlePrompt(string piece, string player);

    [XmlRpcMethod]
    void FirstPlacePrompt(string piece, string player);

    [XmlRpcMethod]
    void SecondPrompt1Position(string piece, string pos, string player);

    [XmlRpcMethod]
    void SecondPrompt2Position(string piece, string pos1, string pos2, string player);

    [XmlRpcMethod]
    void SecondPromptPlace(string piece, string pos, string relativePiece, string player);

    [XmlRpcMethod]
    void ThirdPrompt(string piece, string player);

    [XmlRpcMethod]
    void HardClue(string player);

    //[XmlRpcMethod]
    //void PGreeting(string player);

    //[XmlRpcMethod]
    //void PButtonHelp();

    //[XmlRpcMethod]
    //void PFingerHelp();

    //[XmlRpcMethod]
    //void PWin(string puzzle, string player);

    //[XmlRpcMethod]
    //void PChildTurn(string player);

    //[XmlRpcMethod]
    //void PRobotTurn();

    //[XmlRpcMethod]
    //void PRobotDrag();

    //[XmlRpcMethod]
    //void PRobotRotDrag();

    //[XmlRpcMethod]
    //void PRobotWin(string nPieces, string player);

    //[XmlRpcMethod]
    //void PRobotReminder();

    //[XmlRpcMethod]
    //void PAskingPlace(string piece, string player);

    //[XmlRpcMethod]
    //void PAskingRotate(string piece, string player);

    //[XmlRpcMethod]
    //void PAskingPlaceWin(string player);

    //[XmlRpcMethod]
    //void PAskingRotateWin(string player);

    //[XmlRpcMethod]
    //void PAskingPlaceWrong(string piece);

    //[XmlRpcMethod]
    //void PAskingRotateWrong(string piece);

    //[XmlRpcMethod]
    //void PAskingQuit();

    //[XmlRpcMethod]
    //void PGivingPlace(string piece, string pos);

    //[XmlRpcMethod]
    //void PGivingRotate(string piece);

    [XmlRpcMethod]
    void WriteJSON(string timestamp, string info);

    [XmlRpcMethod]
    void CancelUtterance(string id);
}