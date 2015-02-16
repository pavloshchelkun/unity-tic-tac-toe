using Assets.Scripts.Network;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class Lobby : BasePanel
    {
        public InputField playerName;
        public InputField joinRoomName;
        public InputField createRoomName;

        public void OnBack()
        {
            Hide();
            Game.Instance.Quit();
        }

        public void OnChangePlayerName()
        {
            NetworkMediator.Instance.PlayerName = playerName.text;
        }

        public void OnJoinRoom()
        {
            NetworkMediator.Instance.JoinRoom(joinRoomName.text);
        }

        public void OnCreateRoom()
        {
            NetworkMediator.Instance.CreateRoom(createRoomName.text);
        }

        public void OnJoinRandomRoom()
        {
            NetworkMediator.Instance.JoinRandomRoom();
        }

        public override void Show()
        {
            base.Show();
            playerName.text = NetworkMediator.Instance.PlayerName;
        }

        protected override void Start()
        {
            base.Start();

            NetworkMediator.Instance.OnConnectedToMasterSignal.AddListener(OnConnectedToMaster);
            NetworkMediator.Instance.OnJoinedRoomSignal.AddListener(OnJoinedRoom);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            NetworkMediator.Instance.OnConnectedToMasterSignal.RemoveListener(OnConnectedToMaster);
            NetworkMediator.Instance.OnJoinedRoomSignal.RemoveListener(OnJoinedRoom);
        }

        private void OnConnectedToMaster()
        {
            Show();
        }

        private void OnJoinedRoom()
        {
            Hide();
            Game.Instance.NewGame();
        }
    }
}
