using Assets.Scripts.Network;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class Hud : BasePanel
    {
        public Text player1Score;
        public Text player2Score;
        public Button buttonRestart;

        public void OnBack()
        {
            Hide();
            GameService.Quit();
        }

        public void OnRestart()
        {
            GameService.NewGame();
        }

        protected override void Show()
        {
            base.Show();
            buttonRestart.gameObject.SetActive(NetworkService.IsConnected == false);
        }

        protected override void Start()
        {
            base.Start();

            GameService.OnGameStartSignal.AddListener(OnGameStart);
            GameService.OnGameResultSignal.AddListener(UpdateGameScore);
            GameService.OnGameQuitSignal.AddListener(Hide);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            GameService.OnGameStartSignal.RemoveListener(OnGameStart);
            GameService.OnGameResultSignal.RemoveListener(UpdateGameScore);
            GameService.OnGameQuitSignal.RemoveListener(Hide);
        }

        private void OnGameStart(Game game)
        {
            Show();
            UpdateGameScore(game);
        }

        private void UpdateGameScore(Game game)
        {
            player1Score.text = game.Player1.ToString();
            player2Score.text = game.Player2.ToString();
        }
    }
}
