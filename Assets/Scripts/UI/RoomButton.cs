using Assets.Scripts.Network;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class RoomButton : CoreBehaviour
    {
        public Text label;

        private string room = string.Empty;

        public void Init(string roomName)
        {
            room = roomName;
            label.text = room;
        }

        public void OnClick()
        {
            NetworkService.JoinRoom(room);
        }
    }
}