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
            Game.Instance.NewGame();
        }

        public override void Show()
        {
            base.Show();
            button.gameObject.SetActive(!NetworkMediator.Instance.IsConnected || NetworkMediator.Instance.IsMaster);
        }

        protected override void Start()
        {
            base.Start();

            Game.Instance.OnGameResultSignal.AddListener(OnGameResult);
            NetworkMediator.Instance.OnNewGameStartedSignal.AddListener(OnNewGame);
            NetworkMediator.Instance.OnDisconnectedFromMasterSignal.AddListener(Hide);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Game.Instance.OnGameResultSignal.RemoveListener(OnGameResult);
            NetworkMediator.Instance.OnNewGameStartedSignal.RemoveListener(OnNewGame);
            NetworkMediator.Instance.OnDisconnectedFromMasterSignal.RemoveListener(Hide);
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
