using System.Threading;
using Thalamus;
using EmoteCommonMessages;
using System;
//using Thalamus.Actions;  //for ExampleRunTimeBehavior()

namespace ThalamusTangram
{

    public interface ITActions : IAction
    {
        void UtteranceStarted(string id);
        void UtteranceFinished(string id);
    }

    public class ThalamusConnector : ThalamusClient, IFMLSpeechEvents
    {
        public ITThalamusPublisher TypifiedPublisher {  get;  private set; }
        public UnityConnector UnityConnector { private get; set; }
        public string fileName = null;

        public ThalamusConnector(string clientName, string character = "") : base(clientName)
        {
            //Thalamus.Environment.Instance.setDebug("messages", false);
            SetPublisher<ITThalamusPublisher>();
            TypifiedPublisher = new ThalamusPublisher(Publisher);
        }

        public override void Dispose()
        {
            UnityConnector.Dispose();
            base.Dispose();
        }

        void IFMLSpeechEvents.UtteranceStarted(string id)
        {
            try
            {
                Console.WriteLine("Thalamus connector started " + id);
                UnityConnector.RPCProxy.UtteranceStarted(id);
            }
            catch (Exception e) {
                DebugException(e);
            }
        }

        void IFMLSpeechEvents.UtteranceFinished(string id)
        {
            try
            {
                Console.WriteLine("Thalamus connector finished " + id);
                UnityConnector.RPCProxy.UtteranceFinished(id);
            }
            catch (Exception e) {
                Debug("Exception: {0}", e.Message);
            }
        }
    }
}
