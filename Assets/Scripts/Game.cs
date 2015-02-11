using UnityEngine;

namespace Assets.Scripts
{
    public class Game : MonoBehaviour
    {
        public static Game Instance { get; private set; }

        public Board board;

        private GameState currentState;

        public GameState CurrentState
        {
            get { return currentState; }
        }

        public bool IsPlaying
        {
            get { return currentState == GameState.Playing; }
        }

        public int Player1Score { get; private set; }

        public int Player2Score { get; private set; }

        public void NewGame()
        {
            board.gameObject.SetActive(true);
            board.Clear();
            board.SetPlayer(Seed.Cross);
            currentState = GameState.Playing;
        }

        public void ResetScore()
        {
            Player1Score = 0;
            Player2Score = 0;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            currentState = GameState.Playing;

            board.Init(OnBoardChange);
            board.SetPlayer(Seed.Empty);
        }

        private void OnBoardChange(Seed player)
        {
            if (board.HasWon(player))
            {
                switch (player)
                {
                    case Seed.Cross:
                        currentState = GameState.CrossWin;
                        Player1Score++;
                        break;
                    case Seed.Nought:
                        currentState = GameState.NoughtWin;
                        Player2Score++;
                        break;
                }
                board.SetPlayer(Seed.Empty);
            }
            else if (board.IsDraw())
            {
                currentState = GameState.Draw;
                board.SetPlayer(Seed.Empty);
            }
            else
            {
                board.SetPlayer(player == Seed.Cross ? Seed.Nought : Seed.Cross);
            }
        }
    }
}
