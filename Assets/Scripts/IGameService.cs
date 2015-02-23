using Assets.Scripts.Signals;

namespace Assets.Scripts
{
    public interface IGameService
    {
        Signal<Game> OnGameStartSignal { get; }
        Signal<Game> OnGameResultSignal { get; }
        Signal OnGameQuitSignal { get; }

        GameState CurrentState { get; }
        bool IsPlaying { get; }
        Player Player1 { get; }
        Player Player2 { get; }

        void PlayOnline();
        void PlayOffline();
        void NewGame();
        void Quit();
    }
}
