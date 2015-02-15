using Assets.Scripts.Signals;

namespace Assets.Scripts.UI
{
    public class MainMenu : BasePanel
    {
        public Hud hud;
        public Lobby lobby;

        public void OnOnlineGame()
        {
            Hide();
            UISignals.OnStartOnlineGameSignal.Dispatch();
        }

        public void OnOfflineGame()
        {
            Hide();
            UISignals.OnStartOfflineGameSignal.Dispatch();
        }

        protected override void Start()
        {
            base.Start();

            UISignals.OnBackToMainMenuSignal.AddListener(OnBackToMainMenu);

            Show();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UISignals.OnBackToMainMenuSignal.RemoveListener(OnBackToMainMenu);
        }

        private void OnBackToMainMenu()
        {
            Show();
        }
    }
}
