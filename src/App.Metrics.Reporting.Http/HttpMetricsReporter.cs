// <copyright file="HttpMetricsReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Json;
using App.Metrics.Reporting.Http.Client;

namespace App.Metrics.Reporting.Http
{
    public class HttpMetricsReporter : IReportMetrics
    {
        private readonly IMetricsOutputFormatter _defaultMetricsOutputFormatter = new MetricsJsonOutputFormatter();
        private readonly DefaultHttpClient _httpClient;

        public HttpMetricsReporter(MetricsReportingHttpOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.FlushInterval < TimeSpan.Zero)
            {
                throw new InvalidOperationException($"{nameof(MetricsReportingHttpOptions.FlushInterval)} must not be less than zero");
            }

            Formatter = options.MetricsOutputFormatter ?? _defaultMetricsOutputFormatter;

            FlushInterval = options.FlushInterval > TimeSpan.Zero
                ? options.FlushInterval
                : AppMetricsConstants.Reporting.DefaultFlushInterval;

            Filter = options.Filter;

            _httpClient = new DefaultHttpClient(options);
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
            var formatter = Formatter ?? _defaultMetricsOutputFormatter;

            using (var stream = new MemoryStream())
            {
                await formatter.WriteAsync(stream, metricsData, cancellationToken);

                var output = Encoding.UTF8.GetString(stream.ToArray());

                var result = await _httpClient.WriteAsync(output, cancellationToken);

                return result.Success;
            }
        }
    }
}