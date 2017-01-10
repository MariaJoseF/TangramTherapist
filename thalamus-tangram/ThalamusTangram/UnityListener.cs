using CookComputing.XmlRpc;
using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace ThalamusTangram
{
    public class UnityListener : XmlRpcListenerService, ITMessages
    {
        private ThalamusConnector _thalamusCS;
        //string fileName = null;

        public UnityListener(ThalamusConnector thalamusCS)
        {
            _thalamusCS = thalamusCS;
        }

        public void Greeting(string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("greeting" + Guid.NewGuid().ToString(), "sessionStart", "greeting", new string[] { "|player|" }, new string[] { player });
        }

        public void GameStart(string puzzle)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("start" + Guid.NewGuid().ToString(), "sessionStart", "puzzle", new string[] { "|puzzle|" }, new string[] { puzzle });
        }

        public void NextGame(string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("nextGame" + Guid.NewGuid().ToString(), "sessionStart", "next_game", new string[] { "|player|" }, new string[] { player });
        }

        public void FingerHelp()
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("fingerHelp" + Guid.NewGuid().ToString(), "sessionStart", "finger_help", new string[] { }, new string[] { });
        }

        public void ButtonHelp()
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("buttonHelp" + Guid.NewGuid().ToString(), "sessionStart", "button_help", new string[] { }, new string[] { });
        }

        public void Win(string puzzle, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("win" + Guid.NewGuid().ToString(), "sessionEnd", "win", new string[] { "|puzzle|", "|player|" }, new string[] { puzzle, player });
        }

        public void FastWin(string puzzle, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("fastWin" + Guid.NewGuid().ToString(), "sessionEnd", "fast_win", new string[] { "|puzzle|", "|player|" }, new string[] { puzzle, player });
        }

        public void Quit(string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("quit" + Guid.NewGuid().ToString(), "sessionEnd", "quit", new string[] { "|player|" }, new string[] { player });
        }

        public void MotorHelp()
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("motorHelp" + Guid.NewGuid().ToString(), "help", "motor_help", new string[] { }, new string[] { });
        }

        public void CloseHelp()
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("closeHelp" + Guid.NewGuid().ToString(), "help", "close_help", new string[] { }, new string[] { });
        }

        public void PositiveFeedback(string nPieces, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("positiveFeedback" + Guid.NewGuid().ToString(), "feedback", "positive", new string[] { "|npieces|", "|player|" }, new string[] { nPieces.ToString(), player });
        }

        public void NegativeFeedback()
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("negativeFeedback" + Guid.NewGuid().ToString(), "feedback", "negative", new string[] { }, new string[] { });
        }

        public void FirstAnglePrompt(string piece, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("firstAnglePrompt" + Guid.NewGuid().ToString(), "anglePrompt", "first", new string[] { "|piece|", "|player|" }, new string[] { piece, player });
        }

        public void FirstAnglePromptFinger(string piece, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("firstAnglePromptFinger" + Guid.NewGuid().ToString(), "anglePrompt", "first_finger", new string[] { "|piece|", "|player|" }, new string[] { piece, player });
        }

        public void FirstAnglePromptButton(string piece, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("firstAnglePromptButton" + Guid.NewGuid().ToString(), "anglePrompt", "first_button", new string[] { "|piece|", "|player|" }, new string[] { piece, player });
        }

        public void SecondAnglePrompt(string piece, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("secondAnglePrompt" + Guid.NewGuid().ToString(), "anglePrompt", "sec", new string[] { "|piece|", "|player|" }, new string[] { piece, player });
        }

        public void SecondAnglePromptFinger(string piece, string direction, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("secondAnglePromptFinger" + Guid.NewGuid().ToString(), "anglePrompt", "sec_finger", new string[] { "|piece|", "|direction|", "|player|" }, new string[] { piece, direction, player });
        }

        public void SecondAnglePromptButton(string piece, string nClicks, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("secondAnglePromptButton" + Guid.NewGuid().ToString(), "anglePrompt", "sec_button", new string[] { "|piece|", "|nclicks|", "|player|" }, new string[] { piece, nClicks.ToString(), player });
        }

        public void ThirdAnglePrompt(string piece, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("thirdAnglePrompt" + Guid.NewGuid().ToString(), "anglePrompt", "third", new string[] { "|piece|", "|player|" }, new string[] { piece, player });
        }

        public void StopAnglePrompt(string piece)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("stopAnglePrompt" + Guid.NewGuid().ToString(), "anglePrompt", "stop", new string[] { "|piece|" }, new string[] { piece });
        }

        public void FirstIdlePrompt(string piece, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("firstIdlePrompt" + Guid.NewGuid().ToString(), "prompt", "idle", new string[] { "|piece|", "|player|" }, new string[] { piece, player });
        }

        public void FirstPlacePrompt(string piece, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("firstPlacePrompt" + Guid.NewGuid().ToString(), "prompt", "place", new string[] { "|piece|", "|player|" }, new string[] { piece, player });
        }

        public void SecondPrompt1Position(string piece, string pos, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("secondPrompt1Position" + Guid.NewGuid().ToString(), "prompt", "sec_1position", new string[] { "|piece|", "|pos|", "|player|" }, new string[] { piece, pos, player });
        }

        public void SecondPrompt2Position(string piece, string pos1, string pos2, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("secondPrompt2Position" + Guid.NewGuid().ToString(), "prompt", "sec_2position", new string[] { "|piece|", "|pos1|", "|pos2|", "|player|" }, new string[] { piece, pos1, pos2, player });
        }

        public void SecondPromptPlace(string piece, string pos, string relativePiece, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("secondPromptPlace" + Guid.NewGuid().ToString(), "prompt", "sec_place", new string[] { "|piece|", "|pos|", "|relativepiece|", "|player|" }, new string[] { piece, pos, relativePiece, player });
        }

        public void ThirdPrompt(string piece, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("thirdPrompt" + Guid.NewGuid().ToString(), "prompt", "third", new string[] { "|piece|", "|player|" }, new string[] { piece, player });
        }

        public void HardClue(string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("hardClue" + Guid.NewGuid().ToString(), "prompt", "hard_clue", new string[] { "|player|" }, new string[] { player });
        }

        public void PGreeting(string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pGreeting" + Guid.NewGuid().ToString(), "pSessionStart", "greeting", new string[] { "|player|" }, new string[] { player });
        }

        public void PButtonHelp()
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pButtonHelp" + Guid.NewGuid().ToString(), "pSessionStart", "rot_button", new string[] { }, new string[] { });
        }

        public void PFingerHelp()
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pFingerHelp" + Guid.NewGuid().ToString(), "pSessionStart", "rot_finger", new string[] { }, new string[] { });
        }

        public void PWin(string puzzle, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pWin" + Guid.NewGuid().ToString(), "pSessionEnd", "win", new string[] { "|puzzle|", "|player|" }, new string[] { puzzle, player });
        }

        public void PChildTurn(string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pChildTurn" + Guid.NewGuid().ToString(), "pTurn", "child", new string[] { "|player|" }, new string[] { player });
        }

        public void PRobotTurn()
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pRobotTurn" + Guid.NewGuid().ToString(), "pTurn", "robot", new string[] { }, new string[] { });
        }

        public void PRobotDrag()
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pRobotDrag" + Guid.NewGuid().ToString(), "pTurn", "robot_drag", new string[] { }, new string[] { });
        }

        public void PRobotRotDrag()
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pRobotRotDrag" + Guid.NewGuid().ToString(), "pTurn", "robot_rot_drag", new string[] { }, new string[] { });
        }

        public void PRobotWin(string nPieces, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pRobotWin" + Guid.NewGuid().ToString(), "pTurn", "robot_win", new string[] { "|npieces|", "|player|" }, new string[] { nPieces.ToString(), player });
        }

        public void PRobotReminder()
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pRobotReminder" + Guid.NewGuid().ToString(), "pTurn", "robot_reminder", new string[] { }, new string[] { });
        }

        public void PAskingPlace(string piece, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pAskingPlace" + Guid.NewGuid().ToString(), "pHelp", "asking_place", new string[] { "|piece|", "|player|" }, new string[] { piece, player });
        }

        public void PAskingRotate(string piece, string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pAskingRotate" + Guid.NewGuid().ToString(), "pHelp", "asking_rotate", new string[] { "|piece|", "|player|" }, new string[] { piece, player });
        }

        public void PAskingPlaceWin(string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pAskingPlaceWin" + Guid.NewGuid().ToString(), "pHelp", "asking_place_win", new string[] { "|player|" }, new string[] { player });
        }

        public void PAskingRotateWin(string player)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pAskingRotateWin" + Guid.NewGuid().ToString(), "pHelp", "asking_rotate_win", new string[] { "|player|" }, new string[] { player });
        }

        public void PAskingPlaceWrong(string piece)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pAskingPlaceWrong" + Guid.NewGuid().ToString(), "pHelp", "asking_place_wrong", new string[] { "|piece|" }, new string[] { piece });
        }

        public void PAskingRotateWrong(string piece)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pAskingRotateWrong" + Guid.NewGuid().ToString(), "pHelp", "asking_rotate_wrong", new string[] { "|piece|" }, new string[] { piece });
        }

        public void PAskingQuit()
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pAskingQuit" + Guid.NewGuid().ToString(), "pHelp", "asking_quit", new string[] { }, new string[] { });
        }

        public void PGivingPlace(string piece, string pos)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pGivingPlace" + Guid.NewGuid().ToString(), "pHelp", "giving_place", new string[] { "|piece|", "|pos|" }, new string[] { piece, pos });
        }

        public void PGivingRotate(string piece)
        {
            _thalamusCS.TypifiedPublisher.PerformUtteranceFromLibrary("pGivingRotate" + Guid.NewGuid().ToString(), "pHelp", "giving_rotate", new string[] { "|piece|" }, new string[] { piece });
        }

        public void WriteJSON(string timestamp, string info) 
        {

            /********************************
             */
            string filePath = "c:";
            Console.WriteLine(filePath);
            try
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                    Console.WriteLine(filePath);
                }

            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            /********************************
            */

            if (_thalamusCS.fileName == null)
            {
                _thalamusCS.fileName = filePath + @"\Developer\Logs\" + String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now) + ".txt";
                Console.WriteLine(filePath + @"\" + String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now) + ".txt");
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(_thalamusCS.fileName))
                {

                    file.WriteLine(timestamp + " " + info);
                }
            }
            else {
                using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(_thalamusCS.fileName, true))
                {

                    file.WriteLine(timestamp + " " + info);
                }
            }
        }

        public void CancelUtterance(string id) {
            _thalamusCS.TypifiedPublisher.CancelUtterance(id);
        }

        /*public void GlanceAtScreen(double x, double y)
        {
            _thalamusCS.TypifiedPublisher.GlanceAtScreen(x, y);
        }

        public void GazeAtScreen(double x, double y)
        {
            _thalamusCS.TypifiedPublisher.GazeAtScreen(x, y);
        }*/

        public void Dispose()
        {
        }
    }
}
