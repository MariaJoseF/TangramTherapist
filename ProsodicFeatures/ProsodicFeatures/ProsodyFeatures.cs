using System;
using System.Text.RegularExpressions;

namespace ProsodicFeatures
{
    public class ProsodyFeatures
    {

        string animationText;
        string outPut_animationText;

        //Emys animations
        Prosody prosody_joy = new Prosody("medium", "x-high", "medium");
        Prosody prosody_sadness = new Prosody("medium", "low", "soft");
        Prosody prosody_anger = new Prosody("fast", "low", "loud");
        Prosody prosody_disgust = new Prosody("", "", "");
        Prosody prosody_fear = new Prosody("fast", "high", "medium");
        Prosody prosody_surprise = new Prosody("", "", "");
        //Prosody prosody_wink = new Prosody("", "", "");

        Prosody default_prosody;

        public ProsodyFeatures(string text_animations)
        {
            animationText = text_animations;
            outPut_animationText = "";
            default_prosody = new Prosody("medium", "x-high", "medium");

        }

        public void InsertProsodyFeatures()
        {
            string spliting = "<ANIMATE";

            string[] array_spliting = Regex.Split(animationText, spliting);

            string prosodyElemntsClose = "</prosody></prosody></prosody>";


            for (int i = 0; i < array_spliting.Length; i++)
            {
                int[] emotion_info = GetEmotion(array_spliting[i]);
                string aux = "";


                if (i == 0)
                {

                    if (emotion_info[0] != -1)//exists emotions
                    {
                        aux = ProsodyElemnts(0, 0) + ProsodyElemnts(emotion_info[0], emotion_info[1]) + spliting + array_spliting[i];
                    }
                    else// just put default prosody elements
                    {
                        aux = ProsodyElemnts(0, 0) + array_spliting[i];
                    }
                }
                else if (emotion_info[0] != -1 && i != 0)
                {
                    if (checkPreviousEmotions(array_spliting[i-1]) == false)//dont has emotion prosody to close
                    {
                        aux = ProsodyElemnts(emotion_info[0], emotion_info[1]) + spliting + array_spliting[i];
                    }
                    else // needs to close previous emotion prosody
                    {
                        aux = prosodyElemntsClose + ProsodyElemnts(emotion_info[0], emotion_info[1]) + spliting + array_spliting[i];
                    }
                }

                if (i == array_spliting.Length - 1) // closes the default prosody
                {
                    if (checkPreviousEmotions(array_spliting[i]) == false)//dont has emotion prosody to close
                    {
                        aux = aux + prosodyElemntsClose;
                    }
                    else // needs to close previous emotion prosody
                    {
                        aux = aux + prosodyElemntsClose + prosodyElemntsClose;
                    }                 
                }

                outPut_animationText = outPut_animationText + aux;
            }

        }

        private bool checkPreviousEmotions(string previous_Text)
        {
            bool exists_previous_emotions = false;

            string spliting = "(\\d\\)\\>)";
            string[] array_emotionsIntensity = Regex.Split(previous_Text, spliting, RegexOptions.IgnoreCase);


            string switch_on = array_emotionsIntensity[0];
            if (!(switch_on.Equals("") || switch_on.Contains("Gaze")))
            {
                exists_previous_emotions = previous_Text.Contains(switch_on);
            }
           
            return exists_previous_emotions;
        }

        private string ProsodyElemnts(int id_emotion, int intensity_emotion)
        {
            string prosodyElemnts = "";

            if (id_emotion == 0 && intensity_emotion == 0)
            {
                prosodyElemnts = "<prosody rate='" + Default_prosody.Rate + "'>" + "<prosody pitch='" + Default_prosody.Pitch + "'>" + "<prosody volume='" + Default_prosody.Volume + "'>";
            }
            else
            {
                switch (id_emotion)
                {
                    case 1://joy
                        prosody_joy.Intensity(intensity_emotion);
                        prosodyElemnts = "<prosody rate='" + prosody_joy.Rate + "'>" + "<prosody pitch='" + prosody_joy.Pitch + "'>" + "<prosody volume='" + prosody_joy.Volume + "'>";
                        break;
                    case 2://anger
                        prosody_anger.Intensity(intensity_emotion);
                        prosodyElemnts = "<prosody rate='" + prosody_anger.Rate + "'>" + "<prosody pitch='" + prosody_anger.Pitch + "'>" + "<prosody volume='" + prosody_anger.Volume + "'>";
                        break;
                    case 3://sadness
                        prosody_sadness.Intensity(intensity_emotion);
                        prosodyElemnts = "<prosody rate='" + prosody_sadness.Rate + "'>" + "<prosody pitch='" + prosody_sadness.Pitch + "'>" + "<prosody volume='" + prosody_sadness.Volume + "'>";
                        break;
                    case 4://surprise
                        prosody_surprise.Intensity(intensity_emotion);
                        prosodyElemnts = "<prosody rate='" + prosody_surprise.Rate + "'>" + "<prosody pitch='" + prosody_surprise.Pitch + "'>" + "<prosody volume='" + prosody_surprise.Volume + "'>";
                        break;
                    case 5://fear
                        prosody_fear.Intensity(intensity_emotion);
                        prosodyElemnts = "<prosody rate='" + prosody_fear.Rate + "'>" + "<prosody pitch='" + prosody_fear.Pitch + "'>" + "<prosody volume='" + prosody_fear.Volume + "'>";
                        break;

                }
            }

            return prosodyElemnts;

        }

        private int[] GetEmotion(string txt_emotion)
        {

            int[] emotionIntensity = { -1, -1 };

            if (!txt_emotion.Equals(""))
            {
                //string spliting = "(\\(\\w*\\d\\)\\>)";
                //string[] array_emotions = Regex.Split(txt_emotion, spliting, RegexOptions.IgnoreCase);

                string spliting = "(\\d\\)\\>)";
                string[] array_emotionsIntensity = Regex.Split(txt_emotion, spliting, RegexOptions.IgnoreCase);


                string switch_on = array_emotionsIntensity[0].ToLower();

                int intensity = Convert.ToInt16(array_emotionsIntensity[1][0] - 48); // Because of decimal value of char '0' is 48

                switch (switch_on)
                {
                    case "(joy":
                        emotionIntensity[0] = 1;
                        emotionIntensity[1] = intensity;
                        break;
                    case "(anger":
                        emotionIntensity[0] = 2;
                        emotionIntensity[1] = intensity;
                        break;
                    case "(sadness":
                        emotionIntensity[0] = 3;
                        emotionIntensity[1] = intensity;
                        break;
                    case "(surprise":
                        emotionIntensity[0] = 4;
                        emotionIntensity[1] = intensity;
                        break;
                    case "(fear":
                        emotionIntensity[0] = 5;
                        emotionIntensity[1] = intensity;
                        break;
                    //case "":
                    //    break;

                    default:
                        emotionIntensity[0] = -1;
                        emotionIntensity[1] = -1;
                        break;
                }
            }

            return emotionIntensity;
        }

        public string AnimationText
        {
            get
            {
                return animationText;
            }

            set
            {
                animationText = value;
            }
        }

        public string OutPut_animationText
        {
            get
            {
                return outPut_animationText;
            }

            set
            {
                outPut_animationText = value;
            }
        }

        internal Prosody Default_prosody
        {
            get
            {
                return default_prosody;
            }

            set
            {
                default_prosody = value;
            }
        }
    }
}