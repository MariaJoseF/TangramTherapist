using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using CookComputing.XmlRpc;
using UnityEngine;

public interface IThalamusTActions
{
    [XmlRpcMethod]
    void UtteranceStarted(string id);
    [XmlRpcMethod]
    void UtteranceFinished(string id);
}


public class ThalamusListener : XmlRpcListenerService, IThalamusTActions
{
    private readonly UtterancesManager _manager;

    public ThalamusListener(UtterancesManager manager)
    {
        _manager = manager;
    }

    public void UtteranceStarted(string id)
    {
        _manager.UtteranceStarted(id);
    }

    public void UtteranceFinished(string id)
    {
        _manager.UtteranceFinished(id);
    }

    //void ILibraryActionsRPC.ChangeLibrary(string newLibrary)
    //{
    //    _manager.ChangeLibrary(newLibrary);
    //}

    //void ILibraryActionsRPC.GetLibraries()
    //{
    //    _manager.GetLibraries();
    //}

    //void ILibraryActionsRPC.GetUtterances(string category, string subcategory)
    //{
    //    _manager.GetUtterances(category, subcategory);
    //}

    //void ILibraryEventsRPC.LibraryList(string[] libraries)
    //{
    //    _manager.LibraryList(libraries);
    //}

    //void ILibraryEventsRPC.LibraryChanged(string serialized_LibraryContents)
    //{
    //    _manager.LibraryChanged(serialized_LibraryContents);
    //}

    //void ILibraryEventsRPC.Utterances(string library, string category, string subcategory, string[] utterances)
    //{
    //    _manager.Utterances(library, category, subcategory, utterances);
    //}
}

public class ThalamusConnector : ITMessages
{
    private string _remoteAddress = "localhost"; //  "192.168.1.65";//

    private bool _printExceptions = true;
    public string RemoteAddress
    {
        get { return _remoteAddress; }
        set
        {
            _remoteAddress = value;
            _remoteUri = string.Format("http://{0}:{1}/", _remoteAddress, _remotePort);
            _rpcProxy.Url = _remoteUri;
        }
    }

    private int _remotePort = 7000;
    public int RemotePort
    {
        get { return _remotePort; }
        set
        {
            _remotePort = value;
            _remoteUri = string.Format("http://{0}:{1}/", _remoteAddress, _remotePort);
            _rpcProxy.Url = _remoteUri;
        }
    }

    private HttpListener _listener;
    private bool _serviceRunning;
    private int _localPort = 7001;
    private bool _shutdown;
    List<HttpListenerContext> _httpRequestsQueue = new List<HttpListenerContext>();
    private Thread _dispatcherThread;
    private Thread _messageDispatcherThread;

    private UtterancesManager _manager;


    ITMessagesRpc _rpcProxy;
    private string _remoteUri = "";

    public ThalamusConnector(UtterancesManager m)
    {
        _manager = m;
        _remoteUri = String.Format("http://{0}:{1}/", _remoteAddress, _remotePort);
        Debug.Log("ThalamusTangram endpoint set to " + _remoteUri);
        _rpcProxy = XmlRpcProxyGen.Create<ITMessagesRpc>();
        _rpcProxy.Timeout = 1000;
        _rpcProxy.Url = _remoteUri;


        _dispatcherThread = new Thread(DispatcherThreadThalamus);
        _messageDispatcherThread = new Thread(MessageDispatcherThalamus);
        _dispatcherThread.Start();
        _messageDispatcherThread.Start();
    }

    #region rpc stuff

    public void Dispose()
    {
        _shutdown = true;

        try
        {
            if (_listener != null) _listener.Stop();
        }
        catch { }

        try
        {
            if (_dispatcherThread != null) _dispatcherThread.Join();
        }
        catch { }

        try
        {
            if (_messageDispatcherThread != null) _messageDispatcherThread.Join();
        }
        catch { }
    }

    public void DispatcherThreadThalamus()
    {
        while (!_serviceRunning)
        {
            try
            {
                Debug.Log("Attempt to start service on port '" + _localPort + "'");
                _listener = new HttpListener();
                _listener.Prefixes.Add(string.Format("http://*:{0}/", _localPort));
                _listener.Start();
                Debug.Log("XMLRPC Listening on " + string.Format("http://*:{0}/", _localPort));
                _serviceRunning = true;
            }
            catch (Exception e)
            {
                _localPort++;
                Debug.Log(e.Message);
                Debug.Log("Port unavaliable.");
                _serviceRunning = false;
            }
        }

        while (!_shutdown)
        {
            try
            {
                HttpListenerContext context = _listener.GetContext();
                lock (_httpRequestsQueue)
                {
                    _httpRequestsQueue.Add(context);
                }
            }
            catch (Exception e)
            {
                if (_printExceptions) Debug.Log("Exception: " + e);
                _serviceRunning = false;
                if (_listener != null)
                    _listener.Close();
            }
        }
        Debug.Log("Terminated DispatcherThreadEnercities");
        _listener.Close();
    }

    public void MessageDispatcherThalamus()
    {
        while (!_shutdown)
        {
            bool performSleep = true;
            try
            {
                if (_httpRequestsQueue.Count > 0)
                {
                    performSleep = false;
                    List<HttpListenerContext> httpRequests;
                    lock (_httpRequestsQueue)
                    {
                        httpRequests = new List<HttpListenerContext>(_httpRequestsQueue);
                        _httpRequestsQueue.Clear();
                    }
                    foreach (HttpListenerContext r in httpRequests)
                    {
                        //ProcessRequest(r);
                        (new Thread(ProcessRequestThalamus)).Start(r);
                        performSleep = false;
                    }
                }
            }
            catch (Exception e)
            {
                if (_printExceptions) Debug.Log("Exception: " + e);
            }
            if (performSleep) Thread.Sleep(10);
        }
        Debug.Log("Terminated PerceptionInfoDispatcherEnercities");
    }

    public void ProcessRequestThalamus(object oContext)
    {
        try
        {
            XmlRpcListenerService svc = new ThalamusListener(_manager);
            svc.ProcessRequest((HttpListenerContext)oContext);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e);
        }
        finally
        {
            Debug.Log("processrequestthalamus exception finally");
        }

    }

    #endregion


    public void Greeting(string player, bool robotN)
    {
        try
        {
            _rpcProxy.Greeting(player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("greeting exception finally");
        }
    }

    public void GameStart(string puzzle, bool robotN)
    {
        try
        {
            _rpcProxy.GameStart(puzzle, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("gamestart exception finally");
        }
    }

    public void NextGame(string player, bool robotN)
    {
        try
        {
            _rpcProxy.NextGame(player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("nextgame exception finally");
        }
    }

    public void FingerHelp(bool robotN)
    {
        try
        {
            _rpcProxy.FingerHelp(robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("fingerhelp exception finally");
        }
    }

    public void ButtonHelp(bool robotN)
    {
        try
        {
            _rpcProxy.ButtonHelp(robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("buttonhelp exception finally");
        }
    }

    public void Win(string puzzle, string player, bool robotN)
    {
        try
        {
            _rpcProxy.Win(puzzle, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("win exception finally");
        }
    }

    public void FastWin(string puzzle, string player, bool robotN)
    {
        try
        {
            _rpcProxy.FastWin(puzzle, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("fastwin exception finally");
        }
    }

    public void Quit(string player, bool robotN)
    {
        try
        {
            _rpcProxy.Quit(player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("quit exception finally");
        }
    }

    public void MotorHelp(bool robotN)
    {
        try
        {
            _rpcProxy.MotorHelp(robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("motorhelp exception finally");
        }
    }

    public void CloseHelp(bool robotN)
    {
        try
        {
            _rpcProxy.CloseHelp(robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("closehelp exception finally");
        }
    }

    public void PositiveFeedback(string nPieces, string player, bool robotN)
    {
        try
        {
            _rpcProxy.PositiveFeedback(nPieces, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("positivefeedback exception finally");
        }
    }

    public void NegativeFeedback(bool robotN)
    {
        try
        {
            _rpcProxy.NegativeFeedback(robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("negativefeedback exception finally");
        }
    }

    public void FirstAnglePrompt(string piece, string player, bool robotN)
    {
        try
        {
            _rpcProxy.FirstAnglePrompt(piece, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("firstangleprompt exception finally");
        }
    }

    public void FirstAnglePromptFinger(string piece, string player, bool robotN)
    {
        try
        {
            _rpcProxy.FirstAnglePromptFinger(piece, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("firstanglepromptfinger exception finally");
        }
    }

    public void FirstAnglePromptButton(string piece, string player, bool robotN)
    {
        try
        {
            _rpcProxy.FirstAnglePromptButton(piece, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("firstanglepromptbutton exception finally");
        }
    }

    public void SecondAnglePrompt(string piece, string player, bool robotN)
    {
        try
        {
            _rpcProxy.SecondAnglePrompt(piece, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("secondangleprompt exception finally");
        }
    }

    public void SecondAnglePromptFinger(string piece, string direction, string player, bool robotN)
    {
        try
        {
            _rpcProxy.SecondAnglePromptFinger(piece, direction, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("secondanglepromptfinger exception finally");
        }
    }

    public void SecondAnglePromptButton(string piece, string nClicks, string player, bool robotN)
    {
        try
        {
            _rpcProxy.SecondAnglePromptButton(piece, nClicks, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("secondanglepromptbutton exception finally");
        }
    }

    public void ThirdAnglePrompt(string piece, string player, bool robotN)
    {
        try
        {
            _rpcProxy.ThirdAnglePrompt(piece, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("thirdangleprompt exception finally");
        }
    }

    public void StopAnglePrompt(string piece, bool robotN)
    {
        try
        {
            _rpcProxy.StopAnglePrompt(piece, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("stopangleprompt exception finally");
        }
    }

    public void FirstIdlePrompt(string piece, string player, bool robotN)
    {
        try
        {
            _rpcProxy.FirstIdlePrompt(piece, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("firstidleprompt exception finally");
        }
    }

    public void FirstPlacePrompt(string piece, string player, bool robotN)
    {
        try
        {
            _rpcProxy.FirstPlacePrompt(piece, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("firstplaceprompt exception finally");
        }
    }

    public void SecondPrompt1Position(string piece, string pos, string player, bool robotN)
    {
        try
        {
            _rpcProxy.SecondPrompt1Position(piece, pos, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("secondprompt1position exception finally");
        }
    }

    public void SecondPrompt2Position(string piece, string pos1, string pos2, string player, bool robotN)
    {
        try
        {
            _rpcProxy.SecondPrompt2Position(piece, pos1, pos2, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("secondprompt2position exception finally");
        }
    }

    public void SecondPromptPlace(string piece, string pos, string relativePiece, string player, bool robotN)
    {
        try
        {
            _rpcProxy.SecondPromptPlace(piece, pos, relativePiece, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("SecondPromptPlace exception finally");
        }
    }

    public void ThirdPrompt(string piece, string player, bool robotN)
    {
        try
        {
            _rpcProxy.ThirdPrompt(piece, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("thirdprompt exception finally");
        }
    }

    public void HardClue(string player, bool robotN)
    {
        try
        {
            _rpcProxy.HardClue(player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("hardclue exception finally");
        }
    }

    public void PGreeting(string player, bool robotN)
    {
        try
        {
            _rpcProxy.PGreeting(player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("pgreeting exception finally");
        }
    }

    public void PButtonHelp(bool robotN)
    {
        try
        {
            _rpcProxy.PButtonHelp(robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("pbuttonhelp exception finally");
        }
    }

    public void PFingerHelp(bool robotN)
    {
        try
        {
            _rpcProxy.PFingerHelp(robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("pfingerhelp exception finally");
        }
    }

    public void PWin(string puzzle, string player, bool robotN)
    {
        try
        {
            _rpcProxy.PWin(puzzle, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            Debug.Log("pwin exception finally");
        }
    }

    public void PChildTurn(string player, bool robotN)
    {
        try
        {
            _rpcProxy.PChildTurn(player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            //  Debug.Log("pchildturn exception finally");
        }
    }

    public void PRobotTurn(bool robotN)
    {
        try
        {
            _rpcProxy.PRobotTurn(robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            //  Debug.Log("probotturn exception finally");
        }
    }

    public void PRobotDrag(bool robotN)
    {
        try
        {
            _rpcProxy.PRobotDrag(robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            // Debug.Log("probotfrag exception finally");
        }
    }

    public void PRobotRotDrag(bool robotN)
    {
        try
        {
            _rpcProxy.PRobotRotDrag(robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            //  Debug.Log("probotrotdrag exception finally");
        }
    }

    public void PRobotWin(string nPieces, string player, bool robotN)
    {
        try
        {
            _rpcProxy.PRobotWin(nPieces, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            //  Debug.Log("probotwin exception finally");
        }
    }

    public void PRobotReminder(bool robotN)
    {
        try
        {
            _rpcProxy.PRobotReminder(robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            //  Debug.Log("probotreminder exception finally");
        }
    }

    public void PAskingPlace(string piece, string player, bool robotN)
    {
        try
        {
            _rpcProxy.PAskingPlace(piece, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            // Debug.Log("paskingplace exception finally");
        }
    }

    public void PAskingRotate(string piece, string player, bool robotN)
    {
        try
        {
            _rpcProxy.PAskingRotate(piece, player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            //   Debug.Log("paskingrotate exception finally");
        }
    }

    public void PAskingPlaceWin(string player, bool robotN)
    {
        try
        {
            _rpcProxy.PAskingPlaceWin(player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            //  Debug.Log("paskingplacewin exception finally");
        }
    }

    public void PAskingRotateWin(string player, bool robotN)
    {
        try
        {
            _rpcProxy.PAskingRotateWin(player, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            // Debug.Log("paskingrotatewin exception finally");
        }
    }

    public void PAskingPlaceWrong(string piece, bool robotN)
    {
        try
        {
            _rpcProxy.PAskingPlaceWrong(piece, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            //  Debug.Log("paskingplacewrong exception finally");
        }
    }

    public void PAskingRotateWrong(string piece, bool robotN)
    {
        try
        {
            _rpcProxy.PAskingRotateWrong(piece, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            //  Debug.Log("paskingrotatewrong exception finally");
        }
    }

    public void PAskingQuit(bool robotN)
    {
        try
        {
            _rpcProxy.PAskingQuit(robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            //  Debug.Log("paskingquit exception finally");
        }
    }

    public void PGivingPlace(string piece, string pos, bool robotN)
    {
        try
        {
            _rpcProxy.PGivingPlace(piece, pos, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            // Debug.Log("pgivingplace exception finally");
        }
    }

    public void PGivingRotate(string piece, bool robotN)
    {
        try
        {
            _rpcProxy.PGivingRotate(piece, robotN);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            //  Debug.Log("pgivingrotate exception finally");
        }
    }
    public void WriteJSON(string timestamp, string info)

    {
        try
        {
            _rpcProxy.WriteJSON(timestamp, info, @"c:\Developer\Logs\", @"Logs");
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            // Debug.Log("writejson exception finally");
        }
    }

    public void CancelUtterance(string id)
    {
        try
        {
            _rpcProxy.CancelUtterance(id);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            // Debug.Log("CancelUtterance exception finally");
        }
    }

    public void WriteJSON(string timestamp, string info, string path_file, string aux_path)
    {
        try
        {
            _rpcProxy.WriteJSON(timestamp, info, path_file, aux_path);

        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }
        finally
        {
            // Debug.Log("writejson exception finally");
        }
    }

    /*public void GlanceAtScreen(double x, double y)
    {
        try
        {
            _rpcProxy.GlanceAtScreen(x, y);
        }
        catch (Exception e)
        {
            if (_printExceptions) Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : ""));
        }

    }*/

    ///*NEW*/

    //void LibraryList(string[] libraries)
    //{
    //    try
    //    {
    //        Debug.Log("Sent to Unity: LibraryList");
    //        _rpcProxy.LibraryList(libraries);
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : "")
    //    }
    //}

    //void LibraryChanged(string serialized_LibraryContents)
    //{
    //    try
    //    {
    //        Debug.Log("Sent to Unity: LibraryChanged");
    //        _rpcProxy.LibraryChanged(serialized_LibraryContents);
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : "")
    //    }
    //}

    //void Utterances(string library, string category, string subcategory, string[] utterances)
    //{
    //    try
    //    {
    //        Debug.Log("Sent to Unity: Utterances");
    //        _rpcProxy.Utterances(library, category, subcategory, utterances);
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log("Exception: " + e.Message + (e.InnerException != null ? ": " + e.InnerException : "")
    //    }
    //}

    ///*NEW*/

}
