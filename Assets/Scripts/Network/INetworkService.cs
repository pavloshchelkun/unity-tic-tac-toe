using System.Collections.Generic;
using Assets.Scripts.Signals;

namespace Assets.Scripts.Network
{
    public interface INetworkService
    {
        Signal OnBeginConnectingSignal { get; }
        Signal OnConnectedToMasterSignal { get; }
        Signal OnDisconnectedFromMasterSignal { get; }
        Signal<string> OnConnectionFailSignal { get; }
        Signal OnJoinedRoomSignal { get; }
        Signal OnAllPlayersConnectedSignal { get; }
        Signal<Seed, int, int> OnRemoteBoardChangeSignal { get; }
        Signal OnNewGameStartedSignal { get; }

        string PlayerName { get; set; }
        string OpponentName { get; }
        bool HasAllPlayers { get; }
        bool IsMaster { get; }
        bool IsConnected { get; }

        void Connect();
        void Disconnect();
        void JoinRoom(string roomName);
        void JoinRandomRoom();
        void CreateRoom(string roomName);
        List<string> GetRoomList();

        void SendNewGameStarted();
        void SendBoardChange(Seed seed, int row, int col);
    }
}
