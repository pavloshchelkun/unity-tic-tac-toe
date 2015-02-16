﻿using Assets.Scripts.Signals;
using UnityEngine;

namespace Assets.Scripts.Network
{
    public class NetworkMediator : Photon.MonoBehaviour
    {
        public static NetworkMediator Instance { get; private set; }

        public readonly Signal OnBeginConnectingSignal = new Signal();
        public readonly Signal OnConnectedToMasterSignal = new Signal();
        public readonly Signal<string> OnConnectionFailSignal = new Signal<string>();
        public readonly Signal OnJoinedRoomSignal = new Signal();
        public readonly Signal<Seed, int, int> OnRemoteBoardChangeSignal = new Signal<Seed, int, int>();

        public string PlayerName
        {
            get { return PhotonNetwork.playerName; }
            set
            {
                PhotonNetwork.playerName = value;
                PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
            }
        }

        public bool HasAllPlayers 
        { 
            get
            {
                return PhotonNetwork.connected && PhotonNetwork.room != null && PhotonNetwork.room.playerCount == 2;
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Connect()
        {
            if (!PhotonNetwork.connected)
            {
                OnConnectedToMasterSignal.Dispatch();

                PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "Player" + Random.Range(1, 9999));
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

        public bool JoinRoom(string roomName)
        {
            return PhotonNetwork.JoinRoom(roomName);
        }

        public bool JoinRandomRoom()
        {
            return PhotonNetwork.JoinRandomRoom();
        }

        public bool CreateRoom(string roomName)
        {
            return PhotonNetwork.CreateRoom(roomName, new RoomOptions() { maxPlayers = 2 }, TypedLobby.Default);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        private void OnConnectedToMaster()
        {
            OnConnectedToMasterSignal.Dispatch();
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
        }

        public void SendBoardChange(Seed seed, int row, int col)
        {
            if (PhotonNetwork.room != null && PhotonNetwork.room.playerCount == 2)
            {
                photonView.RPC("OnRemoteBoardChange", PhotonTargets.AllBufferedViaServer, seed, row, col);
            }
        }

        [RPC]
        private void OnRemoteBoardChange(Seed seed, int row, int col)
        {
            OnRemoteBoardChangeSignal.Dispatch(seed, row, col);
        }
    }
}