using Assets.Scripts.Network;
using Assets.Scripts.Signals;
using UnityEngine;

namespace Assets.Scripts
{
    public class Game : CoreBehaviour, IGameService
    {
        public Board board;

        public Signal<Game> OnGameStartSignal { get; private set; }

        public Signal<Game> OnGameResultSignal { get; private set; }

        public Signal OnGameQuitSignal { get; private set; }

        public GameState CurrentState { get; private set; }

        public bool IsPlaying
        {
            get { return CurrentState == GameState.Playing; }
        }
        
        public Player Player1 { get; private set; }

        public Player Player2 { get; private set; }

        public void PlayOnline()
        {
            CurrentState = GameState.Lobby;
            NetworkService.Connect();
        }

        public void PlayOffline()
        {
            Reset();
            NewGame();
        }
        
        public void NewGame()
        {
            board.gameObject.SetActive(true);
            board.Clear();
            board.SetPlayer(Seed.Cross);
            CurrentState = GameState.Playing;

            if (NetworkService.IsConnected)
            {
                if (NetworkService.IsMaster)
                {
                    Player1.Name = NetworkService.PlayerName;
                    Player2.Name = NetworkService.OpponentName;

                    Player1.Type = Seed.Cross;
                    Player2.Type = Seed.Nought;
                    board.SetPlayer(Seed.Cross);

                    NetworkService.SendNewGameStarted();
                }
                else
                {
                    Player1.Name = NetworkService.OpponentName;
                    Player2.Name = NetworkService.PlayerName;

                    Player1.Type = Seed.Nought;
                    Player2.Type = Seed.Cross;
                    board.SetPlayer(Seed.Empty);
                }
            }

            OnGameStartSignal.Dispatch(this);
        }

        public void Quit()
        {
            CurrentState = GameState.MainMenu;
            HideBoard();
            OnGameQuitSignal.Dispatch();

            if (NetworkService.IsConnected)
            {
                NetworkService.Disconnect();
            }
        }

        protected override void Awake()
        {
            base.Awake();

            OnGameStartSignal = new Signal<Game>();
            OnGameResultSignal = new Signal<Game>();
            OnGameQuitSignal = new Signal();

            ServiceLocator.AddService<IGameService>(this);
        }

        protected override void Start()
        {
            base.Start();

            Reset();

            CurrentState = GameState.MainMenu;

            board.Init(OnBoardChange);
            board.SetPlayer(Seed.Empty);
            board.gameObject.SetActive(false);

            NetworkService.OnAllPlayersConnectedSignal.AddListener(OnAllPlayersConnected);
            NetworkService.OnDisconnectedFromMasterSignal.AddListener(OnDisconnectedFromMaster);
            NetworkService.OnRemoteBoardChangeSignal.AddListener(OnRemoteBoardChange);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            NetworkService.OnAllPlayersConnectedSignal.RemoveListener(OnAllPlayersConnected);
            NetworkService.OnDisconnectedFromMasterSignal.RemoveListener(OnDisconnectedFromMaster);
            NetworkService.OnRemoteBoardChangeSignal.RemoveListener(OnRemoteBoardChange);

            ServiceLocator.RemoveService<IGameService>();
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (CurrentState == GameState.MainMenu)
                {
                    Application.Quit();
                }
                else
                {
                    Quit();
                }
            }
        }

        private void Reset()
        {
            Player1 = new Player("Player X", 0, Seed.Cross);
            Player2 = new Player("Player O", 0, Seed.Nought);
        }

        private void HideBoard()
        {
            board.gameObject.SetActive(false);
        }

        private void OnAllPlayersConnected()
        {
            Reset();
            NewGame();
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

            if (NetworkService.IsConnected && player == Player1.Type)
            {
                board.SetPlayer(Seed.Empty);
                NetworkService.SendBoardChange(player, row, col);
            }
        }
    }
}
