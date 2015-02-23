namespace Assets.Scripts.UI
{
    public class MainMenu : BasePanel
    {
        public void OnOnlineGame()
        {
            Hide();
            GameService.PlayOnline();
        }

        public void OnOfflineGame()
        {
            Hide();
            GameService.PlayOffline();
        }

        protected override void Start()
        {
            base.Start();

            GameService.OnGameQuitSignal.AddListener(Show);

            Show();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            GameService.OnGameQuitSignal.RemoveListener(Show);
        }
    }
}
