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
            GameService.Quit();
        }

        public void OnChangePlayerName()
        {
            NetworkService.PlayerName = playerName.text;
        }
        
        public void OnCreateRoom()
        {
            NetworkService.CreateRoom(createRoomName.text);
        }

        public void OnJoinRandomRoom()
        {
            NetworkService.JoinRandomRoom();
        }

        protected override void Show()
        {
            base.Show();

            playerName.text = NetworkService.PlayerName;
            createRoomName.text = NetworkService.PlayerName;
        }

        protected override void Start()
        {
            base.Start();

            NetworkService.OnConnectedToMasterSignal.AddListener(Show);
            NetworkService.OnJoinedRoomSignal.AddListener(Hide);
            NetworkService.OnDisconnectedFromMasterSignal.AddListener(Hide);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            NetworkService.OnConnectedToMasterSignal.RemoveListener(Show);
            NetworkService.OnJoinedRoomSignal.RemoveListener(Hide);
            NetworkService.OnDisconnectedFromMasterSignal.RemoveListener(Hide);
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

            var roomList = NetworkService.GetRoomList();

            // Create more buttons if needed
            for (int i = 0; i < roomList.Count - roomButtonList.Count; i++)
            {
                var button = GetRoomButton();
                roomButtonList.Add(button);
            }

            // Update all buttons
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
