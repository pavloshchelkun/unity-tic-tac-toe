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
            Game.Instance.Quit();
        }

        public void OnRestart()
        {
            Game.Instance.NewGame();
        }

        public override void Show()
        {
            base.Show();
            buttonRestart.gameObject.SetActive(NetworkMediator.Instance.IsConnected == false);
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
            player1Score.text = game.Player1.ToString();
            player2Score.text = game.Player2.ToString();
        }
    }
}
