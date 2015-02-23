using Assets.Scripts.Network;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class Result : BasePanel
    {
        public Text label;
        public Button button;

        public void OnNewGame()
        {
            Hide();
            GameService.NewGame();
        }

        protected override void Show()
        {
            base.Show();
            button.gameObject.SetActive(!NetworkService.IsConnected || NetworkService.IsMaster);
        }

        protected override void Start()
        {
            base.Start();

            GameService.OnGameResultSignal.AddListener(OnGameResult);
            NetworkService.OnNewGameStartedSignal.AddListener(OnNewGame);
            NetworkService.OnDisconnectedFromMasterSignal.AddListener(Hide);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            GameService.OnGameResultSignal.RemoveListener(OnGameResult);
            NetworkService.OnNewGameStartedSignal.RemoveListener(OnNewGame);
            NetworkService.OnDisconnectedFromMasterSignal.RemoveListener(Hide);
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
