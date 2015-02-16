using Assets.Scripts.Network;

namespace Assets.Scripts.UI
{
    public class Waiting : BasePanel
    {
        public void OnBack()
        {
            Hide();
            Game.Instance.Quit();
        }

        protected override void Start()
        {
            base.Start();

            NetworkMediator.Instance.OnJoinedRoomSignal.AddListener(OnJoinedRoom);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            NetworkMediator.Instance.OnJoinedRoomSignal.RemoveListener(OnJoinedRoom);
        }

        private void OnJoinedRoom()
        {
            Show();
        }

        protected override void Update()
        {
            base.Update();

            if (NetworkMediator.Instance.HasAllPlayers)
            {
                Hide();
            }
        }
    }
}
