using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Reporting.Graphite.Client
{
    public sealed class UdpGraphiteSender : GraphiteSender
    {
        private readonly ILogger<UdpGraphiteSender> _logger;

        private UdpClient _client;

        public UdpGraphiteSender(ILoggerFactory loggerFactory, string host, int port)
            : base(host, port) { _logger = loggerFactory.CreateLogger<UdpGraphiteSender>(); }

        public override void Dispose()
        {
            (_client as IDisposable)?.Dispose();
            _client = null;
        }

        /// <inheritdoc />
        protected override async Task SendData(byte[] bytes)
        {
            _client = _client ?? new UdpClient();
            try
            {
                await _client.SendAsync(bytes, bytes.Length, _host, _port);
            }
            catch (Exception ex)
            {
                (_client as IDisposable)?.Dispose();
                _client = null;
                _logger.LogError((int)LoggingEvents.UdpSendError, ex, "Data was not sent because of an exception");
            }
        }
    }
}