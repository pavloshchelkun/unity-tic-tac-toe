namespace Assets.Scripts.UI
{
    public class MainMenu : BasePanel
    {
        public Hud hud;
        public Lobby lobby;

        public void OnOnlineGame()
        {
            Hide();
            Game.Instance.PlayOnline();
        }

        public void OnOfflineGame()
        {
            Hide();
            Game.Instance.PlayOffline();
        }

        protected override void Start()
        {
            base.Start();

            Game.Instance.OnGameQuitSignal.AddListener(Show);

            Show();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Game.Instance.OnGameQuitSignal.RemoveListener(Show);
        }
    }
}
