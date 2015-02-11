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
            Game.Instance.board.gameObject.SetActive(false);
        }

        public void OnRestart()
        {
            Game.Instance.NewGame();
        }

        private void Start()
        {
            Hide();
        }

        private void Update()
        {
            player1Score.text = "Payer 1: " + Game.Instance.Player1Score;
            player2Score.text = "Payer 2: " + Game.Instance.Player2Score;
        }
    }
}
