namespace Assets.Scripts.UI
{
    public class BasePanel : CoreBehaviour
    {
        public bool IsShown
        {
            get { return gameObject.activeSelf; }
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();

            Hide();
        }
    }
}
