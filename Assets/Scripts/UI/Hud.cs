using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class Hud : BasePanel
    {
        public Text player1Score;
        public Text player2Score;

        public void OnBack()
        {
            Hide();
            Game.Instance.Quit();
        }

        public void OnRestart()
        {
            Game.Instance.NewGame();
        }

        protected override void Start()
        {
            base.Start();

            Game.Instance.OnGameStartSignal.AddListener(OnGameStart);
            Game.Instance.OnGameResultSignal.AddListener(UpdateGameScore);
            Game.Instance.OnGameQuitSignal.AddListener(Hide);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Game.Instance.OnGameStartSignal.RemoveListener(OnGameStart);
            Game.Instance.OnGameResultSignal.RemoveListener(UpdateGameScore);
            Game.Instance.OnGameQuitSignal.RemoveListener(Hide);
        }

        private void OnGameStart(Game game)
        {
            Show();
            UpdateGameScore(game);
        }

        private void UpdateGameScore(Game game)
        {
            player1Score.text = "Payer 1: " + game.Player1Score;
            player2Score.text = "Payer 2: " + game.Player2Score;
        }
    }
}
