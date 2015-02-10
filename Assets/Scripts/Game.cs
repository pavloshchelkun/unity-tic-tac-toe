using UnityEngine;

namespace Assets.Scripts
{
    public class Game : MonoBehaviour
    {
        public Board board;

        private GameState currentState;

        private void Start()
        {
            currentState = GameState.Playing;
            board.Init(OnBoardChange);
            board.SetPlayer(Seed.Cross);
        }

        private void OnBoardChange(Seed player)
        {
            if (board.HasWon(player))
            {
                board.Clear();
                board.SetPlayer(Seed.Cross);
                Debug.Log("Win: " + player);
            }
            else if (board.IsDraw())
            {
                board.Clear();
                board.SetPlayer(Seed.Cross);
                Debug.Log("Draw");
            }
            else
            {
                board.SetPlayer(player == Seed.Cross ? Seed.Nought : Seed.Cross);
            }
        }
    }
}
