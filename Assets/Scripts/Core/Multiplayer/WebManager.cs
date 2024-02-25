using Core.Car;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Multiplayer
{
    using Car = Core.Car.Car;

    public class WebManager : MonoBehaviour
    {
        [SerializeField] private string _ipAddress;
        [SerializeField] private int _port;

        private Client _client;

        #region TEST CAR SYNC
        private readonly object _lock = new object();

        [SerializeField] private bool _isLeader;
        [SerializeField] private Car _car;

        private CarState _state;

        private void SyncSendState()
        {
            var state = _car.GetSyncState();
            var json = JsonConvert.SerializeObject(state, Formatting.None);
            _client.SendToServer(json);
        }

        private void SyncReceiveState(string data)
        {
            lock (_lock)
            {
                try
                {
                    _state = JsonConvert.DeserializeObject<CarState>(data);
                }
                catch
                {
                    Debug.Log(data);
                }
            }
        }
        #endregion

        private void Awake()
        {
            _client = new Client(_ipAddress, _port);

            _client.OnDataReceived += SyncReceiveState;

            _client.ConnectToServer();
        }

        private void Start()
        {
            _car.Syncable = !_isLeader;
        }

        private void Update()
        {
            if (_isLeader)
            {
                SyncSendState();
            }

            lock (_lock)
            {
                if (_state != null)
                {
                    _car.Synchronize(_state);
                }
            }
        }

        private void OnDestroy()
        {
            _client.Disconnect();
        }
    }
}