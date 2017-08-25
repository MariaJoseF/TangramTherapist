using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Exp3
{
    class Elements
    {
        private int time_index;
        private int element;
        private double value;

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

        public int Element
        {
            get
            {
                return element;
            }

            set
            {
                element = value;
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

        public Elements(int time, int elem, double val)
        {
            time_index = time;
            element = elem;
            value = val;
        }

        public override string ToString()
        {
            return "" + value;
        }
    }
}
