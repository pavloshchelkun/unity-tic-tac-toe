namespace Assets.Scripts.Signals
{
    public class NetworkSignals
    {
        public static readonly Signal OnBeginConnectingSignal = new Signal();
        public static readonly Signal OnConnectedToMasterSignal = new Signal();
    }
}
