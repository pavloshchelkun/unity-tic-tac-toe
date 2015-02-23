namespace Assets.Scripts.UI
{
    public class BasePanel : CoreBehaviour
    {
        protected virtual void Show()
        {
            gameObject.SetActive(true);
        }

        protected virtual void Hide()
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
