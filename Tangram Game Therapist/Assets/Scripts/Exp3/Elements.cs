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
        private float value;

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

        public float Value
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

        public Elements(int time, int elem, float val)
        {
            time_index = time;
            element = elem;
            value = val;
        }
    }
}
