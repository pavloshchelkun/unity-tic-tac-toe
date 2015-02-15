namespace Assets.Scripts.Signals
{
    public static class GameSignals
    {
        public static readonly Signal<Game> OnGameStartSignal = new Signal<Game>();
        public static readonly Signal<Game> OnGameResultSignal = new Signal<Game>();
    }
}
