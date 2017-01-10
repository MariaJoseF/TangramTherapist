using EmoteCommonMessages;
using Thalamus;

namespace ThalamusTangram
{
    public interface IGazeStateActions : IAction
    {
        /// <summary>
        /// Gaze to a point on the screen
        /// </summary>
        /// <param name="x">the coordinate value relative to the vertical axis of the screen (the one in front of the robot)</param>
        /// <param name="z">the coordinate value relative to the horizzontal axis of the screen (the one going at the sides of the robot)</param>
        /// 
        void GazeAtScreen(double x, double y);
        void GlanceAtScreen(double x, double y);
    }


    public interface ITThalamusPublisher : IThalamusPublisher, IFMLSpeech
    {  
    }

    public class ThalamusPublisher : ITThalamusPublisher
    {
        private readonly dynamic _publisher;
        public ThalamusPublisher(dynamic publisher)
        {
            _publisher = publisher;
        }

       
        /*public void GazeAtScreen(double x, double y)
        {
            _publisher.GazeAtScreen(x, y);
        }
        public void GlanceAtScreen(double x, double y)
        {
            _publisher.GlanceAtScreen(x, y);
        }*/


        public void CancelUtterance(string id)
        {
            _publisher.CancelUtterance(id);
        }

        public void PerformUtterance(string id, string utterance, string category)
        {
            _publisher.PerformUtterance(id, utterance, category);
        }

        public void PerformUtteranceFromLibrary(string id, string category, string subcategory, string[] tagNames, string[] tagValues)
        {
            _publisher.PerformUtteranceFromLibrary(id, category, subcategory, tagNames, tagValues);
        }
    }
}
