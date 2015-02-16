using Assets.Scripts.Network;
using Assets.Scripts.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class Connecting : BasePanel
    {
        public Text label;
        public GameObject button;

        public void OnBack()
        {
            Hide();
            Game.Instance.Quit();
        }

        public override void Show()
        {
            base.Show();

            label.text = "Connecting...";
            label.color = Color.white;
            button.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();

            NetworkMediator.Instance.OnBeginConnectingSignal.AddListener(Show);
            NetworkMediator.Instance.OnConnectedToMasterSignal.AddListener(OnConnectedToMaster);
            NetworkMediator.Instance.OnConnectionFailSignal.AddListener(OnConnectionFail);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            NetworkMediator.Instance.OnBeginConnectingSignal.RemoveListener(Show);
            NetworkMediator.Instance.OnConnectedToMasterSignal.RemoveListener(OnConnectedToMaster);
            NetworkMediator.Instance.OnConnectionFailSignal.RemoveListener(OnConnectionFail);
        }

        private void OnConnectedToMaster()
        {
            Hide();
        }

        private void OnConnectionFail(string cause)
        {
            label.text = cause;
            label.color = Color.red;
            button.SetActive(true);
        }
    }
}
