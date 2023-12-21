using System.Threading;

namespace Core.Web
{
    using Newtonsoft.Json.Linq;
    using System.Net;
    using UnityEngine;

    public class WebCarController
    {
        private readonly Thread _sendThread;
        private readonly Thread _receiveThread;

        private readonly int _sendTimeout;
        private readonly int _receiveTimeout;

        private readonly string _sendUrl;
        private readonly string _receiveUrl;

        private string _sendData = string.Empty;

        public delegate void CommandDelegate();
        public event CommandDelegate CloseLock;
        public event CommandDelegate OpenLock;
        public event CommandDelegate OpenUnlock;

        public WebCarController(
            int sendTimeout, int receiveTimeout,
            string sendUrl, string receiveUrl)
        {
            _sendTimeout = sendTimeout;
            _receiveTimeout = receiveTimeout;
            _sendUrl = sendUrl;
            _receiveUrl = receiveUrl;

            _sendThread = new Thread(SendData);
            _receiveThread = new Thread(ReceiveData);
        }

        public void Start()
        {
            _sendThread.Start();
            _receiveThread.Start();
        }

        public void Stop()
        {
            _sendThread.Abort();
            _receiveThread.Abort();
        }

        public void SetSendData(string data)
        {
            _sendData = data;
        }

        private void SendData()
        {
            while (_sendThread.IsAlive)
            {
                using (WebClient client = new WebClient())
                {
                    if(_sendData == string.Empty)
                    {
                        continue;
                    }

                    client.Headers.Add("Content-Type", "application/json");

                    string response = client.UploadString(_sendUrl, "POST", _sendData);

                    Debug.Log(response);
                }


                Thread.Sleep(_sendTimeout);
            }
        }

        private void ReceiveData()
        {
            while (_receiveThread.IsAlive)
            {
                using (WebClient client = new WebClient())
                {
                    string response = client.DownloadString(_receiveUrl);

                    JObject jsonObject = JObject.Parse(response);

                    Debug.Log("RESPONSE: " + response);

                    switch ((int)jsonObject["message"])
                    {
                        case 0:
                            break;
                        case 1:
                            CloseLock?.Invoke();
                            break;
                        case 2:
                            OpenLock?.Invoke();
                            break;
                        case 3:
                            OpenUnlock?.Invoke();
                            break;

                    }
                }

                Thread.Sleep(_receiveTimeout);
            }
        }
    }
}