using Assets.Scripts.Network;

namespace Assets.Scripts.UI
{
    public class Waiting : BasePanel
    {
        public void OnBack()
        {
            Hide();
            GameService.Quit();
        }

        protected override void Start()
        {
            base.Start();

            NetworkService.OnJoinedRoomSignal.AddListener(Show);
            NetworkService.OnAllPlayersConnectedSignal.AddListener(Hide);
            NetworkService.OnDisconnectedFromMasterSignal.AddListener(Hide);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            NetworkService.OnJoinedRoomSignal.RemoveListener(Show);
            NetworkService.OnAllPlayersConnectedSignal.RemoveListener(Hide);
            NetworkService.OnDisconnectedFromMasterSignal.RemoveListener(Hide);
        }

        protected override void Update()
        {
            base.Update();

            if (NetworkService.HasAllPlayers)
            {
                Hide();
            }
        }
    }
}
