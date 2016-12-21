using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Exp3
{
    class Elements
    {
        private int time_index;
        private float element;

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

        public float Element
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

        
        public Elements(int time, float elem)
        {
            time_index = time;
            element = elem;
        }
    }
}
