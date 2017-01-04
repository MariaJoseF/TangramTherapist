using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Exp3
{
    class Exp3
    {
        private List<Elements> Weights;

        private List<Elements> Probabilities;
        private List<Elements> EstimatedRewards;
        private List<Elements> Rewards;
        private int action = 0;
        int iterations = 0;

        public int RunExp3(int num_actions, float[] reward_actions, float gamma)
        {

            Weights = new List<Elements>();
            Probabilities = new List<Elements>();
            EstimatedRewards = new List<Elements>();
            Rewards = new List<Elements>();

            //initialize all weights to 1
            //Wi(1)= 1, for i=1,....,n
            for (int i = 0; i < num_actions; i++)
            {
                Weights.Add(new Elements(iterations, i, 1.0f));
                //Weights[i, 0] = 1.0f;
            }

            float sum_weights = 0.0f;
            //At time step t -> for each round t
            for (int t = 0; t < iterations; t++)
            {
                Debug.Log(" ------ t = " + t + " ------");
                sum_weights = SumWeights(num_actions, t);
                //for each action i, set
                for (int i = 0; i < num_actions; i++)
                {
                    //pi(t) = (1-gamma) *[(Wi(t)/(Sum of i until n Wi(t))]+(gamma/n) 


                    // parts.Find(x => x.PartName.Contains("seat")));

                    Elements _weight = Weights.Find(x => x.Time_index.Equals(iterations) && x.Element.Equals(i));

                    float prob = (1 - gamma) * (_weight.Value / sum_weights) + (gamma / num_actions);
                    Probabilities.Add(new Elements(iterations, i, prob));
                    Debug.Log("     Probabilities[i, t]");
                }

                //Randomly draw an action according to p1(t), ...., pn(t)
                action = RandomPickAction(num_actions, t);
                Debug.Log("     Action number = " + action);
                //observe the reward r(t)

                //Rewards[action, t] = reward_actions[action];
                Rewards.Add(new Elements(iterations, action, reward_actions[action]));

                //update estimated reward   ^ri(t) 
                for (int j = 0; j < num_actions; j++)
                {
                    //if a(t)=i
                    if (action == j)
                    {
                        //^ri(t)  = r(t)/pi(t)
                        // EstimatedRewards[j, t] = Rewards[j, t] / Probabilities[j, t];
                        Elements _reward = Rewards.Find(x => x.Time_index.Equals(iterations) && x.Element.Equals(j));
                        Elements _probabilities = Probabilities.Find(x => x.Time_index.Equals(iterations) && x.Element.Equals(j));
                        Elements reward_prob = new Elements(iterations, j, (_reward.Value / _probabilities.Value));
                        EstimatedRewards.Add(reward_prob);

                        Debug.Log("     EstimatedRewards[j, t]  = " + EstimatedRewards.LastIndexOf(reward_prob));
                    }
                    else
                    {
                        // ^ri(t) =0
                        //EstimatedRewards[j, t] = 0.0f;
                        EstimatedRewards.Add(new Elements(iterations, j, 0.0f));
                    }

                    //Update weights
                    float ga_act = (gamma / num_actions);
                    Elements _EstRewards = EstimatedRewards.Find(x => x.Time_index.Equals(iterations) && x.Element.Equals(action));
                    float e_power = (float)Math.Exp(ga_act * _EstRewards.Value);

                    //Wt+1(at) = Wt(at) e^(n*^rat(t))
                    Elements _weights_action = Weights.Find(x => x.Time_index.Equals(iterations) && x.Element.Equals(action));
                    //Weights[action, t + 1] = _weights_action.Value * e_power;
                    Elements _weight_plus_one = new Elements(iterations + 1, action, (_weights_action.Value * e_power));
                    Weights.Add(_weight_plus_one);

                    Debug.Log("     Weights[action, t + 1] = " + Weights.LastIndexOf(_weight_plus_one));
                }
            }
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
            for (int i = 0; i < actions_num; i++)
            {
                selected_action = i;
                Elements _probabilities = Probabilities.Find(x => x.Time_index.Equals(time_) && x.Element.Equals(i));
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
            for (int i = 0; i < actions_num; i++)
            {
                Elements w = Weights.Find(x => x.Time_index.Equals(time_) && x.Element.Equals(i));
                sum_weights = sum_weights + w.Value;
            }
            return sum_weights;
        }
    }
}
