using System;
using System.Threading;

namespace Core.Web
{
    using Core.Car;
    using UnityEngine;

    public class WebCarController
    {
        private readonly Car _car;

        private readonly Thread _sendThread;
        private readonly Thread _receiveThread;

        private readonly int _sendTimeout;
        private readonly int _receiveTimeout;

        public WebCarController(Car car, int sendTimeout, int receiveTimeout)
        {
            _car = car;
            _sendTimeout = sendTimeout;
            _receiveTimeout = receiveTimeout;

            _sendThread = new Thread(SendData);
            _receiveThread = new Thread(ReceiveData);

            _sendThread.Start();
            _receiveThread.Start();
        }

        private void SendData()
        {
            while (_sendThread.IsAlive)
            {
                Debug.Log("Send some data");

                Thread.Sleep(_sendTimeout);
            }
        }

        private void ReceiveData()
        {
            while (_receiveThread.IsAlive)
            {
                Debug.Log("Receive some data");

                Thread.Sleep(_receiveTimeout);
            }
        }
    }
}
