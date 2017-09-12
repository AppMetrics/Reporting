// <copyright file="HttpMetricsReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using App.Metrics.Reporting.Http.Client;

namespace App.Metrics.Reporting.Http
{
    public class HttpMetricsReporterProvider : IMetricsReporterProvider
    {
        private readonly MetricsReportingHttpOptions _httpReportingOptions;
        private readonly DefaultHttpClient _httpClient;

        public HttpMetricsReporterProvider(
            MetricsReportingHttpOptions httpReportingOptions)
        {
            _httpReportingOptions = httpReportingOptions;
            _httpClient = new DefaultHttpClient(httpReportingOptions);
            Filter = _httpReportingOptions.Filter;
            ReportInterval = httpReportingOptions.ReportInterval;
        }

        /// <inheritdoc />
        public IFilterMetrics Filter { get; }

        /// <inheritdoc />
        public TimeSpan ReportInterval { get; }

        /// <inheritdoc />
        public async Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default)
        {
            using (var stream = new MemoryStream())
            {
                await _httpReportingOptions.MetricsOutputFormatter.WriteAsync(stream, metricsData, cancellationToken);

                var output = Encoding.UTF8.GetString(stream.ToArray());

                var result = await _httpClient.WriteAsync(output, cancellationToken);

                return result.Success;
            }
        }
    }
}