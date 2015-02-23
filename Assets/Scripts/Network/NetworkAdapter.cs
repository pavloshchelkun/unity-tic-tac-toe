using System.Collections.Generic;
using Assets.Scripts.Signals;
using UnityEngine;

namespace Assets.Scripts.Network
{
    public class NetworkAdapter : Photon.MonoBehaviour, INetworkService
    {
        public Signal OnBeginConnectingSignal { get; private set; }

        public Signal OnConnectedToMasterSignal { get; private set; }

        public Signal OnDisconnectedFromMasterSignal { get; private set; }

        public Signal<string> OnConnectionFailSignal { get; private set; }

        public Signal OnJoinedRoomSignal { get; private set; }

        public Signal OnAllPlayersConnectedSignal { get; private set; }

        public Signal<Seed, int, int> OnRemoteBoardChangeSignal { get; private set; }

        public Signal OnNewGameStartedSignal { get; private set; }

        public string PlayerName
        {
            get
            {
                if (PhotonNetwork.connected)
                {
                    return PhotonNetwork.playerName;
                }
                return PlayerPrefs.GetString("playerName", "Player" + Random.Range(1, 9999));
            }
            set
            {
                PhotonNetwork.playerName = value;
                PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
            }
        }

        public string OpponentName
        {
            get
            {
                if (PhotonNetwork.connected && PhotonNetwork.otherPlayers.Length > 0)
                {
                    return PhotonNetwork.otherPlayers[0].name;
                }
                return "Player O";
            }
        }

        public bool HasAllPlayers
        {
            get
            {
                return PhotonNetwork.connected && PhotonNetwork.room != null && PhotonNetwork.room.playerCount == 2;
            }
        }

        public bool IsMaster
        {
            get { return PhotonNetwork.isMasterClient; }
        }

        public bool IsConnected
        {
            get { return PhotonNetwork.connected; }
        }

        public void Connect()
        {
            if (!PhotonNetwork.connected)
            {
                OnConnectedToMasterSignal.Dispatch();

                PhotonNetwork.playerName = PlayerName;
                PhotonNetwork.ConnectUsingSettings("v1.0");
            }
            else
            {
                OnConnectedToMaster();
            }
        }

        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public void CreateRoom(string roomName)
        {
            PhotonNetwork.CreateRoom(roomName, new RoomOptions() { maxPlayers = 2 }, TypedLobby.Default);
        }

        public List<string> GetRoomList()
        {
            List<string> list = new List<string>();

            foreach (var roomInfo in PhotonNetwork.GetRoomList())
            {
                if (roomInfo.playerCount == 1)
                {
                    list.Add(roomInfo.name);
                }
            }

            return list;
        }

        public void SendNewGameStarted()
        {
            if (PhotonNetwork.room != null && PhotonNetwork.room.playerCount == 2)
            {
                photonView.RPC("OnNewGameStart", PhotonTargets.OthersBuffered);
            }
        }

        public void SendBoardChange(Seed seed, int row, int col)
        {
            if (PhotonNetwork.room != null && PhotonNetwork.room.playerCount == 2)
            {
                photonView.RPC("OnRemoteBoardChange", PhotonTargets.OthersBuffered, (int)seed, row, col);
            }
        }

        private void Awake()
        {
            OnBeginConnectingSignal = new Signal();
            OnConnectedToMasterSignal = new Signal();
            OnDisconnectedFromMasterSignal = new Signal();
            OnConnectionFailSignal = new Signal<string>();
            OnJoinedRoomSignal = new Signal();
            OnAllPlayersConnectedSignal = new Signal();
            OnRemoteBoardChangeSignal = new Signal<Seed, int, int>();
            OnNewGameStartedSignal = new Signal();

            ServiceLocator.AddService<INetworkService>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.RemoveService<INetworkService>();
        }

        private void OnConnectedToMaster()
        {
            OnConnectedToMasterSignal.Dispatch();
        }

        private void OnDisconnectedFromPhoton()
        {
            OnDisconnectedFromMasterSignal.Dispatch();
        }

        private void OnFailToConnectToPhoton()
        {
            OnConnectionFailSignal.Dispatch("Connection failed doe to invalid AppId or some network issues");
        }

        private void OnConnectionFail(DisconnectCause cause)
        {
            OnConnectionFailSignal.Dispatch("Connection failed doe to " + cause);
        }

        private void OnJoinedRoom()
        {
            OnJoinedRoomSignal.Dispatch();
            CheckForAllPlayers();
        }

        private void OnPhotonPlayerConnected(PhotonPlayer player)
        {
            CheckForAllPlayers();
        }

        private void CheckForAllPlayers()
        {
            if (PhotonNetwork.room.playerCount == 2)
            {
                OnAllPlayersConnectedSignal.Dispatch();
            }
        }

        private void OnPhotonPlayerDisconnected(PhotonPlayer player)
        {
            Disconnect();
        }

        [RPC]
        private void OnNewGameStart()
        {
            OnNewGameStartedSignal.Dispatch();
        }

        [RPC]
        private void OnRemoteBoardChange(int seed, int row, int col)
        {
            OnRemoteBoardChangeSignal.Dispatch((Seed)seed, row, col);
        }

        private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }
    }
}
