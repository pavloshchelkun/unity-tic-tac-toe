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
        
        public Player Player1 { get; private set; }

        public Player Player2 { get; private set; }

        public void PlayOnline()
        {
            NetworkMediator.Instance.Connect();
        }

        public void PlayOffline()
        {
            Reset();
            NewGame();
        }

        public void Quit()
        {
            HideBoard();
            OnGameQuitSignal.Dispatch();

            if (NetworkMediator.Instance.IsConnected)
            {
                NetworkMediator.Instance.Disconnect();
            }
        }

        public void NewGame()
        {
            board.gameObject.SetActive(true);
            board.Clear();
            board.SetPlayer(Seed.Cross);
            CurrentState = GameState.Playing;

            OnGameStartSignal.Dispatch(this);
        }

        public void Reset()
        {
            Player1 = new Player("Player X", 0, Seed.Cross);
            Player2 = new Player("Player O", 0, Seed.Nought);
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

            Reset();

            CurrentState = GameState.Playing;

            board.Init(OnBoardChange);
            board.SetPlayer(Seed.Empty);
            board.gameObject.SetActive(false);

            NetworkMediator.Instance.OnAllPlayersConnectedSignal.AddListener(OnAllPlayersConnected);
            NetworkMediator.Instance.OnDisconnectedFromMasterSignal.AddListener(OnDisconnectedFromMaster);
            NetworkMediator.Instance.OnRemoteBoardChangeSignal.AddListener(OnRemoteBoardChange);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            NetworkMediator.Instance.OnAllPlayersConnectedSignal.RemoveListener(OnAllPlayersConnected);
            NetworkMediator.Instance.OnDisconnectedFromMasterSignal.RemoveListener(OnDisconnectedFromMaster);
            NetworkMediator.Instance.OnRemoteBoardChangeSignal.RemoveListener(OnRemoteBoardChange);
        }

        private void OnAllPlayersConnected()
        {
            Reset();
            NewGame();

            Player1.Name = NetworkMediator.Instance.PlayerName;
            Player2.Name = NetworkMediator.Instance.OpponentName;

            if (NetworkMediator.Instance.IsMaster)
            {
                Player1.Type = Seed.Cross;
                Player2.Type = Seed.Nought;
                board.SetPlayer(Seed.Cross);
            }
            else
            {
                Player1.Type = Seed.Nought;
                Player2.Type = Seed.Cross;
                board.SetPlayer(Seed.Empty);
            }
        }

        private void OnDisconnectedFromMaster()
        {
            Quit();
        }

        private void OnRemoteBoardChange(Seed seed, int row, int col)
        {
            board.SetCell(seed, row, col);
        }

        private void OnBoardChange(Seed player, int row, int col)
        {
            Seed nextPlayer = Seed.Empty;

            if (board.HasWon(player))
            {
                switch (player)
                {
                    case Seed.Cross:
                        CurrentState = GameState.CrossWin;
                        Player1.Score++;
                        break;
                    case Seed.Nought:
                        CurrentState = GameState.NoughtWin;
                        Player2.Score++;
                        break;
                }

                OnGameResultSignal.Dispatch(this);
            }
            else if (board.IsDraw())
            {
                CurrentState = GameState.Draw;
                OnGameResultSignal.Dispatch(this);
            }
            else
            {
                nextPlayer = player == Seed.Cross ? Seed.Nought : Seed.Cross;
            }

            board.SetPlayer(nextPlayer);

            if (NetworkMediator.Instance.IsConnected && player == Player1.Type)
            {
                board.SetPlayer(Seed.Empty);
                NetworkMediator.Instance.SendBoardChange(player, row, col);
            }
        }
    }
}
