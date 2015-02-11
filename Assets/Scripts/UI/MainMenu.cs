namespace Assets.Scripts.UI
{
    public class MainMenu : BasePanel
    {
        public Hud hud;

        public void OnOnlineGame()
        {
        }

        public void OnOfflineGame()
        {
            Hide();
            hud.Show();
            Game.Instance.ResetScore();
            Game.Instance.NewGame();
        }
    }
}
