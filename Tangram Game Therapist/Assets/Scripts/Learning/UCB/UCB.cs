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

        private List<Elements> Rewards = new List<Elements>();

        private int actionSelected = 0;
        private string fileName = null;
        private int iterations = 1;
        private int number_actions;
        private double[] reward_actions;
        private int[] random_actions;
        int id = 0;

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
            reward_actions = new double[numAct];
        }

        public int RunUCB()
        {
            if (iterations == 1)
            {
                random_actions = new int[number_actions];
                id = 0;
                Console.WriteLine("All numbers between 0 and 4 in random order:");
                foreach (int i in UniqueRandom(0, (number_actions - 1)))
                {
                    Console.WriteLine(i);
                    random_actions[id] = i;
                    id++;
                }
                id = 0;
            }


            if (random_actions.Length > 0)// run all the actions once and update all the rewards before moving for the algorithm generation
            {
                if (id >= random_actions.Length)//did all actions once but some did not received feedback show them again
                {
                    id = 0;
                }

                actionSelected = random_actions[id];

                id++;

                UpdateActionPlayed(actionSelected);

                if (iterations == 1)
                {
                    WriteJSON("", "DATE/TIME;PLAYER;PUZZLE;DIFICULDADE;MODO_ROTAÇAO;THRESHOLD;ACTION;TIME_t;UCB;AVG_REWARD;PLAYED_ACTIONS;SELECTED_ACTION");
                }

                WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), ";" + GameManager.Instance.playerName + ";" + GameManager.Instance.CurrentPuzzle + ";" + GameManager.Instance.Difficulty_ + ";" + GameManager.Instance.RotationMode_ + ";" + GameManager.Instance.DistanceThreshold + ";-" + ";" + iterations + ";-" + ";-" + ";-" + ";" + actionSelected);

                Console.WriteLine("------------------- iterations = " + iterations + ", actionSelected = " + actionSelected + " -------------------");
            }
            else // all actions were already run once, know they can be run acording the algorithm selection
            {

                for (int i = 0; i < number_actions; i++)
                {
                    double aux_1 = 2 * Math.Log(iterations);
                    Elements play_action = PlayedActions.Find(x => (x.Action == i));
                    Elements avgReward = AvgReceivedRewards.Find(x => (x.Action == i));


                    // a(t) = argmax ^ri + Sqrt(2 ln t/ ti)
                    double A_t_aux = Math.Sqrt(aux_1 / play_action.Value) + avgReward.Value;
                    //Math.Round(A_t_aux, 4); // convert double into 4 decimal places
                    A_t.Add(new Elements(iterations, i, Math.Round(A_t_aux, 4)));
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

                actionSelected = Convert.ToInt32(aux_max[0]);

                //update number of times action i was played
                UpdateActionPlayed(actionSelected);

                //observe

                SaveData();
            }

            //update to next iteration
            iterations++;
            return actionSelected;
        }

        private void UpdateActionPlayed(int actionSelected)
        {
            Elements _action = PlayedActions.Find(x => (x.Action == actionSelected));
            if (_action == null)
            {
                PlayedActions.Add(new Elements(actionSelected, 1.0f));
            }
            else
            {
                _action.Value = _action.Value + 1;
            }

        }


        // Observe the reward and update the empirical mean for the chosen action.
        public void UpdateReward(int action, double reward_rating)
        {
            float A = 1f, B = 5f;
            double res_reward = 0.0f;

            if (reward_rating <= A)
            {
                res_reward = 0.1f;
            }
            else if (reward_rating > A && reward_rating < B)
            {
                res_reward = (reward_rating - A) / (B - A);
            }
            else // reward_rating >= B
            {
                res_reward = 0.9f;
            }

            reward_actions[action] = Math.Round(res_reward, 2);

            //update average reward of action i
            UpdateAvgReward(action);

            if (random_actions.Length > 0)
            {
                //remove elements from the array with the initial random_actions that received feedback
                random_actions = RemoveElement(action);
            }

        }

        private int[] RemoveElement(int action)
        {
            int[] aux = new int[random_actions.Length - 1];
            int ids = 0;
            for (int i = 0; i < random_actions.Length; i++)
            {
                if (random_actions[i] != action)
                {
                    aux[ids] = random_actions[i];
                    ids++;
                }
            }

            return aux;
        }

        private void UpdateAvgReward(int action)
        {
            ///////

            Elements avgReward = AvgReceivedRewards.Find(x => (x.Action == action));
            //update the action reward
            Elements played_action = PlayedActions.Find(x => (x.Action == action));

            //   avgReward = old_avg + (newval - old_avg)/numplayactions
            //https://math.stackexchange.com/questions/106700/incremental-averageing

            double aux;
            if (avgReward == null)//receives the first reward
            {
                aux = reward_actions[action];
                AvgReceivedRewards.Add(new Elements(action, aux));
            }
            else
            {
                aux = (avgReward.Value + ((reward_actions[action] - avgReward.Value) / played_action.Value));
                //Math.Round(aux, 2); // convert double into 2 decimal places
                avgReward.Value = Math.Round(aux, 4);
            }
        }

        private void SaveData()
        {
            if (iterations == number_actions)
            {
                WriteJSON("", "DATE/TIME;PLAYER;PUZZLE;DIFICULDADE;MODO_ROTAÇAO;THRESHOLD;ACTION;TIME_t;UCB;AVG_REWARD;PLAYED_ACTIONS;SELECTED_ACTION");
            }

            for (int i = 0; i < number_actions; i++)
            {
                Elements _A_t = A_t.Find(x => (x.Time_index == iterations && x.Action == i));
                Elements _playedActions = PlayedActions.Find(x => (x.Action == i));
                Elements _avgRewards = AvgReceivedRewards.Find(x => (x.Action == i));

                WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), ";" + GameManager.Instance.playerName + ";" + GameManager.Instance.CurrentPuzzle + ";" + GameManager.Instance.Difficulty_ + ";" + GameManager.Instance.RotationMode_ + ";" + GameManager.Instance.DistanceThreshold + ";" +
                    i + ";" + iterations + ";" + _A_t + ";" + _avgRewards + ";" + _playedActions + ";" + actionSelected);

            }
        }

        /// <summary>
        /// Returns all numbers, between min and max inclusive, once in a random sequence.
        /// </summary>
        IEnumerable<int> UniqueRandom(int minInclusive, int maxInclusive)
        {
            List<int> candidates = new List<int>();
            for (int i = minInclusive; i <= maxInclusive; i++)
            {
                candidates.Add(i);
            }
            Random rnd = new Random();
            while (candidates.Count > 0)
            {
                int index = rnd.Next(candidates.Count);
                yield return candidates[index];
                candidates.RemoveAt(index);
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
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
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
