using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class Result : BasePanel
    {
        public Text label;

        public void OnNewGame()
        {
            Hide();
            Game.Instance.NewGame();
        }

        protected override void Start()
        {
            base.Start();

            Game.Instance.OnGameResultSignal.AddListener(OnGameResult);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Game.Instance.OnGameResultSignal.RemoveListener(OnGameResult);
        }

        private void OnGameResult(Game game)
        {
            switch (game.CurrentState)
            {
                case GameState.CrossWin:
                    label.text = "X WIN!";
                    break;
                case GameState.NoughtWin:
                    label.text = "O WIN!";
                    break;
                case GameState.Draw:
                    label.text = "DRAW!";
                    break;
            }

            Show();
        }
    }
}
