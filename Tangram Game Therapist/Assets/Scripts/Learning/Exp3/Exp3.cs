using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace Assets.Scripts.Exp3
{
    class Exp3
    {
        private List<Elements> Weights = new List<Elements>();

        private List<Elements> Probabilities = new List<Elements>();
        private List<Elements> EstimatedRewards = new List<Elements>();
        private List<Elements> Rewards = new List<Elements>();
        private int action = 0;
        private int iterations = 0;
        private string fileName = "";
        private string utterance_name = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num_actions"></param>
        /// <param name="reward_actions"></param>
        /// <param name="gamma"></param>
        /// <returns></returns>
        /// 

        private int num_actions;
        private double[] reward_actions;
        private float gamma;

        public int Action
        {
            get
            {
                return action;
            }

            set
            {
                action = value;
            }
        }

        public Exp3(int num_actions_, double[] reward_actions_, float gamma_)
        {
            num_actions = num_actions_;
            reward_actions = reward_actions_;
            gamma = gamma_;
        }


        public int RunExp3(string name_utterance)
        {
            utterance_name = name_utterance;

            //initialize all weights to 1
            //Wi(1)= 1, for i=1,....,n
            if (iterations == 0)
            {
                for (int i = 0; i < num_actions; i++)
                {
                    //Weights[i, 0] = 1.0f;
                    Weights.Add(new Elements(iterations, i, 1.0f));
                    //             WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), "Weights[" + i + ", " + iterations + "] = " + Weights.Find(x => (x.Time_index == iterations && x.Element == i)));

                }
            }


            double sum_weights = 0.0f;
            //At time step t -> for each round t

            //      WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), " ------ t = " + iterations + " ------");

            sum_weights = SumWeights(num_actions, iterations);
            //for each action i, set
            Elements _weight;
            double prob;
            for (int i = 0; i < num_actions; i++)
            {
                //pi(t) = (1-gamma) *[(Wi(t)/(Sum of i until n Wi(t))]+(gamma/n); 

                _weight = Weights.Find(x => (x.Time_index == iterations && x.Element == i));

                prob = (1 - gamma) * (_weight.Value / sum_weights) + (gamma / num_actions);
                Probabilities.Add(new Elements(iterations, i, prob));

                //     WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), "     Probabilities[" + i + ", " + iterations + "] = " + Probabilities.Find(x => (x.Time_index == iterations && x.Element == i)));

            }

            //Randomly draw an action according to p1(t), ...., pn(t)
            Action = RandomPickAction(num_actions, iterations);

            //  WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), "     Action number = " + Action);

            //observe the reward r(t)
            //Rewards[action, t] = reward_actions[action];
            Rewards.Add(new Elements(iterations, Action, reward_actions[Action]));

            //WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), "Rewards[" + Action + ", " + iterations + "] = " + Rewards.Find(x => (x.Time_index == iterations && x.Element == Action)));
            //update estimated reward   ^ri(t) 

            Elements _weight_plus_one;
            Elements _EstRewards;
            Elements _reward;
            Elements _probabilities;
            Elements reward_prob;

            for (int j = 0; j < num_actions; j++)
            {
                //if a(t)=i
                if (Action == j)
                {
                    //^ri(t)  = r(t)/pi(t)
                    // EstimatedRewards[j, t] = Rewards[j, t] / Probabilities[j, t];
                    _reward = Rewards.Find(x => (x.Time_index == iterations && x.Element == j));
                    _probabilities = Probabilities.Find(x => (x.Time_index == iterations && x.Element == j));
                    reward_prob = new Elements(iterations, j, (_reward.Value / _probabilities.Value));
                    EstimatedRewards.Add(reward_prob);

                    //Update weights
                    float ga_act = (gamma / num_actions);
                    _EstRewards = EstimatedRewards.Find(x => (x.Time_index == iterations && x.Element == j));
                    float e_power = (float)Math.Exp(ga_act * _EstRewards.Value);

                    //Wt+1(at) = Wt(at) e^(n*^rat(t))
                    _weight = Weights.Find(x => (x.Time_index == iterations && x.Element == j));
                    //Weights[action, t + 1] = _weights_action.Value * e_power;
                    _weight_plus_one = new Elements(iterations + 1, j, (_weight.Value * e_power));
                    Weights.Add(_weight_plus_one);
                }
                else
                {
                    // ^ri(t) =0
                    //EstimatedRewards[j, t] = 0.0f;
                    EstimatedRewards.Add(new Elements(iterations, j, 0.0f));

                    //Wt+1(j) = Wt(j)
                    _weight = Weights.Find(x => (x.Time_index == iterations && x.Element == j));
                    _weight_plus_one = new Elements(iterations + 1, j, (_weight.Value));
                    Weights.Add(_weight_plus_one);
                }

                //    WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), "     EstimatedRewards[" + j + ", " + iterations + "] = " + EstimatedRewards.Find(x => (x.Time_index == iterations && x.Element == j)));
                // WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), "     Weights[" + j + ", " + (iterations + 1) + "] = " + Weights.Find(x => (x.Time_index == (iterations + 1) && x.Element == j)));

            }

            SaveData();
            iterations++;

            return Action;
        }

        private void SaveData()
        {
            if (iterations == 0)
            {
                WriteJSON("", "DATE/TIME;PLAYER;PUZZLE;DIFICULDADE;MODO_ROTAÇAO;THRESHOLD;ACTION;TIME_t;WEIGHT;PROBABILITY;ESTIMATED_REWARD;SELECTED_ACTION;UTTERANCE_NAME");
            }

            for (int i = 0; i < num_actions; i++)
            {
                Elements weigth = Weights.Find(x => (x.Time_index == iterations && x.Element == i));
                Elements probability = Probabilities.Find(x => (x.Time_index == iterations && x.Element == i));
                Elements estimeted_reward = EstimatedRewards.Find(x => (x.Time_index == iterations && x.Element == i));

                WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), ";" + GameManager.Instance.playerName + ";" + GameManager.Instance.CurrentPuzzle + ";" + GameManager.Instance.Difficulty_ + ";" + GameManager.Instance.RotationMode_ + ";" + GameManager.Instance.DistanceThreshold + ";" +
                 i + ";" + iterations + ";" + weigth + ";" + probability + ";" + estimeted_reward + ";" + Action + ";" +utterance_name);
            }
        }


        //using Cumulative Distribution Function - CDF to draw the action based on the probabilities
        private int RandomPickAction(int actions_num, int time_)
        {
            int selected_action = 0;

            System.Random rnd = new System.Random();
            float dou_ = (float)rnd.NextDouble();
            Debug.Log("         Random val = " + dou_);

            double sum_prob = 0.0f;
            Elements _probabilities;
            for (int i = 0; i < actions_num; i++)
            {
                selected_action = i;
                _probabilities = Probabilities.Find(x => (x.Time_index == time_ && x.Element == i));
                sum_prob = sum_prob + _probabilities.Value;
                // Debug.Log("         sum_prob = " + sum_prob);
                if (sum_prob >= dou_)
                {
                    break;
                }
            }

            return selected_action;
        }

        private double SumWeights(int actions_num, int time_)
        {
            double sum_weights = 0.0f;
            Elements w;
            for (int i = 0; i < actions_num; i++)
            {
                w = Weights.Find(x => (x.Time_index == time_ && x.Element == i));
                sum_weights = sum_weights + w.Value;
            }
            return sum_weights;
        }

        private void WriteJSON(string timestamp, string info)
        {

            string filePath = @"c:\Developer\Logs\EXP3\";
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
                fileName = filePath + @"exp3_" + String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now) + ".txt";
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

        internal void UpdateReward(int previousAction, double reward_rating)
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

            //reward_actions[action] = Math.Round(res_reward, 2);
            reward_actions[previousAction] = Math.Round(res_reward, 2);

        }

    }
}
