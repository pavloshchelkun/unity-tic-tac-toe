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
            GameService.Quit();
        }

        protected override void Show()
        {
            base.Show();

            label.text = "Connecting...";
            label.color = Color.white;
            button.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();

            NetworkService.OnBeginConnectingSignal.AddListener(Show);
            NetworkService.OnConnectedToMasterSignal.AddListener(OnConnectedToMaster);
            NetworkService.OnConnectionFailSignal.AddListener(OnConnectionFail);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            NetworkService.OnBeginConnectingSignal.RemoveListener(Show);
            NetworkService.OnConnectedToMasterSignal.RemoveListener(OnConnectedToMaster);
            NetworkService.OnConnectionFailSignal.RemoveListener(OnConnectionFail);
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
