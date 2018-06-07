﻿// <copyright file="UdpMetricsReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Json;
using App.Metrics.Logging;
using App.Metrics.Reporting.Udp.Client;

namespace App.Metrics.Reporting.Udp
{
    public class UdpMetricsReporter : IReportMetrics
    {
        private static readonly ILog Logger = LogProvider.For<UdpMetricsReporter>();
        private readonly DefaultUdpClient _udpClient;

        public UdpMetricsReporter(MetricsReportingUdpOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.MetricsOutputFormatter == null)
            {
                throw new ArgumentNullException(nameof(options.MetricsOutputFormatter));
            }

            if (options.FlushInterval < TimeSpan.Zero)
            {
                throw new InvalidOperationException($"{nameof(MetricsReportingUdpOptions.FlushInterval)} must not be less than zero");
            }

            Formatter = options.MetricsOutputFormatter;

            FlushInterval = options.FlushInterval > TimeSpan.Zero
                ? options.FlushInterval
                : AppMetricsConstants.Reporting.DefaultFlushInterval;

            Filter = options.Filter;

            _udpClient = new DefaultUdpClient(options);

            Logger.Info($"Using Metrics Reporter {this}. Url: {_udpClient.Endpoint} FlushInterval: {FlushInterval}");
        }

        /// <inheritdoc />
        public IFilterMetrics Filter { get; set; }

        /// <inheritdoc />
        public TimeSpan FlushInterval { get; set; }

        /// <inheritdoc />
        public IMetricsOutputFormatter Formatter { get; set; }

        /// <inheritdoc />
        public async Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default)
        {
            Logger.Trace("Flushing metrics snapshot");

            using (var stream = new MemoryStream())
            {
                await Formatter.WriteAsync(stream, metricsData, cancellationToken);

                var output = Encoding.UTF8.GetString(stream.ToArray());

                var result = await _udpClient.WriteAsync(output, Formatter.MediaType, cancellationToken);

                if (result.Success)
                {
                    Logger.Trace("Flushed metrics snapshot");
                    return true;
                }

                Logger.Error(result.ErrorMessage);

                return false;
            }
        }
    }
}