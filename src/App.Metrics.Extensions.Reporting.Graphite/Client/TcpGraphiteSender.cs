using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Reporting.Graphite.Client
{
    public sealed class TcpGraphiteSender : GraphiteSender
    {
        private readonly ILogger<TcpGraphiteSender> _logger;
        private TcpClient _client;

        public TcpGraphiteSender(ILoggerFactory loggerFactory, string host, int port)
            : base(host, port)
        {
            _logger = loggerFactory.CreateLogger<TcpGraphiteSender>();
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            (_client as IDisposable)?.Dispose();
            _client = null;
        }

        /// <inheritdoc />
        public override async Task Flush()
        {
            await base.Flush();
            try
            {
                await _client.GetStream().FlushAsync();
            }
            catch
            {
                (_client as IDisposable)?.Dispose();
                _client = null;
                throw;
            }
        }

        /// <inheritdoc />
        protected override async Task SendData(byte[] bytes)
        {
            try
            {
                await InitClient();
                if (_client == null) return;
                await _client.GetStream().WriteAsync(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                (_client as IDisposable)?.Dispose();
                _client = null;
                _logger.LogError((int)LoggingEvents.TcpSendError, ex, "Data was not sent because of an exception");
            }
        }

        private async Task InitClient()
        {
            if (_client?.Connected != true)
            {
                (_client as IDisposable)?.Dispose();
                _client = new TcpClient();
                try
                {
                    await _client.ConnectAsync(_host, _port);
                    _logger.LogDebug($"TCP client for graphite initialized for {_host}:{_port}");
                }
                catch (Exception ex)
                {
                    _client = null;
                    _logger.LogError((int)LoggingEvents.TcpClientCreateError, ex, "Unable to create TcpClient");
                }
                
            }
        }
    }
}