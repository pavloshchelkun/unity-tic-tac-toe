using System.Collections.Generic;
using Assets.Scripts.Network;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class Lobby : BasePanel
    {
        public InputField playerName;
        public InputField createRoomName;

        public ScrollRect scrollRect;
        public GridLayoutGroup grid;

        public GameObject roomButtonPrefab;

        private readonly List<RoomButton> roomButtonList = new List<RoomButton>();

        public void OnBack()
        {
            Hide();
            Game.Instance.Quit();
        }

        public void OnChangePlayerName()
        {
            NetworkMediator.Instance.PlayerName = playerName.text;
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
            createRoomName.text = NetworkMediator.Instance.PlayerName;
        }

        protected override void Start()
        {
            base.Start();

            NetworkMediator.Instance.OnConnectedToMasterSignal.AddListener(Show);
            NetworkMediator.Instance.OnJoinedRoomSignal.AddListener(Hide);
            NetworkMediator.Instance.OnDisconnectedFromMasterSignal.AddListener(Hide);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            NetworkMediator.Instance.OnConnectedToMasterSignal.RemoveListener(Show);
            NetworkMediator.Instance.OnJoinedRoomSignal.RemoveListener(Hide);
            NetworkMediator.Instance.OnDisconnectedFromMasterSignal.RemoveListener(Hide);
        }

        private RoomButton GetRoomButton()
        {
            GameObject aGO = (GameObject)Instantiate(roomButtonPrefab, Vector3.zero, Quaternion.identity);
            aGO.transform.SetParent(grid.transform, false);
            return aGO.GetComponent<RoomButton>();
        }

        protected override void Update()
        {
            base.Update();

            var roomList = NetworkMediator.Instance.GetRoomList();

            for (int i = 0; i < roomList.Count - roomButtonList.Count; i++)
            {
                var button = GetRoomButton();
                roomButtonList.Add(button);
            }

            for (int i = 0; i < roomButtonList.Count; i++)
            {
                if (i < roomList.Count)
                {
                    roomButtonList[i].Init(roomList[i]);
                    roomButtonList[i].gameObject.SetActive(true);
                }
                else
                {
                    roomButtonList[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
