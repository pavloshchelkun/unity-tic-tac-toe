namespace Assets.Scripts.Signals
{
    public static class UISignals
    {
        public static readonly Signal OnStartOnlineGameSignal = new Signal();
        public static readonly Signal OnStartOfflineGameSignal = new Signal();
        public static readonly Signal OnStartNewGameSignal = new Signal();
        public static readonly Signal OnBackToMainMenuSignal = new Signal();
    }
}
