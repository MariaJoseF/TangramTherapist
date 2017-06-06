using System;

namespace ProsodicFeatures
{
    class Prosody
    {

        string rate;
        string pitch;
        string volume;

        public Prosody(string _rate, string _pitch, string _volume)
        {
            rate = _rate;
            pitch = _pitch;
            volume = _volume;
        }

        public string Rate
        {
            get
            {
                return rate;
            }

            set
            {
                rate = value;
            }
        }

        public string Pitch
        {
            get
            {
                return pitch;
            }

            set
            {
                pitch = value;
            }
        }

        public string Volume
        {
            get
            {
                return volume;
            }

            set
            {
                volume = value;
            }
        }

        internal void Intensity(int intensity_emotion)
        {
           
            /////// ver em quais campos devo alterar a intensidade, rate , volume e/ou pitch também???


        }
    }
}