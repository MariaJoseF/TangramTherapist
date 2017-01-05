using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Exp3
{
    class Exp3
    {
        private List<Elements> Weights = new List<Elements>();

        private List<Elements> Probabilities = new List<Elements>();
        private List<Elements> EstimatedRewards = new List<Elements>();
        private List<Elements> Rewards = new List<Elements>();
        private int action = 0;
        int iterations = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num_actions"></param>
        /// <param name="reward_actions"></param>
        /// <param name="gamma"></param>
        /// <returns></returns>
        public int RunExp3(int num_actions, float[] reward_actions, float gamma)
        {

            //initialize all weights to 1
            //Wi(1)= 1, for i=1,....,n
            if (iterations == 0)
            {
                for (int i = 0; i < num_actions; i++)
                {
                    Weights.Add(new Elements(iterations, i, 1.0f));
                    //Weights[i, 0] = 1.0f;
                }
            }
           

            float sum_weights = 0.0f;
            //At time step t -> for each round t
            //for (int t = 0; t < iterations; t++)
            //{
            Debug.Log(" ------ t = " + iterations + " ------");
            sum_weights = SumWeights(num_actions, iterations);
            //for each action i, set
            Elements _weight;
            float prob;
            for (int i = 0; i < num_actions; i++)
            {
                //pi(t) = (1-gamma) *[(Wi(t)/(Sum of i until n Wi(t))]+(gamma/n) 


                // parts.Find(x => x.PartName.Contains("seat")));

                _weight = Weights.Find(x => (x.Time_index == iterations && x.Element == i));

                prob = (1 - gamma) * (_weight.Value / sum_weights) + (gamma / num_actions);
                Probabilities.Add(new Elements(iterations, i, prob));
                Debug.Log("     Probabilities[i, t] = " + Probabilities.LastIndexOf(new Elements(iterations, i, prob)).ToString());
            }

            //Randomly draw an action according to p1(t), ...., pn(t)
            action = RandomPickAction(num_actions, iterations);
            Debug.Log("     Action number = " + action);
            //observe the reward r(t)

            //Rewards[action, t] = reward_actions[action];
            Rewards.Add(new Elements(iterations, action, reward_actions[action]));

            //update estimated reward   ^ri(t) 

            Elements _weight_plus_one;
            Elements _EstRewards;
            Elements _reward;
            Elements _probabilities;
            Elements reward_prob;

            for (int j = 0; j < num_actions; j++)
            {
                //if a(t)=i
                if (action == j)
                {
                    //^ri(t)  = r(t)/pi(t)
                    // EstimatedRewards[j, t] = Rewards[j, t] / Probabilities[j, t];
                    _reward = Rewards.Find(x => (x.Time_index == iterations && x.Element == j));
                    _probabilities = Probabilities.Find(x => (x.Time_index == iterations && x.Element == j));
                    reward_prob = new Elements(iterations, j, (_reward.Value / _probabilities.Value));
                    EstimatedRewards.Add(reward_prob);

                    Debug.Log("     EstimatedRewards[j, t]  = " + EstimatedRewards.LastIndexOf(reward_prob).ToString());

                    //Update weights
                    float ga_act = (gamma / num_actions);
                    _EstRewards = EstimatedRewards.Find(x => (x.Time_index == iterations && x.Element == action));
                    float e_power = (float)Math.Exp(ga_act * _EstRewards.Value);

                    //Wt+1(at) = Wt(at) e^(n*^rat(t))
                    _weight = Weights.Find(x => (x.Time_index == iterations && x.Element == action));
                    //Weights[action, t + 1] = _weights_action.Value * e_power;
                    _weight_plus_one = new Elements(iterations + 1, action, (_weight.Value * e_power));
                    Weights.Add(_weight_plus_one);
                }
                else
                {
                    // ^ri(t) =0
                    //EstimatedRewards[j, t] = 0.0f;
                    EstimatedRewards.Add(new Elements(iterations, j, 0.0f));
                    //Wt+1(j) = Wt(j)
                    _weight = Weights.Find(x => (x.Time_index == iterations && x.Element == j));
                    _weight_plus_one = new Elements(iterations + 1, action, (_weight.Value));
                    Weights.Add(_weight_plus_one);
                }

                Debug.Log("     Weights[action, t + 1] = " + Weights.LastIndexOf(_weight_plus_one).ToString());
            }
            //}

            //////////////////////
            String text = "";
            for (int i = 0; i < num_actions; i++)
            {
                //Debug.Log(" action = " + i);
                _weight = Weights.Find(x => (x.Time_index == iterations && x.Element == i));
                _probabilities = Probabilities.Find(x => (x.Time_index == iterations && x.Element == i));
                _EstRewards = EstimatedRewards.Find(x => (x.Time_index == iterations && x.Element == i));
                _weight_plus_one = Weights.Find(x => (x.Time_index == iterations + 1 && x.Element == i));

                if (_weight != null)
                {
                    text = "Weights[" + i + ", " + iterations + "] = " + _weight.Value;
                }

                if (_probabilities != null)
                {
                    text = text + "     Probabilities[" + i + ", " + iterations + "] = " + _probabilities.Value;
                }

                if (_EstRewards != null)
                {
                    text = text + "     EstimatedRewards[" + i + ", " + iterations + "]  = " + _EstRewards.Value;
                }

                if (_weight_plus_one != null)
                {
                    text = text + "     Weights[" + i + ", " + (iterations + 1) + "] = " + _weight_plus_one.Value;
                }

                //text = "Weights[" + i + ", " + iterations + "] = " + _weight.Value +
                //    "     Probabilities[" + i + ", " + iterations + "] = " + _probabilities.Value +
                //    "     EstimatedRewards[" + i + ", " + iterations + "]  = " + _EstRewards.Value +
                //    "     Weights[" + i + ", " + (iterations + 1) + "] = " + _weight_plus_one.Value;

                Debug.Log(text);
            }
            //////////////////////
            iterations++;

            Debug.Log(" ------------");
            return action;
        }


        //using Cumulative Distribution Function - CDF to draw the action based on the probabilities
        private int RandomPickAction(int actions_num, int time_)
        {
            int selected_action = 0;

            System.Random rnd = new System.Random();
            float dou_ = (float)rnd.NextDouble();
            Debug.Log("         Random val = " + dou_);

            float sum_prob = 0.0f;
            Elements _probabilities;
            for (int i = 0; i < actions_num; i++)
            {
                selected_action = i;
                _probabilities = Probabilities.Find(x => (x.Time_index == time_ && x.Element == i));
                sum_prob = sum_prob + _probabilities.Value;
                Debug.Log("         sum_prob = " + sum_prob);
                if (sum_prob >= dou_)
                {
                    break;
                }
            }

            return selected_action;
        }

        private float SumWeights(int actions_num, int time_)
        {
            float sum_weights = 0.0f;
            Elements w;
            for (int i = 0; i < actions_num; i++)
            {
                w = Weights.Find(x => (x.Time_index == time_ && x.Element == i));
                sum_weights = sum_weights + w.Value;
            }
            return sum_weights;
        }
    }
}
