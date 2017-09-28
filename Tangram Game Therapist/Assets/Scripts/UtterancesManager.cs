

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UtterancesManager : MonoBehaviour
{
    private static UtterancesManager instance = null;
    StateConnector s;
    string currentUtterance = null;
    bool withoutHelp = false, canceling = false, doubleCanceling = false, writeRobot = false;
    public float hardClueSeconds;
    public static UtterancesManager Instance { get { return instance; } }

    

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        s = new StateConnector(Instance);
    }

    public void UtteranceStarted(string id)
    {
        currentUtterance = id;
        canceling = false;
        print("COMEÇOU UTTERANCE " + id);
        if (doubleCanceling)
        {
            s.CancelUtterance(id);
            doubleCanceling = false;
            canceling = true;
        }

        /*NEW*/

        writeRobot = false;

        if (Therapist.Instance.NiceRobotGet)
        {
            WriteJSON("ROBOT NICE: " + id);
            writeRobot = true;
        }
        else
        {
            WriteJSON("ROBOT RUDE: " + id);
            writeRobot = true;
        }

        /*NEW*/
    }

    public void UtteranceFinished(string id)
    {
        if (id != null && id != string.Empty)
        {
            if (!canceling)
                currentUtterance = null;
            print("ACABOU UTTERANCE " + id);

            if (id.Contains("pChildTurn") || id.Contains("fingerHelp") || id.Contains("buttonHelp")
                || id.Contains("pFingerHelp") || id.Contains("pButtonHelp") || (id.Contains("start") && withoutHelp))
            {
                GameState.Instance.haveToEnableAllPieces = true;
            }
            else if (id.Contains("pGreeting") || id.Contains("greeting") || id.Contains("nextGame"))
            {
                GameState.Instance.haveToEnablePlayButton = true;
            }
            else if (id.Contains("hardClue"))
            {
                Therapist.Instance.nFailedTries = 0;
                Therapist.Instance.nWrongAngleTries = 0;
                GameState.Instance.showHardClue = true;
            }
            else if (id.Contains("thirdPrompt") || id.Contains("thirdAnglePrompt"))
            {
                Therapist.Instance.nFailedTries = 0;
                Therapist.Instance.nWrongAngleTries = 0;
                GameState.Instance.showCluePiece = Therapist.Instance.currentPlace;
                GameState.Instance.showClue = true;
            }
            else if (id.Contains("firstAnglePrompt") || id.Contains("firstAnglePromptFinger") || id.Contains("firstAnglePromptButton")
                || id.Contains("secondAnglePrompt") || id.Contains("secondAnglePromptFinger") || id.Contains("secondAnglePromptButton"))
            {
                Therapist.Instance.nWrongAngleTries = 0;
            }
            else if (id.Contains("firstPlacePrompt") || id.Contains("secondPrompt1Position") || id.Contains("secondPrompt2Position")
                || id.Contains("secondPromptPlace") || id.Contains("closeHelp"))
            {
                Therapist.Instance.nFailedTries = 0;
            }

            /*NEW*/

            //didint write on the log file when the utterance started
            if (!writeRobot)
            {
                if (Therapist.Instance.NiceRobotGet)
                {
                    WriteJSON("ROBOT NICE: " + id);
                    writeRobot = true;
                }
                else
                {
                    WriteJSON("ROBOT RUDE: " + id);
                    writeRobot = true;
                }
            }

            /*NEW*/

        }
    }

    public void Greeting(bool robotN)
    {
        s.Greeting(robotN);
    }

    public void GameStart(string puzzle, bool help, bool robotN)
    {
        s.GameStart(puzzle, robotN);
        withoutHelp = help;
    }

    public void NextGame(bool robotN)
    {
        s.NextGame(robotN);
    }

    public void FingerHelp(bool robotN)
    {
        s.FingerHelp(robotN);
    }

    public void ButtonHelp(bool robotN)
    {
        s.ButtonHelp(robotN);
    }

    public void Win(string puzzle, bool robotN)
    {
        if (currentUtterance != null)
        {
            if (!canceling)
            {
                print("CANCELAR " + currentUtterance);
                s.CancelUtterance(currentUtterance);
                canceling = true;
            }
            else
            {
                print("CANCELAR 2º vez " + currentUtterance);
                doubleCanceling = true;
            }
        }
        s.Win(puzzle, robotN);
    }

    public void FastWin(string puzzle, bool robotN)
    {
        if (currentUtterance != null)
        {
            if (!canceling)
            {
                print("CANCELAR " + currentUtterance);
                s.CancelUtterance(currentUtterance);
                canceling = true;
            }
            else
            {
                print("CANCELAR 2º vez " + currentUtterance);
                doubleCanceling = true;
            }
        }
        s.FastWin(puzzle, robotN);
    }

    public void Quit(bool robotN)
    {
        if (currentUtterance != null)
        {
            if (!canceling)
            {
                print("CANCELAR " + currentUtterance);
                s.CancelUtterance(currentUtterance);
                canceling = true;
            }
            else
            {
                print("CANCELAR 2º vez " + currentUtterance);
                doubleCanceling = true;
            }
        }
        s.Quit(robotN);
    }

    public void MotorHelp(bool robotN)
    {
        if (currentUtterance == null)
        {
            s.MotorHelp(robotN);
        }
    }

    public void CloseHelp(bool robotN)
    {
        if (currentUtterance != null)
        {
            if (!canceling)
            {
                print("CANCELAR " + currentUtterance);
                s.CancelUtterance(currentUtterance);
                canceling = true;
            }
            else
            {
                print("CANCELAR 2º vez " + currentUtterance);
                doubleCanceling = true;
            }
        }
        s.CloseHelp(robotN);
    }

    public void PositiveFeedback(string nPieces, bool robotN)
    {
        if (currentUtterance != null)
        {
            if (!currentUtterance.Contains("positiveFeedback"))
            {
                if (!canceling)
                {
                    print("CANCELAR " + currentUtterance);
                    s.CancelUtterance(currentUtterance);
                    canceling = true;
                }
                else
                {
                    print("CANCELAR 2º vez " + currentUtterance);
                    doubleCanceling = true;
                }
            }
            else
                return;
        }
        s.PositiveFeedback(nPieces, robotN);
    }

    public bool NegativeFeedback(bool robotN)
    {
        if (currentUtterance == null)
        {
            s.NegativeFeedback(robotN);
            return true;
        }
        else
            return false;
    }

    public bool FirstAnglePrompt(string piece, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.FirstAnglePrompt(piece, robotN);

            return true;
        }
        else
            return false;
    }

    public bool FirstAnglePromptFinger(string piece, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.FirstAnglePromptFinger(piece, robotN);
            return true;
        }
        else
            return false;
    }

    public bool FirstAnglePromptButton(string piece, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.FirstAnglePromptButton(piece, robotN);
            return true;
        }
        else
            return false;
    }

    public bool SecondAnglePrompt(string piece, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.SecondAnglePrompt(piece, robotN);
            return true;
        }
        else
            return false;
    }

    public bool SecondAnglePromptFinger(string piece, string direction, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.SecondAnglePromptFinger(piece, direction, robotN);
            return true;
        }
        else
            return false;
    }

    public bool SecondAnglePromptButton(string piece, string nClicks, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.SecondAnglePromptButton(piece, nClicks, robotN);
            return true;
        }
        else
            return false;
    }

    public bool ThirdAnglePrompt(string piece, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.ThirdAnglePrompt(piece, robotN);
            return true;
        }
        else
            return false;
    }

    public bool StopAnglePrompt(string piece, bool robotN)
    {
        if (currentUtterance != null)
        {
            if (!canceling)
            {
                print("CANCELAR " + currentUtterance);
                s.CancelUtterance(currentUtterance);
                canceling = true;
            }
            else
            {
                print("CANCELAR 2º vez " + currentUtterance);
                doubleCanceling = true;
            }
        }

        if (currentUtterance == null)
        {
            s.ThirdAnglePrompt(piece, robotN);
            return true;
        }
        else
            return false;
    }

    public bool FirstIdlePrompt(string piece, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.FirstIdlePrompt(piece, robotN);
            return true;
        }
        else
            return false;
    }

    public bool FirstPlacePrompt(string piece, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.FirstPlacePrompt(piece, robotN);
            return true;
        }
        else
            return false;
    }

    public bool SecondPrompt1Position(string piece, string pos, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.SecondPrompt1Position(piece, pos, robotN);
            return true;
        }
        else
            return false;
    }

    public bool SecondPrompt2Position(string piece, string pos1, string pos2, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.SecondPrompt2Position(piece, pos1, pos2, robotN);
            return true;
        }
        else
            return false;
    }

    public bool SecondPromptPlace(string piece, string pos, string relativePiece, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.SecondPromptPlace(piece, pos, relativePiece, robotN);
            return true;
        }
        else
            return false;
    }

    public bool ThirdPrompt(string piece, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.ThirdPrompt(piece, robotN);
            return true;
        }
        else
            return false;
    }

    public bool HardClue(float seconds, bool robotN)
    {
        if (currentUtterance == null)
        {
            hardClueSeconds = seconds;
            s.HardClue(robotN);
            return true;
        }
        else
            return false;
    }

    public void PGreeting(bool robotN)
    {
        s.PGreeting(robotN);
    }

    public void PButtonHelp(bool robotN)
    {
        s.PButtonHelp(robotN);
    }

    public void PFingerHelp(bool robotN)
    {
        s.PFingerHelp(robotN);
    }

    public void PWin(string puzzle, bool robotN)
    {
        if (currentUtterance != null)
        {
            s.CancelUtterance(currentUtterance);
        }
        s.PWin(puzzle, robotN);
    }

    public void PChildTurn(bool robotN)
    {
        s.PChildTurn(robotN);
    }

    public void PRobotTurn(bool robotN)
    {
        s.PRobotTurn(robotN);
    }

    public void PRobotWin(string nPieces, bool robotN)
    {
        if (currentUtterance != null)
        {
            s.CancelUtterance(currentUtterance);
        }
        s.PRobotWin(nPieces, robotN);
    }

    public void PRobotReminder(bool robotN)
    {
        if (currentUtterance == null)
            s.PRobotTurn(robotN);
    }

    public bool PAskingPlace(string piece, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.PAskingPlace(piece, robotN);
            return true;
        }
        else
            return false;
    }

    public bool PAskingRotate(string piece, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.PAskingRotate(piece, robotN);
            return true;
        }
        else
            return false;
    }

    public void PAskingPlaceWin(bool robotN)
    {
        if (currentUtterance != null)
        {
            s.CancelUtterance(currentUtterance);
        }
        s.PAskingPlaceWin(robotN);
    }

    public void PAskingRotateWin(bool robotN)
    {
        if (currentUtterance != null)
        {
            s.CancelUtterance(currentUtterance);
        }
        s.PAskingRotateWin(robotN);
    }

    public void PAskingPlaceWrong(string piece, bool robotN)
    {
        if (currentUtterance != null)
        {
            s.CancelUtterance(currentUtterance);
        }
        s.PAskingPlaceWrong(piece, robotN);
    }

    public bool PAskingRotateWrong(string piece, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.PAskingRotateWrong(piece, robotN);
            return true;
        }
        else
            return false;
    }

    public void PAskingQuit(bool robotN)
    {
        if (currentUtterance != null)
        {
            s.CancelUtterance(currentUtterance);
        }
        s.PAskingQuit(robotN);
    }

    public bool PGivingPlace(string piece, string pos, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.PGivingPlace(piece, pos, robotN);
            return true;
        }
        else
            return false;
    }

    public bool PGivingRotate(string piece, bool robotN)
    {
        if (currentUtterance == null)
        {
            s.PGivingRotate(piece, robotN);
            return true;
        }
        else
            return false;
    }

    public void WriteJSON(string info)
    {
        s.WriteJSON(DateTime.Now.ToString(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss")), info);
    }

    public void Dispose()
    {
        Instance.Dispose();

    }

    internal void CheckUtteranceFinish()
    {
        if (currentUtterance != null)
        {
            Console.WriteLine("Canceled ");
            s.CancelUtterance(currentUtterance);
            currentUtterance = null;
        }
    }
}
