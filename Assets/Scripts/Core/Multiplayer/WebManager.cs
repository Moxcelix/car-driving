using UnityEngine;

namespace Core.Multiplayer
{
    public class WebManager : MonoBehaviour
    {
        [SerializeField] private string _ipAddress;
        [SerializeField] private int _port;

        private Client _client;

        private void Awake()
        {
            _client = new Client(_ipAddress, _port);

            _client.ConnectToServer();
        }

        private void Update()
        {
            _client.SendToServer("Aboba");
        }

        private void OnDestroy()
        {
            _client.Disconnect();
        }
    }
}