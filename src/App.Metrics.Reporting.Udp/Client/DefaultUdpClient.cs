// <copyright file="DefaultUdpClient.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters;
using App.Metrics.Logging;

namespace App.Metrics.Reporting.Udp.Client
{
    public class DefaultUdpClient
    {
        private static readonly ILog Logger = LogProvider.For<DefaultUdpClient>();

        private static long _backOffTicks;
        private static long _failureAttempts;
        private readonly UdpClient _udpClient;
        private readonly UdpSettings _udpSettings;
        private readonly UdpPolicy _udpPolicy;

        public string Endpoint
        {
            get
            {
                return _udpSettings.ToString();
            }
        }

        public DefaultUdpClient(MetricsReportingUdpOptions options)
        {
            _udpClient = CreateUdpClient(options.UdpSettings);
            _udpSettings = options.UdpSettings;
            _udpPolicy = options.UdpPolicy;
            _failureAttempts = 0;
        }

        public async Task<UdpWriteResult> WriteAsync(
            string payload,
            MetricsMediaTypeValue mediaType,
            CancellationToken cancellationToken = default)
        {
            if (NeedToBackoff())
            {
                return new UdpWriteResult(false, $"Too many failures in writing to {_udpSettings}, Circuit Opened");
            }

            try
            {
                byte[] output = System.Text.Encoding.UTF8.GetBytes(payload);

                var response = await _udpClient.SendAsync(
                    output, output.Length, _udpSettings.Address, _udpSettings.Port);

                if (response != output.Length)
                {
                    Interlocked.Increment(ref _failureAttempts);

                    var errorMessage =
                        $"Failed to write to {Endpoint}. Bytes: {output.Length}. Sended: {response}";
                    Logger.Error(errorMessage);

                    return new UdpWriteResult(false, errorMessage);
                }

                Logger.Trace($"Successful write to {Endpoint}");

                return new UdpWriteResult(true);
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _failureAttempts);
                Logger.Error(ex, $"Failed to write to {Endpoint}");
                return new UdpWriteResult(false, ex.ToString());
            }
        }

        private static UdpClient CreateUdpClient(
            UdpSettings udpSettings)
        {
            UdpSettings.Validate(udpSettings.Address, udpSettings.Port);
            var client = new UdpClient();
            return client;
        }

        private bool NeedToBackoff()
        {
            if (Interlocked.Read(ref _failureAttempts) < _udpPolicy.FailuresBeforeBackoff)
            {
                return false;
            }

            Logger.Error($"{Endpoint} write backoff for {_udpPolicy.BackoffPeriod.Seconds} secs");

            if (Interlocked.Read(ref _backOffTicks) == 0)
            {
                Interlocked.Exchange(ref _backOffTicks, DateTime.UtcNow.Add(_udpPolicy.BackoffPeriod).Ticks);
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