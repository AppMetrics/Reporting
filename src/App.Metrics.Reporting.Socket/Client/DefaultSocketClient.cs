// <copyright file="DefaultSocketClient.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters;
using App.Metrics.Logging;

namespace App.Metrics.Reporting.Socket.Client
{
    public class DefaultSocketClient
    {
        private static readonly ILog Logger = LogProvider.For<DefaultSocketClient>();

        private static long _backOffTicks;
        private static long _failureAttempts;
        private readonly SocketClient _socketClient;
        private readonly SocketSettings _socketSettings;
        private readonly SocketPolicy _socketPolicy;

        public string Endpoint
        {
            get
            {
                return _socketSettings.ToString();
            }
        }

        public DefaultSocketClient(MetricsReportingSocketOptions options)
        {
            _socketClient = CreateSocketClient(options.SocketSettings);
            _socketSettings = options.SocketSettings;
            _socketPolicy = options.SocketPolicy;
            _failureAttempts = 0;
        }

        public async Task<SocketWriteResult> WriteAsync(
            string payload,
            MetricsMediaTypeValue mediaType,
            CancellationToken cancellationToken = default)
        {
            if (NeedToBackoff())
            {
                return new SocketWriteResult(false, $"Too many failures in writing to {_socketSettings}, Circuit Opened");
            }

            try
            {
                byte[] output = System.Text.Encoding.UTF8.GetBytes(payload);

                var response = await _socketClient.SendAsync(
                    output, output.Length, _socketSettings.Address, _socketSettings.Port);

                if (response != output.Length)
                {
                    Interlocked.Increment(ref _failureAttempts);

                    var errorMessage =
                        $"Failed to write to {Endpoint}. Bytes: {output.Length}. Sended: {response}";
                    Logger.Error(errorMessage);

                    return new SocketWriteResult(false, errorMessage);
                }

                Logger.Trace($"Successful write to {Endpoint}");

                return new SocketWriteResult(true);
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _failureAttempts);
                Logger.Error(ex, $"Failed to write to {Endpoint}");
                return new SocketWriteResult(false, ex.ToString());
            }
        }

        private static SocketClient CreateSocketClient(
            SocketSettings socketSettings)
        {
            SocketSettings.Validate(socketSettings.Address, socketSettings.Port);
            var client = new UdpClient();
            return client;
        }

        private bool NeedToBackoff()
        {
            if (Interlocked.Read(ref _failureAttempts) < _socketPolicy.FailuresBeforeBackoff)
            {
                return false;
            }

            Logger.Error($"{Endpoint} write backoff for {_socketPolicy.BackoffPeriod.Seconds} secs");

            if (Interlocked.Read(ref _backOffTicks) == 0)
            {
                Interlocked.Exchange(ref _backOffTicks, DateTime.UtcNow.Add(_socketPolicy.BackoffPeriod).Ticks);
            }

            if (DateTime.UtcNow.Ticks <= Interlocked.Read(ref _backOffTicks))
            {
                return true;
            }

            Interlocked.Exchange(ref _failureAttempts, 0);
            Interlocked.Exchange(ref _backOffTicks, 0);

            return false;
        }
    }
}