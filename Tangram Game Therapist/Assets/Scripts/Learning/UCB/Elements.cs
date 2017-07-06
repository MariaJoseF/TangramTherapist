using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UCB
{
    class Elements
    {
        private int time_index;
        private int action;
        private double value;

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

        public double Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        public int Time_index
        {
            get
            {
                return time_index;
            }

            set
            {
                time_index = value;
            }
        }

        public Elements(int time, int act, double val)
        {
            time_index = time;
            action = act;
            value = val;
        }

        public Elements(int act, double val)
        {
            action = act;
            value = val;
        }

        public override string ToString()
        {
            return "" + value;
        }

    }
}
