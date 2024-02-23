using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Core.Multiplayer
{
    public class Client
    {
        private readonly string _ipAddress;
        private readonly int _port;

        private TcpClient _connection;
        private NetworkStream _stream;

        private readonly byte[] _buffer = new byte[1024];

        public Client(string ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;
        }

        public void ConnectToServer()
        {
            try
            {
                _connection = new TcpClient(_ipAddress, _port);
                _stream = _connection.GetStream();

                _stream.BeginRead(_buffer, 0, _buffer.Length, ReceiveCallback, null);
            }
            catch (Exception exception)
            {
                Debug.Log("Ошибка подключения к серверу: " + exception.Message);
            }
        }

        public void Disconnect()
        {
            _connection?.Close();
            _stream?.Close();

            _connection = null;
            _stream = null;
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                var bytesRead = _stream.EndRead(result);

                if (bytesRead <= 0)
                {
                    Disconnect();

                    return;
                }

                var message = Encoding.UTF8.GetString(_buffer, 0, bytesRead);

                Debug.Log("Получено сообщение от сервера: " + message);

                _stream.BeginRead(_buffer, 0, _buffer.Length, ReceiveCallback, null);
            }
            catch (Exception exception)
            {
                Debug.Log("Ошибка чтения данных: " + exception.Message);

                Disconnect();
            }
        }

        public void SendToServer(string message)
        {
            if (!_connection?.Connected ?? false)
            {
                Debug.Log("Нет подключения к серверу");

                return;
            }

            try
            {
                var data = Encoding.UTF8.GetBytes(message);

                _stream.Write(data, 0, data.Length);

                Debug.Log("Отправлено сообщение на сервер: " + message);
            }
            catch (Exception exception)
            {
                Debug.Log("Ошибка отправки сообщения: " + exception.Message);
            }
        }
    }
}