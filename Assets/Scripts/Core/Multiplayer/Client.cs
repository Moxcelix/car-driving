using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Core.Multiplayer
{
    public class Client
    {
        public delegate void OnDataReceivedDelegate(string data);
        public event OnDataReceivedDelegate OnDataReceived;

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
                Debug.Log("������ ����������� � �������: " + exception.Message);
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
                if (!_connection.Connected)
                {
                    Debug.Log("���������� � �������� �������");
                    Disconnect();
                    return;
                }

                var bytesRead = _stream.EndRead(result);

                if (bytesRead <= 0)
                {
                    Debug.Log("���������� � �������� �������");
                    Disconnect();
                    return;
                }

                var message = Encoding.UTF8.GetString(_buffer, 0, bytesRead);

                OnDataReceived?.Invoke(message);

                Debug.Log("�������� ��������� �� �������: " + message);
            }
            catch (Exception exception)
            {
                Debug.Log("������ ������ ������: " + exception.Message);
            }
            finally
            {
                if (_connection != null && _connection.Connected)
                {
                    _stream.BeginRead(_buffer, 0, _buffer.Length, ReceiveCallback, null);
                }
            }
        }

        public void SendToServer(string message)
        {
            if (!_connection?.Connected ?? false)
            {
                Debug.Log("��� ����������� � �������");

                return;
            }

            try
            {
                var data = Encoding.UTF8.GetBytes(message);

                _stream.Write(data, 0, data.Length);

                Debug.Log("���������� ��������� �� ������: " + message);
            }
            catch (Exception exception)
            {
                Debug.Log("������ �������� ���������: " + exception.Message);
            }
        }
    }
}