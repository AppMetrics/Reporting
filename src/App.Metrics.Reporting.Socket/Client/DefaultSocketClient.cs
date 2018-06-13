// <copyright file="DefaultSocketClient.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net.Sockets;
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
        private readonly SocketPolicy _socketPolicy;

        public string Endpoint
        {
            get
            {
                return _socketClient.Endpoint;
            }
        }

        public DefaultSocketClient(MetricsReportingSocketOptions options)
        {
            _socketClient = CreateSocketClient(options.SocketSettings);
            _socketPolicy = options.SocketPolicy;
            _failureAttempts = 0;
        }

        public async Task<SocketWriteResult> WriteAsync(
            string payload,
            CancellationToken cancellationToken = default)
        {
            if (NeedToBackoff())
            {
                return new SocketWriteResult(false, $"Too many failures in writing to {Endpoint}, Circuit Opened");
            }

            if (!_socketClient.IsConnected())
            {
                Logger.Debug($"Try to connect to {Endpoint}");
                await _socketClient.ReConnect();
            }

            try
            {
                var response = await _socketClient.WriteAsync(payload, cancellationToken);

                if (!response.Success)
                {
                    Interlocked.Increment(ref _failureAttempts);

                    var errorMessage =
                        $"Failed to write {payload.Length} bytes to {Endpoint}";
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
            var client = new SocketClient(socketSettings);
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
