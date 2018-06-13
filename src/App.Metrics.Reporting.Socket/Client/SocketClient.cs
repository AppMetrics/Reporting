// <copyright file="SocketClient.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Logging;

namespace App.Metrics.Reporting.Socket.Client
{
    public class SocketClient
    {
        private static readonly ILog Logger = LogProvider.For<SocketClient>();

        private TcpClient _tcpClient;
        private readonly UdpClient _udpClient;
        private readonly SocketSettings _socketSettings;

        public string Endpoint
        {
            get
            {
                return _socketSettings.Endpoint;
            }
        }

        public SocketClient(SocketSettings socketSettings)
        {
            SocketSettings.Validate(socketSettings.ProtocolType, socketSettings.Address, socketSettings.Port);

            if (socketSettings.ProtocolType == ProtocolType.Tcp)
            {
                _tcpClient = new TcpClient();
            }

            if (socketSettings.ProtocolType == ProtocolType.Udp)
            {
                _udpClient = new UdpClient();
            }

            _socketSettings = socketSettings;
        }

        public bool IsConnected()
        {
            if (_tcpClient != null)
            {
                if (!_tcpClient.Connected
                    || !_tcpClient.Client.Poll(0, SelectMode.SelectWrite)
                    || _tcpClient.Client.Poll(0, SelectMode.SelectError))
                {
                    return false;
                }

                return true;
            }

            if (_udpClient != null)
            {
                return true;
            }

            return false;
        }

        public async Task ReConnect()
        {
            if (_tcpClient != null)
            {
                _tcpClient.Client.Dispose();
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(_socketSettings.Address, _socketSettings.Port);
            }
        }

        public async Task<SocketWriteResult> WriteAsync(
            string payload,
            CancellationToken cancellationToken = default)
        {
            byte[] output = Encoding.UTF8.GetBytes(payload);

            if (_tcpClient != null)
            {
                NetworkStream stream = _tcpClient.GetStream();
                await Task.Run(() =>
                {
                    return stream.WriteAsync(output, 0, output.Length, cancellationToken);
                });
                return new SocketWriteResult(true);
            }

            if (_udpClient != null)
            {
                int sended = await _udpClient.SendAsync(
                    output, output.Length, _socketSettings.Address, _socketSettings.Port);
                var success = sended == output.Length;
                return new SocketWriteResult(success);
            }

            return new SocketWriteResult(false, "There is no socket instance!");
        }
    }
}