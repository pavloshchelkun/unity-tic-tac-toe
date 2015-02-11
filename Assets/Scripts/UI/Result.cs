using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class Result : BasePanel
    {
        public Text label;

        public void OnNewGame()
        {
            Hide();
            Game.Instance.NewGame();
        }

        private void Start()
        {
            Hide();
        }

        private void Update()
        {
            if (!IsShown && !Game.Instance.IsPlaying)
            {
                Show();
            }
        }

        public override void Show()
        {
            base.Show();

            switch (Game.Instance.CurrentState)
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
        }
    }
}
