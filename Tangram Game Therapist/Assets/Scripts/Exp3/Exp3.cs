using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace Assets.Scripts.Exp3
{
    class Exp3
    {
        private float[,] Weights_;

        private List<Elements> Weights;

        private float[,] Probabilities;
        private float[,] EstimatedRewards;
        private float[,] Rewards;
        private int action = 0;

        public int RunExp3(int num_actions, float [] reward_actions, float gamma)
        {
 
            Weights = new float[num_actions,iterations+1];
            Probabilities = new float[num_actions, iterations];
            EstimatedRewards = new float[num_actions, iterations];
            Rewards = new float[num_actions, iterations];
            
            //initialize all weights to 1
            //Wi(1)= 1, for i=1,....,n
            for (int i = 0; i < num_actions; i++)
            {
                Weights[i, 0] = 1.0f;
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
                    Probabilities[i, t] = (1 - gamma)*(Weights[i,t]/sum_weights)+(gamma/num_actions);
                    Debug.Log("     Probabilities[i, t]");
                }

                //Randomly draw an action according to p1(t), ...., pn(t)
                action = RandomPickAction(num_actions, t);
                Debug.Log("     Action number = " + action);
                //observe the reward r(t)

                Rewards[action, t] = reward_actions[action];

                //update estimated reward   ^ri(t) 
                for (int j = 0; j < num_actions; j++)
                {
                    //if a(t)=i
                    if (action == j)
                    {
                        //^ri(t)  = r(t)/pi(t)
                        EstimatedRewards[j, t] = Rewards[j, t] / Probabilities[j, t];
                        Debug.Log("     EstimatedRewards[j, t]  = " + EstimatedRewards[j, t]);
                    }
                    else
                    {
                        // ^ri(t) =0
                        EstimatedRewards[j, t] = 0.0f;
                    }

                    //Update weights
                    float ga_act = (gamma / num_actions);
                    float e_power = (float)Math.Exp(ga_act * EstimatedRewards[action, t]);

                    //Wt+1(at) = Wt(at) e^(n*^rat(t))
                    Weights[action, t + 1] = Weights[action, t] * e_power;
                    Debug.Log("     Weights[action, t + 1] = " + Weights[action, t + 1]);
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
            float dou_ = (float) rnd.NextDouble();
            Debug.Log("         Random val = " + dou_);

            float sum_prob = 0.0f;
            for (int i = 0; i < actions_num; i++)
            {
                selected_action = i;
                sum_prob = sum_prob + Probabilities[i, time_];
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
                sum_weights = sum_weights + Weights[i, time_];
            }
            return sum_weights;
        }
    }
}
