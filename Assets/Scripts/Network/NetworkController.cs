using Assets.Scripts.Signals;
using UnityEngine;

namespace Assets.Scripts.Network
{
    public class NetworkController : Photon.MonoBehaviour
    {
        public string PlayerName
        {
            get { return PhotonNetwork.playerName; }
            set
            {
                PhotonNetwork.playerName = value;
                PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
            }
        }

        public bool Connect()
        {
            if (!PhotonNetwork.connected)
            {
                NetworkSignals.OnConnectedToMasterSignal.Dispatch();

                PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "Player" + Random.Range(1, 9999));
                return PhotonNetwork.ConnectUsingSettings("v1.0");
            }
            
            return true;
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

        private void OnConnectedToMaster()
        {
            NetworkSignals.OnConnectedToMasterSignal.Dispatch();
        }
    }
}
