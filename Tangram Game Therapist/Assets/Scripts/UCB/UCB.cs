using System;
using System.Collections.Generic;
using System.IO;


namespace Assets.Scripts.UCB
{
    class UCB
    {

        private List<Elements> PlayedActions = new List<Elements>();
        private List<Elements> AvgReceivedRewards = new List<Elements>();
        private List<Elements> A_t = new List<Elements>();
        private int actionSelected = 0;
        private string fileName = null;
        private int iterations = 0;
        private int number_actions;
        private float[] reward_actions;

        public int Action
        {
            get
            {
                return actionSelected;
            }

            set
            {
                actionSelected = value;
            }
        }

        public UCB(int numAct, float[] reward_actions_)
        {
            number_actions = numAct;
            reward_actions = reward_actions_;
        }

        public int RunUCB()
        {
            if (iterations == 0)
            {
                for (int i = 0; i < number_actions; i++)
                {
                    PlayedActions.Add(new Elements(i, 1.0f));
                    AvgReceivedRewards.Add(new Elements(i, reward_actions[i]));
                }
            }

            for (int i = 0; i < number_actions; i++)
            {
                double aux_1 = 2 * Math.Log(iterations);
                Elements play_action = PlayedActions.Find(x => (x.Action == i));
                Elements avgReward = AvgReceivedRewards.Find(x => (x.Action == i));


                // a(t) = argmax ^ri + Sqrt(2 ln t/ ti)
                double A_t_aux = Math.Sqrt(aux_1 / play_action.Value) + (avgReward.Value / play_action.Value);
                A_t.Add(new Elements(iterations, i, A_t_aux));
            }

            //find the argmax action
            double[] aux_max = new double[2];

            foreach (Elements item in A_t)
            {
                if (item.Time_index == iterations)
                {
                    if (item.Action == 0 || (item.Value >= aux_max[1]))
                    {
                        aux_max[0] = item.Action;
                        aux_max[1] = item.Value;
                    }
                }
            }

            actionSelected = Convert.ToInt32(aux_max[1]);

            //update number of times action i was played
            Elements _action = PlayedActions.Find(x => (x.Action == actionSelected));
            _action.Value = _action.Value + 1;

            //update average reward of action i
            Elements _reward = AvgReceivedRewards.Find(x => (x.Action == actionSelected));
            _reward.Value = _reward.Value + reward_actions[actionSelected];



            SaveData();
            //update to next iteration
            iterations++;
            return actionSelected;
        }

        private void SaveData()
        {
            if (iterations == 0)
            {
                WriteJSON("", "DATE/TIME;PLAYER;PUZZLE;DIFICULDADE;MODO_ROTAÇAO;THRESHOLD;ACTION;TIME_t;UCB;AVG_REWARD;PLAYED_ACTIONS;SELECTED_ACTION");
            }

            for (int i = 0; i < number_actions; i++)
            {
                Elements _A_t = A_t.Find(x => (x.Time_index == iterations && x.Action == i));
                Elements _playedActions = PlayedActions.Find(x => (x.Time_index == iterations && x.Action == i));
                Elements _avgRewards = AvgReceivedRewards.Find(x => (x.Time_index == iterations && x.Action == i));

                WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), ";" + GameManager.Instance.playerName + ";" + GameManager.Instance.CurrentPuzzle + ";" + GameManager.Instance.Difficulty_ + ";" + GameManager.Instance.RotationMode_ + ";" + GameManager.Instance.DistanceThreshold + ";" +
                    i + ";" + iterations + ";" + _A_t.Value + ";" + _avgRewards.Value + ";" + _playedActions.Value + ";" + actionSelected);

            }
        }

        private void WriteJSON(string timestamp, string info)
        {

            string filePath = @"c:\Developer\Logs\UCB\";
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

            if (fileName == null)
            {
                fileName = filePath + @"ucb_" + String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now) + ".txt";
                Console.WriteLine(filePath + @"\" + String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now) + ".txt");
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(fileName))
                {

                    file.WriteLine(timestamp + " " + info);
                }
            }
            else
            {
                using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(fileName, true))
                {

                    file.WriteLine(timestamp + " " + info);
                }
            }
        }
    }
}
