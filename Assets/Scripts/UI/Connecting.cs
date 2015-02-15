using Assets.Scripts.Signals;

namespace Assets.Scripts.UI
{
    public class Connecting : BasePanel
    {
        protected override void Start()
        {
            base.Start();

            NetworkSignals.OnBeginConnectingSignal.AddListener(Show);
            NetworkSignals.OnConnectedToMasterSignal.AddListener(Hide);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            NetworkSignals.OnBeginConnectingSignal.RemoveListener(Show);
            NetworkSignals.OnConnectedToMasterSignal.RemoveListener(Hide);
        }
    }
}
