using Assets.Scripts.Signals;

namespace Assets.Scripts
{
    public class Game : CoreBehaviour
    {
        public Board board;

        public GameState CurrentState { get; private set; }

        public bool IsPlaying
        {
            get { return CurrentState == GameState.Playing; }
        }

        public int Player1Score { get; private set; }

        public int Player2Score { get; private set; }

        public void NewGame()
        {
            board.gameObject.SetActive(true);
            board.Clear();
            board.SetPlayer(Seed.Cross);
            CurrentState = GameState.Playing;

            GameSignals.OnGameStartSignal.Dispatch(this);
        }

        public void ResetScore()
        {
            Player1Score = 0;
            Player2Score = 0;
        }

        protected override void Start()
        {
            base.Start();

            CurrentState = GameState.Playing;

            board.Init(OnBoardChange);
            board.SetPlayer(Seed.Empty);
            board.gameObject.SetActive(false);

            UISignals.OnStartOnlineGameSignal.AddListener(OnStartOnlineGame);
            UISignals.OnStartOfflineGameSignal.AddListener(OnStartOfflineGame);
            UISignals.OnBackToMainMenuSignal.AddListener(OnBackToMainMenu);
            UISignals.OnStartNewGameSignal.AddListener(NewGame);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UISignals.OnStartOnlineGameSignal.RemoveListener(OnStartOnlineGame);
            UISignals.OnStartOfflineGameSignal.RemoveListener(OnStartOfflineGame);
            UISignals.OnBackToMainMenuSignal.RemoveListener(OnBackToMainMenu);
            UISignals.OnStartNewGameSignal.RemoveListener(NewGame);
        }

        private void OnStartOnlineGame()
        {
            UISignals.OnBackToMainMenuSignal.Dispatch();
        }

        private void OnStartOfflineGame()
        {
            ResetScore();
            NewGame();
        }

        private void OnBackToMainMenu()
        {
            board.gameObject.SetActive(false);
        }

        private void OnBoardChange(Seed player)
        {
            if (board.HasWon(player))
            {
                switch (player)
                {
                    case Seed.Cross:
                        CurrentState = GameState.CrossWin;
                        Player1Score++;
                        break;
                    case Seed.Nought:
                        CurrentState = GameState.NoughtWin;
                        Player2Score++;
                        break;
                }

                board.SetPlayer(Seed.Empty);

                GameSignals.OnGameResultSignal.Dispatch(this);
            }
            else if (board.IsDraw())
            {
                CurrentState = GameState.Draw;

                board.SetPlayer(Seed.Empty);

                GameSignals.OnGameResultSignal.Dispatch(this);
            }
            else
            {
                board.SetPlayer(player == Seed.Cross ? Seed.Nought : Seed.Cross);
            }
        }
    }
}
