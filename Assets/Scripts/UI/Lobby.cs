using Assets.Scripts.Signals;

namespace Assets.Scripts.UI
{
    public class Lobby : BasePanel
    {
        protected override void Start()
        {
            base.Start();

            NetworkSignals.OnConnectedToMasterSignal.AddListener(OnConnectedToMaster);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            NetworkSignals.OnConnectedToMasterSignal.RemoveListener(OnConnectedToMaster);
        }

        private void OnConnectedToMaster()
        {
            Show();
        }
    }
}
