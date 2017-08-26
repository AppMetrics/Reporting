// <copyright file="DefaultHttpClient.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.Reporting.Http.Client
{
    public class DefaultHttpClient
    {
        private static readonly ILog Logger = LogProvider.For<DefaultHttpClient>();

        private static long _backOffTicks;
        private static long _failureAttempts;
        private static long _failuresBeforeBackoff;
        private static TimeSpan _backOffPeriod;
        private readonly HttpClient _httpClient;
        private readonly HttpSettings _httpSettings;

        public DefaultHttpClient(IOptions<MetricsReportingHttpOptions> httpOptionsAccessor)
        {
            _httpClient = CreateHttpClient(httpOptionsAccessor.Value.HttpSettings, httpOptionsAccessor.Value.HttpPolicy, httpOptionsAccessor.Value.InnerHttpMessageHandler);
            _httpSettings = httpOptionsAccessor.Value.HttpSettings;
            _backOffPeriod = httpOptionsAccessor.Value.HttpPolicy.BackoffPeriod;
            _failuresBeforeBackoff = httpOptionsAccessor.Value.HttpPolicy.FailuresBeforeBackoff;
            _failureAttempts = 0;
        }

        public async Task<HttpWriteResult> WriteAsync(
            string payload,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (NeedToBackoff())
            {
                return new HttpWriteResult(false, $"Too many failures in writing to {_httpSettings.RequestUri}, Circuit Opened");
            }

            var content = new StringContent(payload, Encoding.UTF8);

            try
            {
                var response = await _httpClient.PostAsync(_httpSettings.RequestUri, content, cancellationToken).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    Interlocked.Increment(ref _failureAttempts);

                    var errorMessage =
                        $"Failed to write to {_httpSettings.RequestUri} - StatusCode: {response.StatusCode} Reason: {response.ReasonPhrase}";
                    Logger.Error(errorMessage);

                    return new HttpWriteResult(false, errorMessage);
                }

                Logger.Trace($"Successful write to {_httpSettings.RequestUri}");

                return new HttpWriteResult(true);
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _failureAttempts);
                Logger.Error(ex, $"Failed to write to {_httpSettings.RequestUri}");
                return new HttpWriteResult(false, ex.ToString());
            }
        }

        private static HttpClient CreateHttpClient(
            HttpSettings httpSettings,
            HttpPolicy httpPolicy,
            HttpMessageHandler httpMessageHandler = null)
        {
            var client = httpMessageHandler == null
                ? new HttpClient()
                : new HttpClient(httpMessageHandler);

            client.BaseAddress = httpSettings.RequestUri;
            client.Timeout = httpPolicy.Timeout;

            if (string.IsNullOrWhiteSpace(httpSettings.UserName) || string.IsNullOrWhiteSpace(httpSettings.Password))
            {
                return client;
            }

            var byteArray = Encoding.ASCII.GetBytes($"{httpSettings.UserName}:{httpSettings.Password}");
            client.BaseAddress = httpSettings.RequestUri;
            client.Timeout = httpPolicy.Timeout;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            return client;
        }

        private bool NeedToBackoff()
        {
            if (Interlocked.Read(ref _failureAttempts) < _failuresBeforeBackoff)
            {
                return false;
            }

            Logger.Error($"{_httpSettings.RequestUri} write backoff for {_backOffPeriod.Seconds} secs");

            if (Interlocked.Read(ref _backOffTicks) == 0)
            {
                Interlocked.Exchange(ref _backOffTicks, DateTime.UtcNow.Add(_backOffPeriod).Ticks);
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