using Assets.Scripts.Signals;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class Hud : BasePanel
    {
        public MainMenu mainMenu;
        public Text player1Score;
        public Text player2Score;

        public void OnBack()
        {
            Hide();
            mainMenu.Show();

            UISignals.OnBackToMainMenuSignal.Dispatch();
        }

        public void OnRestart()
        {
            UISignals.OnStartOfflineGameSignal.Dispatch();
        }

        protected override void Start()
        {
            base.Start();

            GameSignals.OnGameStartSignal.AddListener(OnGameStart);
            GameSignals.OnGameResultSignal.AddListener(UpdateGameScore);

            UISignals.OnBackToMainMenuSignal.AddListener(Hide);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            GameSignals.OnGameStartSignal.RemoveListener(OnGameStart);
            GameSignals.OnGameResultSignal.RemoveListener(UpdateGameScore);

            UISignals.OnBackToMainMenuSignal.RemoveListener(Hide);
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
