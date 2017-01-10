using CookComputing.XmlRpc;

namespace ThalamusTangram
{
    public interface ITUnityCaller
    {
        void UtteranceStarted(string id);
        void UtteranceFinished(string id);
    }

    public interface ITUnityCallerRpc : ITUnityCaller, IXmlRpcProxy
    {
        [XmlRpcMethod]
        new void UtteranceStarted(string id);

        [XmlRpcMethod]
        new void UtteranceFinished(string id);
    }

}
