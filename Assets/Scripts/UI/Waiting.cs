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

            NetworkMediator.Instance.OnJoinedRoomSignal.AddListener(Show);
            NetworkMediator.Instance.OnAllPlayersConnectedSignal.AddListener(Hide);
            NetworkMediator.Instance.OnDisconnectedFromMasterSignal.AddListener(Hide);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            NetworkMediator.Instance.OnJoinedRoomSignal.RemoveListener(Show);
            NetworkMediator.Instance.OnAllPlayersConnectedSignal.RemoveListener(Hide);
            NetworkMediator.Instance.OnDisconnectedFromMasterSignal.RemoveListener(Hide);
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
