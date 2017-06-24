/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UtterancesManager : MonoBehaviour {
    private static UtterancesManager instance = null;
    StateConnector s;
    string currentUtterance = null;
    bool withoutHelp = false, canceling = false, doubleCanceling = false;
    public float hardClueSeconds;
    public static UtterancesManager Instance { get { return instance; } }
    Queue utterancesQueue = new Queue();

    void Awake() {
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

    }

    public void UtteranceFinished(string id)
    {
        if(!canceling)
            currentUtterance = null;
        print("ACABOU UTTERANCE " + id);

        utterancesQueue.Dequeue();

        if (id.Contains("fingerHelp") || id.Contains("buttonHelp") || (id.Contains("start") && withoutHelp))
        {
            GameState.Instance.haveToEnableAllPieces = true;
        }
        else if (id.Contains("greeting") || id.Contains("nextGame")) 
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
    }

    void CancelingUtterance()
    {
        if (utterancesQueue.Count > 0)
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
    }

    public void Greeting()
    {
        utterancesQueue.Enqueue("greeting");
        s.Greeting();
    }

    public void GameStart(string puzzle, bool help)
    {
        utterancesQueue.Enqueue("start");
        s.GameStart(puzzle);
        withoutHelp = help;
    }

    public void NextGame()
    {
        utterancesQueue.Enqueue("nextGame");
        s.NextGame();
    }

    public void FingerHelp()
    {
        utterancesQueue.Enqueue("fingerHelp");
        s.FingerHelp();
    }

    public void ButtonHelp()
    {
        utterancesQueue.Enqueue("buttonHelp");
        s.ButtonHelp();
    }

    public void Win(string puzzle)
    {
        CancelingUtterance();
        utterancesQueue.Enqueue("win");
        s.Win(puzzle);
    }

    public void FastWin(string puzzle)
    {
        CancelingUtterance();
        utterancesQueue.Enqueue("fastWin");
        s.FastWin(puzzle);
    }

    public void Quit()
    {
        CancelingUtterance();
        utterancesQueue.Enqueue("quit");
        s.Quit();
    }

    public void MotorHelp()
    {
        if (utterancesQueue.Count == 0)
        {
            utterancesQueue.Enqueue("motorHelp");
            s.MotorHelp();
        }
    }

    public void CloseHelp()
    {
        CancelingUtterance();
        utterancesQueue.Enqueue("closeHelp");
        s.CloseHelp();
    }

    public void PositiveFeedback(string nPieces)
    {
        if (!currentUtterance.Contains("positiveFeedback"))
            CancelingUtterance();
        else
            return;

        utterancesQueue.Enqueue("positiveFeedback");
        s.PositiveFeedback(nPieces);
    }

    public bool NegativeFeedback()
    {
        if(utterancesQueue.Count == 0)
        {
            utterancesQueue.Enqueue("negativeFeedback");
            s.NegativeFeedback();
            return true;
        }
        else
            return false;
    }

    public bool FirstAnglePrompt(string piece)
    {
        if (utterancesQueue.Count == 0)
        {
            utterancesQueue.Enqueue("firstAnglePrompt");
            s.FirstAnglePrompt(piece);
            return true;
        }
        else
            return false;
    }

    public bool FirstAnglePromptFinger(string piece)
    {
        if (utterancesQueue.Count == 0)
        {
            utterancesQueue.Enqueue("firstAnglePromptFinger");
            s.FirstAnglePromptFinger(piece);
            return true;
        }
        else
            return false;
    }

    public bool FirstAnglePromptButton(string piece)
    {
        if (utterancesQueue.Count == 0)
        {
            utterancesQueue.Enqueue("firstAnglePromptButton");
            s.FirstAnglePromptButton(piece);
            return true;
        }
        else
            return false;
    }

    public bool SecondAnglePrompt(string piece)
    {
        if (utterancesQueue.Count == 0)
        {
            utterancesQueue.Enqueue("secondAnglePrompt");
            s.SecondAnglePrompt(piece);
            return true;
        }
        else
            return false;
    }

    public bool SecondAnglePromptFinger(string piece, string direction)
    {
        if (utterancesQueue.Count == 0)
        {
            utterancesQueue.Enqueue("secondAnglePromptFinger");
            s.SecondAnglePromptFinger(piece, direction);
            return true;
        }
        else
            return false;
    }

    public bool SecondAnglePromptButton(string piece, string nClicks)
    {
        if (utterancesQueue.Count == 0)
        {
            utterancesQueue.Enqueue("secondAnglePromptButton");
            s.SecondAnglePromptButton(piece, nClicks);
            return true;
        }
        else
            return false;
    }

    public bool ThirdAnglePrompt(string piece)
    {
        if (utterancesQueue.Count == 0)
        {
            utterancesQueue.Enqueue("thirdAnglePrompt");
            s.ThirdAnglePrompt(piece);
            return true;
        }
        else
            return false;
    }

    public void StopAnglePrompt(string piece)
    {
        CancelingUtterance();
        utterancesQueue.Enqueue("stopAnglePrompt");
        s.StopAnglePrompt(piece);
    }

    public bool FirstIdlePrompt(string piece)
    {
        if (utterancesQueue.Count == 0)
        {
            utterancesQueue.Enqueue("firstIdlePrompt");
            s.FirstIdlePrompt(piece);
            return true;
        }
        else
            return false;
    }

    public bool FirstPlacePrompt(string piece)
    {
        if (utterancesQueue.Count == 0)
        {
            utterancesQueue.Enqueue("firstPlacePrompt");
            s.FirstPlacePrompt(piece);
            return true;
        }
        else
            return false;
    }

    public bool SecondPrompt1Position(string piece, string pos)
    {
        if (utterancesQueue.Count == 0)
        {
            utterancesQueue.Enqueue("secondPrompt1Position");
            s.SecondPrompt1Position(piece, pos);
            return true;
        }
        else
            return false;
    }

    public bool SecondPrompt2Position(string piece, string pos1, string pos2)
    {
        if (utterancesQueue.Count == 0)
        {
            utterancesQueue.Enqueue("secondPrompt2Position");
            s.SecondPrompt2Position(piece, pos1, pos2);
            return true;
        }
        else
            return false;
    }

    public bool SecondPromptPlace(string piece, string pos, string relativePiece)
    {
        if (utterancesQueue.Count == 0)
        {
            utterancesQueue.Enqueue("secondPromptPlace");
            s.SecondPromptPlace(piece, pos, relativePiece);
            return true;
        }
        else
            return false;
    }

    public bool ThirdPrompt(string piece)
    {
        if (utterancesQueue.Count == 0)
        {
            utterancesQueue.Enqueue("thirdPrompt");
            s.ThirdPrompt(piece);
            return true;
        }
        else
            return false;
    }

    public bool HardClue(float seconds)
    {
        if (utterancesQueue.Count == 0)
        {
            utterancesQueue.Enqueue("hardClue");
            hardClueSeconds = seconds;
            s.HardClue();
            return true;
        }
        else
            return false;
    }

    public void Dispose()
    {
        Instance.Dispose();
    }
}*/


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
    bool withoutHelp = false, canceling = false, doubleCanceling = false;
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
        WriteJSON("ROBOT: " + id);
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
        }
    }

    public void Greeting()
    {
        s.Greeting();
    }

    public void GameStart(string puzzle, bool help)
    {
        s.GameStart(puzzle);
        withoutHelp = help;
    }

    public void NextGame()
    {
        s.NextGame();
    }

    public void FingerHelp()
    {
        s.FingerHelp();
    }

    public void ButtonHelp()
    {
        s.ButtonHelp();
    }

    public void Win(string puzzle)
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
        s.Win(puzzle);
    }

    public void FastWin(string puzzle)
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
        s.FastWin(puzzle);
    }

    public void Quit()
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
        s.Quit();
    }

    public void MotorHelp()
    {
        if (currentUtterance == null)
        {
            s.MotorHelp();
        }
    }

    public void CloseHelp()
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
        s.CloseHelp();
    }

    public void PositiveFeedback(string nPieces)
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
        s.PositiveFeedback(nPieces);
    }

    public bool NegativeFeedback()
    {
        if (currentUtterance == null)
        {
            s.NegativeFeedback();
            return true;
        }
        else
            return false;
    }

    public bool FirstAnglePrompt(string piece, int no_voice)
    {
        if (currentUtterance == null)
        {
            if (no_voice == 0)
            {
s.FirstAnglePrompt(piece);
            }
                
            return true;
        }
        else
            return false;
    }

    public bool FirstAnglePromptFinger(string piece, int no_voice)
    {
        if (currentUtterance == null)
        {
            if (no_voice == 0)
            {
                s.FirstAnglePromptFinger(piece);
            }
            return true;
        }
        else
            return false;
    }

    public bool FirstAnglePromptButton(string piece, int no_voice)
    {
        if (currentUtterance == null)
        {
            if (no_voice == 0)
            {
                s.FirstAnglePromptButton(piece);
            }
            return true;
        }
        else
            return false;
    }

    public bool SecondAnglePrompt(string piece, int no_voice)
    {
        if (currentUtterance == null)
        {
            if (no_voice == 0)
            {
                s.SecondAnglePrompt(piece);
            }
            return true;
        }
        else
            return false;
    }

    public bool SecondAnglePromptFinger(string piece, string direction, int no_voice)
    {
        if (currentUtterance == null)
        {
            if (no_voice == 0)
            {
                s.SecondAnglePromptFinger(piece, direction);
            }
            return true;
        }
        else
            return false;
    }

    public bool SecondAnglePromptButton(string piece, string nClicks, int no_voice)
    {
        if (currentUtterance == null)
        {
            if (no_voice == 0)
            {
                s.SecondAnglePromptButton(piece, nClicks);
            }
            return true;
        }
        else
            return false;
    }

    public bool ThirdAnglePrompt(string piece, int no_voice)
    {
        if (currentUtterance == null)
        {
            if (no_voice == 0)
            {
                s.ThirdAnglePrompt(piece);
            }
            return true;
        }
        else
            return false;
    }

    public void StopAnglePrompt(string piece)
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
        s.StopAnglePrompt(piece);
    }

    public bool FirstIdlePrompt(string piece, int no_voice)
    {
        if (currentUtterance == null)
        {
            if (no_voice == 0)
            {
                s.FirstIdlePrompt(piece);
            }
            return true;
        }
        else
            return false;
    }

    public bool FirstPlacePrompt(string piece, int no_voice)
    {
        if (currentUtterance == null)
        {
            if (no_voice == 0)
            {
                s.FirstPlacePrompt(piece);
            }
            return true;
        }
        else
            return false;
    }

    public bool SecondPrompt1Position(string piece, string pos, int no_voice)
    {
        if (currentUtterance == null)
        {
            if (no_voice == 0)
            {
                s.SecondPrompt1Position(piece, pos);
            }
            return true;
        }
        else
            return false;
    }

    public bool SecondPrompt2Position(string piece, string pos1, string pos2, int no_voice)
    {
        if (currentUtterance == null)
        {
            if (no_voice == 0)
            {
                s.SecondPrompt2Position(piece, pos1, pos2);
            }
            return true;
        }
        else
            return false;
    }

    public bool SecondPromptPlace(string piece, string pos, string relativePiece, int no_voice)
    {
        if (currentUtterance == null)
        {
            if (no_voice == 0)
            {
                s.SecondPromptPlace(piece, pos, relativePiece);
                }
            return true;
        }
        else
            return false;
    }

    public bool ThirdPrompt(string piece, int no_voice)
    {
        if (currentUtterance == null)
        {
            if (no_voice == 0)
            {
                s.ThirdPrompt(piece);
            }
            return true;
        }
        else
            return false;
    }

    public bool HardClue(float seconds, int no_voice)
    {
        if (currentUtterance == null)
        {
            if (no_voice == 0)
            {
                hardClueSeconds = seconds;
                s.HardClue();
            }

            return true;
        }
        else
            return false;
    }

    public void PGreeting()
    {
        s.PGreeting();
    }

    public void PButtonHelp()
    {
        s.PButtonHelp();
    }

    public void PFingerHelp()
    {
        s.PFingerHelp();
    }

    public void PWin(string puzzle)
    {
        if (currentUtterance != null)
        {
            s.CancelUtterance(currentUtterance);
        }
        s.PWin(puzzle);
    }

    public void PChildTurn()
    {
        s.PChildTurn();
    }

    public void PRobotTurn()
    {
        s.PRobotTurn();
    }

    public void PRobotWin(string nPieces)
    {
        if (currentUtterance != null)
        {
            s.CancelUtterance(currentUtterance);
        }
        s.PRobotWin(nPieces);
    }

    public void PRobotReminder()
    {
        if (currentUtterance == null)
            s.PRobotTurn();
    }

    public bool PAskingPlace(string piece)
    {
        if (currentUtterance == null)
        {
            s.PAskingPlace(piece);
            return true;
        }
        else
            return false;
    }

    public bool PAskingRotate(string piece)
    {
        if (currentUtterance == null)
        {
            s.PAskingRotate(piece);
            return true;
        }
        else
            return false;
    }

    public void PAskingPlaceWin()
    {
        if (currentUtterance != null)
        {
            s.CancelUtterance(currentUtterance);
        }
        s.PAskingPlaceWin();
    }

    public void PAskingRotateWin()
    {
        if (currentUtterance != null)
        {
            s.CancelUtterance(currentUtterance);
        }
        s.PAskingRotateWin();
    }

    public void PAskingPlaceWrong(string piece)
    {
        if (currentUtterance != null)
        {
            s.CancelUtterance(currentUtterance);
        }
        s.PAskingPlaceWrong(piece);
    }

    public bool PAskingRotateWrong(string piece)
    {
        if (currentUtterance == null)
        {
            s.PAskingRotateWrong(piece);
            return true;
        }
        else
            return false;
    }

    public void PAskingQuit()
    {
        if (currentUtterance != null)
        {
            s.CancelUtterance(currentUtterance);
        }
        s.PAskingQuit();
    }

    public bool PGivingPlace(string piece, string pos)
    {
        if (currentUtterance == null)
        {
            s.PGivingPlace(piece, pos);
            return true;
        }
        else
            return false;
    }

    public bool PGivingRotate(string piece)
    {
        if (currentUtterance == null)
        {
            s.PGivingRotate(piece);
            return true;
        }
        else
            return false;
    }

    public void WriteJSON(string info)
    {
        s.WriteJSON(DateTime.Now.ToString(), info);
    }

    public void Dispose()
    {
        Instance.Dispose();
        
    }

    internal void CheckUtteranceFinish()
    {
        if (currentUtterance != null)
        {
            Console.WriteLine();
            s.CancelUtterance(currentUtterance);
        }
    }
}
