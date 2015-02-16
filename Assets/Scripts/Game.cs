using Assets.Scripts.Network;
using Assets.Scripts.Signals;
using UnityEngine;

namespace Assets.Scripts
{
    public class Game : CoreBehaviour
    {
        public static Game Instance { get; private set; }

        public readonly Signal<Game> OnGameStartSignal = new Signal<Game>();
        public readonly Signal<Game> OnGameResultSignal = new Signal<Game>();
        public readonly Signal OnGameQuitSignal = new Signal();

        public Board board;

        public GameState CurrentState { get; private set; }

        public bool IsPlaying
        {
            get { return CurrentState == GameState.Playing; }
        }

        public int Player1Score { get; private set; }

        public int Player2Score { get; private set; }

        public void PlayOnline()
        {
            NetworkMediator.Instance.Connect();
        }

        public void PlayOffline()
        {
            ResetScore();
            NewGame();
        }

        public void Quit()
        {
            HideBoard();
            OnGameQuitSignal.Dispatch();
        }

        public void NewGame()
        {
            board.gameObject.SetActive(true);
            board.Clear();
            board.SetPlayer(Seed.Cross);
            CurrentState = GameState.Playing;

            OnGameStartSignal.Dispatch(this);
        }

        public void ResetScore()
        {
            Player1Score = 0;
            Player2Score = 0;
        }

        public void HideBoard()
        {
            board.gameObject.SetActive(false);
        }

        protected override void Awake()
        {
            base.Awake();
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected override void Start()
        {
            base.Start();

            CurrentState = GameState.Playing;

            board.Init(OnBoardChange);
            board.SetPlayer(Seed.Empty);
            board.gameObject.SetActive(false);

            NetworkMediator.Instance.OnRemoteBoardChangeSignal.AddListener(OnRemoteBoardChange);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            NetworkMediator.Instance.OnRemoteBoardChangeSignal.RemoveListener(OnRemoteBoardChange);
        }

        private void OnRemoteBoardChange(Seed seed, int row, int col)
        {
            board.SetCell(seed, row, col);
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

                OnGameResultSignal.Dispatch(this);
            }
            else if (board.IsDraw())
            {
                CurrentState = GameState.Draw;

                board.SetPlayer(Seed.Empty);

                OnGameResultSignal.Dispatch(this);
            }
            else
            {
                board.SetPlayer(player == Seed.Cross ? Seed.Nought : Seed.Cross);
            }
        }
    }
}
