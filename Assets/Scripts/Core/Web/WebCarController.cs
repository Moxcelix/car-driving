using System.Threading;

namespace Core.Web
{
    using Core.Car;
    using System.Collections.Specialized;
    using System.Net;
    using UnityEngine;

    public class WebCarController
    {
        private readonly Car _car;

        private readonly Thread _sendThread;
        private readonly Thread _receiveThread;

        private readonly int _sendTimeout;
        private readonly int _receiveTimeout;

        private readonly string _sendUrl;
        private readonly string _receiveUrl;

        public WebCarController(Car car,
            int sendTimeout, int receiveTimeout,
            string sendUrl, string receiveUrl)
        {
            _car = car;
            _sendTimeout = sendTimeout;
            _receiveTimeout = receiveTimeout;
            _sendUrl = sendUrl;
            _receiveUrl = receiveUrl;

            _sendThread = new Thread(SendData);
            _receiveThread = new Thread(ReceiveData);

            _sendThread.Start();
            _receiveThread.Start();
        }

        private void SendData()
        {
            while (_sendThread.IsAlive)
            {
                using (WebClient client = new WebClient())
                {
                    var data = new NameValueCollection
                    {
                        ["param1"] = "value1",
                        ["param2"] = "value2"
                    };

                    byte[] responseBytes = client.UploadValues(_sendUrl, "POST", data);
                    string response = System.Text.Encoding.UTF8.GetString(responseBytes);

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

                    Debug.Log(response);
                }

                Thread.Sleep(_receiveTimeout);
            }
        }
    }
}
