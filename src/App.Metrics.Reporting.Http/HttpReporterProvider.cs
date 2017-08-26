// <copyright file="HttpReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using App.Metrics.Reporting.Http.Client;
using Microsoft.Extensions.Options;

namespace App.Metrics.Reporting.Http
{
    public class HttpReporterProvider : IReporterProvider
    {
        private readonly IOptions<MetricsReportingHttpOptions> _httpOptionsAccessor;
        private readonly DefaultHttpClient _httpClient;

        public HttpReporterProvider(
            IOptions<MetricsReportingOptions> optionsAccessor,
            IOptions<MetricsReportingHttpOptions> httpOptionsAccessor)
        {
            _httpOptionsAccessor = httpOptionsAccessor;
            _httpClient = new DefaultHttpClient(httpOptionsAccessor);
            Filter = _httpOptionsAccessor.Value.Filter ?? optionsAccessor.Value.Filter;
            ReportInterval = httpOptionsAccessor.Value.ReportInterval;
        }

        /// <inheritdoc />
        public IFilterMetrics Filter { get; }

        /// <inheritdoc />
        public TimeSpan ReportInterval { get; }

        /// <inheritdoc />
        public async Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var stream = new MemoryStream())
            {
                await _httpOptionsAccessor.Value.MetricsOutputFormatter.WriteAsync(stream, metricsData, cancellationToken);

                var output = Encoding.UTF8.GetString(stream.ToArray());

                var result = await _httpClient.WriteAsync(output, cancellationToken);

                return result.Success;
            }
        }
    }
}